using UnityEngine;

public class ShopUIScript : UIGameMenuShop, UIMsgListener
{
	public static ShopUIScript mInstance;

	public GameObject EquipPage;

	public GameObject BulletPage;

	public GameObject PillItemPage;

	public GameObject BuyBackPage;

	public GameObject CommonPage;

	public GameObject SellPage;

	public GameObject BlackMarketPage;

	public GameObject OtherObjects;

	public GameObject Description;

	public GameObject ChangeBuyPage;

	public GameObject ChangeSellPage;

	public GameObject ChangeBlackMarketPage;

	private string ChangeBuyName;

	private string ChangeSellName;

	private string ChangeBlackMarketName;

	public GameObject BuyButton;

	public GameObject RefreshButton;

	public GameObject ExtendSMGButton;

	public GameObject BuySMGButton;

	public GameObject SellButton;

	public GameObject DescriptionOriginPoint;

	public GameObject DescriptionLeftPoint;

	public GameObject DescriptionRightPoint;

	private bool isInFirstGunTutorial;

	private bool isInRefreshShopTutorial;

	private bool isInBulletTutorial;

	private bool isInSellItemTutorial;

	private bool isInBuyBackTutorial;

	private ShopItemSlotScript m_SeletedItem;

	public bool IsInFirstGunTutorial
	{
		get
		{
			return isInFirstGunTutorial;
		}
	}

	public bool IsInRefreshShopTutorial
	{
		get
		{
			return isInRefreshShopTutorial;
		}
	}

	public bool IsInBulletTutorial
	{
		get
		{
			return isInBulletTutorial;
		}
	}

	public bool IsInSellItemTutorial
	{
		get
		{
			return isInSellItemTutorial;
		}
	}

	public bool IsInBuyBackTutorial
	{
		get
		{
			return isInBuyBackTutorial;
		}
	}

	public ShopItemSlotScript SelectedItem
	{
		get
		{
			return m_SeletedItem;
		}
		set
		{
			if (m_SeletedItem != null)
			{
				UIFrameManager.GetInstance().DeleteFrame(m_SeletedItem.gameObject);
			}
			m_SeletedItem = value;
			if (Description != null && Description.GetComponent<ItemDescriptionScript>() != null)
			{
				if (value != null && value.ShopItem != null)
				{
					UIFrameManager.GetInstance().CreateFrame(m_SeletedItem.gameObject, new Vector2(0f, 0f), -11f);
					Description.GetComponent<ItemDescriptionScript>().SetObserveItem(value.ShopItem.baseItem);
				}
				else
				{
					Description.GetComponent<ItemDescriptionScript>().SetObserveItem(null);
				}
			}
		}
	}

	public ShopPageType CurrentPage { get; set; }

