namespace SponsorPay
{
	public interface ISponsorPayPlugin
	{
		void SendAdvertiserCallbackNow(string appId);

		void SendAdvertiserCallbackWithDelay(string appId, int delayMin);

		void LaunchOfferWall(string appId, string userId);

		void LaunchInterstitial(string appId, string userId);

		void RequestNewCoins(string appId, string userId, string securityToken);

		double LastReceivedDeltaOfCoins();

		string LastReceivedTransactionId();

		string RequestResultSuccessMessage();

		RequestError LastReceivedErrorForCurrencyStatusRequest();

		void LaunchSPUnlockOfferWall(string appId, string userId, string unlockItemId, string unlockItemName);

		void RequestSPUnlockItemsStatus(string appId, string userId, string securityToken);

		RequestError LastReceivedErrorForUnlockStatusRequest();

		UnlockItem[] LastReceivedUnlockItems();

		void RequestOfferBanner(string appId, string userId, string currencyName);

		void ShowLastReceivedOfferBanner(SPBannerPosition position);

		void RemoveOfferBannerFromScreen();

		string RequestResultOfferBannerAvailableMessage();

		string RequestResultOfferBannerNotAvailableMessage();
	}
}
