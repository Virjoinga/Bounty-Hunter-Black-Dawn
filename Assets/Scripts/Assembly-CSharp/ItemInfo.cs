using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
	public const float SHOP_REFRESH_TIME = 300f;

	public const float SPECIAL_SELL_REFRESH_MIN_TIME = 600f;

	public const float SPECIAL_SELL_REFRESH_MAX_TIME = 900f;

	public int BackpackSlotCount = 10;

	public NGUIGameItem Weapon1;

	public bool IsWeapon1Equiped;

	public NGUIGameItem Weapon2;

	public bool IsWeapon2Equiped;

	public NGUIGameItem Weapon3;

	public bool IsWeapon3Equiped;

	public bool IsWeapon3Enable;

	public NGUIGameItem Weapon4;

	public bool IsWeapon4Equiped;

	public bool IsWeapon4Enable;

	public NGUIGameItem HandGrenade;

	public bool IsHandGrenadeEquiped;

	public NGUIGameItem Shield;

	public bool IsShieldEquiped;

	public NGUIGameItem Slot1;

	public bool IsSlot1Equiped;

	public NGUIGameItem Slot2;

	public bool IsSlot2Equiped;

	public bool IsSlot2Enable;

	public NGUIGameItem Slot3;

	public bool IsSlot3Equiped;

	public bool IsSlot3Enable;

	public NGUIGameItem Slot4;

	public bool IsSlot4Equiped;

	public bool IsSlot4Enable;

	public List<NGUIGameItem> BackPackItems = new List<NGUIGameItem>();

	public Dictionary<short, StoryItem> StoryItems = new Dictionary<short, StoryItem>();

	public byte CurrentEquipWeaponSlot;

	public List<NGUIGameItem> Shop_WeaponList = new List<NGUIGameItem>();

	public List<NGUIGameItem> Shop_ShieldList = new List<NGUIGameItem>();

	public List<NGUIGameItem> Shop_ChipList = new List<NGUIGameItem>();

	public List<NGUIGameItem> Shop_PillList = new List<NGUIGameItem>();

	public List<NGUIGameItem> Shop_BlackMarketList = new List<NGUIGameItem>();

	public NGUIGameItem Shop_SpecialSell;

	public int SpecialOff;

	public NGUIGameItem Shop_LimitSell;

	public List<NGUIGameItem> Shop_BuyBackList = new List<NGUIGameItem>();

	public float TimeToShopRefresh;

	public bool IsFirstTimeToRefreshShop = true;

	public int Bag_Extend_Time;

	public ItemInfo()
	{
		for (int i = 0; i < BackpackSlotCount; i++)
		{
			BackPackItems.Add(null);
		}
	}

	public void Init()
	{
		Weapon1 = null;
		IsWeapon1Equiped = false;
		Weapon2 = null;
		IsWeapon2Equiped = false;
		Weapon3 = null;
		IsWeapon3Enable = false;
		IsWeapon3Equiped = false;
		Weapon4 = null;
		IsWeapon4Enable = false;
		IsWeapon4Equiped = false;
		Shield = null;
		IsShieldEquiped = false;
		HandGrenade = null;
		IsHandGrenadeEquiped = false;
		Slot1 = null;
		IsSlot1Equiped = false;
		Slot2 = null;
		IsSlot2Enable = false;
		IsSlot2Equiped = false;
		Slot3 = null;
		IsSlot3Enable = false;
		IsSlot3Equiped = false;
		Slot4 = null;
		IsSlot4Enable = false;
		IsSlot4Equiped = false;
		for (int i = 0; i < BackpackSlotCount; i++)
		{
			BackPackItems[i] = null;
		}
		StoryItems.Clear();
		CurrentEquipWeaponSlot = 0;
		Shop_WeaponList.Clear();
		Shop_ShieldList.Clear();
		Shop_ChipList.Clear();
		Shop_PillList.Clear();
		Shop_BlackMarketList.Clear();
		Shop_SpecialSell = null;
		Shop_LimitSell = null;
		Shop_BuyBackList.Clear();
		TimeToShopRefresh = 0f;
		IsFirstTimeToRefreshShop = true;
		BackpackSlotCount = 10;
		Bag_Extend_Time = 0;
	}

	private NGUIGameItem GetPill(NGUIBaseItem.EquipmentSlot slot)
	{
		foreach (NGUIGameItem backPackItem in BackPackItems)
		{
			if (backPackItem != null && backPackItem.baseItem != null && backPackItem.baseItem.equipmentSlot == slot)
			{
				return backPackItem;
			}
		}
		return null;
	}

	public bool HasPillInBag()
	{
		return GetPill(NGUIBaseItem.EquipmentSlot.Pill) != null;
	}

	public int GetPillCount()
	{
		int num = 0;
		foreach (NGUIGameItem backPackItem in BackPackItems)
		{
			if (backPackItem != null && backPackItem.baseItem != null && backPackItem.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Pill)
			{
				num++;
			}
		}
		return num;
	}

	public bool UsePill()
	{
		NGUIGameItem pill = GetPill(NGUIBaseItem.EquipmentSlot.Pill);
		return UsePill(pill);
	}

	public bool UsePill(NGUIGameItem item)
	{
		if (item != null && item.baseItem != null && item.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Pill)
		{
			int index = BackPackItems.IndexOf(item);
			float statValue = item.baseItem.itemStats[0].statValue;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.RecoverHP(statValue);
			BackPackItems[index] = null;
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.The_Wounded, AchievementTrigger.Type.Data);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			return true;
		}
		return false;
	}

	public void RefreshCurrentInfo()
	{
		Weapon1 = NGUIBackPackUIScript.mInstance.mWeapon1Slot.GameItem;
		IsWeapon1Equiped = Weapon1 != null;
		Weapon2 = NGUIBackPackUIScript.mInstance.mWeapon2Slot.GameItem;
		IsWeapon2Equiped = Weapon2 != null;
		if (IsWeapon3Enable)
		{
			Weapon3 = NGUIBackPackUIScript.mInstance.mWeapon3Slot.GameItem;
			IsWeapon3Equiped = Weapon3 != null;
		}
		else
		{
			Weapon3 = null;
			IsWeapon3Equiped = false;
		}
		if (IsWeapon4Enable)
		{
			Weapon4 = NGUIBackPackUIScript.mInstance.mWeapon4Slot.GameItem;
			IsWeapon4Equiped = Weapon4 != null;
		}
		else
		{
			Weapon4 = null;
			IsWeapon4Equiped = false;
		}
		HandGrenade = NGUIBackPackUIScript.mInstance.mGrenadeSlot.GameItem;
		IsHandGrenadeEquiped = HandGrenade != null;
		Shield = NGUIBackPackUIScript.mInstance.mShieldSlot.GameItem;
		IsShieldEquiped = Shield != null;
		Slot1 = NGUIBackPackUIScript.mInstance.mSlot1Slot.GameItem;
		IsSlot1Equiped = Slot1 != null;
		if (IsSlot2Enable)
		{
			Slot2 = NGUIBackPackUIScript.mInstance.mSlot2Slot.GameItem;
			IsSlot2Equiped = Slot2 != null;
		}
		else
		{
			Slot2 = null;
			IsSlot2Equiped = false;
		}
		if (IsSlot3Enable)
		{
			Slot3 = NGUIBackPackUIScript.mInstance.mSlot3Slot.GameItem;
			IsSlot3Equiped = Slot3 != null;
		}
		else
		{
			Slot3 = null;
			IsSlot3Equiped = false;
		}
		if (IsSlot4Enable)
		{
			Slot4 = NGUIBackPackUIScript.mInstance.mSlot4Slot.GameItem;
			IsSlot4Equiped = Slot4 != null;
		}
		else
		{
			Slot4 = null;
			IsSlot4Equiped = false;
		}
		BackPackItems.Clear();
		List<NGUIGameItem> items = NGUIBackPackUIScript.mInstance.mBackPack.items;
		for (int i = 0; i < items.Count; i++)
		{
			BackPackItems.Add(items[i]);
		}
	}

	public int GetItemCountByID(short itemID)
	{
		int num = 0;
		foreach (NGUIGameItem backPackItem in BackPackItems)
		{
			if (backPackItem != null && backPackItem.baseItemID == itemID)
			{
				num++;
			}
		}
		if (StoryItems.ContainsKey(itemID))
		{
			num = StoryItems[itemID].Count;
		}
		return num;
	}

	public bool HaveItem(short itemID, int itemCount)
	{
		int itemCountByID = GetItemCountByID(itemID);
		if (itemCountByID >= itemCount)
		{
			return true;
		}
		return false;
	}

	public void RemoveItem(short itemID, int itemCount)
	{
		if (!HaveItem(itemID, itemCount))
		{
			return;
		}
		int num = 0;
		List<NGUIGameItem> backPackItems = BackPackItems;
		for (int i = 0; i < backPackItems.Count; i++)
		{
			if (num >= itemCount)
			{
				break;
			}
			if (backPackItems[i] != null && backPackItems[i].baseItemID == itemID)
			{
				backPackItems[i] = null;
				num++;
			}
		}
		if (StoryItems.ContainsKey(itemID))
		{
			StoryItems.Remove(itemID);
		}
	}

	public bool HaveWeapon()
	{
		return IsWeapon1Equiped || IsWeapon2Equiped || IsWeapon3Equiped || IsWeapon4Equiped;
	}

	public bool BackPackIsFull()
	{
		for (int i = 0; i < BackpackSlotCount; i++)
		{
			if (BackPackItems[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool CanPickUpItem(NGUIBaseItem baseItem)
	{
		if (baseItem != null)
		{
			switch (baseItem.equipmentSlot)
			{
			case NGUIBaseItem.EquipmentSlot.Weapon:
				return CanPickUpWeapon();
			case NGUIBaseItem.EquipmentSlot.WeaponG:
				return CanPickUpGrenade();
			case NGUIBaseItem.EquipmentSlot.Shield:
				return CanPickUpShield();
			case NGUIBaseItem.EquipmentSlot.SkillSlot:
				return CanPickUpChip();
			default:
				return !BackPackIsFull();
			}
		}
		return false;
	}

	public bool CanPickUpWeapon()
	{
		if (!IsWeapon1Equiped)
		{
			return true;
		}
		if (!IsWeapon2Equiped)
		{
			return true;
		}
		if (IsWeapon3Enable && !IsWeapon3Equiped)
		{
			return true;
		}
		if (IsWeapon4Enable && !IsWeapon4Equiped)
		{
			return true;
		}
		if (!BackPackIsFull())
		{
			return true;
		}
		return false;
	}

	public bool CanPickUpGrenade()
	{
		if (!IsHandGrenadeEquiped)
		{
			return true;
		}
		if (!BackPackIsFull())
		{
			return true;
		}
		return false;
	}

	public bool CanPickUpShield()
	{
		if (!IsShieldEquiped)
		{
			return true;
		}
		if (!BackPackIsFull())
		{
			return true;
		}
		return false;
	}

	public bool CanPickUpChip()
	{
		if (!IsSlot1Equiped)
		{
			return true;
		}
		if (IsSlot2Enable && !IsSlot2Equiped)
		{
			return true;
		}
		if (IsSlot3Enable && !IsSlot3Equiped)
		{
			return true;
		}
		if (IsSlot4Enable && !IsSlot4Equiped)
		{
			return true;
		}
		if (!BackPackIsFull())
		{
			return true;
		}
		return false;
	}

	public void ClearShopItemList()
	{
		Shop_WeaponList.Clear();
		Shop_ShieldList.Clear();
		Shop_ChipList.Clear();
		Shop_PillList.Clear();
		Shop_LimitSell = null;
	}

	public void RefreshShopItems()
	{
		ClearShopItemList();
		byte charLevel = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
		RefreshShopItem(ShopListType.Weapon, charLevel, 16);
		RefreshShopItem(ShopListType.Pill, charLevel, 9);
		if (ShopUIScript.mInstance != null && (ShopUIScript.mInstance.CurrentPage == ShopPageType.Equip || ShopUIScript.mInstance.CurrentPage == ShopPageType.Pill || ShopUIScript.mInstance.CurrentPage == ShopPageType.BuyBack))
		{
			ShopUIScript.mInstance.ChangePage(ShopUIScript.mInstance.CurrentPage);
		}
		GameApp.GetInstance().GetGameWorld().mShopItemRefreshTimer.SetTimer(300f, false);
		GameApp.GetInstance().Save();
	}

	public void RefreshShopItem(ShopListType shopListType, byte charLevel, int itemCount)
	{
		if (shopListType == ShopListType.BlackMarket)
		{
			Shop_BlackMarketList.Clear();
			GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.SetTimer(RandomSpecialSellTime(), false);
		}
		for (int i = 0; i < itemCount; i++)
		{
			GameObject gameObject = new GameObject();
			int num = Random.Range(0, 2);
			if (charLevel <= num)
			{
				num = charLevel - 1;
			}
			byte level = (byte)(charLevel - num);
			int num2 = Random.Range(0, 100);
			byte b = 0;
			if ((float)num2 < 12.5f)
			{
				b = 2;
			}
			else if (num2 < 22 && charLevel >= 3)
			{
				b = 3;
			}
			ItemClasses itemClass = ItemClasses.AssultRifle;
			switch (b)
			{
			case 0:
			{
				int num3 = 1;
				num3 = Random.Range(1, 9);
				if (charLevel < 9 && num3 == 7)
				{
					num3 = Random.Range(1, 7);
				}
				if (num3 == 8)
				{
					b = 1;
				}
				itemClass = (ItemClasses)num3;
				break;
			}
			case 2:
				itemClass = ItemClasses.U_Shield;
				break;
			case 3:
				itemClass = ItemClasses.V_Slot;
				break;
			}
			if (shopListType == ShopListType.Pill)
			{
				b = 4;
				itemClass = ItemClasses.W_Pills;
			}
			int num4 = Random.Range(0, 100);
			ItemQuality itemQuality;
			switch (shopListType)
			{
			case ShopListType.Chip:
				itemQuality = ((num4 >= 5) ? ((num4 >= 15) ? ((num4 >= 35) ? ((num4 >= 90) ? ItemQuality.Common : ItemQuality.Uncommon) : ItemQuality.Rare) : ItemQuality.Epic) : ItemQuality.Legendary);
				break;
			case ShopListType.BlackMarket:
				itemQuality = ((num4 >= 15) ? ((num4 >= 65) ? ItemQuality.Rare : ItemQuality.Epic) : ItemQuality.Legendary);
				break;
			default:
				itemQuality = ((num4 >= 3) ? ((num4 >= 25) ? ItemQuality.Uncommon : ItemQuality.Rare) : ItemQuality.Epic);
				break;
			}
			Vector3 vector = new Vector3(0f, 1000f, 0f);
			string itemName = string.Empty;
			int number = 1;
			byte itemType = 0;
			GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
			if (b == 4 || b == 5 || b == 6)
			{
				itemQuality = ItemQuality.Common;
			}
			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				gameObject.GetComponent<Rigidbody>().useGravity = false;
				gameObject.GetComponent<Rigidbody>().Sleep();
			}
			GameApp.GetInstance().GetLootManager().CreateItemBaseFromGameObject(gameObject, level, itemClass, (byte)number, itemQuality, 0);
			int price = gameObject.GetComponent<ItemBase>().mNGUIBaseItem.GetPrice();
			if (b != 4)
			{
				gameObject.GetComponent<ItemBase>().mNGUIBaseItem.SetPrice(price * 40);
			}
			else
			{
				gameObject.GetComponent<ItemBase>().mNGUIBaseItem.SetPrice(price * 10);
			}
			NGUIGameItem nGUIGameItem = new NGUIGameItem(0, gameObject.GetComponent<ItemBase>().mNGUIBaseItem);
			switch (shopListType)
			{
			case ShopListType.Weapon:
				Shop_WeaponList.Add(nGUIGameItem);
				break;
			case ShopListType.Shield:
				Shop_ShieldList.Add(nGUIGameItem);
				break;
			case ShopListType.Chip:
				Shop_ChipList.Add(nGUIGameItem);
				break;
			case ShopListType.Pill:
				Shop_PillList.Add(nGUIGameItem);
				break;
			case ShopListType.BlackMarket:
			{
				int num5 = 50;
				if (b == 3 && (itemQuality == ItemQuality.Epic || itemQuality == ItemQuality.Legendary))
				{
					num5 = 100;
				}
				if (Random.Range(0, 100) < num5)
				{
					nGUIGameItem.baseItem.MithrilItem = true;
					if (Random.Range(0, 2) < 1)
					{
						nGUIGameItem.baseItem.MithrilOff = Random.Range(5, 11);
					}
				}
				Shop_BlackMarketList.Add(nGUIGameItem);
				break;
			}
			}
			Object.Destroy(gameObject);
		}
	}

	public void RefreshShopSpecialSellingItem(byte charLevel)
	{
		SpecialOff = 10 + 5 * Random.Range(0, 5);
		GameObject gameObject = new GameObject();
		int num = Random.Range(0, 2);
		if (charLevel <= num)
		{
			num = charLevel - 1;
		}
		byte level = (byte)(charLevel - num);
		int num2 = Random.Range(0, 100);
		byte b = 0;
		if ((float)num2 < 12.5f)
		{
			b = 2;
		}
		else if (num2 < 22)
		{
			b = 3;
		}
		ItemClasses itemClass = ItemClasses.AssultRifle;
		switch (b)
		{
		case 0:
		{
			int num3 = 1;
			num3 = Random.Range(1, 9);
			if (charLevel < 9 && num3 == 7)
			{
				num3 = Random.Range(1, 7);
			}
			if (num3 == 8)
			{
				b = 1;
			}
			itemClass = (ItemClasses)num3;
			break;
		}
		case 2:
			itemClass = ItemClasses.U_Shield;
			break;
		case 3:
			itemClass = ItemClasses.V_Slot;
			break;
		}
		ItemQuality quality = ItemQuality.Epic;
		int num4 = Random.Range(0, 100);
		if (num4 < 50)
		{
			quality = ItemQuality.Rare;
		}
		Vector3 vector = new Vector3(0f, 1000f, 0f);
		string itemName = string.Empty;
		int number = 1;
		byte itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
		string text = GameApp.GetInstance().GetLootManager().GeneratePrefabName(b, itemName);
		if (b == 4 || b == 5 || b == 6)
		{
			quality = ItemQuality.Common;
		}
		if (gameObject.GetComponent<Rigidbody>() != null)
		{
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Rigidbody>().Sleep();
		}
		GameApp.GetInstance().GetLootManager().CreateItemBaseFromGameObject(gameObject, level, itemClass, (byte)number, quality, 0);
		int price = gameObject.GetComponent<ItemBase>().mNGUIBaseItem.GetPrice();
		gameObject.GetComponent<ItemBase>().mNGUIBaseItem.SetPrice(price * 40);
		NGUIGameItem nGUIGameItem = new NGUIGameItem(0, gameObject.GetComponent<ItemBase>().mNGUIBaseItem);
		nGUIGameItem.baseItem.SetPrice((int)((float)(nGUIGameItem.baseItem.GetPrice() * (100 - SpecialOff)) / 100f));
		Shop_SpecialSell = nGUIGameItem;
		Object.Destroy(gameObject);
	}

	public void RefreshShopLimitSellingItem(byte charLevel)
	{
		GameObject gameObject = new GameObject();
		byte level = charLevel;
		int num = Random.Range(0, 100);
		byte b = 0;
		if (num < 11)
		{
			b = 2;
		}
		else if (num < 22)
		{
			b = 3;
		}
		ItemClasses itemClass = ItemClasses.AssultRifle;
		switch (b)
		{
		case 0:
		{
			int num2 = 1;
			num2 = Random.Range(1, 9);
			if (charLevel < 9 && num2 == 7)
			{
				num2 = Random.Range(1, 7);
			}
			if (num2 == 8)
			{
				b = 1;
			}
			itemClass = (ItemClasses)num2;
			break;
		}
		case 2:
			itemClass = ItemClasses.U_Shield;
			break;
		case 3:
			itemClass = ItemClasses.V_Slot;
			break;
		}
		ItemQuality quality = ItemQuality.Epic;
		int num3 = Random.Range(0, 100);
		if (num3 < 15)
		{
			quality = ItemQuality.Legendary;
		}
		else if (num3 < 100)
		{
			quality = ItemQuality.Epic;
		}
		Vector3 vector = new Vector3(0f, 1000f, 0f);
		string itemName = string.Empty;
		int number = 1;
		byte itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(itemClass, ref itemName, ref number, ref itemType);
		string text = GameApp.GetInstance().GetLootManager().GeneratePrefabName(b, itemName);
		if (b == 4 || b == 5 || b == 6)
		{
			quality = ItemQuality.Common;
		}
		if (gameObject.GetComponent<Rigidbody>() != null)
		{
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Rigidbody>().Sleep();
		}
		GameApp.GetInstance().GetLootManager().CreateItemBaseFromGameObject(gameObject, level, itemClass, (byte)number, quality, 0);
		int price = gameObject.GetComponent<ItemBase>().mNGUIBaseItem.GetPrice();
		gameObject.GetComponent<ItemBase>().mNGUIBaseItem.SetPrice(price * 40);
		NGUIGameItem shop_LimitSell = new NGUIGameItem(0, gameObject.GetComponent<ItemBase>().mNGUIBaseItem);
		Shop_LimitSell = shop_LimitSell;
		Object.Destroy(gameObject);
	}

	public void AddStoryItem(short specialId)
	{
		AddStoryItem(specialId, true);
	}

	public void AddStoryItem(short specialId, bool needVerify)
	{
		if (needVerify && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && !GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(specialId))
		{
			Debug.Log("bbbbbbbbbbbbbbbbbbbbbbbbbb");
			return;
		}
		NGUIBaseItem nGUIBaseItem = new NGUIBaseItem();
		nGUIBaseItem.name = string.Empty;
		nGUIBaseItem.iconName = string.Empty;
		nGUIBaseItem.previewIconName = string.Empty;
		NGUIGameItem item = new NGUIGameItem(specialId, nGUIBaseItem);
		if (StoryItems.ContainsKey(specialId))
		{
			StoryItems[specialId].Count++;
			return;
		}
		StoryItem storyItem = new StoryItem();
		storyItem.Item = item;
		storyItem.Count = 1;
		StoryItems.Add(specialId, storyItem);
	}

	public static float RandomSpecialSellTime()
	{
		if (GameApp.GetInstance().GetGameWorld() != null)
		{
			GameApp.GetInstance().GetGameWorld().mBlackMarketInfoPoped = false;
		}
		return BlackMarketIcon.BlackMarketCD = Random.Range(600f, 900f);
	}
}
