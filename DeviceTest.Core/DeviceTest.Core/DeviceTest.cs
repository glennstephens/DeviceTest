using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if DEVICETEST

namespace DeviceTest.Core
{
	/// <summary>
	/// Base class for all tests. Each public method in a DeviceTest will be tested. 
	/// </summary>
	public abstract class DeviceTest
	{
		public abstract Task Execute();

		public virtual async Task Setup() { 
			
		}

		public virtual async Task TearDown() {

		}

		protected void Assert(bool condition, string errorMessage = "")
		{
			if (!condition)
				throw new DeviceTestException(errorMessage);
		}
	}

	public class RegisteredTestEntry
	{
		public Type TestType;
		public List<Type> DevicePermissionSetup;
	}

	public class RegisteredTests : List<RegisteredTestEntry>
	{
		static Lazy<RegisteredTests> _instance = new Lazy<RegisteredTests>(() => new RegisteredTests());

		public static RegisteredTests Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		private RegisteredTests()
		{
		}

		public RegisteredTests AddTest(Type testType, Type SetupType = null)
		{
			RegisteredTestEntry entry = new RegisteredTestEntry() {
				TestType = testType,
				DevicePermissionSetup = new List<Type>(new [] { SetupType })
			};
			this.Add(entry);

			return this;
		}

		public RegisteredTests AddTest<T>() where T : DeviceTest
		{
			return AddTest(typeof(T));
		}

		public RegisteredTests AddTest<T, P>() where T : DeviceTest where P : DevicePermissionSetup
		{
			return AddTest(typeof(T), typeof(P));
		}

		public DeviceTest GetTestAtIndex(int index)
		{
			var item = this[index];

			return Activator.CreateInstance(item.TestType) as DeviceTest;
		}

		public RegisteredTestEntry GetTestEntryAtIndex(int index)
		{
			return this[index];
		}
	}

	public abstract class DevicePermissionSetup
	{
		public abstract Task<bool> IsSetup();
		public abstract Task<bool> Setup();
	}
}

#endif