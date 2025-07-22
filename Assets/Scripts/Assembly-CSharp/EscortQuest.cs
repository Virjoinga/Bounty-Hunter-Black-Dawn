using System.Collections.Generic;

public class EscortQuest : Quest
{
	public short m_targetId;

	public byte m_mapId;

	public string m_triggerName;

	public EscortQuest(int id)
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
		m_targetId = Quest.GetPrimaryKey(data, 37);
		m_mapId = (byte)unitDataTable.GetData(contextId, 1, 0, false);
		m_triggerName = unitDataTable.GetData(contextId, 2, string.Empty, false);
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		AccQuestContent(conter);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		return true;
	}

	public override string GetDescription(Dictionary<int, QuestState.Conter> conter)
	{
		return string.Empty;
	}

	public override string GetDescription()
	{
		return string.Empty;
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
		return null;
	}

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
	}
}
