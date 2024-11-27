using System.Collections.Generic;
using UnityEngine;

public class ShopItemSlotScript : MonoBehaviour, UIMsgListener
{
	public UISprite icon;

	public UISprite backgroundIcon;

	public bool IsForSellPage;

	public int ListID;

	public ShopListType ShopType;

	protected GameObject QualityEffectObject;

	public NGUIGameItem ShopItem { get; set; }

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
		SetShopItem();
	}

	private void Update()
	{
		if (!(icon != null))
		{
			return;
		}
		NGUIBaseItem nGUIBaseItem = ((ShopItem == null) ? null : ShopItem.baseItem);
		if (nGUIBaseItem == null || nGUIBaseItem.iconAtlas == null)
		{
			icon.gameObject.SetActive(false);
			backgroundIcon.gameObject.SetActive(false);
			Transform transform = base.transform.Find("MithrilIcon");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			Transform transform2 = base.transform.Find("MithrilOff");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
		}
		else
		{
			icon.atlas = nGUIBaseItem.iconAtlas;
			icon.spriteName = nGUIBaseItem.iconName;
			icon.gameObject.SetActive(true);
			backgroundIcon.gameObject.SetActive(true);
			backgroundIcon.atlas = nGUIBaseItem.iconAtlas;
			backgroundIcon.spriteName = nGUIBaseItem.GetBackGroundColorStringByQuality();
		}
		if ((nGUIBaseItem == null || (nGUIBaseItem.Quality != ItemQuality.Legendary && nGUIBaseItem.Quality != ItemQuality.Epic)) && QualityEffectObject != null)
		{
			Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
		if (nGUIBaseItem != null && QualityEffectObject == null)
		{
			if (ShopItem.baseItem.Quality == ItemQuality.Legendary)
			{
				GameObject original = Resources.Load("RPG_effect/RPG_UI_Orange001") as GameObject;
				QualityEffectObject = Object.Instantiate(original) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = base.transform.Find("Background").localPosition + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(7f, 7f, 7f);
			}
			else if (ShopItem.baseItem.Quality == ItemQuality.Epic)
			{
				GameObject original2 = Resources.Load("RPG_effect/RPG_UI_Purple001") as GameObject;
				QualityEffectObject = Object.Instantiate(original2) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = base.transform.Find("Background").localPosition + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(5f, 5f, 5f);
			}
			if (QualityEffectObject != null && !base.GetComponent<Collider>().enabled)
			{
				QualityEffectObject.SetActive(false);
			}
		}
	}

	private void OnClick()
	{
		if (ShopUIScript.mInstance != null)
		{
			if (ShopType == ShopListType.InBag)
			{
				SetDescriptionPosition();
			}
			ShopUIScript.mInstance.SelectedItem = this;
		}
	}

	public void SetShopItem()
	{
		ShopItem = null;
		if (QualityEffectObject != null)
		{
			Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (!IsForSellPage)
		{
			switch (ShopType)
			{
			case ShopListType.Weapon:
				if (itemInfoData.Shop_WeaponList.Count > ListID)
				{
					ShopItem = itemInfoData.Shop_WeaponList[ListID];
				}
				break;
			case ShopListType.Shield:
				if (itemInfoData.Shop_ShieldList.Count > ListID)
				{
					ShopItem = itemInfoData.Shop_ShieldList[ListID];
				}
				break;
			case ShopListType.Chip:
				if (itemInfoData.Shop_ChipList.Count > ListID)
				{
					ShopItem = itemInfoData.Shop_ChipList[ListID];
				}
				break;
			case ShopListType.Pill:
				if (itemInfoData.Shop_PillList.Count > ListID)
				{
					ShopItem = itemInfoData.Shop_PillList[ListID];
				}
				break;
			case ShopListType.BuyBack:
				if (itemInfoData.Shop_BuyBackList.Count > ListID)
				{
					ShopItem = itemInfoData.Shop_BuyBackList[ListID];
				}
				break;
			case ShopListType.BlackMarket:
			{
				if (itemInfoData.Shop_BlackMarketList.Count <= ListID)
				{
					break;
				}
				ShopItem = itemInfoData.Shop_BlackMarketList[ListID];
				Transform transform = base.transform.Find("MithrilIcon");
				if (transform != null)
				{
					if (ShopItem.baseItem.MithrilItem)
					{
						transform.gameObject.SetActive(true);
					}
					else
					{
						transform.gameObject.SetActive(false);
					}
				}
				Transform transform2 = base.transform.Find("MithrilOff");
				if (transform2 != null)
				{
					if (ShopItem.baseItem.MithrilItem && ShopItem.baseItem.MithrilOff > 0)
					{
						transform2.gameObject.SetActive(true);
						transform2.GetComponentInChildren<UILabel>().text = ShopItem.baseItem.MithrilOff + "%";
					}
					else
					{
						transform2.gameObject.SetActive(false);
					}
				}
				break;
			}
			case ShopListType.LimitSell:
				ShopItem = itemInfoData.Shop_LimitSell;
				break;
			case ShopListType.SpecailSell:
				ShopItem = itemInfoData.Shop_SpecialSell;
				break;
			}
			return;
		}
		switch (ShopType)
		{
		case ShopListType.Weapon:
			switch (ListID)
			{
			case 1:
				ShopItem = itemInfoData.Weapon1;
				break;
			case 2:
				ShopItem = itemInfoData.Weapon2;
				break;
			case 3:
				ShopItem = itemInfoData.Weapon3;
				break;
			case 4:
				ShopItem = itemInfoData.Weapon4;
				break;
			case 5:
				ShopItem = itemInfoData.HandGrenade;
				break;
			}
			break;
		case ShopListType.Shield:
			ShopItem = itemInfoData.Shield;
			break;
		case ShopListType.Chip:
			switch (ListID)
			{
			case 1:
				ShopItem = itemInfoData.Slot1;
				break;
			case 2:
				ShopItem = itemInfoData.Slot2;
				break;
			case 3:
				ShopItem = itemInfoData.Slot3;
				break;
			case 4:
				ShopItem = itemInfoData.Slot4;
				break;
			}
			break;
		case ShopListType.InBag:
			if (itemInfoData.BackPackItems.Count > ListID && base.transform.parent.GetComponent<ShopItemStorageScript>() != null)
			{
				ShopItem = itemInfoData.BackPackItems[ListID];
			}
			break;
		}
	}

	public bool CheckBeforeBuy()
	{
		if (ShopItem.baseItem.MithrilItem)
		{
			if (GameApp.GetInstance().GetGlobalState().GetMithril() < ShopItem.baseItem.MithrilPrice)
			{
				if (ShopUIScript.mInstance != null)
				{
					ShopUIScript.mInstance.MithrilNotEnough();
				}
				return false;
			}
		}
		else if (GameApp.GetInstance().GetUserState().GetCash() < ShopItem.baseItem.GetPrice())
		{
			if (ShopUIScript.mInstance != null)
			{
				ShopUIScript.mInstance.MoneyNotEnough();
			}
			return false;
		}
		if (CheckChipConflict())
		{
			ChipConflict();
			return false;
		}
		return true;
	}

	public bool CheckChipConflict()
	{
		if (ShopItem != null && ShopItem.baseItem.ItemClass == ItemClasses.V_Slot)
		{
			ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
			List<NGUIGameItem> list = new List<NGUIGameItem>();
			list.Add(itemInfoData.Slot1);
			list.Add(itemInfoData.Slot2);
			list.Add(itemInfoData.Slot3);
			list.Add(itemInfoData.Slot4);
			foreach (NGUIGameItem backPackItem in itemInfoData.BackPackItems)
			{
				if (backPackItem != null && backPackItem.baseItem.ItemClass == ItemClasses.V_Slot)
				{
					list.Add(backPackItem);
				}
			}
			foreach (NGUIGameItem item in list)
			{
				if (item != null && item.baseItem.skillIDs[0] % 100 == ShopItem.baseItem.skillIDs[0] % 100)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void AfterBuy(bool isBagFull)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		ItemInfo itemInfoData = userState.ItemInfoData;
		if (ShopType != ShopListType.BuyBack)
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Keep_the_change, AchievementTrigger.Type.Data);
			achievementTrigger.PutData(ShopItem.baseItem.GetPrice());
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
		else
		{
			AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.My_Precious, AchievementTrigger.Type.Data);
			achievementTrigger2.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger2);
		}
		if (ShopItem.baseItem.MithrilItem)
		{
			GameApp.GetInstance().GetGlobalState().BuyWithMithril(ShopItem.baseItem.MithrilPrice);
			if (ShopType != ShopListType.BuyBack && ShopItem.baseItem.ItemClass != ItemClasses.W_Pills)
			{
				userState.OperInfo.AddInfo((OperatingInfoType)(11 + ShopItem.baseItem.ItemClass - 1), ShopItem.baseItem.MithrilPrice);
			}
		}
		else
		{
			GameApp.GetInstance().GetUserState().Buy(ShopItem.baseItem.GetPrice());
			if (ShopType != ShopListType.BuyBack && ShopItem.baseItem.ItemClass != ItemClasses.W_Pills)
			{
				userState.OperInfo.AddInfo((OperatingInfoType)(1 + ShopItem.baseItem.ItemClass - 1), ShopItem.baseItem.GetPrice());
			}
		}
		if (ShopType != ShopListType.BuyBack && ShopItem.baseItem.ItemClass != ItemClasses.W_Pills)
		{
			userState.OperInfo.AddInfo((OperatingInfoType)(34 + ShopItem.baseItem.ItemClass - 1), 1);
		}
		ShopItem.baseItem.MithrilItem = false;
		if (ShopType != ShopListType.BuyBack)
		{
			if (ShopItem.baseItem.ItemClass != ItemClasses.W_Pills)
			{
				ShopItem.baseItem.SetPrice((int)((float)ShopItem.baseItem.GetPrice() * 0.025f));
			}
			else
			{
				ShopItem.baseItem.SetPrice((int)((float)ShopItem.baseItem.GetPrice() * 0.1f));
			}
		}
		if (!isBagFull)
		{
			for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
			{
				if (itemInfoData.BackPackItems[i] == null && !isBagFull)
				{
					itemInfoData.BackPackItems[i] = ShopItem;
					break;
				}
			}
		}
		else
		{
			if (ShopUIScript.mInstance != null)
			{
				ShopUIScript.mInstance.BagIsFull();
			}
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(ShopItem.baseItem, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition() + Vector3.up, Vector3.zero);
		}
		switch (ShopType)
		{
		case ShopListType.Weapon:
			if (itemInfoData.Shop_WeaponList.Count > ListID)
			{
				itemInfoData.Shop_WeaponList[ListID] = null;
			}
			break;
		case ShopListType.Shield:
			if (itemInfoData.Shop_ShieldList.Count > ListID)
			{
				itemInfoData.Shop_ShieldList[ListID] = null;
			}
			break;
		case ShopListType.Chip:
			if (itemInfoData.Shop_ChipList.Count > ListID)
			{
				itemInfoData.Shop_ChipList[ListID] = null;
			}
			break;
		case ShopListType.Pill:
			if (itemInfoData.Shop_PillList.Count > ListID)
			{
				itemInfoData.Shop_PillList[ListID] = null;
			}
			break;
		case ShopListType.BlackMarket:
			if (itemInfoData.Shop_BlackMarketList.Count > ListID)
			{
				itemInfoData.Shop_BlackMarketList[ListID] = null;
			}
			break;
		case ShopListType.SpecailSell:
			itemInfoData.Shop_SpecialSell = null;
			ShopItem.baseItem.SetPrice((int)((float)ShopItem.baseItem.GetPrice() / ((float)(100 - itemInfoData.SpecialOff) / 100f)));
			break;
		case ShopListType.LimitSell:
			itemInfoData.Shop_LimitSell = null;
			break;
		case ShopListType.BuyBack:
			if (itemInfoData.Shop_BuyBackList.Count > ListID)
			{
				itemInfoData.Shop_BuyBackList.RemoveAt(ListID);
			}
			break;
		}
		ShopUIScript.mInstance.SelectedItem = null;
		ShopItem = null;
	}

	public void AfterSell()
	{
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		switch (ShopType)
		{
		case ShopListType.Weapon:
			if (ListID != 5 && !localPlayer.CanUnEquipWeapon(this))
			{
				return;
			}
			switch (ListID)
			{
			case 1:
				itemInfoData.Weapon1 = null;
				itemInfoData.IsWeapon1Equiped = false;
				break;
			case 2:
				itemInfoData.Weapon2 = null;
				itemInfoData.IsWeapon2Equiped = false;
				break;
			case 3:
				itemInfoData.Weapon3 = null;
				itemInfoData.IsWeapon3Equiped = false;
				break;
			case 4:
				itemInfoData.Weapon4 = null;
				itemInfoData.IsWeapon4Equiped = false;
				break;
			case 5:
				itemInfoData.HandGrenade = null;
				itemInfoData.IsHandGrenadeEquiped = false;
				break;
			}
			localPlayer.RefreshWeaponListFromItemInfo();
			break;
		case ShopListType.Shield:
			itemInfoData.Shield = null;
			itemInfoData.IsShieldEquiped = false;
			localPlayer.RefreshShieldFromItemInfo();
			break;
		case ShopListType.Chip:
			switch (ListID)
			{
			case 1:
				itemInfoData.Slot1 = null;
				itemInfoData.IsSlot1Equiped = false;
				break;
			case 2:
				itemInfoData.Slot2 = null;
				itemInfoData.IsSlot2Equiped = false;
				break;
			case 3:
				itemInfoData.Slot3 = null;
				itemInfoData.IsSlot3Equiped = false;
				break;
			case 4:
				itemInfoData.Slot4 = null;
				itemInfoData.IsSlot4Equiped = false;
				break;
			}
			break;
		case ShopListType.InBag:
			itemInfoData.BackPackItems[ListID] = null;
			break;
		}
		itemInfoData.Shop_BuyBackList.Add(ShopItem);
		GameApp.GetInstance().GetUserState().AddCash(ShopItem.baseItem.GetPrice());
		ShopUIScript.mInstance.SelectedItem = null;
		ShopItem = null;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		switch (buttonId)
		{
		case UIMsg.UIMsgButton.Ok:
			UIMsgBox.instance.CloseMessage();
			if (whichMsg.EventId == 32 && ShopItem != null)
			{
				if (GameApp.GetInstance().GetUserState().ItemInfoData.BackPackIsFull())
				{
					AfterBuy(true);
				}
				else
				{
					AfterBuy(false);
				}
			}
			break;
		case UIMsg.UIMsgButton.Cancel:
			UIMsgBox.instance.CloseMessage();
			break;
		}
	}

	public void ChipConflict()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_SHOP_CHIP_WARNING"), 3, 32);
	}

	protected void SetDescriptionPosition()
	{
		if (base.transform.localPosition.x < 150f)
		{
			ShopUIScript.mInstance.Description.transform.localPosition = ShopUIScript.mInstance.DescriptionRightPoint.transform.localPosition;
			int childCount = ShopSellPageScript.mInstance.m_BackPack.transform.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				Transform child = ShopSellPageScript.mInstance.m_BackPack.transform.GetChild(i);
				for (int j = 0; j < child.GetChildCount(); j++)
				{
					if (child.GetChild(j).tag == TagName.QUALITY_EFFECT)
					{
						if (child.localPosition.x > 150f)
						{
							child.GetChild(j).gameObject.SetActive(false);
						}
						else
						{
							child.GetChild(j).gameObject.SetActive(true);
						}
					}
				}
			}
			return;
		}
		ShopUIScript.mInstance.Description.transform.localPosition = ShopUIScript.mInstance.DescriptionLeftPoint.transform.localPosition;
		int childCount2 = ShopSellPageScript.mInstance.m_BackPack.transform.GetChildCount();
		for (int k = 0; k < childCount2; k++)
		{
			Transform child2 = ShopSellPageScript.mInstance.m_BackPack.transform.GetChild(k);
			for (int l = 0; l < child2.GetChildCount(); l++)
			{
				if (child2.GetChild(l).tag == TagName.QUALITY_EFFECT)
				{
					if (child2.localPosition.x > 150f)
					{
						child2.GetChild(l).gameObject.SetActive(true);
					}
					else
					{
						child2.GetChild(l).gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
