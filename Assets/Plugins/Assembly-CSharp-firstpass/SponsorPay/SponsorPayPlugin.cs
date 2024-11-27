using System;
using System.Collections;
using UnityEngine;

namespace SponsorPay
{
	public class SponsorPayPlugin
	{
		public const string PluginVersion = "3.0";

		private ISponsorPayPlugin plugin;

		public event DeltaOfCoinsResponseReceivedHandler OnDeltaOfCoinsReceived;

		public event ErrorHandler OnDeltaOfCoinsRequestFailed;

		public event UnlockItemsStatusResponseReceivedHandler OnUnlockItemsStatusReceived;

		public event ErrorHandler OnUnlockItemsStatusRequestFailed;

		public event OfferBannerResponseReceivedHandler OnOfferBannerResponseReceived;

		public event ErrorHandler OnOfferBannerRequestFailed;

		public SponsorPayPlugin(string gameObjectName)
		{
			Debug.Log("Android initialization");
			plugin = new AndroidSponsorPayPlugin(gameObjectName);
		}

		public void SendAdvertiserCallbackNow(string appId)
		{
			CheckRequiredParameters(new string[1] { appId }, new string[1] { "appId" });
			plugin.SendAdvertiserCallbackNow(appId);
		}

		public void SendAdvertiserCallbackWithDelay(string appId, int delayMin)
		{
			CheckRequiredParameters(new string[1] { appId }, new string[1] { "appId" });
			try
			{
				plugin.SendAdvertiserCallbackWithDelay(appId, delayMin);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void LaunchOfferWall(string appId, string userId)
		{
			CheckRequiredParameters(new string[2] { appId, userId }, new string[2] { "appId", "userId" });
			plugin.LaunchOfferWall(appId, userId);
		}

		public void LaunchInterstitial(string appId, string userId)
		{
			CheckRequiredParameters(new string[2] { appId, userId }, new string[2] { "appId", "userId" });
			plugin.LaunchInterstitial(appId, userId);
		}

		public void RequestNewCoins(string appId, string userId, string securityToken)
		{
			CheckRequiredParameters(new string[3] { appId, userId, securityToken }, new string[3] { "appId", "userId", "securityToken" });
			plugin.RequestNewCoins(appId, userId, securityToken);
		}

		public void LaunchSPUnlockOfferWall(string appId, string userId, string unlockItemId, string unlockItemName)
		{
			CheckRequiredParameters(new string[3] { appId, userId, unlockItemId }, new string[3] { "appId", "userId", "unlockItemId" });
			try
			{
				plugin.LaunchSPUnlockOfferWall(appId, userId, unlockItemId, unlockItemName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void RequestSPUnlockItemsStatus(string appId, string userId, string securityToken)
		{
			CheckRequiredParameters(new string[3] { appId, userId, securityToken }, new string[3] { "appId", "userId", "securityToken" });
			try
			{
				plugin.RequestSPUnlockItemsStatus(appId, userId, securityToken);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void RequestOfferBanner(string appId, string userId, string currencyName)
		{
			CheckRequiredParameters(new string[2] { appId, userId }, new string[2] { "appId", "userId" });
			try
			{
				plugin.RequestOfferBanner(appId, userId, currencyName);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void ShowLastReceivedOfferBanner(SPBannerPosition position)
		{
			try
			{
				plugin.ShowLastReceivedOfferBanner(position);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void RemoveOfferBannerFromScreen()
		{
			try
			{
				plugin.RemoveOfferBannerFromScreen();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool DoesMessageSignalSuccess(string message)
		{
			return message.Equals(plugin.RequestResultSuccessMessage());
		}

		private bool DoesMessageSignalOfferBannerAvailable(string message)
		{
			try
			{
				return message.Equals(plugin.RequestResultOfferBannerAvailableMessage());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private bool DoesMessageSignalOfferBannerNotAvailable(string message)
		{
			try
			{
				return message.Equals(plugin.RequestResultOfferBannerNotAvailableMessage());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void HandleCurrencyDeltaOfCoinsMessageFromSDK(string message)
		{
			if (DoesMessageSignalSuccess(message))
			{
				this.OnDeltaOfCoinsReceived(plugin.LastReceivedDeltaOfCoins(), plugin.LastReceivedTransactionId());
			}
			else
			{
				this.OnDeltaOfCoinsRequestFailed(plugin.LastReceivedErrorForCurrencyStatusRequest());
			}
		}

		public void HandleUnlockItemsStatusMessageFromSDK(string message)
		{
			if (DoesMessageSignalSuccess(message))
			{
				this.OnUnlockItemsStatusReceived(plugin.LastReceivedUnlockItems());
			}
			else
			{
				this.OnUnlockItemsStatusRequestFailed(plugin.LastReceivedErrorForUnlockStatusRequest());
			}
		}

		public void HandleOfferBannerMessageFromSDK(string message)
		{
			if (DoesMessageSignalOfferBannerAvailable(message))
			{
				this.OnOfferBannerResponseReceived(true);
			}
			else if (DoesMessageSignalOfferBannerNotAvailable(message))
			{
				this.OnOfferBannerResponseReceived(false);
			}
			else
			{
				this.OnOfferBannerRequestFailed(new RequestError("--", "--", message));
			}
		}

		private void CheckRequiredParameters(string[] values, string[] names)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < values.Length; i++)
			{
				string value = values[i];
				if (string.IsNullOrEmpty(value))
				{
					arrayList.Add(names[i]);
				}
			}
			if (arrayList.Count > 0)
			{
				string paramName = string.Join(",", (string[])arrayList.ToArray(Type.GetType("System.String")));
				throw new ArgumentException("Parameter cannot be null", paramName);
			}
		}
	}
}
