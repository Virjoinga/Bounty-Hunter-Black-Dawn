using System.Collections.Generic;
using System.IO;

public class IAPItemState
{
	public enum ItemType
	{
		ExpScale = 0,
		InfiniteBullet = 1,
		Newbie1 = 2,
		Newbie2 = 3
	}

	private Dictionary<ItemType, GlobalIAPItem> iapItemDic = new Dictionary<ItemType, GlobalIAPItem>();

	public IAPItemState()
	{
		iapItemDic.Add(ItemType.ExpScale, new ItemExpScale());
		iapItemDic.Add(ItemType.InfiniteBullet, new ItemInfiniteBullet());
		iapItemDic.Add(ItemType.Newbie1, new ItemNewbie1());
		iapItemDic.Add(ItemType.Newbie2, new ItemNewbie2());
	}

	public void ReadData(BinaryReader br)
	{
		foreach (KeyValuePair<ItemType, GlobalIAPItem> item in iapItemDic)
		{
			item.Value.ReadData(br);
		}
	}

	public void WriteData(BinaryWriter bw)
	{
		foreach (KeyValuePair<ItemType, GlobalIAPItem> item in iapItemDic)
		{
			item.Value.WriteData(bw);
		}
	}

	public GlobalIAPItem GetGlobalIAPItem(ItemType type)
	{
		return iapItemDic[type];
	}
}
