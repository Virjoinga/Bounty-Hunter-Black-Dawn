using UnityEngine;

public class ItemWeapon : ItemBase
{
	private const int DPS_BASE_PARA_1 = 40;

	private const int DPS_BASE_PARA_2 = 20;

	private const short DEFAULT_CRITICAL_DAMAGE = 150;

	public static float[] DPS_TYPE_PARAs_PVE = new float[9] { 0f, 1f, 1.5f, 1.1f, 2.7f, 0.9f, 5f, 10f, 8f };

	public float DPH { get; set; }

	public ElementType Element { get; set; }

	public short BasicDamage { get; set; }

	public float Accurancy { get; set; }

	public float BasicAttackInterval { get; set; }

	public float BasicReloadTime { get; set; }

	public short BasicMags { get; set; }

	public short BasicRecoil { get; set; }

	public short BasicMeleeDamage { get; set; }

	public short BasicCriticalRate { get; set; }

	public short BasicCriticalDamage { get; set; }

	public short BasicHaveScope { get; set; }

	public short BasicScopeRate { get; set; }

	public short BasicExplosionRange { get; set; }

	public short ElementFirePara { get; set; }

	public short ElementShockPara { get; set; }

	public short ElementCorrosivePara { get; set; }

	public short BasicFadeRange { get; set; }

	public float BasicExplosionFadeRange { get; set; }

	public short MaxHpBonus { get; set; }

	public short HpRecovery { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		WeaponConfig weaponConfig = GameConfig.GetInstance().weaponConfig[base.ItemName];
		base.ItemCompany = weaponConfig.ItemCompany;
		base.LegendaryName = weaponConfig.LegendaryName;
		BasicDamage = weaponConfig.BasicDamage;
		Accurancy = (int)weaponConfig.Accurancy;
		BasicAttackInterval = weaponConfig.BasicAttackInterval;
		BasicReloadTime = weaponConfig.BasicReloadTime;
		BasicMags = weaponConfig.BasicMags;
		BasicRecoil = weaponConfig.BasicRecoil;
		BasicMeleeDamage = weaponConfig.BasicMeleeDamage;
		BasicCriticalRate = weaponConfig.BasicCriticalRate;
		BasicCriticalDamage = 150;
		BasicExplosionRange = weaponConfig.BasicExplosionRange;
		BasicHaveScope = weaponConfig.BasicHaveScope;
		BasicScopeRate = 0;
		BasicFadeRange = weaponConfig.BasicFadeRange;
		BasicExplosionFadeRange = (int)weaponConfig.BasicExplosionFadeRange;
		DPH = (float)(40 + base.ItemLevel * 20) * DPS_TYPE_PARAs_PVE[(int)base.ItemClass] * (float)BasicDamage * 0.01f;
		Element = ElementType.NoElement;
		ElementFirePara = 0;
		ElementShockPara = 0;
		ElementCorrosivePara = 0;
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
		base.generateEquipmentSkills();
		if (BasicHaveScope == 1)
		{
			int skillScore = Random.Range(1, 5) * 2 - 1;
			generateSkill(skillScore, false, true);
		}
	}

	protected override void OnTriggerEnter(Collider c)
	{
		base.OnTriggerEnter(c);
	}

	protected override void GetIntoBackPack()
	{
		NGUIGameItem nGUIGameItem = new NGUIGameItem(base.SpecialID, mNGUIBaseItem);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (base.ItemLevel <= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			if (!itemInfoData.IsHandGrenadeEquiped && base.ItemType == 1)
			{
				itemInfoData.HandGrenade = nGUIGameItem;
				itemInfoData.IsHandGrenadeEquiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				return;
			}
			if (!itemInfoData.IsWeapon1Equiped && base.ItemType != 1)
			{
				itemInfoData.Weapon1 = nGUIGameItem;
				itemInfoData.IsWeapon1Equiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				return;
			}
			if (!itemInfoData.IsWeapon2Equiped && base.ItemType != 1)
			{
				itemInfoData.Weapon2 = nGUIGameItem;
				itemInfoData.IsWeapon2Equiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				return;
			}
			if (itemInfoData.IsWeapon3Enable && !itemInfoData.IsWeapon3Equiped && base.ItemType != 1)
			{
				itemInfoData.Weapon3 = nGUIGameItem;
				itemInfoData.IsWeapon3Equiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				return;
			}
			if (itemInfoData.IsWeapon4Enable && !itemInfoData.IsWeapon4Equiped && base.ItemType != 1)
			{
				itemInfoData.Weapon4 = nGUIGameItem;
				itemInfoData.IsWeapon4Equiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshWeaponListFromItemInfo();
				return;
			}
			for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
			{
				if (itemInfoData.BackPackItems[i] == null)
				{
					itemInfoData.BackPackItems[i] = nGUIGameItem;
					break;
				}
			}
			return;
		}
		for (int j = 0; j < itemInfoData.BackpackSlotCount; j++)
		{
			if (itemInfoData.BackPackItems[j] == null)
			{
				itemInfoData.BackPackItems[j] = nGUIGameItem;
				break;
			}
		}
	}

