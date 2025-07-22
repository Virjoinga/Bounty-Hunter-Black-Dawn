using System;
using System.Collections.Generic;

public class DefendQuest : Quest
{
	public class TriggerInfo
	{
		public QuestPoint m_point;
	}

	public List<TriggerInfo> m_triInfoLst = new List<TriggerInfo>();

	public byte m_defendPoint;

	public short m_defendTarget;

	public short m_defendTime;

	public bool m_bKillall;

	public DefendQuest(int id)
		: base(id)
	{
	}

	public override void LoadConfig(int id)
	{
		base.LoadConfig(id);
		int questType = (int)m_questType;
		int contextId = m_contextId;
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29 + questType];
		m_defendPoint = (byte)unitDataTable.GetData(contextId, 0, 0, false);
		string data = unitDataTable.GetData(contextId, 1, string.Empty, false);
		m_defendTarget = 0;
		m_defendTime = (short)unitDataTable.GetData(contextId, 2, 0, false);
		m_triInfoLst.Clear();
		for (int i = 0; i < 2; i++)
		{
			int data2 = unitDataTable.GetData(contextId, 3 + i * 2, 0, false);
			if (data2 != 0)
			{
				TriggerInfo triggerInfo = new TriggerInfo();
				triggerInfo.m_point = new QuestPoint();
				triggerInfo.m_point.m_siteId = (byte)data2;
				string data3 = unitDataTable.GetData(contextId, 4 + i * 2, string.Empty, false);
				triggerInfo.m_point.SetPos(data3);
				m_triInfoLst.Add(triggerInfo);
			}
		}
		m_bKillall = false;
		int num = (byte)unitDataTable.GetData(contextId, 7, 0, false);
		if (num == 1)
		{
			m_bKillall = true;
		}
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
		if (m_defendTime > 0)
		{
			QuestState.Conter conter2 = new QuestState.Conter();
			conter2.m_id = 0;
			conter2.m_type = QuestConterType.TIME;
			conter2.m_currNum = 0;
			conter2.m_maxNum = m_defendTime;
			conter.Add(conter2.m_id, conter2);
		}
		if (m_bKillall)
		{
			QuestState.Conter conter3 = new QuestState.Conter();
			conter3.m_id = 0;
			conter3.m_type = QuestConterType.KILLALL;
			conter3.m_currNum = 0;
			conter3.m_maxNum = 1;
			conter.Add(conter3.m_id, conter3);
		}
		if (m_defendTarget != 0)
		{
			QuestState.Conter conter4 = new QuestState.Conter();
			conter4.m_id = m_defendTarget;
			conter4.m_type = QuestConterType.DEFEND;
			conter4.m_currNum = 1;
			conter4.m_maxNum = 1;
			conter.Add(conter4.m_id, conter4);
		}
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		AccQuestContent(conter);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		foreach (KeyValuePair<int, QuestState.Conter> item in conter)
		{
			if (item.Value.m_currNum < item.Value.m_maxNum)
			{
				return false;
			}
		}
		return true;
	}

	public override string GetAccQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_accQuestDesc);
	}

	public override string GetFinQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_finQuestDesc);
	}

	public override string GetDescription(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_details);
	}

	public override string GetDescription()
	{
		return LocalizationManager.GetInstance().GetString(m_details);
	}

	public override void QuestsSubmit(QuestState questState)
	{
		ExeQuestEvent();
	}

	public override void RemoveQuest(Dictionary<int, QuestState.Conter> conter)
	{
	}

	public override Dictionary<string, QuestPoint> GetQuestPoint(QuestState questState)
	{
		Dictionary<string, QuestPoint> dictionary = new Dictionary<string, QuestPoint>();
		for (int i = 0; i < m_triInfoLst.Count; i++)
		{
			dictionary.Add(Convert.ToString(i), m_triInfoLst[i].m_point);
		}
		return dictionary;
	}

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
	}
}
