using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GambleManagerAbandoned
{
	public const int MITHRIL_OF_RESET = 5;

	private static float TIME_OF_REFRESH = 600f;

	private static GambleManagerAbandoned instance;

	private bool bUsing;

	private int gambleTimes;

	private List<GambleItemAbility> gambleItemList;

	private List<int> gambleItemIndexList;

	private GambleDetails mGambleDetails;

	public Timer mGambleItemRefreshTimer;

	private GambleManagerAbandoned()
	{
		gambleItemList = new List<GambleItemAbility>();
		gambleItemIndexList = new List<int>();
		mGambleDetails = new GambleDetails();
		mGambleItemRefreshTimer = new Timer();
	}

	public static GambleManagerAbandoned GetInstance()
	{
		if (instance == null)
		{
			instance = new GambleManagerAbandoned();
		}
		return instance;
	}

	public bool Use()
	{
		if (bUsing)
		{
			return false;
		}
		if (mGambleDetails.Spend(gambleTimes))
		{
			bUsing = true;
			GameApp.GetInstance().Save();
			return true;
		}
		return false;
	}

	public bool IsUsing()
	{
		return bUsing;
	}

	public void UseFinish()
	{
		bUsing = false;
		gambleTimes++;
		GameApp.GetInstance().Save();
	}

	public void ResetWhenOnLine()
	{
		bUsing = false;
		gambleTimes = 0;
		GameApp.GetInstance().Save();
	}

	public void ResetWhenClickButton()
	{
		gambleTimes = 1;
		GameApp.GetInstance().Save();
	}

	public string GetPriceInfo()
	{
		return mGambleDetails.GetDescription(gambleTimes);
	}

	public string GetWarning()
	{
		return mGambleDetails.GetWarning(gambleTimes);
	}

	public int GetRandomItemId()
	{
		int num = mGambleDetails.GetRandomItemIndex(gambleTimes);
		Debug.Log("---------------------");
		Debug.Log("index : " + num);
		for (int i = 0; i < gambleItemIndexList.Count; i++)
		{
			if (num == gambleItemIndexList[i])
			{
				num = i;
				break;
			}
		}
		Debug.Log("gambleItemIndexList[i] : " + num);
		return num;
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(gambleItemList.Count);
		for (int i = 0; i < gambleItemList.Count; i++)
		{
			bw.Write((byte)gambleItemList[i].IconIndex);
			bw.Write((byte)gambleItemList[i].Quality);
			int count = gambleItemList[i].ItemClass.Count;
			bw.Write(count);
			for (int j = 0; j < count; j++)
			{
				bw.Write((byte)gambleItemList[i].ItemClass[j]);
				bw.Write(gambleItemList[i].Number[j]);
			}
			bw.Write(gambleItemIndexList[i]);
		}
		bw.Write(mGambleItemRefreshTimer.GetTimeNeededToNextReady());
		bw.Write(gambleTimes);
		bw.Write(bUsing);
	}

	public void Load(BinaryReader br)
	{
		gambleItemList.Clear();
		gambleItemIndexList.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			GambleItemAbility gambleItemAbility = new GambleItemAbility();
			gambleItemAbility.IconIndex = br.ReadByte();
			gambleItemAbility.Quality = (ItemQuality)br.ReadByte();
			gambleItemAbility.ItemClass.Clear();
			gambleItemAbility.Number.Clear();
			int num2 = br.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				gambleItemAbility.ItemClass.Add((ItemClasses)br.ReadByte());
				gambleItemAbility.Number.Add(br.ReadByte());
			}
			gambleItemList.Add(gambleItemAbility);
			gambleItemIndexList.Add(br.ReadInt32());
		}
		float interval = br.ReadSingle();
		mGambleItemRefreshTimer.SetTimer(interval, false);
		gambleTimes = br.ReadInt32();
		bUsing = br.ReadBoolean();
	}

	public void Init()
	{
		bUsing = false;
		gambleTimes = 0;
		mGambleItemRefreshTimer.SetTimer(0f, false);
		mGambleItemRefreshTimer.Do();
	}

	public void UpdateGambleInfo()
	{
		if (mGambleItemRefreshTimer.Ready())
		{
			mGambleItemRefreshTimer.SetTimer(TIME_OF_REFRESH, false);
			mGambleItemRefreshTimer.Do();
			RefreshItemList();
		}
	}

	public List<GambleItemAbility> GetGambleItemList()
	{
		return gambleItemList;
	}

	public void RefreshItemList()
	{
		List<GambleItemAbility> list = new List<GambleItemAbility>();
		GambleItemAbility gambleItemAbility = new GambleItemAbility();
		gambleItemAbility.IconIndex = 0;
		gambleItemAbility.Quality = ItemQuality.Legendary;
		gambleItemAbility.ItemClass.Add(GetRandomItemClass());
		string itemName = string.Empty;
		int number = 1;
		byte itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(gambleItemAbility.ItemClass[0], ref itemName, ref number, ref itemType);
		gambleItemAbility.Number.Add((byte)number);
		list.Add(gambleItemAbility);
		GambleItemAbility gambleItemAbility2 = new GambleItemAbility();
		gambleItemAbility2.IconIndex = 1;
		gambleItemAbility2.Quality = ItemQuality.Epic;
		gambleItemAbility2.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(gambleItemAbility2.ItemClass[0], ref itemName, ref number, ref itemType);
		gambleItemAbility2.Number.Add((byte)number);
		list.Add(gambleItemAbility2);
		GambleItemAbility gambleItemAbility3 = new GambleItemAbility();
		gambleItemAbility3.IconIndex = 2;
		gambleItemAbility3.Quality = ItemQuality.Rare;
		gambleItemAbility3.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(gambleItemAbility3.ItemClass[0], ref itemName, ref number, ref itemType);
		gambleItemAbility3.Number.Add((byte)number);
		list.Add(gambleItemAbility3);
		GambleItemAbility gambleItemAbility4 = new GambleItemAbility();
		gambleItemAbility4.IconIndex = 3;
		gambleItemAbility4.Quality = ItemQuality.Uncommon;
		for (int i = 0; i < 5; i++)
		{
			gambleItemAbility4.ItemClass.Add(GetRandomItemClass());
			string itemName2 = string.Empty;
			int number2 = 1;
			byte itemType2 = 0;
			GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(gambleItemAbility4.ItemClass[i], ref itemName2, ref number2, ref itemType2);
			gambleItemAbility4.Number.Add((byte)number2);
		}
		list.Add(gambleItemAbility4);
		GambleItemAbility gambleItemAbility5 = new GambleItemAbility();
		gambleItemAbility5.IconIndex = 4;
		gambleItemAbility5.Quality = ItemQuality.Common;
		for (int j = 0; j < 5; j++)
		{
			gambleItemAbility5.ItemClass.Add(GetRandomItemClass());
			string itemName3 = string.Empty;
			int number3 = 1;
			byte itemType3 = 0;
			GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(gambleItemAbility5.ItemClass[j], ref itemName3, ref number3, ref itemType3);
			gambleItemAbility5.Number.Add((byte)number3);
		}
		list.Add(gambleItemAbility5);
		list.Add(gambleItemAbility5);
		gambleItemList.Clear();
		gambleItemIndexList.Clear();
		List<int> list2 = new List<int>();
		for (int k = 0; k < list.Count; k++)
		{
			list2.Add(k);
		}
		while (list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			gambleItemIndexList.Add(list2[index]);
			gambleItemList.Add(list[index]);
			Debug.Log("i : " + list2[index]);
			list.RemoveAt(index);
			list2.RemoveAt(index);
		}
	}

	private int GetItemCount()
	{
		return 6;
	}

	public static ItemQuality GetRandomItemQuality()
	{
		int num = Random.Range(0, 100);
		if (num < 3)
		{
			return ItemQuality.Legendary;
		}
		if (num < 11)
		{
			return ItemQuality.Epic;
		}
		if (num < 26)
		{
			return ItemQuality.Rare;
		}
		if (num < 50)
		{
			return ItemQuality.Uncommon;
		}
		return ItemQuality.Common;
	}

	public static ItemClasses GetRandomItemClass()
	{
		int num = 0;
		num = ((GameApp.GetInstance().GetUserState().GetCharLevel() < 9) ? Random.Range(0, 7) : Random.Range(0, 8));
		if (num == 0)
		{
			return ItemClasses.U_Shield;
		}
		return (ItemClasses)num;
	}

	public ItemBase CreateItemByRouletteIndex(int index)
	{
		List<GambleItemAbility> list = GetGambleItemList();
		int index2 = index % list.Count;
		GambleItemAbility gambleItemAbility = list[index2];
		byte b = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
		int num = Random.Range(0, 2);
		if (b <= num)
		{
			num = b - 1;
		}
		byte level = (byte)(b - num);
		int index3 = Random.Range(0, gambleItemAbility.ItemClass.Count);
		return GameApp.GetInstance().GetLootManager().CreateItemBase(level, gambleItemAbility.ItemClass[index3], gambleItemAbility.Number[index3], gambleItemAbility.Quality, 0);
	}
}
