public class ItemBackPack : ItemBase
{
	public byte BasicBagCapacity { get; set; }

	public override void generateItemProperties()
	{
		base.generateItemProperties();
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[7];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 0, string.Empty, false);
			if (data.Equals(base.ItemStyle))
			{
				BasicBagCapacity = (byte)unitDataTable.GetData(i, 2, 0, false);
				base.FormulaParameter1 = (int)(byte)unitDataTable.GetData(i, 3, 0, false);
				base.FormulaParameter2 = (int)(byte)unitDataTable.GetData(i, 4, 0, false);
				break;
			}
		}
		affectItemPropertiesByLevelAndFormula();
	}

	public override void affectItemPropertiesByLevelAndFormula()
	{
	}

	public override void generateEquipmentSkills()
	{
	}

	protected override void GetIntoBackPack()
	{
	}
}
