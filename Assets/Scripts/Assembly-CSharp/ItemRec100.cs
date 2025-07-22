using System.IO;

public class ItemRec100 : ISubRecordset
{
	private ItemInfo m_ItemInfo;

	public ItemRec100(ItemInfo itemInfo)
	{
		m_ItemInfo = itemInfo;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(m_ItemInfo.BackpackSlotCount);
		bw.Write(m_ItemInfo.IsWeapon3Enable);
		bw.Write(m_ItemInfo.IsWeapon4Enable);
		bw.Write(m_ItemInfo.IsSlot2Enable);
		bw.Write(m_ItemInfo.IsSlot3Enable);
		bw.Write(m_ItemInfo.IsSlot4Enable);
		bw.Write(m_ItemInfo.IsWeapon1Equiped);
		bw.Write(m_ItemInfo.IsWeapon2Equiped);
		bw.Write(m_ItemInfo.IsWeapon3Equiped);
		bw.Write(m_ItemInfo.IsWeapon4Equiped);
		bw.Write(m_ItemInfo.IsHandGrenadeEquiped);
		bw.Write(m_ItemInfo.IsShieldEquiped);
		bw.Write(m_ItemInfo.IsSlot1Equiped);
		bw.Write(m_ItemInfo.IsSlot2Equiped);
		bw.Write(m_ItemInfo.IsSlot3Equiped);
		bw.Write(m_ItemInfo.IsSlot4Equiped);
		bw.Write(m_ItemInfo.StoryItems.Count);
		if (m_ItemInfo.Weapon1 != null)
		{
			bw.Write(m_ItemInfo.Weapon1.baseItemID);
			SaveItem(m_ItemInfo.Weapon1.baseItem, bw);
		}
		if (m_ItemInfo.Weapon2 != null)
		{
			bw.Write(m_ItemInfo.Weapon2.baseItemID);
			SaveItem(m_ItemInfo.Weapon2.baseItem, bw);
		}
		if (m_ItemInfo.IsWeapon3Enable && m_ItemInfo.Weapon3 != null)
		{
			bw.Write(m_ItemInfo.Weapon3.baseItemID);
			SaveItem(m_ItemInfo.Weapon3.baseItem, bw);
		}
		if (m_ItemInfo.IsWeapon4Enable && m_ItemInfo.Weapon4 != null)
		{
			bw.Write(m_ItemInfo.Weapon4.baseItemID);
			SaveItem(m_ItemInfo.Weapon4.baseItem, bw);
		}
		if (m_ItemInfo.HandGrenade != null)
		{
			bw.Write(m_ItemInfo.HandGrenade.baseItemID);
			SaveItem(m_ItemInfo.HandGrenade.baseItem, bw);
		}
		if (m_ItemInfo.Shield != null)
		{
			bw.Write(m_ItemInfo.Shield.baseItemID);
			SaveItem(m_ItemInfo.Shield.baseItem, bw);
		}
		if (m_ItemInfo.Slot1 != null)
		{
			bw.Write(m_ItemInfo.Slot1.baseItemID);
			SaveItem(m_ItemInfo.Slot1.baseItem, bw);
		}
		if (m_ItemInfo.IsSlot2Enable && m_ItemInfo.Slot2 != null)
		{
			bw.Write(m_ItemInfo.Slot2.baseItemID);
			SaveItem(m_ItemInfo.Slot2.baseItem, bw);
		}
		if (m_ItemInfo.IsSlot3Enable && m_ItemInfo.Slot3 != null)
		{
			bw.Write(m_ItemInfo.Slot3.baseItemID);
			SaveItem(m_ItemInfo.Slot3.baseItem, bw);
		}
		if (m_ItemInfo.IsSlot4Enable && m_ItemInfo.Slot4 != null)
		{
			bw.Write(m_ItemInfo.Slot4.baseItemID);
			SaveItem(m_ItemInfo.Slot4.baseItem, bw);
		}
		for (int i = 0; i < m_ItemInfo.BackpackSlotCount; i++)
		{
			if (m_ItemInfo.BackPackItems[i] != null)
			{
				bw.Write(m_ItemInfo.BackPackItems[i].baseItemID);
				SaveItem(m_ItemInfo.BackPackItems[i].baseItem, bw);
			}
			else
			{
				bw.Write(-1);
			}
		}
		foreach (StoryItem value in m_ItemInfo.StoryItems.Values)
		{
			bw.Write(value.Item.baseItemID);
			SaveItem(value.Item.baseItem, bw);
			bw.Write(value.Count);
		}
		bw.Write(m_ItemInfo.CurrentEquipWeaponSlot);
		bw.Write(m_ItemInfo.IsFirstTimeToRefreshShop);
		if (GameApp.GetInstance().GetGameWorld() != null)
		{
			m_ItemInfo.TimeToShopRefresh = GameApp.GetInstance().GetGameWorld().mShopItemRefreshTimer.GetTimeNeededToNextReady();
			bw.Write(m_ItemInfo.TimeToShopRefresh);
		}
		else
		{
			bw.Write(300f);
		}
		if (GameApp.GetInstance().GetGameWorld() != null)
		{
			BlackMarketIcon.BlackMarketTimeToReady = GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.GetTimeNeededToNextReady();
		}
		else
		{
			BlackMarketIcon.BlackMarketTimeToReady = 120f;
		}
		bw.Write(BlackMarketIcon.BlackMarketTimeToReady);
		bw.Write(BlackMarketIcon.BlackMarketCD);
		bw.Write(m_ItemInfo.Shop_WeaponList.Count);
		for (int j = 0; j < m_ItemInfo.Shop_WeaponList.Count; j++)
		{
			if (m_ItemInfo.Shop_WeaponList[j] != null)
			{
				bw.Write(m_ItemInfo.Shop_WeaponList[j].baseItemID);
				SaveItem(m_ItemInfo.Shop_WeaponList[j].baseItem, bw);
			}
			else
			{
				bw.Write(-1);
			}
		}
		bw.Write(m_ItemInfo.Shop_ShieldList.Count);
		for (int k = 0; k < m_ItemInfo.Shop_ShieldList.Count; k++)
		{
			if (m_ItemInfo.Shop_ShieldList[k] != null)
			{
				bw.Write(m_ItemInfo.Shop_ShieldList[k].baseItemID);
				SaveItem(m_ItemInfo.Shop_ShieldList[k].baseItem, bw);
			}
			else
			{
				bw.Write(-1);
			}
		}
		bw.Write(m_ItemInfo.Shop_ChipList.Count);
		for (int l = 0; l < m_ItemInfo.Shop_ChipList.Count; l++)
		{
			if (m_ItemInfo.Shop_ChipList[l] != null)
			{
				bw.Write(m_ItemInfo.Shop_ChipList[l].baseItemID);
				SaveItem(m_ItemInfo.Shop_ChipList[l].baseItem, bw);
			}
			else
			{
				bw.Write(-1);
			}
		}
		bw.Write(m_ItemInfo.Shop_PillList.Count);
		for (int m = 0; m < m_ItemInfo.Shop_PillList.Count; m++)
		{
			if (m_ItemInfo.Shop_PillList[m] != null)
			{
				bw.Write(m_ItemInfo.Shop_PillList[m].baseItemID);
				SaveItem(m_ItemInfo.Shop_PillList[m].baseItem, bw);
			}
			else
			{
				bw.Write(-1);
			}
		}
		if (m_ItemInfo.Shop_LimitSell != null)
		{
			bw.Write(m_ItemInfo.Shop_LimitSell.baseItemID);
			SaveItem(m_ItemInfo.Shop_LimitSell.baseItem, bw);
		}
		else
		{
			bw.Write(-1);
		}
	}

