using System;
using SponsorPay;
using UnityEngine;

public class SponsorPaySDKDemo : MonoBehaviour
{
	private string appId = "1246";

	private string userId = "test_user_id_1";

	private string securityToken = "12345678";

	private string delayForAdvertiserCallback = "0";

	private short delayValue;

	private string customCurrencyName = "TestCoins";

	private string unlockItemId = "TEST_ITEM_ID";

	private string unlockItemName = string.Empty;

	private string unlockItemStatusResults = "Results will appear here.";

	private SponsorPayPlugin sponsorPayPlugin;

	private bool showScrollView = true;

	private static readonly float ScrollBarWidth = 20f;

	private static readonly float HorizontalMargin = 8f;

	private static readonly float GuiRectWidth = 0.7f * ((float)Screen.width - 2f * HorizontalMargin - ScrollBarWidth);

	private static readonly float GuiLabelHeight = 25f;

	private static readonly float GuiTapTargetHeight = 45f;

	private static readonly float GuiRectSmallPadding = 0f;

	private static readonly float GuiRectBigPadding = 15f;

	private static readonly float HorizontalPadding = 5f;

	private static readonly float BannerHeight = 110f;

	private string coinsLabel = string.Empty;

	private string bannerStatusLabel = string.Empty;

	private Vector2 scrollPosition = Vector2.zero;

	private string unlockRequestStatusLabel = "Request Unlock Items status";

	private string errorMessage;

	private bool showError;

	private void Start()
	{
		MonoBehaviour.print("SponsorPaySDKDemo's Start invoked");
		sponsorPayPlugin = SponsorPayPluginMonoBehaviour.PluginInstance;
		sponsorPayPlugin.OnDeltaOfCoinsReceived += OnSPDeltaOfCoinsReceived;
		sponsorPayPlugin.OnDeltaOfCoinsRequestFailed += OnSPDeltaOfCoinsRequestFailed;
		sponsorPayPlugin.OnUnlockItemsStatusReceived += OnSPUnlockItemsStatusResponseReceived;
		sponsorPayPlugin.OnUnlockItemsStatusRequestFailed += OnSPUnlockItemsRequestError;
		sponsorPayPlugin.OnOfferBannerResponseReceived += OnSPOfferBannerResponseReceived;
		sponsorPayPlugin.OnOfferBannerRequestFailed += OnSPOfferBannerRequestError;
		showScrollView = Screen.height < 1310;
	}

	private void OnGUI()
	{
		drawTestUI();
	}

	private void sendAdvertiserCallbackNow()
	{
		sponsorPayPlugin.SendAdvertiserCallbackNow(appId);
	}

	private void sendAdvertiserCallbackWithDelay()
	{
		sponsorPayPlugin.SendAdvertiserCallbackWithDelay(appId, delayValue);
	}

	private void launchOfferWall()
	{
		sponsorPayPlugin.LaunchOfferWall(appId, userId);
	}

	private void launchInterstitial()
	{
		sponsorPayPlugin.LaunchInterstitial(appId, userId);
	}

	private void requestNewCoins()
	{
		sponsorPayPlugin.RequestNewCoins(appId, userId, securityToken);
	}

	public void OnSPDeltaOfCoinsReceived(double deltaOfCoins, string lastTransactionId)
	{
		coinsLabel = "Delta of coins: " + deltaOfCoins + ". Transaction ID: " + lastTransactionId;
	}

	public void OnSPDeltaOfCoinsRequestFailed(RequestError error)
	{
		coinsLabel = string.Format("Delta of coins request failed.\nError Type: {0}\nError Code: {1}\nError Message: {2}", error.Type, error.Code, error.Message);
	}

	private void requestOfferBanner()
	{
		sponsorPayPlugin.RequestOfferBanner(appId, userId, customCurrencyName);
	}

