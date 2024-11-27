using SponsorPay;
using UnityEngine;

public class SponsorPayPluginMonoBehaviour : MonoBehaviour
{
	public static SponsorPayPlugin PluginInstance;

	private void Awake()
	{
		MonoBehaviour.print("CallbackGameObjectName set to " + base.gameObject.name);
		PluginInstance = new SponsorPayPlugin(base.gameObject.name);
	}

	public void OnSPCurrencyDeltaOfCoinsMessageFromSDK(string message)
	{
		MonoBehaviour.print("OnSPCurrencyDeltaOfCoinsMessageFromSDK invoked on C# side");
		PluginInstance.HandleCurrencyDeltaOfCoinsMessageFromSDK(message);
	}

	private void OnSPUnlockItemsStatusMessageFromSDK(string message)
	{
		PluginInstance.HandleUnlockItemsStatusMessageFromSDK(message);
	}

	private void OnSPOfferBannerMessageFromSDK(string message)
	{
		MonoBehaviour.print("OnSPOfferBannerMessageFromSDK invoked");
		PluginInstance.HandleOfferBannerMessageFromSDK(message);
	}
}
