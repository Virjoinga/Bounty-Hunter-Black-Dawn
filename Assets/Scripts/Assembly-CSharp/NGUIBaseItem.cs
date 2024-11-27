using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NGUIBaseItem
{
	public enum EquipmentSlot
	{
		None = 0,
		Weapon = 1,
		WeaponG = 2,
		Shield = 3,
		SkillSlot = 4,
		Pill = 5,
		_LastDoNotUse = 6
	}

	public int[,] UpgradePrice = new int[5, 25]
	{
		{
			10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
			10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
			10, 10, 10, 10, 10
		},
		{
			10, 14, 20, 28, 30, 34, 38, 38, 38, 38,
			38, 38, 38, 38, 38, 38, 38, 38, 38, 38,
			38, 38, 38, 38, 38
		},
		{
			10, 14, 20, 28, 30, 34, 38, 38, 38, 38,
			38, 38, 38, 38, 38, 38, 38, 38, 38, 38,
			38, 38, 38, 38, 38
		},
		{
			10, 14, 20, 28, 30, 34, 38, 38, 38, 38,
			38, 38, 38, 38, 38, 38, 38, 38, 38, 38,
			38, 38, 38, 38, 38
		},
		{
			10, 12, 12, 14, 14, 16, 16, 18, 18, 20,
			20, 22, 22, 24, 24, 26, 26, 28, 28, 30,
			30, 32, 32, 34, 34
		}
	};

	public EquipmentSlot equipmentSlot;

	public List<NGUIItemStat> itemStats = new List<NGUIItemStat>();

	public List<NGUIItemSkill> skills = new List<NGUIItemSkill>();

	public List<short> skillIDs = new List<short>();

	public byte ItemLevel = 1;

	public byte UpgradeTimes;

	protected string mPrice;

	public ItemQuality Quality = ItemQuality.Common;

	public ItemCompanyName Company;

	public string LegendaryName = string.Empty;

	public string previewIconName = string.Empty;

	public int id16;

	public string name;

	public UIAtlas iconAtlas;

	public string iconName = string.Empty;

	public ItemClasses ItemClass { get; set; }

	public bool MithrilItem { get; set; }

	public string DisplayName
	{
		get
		{
			string text = string.Empty;
			if (ItemClass != ItemClasses.V_Slot)
			{
				foreach (short skillID in skillIDs)
				{
					text = text + LocalizationManager.GetInstance().GetString(GameConfig.GetInstance().equipPrefixConfig[skillID].Name) + " ";
				}
			}
			else
			{
				foreach (short skillID2 in skillIDs)
				{
					text = text + LocalizationManager.GetInstance().GetString(GameConfig.GetInstance().chipPrefixConfig[skillID2].Name) + " ";
				}
			}
			if (ItemClass == ItemClasses.StoryItem)
			{
				return text + LocalizationManager.GetInstance().GetString(name);
			}
			if (Quality == ItemQuality.Legendary && ItemClass != ItemClasses.V_Slot)
			{
				return text + LocalizationManager.GetInstance().GetString(LegendaryName);
			}
			return text + ItemBase.ItemClassName(ItemClass);
		}
	}

	public string Description
	{
		get
		{
			string text = string.Empty;
			if (ItemClass == ItemClasses.V_Slot)
			{
				if (itemStats[0].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_01") + " " + Mathf.RoundToInt(itemStats[0].statValue * 100f) + "%";
				}
				if (itemStats[1].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_02") + " " + Mathf.RoundToInt(itemStats[1].statValue * 100f) + "%";
				}
				if (itemStats[2].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_03") + " " + Mathf.RoundToInt(itemStats[2].statValue * 100f) + "%";
				}
				if (itemStats[3].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_04") + " " + Mathf.RoundToInt(itemStats[3].statValue * 100f) + "%";
				}
				if (itemStats[4].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_05") + " " + Mathf.RoundToInt(itemStats[4].statValue * 100f) + "%";
				}
				if (itemStats[5].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_06") + " " + Mathf.RoundToInt(itemStats[5].statValue * 100f) + "%";
				}
				if (itemStats[6].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_07") + " " + Mathf.RoundToInt(itemStats[6].statValue) + "%";
				}
				if (itemStats[7].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_08") + " " + Mathf.RoundToInt(itemStats[7].statValue * 100f) + "%";
				}
				if (itemStats[8].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_09") + " " + Mathf.RoundToInt(itemStats[8].statValue * 100f) + "%";
				}
				if (itemStats[9].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_10") + " " + Mathf.RoundToInt(itemStats[9].statValue * 100f) + "%";
				}
				if (itemStats[10].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_11") + " " + Mathf.RoundToInt(itemStats[10].statValue * 100f) + "%";
				}
				if (itemStats[11].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_12") + " " + Mathf.RoundToInt(itemStats[11].statValue * 100f) + "%";
				}
				if (itemStats[12].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_13") + " " + Mathf.RoundToInt(itemStats[12].statValue * 100f) + "%";
				}
				if (itemStats[13].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_14") + " " + Mathf.RoundToInt(itemStats[13].statValue) + "%";
				}
				if (itemStats[14].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_15") + " " + Mathf.RoundToInt(itemStats[14].statValue) + "%";
				}
				if (itemStats[15].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_16") + " " + Mathf.RoundToInt(itemStats[15].statValue) + "%";
				}
				if (itemStats[16].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_17") + " " + Mathf.RoundToInt(itemStats[16].statValue * 100f) + "%";
				}
				if (itemStats[17].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_18") + " " + Mathf.RoundToInt(itemStats[17].statValue);
				}
				if (itemStats[18].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_19") + " " + Mathf.RoundToInt(itemStats[18].statValue * 100f) + "%";
				}
				if (itemStats[19].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_20") + " " + Mathf.RoundToInt(itemStats[19].statValue) + "%";
				}
				if (itemStats[20].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_21") + " " + Mathf.RoundToInt(itemStats[20].statValue) + "%";
				}
				if (itemStats[21].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_22") + " " + Mathf.RoundToInt(itemStats[21].statValue) + "%";
				}
				if (itemStats[22].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_23") + " " + Mathf.RoundToInt(itemStats[22].statValue) + "%";
				}
				if (itemStats[23].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_24") + " " + Mathf.RoundToInt(itemStats[23].statValue * 100f) + "%";
				}
				if (itemStats[24].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_25") + " " + Mathf.RoundToInt(itemStats[24].statValue * 100f) + "%";
				}
				if (itemStats[25].statValue != 0f)
				{
					string text2 = text;
					text = text2 + LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CHIP_26") + " " + Mathf.RoundToInt(itemStats[25].statValue * 100f) + "%";
				}
			}
			else if (ItemClass == ItemClasses.W_Pills)
			{
				text = LocalizationManager.GetInstance().GetString("MENU_DROP_ITEM_PILL_RECOVERY") + " [00ff00]" + itemStats[0].statValue + " HP[-]";
			}
			return text;
		}
	}

	public int MithrilPrice
	{
		get
		{
			if (ItemClass == ItemClasses.V_Slot)
			{
				switch (Quality)
				{
				case ItemQuality.Common:
					return Mathf.CeilToInt(100f * (1f - (float)MithrilOff / 100f));
				case ItemQuality.Uncommon:
					return Mathf.CeilToInt(400f * (1f - (float)MithrilOff / 100f));
				case ItemQuality.Rare:
					return Mathf.CeilToInt(400f * (1f - (float)MithrilOff / 100f));
				case ItemQuality.Epic:
					return Mathf.CeilToInt(900f * (1f - (float)MithrilOff / 100f));
				case ItemQuality.Legendary:
					return Mathf.CeilToInt(1600f * (1f - (float)MithrilOff / 100f));
				}
			}
			return Mathf.CeilToInt((float)GetPrice() * (1f - (float)MithrilOff / 100f) / (float)(GameApp.GetInstance().GetUserState().GetCharLevel() * 15));
		}
	}

	public int MithrilOff { get; set; }

	public NGUIBaseItem()
	{
		MithrilItem = false;
		MithrilOff = 0;
		SetPrice(0);
	}

	public Color GetBackGroundColorByQuality()
	{
		switch (Quality)
		{
		case ItemQuality.Uncommon:
			return new Color(0.16f, 0.93f, 0.45f, 0.75f);
		case ItemQuality.Rare:
			return new Color(0.16f, 0.72f, 0.93f, 0.75f);
		case ItemQuality.Epic:
			return new Color(0.78f, 0.16f, 0.93f, 0.75f);
		case ItemQuality.Legendary:
			return new Color(0.91f, 0.47f, 0.16f, 0.75f);
		default:
			return new Color(1f, 1f, 1f, 0.75f);
		}
	}

	public string GetBackGroundColorStringByQuality()
	{
		switch (Quality)
		{
		case ItemQuality.Uncommon:
			return "LVB";
		case ItemQuality.Rare:
			return "LVC";
		case ItemQuality.Epic:
			return "LVD";
		case ItemQuality.Legendary:
			return "LVE";
		default:
			return "LVA";
		}
	}

	public string GetDescBackGroundColorStringByQuality()
	{
		switch (Quality)
		{
		case ItemQuality.Uncommon:
			return "LVB1";
		case ItemQuality.Rare:
			return "LVC1";
		case ItemQuality.Epic:
			return "LVD1";
		case ItemQuality.Legendary:
			return "LVE1";
		default:
			return "LVA1";
		}
	}

	public string GetStringColorPrefixByQuality()
	{
		switch (Quality)
		{
		case ItemQuality.Uncommon:
			return "[29ee73]";
		case ItemQuality.Rare:
			return "[2ab8ef]";
		case ItemQuality.Epic:
			return "[c527eb]";
		case ItemQuality.Legendary:
			return "[eb7a29]";
		default:
			return "[FFFFFF]";
		}
	}

	public int GetPrice()
	{
		return AntiCracking.DecryptBufferStr(mPrice, "jiage");
	}

	public int GetUpgradePrice()
	{
		return GetPrice() * UpgradePrice[(int)(Quality - 1), UpgradeTimes];
	}

	public void SetPrice(int _price)
	{
		mPrice = AntiCracking.CryptBufferStr(_price, "jiage");
	}

	public int GetMaxUpgradeCount()
	{
		int result = 0;
		if (equipmentSlot == EquipmentSlot.Weapon || equipmentSlot == EquipmentSlot.WeaponG || equipmentSlot == EquipmentSlot.Shield)
		{
			switch (Quality)
			{
			case ItemQuality.Common:
				result = 99;
				break;
			case ItemQuality.Uncommon:
				result = 6;
				break;
			case ItemQuality.Rare:
				result = 6;
				break;
			case ItemQuality.Epic:
				result = 6;
				break;
			case ItemQuality.Legendary:
				result = 99;
				break;
			}
		}
		return result;
	}
}