	protected override void Awake()
	{
		base.Awake();
		mInstance = this;
		CurrentPage = ShopPageType.Equip;
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Keep_the_change, AchievementTrigger.Type.Start);
		AchievementManager.GetInstance().Trigger(trigger);
	}

	private void Start()
	{
		AddDelegate(ChangeBuyPage, out ChangeBuyName);
		AddDelegate(ChangeSellPage, out ChangeSellName);
		AddDelegate(ChangeBlackMarketPage, out ChangeBlackMarketName);
	}

	private void Update()
	{
	}

	protected override void OnDestroy()
	{
		if (CurrentPage == ShopPageType.BuyBack)
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.Shop_BuyBackList.Reverse();
		}
		else if (CurrentPage == ShopPageType.BlackMarket)
		{
			GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.SetTimer(ItemInfo.RandomSpecialSellTime(), false);
		}
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Keep_the_change, AchievementTrigger.Type.Stop);
		AchievementManager.GetInstance().Trigger(trigger);
		GameApp.GetInstance().Save();
		base.OnDestroy();
		mInstance = null;
	}

	public void ChangePage(ShopPageType page)
	{
		if (m_SeletedItem != null)
		{
			UIFrameManager.GetInstance().DeleteFrame(m_SeletedItem.gameObject);
		}
		switch (CurrentPage)
		{
		case ShopPageType.Equip:
			EquipPage.SetActive(false);
			break;
		case ShopPageType.Bullet:
			BulletPage.SetActive(false);
			break;
		case ShopPageType.Pill:
			PillItemPage.SetActive(false);
			break;
		case ShopPageType.BuyBack:
			if (CurrentPage != page)
			{
				GameApp.GetInstance().GetUserState().ItemInfoData.Shop_BuyBackList.Reverse();
			}
			BuyBackPage.SetActive(false);
			if (!RefreshButton.activeSelf)
			{
				RefreshButton.SetActive(true);
			}
			break;
		case ShopPageType.Sell:
			SellPage.SetActive(false);
			Description.SetActive(true);
			Description.transform.localPosition = DescriptionOriginPoint.transform.localPosition;
			Description.GetComponent<ItemDescriptionScript>().CloseBtn.gameObject.SetActive(false);
			break;
		case ShopPageType.BlackMarket:
			BlackMarketPage.SetActive(false);
			GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.SetTimer(ItemInfo.RandomSpecialSellTime(), false);
			if (RefreshButton != null)
			{
				RefreshButton.GetComponent<RefreshShopItemScript>().Mithril = 10;
			}
			break;
		}
		switch (page)
		{
		case ShopPageType.Equip:
			EquipPage.SetActive(true);
			CommonPage.SetActive(true);
			Description.SetActive(true);
			CurrentPage = ShopPageType.Equip;
			break;
		case ShopPageType.Bullet:
			BulletPage.SetActive(true);
			CommonPage.SetActive(false);
			Description.SetActive(false);
			CurrentPage = ShopPageType.Bullet;
			break;
		case ShopPageType.Pill:
			PillItemPage.SetActive(true);
			CommonPage.SetActive(true);
			Description.SetActive(true);
			CurrentPage = ShopPageType.Pill;
			break;
		case ShopPageType.BuyBack:
			if (CurrentPage != page)
			{
				while (GameApp.GetInstance().GetUserState().ItemInfoData.Shop_BuyBackList.Count > 15)
				{
					GameApp.GetInstance().GetUserState().ItemInfoData.Shop_BuyBackList.RemoveAt(0);
				}
				GameApp.GetInstance().GetUserState().ItemInfoData.Shop_BuyBackList.Reverse();
			}
			BuyBackPage.SetActive(true);
			CommonPage.SetActive(true);
			Description.SetActive(true);
			if (RefreshButton.activeSelf)
			{
				RefreshButton.SetActive(false);
			}
			CurrentPage = ShopPageType.BuyBack;
			break;
		case ShopPageType.Sell:
			SellPage.SetActive(true);
			CommonPage.SetActive(false);
			Description.transform.localPosition = DescriptionRightPoint.transform.localPosition;
			Description.GetComponent<ItemDescriptionScript>().CloseBtn.gameObject.SetActive(true);
			Description.SetActive(false);
			CurrentPage = ShopPageType.Sell;
			break;
		case ShopPageType.BlackMarket:
			GameApp.GetInstance().GetUserState().ItemInfoData.RefreshShopItem(ShopListType.BlackMarket, (byte)GameApp.GetInstance().GetUserState().GetCharLevel(), 16);
			GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.Pause();
			BlackMarketPage.SetActive(true);
			CommonPage.SetActive(true);
			Description.SetActive(true);
			if (RefreshButton != null)
			{
				RefreshButton.GetComponent<RefreshShopItemScript>().Mithril = 15;
			}
			CurrentPage = ShopPageType.BlackMarket;
			ChangeBuyPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_n";
			ChangeBuyPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "buy_n";
			ChangeSellPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_n";
			ChangeSellPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "sell_n";
			break;
		}
		SelectedItem = null;
		ShopItemSlotScript[] componentsInChildren = base.transform.GetComponentsInChildren<ShopItemSlotScript>();
		ShopItemSlotScript[] array = componentsInChildren;
		foreach (ShopItemSlotScript shopItemSlotScript in array)
		{
			shopItemSlotScript.SetShopItem();
		}
	}

	public bool IsInBuyPage()
	{
		if (CurrentPage == ShopPageType.Sell)
		{
			return false;
		}
		return true;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 9)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
			}
		}
		else if (whichMsg.EventId == 99)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.Exchange);
			}
		}
		else if (whichMsg.EventId == 30)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 33)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else
		{
			if (whichMsg.EventId != 35)
			{
				return;
			}
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(Mathf.CeilToInt(25f * GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.GetTimeNeededToNextReady() / BlackMarketIcon.BlackMarketCD)))
				{
					UIMsgBox.instance.CloseMessage();
					ChangePage(ShopPageType.BlackMarket);
					GameApp.GetInstance().Save();
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
	}

	public void MoneyNotEnough()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
	}

	public void MithrilNotEnough()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
	}

	public void BagIsFull()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_BAG_FULL"), 2, 30);
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (IsThisObject(go, ChangeBuyName))
		{
			if (CurrentPage == ShopPageType.Sell || CurrentPage == ShopPageType.BlackMarket)
			{
				ChangeBuyPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_d";
				ChangeBuyPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "buy_d";
				ChangeSellPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_n";
				ChangeSellPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "sell_n";
				ChangePage(ShopPageType.Equip);
			}
		}
		else if (IsThisObject(go, ChangeSellName))
		{
			if (CurrentPage != ShopPageType.Sell)
			{
				ChangeBuyPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_n";
				ChangeBuyPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "buy_n";
				ChangeSellPage.transform.Find("Background").GetComponent<UISprite>().spriteName = "sure_bk_d";
				ChangeSellPage.transform.Find("Icon").GetComponent<UISprite>().spriteName = "sell_d";
				ChangePage(ShopPageType.Sell);
			}
		}
		else if (IsThisObject(go, ChangeBlackMarketName) && CurrentPage != ShopPageType.BlackMarket)
		{
			if (GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.Ready())
			{
				ChangePage(ShopPageType.BlackMarket);
				GameApp.GetInstance().Save();
			}
			else
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_ENTER_BLACK_MARKET"), 3, 35);
			}
		}
	}

	public void BlockAllButton(bool isBlock)
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>(true);
		if (componentsInChildren == null)
		{
			return;
		}
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				collider.enabled = !isBlock;
			}
		}
	}

	public void BlockForBuyFirstGunTutorial(bool isBlock)
	{
		isInFirstGunTutorial = isBlock;
		BlockAllButton(isBlock);
		ShopItemSlotScript[] componentsInChildren = GetComponentsInChildren<ShopItemSlotScript>(true);
		foreach (ShopItemSlotScript shopItemSlotScript in componentsInChildren)
		{
			if (shopItemSlotScript.ShopType == ShopListType.Weapon && shopItemSlotScript.ListID == 0)
			{
				Debug.Log("First Gun Find!");
				shopItemSlotScript.GetComponent<Collider>().enabled = true;
			}
		}
		BuyButton.GetComponent<Collider>().enabled = true;
	}

	public void BlockForRefreshTutorial(bool isBlock)
	{
		isInRefreshShopTutorial = isBlock;
		BlockAllButton(isBlock);
		RefreshButton.GetComponent<Collider>().enabled = true;
	}

	public void BlockForBulletTutorial(bool isBlock)
	{
		isInBulletTutorial = isBlock;
		BlockAllButton(isBlock);
		ExtendSMGButton.GetComponent<Collider>().enabled = true;
	}

	public void BlockForSellItemTutorial(bool isBlock)
	{
		isInSellItemTutorial = isBlock;
		BlockAllButton(isBlock);
		ShopItemSlotScript[] componentsInChildren = GetComponentsInChildren<ShopItemSlotScript>(true);
		foreach (ShopItemSlotScript shopItemSlotScript in componentsInChildren)
		{
			if (shopItemSlotScript.ShopType == ShopListType.InBag && shopItemSlotScript.ListID == 0)
			{
				Debug.Log("First Item In Bag Find!");
				shopItemSlotScript.GetComponent<Collider>().enabled = true;
			}
		}
		SellButton.GetComponent<Collider>().enabled = true;
	}

	public void BlockForBuyBackTutorial(bool isBlock)
	{
		isInBuyBackTutorial = isBlock;
		BlockAllButton(isBlock);
		ShopItemSlotScript[] componentsInChildren = GetComponentsInChildren<ShopItemSlotScript>(true);
		foreach (ShopItemSlotScript shopItemSlotScript in componentsInChildren)
		{
			if (shopItemSlotScript.ShopType == ShopListType.BuyBack && shopItemSlotScript.ListID == 0)
			{
				Debug.Log("First Item In Buy Back Find!");
				shopItemSlotScript.GetComponent<Collider>().enabled = true;
			}
		}
		BuyButton.GetComponent<Collider>().enabled = true;
	}

	public void OpenChangeToBulletPageButton()
	{
		Collider[] componentsInChildren = base.transform.GetComponentsInChildren<Collider>(true);
		foreach (Collider collider in componentsInChildren)
		{
			if (collider.name == "BulletButton")
			{
				collider.GetComponent<Collider>().enabled = true;
			}
		}
	}

	public void OpenChangeToBuyBackPageButton()
	{
		ChangeBuyPage.GetComponent<Collider>().enabled = true;
		Collider[] componentsInChildren = base.transform.GetComponentsInChildren<Collider>(true);
		foreach (Collider collider in componentsInChildren)
		{
			if (collider.name == "BuyBackButton")
			{
				collider.GetComponent<Collider>().enabled = true;
			}
		}
	}

	public void OpenChangeToSellPageButton()
	{
		ChangeSellPage.GetComponent<Collider>().enabled = true;
	}

	public void OpenChangeToBuyPageButton()
	{
		ChangeBuyPage.GetComponent<Collider>().enabled = true;
	}

	public void CloseExtendSMGButton()
	{
		ExtendSMGButton.GetComponent<Collider>().enabled = false;
		BuySMGButton.GetComponent<Collider>().enabled = true;
	}
}
