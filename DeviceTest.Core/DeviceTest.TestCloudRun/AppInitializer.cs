using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace DeviceTest.TestCloudRun
{
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
					.PreferIdeSettings()
					.StartApp();
			}

			return ConfigureApp
				.iOS
				.PreferIdeSettings()
				.StartApp();
		}
	}
}
