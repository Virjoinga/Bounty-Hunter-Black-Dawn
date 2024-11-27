using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MithrilFruitMachine : FruitMachineConfig
{
	public const int MITHRIL_OF_RESET = 10;

	private int mMithrilCost
	{
		get
		{
			int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
			return 20 + 8 * charLevel;
		}
	}

	public MithrilFruitMachine()
	{
		SetBonusLimited(new Dictionary<ItemQuality, int>
		{
			{
				ItemQuality.Common,
				3
			},
			{
				ItemQuality.Uncommon,
				3
			},
			{
				ItemQuality.Rare,
				3
			},
			{
				ItemQuality.Epic,
				3
			},
			{
				ItemQuality.Legendary,
				3
			}
		}, new Dictionary<ItemQuality, int>
		{
			{
				ItemQuality.Common,
				1
			},
			{
				ItemQuality.Uncommon,
				3
			},
			{
				ItemQuality.Rare,
				6
			},
			{
				ItemQuality.Epic,
				12
			},
			{
				ItemQuality.Legendary,
				20
			}
		});
	}

	protected override void OnSave(BinaryWriter bw)
	{
		base.OnSave(bw);
		bw.Write(0);
	}

	protected override void OnLoad(BinaryReader br)
	{
		base.OnLoad(br);
		br.ReadInt32();
	}

	protected override int GetRandomItemIndex()
	{
		Interval interval = new Interval(12, 12, 12, 12, 12, 12, 8, 8, 4, 4, 2, 2);
		return interval.GetIndex();
	}

	protected override GambleResult OnUse()
	{
		GambleResult gambleResult = new GambleResult();
		gambleResult.Success = GameApp.GetInstance().GetGlobalState().BuyWithMithril(mMithrilCost);
		if (!gambleResult.Success)
		{
			gambleResult.Discription = LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH");
		}
		else
		{
			GameApp.GetInstance().GetUserState().OperInfo.AddInfo(OperatingInfoType.GAMBLE_MITHRIL, mMithrilCost);
			Interval interval = new Interval(0, 30, 30, 20, 20);
			int quality = interval.GetIndex() + 1;
			AddBonusUseTimes((ItemQuality)quality, 1);
		}
		return gambleResult;
	}

	public override string GetResetCost()
	{
		return string.Empty + 10;
	}

	public override string GetCost()
	{
		return string.Empty + mMithrilCost;
	}

	protected override GambleResult OnResetClickButton()
	{
		GambleResult gambleResult = new GambleResult();
		gambleResult.Success = GameApp.GetInstance().GetGlobalState().BuyWithMithril(10);
		if (!gambleResult.Success)
		{
			gambleResult.Discription = LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH");
		}
		return gambleResult;
	}

	protected override List<FruitMachineItemAbility> OnRefreshItemList()
	{
		List<FruitMachineItemAbility> list = new List<FruitMachineItemAbility>();
		FruitMachineItemAbility fruitMachineItemAbility = new FruitMachineItemAbility();
		fruitMachineItemAbility.IconIndex = 0;
		fruitMachineItemAbility.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility.ItemClass.Add(GetRandomItemClass());
		string itemName = string.Empty;
		int number = 1;
		byte itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility);
		FruitMachineItemAbility fruitMachineItemAbility2 = new FruitMachineItemAbility();
		fruitMachineItemAbility2.IconIndex = 1;
		fruitMachineItemAbility2.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility2.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility2.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility2.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility2);
		FruitMachineItemAbility fruitMachineItemAbility3 = new FruitMachineItemAbility();
		fruitMachineItemAbility3.IconIndex = 2;
		fruitMachineItemAbility3.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility3.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility3.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility3.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility3);
		FruitMachineItemAbility fruitMachineItemAbility4 = new FruitMachineItemAbility();
		fruitMachineItemAbility4.IconIndex = 3;
		fruitMachineItemAbility4.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility4.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility4.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility4.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility4);
		FruitMachineItemAbility fruitMachineItemAbility5 = new FruitMachineItemAbility();
		fruitMachineItemAbility5.IconIndex = 4;
		fruitMachineItemAbility5.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility5.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility5.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility5.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility5);
		FruitMachineItemAbility fruitMachineItemAbility6 = new FruitMachineItemAbility();
		fruitMachineItemAbility6.IconIndex = 5;
		fruitMachineItemAbility6.Quality = ItemQuality.Uncommon;
		fruitMachineItemAbility6.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(fruitMachineItemAbility6.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility6.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility6);
		FruitMachineItemAbility fruitMachineItemAbility7 = new FruitMachineItemAbility();
		fruitMachineItemAbility7.IconIndex = 6;
		fruitMachineItemAbility7.Quality = ItemQuality.Rare;
		fruitMachineItemAbility7.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility7.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility7.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility7);
		FruitMachineItemAbility fruitMachineItemAbility8 = new FruitMachineItemAbility();
		fruitMachineItemAbility8.IconIndex = 7;
		fruitMachineItemAbility8.Quality = ItemQuality.Rare;
		fruitMachineItemAbility8.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility8.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility8.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility8);
		FruitMachineItemAbility fruitMachineItemAbility9 = new FruitMachineItemAbility();
		fruitMachineItemAbility9.IconIndex = 8;
		fruitMachineItemAbility9.Quality = ItemQuality.Epic;
		fruitMachineItemAbility9.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility9.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility9.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility9);
		FruitMachineItemAbility fruitMachineItemAbility10 = new FruitMachineItemAbility();
		fruitMachineItemAbility10.IconIndex = 9;
		fruitMachineItemAbility10.Quality = ItemQuality.Epic;
		fruitMachineItemAbility10.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility10.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility10.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility10);
		FruitMachineItemAbility fruitMachineItemAbility11 = new FruitMachineItemAbility();
		fruitMachineItemAbility11.IconIndex = 10;
		fruitMachineItemAbility11.Quality = ItemQuality.Legendary;
		fruitMachineItemAbility11.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility11.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility11.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility11);
		FruitMachineItemAbility fruitMachineItemAbility12 = new FruitMachineItemAbility();
		fruitMachineItemAbility12.IconIndex = 11;
		fruitMachineItemAbility12.Quality = ItemQuality.Legendary;
		fruitMachineItemAbility12.ItemClass.Add(GetRandomItemClass());
		itemName = string.Empty;
		number = 1;
		itemType = 0;
		GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClassFromAll(fruitMachineItemAbility12.ItemClass[0], ref itemName, ref number, ref itemType);
		fruitMachineItemAbility12.Number.Add((byte)number);
		list.Add(fruitMachineItemAbility12);
		int index = Random.Range(9, 11);
		list[index].IsUnKnown = true;
		return list;
	}
}
