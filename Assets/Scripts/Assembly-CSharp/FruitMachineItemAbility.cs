using System.Collections.Generic;
using System.IO;

public class FruitMachineItemAbility
{
	public List<ItemClasses> ItemClass = new List<ItemClasses>();

	public List<byte> Number = new List<byte>();

	public ItemQuality Quality { get; set; }

	public int IconIndex { get; set; }

	public bool IsUnKnown { get; set; }

	public string SmallIconName
	{
		get
		{
			if (IsUnKnown)
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

	public FruitMachineItemAbility()
	{
		IsUnKnown = false;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(IsUnKnown);
		bw.Write(IconIndex);
		bw.Write((byte)Quality);
		int count = ItemClass.Count;
		bw.Write(count);
		for (int i = 0; i < count; i++)
		{
			bw.Write((byte)ItemClass[i]);
			bw.Write(Number[i]);
		}
	}

	public void Load(BinaryReader br)
	{
		IsUnKnown = br.ReadBoolean();
		IconIndex = br.ReadInt32();
		Quality = (ItemQuality)br.ReadByte();
		ItemClass.Clear();
		Number.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ItemClass.Add((ItemClasses)br.ReadByte());
			Number.Add(br.ReadByte());
		}
	}
}
