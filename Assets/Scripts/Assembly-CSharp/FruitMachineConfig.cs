using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FruitMachineConfig : GambleConfig
{
	private Dictionary<ItemQuality, int> mBonusUseTimesDic;

	private Dictionary<ItemQuality, int> mBonusUseTimesLimitedDic;

	private Dictionary<ItemQuality, int> mBonusPointDic;

	private Dictionary<ItemQuality, int> mBonusPointLimitedDic;

	public FruitMachineConfig()
	{
		mBonusPointLimitedDic = InitDic();
		mBonusUseTimesLimitedDic = InitDic();
		mBonusPointDic = InitDic();
		mBonusUseTimesDic = InitDic();
	}

	private Dictionary<ItemQuality, int> InitDic()
	{
		Dictionary<ItemQuality, int> dictionary = new Dictionary<ItemQuality, int>();
		dictionary.Add(ItemQuality.Common, 0);
		dictionary.Add(ItemQuality.Uncommon, 0);
		dictionary.Add(ItemQuality.Rare, 0);
		dictionary.Add(ItemQuality.Epic, 0);
		dictionary.Add(ItemQuality.Legendary, 0);
		return dictionary;
	}

	protected override void OnInit()
	{
		base.OnInit();
		mBonusPointDic[ItemQuality.Common] = 0;
		mBonusPointDic[ItemQuality.Uncommon] = 0;
		mBonusPointDic[ItemQuality.Rare] = 0;
		mBonusPointDic[ItemQuality.Epic] = 0;
		mBonusPointDic[ItemQuality.Legendary] = 0;
		mBonusUseTimesDic[ItemQuality.Common] = 0;
		mBonusUseTimesDic[ItemQuality.Uncommon] = 0;
		mBonusUseTimesDic[ItemQuality.Rare] = 0;
		mBonusUseTimesDic[ItemQuality.Epic] = 0;
		mBonusUseTimesDic[ItemQuality.Legendary] = 0;
	}

	protected override void OnSave(BinaryWriter bw)
	{
		base.OnSave(bw);
		bw.Write(mBonusUseTimesDic.Count);
		foreach (KeyValuePair<ItemQuality, int> item in mBonusUseTimesDic)
		{
			bw.Write((int)item.Key);
			bw.Write(item.Value);
		}
		bw.Write(mBonusPointDic.Count);
		foreach (KeyValuePair<ItemQuality, int> item2 in mBonusPointDic)
		{
			bw.Write((int)item2.Key);
			bw.Write(item2.Value);
		}
	}

	protected override void OnLoad(BinaryReader br)
	{
		base.OnLoad(br);
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ItemQuality key = (ItemQuality)br.ReadInt32();
			mBonusUseTimesDic[key] = br.ReadInt32();
		}
		num = br.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			ItemQuality key2 = (ItemQuality)br.ReadInt32();
			mBonusPointDic[key2] = br.ReadInt32();
		}
	}

	protected void SetBonusLimited(Dictionary<ItemQuality, int> bonusUseTimesLimitedDic, Dictionary<ItemQuality, int> bonusPointLimitedDic)
	{
		mBonusPointLimitedDic = bonusPointLimitedDic;
		mBonusUseTimesLimitedDic = bonusUseTimesLimitedDic;
	}

	public string GetBonusPointDiscription(ItemQuality quality)
	{
		return mBonusPointDic[quality] + "/" + mBonusPointLimitedDic[quality];
	}

	public float GetBonusUseTimesPercent(ItemQuality quality)
	{
		return (float)mBonusUseTimesDic[quality] / (float)mBonusUseTimesLimitedDic[quality];
	}

	public void AddBonusUseTimes(ItemQuality quality, int times)
	{
		Debug.Log(string.Concat("ItemQuality : ", quality, " times : ", times));
		if (mBonusPointDic[quality] >= mBonusPointLimitedDic[quality])
		{
			return;
		}
		Dictionary<ItemQuality, int> dictionary;
		Dictionary<ItemQuality, int> dictionary2 = (dictionary = mBonusUseTimesDic);
		ItemQuality key;
		ItemQuality key2 = (key = quality);
		int num = dictionary[key];
		dictionary2[key2] = num + times;
		if (mBonusUseTimesDic[quality] >= mBonusUseTimesLimitedDic[quality])
		{
			Dictionary<ItemQuality, int> dictionary3;
			Dictionary<ItemQuality, int> dictionary4 = (dictionary3 = mBonusPointDic);
			ItemQuality key3 = (key = quality);
			num = dictionary3[key];
			dictionary4[key3] = num + 1;
			if (mBonusPointDic[quality] >= mBonusPointLimitedDic[quality])
			{
				mBonusPointDic[quality] = mBonusPointLimitedDic[quality];
				mBonusUseTimesDic[quality] = 0;
				return;
			}
			Dictionary<ItemQuality, int> dictionary5;
			Dictionary<ItemQuality, int> dictionary6 = (dictionary5 = mBonusUseTimesDic);
			ItemQuality key4 = (key = quality);
			num = dictionary5[key];
			dictionary6[key4] = num - mBonusUseTimesLimitedDic[quality];
		}
	}

	public bool IsBonusPointEnough(ItemQuality quality)
	{
		return mBonusPointDic[quality] == mBonusPointLimitedDic[quality];
	}

	public ItemBase UseBonusPoint(ItemQuality quality)
	{
		if (IsBonusPointEnough(quality))
		{
			mBonusPointDic[quality] = 0;
			byte b = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
			int num = Random.Range(0, 100);
			byte itemType = 0;
			if (num < 11)
			{
				itemType = 2;
			}
			ItemClasses itemClass = ItemClasses.AssultRifle;
			switch (itemType)
			{
			case 0:
			{
				int num2 = 1;
				num2 = Random.Range(1, 9);
				if (b < 9 && num2 == 7)
				{
					num2 = Random.Range(1, 7);
				}
				if (num2 == 8)
				{
					itemType = 1;
				}
				itemClass = (ItemClasses)num2;
				break;
			}
			case 2:
				itemClass = ItemClasses.U_Shield;
				break;
			}
			string itemName = string.Empty;
			int number = 1;
			GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
			return GameApp.GetInstance().GetLootManager().CreateItemBase(b, itemClass, (byte)number, quality, 0);
		}
		return null;
	}
}
