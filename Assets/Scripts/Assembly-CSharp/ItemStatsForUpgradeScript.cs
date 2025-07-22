using System.Collections.Generic;
using UnityEngine;

public class ItemStatsForUpgradeScript : ItemStatsScript
{
	public ItemDescriptionScript CompareBase;

	public override void SetObserveItem(NGUIBaseItem baseItem)
	{
		int childCount = base.transform.GetChildCount();
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < childCount; i++)
		{
			list.Add(base.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < list.Count; j++)
		{
			Object.Destroy(list[j]);
		}
		if (baseItem == null || !(StatsTemplate != null) || !(StatsTemplate.GetComponent<ItemStatsContentScript>() != null))
		{
			return;
		}
		byte b = 0;
		if (baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon)
		{
			NGUIBaseItem nGUIBaseItem = ((!(CompareBase != null)) ? baseItem : CompareBase.GetObjserveItem());
			bool flag = false;
			if (baseItem.itemStats[11].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, " X 7", b++, false);
				}
				float value = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.FireDotDamage[(int)baseItem.itemStats[11].statValue]);
				float equipedValue = Mathf.FloorToInt(nGUIBaseItem.itemStats[0].statValue * ElementWeaponConfig.FireDotDamage[(int)nGUIBaseItem.itemStats[11].statValue]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRE_DAMAGE"), value, equipedValue, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[12].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, " X 7", b++, false);
				}
				float value2 = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.ShockDotDamage[(int)baseItem.itemStats[12].statValue]);
				float equipedValue2 = Mathf.FloorToInt(nGUIBaseItem.itemStats[0].statValue * ElementWeaponConfig.ShockDotDamage[(int)nGUIBaseItem.itemStats[12].statValue]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHOCK_DAMAGE"), value2, equipedValue2, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[13].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, " X 7", b++, false);
				}
				float value3 = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.CorrosiveDotDamage[(int)baseItem.itemStats[13].statValue]);
				float equipedValue3 = Mathf.FloorToInt(nGUIBaseItem.itemStats[0].statValue * ElementWeaponConfig.CorrosiveDotDamage[(int)nGUIBaseItem.itemStats[13].statValue]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CORRO_DAMAGE"), value3, equipedValue3, false, string.Empty, b++, false);
			}
			if (!flag)
			{
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(nGUIBaseItem.itemStats[0].statValue), false, " X 7", b++, false);
				}
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_ACCURACY"), baseItem.itemStats[1].statValue, nGUIBaseItem.itemStats[1].statValue, false, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRERATE"), 1f / baseItem.itemStats[2].statValue, 1f / nGUIBaseItem.itemStats[2].statValue, false, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RELOAD_SPEED"), baseItem.itemStats[3].statValue, nGUIBaseItem.itemStats[3].statValue, true, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RECOIL"), baseItem.itemStats[5].statValue, nGUIBaseItem.itemStats[5].statValue, true, string.Empty, b++, false);
			if (baseItem.ItemClass == ItemClasses.RPG)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RANGE"), baseItem.itemStats[10].statValue, nGUIBaseItem.itemStats[10].statValue, false, string.Empty, b++, false);
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_CHANCE"), Mathf.Ceil(baseItem.itemStats[7].statValue * 100f), Mathf.Ceil(nGUIBaseItem.itemStats[7].statValue * 100f), false, "%", b++, false);
			if (baseItem.itemStats[8].statValue > 1.5f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_DAMAGE"), Mathf.Ceil(baseItem.itemStats[8].statValue * 100f - 150f), Mathf.Ceil(nGUIBaseItem.itemStats[8].statValue * 100f - 150f), false, "%", b++, false);
			}
			Debug.Log("baseItem.itemStats[6].statValue: " + baseItem.itemStats[6].statValue);
			if (baseItem.itemStats[6].statValue > 1f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_MELEE_DAMAGE"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f - 100f), Mathf.Ceil(nGUIBaseItem.itemStats[6].statValue * 100f - 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[14].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_WEAPON_ZOOM"), (baseItem.itemStats[14].statValue + 100f) / 100f, (nGUIBaseItem.itemStats[14].statValue + 100f) / 100f, false, string.Empty, b++, false);
			}
		}
		else if (baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 1f;
			float num9 = 0f;
			float num10 = 0f;
			NGUIBaseItem nGUIBaseItem2 = ((!(CompareBase != null)) ? baseItem : CompareBase.GetObjserveItem());
			num = nGUIBaseItem2.itemStats[0].statValue;
			num2 = nGUIBaseItem2.itemStats[10].statValue;
			num3 = nGUIBaseItem2.itemStats[11].statValue;
			num4 = nGUIBaseItem2.itemStats[12].statValue;
			num5 = nGUIBaseItem2.itemStats[13].statValue;
			num6 = nGUIBaseItem2.itemStats[7].statValue;
			num7 = nGUIBaseItem2.itemStats[8].statValue;
			num8 = nGUIBaseItem2.itemStats[6].statValue;
			num9 = nGUIBaseItem2.itemStats[16].statValue;
			num10 = nGUIBaseItem2.itemStats[17].statValue;
			bool flag2 = false;
			if (baseItem.itemStats[11].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value4 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.FireDotDamage[(int)baseItem.itemStats[11].statValue]);
				float equipedValue4 = Mathf.CeilToInt(num * ElementWeaponConfig.FireDotDamage[(int)num3]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRE_DAMAGE"), value4, equipedValue4, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[12].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value5 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.ShockDotDamage[(int)baseItem.itemStats[12].statValue]);
				float equipedValue5 = Mathf.CeilToInt(num * ElementWeaponConfig.ShockDotDamage[(int)num4]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHOCK_DAMAGE"), value5, equipedValue5, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[13].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value6 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.CorrosiveDotDamage[(int)baseItem.itemStats[13].statValue]);
				float equipedValue6 = Mathf.CeilToInt(num * ElementWeaponConfig.CorrosiveDotDamage[(int)num5]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CORRO_DAMAGE"), value6, equipedValue6, false, string.Empty, b++, false);
			}
			if (!flag2)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_CHANCE"), Mathf.Ceil(baseItem.itemStats[7].statValue * 100f), Mathf.Ceil(num6 * 100f), false, "%", b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RANGE"), baseItem.itemStats[10].statValue, num2, false, string.Empty, b++, false);
			if (baseItem.itemStats[8].statValue > 1.5f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_DAMAGE"), Mathf.Ceil(baseItem.itemStats[8].statValue * 100f - 150f), Mathf.Ceil(num7 * 100f - 150f), false, "%", b++, false);
			}
			if (baseItem.itemStats[6].statValue > 1f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_MELEE_DAMAGE"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f - 100f), Mathf.Ceil(num8 * 100f - 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[16].statValue != 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_BONUS"), Mathf.Ceil(baseItem.itemStats[16].statValue * 100f), Mathf.Ceil(num9 * 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[17].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_RECOVERY"), baseItem.itemStats[17].statValue, num10, false, string.Empty, b++, false);
			}
		}
		else if (baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Shield)
		{
			float num11 = 0f;
			float num12 = 0f;
			float num13 = 0f;
			float num14 = 0f;
			float num15 = 0f;
			float num16 = 0f;
			float num17 = 0f;
			float num18 = 0f;
			NGUIBaseItem nGUIBaseItem3 = ((!(CompareBase != null)) ? baseItem : CompareBase.GetObjserveItem());
			num11 = nGUIBaseItem3.itemStats[0].statValue;
			num12 = nGUIBaseItem3.itemStats[1].statValue;
			num13 = nGUIBaseItem3.itemStats[2].statValue;
			num14 = nGUIBaseItem3.itemStats[3].statValue;
			num15 = nGUIBaseItem3.itemStats[4].statValue;
			num16 = nGUIBaseItem3.itemStats[5].statValue;
			num17 = nGUIBaseItem3.itemStats[6].statValue;
			num18 = nGUIBaseItem3.itemStats[7].statValue;
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_CAP"), baseItem.itemStats[0].statValue, num11, false, string.Empty, b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_RECOVERY_SPEED"), baseItem.itemStats[1].statValue, num12, false, string.Empty, b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_RECOVERY_DELAY"), baseItem.itemStats[2].statValue, num13, true, string.Empty, b++, true);
			if (baseItem.itemStats[3].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_FIRE_RESIST"), baseItem.itemStats[3].statValue, num14, false, "%", b++, false);
			}
			if (baseItem.itemStats[4].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_SHOCK_RESIST"), baseItem.itemStats[4].statValue, num15, false, "%", b++, false);
			}
			if (baseItem.itemStats[5].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_CORRO_RESIST"), baseItem.itemStats[5].statValue, num16, false, "%", b++, false);
			}
			if (baseItem.itemStats[6].statValue != 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_BONUS"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f), Mathf.Ceil(num17 * 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[7].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_RECOVERY"), baseItem.itemStats[7].statValue, num18, false, string.Empty, b++, false);
			}
		}
		UIGridX component = base.gameObject.GetComponent<UIGridX>();
		if (component != null)
		{
			component.sorted = true;
			component.repositionNow = true;
			component.m_UIDraggablePanelAlign.ResetPosition();
		}
	}

	protected override string CompareValue(float newValue, float equipedValue, bool revert, bool needKeepDemical, string suffix)
	{
		string empty = string.Empty;
		if (needKeepDemical)
		{
			int num = (int)(newValue * 100f);
			newValue = (float)num / 100f;
			num = (int)(equipedValue * 100f);
			equipedValue = (float)num / 100f;
		}
		if (CompareBase != null)
		{
			if (revert)
			{
				if (newValue > equipedValue)
				{
					return "[ff0000]" + newValue + suffix + "/down2[-]";
				}
				if (newValue < equipedValue)
				{
					return "[00ff00]" + newValue + suffix + "/up1[-]";
				}
				return "[ffffff]" + newValue + suffix + "/right[-]";
			}
			if (newValue < equipedValue)
			{
				return "[ff0000]" + newValue + suffix + "/down2[-]";
			}
			if (newValue > equipedValue)
			{
				return "[00ff00]" + newValue + suffix + "/up1[-]";
			}
			return "[ffffff]" + newValue + suffix + "/right[-]";
		}
		return newValue + suffix;
	}
}
