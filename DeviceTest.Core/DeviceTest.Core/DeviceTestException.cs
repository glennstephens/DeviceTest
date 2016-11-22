using System;

#if DEVICETEST

namespace DeviceTest.Core
{
	public class DeviceTestException : Exception
	{
		public DeviceTestException(string message) : base(message)
		{
		}
	}
}

#endif