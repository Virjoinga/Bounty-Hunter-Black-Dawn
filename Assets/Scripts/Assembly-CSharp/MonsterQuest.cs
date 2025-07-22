using System.Collections.Generic;

public class MonsterQuest : Quest
{
	public class MonsterInfo
	{
		public short m_monsterId;

		public byte m_monsterNum;

		public string m_monsterName;

		public QuestPoint m_point;
	}

	public List<MonsterInfo> m_monsterInfoLst = new List<MonsterInfo>();

	public MonsterQuest(int id)
		: base(id)
	{
	}

	public override void LoadConfig(int id)
	{
		base.LoadConfig(id);
		int questType = (int)m_questType;
		int contextId = m_contextId;
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29 + questType];
		m_monsterInfoLst.Clear();
		for (int i = 0; i < 3; i++)
		{
			MonsterInfo monsterInfo = new MonsterInfo();
			monsterInfo.m_monsterId = (short)unitDataTable.GetData(contextId, 0 + i * 5, 0, false);
			monsterInfo.m_monsterName = unitDataTable.GetData(contextId, 1 + i * 5, string.Empty, false);
			monsterInfo.m_monsterNum = (byte)unitDataTable.GetData(contextId, 2 + i * 5, 0, false);
			monsterInfo.m_point = new QuestPoint();
			monsterInfo.m_point.m_siteId = (byte)unitDataTable.GetData(contextId, 3 + i * 5, 0, false);
			string data = unitDataTable.GetData(contextId, 4 + i * 5, string.Empty, false);
			monsterInfo.m_point.SetPos(data);
			m_monsterInfoLst.Add(monsterInfo);
		}
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
		foreach (MonsterInfo item in m_monsterInfoLst)
		{
			if (item.m_monsterNum > 0)
			{
				QuestState.Conter conter2 = new QuestState.Conter();
				conter2.m_id = item.m_monsterId;
				conter2.m_name = item.m_monsterName;
				conter2.m_type = QuestConterType.MONSTER;
				conter2.m_currNum = 0;
				conter2.m_maxNum = item.m_monsterNum;
				conter.Add(item.m_monsterId, conter2);
			}
		}
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		AccQuestContent(conter);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		foreach (MonsterInfo item in m_monsterInfoLst)
		{
			if (item.m_monsterNum > 0)
			{
				if (!conter.ContainsKey(item.m_monsterId))
				{
					return false;
				}
				if (conter[item.m_monsterId].m_currNum < item.m_monsterNum)
				{
					return false;
				}
			}
		}
		return true;
	}

	public override string GetDescription(Dictionary<int, QuestState.Conter> conter)
	{
		string text = LocalizationManager.GetInstance().GetString(m_details);
		for (int i = 0; i < m_monsterInfoLst.Count; i++)
		{
			MonsterInfo monsterInfo = m_monsterInfoLst[i];
			if (monsterInfo.m_monsterNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = ((conter != null && conter.ContainsKey(monsterInfo.m_monsterId)) ? (conter[monsterInfo.m_monsterId].m_currNum + "/" + monsterInfo.m_monsterNum) : (0 + "/" + monsterInfo.m_monsterNum));
					text = text.Replace(text2, empty);
				}
			}
		}
		return text;
	}

	public override string GetDescription()
	{
		string text = LocalizationManager.GetInstance().GetString(m_details);
		for (int i = 0; i < m_monsterInfoLst.Count; i++)
		{
			MonsterInfo monsterInfo = m_monsterInfoLst[i];
			if (monsterInfo.m_monsterNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = monsterInfo.m_monsterNum + "/" + monsterInfo.m_monsterNum;
					text = text.Replace(text2, empty);
				}
			}
		}
		return text;
	}

	public override string GetAccQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		string text = LocalizationManager.GetInstance().GetString(m_accQuestDesc);
		for (int i = 0; i < m_monsterInfoLst.Count; i++)
		{
			MonsterInfo monsterInfo = m_monsterInfoLst[i];
			if (monsterInfo.m_monsterNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = conter[monsterInfo.m_monsterId].m_currNum + "/" + monsterInfo.m_monsterNum;
					text = text.Replace(text2, empty);
				}
			}
		}
		return text;
	}

	public override string GetFinQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_finQuestDesc);
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
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		for (int i = 0; i < m_monsterInfoLst.Count; i++)
		{
			MonsterInfo monsterInfo = m_monsterInfoLst[i];
			if (monsterInfo.m_monsterNum > 0 && conter.ContainsKey(monsterInfo.m_monsterId))
			{
				string key = m_id + "_" + i;
				dictionary.Add(key, monsterInfo.m_point);
			}
		}
		return dictionary;
	}

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
	}
}
