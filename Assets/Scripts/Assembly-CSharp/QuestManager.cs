using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
	public const short QUEST_0_2 = 101;

	public const short QUEST_N_0_2 = 126;

	public const int QUEST_ITEM_FRUITMACHINE = 10502;

	private static QuestManager instance;

	public Dictionary<int, Quest> m_questLst = new Dictionary<int, Quest>();

	public Dictionary<short, List<Quest>> m_groupQuestLst = new Dictionary<short, List<Quest>>();

	public Dictionary<short, List<Quest>> m_commonQuestLst = new Dictionary<short, List<Quest>>();

	public static QuestManager GetInstance()
	{
		if (instance == null)
		{
			instance = new QuestManager();
		}
		return instance;
	}

	public void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29];
		if (unitDataTable != null)
		{
			m_questLst.Clear();
			for (int i = 0; i < unitDataTable.sRows; i++)
			{
				QuestType data = (QuestType)unitDataTable.GetData(i, 39, 0, false);
				int data2 = unitDataTable.GetData(i, 0, 0, false);
				Quest quest = null;
				switch (data)
				{
				case QuestType.Monster:
					quest = new MonsterQuest(data2);
					break;
				case QuestType.Dialog:
					quest = new DialogQuest(data2);
					break;
				case QuestType.Messenger:
					quest = new MessengerQuest(data2);
					break;
				case QuestType.Escort:
					quest = new EscortQuest(data2);
					break;
				case QuestType.Collection:
					quest = new CollectionQuest(data2);
					break;
				case QuestType.Defend:
					quest = new DefendQuest(data2);
					break;
				case QuestType.Discovery:
					quest = new DiscoveryQuest(data2);
					break;
				default:
					Debug.Log("Unknow quest type!");
					break;
				}
				quest.m_questType = data;
				quest.LoadConfig(i);
				m_questLst.Add(data2, quest);
			}
		}
		InitGroupQuestList();
		InitCommonQuestList();
	}

	public void InitGroupQuestList()
	{
		foreach (int key in m_questLst.Keys)
		{
			Quest quest = m_questLst[key];
			short groupId = quest.m_groupId;
			if (m_groupQuestLst.ContainsKey(groupId))
			{
				m_groupQuestLst[groupId].Add(quest);
				continue;
			}
			List<Quest> list = new List<Quest>();
			list.Add(quest);
			m_groupQuestLst.Add(groupId, list);
		}
	}

	public void InitCommonQuestList()
	{
		foreach (int key in m_questLst.Keys)
		{
			Quest quest = m_questLst[key];
			short commonId = quest.m_commonId;
			if (m_commonQuestLst.ContainsKey(commonId))
			{
				m_commonQuestLst[commonId].Add(quest);
				continue;
			}
			List<Quest> list = new List<Quest>();
			list.Add(quest);
			m_commonQuestLst.Add(commonId, list);
		}
	}
}
