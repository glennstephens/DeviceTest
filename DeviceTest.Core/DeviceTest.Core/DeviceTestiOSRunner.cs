using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if DEVICETEST

#if __IOS__
using UIKit;

namespace DeviceTest.Core.TestRunner
{
	public class DeviceTestiOSRunner : UITableViewController
	{
		public DeviceTestiOSRunner() : base()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var testCount = RegisteredTests.Instance.Count;

			NavigationItem.Prompt = "Test Runner";
			NavigationItem.Title = "Click Run All to Start";

			NavigationItem.RightBarButtonItem = new UIBarButtonItem("Run All", UIBarButtonItemStyle.Plain, HandleEventHandler);
		}

		async void HandleEventHandler(object sender, EventArgs e)
		{
			// Start the tests
			try
			{
				NavigationItem.Title = "Running Tests...";

				await RunAllTheTests();

				TableView.ReloadData();
			}
			catch (Exception details)
			{
				DisplayTestRunException(details);
			}
		}

		void DisplayTestRunException(Exception details)
		{
			// Update the UI with the details
			NavigationItem.Title = "Exception";
		}

		async Task RunAllTheTests()
		{
			TestResults.Clear();
			TableView.ReloadData();

			if (RegisteredTests.Instance.Count == 0)
				NavigationItem.Title = "No Tests to Run";

			for (int counter = 0; counter < RegisteredTests.Instance.Count; counter++)
			{
				var number = counter + 1;
				NavigationItem.Title = $"Running Test {number} of {RegisteredTests.Instance.Count}";

				DeviceTest test = null;
				try
				{
					test = RegisteredTests.Instance.GetTestAtIndex(counter);
					await test.Execute();

					LogTestResult(test, counter, true);
				}
				catch (Exception e)
				{
					LogFailedException(test, counter, e);
				}
			}
		}

		public List<TestResultDetails> TestResults = new List<TestResultDetails>();

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return TestResults.Count;
		}

		const string detailsCell = "testResultsDetailsCell";

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(detailsCell);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Subtitle, detailsCell);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			}

			var test = TestResults[indexPath.Row];

			cell.TextLabel.Text = test.test.GetType().Name;
			cell.DetailTextLabel.Text = test.Result ? "Success" : $"Failed {test.Exception.Message}";
			// cell.BackgroundColor = test.Result ? UIColor.White : UIColor.FromRGBA(255, 0, 0, 45);
			cell.AccessoryView = new UIView(new CoreGraphics.CGRect(0, 0, 30, 30))
			{
				BackgroundColor = test.Result ? UIColor.Green : UIColor.Red
			};

			return cell;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var test = TestResults[indexPath.Row];

			var vc = new DeviceTestRunDetails(test);
			NavigationController.PushViewController(vc, true);
		}

		void LogFailedException(DeviceTest test, int counter, Exception e)
		{
			var item = new TestResultDetails()
			{
				test = test,
				Exception = e,
				Index = counter,
				Result = false
			};

			TestResults.Add(item);
		}

		void LogTestResult(DeviceTest test, int counter, bool result)
		{
			var item = new TestResultDetails()
			{
				test = test,
				Exception = null,
				Index = counter,
				Result = result
			};

			TestResults.Add(item);
		}
	}

	public class DeviceTestRunDetails : UIViewController
	{
		UITextView display;
		readonly TestResultDetails item;

		public DeviceTestRunDetails(TestResultDetails item)
		{
			this.item = item;
		}

		void Log(string s)
		{
			display.Text += s + Environment.NewLine;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = item.test.GetType().Name;

			display = new UITextView(this.View.Frame);
			Add(display);

			Log("Test: " + Title);
			Log(item.CreatedDateTime.ToString("G"));
			Log("Result: " + item.Result);
			if (item.Result)
				return;

			if (item.Exception == null)
				return;

			Log("");
			Log("Exception Details");
			Log(item.Exception.Message);
			Log(item.Exception.Source);
			Log(item.Exception.StackTrace);
		}

		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();

			display.Frame = this.View.Frame;
		}
	}
}

#endif
#endif