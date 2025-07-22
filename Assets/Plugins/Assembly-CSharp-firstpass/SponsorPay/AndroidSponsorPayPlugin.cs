using System;
using UnityEngine;

namespace SponsorPay
{
	public class AndroidSponsorPayPlugin : ISponsorPayPlugin
	{
		private AndroidJavaObject spAdvertiserCallback;

		private AndroidJavaObject spOfferWall;

		private AndroidJavaObject spInterstitial;

		private AndroidJavaObject spOfferBanner;

		private AndroidJavaObject spCurrency;

		private AndroidJavaObject spUnlock;

		private string CallbackGameObjectName { get; set; }

		private AndroidJavaObject SPAdvertiserCallback
		{
			get
			{
				if (spAdvertiserCallback == null)
				{
					spAdvertiserCallback = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityAdvertiserCallbackWrapper");
				}
				return spAdvertiserCallback;
			}
		}

		private AndroidJavaObject SPOfferWall
		{
			get
			{
				if (spOfferWall == null)
				{
					spOfferWall = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityOfferWallWrapper");
				}
				return spOfferWall;
			}
		}

		private AndroidJavaObject SPInterstitial
		{
			get
			{
				if (spInterstitial == null)
				{
					spInterstitial = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityInterstitialWrapper");
				}
				return spInterstitial;
			}
		}

		private AndroidJavaObject SPCurrency
		{
			get
			{
				if (spCurrency == null)
				{
					spCurrency = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityCurrencyWrapper");
					spCurrency.Call("setListenerObjectName", CallbackGameObjectName);
				}
				return spCurrency;
			}
		}

		private AndroidJavaObject SPOfferBanner
		{
			get
			{
				if (spOfferBanner == null)
				{
					spOfferBanner = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityOfferBannerWrapper");
					spOfferBanner.Call("setListenerObjectName", CallbackGameObjectName);
				}
				return spOfferBanner;
			}
		}

		private AndroidJavaObject SPUnlock
		{
			get
			{
				if (spUnlock == null)
				{
					spUnlock = new AndroidJavaObject("com.sponsorpay.sdk.android.unity.SPUnityUnlockWrapper");
					spUnlock.Call("setListenerObjectName", CallbackGameObjectName);
				}
				return spUnlock;
			}
		}

		private string BannerPositionTop
		{
			get
			{
				return SPOfferBanner.GetStatic<string>("BANNER_POSITION_TOP");
			}
		}

		private string BannerPositionBottom
		{
			get
			{
				return SPOfferBanner.GetStatic<string>("BANNER_POSITION_BOTTOM");
			}
		}

		public AndroidSponsorPayPlugin(string gameObjectName)
		{
			AndroidJNI.AttachCurrentThread();
			CallbackGameObjectName = gameObjectName;
			SetPluginVersion();
		}

		public string RequestResultSuccessMessage()
		{
			return SPCurrency.GetStatic<string>("REQUEST_RESULT_SUCCESS");
		}

		public string RequestResultOfferBannerAvailableMessage()
		{
			return SPOfferBanner.GetStatic<string>("OFFER_BANNER_AVAILABLE");
		}

		public string RequestResultOfferBannerNotAvailableMessage()
		{
			return SPOfferBanner.GetStatic<string>("OFFER_BANNER_NOT_AVAILABLE");
		}

		public void SendAdvertiserCallbackNow(string appId)
		{
			SPAdvertiserCallback.Call("sendAdvertiserCallbackNow", appId);
		}

		public void SendAdvertiserCallbackWithDelay(string appId, int delayMin)
		{
			SPAdvertiserCallback.Call("sendAdvertiserCallbackWithDelay", appId, delayMin);
		}

		public void LaunchOfferWall(string appId, string userId)
		{
			SPOfferWall.Call("launchOfferWall", appId, userId);
		}

