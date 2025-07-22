using System.Collections.Generic;
using UnityEngine;

public class DiscoveryQuest : Quest
{
	public class ItemInfo
	{
		public short itemId;

		public QuestPoint m_point;

		public string itemName;
	}

	public List<ItemInfo> m_itemInfoLst = new List<ItemInfo>();

	public DiscoveryQuest(int id)
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
		for (int i = 0; i < 5; i++)
		{
			string data = unitDataTable.GetData(contextId, 2 + i * 3, string.Empty, false);
			if (!string.IsNullOrEmpty(data))
			{
				ItemInfo itemInfo = new ItemInfo();
				itemInfo.itemId = GameConfig.GetInstance().specialItemConfigCallName[data].ID;
				itemInfo.itemName = GameConfig.GetInstance().specialItemConfigCallName[data].ItemName;
				itemInfo.m_point = new QuestPoint();
				itemInfo.m_point.m_siteId = (byte)unitDataTable.GetData(contextId, 0 + i * 3, 0, false);
				string data2 = unitDataTable.GetData(contextId, 1 + i * 3, string.Empty, false);
				itemInfo.m_point.SetPos(data2);
				m_itemInfoLst.Add(itemInfo);
			}
		}
	}

	public override void AccQuestContent(Dictionary<int, QuestState.Conter> conter)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemId != 0)
			{
				QuestState.Conter conter2 = new QuestState.Conter();
				conter2.m_id = item.itemId;
				conter2.m_name = item.itemName;
				Debug.Log("AccQuestContent " + conter2.m_name);
				conter2.m_type = QuestConterType.ITEM;
				conter2.m_currNum = userState.ItemInfoData.GetItemCountByID(item.itemId);
				conter2.m_maxNum = 1;
				conter.Add(item.itemId, conter2);
			}
		}
		ResetQuestPoint();
	}

	public override void AccQuestContentForRemotePlayer(Dictionary<int, QuestState.Conter> conter)
	{
		AccQuestContent(conter);
	}

	public override bool IsCompletedQuest(QuestState questState)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemId != 0)
			{
				if (!conter.ContainsKey(item.itemId))
				{
					return false;
				}
				if (!userState.ItemInfoData.HaveItem(item.itemId, 1))
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
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			string text2 = "<q>" + i + "<->";
			if (text.Contains(text2))
			{
				string empty = string.Empty;
				empty = ((conter != null && conter.ContainsKey(itemInfo.itemId)) ? (conter[itemInfo.itemId].m_currNum + "/" + 1) : (0 + "/" + 1));
				text = text.Replace(text2, empty);
			}
		}
		return text;
	}

	public override string GetDescription()
	{
		string text = LocalizationManager.GetInstance().GetString(m_details);
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			string text2 = "<q>" + i + "<->";
			if (text.Contains(text2))
			{
				string empty = string.Empty;
				empty = 1 + "/" + 1;
				text = text.Replace(text2, empty);
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
				userState.ItemInfoData.RemoveItem((short)item.Value.m_id, item.Value.m_currNum);
			}
		}
		ResetQuestPoint();
	}

	public override Dictionary<string, QuestPoint> GetQuestPoint(QuestState questState)
	{
		Dictionary<string, QuestPoint> dictionary = new Dictionary<string, QuestPoint>();
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			if (itemInfo.itemId == 0)
			{
				continue;
			}
			if (conter.ContainsKey(itemInfo.itemId))
			{
				if (conter[itemInfo.itemId].m_currNum < 1)
				{
					string key = m_id + "_" + i;
					dictionary.Add(key, itemInfo.m_point);
				}
			}
			else
			{
				string key2 = m_id + "_" + i;
				dictionary.Add(key2, itemInfo.m_point);
			}
		}
		return dictionary;
	}

	private void ResetQuestPoint()
	{
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemId <= 0)
			{
				continue;
			}
			foreach (QuestPoint.QuestPosition item2 in item.m_point.m_position)
			{
				item2.m_state = QuestPointState.enable;
			}
		}
	}

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			if (itemInfo.itemId > 0 && itemInfo.itemId == id)
			{
				itemInfo.m_point.m_position[posIndex].SetState(state);
			}
		}
	}
}
