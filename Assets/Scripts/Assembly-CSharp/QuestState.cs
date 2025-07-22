using System.Collections.Generic;

public class QuestState
{
	public class Conter
	{
		public int m_id;

		public QuestConterType m_type;

		public string m_name = string.Empty;

		public int m_currNum;

		public int m_maxNum;
	}

	public int m_id;

	public Quest m_quest;

	public QuestPhase m_status;

	public Dictionary<int, Conter> m_conter;

	public bool m_animFlag;

	public string GetDescription()
	{
		return m_quest.GetDescription(m_conter);
	}

	public string GetFinQuestDesc()
	{
		return m_quest.GetFinQuestDesc(m_conter);
	}

	public void RemoveQuest()
	{
		m_quest.RemoveQuest(m_conter);
	}

	public string GetAccQuestDesc()
	{
		return m_quest.GetAccQuestDesc(m_conter);
	}

	public bool IsCompletedQuest()
	{
		return m_quest.IsCompletedQuest(this);
	}

	public Dictionary<string, QuestPoint> GetQuestPoint()
	{
		return m_quest.GetQuestPoint(this);
	}
}
