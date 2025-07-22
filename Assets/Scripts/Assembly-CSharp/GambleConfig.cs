using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class GambleConfig
{
	public class GambleResult
	{
		public bool Success { get; set; }

		public string Discription { get; set; }
	}

	public enum ResetType
	{
		Online = 0,
		ClickButton = 1
	}

	private List<FruitMachineItemAbility> mItemList;

	private List<int> mItemIndexList;

	private int mTotalUseTimes;

	public Timer mItemRefreshTimer;

	public GambleConfig()
	{
		mItemList = new List<FruitMachineItemAbility>();
		mItemIndexList = new List<int>();
		mItemRefreshTimer = new Timer();
	}

	public void Init()
	{
		mTotalUseTimes = 0;
		mItemRefreshTimer.SetTimer(0f, false);
		mItemRefreshTimer.Do();
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void Save(BinaryWriter bw)
	{
		bw.Write(mItemList.Count);
		for (int i = 0; i < mItemList.Count; i++)
		{
			mItemList[i].Save(bw);
			bw.Write(mItemIndexList[i]);
		}
		bw.Write(mItemRefreshTimer.GetTimeNeededToNextReady());
		bw.Write(mTotalUseTimes);
		OnSave(bw);
	}

	protected virtual void OnSave(BinaryWriter bw)
	{
	}

	public void Load(BinaryReader br)
	{
		mItemList.Clear();
		mItemIndexList.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			FruitMachineItemAbility fruitMachineItemAbility = new FruitMachineItemAbility();
			fruitMachineItemAbility.Load(br);
			mItemList.Add(fruitMachineItemAbility);
			mItemIndexList.Add(br.ReadInt32());
		}
		float interval = br.ReadSingle();
		mItemRefreshTimer.SetTimer(interval, false);
		mTotalUseTimes = br.ReadInt32();
		OnLoad(br);
	}

	protected virtual void OnLoad(BinaryReader br)
	{
	}

	public GambleResult Reset(ResetType type)
	{
		GambleResult gambleResult = new GambleResult();
		switch (type)
		{
		case ResetType.Online:
			gambleResult = OnResetOnline();
			if (gambleResult.Success)
			{
				ResetTimer();
				mTotalUseTimes = 0;
				RefreshItemList();
				GameApp.GetInstance().Save();
			}
			break;
		case ResetType.ClickButton:
			gambleResult = OnResetClickButton();
			if (gambleResult.Success)
			{
				ResetTimer();
				mTotalUseTimes = 1;
				RefreshItemList();
				GameApp.GetInstance().Save();
			}
			break;
		}
		return gambleResult;
	}

	protected virtual GambleResult OnResetOnline()
	{
		GambleResult gambleResult = new GambleResult();
		gambleResult.Success = true;
		return gambleResult;
	}

	protected virtual GambleResult OnResetClickButton()
	{
		GambleResult gambleResult = new GambleResult();
		gambleResult.Success = true;
		return gambleResult;
	}

	private void RefreshItemList()
	{
		List<FruitMachineItemAbility> list = OnRefreshItemList();
		if (list != null)
		{
			mItemList.Clear();
			mItemIndexList.Clear();
			List<int> list2 = new List<int>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(i);
			}
			while (list.Count > 0)
			{
				int index = Random.Range(0, list.Count);
				mItemIndexList.Add(list2[index]);
				mItemList.Add(list[index]);
				list.RemoveAt(index);
				list2.RemoveAt(index);
			}
		}
	}

	protected virtual List<FruitMachineItemAbility> OnRefreshItemList()
	{
		return new List<FruitMachineItemAbility>();
	}

	public void Update()
	{
		if (mItemRefreshTimer.Ready())
		{
			ResetTimer();
			RefreshItemList();
			Debug.Log("FruitMachine Refresh!!!");
		}
		OnUpdate();
	}

	public void Pause()
	{
		mItemRefreshTimer.Pause();
	}

	public void Resume()
	{
		mItemRefreshTimer.Resume();
	}

	protected virtual void OnUpdate()
	{
	}

	private void ResetTimer()
	{
		mItemRefreshTimer.SetTimer(600f, false);
		mItemRefreshTimer.Do();
	}

	public virtual string GetResetCost()
	{
		return "Unknown";
	}

	public virtual string GetCost()
	{
		return "Unknown";
	}

	public GambleResult Use()
	{
		GambleResult gambleResult = OnUse();
		if (gambleResult.Success)
		{
			mTotalUseTimes++;
			GameApp.GetInstance().Save();
		}
		return gambleResult;
	}

	protected virtual GambleResult OnUse()
	{
		GambleResult gambleResult = new GambleResult();
		gambleResult.Success = true;
		return gambleResult;
	}

	protected int GetTotalUseTimes()
	{
		return mTotalUseTimes;
	}

	public List<FruitMachineItemAbility> GetItemList()
	{
		return mItemList;
	}

	public ItemBase GetItem(int index)
	{
		List<FruitMachineItemAbility> itemList = GetItemList();
		int index2 = index % itemList.Count;
		FruitMachineItemAbility fruitMachineItemAbility = itemList[index2];
		byte b = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
		int num = Random.Range(0, 2);
		if (b <= num)
		{
			num = b - 1;
		}
		byte level = (byte)(b - num);
		int index3 = Random.Range(0, fruitMachineItemAbility.ItemClass.Count);
		return GameApp.GetInstance().GetLootManager().CreateItemBase(level, fruitMachineItemAbility.ItemClass[index3], fruitMachineItemAbility.Number[index3], fruitMachineItemAbility.Quality, 0);
	}

	public int GetRandomItemId()
	{
		int num = GetRandomItemIndex();
		Debug.Log("---------------------");
		Debug.Log("index : " + num);
		for (int i = 0; i < mItemIndexList.Count; i++)
		{
			if (num == mItemIndexList[i])
			{
				num = i;
				break;
			}
		}
		Debug.Log("gambleItemIndexList[i] : " + num);
		return num;
	}

	protected virtual int GetRandomItemIndex()
	{
		return 0;
	}

	protected ItemQuality GetRandomItemQuality()
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

	protected ItemClasses GetRandomItemClass()
	{
		int num = 0;
		num = Random.Range(1, 10);
		if (GameApp.GetInstance().GetUserState().GetCharLevel() < 9 && num == 7)
		{
			num = Random.Range(1, 7);
		}
		return (ItemClasses)num;
	}
}
