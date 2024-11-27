using System.Collections.Generic;
using UnityEngine;

public class QuestAward : IQuestAward
{
	public class ItemAward
	{
		public short m_itemId;

		public byte m_itemNum;

		public byte m_itemNameNum;
	}

	public int m_exp;

	public int m_cash;

	public short m_mithril;

	public byte m_skillNum;

	public string m_unlockSceneName = string.Empty;

	public List<ItemAward> m_itemAwardLst = new List<ItemAward>();

	public void LoadConfig(int id)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29];
		m_exp = unitDataTable.GetData(id, 28, 0, false);
		m_cash = unitDataTable.GetData(id, 29, 0, false);
		m_itemAwardLst.Clear();
		for (int i = 0; i < 3; i++)
		{
			int num = (byte)unitDataTable.GetData(id, 31 + i * 2, 0, false);
			if (num <= 0)
			{
				continue;
			}
			ItemAward itemAward = new ItemAward();
			string data = unitDataTable.GetData(id, 30 + i * 2, string.Empty, false);
			if (GameConfig.GetInstance().specialItemConfigCallName.ContainsKey(data))
			{
				itemAward.m_itemId = GameConfig.GetInstance().specialItemConfigCallName[data].ID;
				itemAward.m_itemNum = (byte)num;
				itemAward.m_itemNameNum = 0;
				SpecialItemConfig specialItemConfig = GameConfig.GetInstance().specialItemConfig[itemAward.m_itemId];
				if (specialItemConfig.ItemNumber != 0)
				{
					byte itemNameNum = (byte)Random.Range(1, specialItemConfig.ItemNumber + 1);
					itemAward.m_itemNameNum = itemNameNum;
				}
				m_itemAwardLst.Add(itemAward);
			}
		}
		m_mithril = (short)unitDataTable.GetData(id, 36, 0, false);
		m_skillNum = (byte)unitDataTable.GetData(id, 37, 0, false);
		string data2 = unitDataTable.GetData(id, 38, string.Empty, false);
		m_unlockSceneName = data2;
	}

	public void GiveAward(byte level)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		userState.AddExp(m_exp);
		userState.AddCash(m_cash);
		GameApp.GetInstance().GetGlobalState().AddMithril(m_mithril);
		if (!string.IsNullOrEmpty(m_unlockSceneName))
		{
			GameApp.GetInstance().GetUserState().SetStageInstanceState(m_unlockSceneName, 1);
		}
		foreach (ItemAward item in m_itemAwardLst)
		{
			if (item.m_itemNum > 0)
			{
				GameApp.GetInstance().GetLootManager().GenerateQuestRewardItemAndPickUp(item.m_itemId, level, item.m_itemNameNum);
			}
		}
	}
}
