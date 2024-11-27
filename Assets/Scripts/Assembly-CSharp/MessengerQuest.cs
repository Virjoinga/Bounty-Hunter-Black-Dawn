using System;
using System.Collections.Generic;
using UnityEngine;

public class MessengerQuest : Quest
{
	public class ItemInfo
	{
		public short itemId;

		public byte itemNum;
	}

	public List<ItemInfo> m_itemInfoLst = new List<ItemInfo>();

	public short m_targetId;

	public QuestPoint m_point;

	public string m_context;

	public MessengerQuest(int id)
		: base(id)
	{
	}

	public override void LoadConfig(int id)
	{
		base.LoadConfig(id);
		int questType = (int)m_questType;
		int contextId = m_contextId;
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[29 + questType];
		m_itemInfoLst.Clear();
		for (int i = 0; i < 3; i++)
		{
			string data = unitDataTable.GetData(contextId, 0 + i * 2, string.Empty, false);
			if (!string.IsNullOrEmpty(data))
			{
				ItemInfo itemInfo = new ItemInfo();
				Debug.Log("key: " + data);
				itemInfo.itemId = GameConfig.GetInstance().specialItemConfigCallName[data].ID;
				itemInfo.itemNum = (byte)unitDataTable.GetData(contextId, 1 + i * 2, 0, false);
				m_itemInfoLst.Add(itemInfo);
			}
		}
		string data2 = unitDataTable.GetData(contextId, 6, string.Empty, false);
		m_targetId = GameConfig.GetInstance().npcConfig[data2].m_id;
		m_point = new QuestPoint();
		m_point.m_siteId = (byte)unitDataTable.GetData(contextId, 7, 0, false);
		string data3 = unitDataTable.GetData(contextId, 8, string.Empty, false);
		m_point.SetPos(data3);
		m_context = unitDataTable.GetData(contextId, 9, string.Empty, false);
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemNum > 0)
			{
				QuestState.Conter conter2 = new QuestState.Conter();
				conter2.m_id = item.itemId;
				conter2.m_type = QuestConterType.ITEM;
				conter2.m_currNum = item.itemNum;
				conter2.m_maxNum = item.itemNum;
				conter.Add(item.itemId, conter2);
				GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem((short)conter2.m_id, false);
			}
		}
		QuestState.Conter conter3 = new QuestState.Conter();
		conter3.m_id = m_targetId;
		conter3.m_type = QuestConterType.NPC;
		conter3.m_currNum = 0;
		conter3.m_maxNum = 1;
		conter.Add(conter3.m_id, conter3);
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemNum > 0)
			{
				QuestState.Conter conter2 = new QuestState.Conter();
				conter2.m_id = item.itemId;
				conter2.m_type = QuestConterType.ITEM;
				conter2.m_currNum = item.itemNum;
				conter2.m_maxNum = item.itemNum;
				conter.Add(item.itemId, conter2);
			}
		}
		QuestState.Conter conter3 = new QuestState.Conter();
		conter3.m_id = m_targetId;
		conter3.m_type = QuestConterType.NPC;
		conter3.m_currNum = 0;
		conter3.m_maxNum = 1;
		conter.Add(conter3.m_id, conter3);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemNum > 0)
			{
				if (!conter.ContainsKey(item.itemId))
				{
					return false;
				}
				if (!userState.ItemInfoData.HaveItem(item.itemId, item.itemNum))
				{
					return false;
				}
			}
		}
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
		string text = string.Empty;
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemNum > 0)
			{
				string text2 = text;
				text = text2 + item.itemNum + "/" + item.itemNum + "\n";
			}
		}
		return text;
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
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (KeyValuePair<int, QuestState.Conter> item in questState.m_conter)
		{
			if (item.Value.m_type == QuestConterType.ITEM)
			{
				userState.ItemInfoData.RemoveItem((short)item.Value.m_id, item.Value.m_maxNum);
			}
		}
	}

	public override void RemoveQuest(Dictionary<int, QuestState.Conter> conter)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (KeyValuePair<int, QuestState.Conter> item in conter)
		{
			if (item.Value.m_type == QuestConterType.ITEM)
			{
				userState.ItemInfoData.RemoveItem((short)item.Value.m_id, item.Value.m_maxNum);
			}
		}
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
