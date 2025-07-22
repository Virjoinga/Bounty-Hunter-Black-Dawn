using System.Collections.Generic;
using UnityEngine;

public class ItemSpecial : ItemBase
{
	public string CallName { get; set; }

	public float DPH { get; set; }

	public ElementType Element { get; set; }

	public short Damage { get; set; }

	public byte Accurancy { get; set; }

	public float AttackInterval { get; set; }

	public float ReloadTime { get; set; }

	public byte Mags { get; set; }

	public byte Recoil { get; set; }

	public byte MeleeDamage { get; set; }

	public byte CriticalRate { get; set; }

	public byte CriticalDamage { get; set; }

	public byte HaveScope { get; set; }

	public byte ExplosionRange { get; set; }

	public byte ElementPara { get; set; }

	public float ZoomRate { get; set; }

	public short FadeRange { get; set; }

	public float ExplosionFadeRange { get; set; }

	public short ShieldCapacity { get; set; }

	public short ShieldRecoverySpeed { get; set; }

	public float ShieldRecoveryDelay { get; set; }

	public byte SkillCapacity { get; set; }

	public short HPRecovery { get; set; }

	public byte Superposition { get; set; }

	public byte BagCapacity { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		Debug.Log("SpecialId: " + base.SpecialID);
		SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[base.SpecialID];
		base.ItemName = specialItemConfig.ItemName;
		mNGUIBaseItem.Quality = specialItemConfig.Quality;
		mNGUIBaseItem.name = base.ItemName;
		mNGUIBaseItem.iconAtlas = ResourceLoad.GetInstance().LoadAtlas("HUD", "Item").GetComponent<UIAtlas>();
		byte b = base.ItemType;
		if (b == 7)
		{
		}
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
	}

	protected override void GetIntoBackPack()
	{
		NGUIGameItem value = new NGUIGameItem(base.SpecialID, mNGUIBaseItem);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (base.ItemType != 7)
		{
			List<NGUIGameItem> backPackItems = itemInfoData.BackPackItems;
			for (int i = 0; i < backPackItems.Count; i++)
			{
				if (backPackItems[i] == null)
				{
					backPackItems[i] = value;
					break;
				}
			}
		}
		else
		{
			itemInfoData.AddStoryItem(base.SpecialID);
		}
	}
}
