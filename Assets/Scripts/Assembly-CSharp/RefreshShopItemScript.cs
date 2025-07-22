using UnityEngine;

public class RefreshShopItemScript : MonoBehaviour, UIMsgListener
{
	public UILabel PriceLabel;

	public int Mithril { get; set; }

	private void OnEnable()
	{
		Mithril = 5;
		if (PriceLabel != null)
		{
			PriceLabel.text = Mithril.ToString();
		}
	}

	private void Update()
	{
		if (PriceLabel != null)
		{
			PriceLabel.text = Mithril.ToString();
		}
	}

	private void OnClick()
	{
		RefreshShop();
	}

	private void RefreshShop()
	{
		if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(Mithril))
		{
			if (ShopUIScript.mInstance.CurrentPage == ShopPageType.BlackMarket)
			{
				GameApp.GetInstance().GetUserState().ItemInfoData.RefreshShopItem(ShopListType.BlackMarket, (byte)GameApp.GetInstance().GetUserState().GetCharLevel(), 16);
				GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.Pause();
				ShopUIScript.mInstance.ChangePage(ShopUIScript.mInstance.CurrentPage);
				GameApp.GetInstance().GetUserState().OperInfo.AddInfo(OperatingInfoType.REFRESH_BLACK_MARKET_COUNT, 1);
			}
			else
			{
				GameApp.GetInstance().GetUserState().ItemInfoData.RefreshShopItems();
				GameApp.GetInstance().GetUserState().OperInfo.AddInfo(OperatingInfoType.REFRESH_SHOP_COUNT, 1);
			}
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 26)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(Mithril))
				{
					UIMsgBox.instance.CloseMessage();
					GameApp.GetInstance().GetUserState().ItemInfoData.RefreshShopItems();
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else if (!ShopUIScript.mInstance.IsInRefreshShopTutorial)
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 9 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			UIIAP.Show(UIIAP.Type.IAP);
		}
	}
}
