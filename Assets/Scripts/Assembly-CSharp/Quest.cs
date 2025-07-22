using System;
using System.Collections.Generic;

public abstract class Quest
{
	public const byte REPEATABLE_QUEST = 127;

	public const byte NOT_REPEATABLE_QUEST = 0;

	public const string QUEST_LEVEL = "Lv.";

	public int m_id;

	public short m_commonId;

	public short m_groupId;

	public short m_subId;

	public byte m_subAttr;

	public byte m_difficult;

	public string m_name;

	public string m_desc;

	public string m_details;

	public byte m_period;

	public bool m_bFailRepeat;

	public QuestType m_questType;

	public short m_contextId;

	public IAccQuestCondition m_accCondition;

	public IQuestAward m_award;

	public byte m_LimitTime;

	public short m_accQuestNpcId;

	public string m_accQuestNpcTxt;

	public string m_unfinQuestNpcTxt;

	public short m_finQuestNpcId;

	public string m_finQuestNpcName;

	public string m_finQuestNpcTxt;

	public QuestAttr m_attr;

	public string m_accQuestDesc;

	public string m_finQuestDesc;

	public QuestEvent m_eventType;

	public byte m_eventContentId;

	public IQuestEvent m_questEvent;

	public Quest(int id)
	{
		m_id = id;
		m_accCondition = new AccQuestCondition();
		m_award = new QuestAward();
		m_finQuestNpcId = 0;
		m_accQuestNpcId = 0;
	}

	public virtual void LoadConfig(int id)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29];
		m_name = unitDataTable.GetData(id, 1, string.Empty, false);
		m_commonId = (short)unitDataTable.GetData(id, 2, 0, false);
		m_groupId = (short)unitDataTable.GetData(id, 3, 0, false);
		m_subId = (short)unitDataTable.GetData(id, 4, 0, false);
		m_subAttr = (byte)unitDataTable.GetData(id, 5, 0, false);
		m_difficult = (byte)unitDataTable.GetData(id, 6, 0, false);
		m_desc = unitDataTable.GetData(id, 7, string.Empty, false);
		m_details = unitDataTable.GetData(id, 8, string.Empty, false);
		m_period = (byte)unitDataTable.GetData(id, 9, 0, false);
		int data = unitDataTable.GetData(id, 10, 0, false);
		m_bFailRepeat = data == 1;
		((AccQuestCondition)m_accCondition).LoadConfig(id);
		((QuestAward)m_award).LoadConfig(id);
		m_contextId = (short)unitDataTable.GetData(id, 40, 0, false);
		m_LimitTime = (byte)unitDataTable.GetData(id, 41, 0, false);
		string data2 = unitDataTable.GetData(id, 42, string.Empty, false);
		m_accQuestNpcId = GetPrimaryKey(data2, 37);
		m_accQuestNpcTxt = unitDataTable.GetData(id, 43, string.Empty, false);
		m_unfinQuestNpcTxt = unitDataTable.GetData(id, 44, string.Empty, false);
		m_finQuestNpcName = unitDataTable.GetData(id, 45, string.Empty, false);
		m_finQuestNpcId = GetPrimaryKey(m_finQuestNpcName, 37);
		m_finQuestNpcTxt = unitDataTable.GetData(id, 46, string.Empty, false);
		byte attr = (byte)unitDataTable.GetData(id, 47, 0, false);
		m_attr = (QuestAttr)attr;
		m_accQuestDesc = unitDataTable.GetData(id, 50, string.Empty, false);
		m_finQuestDesc = unitDataTable.GetData(id, 51, string.Empty, false);
		byte eventType = (byte)unitDataTable.GetData(id, 48, 0, false);
		m_eventType = (QuestEvent)eventType;
		m_eventContentId = (byte)unitDataTable.GetData(id, 49, 0, false);
	}

	private void InitEvent()
	{
		if (m_eventType == QuestEvent.UnlockState)
		{
			m_questEvent = new QuestEventUnlockState();
			m_questEvent.LoadConfig(m_eventContentId);
		}
	}

	public bool CheckPeriod(DateTime date)
	{
		if (m_period == 127)
		{
			return true;
		}
		if (m_period == 0)
		{
			return false;
		}
		if ((DateTime.Now - date).TotalDays >= (double)(int)m_period)
		{
			return true;
		}
		return false;
	}

	public static short GetPrimaryKey(string foreighKey, byte dataTableId)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[dataTableId];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 1, string.Empty, false);
			if (data.Equals(foreighKey))
			{
				return (short)unitDataTable.GetData(i, 0, 0, false);
			}
		}
		return 0;
	}

	public QuestPoint GetSubmitPoint()
	{
		if (!string.IsNullOrEmpty(m_finQuestNpcName) && !m_finQuestNpcName.Equals("Null"))
		{
			return GameConfig.GetInstance().npcConfig[m_finQuestNpcName].m_point;
		}
		return null;
	}

	public void ExeQuestEvent()
	{
		if (m_questEvent != null)
		{
			m_questEvent.ExeEvent();
		}
	}

	public abstract Dictionary<string, QuestPoint> GetQuestPoint(QuestState questState);

	public abstract void AccQuestContent(Dictionary<int, QuestState.Conter> conter);

	public abstract void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter);

	public abstract bool IsCompletedQuest(QuestState questState);

	public abstract string GetDescription(Dictionary<int, QuestState.Conter> conter);

	public abstract string GetDescription();

	public abstract void QuestsSubmit(QuestState questState);

	public abstract string GetAccQuestDesc(Dictionary<int, QuestState.Conter> conter);

	public abstract string GetFinQuestDesc(Dictionary<int, QuestState.Conter> conter);

	public abstract void RemoveQuest(Dictionary<int, QuestState.Conter> conter);

	public abstract void SetQuestPoint(int id, int posIndex, QuestPointState state);
}
