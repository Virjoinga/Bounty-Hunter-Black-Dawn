using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest : Quest
{
	public class ItemInfo
	{
		public short itemId;

		public byte itemNum;

		public string itemName;

		public QuestPoint m_point;
	}

	public List<ItemInfo> m_itemInfoLst = new List<ItemInfo>();

	public CollectionQuest(int id)
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
			string data = unitDataTable.GetData(contextId, 0 + i * 4, string.Empty, false);
			if (!string.IsNullOrEmpty(data))
			{
				ItemInfo itemInfo = new ItemInfo();
				itemInfo.itemId = GameConfig.GetInstance().specialItemConfigCallName[data].ID;
				itemInfo.itemName = GameConfig.GetInstance().specialItemConfigCallName[data].ItemName;
				itemInfo.itemNum = (byte)unitDataTable.GetData(contextId, 1 + i * 4, 0, false);
				itemInfo.m_point = new QuestPoint();
				itemInfo.m_point.m_siteId = (byte)unitDataTable.GetData(contextId, 2 + i * 4, 0, false);
				string data2 = unitDataTable.GetData(contextId, 3 + i * 4, string.Empty, false);
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
			if (item.itemNum > 0)
			{
				QuestState.Conter conter2 = new QuestState.Conter();
				conter2.m_id = item.itemId;
				conter2.m_name = item.itemName;
				Debug.Log("cont.m_name: " + conter2.m_name);
				conter2.m_type = QuestConterType.ITEM;
				conter2.m_currNum = userState.ItemInfoData.GetItemCountByID(item.itemId);
				conter2.m_maxNum = item.itemNum;
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
			if (item.itemNum > 0)
			{
				if (!conter.ContainsKey(item.itemId))
				{
					return false;
				}
				if (conter[item.itemId].m_currNum < item.itemNum)
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
			if (itemInfo.itemNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = ((conter != null && conter.ContainsKey(itemInfo.itemId)) ? (conter[itemInfo.itemId].m_currNum + "/" + itemInfo.itemNum) : (0 + "/" + itemInfo.itemNum));
					text = text.Replace(text2, empty);
				}
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
			if (itemInfo.itemNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = itemInfo.itemNum + "/" + itemInfo.itemNum;
					text = text.Replace(text2, empty);
				}
			}
		}
		return text;
	}

	public override string GetAccQuestDesc(Dictionary<int, QuestState.Conter> conter)
	{
		string text = LocalizationManager.GetInstance().GetString(m_accQuestDesc);
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			if (itemInfo.itemNum > 0)
			{
				string text2 = "<q>" + i + "<->";
				if (text.Contains(text2))
				{
					string empty = string.Empty;
					empty = conter[itemInfo.itemId].m_currNum + "/" + itemInfo.itemNum;
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
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (KeyValuePair<int, QuestState.Conter> item in questState.m_conter)
		{
			userState.ItemInfoData.RemoveItem((short)item.Value.m_id, item.Value.m_maxNum);
		}
	}

	public override void RemoveQuest(Dictionary<int, QuestState.Conter> conter)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (KeyValuePair<int, QuestState.Conter> item in conter)
		{
			userState.ItemInfoData.RemoveItem((short)item.Value.m_id, item.Value.m_currNum);
		}
		ResetQuestPoint();
	}

	private void ResetQuestPoint()
	{
		foreach (ItemInfo item in m_itemInfoLst)
		{
			if (item.itemNum <= 0)
			{
				continue;
			}
			foreach (QuestPoint.QuestPosition item2 in item.m_point.m_position)
			{
				item2.m_state = QuestPointState.enable;
			}
		}
	}

	public override Dictionary<string, QuestPoint> GetQuestPoint(QuestState questState)
	{
		Dictionary<string, QuestPoint> dictionary = new Dictionary<string, QuestPoint>();
		Dictionary<int, QuestState.Conter> conter = questState.m_conter;
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			if (itemInfo.itemNum <= 0)
			{
				continue;
			}
			if (conter.ContainsKey(itemInfo.itemId))
			{
				if (conter[itemInfo.itemId].m_currNum < itemInfo.itemNum)
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

	public override void SetQuestPoint(int id, int posIndex, QuestPointState state)
	{
		for (int i = 0; i < m_itemInfoLst.Count; i++)
		{
			ItemInfo itemInfo = m_itemInfoLst[i];
			if (itemInfo.itemNum > 0 && itemInfo.itemId == id)
			{
				if (posIndex < itemInfo.m_point.m_position.Count)
				{
					itemInfo.m_point.m_position[posIndex].SetState(state);
				}
				else
				{
					Debug.Log("Collection Quest - Point Error: " + m_id);
				}
			}
		}
	}
}
