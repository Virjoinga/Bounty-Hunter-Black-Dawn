using System.Collections.Generic;
using UnityEngine;

public class ItemStatsScript : MonoBehaviour
{
	public GameObject StatsTemplate;

	public virtual void SetObserveItem(NGUIBaseItem baseItem)
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
			Weapon weapon = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetWeapon();
			bool flag = false;
			if (baseItem.itemStats[11].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, " X 7", b++, false);
				}
				float value = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.FireDotDamage[(int)baseItem.itemStats[11].statValue]);
				float equipedValue = Mathf.FloorToInt(weapon.DamageInit * ElementWeaponConfig.FireDotDamage[(int)weapon.ElementFirePara]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRE_DAMAGE"), value, equipedValue, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[12].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, " X 7", b++, false);
				}
				float value2 = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.ShockDotDamage[(int)baseItem.itemStats[12].statValue]);
				float equipedValue2 = Mathf.FloorToInt(weapon.DamageInit * ElementWeaponConfig.ShockDotDamage[(int)weapon.ElementShockPara]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHOCK_DAMAGE"), value2, equipedValue2, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[13].statValue > 0f)
			{
				flag = true;
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, " X 7", b++, false);
				}
				float value3 = Mathf.FloorToInt(baseItem.itemStats[0].statValue * ElementWeaponConfig.CorrosiveDotDamage[(int)baseItem.itemStats[13].statValue]);
				float equipedValue3 = Mathf.FloorToInt(weapon.DamageInit * ElementWeaponConfig.CorrosiveDotDamage[(int)weapon.ElementCorrosivePara]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CORRO_DAMAGE"), value3, equipedValue3, false, string.Empty, b++, false);
			}
			if (!flag)
			{
				if (baseItem.ItemClass != ItemClasses.Shotgun)
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, string.Empty, b++, false);
				}
				else
				{
					AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Floor(baseItem.itemStats[0].statValue), Mathf.Floor(weapon.DamageInit), false, " X 7", b++, false);
				}
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_ACCURACY"), baseItem.itemStats[1].statValue, weapon.AccuracyInit, false, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRERATE"), 1f / baseItem.itemStats[2].statValue, 1f / weapon.AttackFrenquencyInit, false, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RELOAD_SPEED"), baseItem.itemStats[3].statValue, weapon.ReloadTime, true, string.Empty, b++, true);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RECOIL"), baseItem.itemStats[5].statValue, weapon.RecoilInit, true, string.Empty, b++, false);
			if (baseItem.ItemClass == ItemClasses.RPG)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RANGE"), baseItem.itemStats[10].statValue, weapon.ExplosionRangeInit, false, string.Empty, b++, false);
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_CHANCE"), Mathf.Ceil(baseItem.itemStats[7].statValue * 100f), Mathf.Ceil(weapon.CriticalRateInit * 100f), false, "%", b++, false);
			if (baseItem.itemStats[8].statValue > 1.5f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_DAMAGE"), Mathf.Ceil(baseItem.itemStats[8].statValue * 100f - 150f), Mathf.Ceil(weapon.CriticalDamageInit * 100f - 150f), false, "%", b++, false);
			}
			if (baseItem.itemStats[6].statValue > 1f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_MELEE_DAMAGE"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f - 100f), Mathf.Ceil(weapon.MeleeDamageInit * 100f - 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[14].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_WEAPON_ZOOM"), (baseItem.itemStats[14].statValue + 100f) / 100f, (weapon.ScopeRate + 100f) / 100f, false, string.Empty, b++, false);
			}
		}
		else if (baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG)
		{
			float num = 0f;
			float equipedValue4 = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 1f;
			float num8 = 0f;
			float equipedValue5 = 0f;
			GrenadeInfo handGrenade = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.HandGrenade;
			if (handGrenade != null)
			{
				num = handGrenade.DamageInit;
				equipedValue4 = handGrenade.ExplosionRangeInit;
				switch (handGrenade.elementType)
				{
				case ElementType.Fire:
					num2 = (int)handGrenade.elementPara;
					break;
				case ElementType.Shock:
					num3 = (int)handGrenade.elementPara;
					break;
				case ElementType.Corrosive:
					num4 = (int)handGrenade.elementPara;
					break;
				}
				num5 = handGrenade.CriticalRateInit;
				num6 = handGrenade.CriticalDamageInit;
				NGUIBaseItem baseItem2 = GameApp.GetInstance().GetUserState().ItemInfoData.HandGrenade.baseItem;
				num7 = baseItem2.itemStats[6].statValue;
				num8 = baseItem2.itemStats[16].statValue;
				equipedValue5 = baseItem2.itemStats[17].statValue;
			}
			bool flag2 = false;
			if (baseItem.itemStats[11].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value4 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.FireDotDamage[(int)baseItem.itemStats[11].statValue]);
				float equipedValue6 = Mathf.CeilToInt(num * ElementWeaponConfig.FireDotDamage[(int)num2]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_FIRE_DAMAGE"), value4, equipedValue6, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[12].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value5 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.ShockDotDamage[(int)baseItem.itemStats[12].statValue]);
				float equipedValue7 = Mathf.CeilToInt(num * ElementWeaponConfig.ShockDotDamage[(int)num3]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHOCK_DAMAGE"), value5, equipedValue7, false, string.Empty, b++, false);
			}
			if (baseItem.itemStats[13].statValue > 0f)
			{
				flag2 = true;
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
				float value6 = Mathf.CeilToInt(Mathf.Ceil(baseItem.itemStats[0].statValue) * ElementWeaponConfig.CorrosiveDotDamage[(int)baseItem.itemStats[13].statValue]);
				float equipedValue8 = Mathf.CeilToInt(num * ElementWeaponConfig.CorrosiveDotDamage[(int)num4]);
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CORRO_DAMAGE"), value6, equipedValue8, false, string.Empty, b++, false);
			}
			if (!flag2)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_DAMAGE"), Mathf.Ceil(baseItem.itemStats[0].statValue), Mathf.Floor(num), false, string.Empty, b++, false);
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_CHANCE"), Mathf.Ceil(baseItem.itemStats[7].statValue * 100f), Mathf.Ceil(num5 * 100f), false, "%", b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_RANGE"), baseItem.itemStats[10].statValue, equipedValue4, false, string.Empty, b++, false);
			if (baseItem.itemStats[8].statValue > 1.5f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_CRIT_DAMAGE"), Mathf.Ceil(baseItem.itemStats[8].statValue * 100f - 150f), Mathf.Ceil(num6 * 100f - 150f), false, "%", b++, false);
			}
			if (baseItem.itemStats[6].statValue > 1f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_MELEE_DAMAGE"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f - 100f), Mathf.Ceil(num7 * 100f - 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[16].statValue != 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_BONUS"), Mathf.Ceil(baseItem.itemStats[16].statValue * 100f), Mathf.Ceil(num8 * 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[17].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_RECOVERY"), baseItem.itemStats[17].statValue, equipedValue5, false, string.Empty, b++, false);
			}
		}
		else if (baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Shield)
		{
			float equipedValue9 = 0f;
			float equipedValue10 = 0f;
			float equipedValue11 = 0f;
			float equipedValue12 = 0f;
			float equipedValue13 = 0f;
			float equipedValue14 = 0f;
			float num9 = 0f;
			float equipedValue15 = 0f;
			if (GameApp.GetInstance().GetUserState().ItemInfoData.IsShieldEquiped && GameApp.GetInstance().GetUserState().ItemInfoData.Shield != null)
			{
				NGUIBaseItem baseItem3 = GameApp.GetInstance().GetUserState().ItemInfoData.Shield.baseItem;
				equipedValue9 = baseItem3.itemStats[0].statValue;
				equipedValue10 = baseItem3.itemStats[1].statValue;
				equipedValue11 = baseItem3.itemStats[2].statValue;
				equipedValue12 = baseItem3.itemStats[3].statValue;
				equipedValue13 = baseItem3.itemStats[4].statValue;
				equipedValue14 = baseItem3.itemStats[5].statValue;
				num9 = baseItem3.itemStats[6].statValue;
				equipedValue15 = baseItem3.itemStats[7].statValue;
			}
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_CAP"), baseItem.itemStats[0].statValue, equipedValue9, false, string.Empty, b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_RECOVERY_SPEED"), baseItem.itemStats[1].statValue, equipedValue10, false, string.Empty, b++, false);
			AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_RECOVERY_DELAY"), baseItem.itemStats[2].statValue, equipedValue11, true, string.Empty, b++, true);
			if (baseItem.itemStats[3].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_FIRE_RESIST"), baseItem.itemStats[3].statValue, equipedValue12, false, "%", b++, false);
			}
			if (baseItem.itemStats[4].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_SHOCK_RESIST"), baseItem.itemStats[4].statValue, equipedValue13, false, "%", b++, false);
			}
			if (baseItem.itemStats[5].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_CORRO_RESIST"), baseItem.itemStats[5].statValue, equipedValue14, false, "%", b++, false);
			}
			if (baseItem.itemStats[6].statValue != 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_BONUS"), Mathf.Ceil(baseItem.itemStats[6].statValue * 100f), Mathf.Ceil(num9 * 100f), false, "%", b++, false);
			}
			if (baseItem.itemStats[7].statValue > 0f)
			{
				AddStat(LocalizationManager.GetInstance().GetString("LOC_ITEM_PARRA_SHIELD_HP_RECOVERY"), baseItem.itemStats[7].statValue, equipedValue15, false, string.Empty, b++, false);
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

	public void AddStat(string statName, float value, float equipedValue, bool revert, string suffix, byte sortNumber, bool needKeepDemical)
	{
		GameObject gameObject = Object.Instantiate(StatsTemplate) as GameObject;
		NGUITools.SetActive(gameObject, true);
		string text = sortNumber.ToString();
		if (sortNumber < 10)
		{
			text = "0" + sortNumber;
		}
		gameObject.name = "ItemStat_" + text;
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		ItemStatsContentScript component = gameObject.GetComponent<ItemStatsContentScript>();
		if (component.StatLabel != null)
		{
			component.StatLabel.text = statName;
		}
		if (component.ValueLabel != null)
		{
			component.ValueLabel.text = CompareValue(value, equipedValue, revert, needKeepDemical, suffix);
		}
	}

	protected virtual string CompareValue(float newValue, float equipedValue, bool revert, bool needKeepDemical, string suffix)
	{
		string empty = string.Empty;
		if (needKeepDemical)
		{
			int num = (int)(newValue * 100f);
			newValue = (float)num / 100f;
			num = (int)(equipedValue * 100f);
			equipedValue = (float)num / 100f;
		}
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
}