		public void LaunchInterstitial(string appId, string userId)
		{
			SPInterstitial.Call("launchSPInterstitial", appId, userId);
		}

		public void RequestNewCoins(string appId, string userId, string securityToken)
		{
			SPCurrency.Call("requestNewCoins", appId, userId, securityToken);
		}

		public void LaunchSPUnlockOfferWall(string appId, string userId, string unlockItemId, string unlockItemName)
		{
			string text = SPUnlock.Call<string>("launchUnlockOfferWall", new object[4] { appId, userId, unlockItemId, unlockItemName });
			if (!string.IsNullOrEmpty(text))
			{
				throw new ArgumentException(text);
			}
		}

		public void RequestSPUnlockItemsStatus(string appId, string userId, string securityToken)
		{
			SPUnlock.Call("requestItemsStatus", appId, userId, securityToken);
		}

		public void RequestOfferBanner(string appId, string userId, string currencyName)
		{
			SPOfferBanner.Call("requestOfferBanner", appId, userId, currencyName);
		}

		public double LastReceivedDeltaOfCoins()
		{
			return SPCurrency.Call<double>("getLastReceivedDeltaOfCoins", new object[0]);
		}

		public string LastReceivedTransactionId()
		{
			return spCurrency.Call<string>("getLastReceivedTransactionId", new object[0]);
		}

		public RequestError LastReceivedErrorForCurrencyStatusRequest()
		{
			string type = SPCurrency.Call<string>("getLastErrorType", new object[0]);
			string code = SPCurrency.Call<string>("getLastErrorCode", new object[0]);
			string message = SPCurrency.Call<string>("getLastErrorMessage", new object[0]);
			return new RequestError(type, code, message);
		}

		public RequestError LastReceivedErrorForUnlockStatusRequest()
		{
			string type = SPUnlock.Call<string>("getLastErrorType", new object[0]);
			string code = SPUnlock.Call<string>("getLastErrorCode", new object[0]);
			string message = SPUnlock.Call<string>("getLastErrorMessage", new object[0]);
			return new RequestError(type, code, message);
		}

		public UnlockItem[] LastReceivedUnlockItems()
		{
			string[] array = SPUnlock.Call<string[]>("getUnlockItemIDs", new object[0]);
			int num = array.Length;
			UnlockItem[] array2 = new UnlockItem[num];
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				string name = SPUnlock.Call<string>("getNameForItem", new object[1] { text });
				bool isUnlocked = SPUnlock.Call<bool>("getUnlockStatusForItem", new object[1] { text });
				long unlockTimestamp = SPUnlock.Call<long>("getUnlockTimestampForItem", new object[1] { text });
				UnlockItem unlockItem = new UnlockItem(text, name, isUnlocked, unlockTimestamp);
				array2[i] = unlockItem;
			}
			return array2;
		}

		public void ShowLastReceivedOfferBanner(SPBannerPosition position)
		{
			string text = BannerPositionBottom;
			if (position == SPBannerPosition.TOP)
			{
				text = BannerPositionTop;
			}
			SPOfferBanner.Call("showLastReceivedOfferBanner", text);
		}

		public void RemoveOfferBannerFromScreen()
		{
			SPOfferBanner.Call("removeOfferBanner");
		}

		private void SetPluginVersion()
		{
			IntPtr intPtr = AndroidJNI.FindClass("com.sponsorpay.sdk.android.unity.SPUnityPlugin");
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(intPtr, "INSTANCE", "Lcom/sponsorpay/sdk/android/unity/SPUnityPlugin;");
			IntPtr staticObjectField = AndroidJNI.GetStaticObjectField(intPtr, staticFieldID);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(intPtr, "setPluginVersion", "(Ljava/lang/String;)V", false);
			jvalue[] array = new jvalue[1] { default(jvalue) };
			array[0].l = AndroidJNI.NewStringUTF("3.0");
			AndroidJNI.CallObjectMethod(staticObjectField, methodID, array);
		}
	}
}
