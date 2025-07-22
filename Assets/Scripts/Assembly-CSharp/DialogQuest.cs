using System;
using System.Collections.Generic;

public class DialogQuest : Quest
{
	public short m_targetId;

	public QuestPoint m_point;

	public string m_context;

	public DialogQuest(int id)
		: base(id)
	{
	}

	public override void LoadConfig(int id)
	{
		base.LoadConfig(id);
		int questType = (int)m_questType;
		int contextId = m_contextId;
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29 + questType];
		string data = unitDataTable.GetData(contextId, 0, string.Empty, false);
		m_targetId = GameConfig.GetInstance().npcConfig[data].m_id;
		m_point = GameConfig.GetInstance().npcConfig[data].m_point;
		m_context = unitDataTable.GetData(contextId, 1, string.Empty, false);
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
		QuestState.Conter conter2 = new QuestState.Conter();
		conter2.m_id = m_targetId;
		conter2.m_type = QuestConterType.NPC;
		conter2.m_currNum = 0;
		conter2.m_maxNum = 1;
		conter.Add(conter2.m_id, conter2);
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		AccQuestContent(conter);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		if (conter.ContainsKey(m_targetId))
		{
			if (conter[m_targetId].m_currNum < conter[m_targetId].m_maxNum)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override string GetDescription(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_details);
	}

	public override string GetDescription()
	{
		return LocalizationManager.GetInstance().GetString(m_details);
	}

	public override string GetAccQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		return LocalizationManager.GetInstance().GetString(m_accQuestDesc);
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
		dictionary.Add(Convert.ToString(m_targetId), m_point);
		return dictionary;
	}

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
	}
}
