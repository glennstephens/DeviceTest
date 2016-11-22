using System;
using System.Threading.Tasks;

#if DEVICETEST

using DeviceTest.Core;
using Foundation;
using HealthKit;

namespace Fit
{
	public class HealthKitDeviceTest : DeviceTest.Core.DeviceTest
	{
		public static Task<double> ReadCurrentHeightInMetres(HKHealthStore store)
		{
			var tcs = new TaskCompletionSource<double>();

			var heightType = HKQuantityTypeIdentifierKey.Height;
			var heightUnit = HKUnit.Meter;
			var sort = new NSSortDescriptor(HKSample.SortIdentifierStartDate, false);

			var sampleType = HKObjectType.GetQuantityType(HKQuantityTypeIdentifierKey.Height);

			var query = new HKSampleQuery(sampleType, null, 1, new NSSortDescriptor[] { sort },
													(q, data, error) =>
													{
														if (error == null)
														{
															var amount = (data[0] as HKQuantitySample).Quantity.GetDoubleValue(heightUnit);
															tcs.TrySetResult(amount);
														}
														else
														{
															tcs.TrySetException(new NSErrorException(error));
														}
													});

			store.ExecuteQuery(query);

			return tcs.Task;
		}

		HKHealthStore store;

		public override async Task Execute()
		{
			store = new HKHealthStore();
			var height = await ReadCurrentHeightInMetres(store);
		}
	}

	public class HealthKitBasicDetails : DevicePermissionSetup
	{
		NSSet DataTypesToWrite
		{
			get
			{
				return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
					HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.Height)
				});
			}
		}

		NSSet DataTypesToRead
		{
			get
			{
				return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
					HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.Height)
				});
			}
		}

		HKHealthStore HealthStore = new HKHealthStore();

		public override async Task<bool> Setup()
		{
			if (HKHealthStore.IsHealthDataAvailable)
			{
				var success = await HealthStore.RequestAuthorizationToShareAsync(DataTypesToWrite, DataTypesToRead);

				return await Task.FromResult(success.Item1);
			}
			else
				return false;

		}

		public override async Task<bool> IsSetup()
		{
			// Lets try and read the date of birth
			return HealthStore.GetAuthorizationStatus(HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.Height)) == HKAuthorizationStatus.SharingAuthorized;
		}
	}
}

#endif