	public void OnSPOfferBannerResponseReceived(bool isOfferBannerAvailable)
	{
		MonoBehaviour.print("OnSPOfferBannerResponseReceived: " + isOfferBannerAvailable);
		if (isOfferBannerAvailable)
		{
			bannerStatusLabel = "Banner available";
			sponsorPayPlugin.ShowLastReceivedOfferBanner(SPBannerPosition.BOTTOM);
		}
		else
		{
			bannerStatusLabel = "Banner not available";
			sponsorPayPlugin.RemoveOfferBannerFromScreen();
		}
	}

	public void OnSPOfferBannerRequestError(RequestError error)
	{
		bannerStatusLabel = "Banner request failed: " + error.ToString();
		sponsorPayPlugin.RemoveOfferBannerFromScreen();
	}

	private void launchUnlockOfferWall()
	{
		sponsorPayPlugin.LaunchSPUnlockOfferWall(appId, userId, unlockItemId, unlockItemName);
	}

	private void requestUnlockItemsStatus()
	{
		sponsorPayPlugin.RequestSPUnlockItemsStatus(appId, userId, securityToken);
	}

	public void OnSPUnlockItemsStatusResponseReceived(UnlockItem[] unlockItems)
	{
		unlockRequestStatusLabel = "Request Unlock Items status";
		unlockItemStatusResults = string.Format("Unlock Items status response received, Item count: {0}\n---------------\n", unlockItems.Length);
		foreach (UnlockItem unlockItem in unlockItems)
		{
			unlockItemStatusResults += string.Format("\nItem ID: [{0}] Name: [{1}] Unlocked: [{2}] Timestamp: [{3}]\n", unlockItem.Id, unlockItem.Name, unlockItem.IsUnlocked, unlockItem.UnlockTimestamp);
		}
	}

	public void OnSPUnlockItemsRequestError(RequestError error)
	{
		unlockRequestStatusLabel = "Request Unlock Items status";
		unlockItemStatusResults = string.Format("Unlock Items request error\n---------------\nError Type: {0}\nError Code: {1}\nError Message: {2}\n", error.Type, error.Code, error.Message);
	}

