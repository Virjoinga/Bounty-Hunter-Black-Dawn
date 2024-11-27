public class ItemShield : ItemBase
{
	private static float[] CapacityCompanyPara = new float[6] { 0f, 1.3f, 1f, 1f, 1.1f, 1f };

	private static float[] RecoveryCompanyPara = new float[6] { 0f, 0.9f, 1f, 1f, 1.1f, 1.3f };

	public static float[] RecoveryDelayCompanyPara = new float[6] { 0f, 1.2f, 0.9f, 1f, 0.85f, 0.8f };

	public static float[] HpEnhancePara = new float[6] { 1f, 1f, 1f, 1.2f, 1.4f, 1.6f };

	public short BasicShieldCapacity { get; set; }

	public short BasicShieldRecoverySpeed { get; set; }

	public float BasicShieldRecoveryDelay { get; set; }

	public short FireResistance { get; set; }

	public short ShockResistance { get; set; }

	public short CorrosiveResistance { get; set; }

	public short MaxHpBonus { get; set; }

	public short HpRecovery { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		ShieldConfig shieldConfig = GameConfig.GetInstance().shieldConfig[base.ItemName];
		base.ItemCompany = shieldConfig.ItemCompany;
		base.LegendaryName = shieldConfig.LegendaryName;
		BasicShieldCapacity = (short)((float)(20 + base.ItemLevel * 80) * CapacityCompanyPara[(int)base.ItemCompany]);
		BasicShieldRecoverySpeed = (short)((float)(8 + base.ItemLevel * 16) * RecoveryCompanyPara[(int)base.ItemCompany]);
		BasicShieldRecoveryDelay = 5f * RecoveryDelayCompanyPara[(int)base.ItemCompany];
		FireResistance = 0;
		ShockResistance = 0;
		CorrosiveResistance = 0;
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
		base.generateEquipmentSkills();
	}

	protected override void GetIntoBackPack()
	{
		NGUIGameItem nGUIGameItem = new NGUIGameItem(base.SpecialID, mNGUIBaseItem);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (base.ItemLevel <= GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			if (!itemInfoData.IsShieldEquiped)
			{
				itemInfoData.Shield = nGUIGameItem;
				itemInfoData.IsShieldEquiped = true;
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.RefreshShieldFromItemInfo();
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
		mNGUIBaseItem.iconName = "Shield01";
		mNGUIBaseItem.previewIconName = "p_Shield01";
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		foreach (SkillInfo skill in Skills)
		{
			num += (float)skill.ShieldCapacity * 0.01f;
			num2 += (float)skill.ShieldRecovery * 0.01f;
			num3 += (float)skill.ShieldRecoveryDelay * 0.01f;
			FireResistance += skill.FireResistance;
			ShockResistance += skill.ShockResistance;
			CorrosiveResistance += skill.CorrosiveResistance;
			MaxHpBonus += skill.MaxHp;
			HpRecovery += skill.HpRecovery;
		}
		BasicShieldCapacity = (short)((float)BasicShieldCapacity * (1f + num));
		BasicShieldRecoverySpeed = (short)((float)BasicShieldRecoverySpeed * (1f + num2));
		BasicShieldRecoveryDelay /= 1f + num3;
		BasicShieldCapacity = (short)((float)BasicShieldCapacity * ItemBase.QUALITY_TYPE_PARAs[(int)base.Quality]);
		BasicShieldRecoverySpeed = (short)((float)BasicShieldRecoverySpeed * ItemBase.QUALITY_TYPE_PARAs[(int)base.Quality]);
		AddNGUIItemStat(NGUIItemStat.StatType.ShieldCapacity, BasicShieldCapacity);
		AddNGUIItemStat(NGUIItemStat.StatType.RecoverySpeed, BasicShieldRecoverySpeed);
		AddNGUIItemStat(NGUIItemStat.StatType.RecoveryDelay, BasicShieldRecoveryDelay);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementResistance, FireResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementResistance, ShockResistance);
		AddNGUIItemStat(NGUIItemStat.StatType.ElementResistance, CorrosiveResistance);
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
