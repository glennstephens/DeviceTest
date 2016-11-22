using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace DeviceTest.TestCloudRun
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;

		Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
		}

		[Test]
		public void RunDeviceTests()
		{
			app.Screenshot("Before running the tests");

			// Get the number of tests that we have on display
			app.Screenshot("Running security checks");


			app.Screenshot("Running tests");


			app.Screenshot("Tests Finished Running");
		}
	}
}