	private void drawTestUI()
	{
		if (showError)
		{
			drawErrorMessage();
			return;
		}
		float num = 5f;
		float horizontalMargin = HorizontalMargin;
		if (showScrollView)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(0f, 0f, Screen.width, (float)Screen.height - BannerHeight), viewRect: new Rect(0f, 0f, (float)Screen.width - ScrollBarWidth, 1200f), scrollPosition: scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: true);
		}
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "App ID: (leave empty to use the App ID defined in the manifest)");
		num += GuiLabelHeight + GuiRectSmallPadding;
		appId = GUI.TextField(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), appId);
		num += GuiTapTargetHeight + GuiRectBigPadding;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "User ID: ");
		num += GuiLabelHeight + GuiRectSmallPadding;
		userId = GUI.TextField(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), userId);
		num += GuiTapTargetHeight + GuiRectBigPadding;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "Security Token: ");
		num += GuiLabelHeight + GuiRectSmallPadding;
		securityToken = GUI.TextField(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), securityToken);
		num += GuiTapTargetHeight + GuiRectBigPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), "Send advertiser callback now"))
		{
			try
			{
				sendAdvertiserCallbackNow();
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				showError = true;
			}
		}
		num += GuiTapTargetHeight + GuiRectBigPadding;
		float num2 = 300f;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "Delay for advertiser callback (in minutes): ");
		horizontalMargin += num2 + HorizontalPadding;
		delayValue = 0;
		if (!short.TryParse(delayForAdvertiserCallback, out delayValue))
		{
			delayForAdvertiserCallback = "[invalid value entered]";
		}
		delayForAdvertiserCallback = GUI.TextField(new Rect(horizontalMargin, num, GuiRectWidth - horizontalMargin, GuiTapTargetHeight), delayForAdvertiserCallback);
		horizontalMargin = HorizontalMargin;
		num += GuiTapTargetHeight + GuiRectBigPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), "Send advertiser callback with delay of " + delayValue + " minutes"))
		{
			try
			{
				sendAdvertiserCallbackWithDelay();
			}
			catch (Exception ex2)
			{
				errorMessage = ex2.Message;
				showError = true;
			}
		}
		num += GuiTapTargetHeight + GuiRectBigPadding;
		float num3 = GuiRectWidth / 3f - HorizontalPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), "Launch OfferWall"))
		{
			try
			{
				launchOfferWall();
			}
			catch (Exception ex3)
			{
				errorMessage = ex3.Message;
				showError = true;
			}
		}
		horizontalMargin += num3 + HorizontalPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), "Launch Interstitial"))
		{
			try
			{
				launchInterstitial();
			}
			catch (Exception ex4)
			{
				errorMessage = ex4.Message;
				showError = true;
			}
		}
		horizontalMargin += num3 + HorizontalPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), "Get delta of coins"))
		{
			try
			{
				requestNewCoins();
				coinsLabel = "Waiting for response from VCS...";
			}
			catch (Exception ex5)
			{
				errorMessage = ex5.Message;
				showError = true;
			}
		}
		horizontalMargin = HorizontalMargin;
		num += GuiTapTargetHeight + GuiRectSmallPadding;
		float num4 = GuiLabelHeight * 3f;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, num4), coinsLabel);
		num += num4 + GuiRectBigPadding;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "Custom currency name: (leave empty to use default)");
		num += GuiLabelHeight + GuiRectSmallPadding;
		customCurrencyName = GUI.TextField(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), customCurrencyName);
		num += GuiTapTargetHeight + GuiRectBigPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), "Request and show Offer Banner"))
		{
			try
			{
				requestOfferBanner();
				bannerStatusLabel = "Waiting for banner...";
			}
			catch (Exception ex6)
			{
				errorMessage = ex6.Message;
				showError = true;
			}
		}
		num += GuiTapTargetHeight + GuiRectSmallPadding;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight * 3f), bannerStatusLabel);
		num += GuiLabelHeight * 3f + GuiRectSmallPadding;
		GUI.Label(new Rect(horizontalMargin, num, num3, GuiLabelHeight), "Item ID:");
		horizontalMargin += num3 + HorizontalPadding;
		GUI.Label(new Rect(horizontalMargin, num, num3, GuiLabelHeight), "Item Name:");
		horizontalMargin = HorizontalMargin;
		num += GuiLabelHeight + GuiRectSmallPadding;
		unlockItemId = GUI.TextField(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), unlockItemId);
		horizontalMargin += num3 + HorizontalPadding;
		unlockItemName = GUI.TextField(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), unlockItemName);
		horizontalMargin += num3 + HorizontalPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, num3, GuiTapTargetHeight), "Launch Unlock OW"))
		{
			try
			{
				launchUnlockOfferWall();
			}
			catch (Exception ex7)
			{
				errorMessage = ex7.Message;
				showError = true;
			}
		}
		horizontalMargin = HorizontalMargin;
		num += GuiTapTargetHeight + GuiRectBigPadding;
		if (GUI.Button(new Rect(horizontalMargin, num, GuiRectWidth, GuiTapTargetHeight), unlockRequestStatusLabel))
		{
			try
			{
				requestUnlockItemsStatus();
				unlockRequestStatusLabel = "Waiting for unlock status request response...";
			}
			catch (Exception ex8)
			{
				errorMessage = ex8.Message;
				showError = true;
			}
		}
		num += GuiTapTargetHeight + GuiRectBigPadding;
		GUI.Label(new Rect(horizontalMargin, num, GuiRectWidth, GuiLabelHeight), "Unlock Items status response:");
		num += GuiLabelHeight + GuiRectSmallPadding;
		unlockItemStatusResults = GUI.TextArea(new Rect(horizontalMargin, num, GuiRectWidth, 350f), unlockItemStatusResults);
		if (showScrollView)
		{
			GUI.EndScrollView();
		}
	}

	private void drawErrorMessage()
	{
		GUILayout.Box("Error");
		GUILayout.Label(errorMessage);
		if (GUILayout.Button("\n\n                   Ok                   \n\n"))
		{
			showError = false;
		}
	}
}
