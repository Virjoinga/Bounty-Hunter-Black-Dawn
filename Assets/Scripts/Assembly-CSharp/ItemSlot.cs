using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : ItemBase
{
	public short Damage { get; set; }

	public short Accurancy { get; set; }

	public short AttackInterval { get; set; }

	public short ReloadTime { get; set; }

	public short Mags { get; set; }

	public short Recoil { get; set; }

	public short BulletRecovery { get; set; }

	public short CriticalRate { get; set; }

	public short CriticalDamage { get; set; }

	public short MeleeDamage { get; set; }

	public short ShieldCapacity { get; set; }

	public short ShieldRecovery { get; set; }

	public short ShieldRecoveryDelay { get; set; }

	public short FireResistance { get; set; }

	public short ShockResistance { get; set; }

	public short CorrosiveResistance { get; set; }

	public short MaxHp { get; set; }

	public short HpRecovery { get; set; }

	public short Speed { get; set; }

	public short DamageReduction { get; set; }

	public short DamageToHealth { get; set; }

	public short AllElementResistance { get; set; }

	public short ExplosionDamageReduction { get; set; }

	public short DamageImmune { get; set; }

	public short DropRate { get; set; }

	public short ElementTriggerRate { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		mNGUIBaseItem.name = base.ItemName;
		mNGUIBaseItem.equipmentSlot = NGUIBaseItem.EquipmentSlot.SkillSlot;
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
		Skills.Clear();
		int num = (int)base.Quality * 2 - 1;
		SkillInfo item = default(SkillInfo);
		int num2 = Random.Range(1, 27);
		short key = (short)(num * 100 + num2);
		ChipPrefixConfig chipPrefixConfig = GameConfig.GetInstance().chipPrefixConfig[key];
		item.ID = chipPrefixConfig.ID;
		item.Name = chipPrefixConfig.Name;
		item.Quality = chipPrefixConfig.Quality;
		item.Score = chipPrefixConfig.Score;
		item.PrefixType = chipPrefixConfig.PrefixType;
		item.Damage = chipPrefixConfig.Damage;
		item.Accurancy = chipPrefixConfig.Accurancy;
		item.AttackInterval = chipPrefixConfig.AttackInterval;
		item.ReloadTime = chipPrefixConfig.ReloadTime;
		item.Mags = chipPrefixConfig.Mags;
		item.Recoil = chipPrefixConfig.Recoil;
		item.BulletRecovery = chipPrefixConfig.BulletRecovery;
		item.CriticalRate = chipPrefixConfig.CriticalRate;
		item.CriticalDamage = chipPrefixConfig.CriticalDamage;
		item.MeleeDamage = chipPrefixConfig.MeleeDamage;
		item.ShieldCapacity = chipPrefixConfig.ShieldCapacity;
		item.ShieldRecovery = chipPrefixConfig.ShieldRecovery;
		item.ShieldRecoveryDelay = chipPrefixConfig.ShieldRecoveryDelay;
		item.FireResistance = chipPrefixConfig.FireResistance;
		item.ShockResistance = chipPrefixConfig.ShockResistance;
		item.CorrosiveResistance = chipPrefixConfig.CorrosiveResistance;
		item.MaxHp = chipPrefixConfig.MaxHp;
		item.HpRecovery = chipPrefixConfig.HpRecovery;
		item.Speed = chipPrefixConfig.Speed;
		item.DamageReduction = chipPrefixConfig.DamageReduction;
		item.DamageToHealth = chipPrefixConfig.DamageToHealth;
		item.AllElementResistance = chipPrefixConfig.AllElementResistance;
		item.ExplosionDamageReduction = chipPrefixConfig.ExplosionDamageReduction;
		item.DamageImmune = chipPrefixConfig.DamageImmune;
		item.DropRate = chipPrefixConfig.DropRate;
		item.ElementTriggerRate = chipPrefixConfig.ElementTriggerRate;
		base.ItemCompany = chipPrefixConfig.Company;
		Skills.Add(item);
	}

	protected override void GetIntoBackPack()
	{
		NGUIGameItem nGUIGameItem = new NGUIGameItem(base.SpecialID, mNGUIBaseItem);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (base.ItemLevel <= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			bool flag = ChipConflict();
			if (!itemInfoData.IsSlot1Equiped && !flag)
			{
				itemInfoData.Slot1 = nGUIGameItem;
				itemInfoData.IsSlot1Equiped = true;
				return;
			}
			if (itemInfoData.IsSlot2Enable && !itemInfoData.IsSlot2Equiped && !flag)
			{
				itemInfoData.Slot2 = nGUIGameItem;
				itemInfoData.IsSlot2Equiped = true;
				return;
			}
			if (itemInfoData.IsSlot3Enable && !itemInfoData.IsSlot3Equiped && !flag)
			{
				itemInfoData.Slot3 = nGUIGameItem;
				itemInfoData.IsSlot3Equiped = true;
				return;
			}
			if (itemInfoData.IsSlot4Enable && !itemInfoData.IsSlot4Equiped && !flag)
			{
				itemInfoData.Slot4 = nGUIGameItem;
				itemInfoData.IsSlot4Equiped = true;
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
		mNGUIBaseItem.ItemLevel = 1;
		mNGUIBaseItem.name = base.ItemName;
		mNGUIBaseItem.SetPrice((calculateSkillScore() * calculateSkillScore() + 1) * 120);
		switch (base.Quality)
		{
		case ItemQuality.Common:
			mNGUIBaseItem.SetPrice(125);
			break;
		case ItemQuality.Uncommon:
			mNGUIBaseItem.SetPrice(375);
			break;
		case ItemQuality.Rare:
			mNGUIBaseItem.SetPrice(1250);
			break;
		case ItemQuality.Epic:
			mNGUIBaseItem.SetPrice(12500);
			break;
		case ItemQuality.Legendary:
			mNGUIBaseItem.SetPrice(25000);
			break;
		}
		foreach (SkillInfo skill in Skills)
		{
			Damage += skill.Damage;
			Accurancy += skill.Accurancy;
			AttackInterval += skill.AttackInterval;
			ReloadTime += skill.ReloadTime;
			Mags += skill.Mags;
			Recoil += skill.Recoil;
			BulletRecovery += skill.BulletRecovery;
			CriticalRate += skill.CriticalRate;
			CriticalDamage += skill.CriticalDamage;
			MeleeDamage += skill.MeleeDamage;
			ShieldCapacity += skill.ShieldCapacity;
			ShieldRecovery += skill.ShieldRecovery;
			ShieldRecoveryDelay += skill.ShieldRecoveryDelay;
			FireResistance += skill.FireResistance;
			ShockResistance += skill.ShockResistance;
			CorrosiveResistance += skill.CorrosiveResistance;
			MaxHp += skill.MaxHp;
			HpRecovery += skill.HpRecovery;
			Speed += skill.Speed;
			DamageReduction += skill.DamageReduction;
			DamageToHealth += skill.DamageToHealth;
			AllElementResistance += skill.AllElementResistance;
			ExplosionDamageReduction += skill.ExplosionDamageReduction;
			DamageImmune += skill.DamageImmune;
			DropRate += skill.DropRate;
			ElementTriggerRate += skill.ElementTriggerRate;
		}
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)Damage * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)Accurancy * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)AttackInterval * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)ReloadTime * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)Mags * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)Recoil * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, BulletRecovery);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)CriticalRate * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)CriticalDamage * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)MeleeDamage * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)ShieldCapacity * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)ShieldRecovery * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)ShieldRecoveryDelay * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, FireResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, ShockResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, CorrosiveResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)MaxHp * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, HpRecovery);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)Speed * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, DamageReduction);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, DamageToHealth);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, AllElementResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, ExplosionDamageReduction);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)DamageImmune * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)DropRate * 0.01f);
		AddNGUIItemStat(NGUIItemStat.StatType.Other, (float)ElementTriggerRate * 0.01f);
		string text = string.Empty;
		foreach (SkillInfo skill2 in Skills)
		{
			text = text + skill2.Name + "|";
			mNGUIBaseItem.skillIDs.Add(skill2.ID);
		}
	}

	public void MakeDefaultChip(short prefixID)
	{
		Skills.Clear();
		mNGUIBaseItem.itemStats.Clear();
		mNGUIBaseItem.skillIDs.Clear();
		Damage = 0;
		Accurancy = 0;
		AttackInterval = 0;
		ReloadTime = 0;
		Mags = 0;
		Recoil = 0;
		BulletRecovery = 0;
		CriticalRate = 0;
		CriticalDamage = 0;
		MeleeDamage = 0;
		ShieldCapacity = 0;
		ShieldRecovery = 0;
		ShieldRecoveryDelay = 0;
		FireResistance = 0;
		ShockResistance = 0;
		CorrosiveResistance = 0;
		MaxHp = 0;
		HpRecovery = 0;
		Speed = 0;
		DamageReduction = 0;
		DamageToHealth = 0;
		AllElementResistance = 0;
		ExplosionDamageReduction = 0;
		DamageImmune = 0;
		DropRate = 0;
		ElementTriggerRate = 0;
		SkillInfo item = default(SkillInfo);
		ChipPrefixConfig chipPrefixConfig = GameConfig.GetInstance().chipPrefixConfig[prefixID];
		item.ID = chipPrefixConfig.ID;
		item.Name = chipPrefixConfig.Name;
		item.Quality = chipPrefixConfig.Quality;
		item.Score = chipPrefixConfig.Score;
		item.PrefixType = chipPrefixConfig.PrefixType;
		item.Damage = chipPrefixConfig.Damage;
		item.Accurancy = chipPrefixConfig.Accurancy;
		item.AttackInterval = chipPrefixConfig.AttackInterval;
		item.ReloadTime = chipPrefixConfig.ReloadTime;
		item.Mags = chipPrefixConfig.Mags;
		item.Recoil = chipPrefixConfig.Recoil;
		item.BulletRecovery = chipPrefixConfig.BulletRecovery;
		item.CriticalRate = chipPrefixConfig.CriticalRate;
		item.CriticalDamage = chipPrefixConfig.CriticalDamage;
		item.MeleeDamage = chipPrefixConfig.MeleeDamage;
		item.ShieldCapacity = chipPrefixConfig.ShieldCapacity;
		item.ShieldRecovery = chipPrefixConfig.ShieldRecovery;
		item.ShieldRecoveryDelay = chipPrefixConfig.ShieldRecoveryDelay;
		item.FireResistance = chipPrefixConfig.FireResistance;
		item.ShockResistance = chipPrefixConfig.ShockResistance;
		item.CorrosiveResistance = chipPrefixConfig.CorrosiveResistance;
		item.MaxHp = chipPrefixConfig.MaxHp;
		item.HpRecovery = chipPrefixConfig.HpRecovery;
		item.Speed = chipPrefixConfig.Speed;
		item.DamageReduction = chipPrefixConfig.DamageReduction;
		item.DamageToHealth = chipPrefixConfig.DamageToHealth;
		item.AllElementResistance = chipPrefixConfig.AllElementResistance;
		item.ExplosionDamageReduction = chipPrefixConfig.ExplosionDamageReduction;
		item.DamageImmune = chipPrefixConfig.DamageImmune;
		item.DropRate = chipPrefixConfig.DropRate;
		item.ElementTriggerRate = chipPrefixConfig.ElementTriggerRate;
		base.ItemCompany = chipPrefixConfig.Company;
		Skills.Add(item);
		generateNGUIBaseItem();
	}

	public bool ChipConflict()
	{
		List<NGUIGameItem> list = new List<NGUIGameItem>();
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		list.Add(itemInfoData.Slot1);
		list.Add(itemInfoData.Slot2);
		list.Add(itemInfoData.Slot3);
		list.Add(itemInfoData.Slot4);
		foreach (NGUIGameItem item in list)
		{
			if (item != null && item.baseItem.skillIDs[0] % 100 == mNGUIBaseItem.skillIDs[0] % 100)
			{
				return true;
			}
		}
		return false;
	}
}