	public override void generateNGUIBaseItem()
	{
		base.generateNGUIBaseItem();
		mNGUIBaseItem.name = base.ItemName;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		short num8 = 0;
		float num9 = 0f;
		foreach (SkillInfo skill in Skills)
		{
			num += (float)skill.Damage * 0.01f;
			num2 += (float)skill.Accurancy * 0.01f;
			num3 += (float)skill.AttackInterval * 0.01f;
			num4 += (float)skill.ReloadTime * 0.01f;
			num5 += (float)skill.Mags * 0.01f;
			num6 += (float)skill.Recoil * 0.01f;
			num7 += (float)skill.MeleeDamage * 0.01f;
			num8 += skill.CriticalRate;
			num9 += (float)skill.ExplosionRange * 0.01f;
			if (skill.FireElement != 0)
			{
				ElementFirePara = skill.FireElement;
			}
			if (skill.ShockElement != 0)
			{
				ElementShockPara = skill.ShockElement;
			}
			if (skill.CorrosiveElement != 0)
			{
				ElementCorrosivePara = skill.CorrosiveElement;
			}
			BasicCriticalDamage += skill.CriticalDamage;
			BasicScopeRate += skill.ScopeRate;
			MaxHpBonus += skill.MaxHp;
			HpRecovery += skill.HpRecovery;
		}
		DPH *= 1f + num;
		Accurancy = 100f - (100f - Accurancy) / (1f + num2);
		BasicAttackInterval *= 1f + num3;
		BasicReloadTime /= 1f + num4;
		BasicMags = (short)Mathf.CeilToInt((float)BasicMags * (1f + num5));
		BasicRecoil = (short)((float)BasicRecoil / (1f + num6));
		BasicMeleeDamage = (short)((float)BasicMeleeDamage * (1f + num7));
		BasicCriticalRate += num8;
		BasicExplosionRange = (short)((float)BasicExplosionRange * (1f + num9));
		DPH *= ItemBase.QUALITY_TYPE_PARAs[(int)base.Quality];
		AddNGUIItemStat(NGUIItemStat.StatType.Damage, DPH);
		AddNGUIItemStat(NGUIItemStat.StatType.Accurancy, Accurancy);
		AddNGUIItemStat(NGUIItemStat.StatType.AttackInterval, 1f / BasicAttackInterval);
		AddNGUIItemStat(NGUIItemStat.StatType.ReloadTime, BasicReloadTime);
		AddNGUIItemStat(NGUIItemStat.StatType.Mags, BasicMags);
		AddNGUIItemStat(NGUIItemStat.StatType.Recoil, BasicRecoil);
		AddNGUIItemStat(NGUIItemStat.StatType.MeleeDamage, (float)BasicMeleeDamage * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.CriticalRate, (float)BasicCriticalRate * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.CriticalDamage, (float)BasicCriticalDamage * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.HasScope, BasicHaveScope);
		AddNGUIItemStat(NGUIItemStat.StatType.ExplosionRange, BasicExplosionRange);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementPara, ElementFirePara);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementPara, ElementShockPara);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementPara, ElementCorrosivePara);
		AddNGUIItemStat(NGUIItemStat.StatType.ScopeRate, BasicScopeRate);
		AddNGUIItemStat(NGUIItemStat.StatType.FadeRange, BasicFadeRange);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)MaxHpBonus * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, HpRecovery);
		string text = string.Empty;
		foreach (SkillInfo skill2 in Skills)
		{
			text = text + skill2.Name + "|";
			mNGUIBaseItem.skillIDs.Add(skill2.ID);
		}
		if (base.Quality == ItemQuality.Legendary)
		{
			mNGUIBaseItem.LegendaryName = base.LegendaryName;
		}
	}
}