	public void LoadData(BinaryReader br)
	{
		m_ItemInfo.Init();
		m_ItemInfo.BackpackSlotCount = br.ReadInt32();
		m_ItemInfo.IsWeapon3Enable = br.ReadBoolean();
		m_ItemInfo.IsWeapon4Enable = br.ReadBoolean();
		m_ItemInfo.IsSlot2Enable = br.ReadBoolean();
		m_ItemInfo.IsSlot3Enable = br.ReadBoolean();
		m_ItemInfo.IsSlot4Enable = br.ReadBoolean();
		m_ItemInfo.IsWeapon1Equiped = br.ReadBoolean();
		m_ItemInfo.IsWeapon2Equiped = br.ReadBoolean();
		m_ItemInfo.IsWeapon3Equiped = br.ReadBoolean();
		m_ItemInfo.IsWeapon4Equiped = br.ReadBoolean();
		m_ItemInfo.IsHandGrenadeEquiped = br.ReadBoolean();
		m_ItemInfo.IsShieldEquiped = br.ReadBoolean();
		m_ItemInfo.IsSlot1Equiped = br.ReadBoolean();
		m_ItemInfo.IsSlot2Equiped = br.ReadBoolean();
		m_ItemInfo.IsSlot3Equiped = br.ReadBoolean();
		m_ItemInfo.IsSlot4Equiped = br.ReadBoolean();
		int num = br.ReadInt32();
		if (m_ItemInfo.IsWeapon1Equiped)
		{
			int id = br.ReadInt32();
			NGUIBaseItem baseItem = new NGUIBaseItem();
			LoadItem(ref baseItem, br);
			m_ItemInfo.Weapon1 = new NGUIGameItem(id, baseItem);
		}
		if (m_ItemInfo.IsWeapon2Equiped)
		{
			int id2 = br.ReadInt32();
			NGUIBaseItem baseItem2 = new NGUIBaseItem();
			LoadItem(ref baseItem2, br);
			m_ItemInfo.Weapon2 = new NGUIGameItem(id2, baseItem2);
		}
		if (m_ItemInfo.IsWeapon3Enable && m_ItemInfo.IsWeapon3Equiped)
		{
			int id3 = br.ReadInt32();
			NGUIBaseItem baseItem3 = new NGUIBaseItem();
			LoadItem(ref baseItem3, br);
			m_ItemInfo.Weapon3 = new NGUIGameItem(id3, baseItem3);
		}
		if (m_ItemInfo.IsWeapon4Enable && m_ItemInfo.IsWeapon4Equiped)
		{
			int id4 = br.ReadInt32();
			NGUIBaseItem baseItem4 = new NGUIBaseItem();
			LoadItem(ref baseItem4, br);
			m_ItemInfo.Weapon4 = new NGUIGameItem(id4, baseItem4);
		}
		if (m_ItemInfo.IsHandGrenadeEquiped)
		{
			int id5 = br.ReadInt32();
			NGUIBaseItem baseItem5 = new NGUIBaseItem();
			LoadItem(ref baseItem5, br);
			m_ItemInfo.HandGrenade = new NGUIGameItem(id5, baseItem5);
		}
		if (m_ItemInfo.IsShieldEquiped)
		{
			int id6 = br.ReadInt32();
			NGUIBaseItem baseItem6 = new NGUIBaseItem();
			LoadItem(ref baseItem6, br);
			m_ItemInfo.Shield = new NGUIGameItem(id6, baseItem6);
		}
		if (m_ItemInfo.IsSlot1Equiped)
		{
			int id7 = br.ReadInt32();
			NGUIBaseItem baseItem7 = new NGUIBaseItem();
			LoadItem(ref baseItem7, br);
			m_ItemInfo.Slot1 = new NGUIGameItem(id7, baseItem7);
		}
		if (m_ItemInfo.IsSlot2Enable && m_ItemInfo.IsSlot2Equiped)
		{
			int id8 = br.ReadInt32();
			NGUIBaseItem baseItem8 = new NGUIBaseItem();
			LoadItem(ref baseItem8, br);
			m_ItemInfo.Slot2 = new NGUIGameItem(id8, baseItem8);
		}
		if (m_ItemInfo.IsSlot3Enable && m_ItemInfo.IsSlot3Equiped)
		{
			int id9 = br.ReadInt32();
			NGUIBaseItem baseItem9 = new NGUIBaseItem();
			LoadItem(ref baseItem9, br);
			m_ItemInfo.Slot3 = new NGUIGameItem(id9, baseItem9);
		}
		if (m_ItemInfo.IsSlot4Enable && m_ItemInfo.IsSlot4Equiped)
		{
			int id10 = br.ReadInt32();
			NGUIBaseItem baseItem10 = new NGUIBaseItem();
			LoadItem(ref baseItem10, br);
			m_ItemInfo.Slot4 = new NGUIGameItem(id10, baseItem10);
		}
		m_ItemInfo.BackPackItems.Clear();
		for (int i = 0; i < m_ItemInfo.BackpackSlotCount; i++)
		{
			int num2 = br.ReadInt32();
			if (num2 == -1)
			{
				m_ItemInfo.BackPackItems.Add(null);
				continue;
			}
			NGUIBaseItem baseItem11 = new NGUIBaseItem();
			LoadItem(ref baseItem11, br);
			NGUIGameItem item = new NGUIGameItem(num2, baseItem11);
			m_ItemInfo.BackPackItems.Add(item);
		}
		m_ItemInfo.StoryItems.Clear();
		for (int j = 0; j < num; j++)
		{
			int num3 = br.ReadInt32();
			NGUIBaseItem baseItem12 = new NGUIBaseItem();
			LoadItem(ref baseItem12, br);
			NGUIGameItem item2 = new NGUIGameItem(num3, baseItem12);
			int count = br.ReadInt32();
			StoryItem storyItem = new StoryItem();
			storyItem.Item = item2;
			storyItem.Count = count;
			m_ItemInfo.StoryItems.Add((short)num3, storyItem);
		}
		m_ItemInfo.CurrentEquipWeaponSlot = br.ReadByte();
		m_ItemInfo.IsFirstTimeToRefreshShop = br.ReadBoolean();
		m_ItemInfo.TimeToShopRefresh = br.ReadSingle();
		BlackMarketIcon.BlackMarketTimeToReady = br.ReadSingle();
		BlackMarketIcon.BlackMarketCD = br.ReadSingle();
		int num4 = br.ReadInt32();
		m_ItemInfo.Shop_WeaponList.Clear();
		for (int k = 0; k < num4; k++)
		{
			int num5 = br.ReadInt32();
			if (num5 == -1)
			{
				m_ItemInfo.Shop_WeaponList.Add(null);
				continue;
			}
			NGUIBaseItem baseItem13 = new NGUIBaseItem();
			LoadItem(ref baseItem13, br);
			NGUIGameItem item3 = new NGUIGameItem(num5, baseItem13);
			m_ItemInfo.Shop_WeaponList.Add(item3);
		}
		int num6 = br.ReadInt32();
		m_ItemInfo.Shop_ShieldList.Clear();
		for (int l = 0; l < num6; l++)
		{
			int num7 = br.ReadInt32();
			if (num7 == -1)
			{
				m_ItemInfo.Shop_ShieldList.Add(null);
				continue;
			}
			NGUIBaseItem baseItem14 = new NGUIBaseItem();
			LoadItem(ref baseItem14, br);
			NGUIGameItem item4 = new NGUIGameItem(num7, baseItem14);
			m_ItemInfo.Shop_ShieldList.Add(item4);
		}
		int num8 = br.ReadInt32();
		m_ItemInfo.Shop_ChipList.Clear();
		for (int m = 0; m < num8; m++)
		{
			int num9 = br.ReadInt32();
			if (num9 == -1)
			{
				m_ItemInfo.Shop_ChipList.Add(null);
				continue;
			}
			NGUIBaseItem baseItem15 = new NGUIBaseItem();
			LoadItem(ref baseItem15, br);
			NGUIGameItem item5 = new NGUIGameItem(num9, baseItem15);
			m_ItemInfo.Shop_ChipList.Add(item5);
		}
		int num10 = br.ReadInt32();
		m_ItemInfo.Shop_PillList.Clear();
		for (int n = 0; n < num10; n++)
		{
			int num11 = br.ReadInt32();
			if (num11 == -1)
			{
				m_ItemInfo.Shop_PillList.Add(null);
				continue;
			}
			NGUIBaseItem baseItem16 = new NGUIBaseItem();
			LoadItem(ref baseItem16, br);
			NGUIGameItem item6 = new NGUIGameItem(num11, baseItem16);
			m_ItemInfo.Shop_PillList.Add(item6);
		}
		int num12 = br.ReadInt32();
		if (num12 == -1)
		{
			m_ItemInfo.Shop_LimitSell = null;
			return;
		}
		NGUIBaseItem baseItem17 = new NGUIBaseItem();
		LoadItem(ref baseItem17, br);
		NGUIGameItem shop_LimitSell = new NGUIGameItem(num12, baseItem17);
		m_ItemInfo.Shop_LimitSell = shop_LimitSell;
	}

