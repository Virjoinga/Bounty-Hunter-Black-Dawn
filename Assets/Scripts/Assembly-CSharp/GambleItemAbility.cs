using System.Collections.Generic;

public class GambleItemAbility
{
	public List<ItemClasses> ItemClass = new List<ItemClasses>();

	public List<byte> Number = new List<byte>();

	public ItemQuality Quality { get; set; }

	public int IconIndex { get; set; }

	public string BigIconName
	{
		get
		{
			if (IconIndex == 3)
			{
				return "unknown";
			}
			if (IconIndex == 4 || IconIndex == 5)
			{
				return "unknown";
			}
			return "p_" + SmallIconName;
		}
	}

	public string SmallIconName
	{
		get
		{
			if (IconIndex == 3)
			{
				return "unknown";
			}
			if (IconIndex == 4 || IconIndex == 5)
			{
				return "unknown";
			}
			if (ItemClass[0] == ItemClasses.U_Shield)
			{
				return LootManager.GetPrefabNameByItemClassAndNumber(ItemClass[0], 1);
			}
			return LootManager.GetPrefabNameByItemClassAndNumber(ItemClass[0], Number[0]);
		}
	}
}
