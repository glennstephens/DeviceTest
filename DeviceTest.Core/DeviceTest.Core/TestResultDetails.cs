using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if DEVICETEST

namespace DeviceTest.Core
{
	public class TestResultDetails
	{
		public DeviceTest test;
		public int Index;
		public DateTime CreatedDateTime = new DateTime();
		public bool Result;
		public Exception Exception;
	}
}

#endif