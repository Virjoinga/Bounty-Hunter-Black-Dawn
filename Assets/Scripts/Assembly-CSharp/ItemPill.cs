public class ItemPill : ItemBase
{
	public short HPRecovery { get; set; }

	public byte Superposition { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		base.ItemLevel = (byte)(base.ItemLevel / 5 * 5);
		if (base.ItemLevel == 0)
		{
			base.ItemLevel = 1;
		}
		HPRecovery = (short)((base.ItemLevel / 5 + 1) * 300);
		Superposition = 1;
		mNGUIBaseItem.name = base.ItemName;
		NGUIItemStat nGUIItemStat = new NGUIItemStat();
		nGUIItemStat.statType = NGUIItemStat.StatType.HPRecovery;
		nGUIItemStat.statValue = HPRecovery;
		mNGUIBaseItem.itemStats.Add(nGUIItemStat);
		NGUIItemStat nGUIItemStat2 = new NGUIItemStat();
		nGUIItemStat2.statType = NGUIItemStat.StatType.Superposition;
		nGUIItemStat2.statValue = (int)Superposition;
		mNGUIBaseItem.itemStats.Add(nGUIItemStat2);
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
		HPRecovery += (short)((float)(int)base.ItemLevel * 0.1f);
	}

	public override void generateEquipmentSkills()
	{
	}

	protected override void GetIntoBackPack()
	{
		NGUIGameItem value = new NGUIGameItem(base.SpecialID, mNGUIBaseItem);
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
		{
			if (itemInfoData.BackPackItems[i] == null)
			{
				itemInfoData.BackPackItems[i] = value;
				break;
			}
		}
	}

	public override void generateNGUIBaseItem()
	{
		mNGUIBaseItem.SetPrice((base.ItemLevel / 5 + 1) * 8);
		mNGUIBaseItem.name = base.ItemName;
		mNGUIBaseItem.ItemLevel = (byte)(base.ItemLevel / 5 + 1);
		mNGUIBaseItem.iconName = base.ItemName;
		mNGUIBaseItem.previewIconName = "p_" + base.ItemName;
	}
}