	protected void SaveItem(NGUIBaseItem baseItem, BinaryWriter bw)
	{
		bw.Write((byte)baseItem.Quality);
		bw.Write(baseItem.name);
		bw.Write(baseItem.iconName);
		bw.Write(baseItem.previewIconName);
		bw.Write((int)baseItem.equipmentSlot);
		bw.Write((byte)baseItem.ItemClass);
		bw.Write(baseItem.ItemLevel);
		bw.Write((byte)baseItem.Company);
		int count = baseItem.itemStats.Count;
		bw.Write(count);
		for (int i = 0; i < count; i++)
		{
			bw.Write((int)baseItem.itemStats[i].statType);
			bw.Write(baseItem.itemStats[i].statValue);
		}
		bw.Write(baseItem.skills.Count);
		for (int j = 0; j < baseItem.skills.Count; j++)
		{
			bw.Write(baseItem.skills[j].description);
			bw.Write(baseItem.skills[j].skillStats.Count);
			for (int k = 0; k < baseItem.skills[j].skillStats.Count; k++)
			{
				bw.Write((int)baseItem.skills[j].skillStats[k].statType);
				bw.Write(baseItem.skills[j].skillStats[k].statValue);
			}
		}
		bw.Write(baseItem.skillIDs.Count);
		for (int l = 0; l < baseItem.skillIDs.Count; l++)
		{
			bw.Write(baseItem.skillIDs[l]);
		}
		bw.Write(baseItem.GetPrice());
	}

