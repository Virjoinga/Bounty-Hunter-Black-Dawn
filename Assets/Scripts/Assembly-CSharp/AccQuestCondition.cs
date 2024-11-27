using System.Collections.Generic;

public class AccQuestCondition : IAccQuestCondition
{
	public class ItemRequired
	{
		public short m_itemId;

		public byte m_itemNum;
	}

	public short m_prevQuestId;

	public byte m_levelRequired;

	public List<ItemRequired> m_itemRequiredLst = new List<ItemRequired>();

	public byte m_yearBegin;

	public byte m_yearEnd;

	public byte m_monthBegin;

	public byte m_monthEnd;

	public byte m_dayBegin;

	public byte m_dayEnd;

	public byte m_hourBegin;

	public byte m_hourEnd;

	public byte m_weekend;

	public bool CheckCondition()
	{
		if (CheckCondForPreQuest())
		{
			return true;
		}
		return false;
	}

	public bool CheckCondForPreQuest()
	{
		if (m_prevQuestId == 0)
		{
			return true;
		}
		UserState userState = GameApp.GetInstance().GetUserState();
		if (QuestManager.GetInstance().m_groupQuestLst.ContainsKey(m_prevQuestId))
		{
			List<Quest> list = QuestManager.GetInstance().m_groupQuestLst[m_prevQuestId];
			foreach (Quest item in list)
			{
				if (!userState.m_questStateContainer.CheckSubQuestCompleted(item))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void LoadConfig(int id)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29];
		m_prevQuestId = (short)unitDataTable.GetData(id, 11, 0, false);
		m_levelRequired = (byte)unitDataTable.GetData(id, 12, 0, false);
		m_itemRequiredLst.Clear();
		for (int i = 0; i < 3; i++)
		{
			ItemRequired itemRequired = new ItemRequired();
			itemRequired.m_itemId = (short)unitDataTable.GetData(id, 13 + i * 2, 0, false);
			itemRequired.m_itemNum = (byte)unitDataTable.GetData(id, 14 + i * 2, 0, false);
			m_itemRequiredLst.Add(itemRequired);
		}
		m_yearBegin = (byte)unitDataTable.GetData(id, 19, 0, false);
		m_yearEnd = (byte)unitDataTable.GetData(id, 20, 0, false);
		m_monthBegin = (byte)unitDataTable.GetData(id, 21, 0, false);
		m_monthEnd = (byte)unitDataTable.GetData(id, 22, 0, false);
		m_dayBegin = (byte)unitDataTable.GetData(id, 23, 0, false);
		m_dayEnd = (byte)unitDataTable.GetData(id, 24, 0, false);
		m_hourBegin = (byte)unitDataTable.GetData(id, 25, 0, false);
		m_hourEnd = (byte)unitDataTable.GetData(id, 26, 0, false);
		m_weekend = (byte)unitDataTable.GetData(id, 27, 0, false);
	}
}