	protected void LoadItem(ref NGUIBaseItem baseItem, BinaryReader br)
	{
		byte quality = br.ReadByte();
		baseItem.Quality = (ItemQuality)quality;
		string name = br.ReadString();
		string iconName = br.ReadString();
		string previewIconName = br.ReadString();
		int equipmentSlot = br.ReadInt32();
		byte itemClass = br.ReadByte();
		byte itemLevel = br.ReadByte();
		byte company = br.ReadByte();
		baseItem.name = name;
		baseItem.iconName = iconName;
		baseItem.previewIconName = previewIconName;
		baseItem.equipmentSlot = (NGUIBaseItem.EquipmentSlot)equipmentSlot;
		baseItem.ItemClass = (ItemClasses)itemClass;
		baseItem.ItemLevel = itemLevel;
		baseItem.Company = (ItemCompanyName)company;
		baseItem.iconAtlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			int statType = br.ReadInt32();
			float statValue = br.ReadSingle();
			NGUIItemStat nGUIItemStat = new NGUIItemStat();
			nGUIItemStat.statType = (NGUIItemStat.StatType)statType;
			nGUIItemStat.statValue = statValue;
			baseItem.itemStats.Add(nGUIItemStat);
		}
		int num2 = br.ReadInt32();
		baseItem.skills.Clear();
		for (int j = 0; j < num2; j++)
		{
			string description = br.ReadString();
			baseItem.skills[j].description = description;
			int num3 = br.ReadInt32();
			baseItem.skills[j].skillStats.Clear();
			for (int k = 0; k < num3; k++)
			{
				int statType2 = br.ReadInt32();
				float statValue2 = br.ReadSingle();
				NGUIItemStat nGUIItemStat2 = new NGUIItemStat();
				nGUIItemStat2.statType = (NGUIItemStat.StatType)statType2;
				nGUIItemStat2.statValue = statValue2;
				baseItem.skills[j].skillStats.Add(nGUIItemStat2);
			}
		}
		int num4 = br.ReadInt32();
		baseItem.skillIDs.Clear();
		for (int l = 0; l < num4; l++)
		{
			short item = br.ReadInt16();
			baseItem.skillIDs.Add(item);
		}
		int price = br.ReadInt32();
		baseItem.SetPrice(price);
	}
}
