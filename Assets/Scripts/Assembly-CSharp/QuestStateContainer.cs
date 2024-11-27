using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestStateContainer : IQuestProgress
{
	public List<QuestState> m_accStateLst = new List<QuestState>();

	public List<QuestCompleted> m_completedQuestLst = new List<QuestCompleted>();

	public void Init()
	{
		m_accStateLst.Clear();
		m_completedQuestLst.Clear();
	}

	public bool CheckIsClosed(int questId)
	{
		Quest objFromQuestLst = GetObjFromQuestLst(questId);
		QuestCompleted objFromCompletedQuestLst = GetObjFromCompletedQuestLst(questId);
		if (objFromCompletedQuestLst != null && objFromQuestLst.CheckPeriod(objFromCompletedQuestLst.m_dateTime))
		{
			return true;
		}
		return false;
	}

	public bool CheckHasBeenAccepted(int questId)
	{
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst != null)
		{
			return true;
		}
		return false;
	}

	public bool CheckHasBeenAcceptedWithCommonID(int CommonId)
	{
		int count = m_accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			if (m_accStateLst[i].m_quest.m_commonId == CommonId)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckCanBeAccepted(int questId)
	{
		bool flag = false;
		Quest objFromQuestLst = GetObjFromQuestLst(questId);
		if (objFromQuestLst == null)
		{
			Debug.Log("Missing Quest Data!!");
			return false;
		}
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst != null)
		{
			return false;
		}
		QuestCompleted objFromCompletedQuestLst = GetObjFromCompletedQuestLst(questId);
		if (objFromCompletedQuestLst != null && !objFromQuestLst.CheckPeriod(objFromCompletedQuestLst.m_dateTime))
		{
			return false;
		}
		if (objFromQuestLst.m_subAttr == 1)
		{
			List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[objFromQuestLst.m_commonId];
			foreach (Quest item in list)
			{
				if (item.m_id == objFromQuestLst.m_id || item.m_subId >= objFromQuestLst.m_subId || CheckSubQuestCompleted(item))
				{
					continue;
				}
				return false;
			}
		}
		if (objFromQuestLst.m_accCondition.CheckCondition())
		{
			return true;
		}
		return false;
	}

	public bool CheckCanBeSubmitted(int questId)
	{
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst != null && objFromAccStateLst.IsCompletedQuest())
		{
			return true;
		}
		return false;
	}

	public bool CheckCanBeSubmittedWithCommonId(short commonId)
	{
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[commonId];
		foreach (Quest item in list)
		{
			if (CheckSubQuestCompleted(item) || CheckCanBeSubmitted(item.m_id))
			{
				continue;
			}
			return false;
		}
		return true;
	}

	public bool CheckQuestCompletedWithCommonId(short commonId)
	{
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[commonId];
		foreach (Quest item in list)
		{
			if (!CheckSubQuestCompleted(item))
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckQuestCompleted(int questId)
	{
		Quest objFromQuestLst = GetObjFromQuestLst(questId);
		if (objFromQuestLst == null)
		{
			return false;
		}
		return CheckQuestCompleted(objFromQuestLst);
	}

	public bool CheckQuestCompleted(Quest quest)
	{
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[quest.m_commonId];
		foreach (Quest item in list)
		{
			if (!CheckSubQuestCompleted(item))
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckSubQuestCompleted(Quest quest)
	{
		if (GetObjFromCompletedQuestLst(quest.m_id) == null)
		{
			return false;
		}
		return true;
	}

	public bool CheckSubQuestCompleted(int questId)
	{
		if (GetObjFromCompletedQuestLst(questId) == null)
		{
			return false;
		}
		return true;
	}

	public QuestCompleted GetObjFromCompletedQuestLst(int questId)
	{
		int count = m_completedQuestLst.Count;
		for (int i = 0; i < count; i++)
		{
			if (m_completedQuestLst[i].m_id == questId)
			{
				return m_completedQuestLst[i];
			}
		}
		return null;
	}

	public bool CheckSubQuestAccepted(Quest quest)
	{
		if (GetObjFromAccStateLst(quest.m_id) == null)
		{
			return false;
		}
		return true;
	}

	public bool CheckQuestAccepted(int questId)
	{
		Quest objFromQuestLst = GetObjFromQuestLst(questId);
		if (objFromQuestLst == null)
		{
			return false;
		}
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[objFromQuestLst.m_commonId];
		foreach (Quest item in list)
		{
			if (CheckSubQuestAccepted(item))
			{
				return true;
			}
		}
		return false;
	}

	public QuestState GetObjFromAccStateLst(int questId)
	{
		int count = m_accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			if (m_accStateLst[i].m_id == questId)
			{
				return m_accStateLst[i];
			}
		}
		return null;
	}

	public QuestState GetObjFromAccStateLstWithCommonId(int commonId)
	{
		int count = m_accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			if (m_accStateLst[i].m_quest.m_commonId == commonId)
			{
				return m_accStateLst[i];
			}
		}
		return null;
	}

	public QuestState GetObjFromTeamAccStateLst(int questId)
	{
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst != null)
		{
			return objFromAccStateLst;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null && item.GetUserState() != null)
			{
				objFromAccStateLst = item.GetUserState().m_questStateContainer.GetObjFromAccStateLst(questId);
				if (objFromAccStateLst != null)
				{
					return objFromAccStateLst;
				}
			}
		}
		return null;
	}

	public Quest GetObjFromQuestLst(int questId)
	{
		Dictionary<int, Quest> questLst = QuestManager.GetInstance().m_questLst;
		if (questLst.ContainsKey(questId))
		{
			return questLst[questId];
		}
		return null;
	}

	public void AbandonQuest(short commonId)
	{
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		foreach (Quest item in commonQuestLst[commonId])
		{
			QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
			if (objFromAccStateLst != null)
			{
				AbandonSubQuest(objFromAccStateLst);
				continue;
			}
			QuestCompleted objFromCompletedQuestLst = GetObjFromCompletedQuestLst(item.m_id);
			if (objFromCompletedQuestLst != null)
			{
				AbandonSubQuest(objFromCompletedQuestLst);
			}
		}
	}

	public void AbandonSubQuest(QuestState questState)
	{
		questState.RemoveQuest();
		m_accStateLst.Remove(questState);
	}

	public void AbandonSubQuest(QuestCompleted questState)
	{
		m_completedQuestLst.Remove(questState);
	}

	public QuestState AcceptQuest(int questId)
	{
		if (GetObjFromAccStateLst(questId) == null)
		{
			Quest objFromQuestLst = GetObjFromQuestLst(questId);
			GameApp.GetInstance().GetUserState().QuestInfo.AccQuest(objFromQuestLst.m_commonId);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (GameApp.GetInstance().GetUserState().GetCurrentQuest() == 0)
				{
					GameApp.GetInstance().GetUserState().SetCurrentQuest(objFromQuestLst.m_commonId);
					if (Lobby.GetInstance().IsMasterPlayer)
					{
						Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
						UploadMarkQuest();
					}
				}
			}
			else if (GameApp.GetInstance().GetUserState().GetCurrentQuest() == 0)
			{
				GameApp.GetInstance().GetUserState().SetCurrentQuest(objFromQuestLst.m_commonId);
			}
			QuestState questState = new QuestState();
			questState.m_id = questId;
			questState.m_quest = objFromQuestLst;
			questState.m_status = QuestPhase.Active;
			questState.m_animFlag = false;
			questState.m_conter = new Dictionary<int, QuestState.Conter>();
			objFromQuestLst.AccQuestContent(questState.m_conter);
			if ((objFromQuestLst.m_questType == QuestType.Collection || objFromQuestLst.m_questType == QuestType.Messenger || objFromQuestLst.m_questType == QuestType.Discovery) && questState.IsCompletedQuest())
			{
				AutoSubmitSubQuest(objFromQuestLst);
				questState.m_status = QuestPhase.Submit;
			}
			m_accStateLst.Add(questState);
			return questState;
		}
		return null;
	}

	public void CompletedQuest(int questId)
	{
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst == null)
		{
			return;
		}
		objFromAccStateLst.m_quest.QuestsSubmit(objFromAccStateLst);
		objFromAccStateLst.m_quest.m_award.GiveAward(objFromAccStateLst.m_quest.m_difficult);
		m_accStateLst.Remove(objFromAccStateLst);
		QuestCompleted objFromCompletedQuestLst = GetObjFromCompletedQuestLst(questId);
		if (objFromCompletedQuestLst != null)
		{
			objFromCompletedQuestLst.m_dateTime = DateTime.Now;
		}
		else
		{
			QuestCompleted questCompleted = new QuestCompleted();
			questCompleted.m_id = questId;
			questCompleted.m_quest = objFromAccStateLst.m_quest;
			Debug.Log("questComp.m_quest: " + questCompleted.m_quest);
			questCompleted.m_groupId = objFromAccStateLst.m_quest.m_groupId;
			questCompleted.m_dateTime = default(DateTime);
			questCompleted.m_dateTime = DateTime.Now;
			m_completedQuestLst.Add(questCompleted);
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != objFromAccStateLst.m_quest.m_commonId)
			{
				return;
			}
			if (m_accStateLst.Count > 0)
			{
				if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckQuestCompletedWithCommonId(objFromAccStateLst.m_quest.m_commonId))
				{
					GameApp.GetInstance().GetUserState().SetCurrentQuest(m_accStateLst[0].m_quest.m_commonId);
				}
			}
			else
			{
				GameApp.GetInstance().GetUserState().SetCurrentQuest(0);
			}
			if (Lobby.GetInstance().IsMasterPlayer)
			{
				Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
				UploadMarkQuest();
			}
		}
		else
		{
			if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != objFromAccStateLst.m_quest.m_commonId)
			{
				return;
			}
			if (m_accStateLst.Count > 0)
			{
				if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckQuestCompletedWithCommonId(objFromAccStateLst.m_quest.m_commonId))
				{
					GameApp.GetInstance().GetUserState().SetCurrentQuest(m_accStateLst[0].m_quest.m_commonId);
				}
			}
			else
			{
				GameApp.GetInstance().GetUserState().SetCurrentQuest(0);
			}
		}
	}

	public QuestState AcceptQuestForRemotePlayer(int questId, byte questSubState)
	{
		QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
		if (objFromAccStateLst == null)
		{
			Quest objFromQuestLst = GetObjFromQuestLst(questId);
			QuestState questState = new QuestState();
			questState.m_id = questId;
			questState.m_quest = objFromQuestLst;
			questState.m_status = (QuestPhase)questSubState;
			questState.m_conter = new Dictionary<int, QuestState.Conter>();
			objFromQuestLst.AccQuestContentForRemotePlayer(questState.m_conter);
			m_accStateLst.Add(questState);
			Debug.Log("AcceptQuestForRemotePlayer: " + questId);
			return objFromAccStateLst;
		}
		return null;
	}

	public QuestCompleted CompletedQuestForRemotePlayer(int questId)
	{
		if (GetObjFromCompletedQuestLst(questId) == null)
		{
			Quest objFromQuestLst = GetObjFromQuestLst(questId);
			QuestCompleted questCompleted = new QuestCompleted();
			questCompleted.m_id = questId;
			questCompleted.m_quest = objFromQuestLst;
			m_completedQuestLst.Add(questCompleted);
			QuestState objFromAccStateLst = GetObjFromAccStateLst(questId);
			if (objFromAccStateLst != null)
			{
				m_accStateLst.Remove(objFromAccStateLst);
			}
			return questCompleted;
		}
		return null;
	}

	public bool QuestItemsAvailable(int itemId)
	{
		int count = m_accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			QuestState questState = m_accStateLst[i];
			if (questState.m_conter.ContainsKey(itemId))
			{
				return true;
			}
		}
		return false;
	}

	public bool QuestItemsAvailableForNet(int itemId)
	{
		if (QuestItemsAvailable(itemId))
		{
			return true;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item == null || item.GetUserState() == null)
			{
				continue;
			}
			List<QuestState> accStateLst = item.GetUserState().m_questStateContainer.m_accStateLst;
			int count = accStateLst.Count;
			for (int i = 0; i < count; i++)
			{
				QuestState questState = accStateLst[i];
				if (questState.m_conter.ContainsKey(itemId))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool QuestItemsEnough(int itemId)
	{
		int count = m_accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			QuestState questState = m_accStateLst[i];
			if (questState.m_conter.ContainsKey(itemId) && questState.m_conter[itemId].m_currNum >= questState.m_conter[itemId].m_maxNum)
			{
				return true;
			}
		}
		return false;
	}

	public Dictionary<int, List<QuestState>> MergeAccQuests()
	{
		Dictionary<int, List<QuestState>> dictionary = new Dictionary<int, List<QuestState>>();
		foreach (QuestState item in m_accStateLst)
		{
			Debug.Log("state.m_quest:" + item.m_quest.m_id);
			Debug.Log("state.m_quest.m_commonId:" + item.m_quest.m_commonId);
			if (dictionary.ContainsKey(item.m_quest.m_commonId))
			{
				dictionary[item.m_quest.m_commonId].Add(item);
				continue;
			}
			List<QuestState> list = new List<QuestState>();
			list.Add(item);
			dictionary.Add(item.m_quest.m_commonId, list);
		}
		return dictionary;
	}

	public Dictionary<int, List<QuestCompleted>> MergeCompQuests()
	{
		Dictionary<int, List<QuestCompleted>> dictionary = new Dictionary<int, List<QuestCompleted>>();
		foreach (QuestCompleted item in m_completedQuestLst)
		{
			Quest objFromQuestLst = GetObjFromQuestLst(item.m_id);
			Debug.Log("MergeCompQuests: " + objFromQuestLst.m_id);
			if (CheckQuestCompleted(objFromQuestLst))
			{
				if (dictionary.ContainsKey(objFromQuestLst.m_commonId))
				{
					dictionary[objFromQuestLst.m_commonId].Add(item);
					continue;
				}
				List<QuestCompleted> list = new List<QuestCompleted>();
				list.Add(item);
				dictionary.Add(objFromQuestLst.m_commonId, list);
			}
		}
		return dictionary;
	}

	public void AutoSubmitSubQuest(Quest quest)
	{
		if (quest.m_subAttr == 1 && quest.m_finQuestNpcId == 0)
		{
			CompletedQuest(quest.m_id);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				UpdatePlayerCmpQuestsRequest request = new UpdatePlayerCmpQuestsRequest((short)quest.m_id);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			AutoAcceptSubQuest(quest);
		}
	}

	public void AutoAcceptSubQuest(int questId)
	{
		Quest objFromQuestLst = GetObjFromQuestLst(questId);
		if (objFromQuestLst != null)
		{
			AutoAcceptSubQuest(objFromQuestLst);
		}
	}

	public void AutoAcceptSubQuest(Quest quest)
	{
		if (quest.m_subAttr != 1)
		{
			return;
		}
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[quest.m_commonId];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].m_id == quest.m_id || list[i].m_accQuestNpcId != 0 || (list[i].m_subId - quest.m_subId != 0 && list[i].m_subId - quest.m_subId != 1) || !CheckCanBeAccepted(list[i].m_id))
			{
				continue;
			}
			QuestState questState = AcceptQuest(list[i].m_id);
			if (questState == null)
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				UpdatePlayerAccQuestsRequest request = new UpdatePlayerAccQuestsRequest((short)questState.m_id);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				if (questState.m_quest.m_questType == QuestType.Collection)
				{
					GameApp.GetInstance().GetGameWorld().UpdateExplorableInBlockForNet((short)questState.m_id);
				}
			}
			else if (questState.m_quest.m_questType == QuestType.Collection)
			{
				GameApp.GetInstance().GetGameWorld().UpdateExplorableInBlock();
			}
		}
	}

	public void AutoAcceptSubQuestForNet()
	{
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		UserState userState = GameApp.GetInstance().GetUserState();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item == null)
			{
				continue;
			}
			UserState userState2 = item.GetUserState();
			if (userState2 == null)
			{
				continue;
			}
			List<QuestState> accStateLst = userState2.m_questStateContainer.m_accStateLst;
			List<int> list = new List<int>();
			for (int i = 0; i < accStateLst.Count; i++)
			{
				if (!userState.m_questStateContainer.CheckHasBeenAccepted(accStateLst[i].m_id) && userState.m_questStateContainer.CheckCanBeAccepted(accStateLst[i].m_id))
				{
					QuestState questState = userState.m_questStateContainer.AcceptQuest(accStateLst[i].m_id);
					list.Add(questState.m_id);
				}
			}
			if (list.Count > 0)
			{
				UpdatePlayerAccQuestsRequest request = new UpdatePlayerAccQuestsRequest(list);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
			}
		}
	}

	public bool IsFirstSubQuest(Quest quest)
	{
		if (quest.m_subAttr == 1)
		{
			if (quest.m_subId != 0)
			{
				return false;
			}
			if (CheckQuestAccepted(quest.m_id))
			{
				return false;
			}
			List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[quest.m_commonId];
			foreach (Quest item in list)
			{
				if (CheckSubQuestCompleted(item))
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	public bool IsFinalSubQuest(List<int> questIds)
	{
		Quest objFromQuestLst = GetObjFromQuestLst(questIds[0]);
		if (objFromQuestLst.m_subAttr == 1)
		{
			Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
			short commonId = objFromQuestLst.m_commonId;
			foreach (Quest item in commonQuestLst[commonId])
			{
				bool flag = false;
				for (int i = 0; i < questIds.Count; i++)
				{
					if (item.m_id == questIds[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag && !CheckSubQuestCompleted(item))
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	public string GetName(short questCommonId)
	{
		string empty = string.Empty;
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		if (!commonQuestLst.ContainsKey(questCommonId))
		{
			return LocalizationManager.GetInstance().GetString("MENU_NO_QUEST");
		}
		List<Quest> list = commonQuestLst[questCommonId];
		return LocalizationManager.GetInstance().GetString(list[0].m_name);
	}

	public string GetLevel(short questCommonId)
	{
		string empty = string.Empty;
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		if (!commonQuestLst.ContainsKey(questCommonId))
		{
			return "--";
		}
		List<Quest> list = commonQuestLst[questCommonId];
		return "Lv." + list[0].m_difficult;
	}

	public string GetDetail(short commonId)
	{
		string text = "[FFFFFF]";
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		foreach (Quest item in commonQuestLst[commonId])
		{
			if (!string.IsNullOrEmpty(item.m_desc))
			{
				text = text + LocalizationManager.GetInstance().GetString(item.m_desc) + "\n";
			}
		}
		if (text.Contains("\n"))
		{
			text = text.Substring(0, text.Length - 2);
		}
		return text + "[-]";
	}

	public string GetProgress(short commonId)
	{
		string text = string.Empty;
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		foreach (Quest item in commonQuestLst[commonId])
		{
			QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
			if (objFromAccStateLst != null)
			{
				text += objFromAccStateLst.GetDescription();
				text = ((objFromAccStateLst.m_status != QuestPhase.Submit) ? (text + "  /hollowtick") : (text + "  /tick"));
				text += "\n";
			}
			else if (GetObjFromCompletedQuestLst(item.m_id) != null)
			{
				text = text + item.GetDescription() + "  /tick";
				text += "\n";
			}
		}
		return text;
	}

	public string GetDescription(QuestState questState)
	{
		string detail = GetDetail(questState.m_quest.m_commonId);
		detail += "\n";
		return detail + GetProgress(questState.m_quest.m_commonId);
	}

	public string GetDescriptionForNotAccept(Quest quest)
	{
		return GetDetail(quest.m_commonId);
	}

	public void OnQuestProgressEnemyKill(int enemyGroupId)
	{
		for (int num = m_accStateLst.Count - 1; num >= 0; num--)
		{
			QuestState questState = m_accStateLst[num];
			Quest quest = questState.m_quest;
			if (quest.m_questType == QuestType.Monster && questState.m_conter.ContainsKey(enemyGroupId))
			{
				QuestState.Conter conter = questState.m_conter[enemyGroupId];
				if (conter != null && conter.m_currNum < conter.m_maxNum)
				{
					conter.m_currNum++;
					conter.m_currNum = Mathf.Clamp(conter.m_currNum, 0, conter.m_maxNum);
					if (conter.m_currNum >= conter.m_maxNum && questState.IsCompletedQuest())
					{
						questState.m_status = QuestPhase.Submit;
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							UpdatePlayerAccQuestSubStateRequest request = new UpdatePlayerAccQuestSubStateRequest((short)quest.m_id, (byte)questState.m_status);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
						AutoSubmitSubQuest(quest);
						UpdateQuestProgressForHUD();
					}
					if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != quest.m_commonId)
					{
						GameApp.GetInstance().GetUserState().SetCurrentQuest(quest.m_commonId);
						UpdateQuestProgressForHUD();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
						{
							Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
							UploadMarkQuest();
						}
					}
					UserStateHUD.GetInstance().InfoBox.PushMissionInfo(LocalizationManager.GetInstance().GetString(conter.m_name), conter.m_currNum, conter.m_maxNum);
					Debug.Log("Quest Progress Enemy Kill:  " + quest.m_name + "  " + conter.m_currNum + "/" + conter.m_maxNum);
				}
			}
		}
	}

	public void OnQuestProgressEnemyKillall(short questId)
	{
		Debug.Log("quest Id for enemy killall: " + questId);
		for (int num = m_accStateLst.Count - 1; num >= 0; num--)
		{
			QuestState questState = m_accStateLst[num];
			Quest quest = questState.m_quest;
			if (quest.m_questType == QuestType.Defend && questState.m_id == questId)
			{
				foreach (KeyValuePair<int, QuestState.Conter> item in questState.m_conter)
				{
					if (item.Value.m_type != QuestConterType.KILLALL)
					{
						continue;
					}
					QuestState.Conter value = item.Value;
					if (value.m_currNum >= value.m_maxNum)
					{
						continue;
					}
					value.m_currNum = value.m_maxNum;
					if (questState.IsCompletedQuest())
					{
						questState.m_status = QuestPhase.Submit;
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							UpdatePlayerAccQuestSubStateRequest request = new UpdatePlayerAccQuestSubStateRequest((short)quest.m_id, (byte)questState.m_status);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
						AutoSubmitSubQuest(quest);
						UpdateQuestProgressForHUD();
					}
					if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != quest.m_commonId)
					{
						GameApp.GetInstance().GetUserState().SetCurrentQuest(quest.m_commonId);
						UpdateQuestProgressForHUD();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
						{
							Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
							UploadMarkQuest();
						}
					}
					Debug.Log("Quest Progress Enemy Killall:  " + quest.m_name + "  " + value.m_currNum + "/" + value.m_maxNum);
				}
			}
		}
	}

	public void OnQuestProgressItemCollection(short itemId)
	{
		Debug.Log("QUEST ITEM ID--------" + itemId);
		UserState userState = GameApp.GetInstance().GetUserState();
		for (int num = m_accStateLst.Count - 1; num >= 0; num--)
		{
			QuestState questState = m_accStateLst[num];
			Quest quest = questState.m_quest;
			if ((quest.m_questType == QuestType.Collection || quest.m_questType == QuestType.Messenger || quest.m_questType == QuestType.Discovery) && questState.m_conter.ContainsKey(itemId))
			{
				QuestState.Conter conter = questState.m_conter[itemId];
				if (conter != null && conter.m_currNum < conter.m_maxNum)
				{
					conter.m_currNum = userState.ItemInfoData.GetItemCountByID(itemId);
					conter.m_currNum = Mathf.Clamp(conter.m_currNum, 0, conter.m_maxNum);
					if (conter.m_currNum >= conter.m_maxNum && questState.IsCompletedQuest())
					{
						questState.m_status = QuestPhase.Submit;
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							UpdatePlayerAccQuestSubStateRequest request = new UpdatePlayerAccQuestSubStateRequest((short)quest.m_id, (byte)questState.m_status);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
						if (GameApp.GetInstance().GetGameScene().mapType == MapType.City)
						{
							GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
						}
						AutoSubmitSubQuest(quest);
						UpdateQuestProgressForHUD();
					}
					if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != quest.m_commonId)
					{
						GameApp.GetInstance().GetUserState().SetCurrentQuest(quest.m_commonId);
						UpdateQuestProgressForHUD();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
						{
							Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
							UploadMarkQuest();
						}
					}
					UserStateHUD.GetInstance().InfoBox.PushMissionInfo(LocalizationManager.GetInstance().GetString(conter.m_name), conter.m_currNum, conter.m_maxNum);
					Debug.Log("Quest Progress Item Collect:  " + quest.m_name + "   " + conter.m_name + "  " + conter.m_currNum + "/" + conter.m_maxNum);
				}
			}
		}
	}

	public void OnQuestProgressTalkWithNPC(int npcId)
	{
		for (int num = m_accStateLst.Count - 1; num >= 0; num--)
		{
			QuestState questState = m_accStateLst[num];
			Quest quest = questState.m_quest;
			if ((quest.m_questType == QuestType.Dialog || quest.m_questType == QuestType.Messenger) && questState.m_conter.ContainsKey(npcId))
			{
				QuestState.Conter conter = questState.m_conter[npcId];
				if (conter != null && conter.m_currNum < conter.m_maxNum)
				{
					conter.m_currNum = conter.m_maxNum;
					if (conter.m_currNum >= conter.m_maxNum && questState.IsCompletedQuest())
					{
						questState.m_status = QuestPhase.Submit;
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							UpdatePlayerAccQuestSubStateRequest request = new UpdatePlayerAccQuestSubStateRequest((short)quest.m_id, (byte)questState.m_status);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
						if (GameApp.GetInstance().GetGameScene().mapType == MapType.City)
						{
							GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
						}
						AutoSubmitSubQuest(quest);
						UpdateQuestProgressForHUD();
					}
					if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != quest.m_commonId)
					{
						GameApp.GetInstance().GetUserState().SetCurrentQuest(quest.m_commonId);
						UpdateQuestProgressForHUD();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
						{
							Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
							UploadMarkQuest();
						}
					}
				}
			}
		}
	}

	public void UploadMarkQuest()
	{
		ChangeQuestMarkRequest request = new ChangeQuestMarkRequest(Lobby.GetInstance().GetCurrentMarkQuest());
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void UpdateQuestProgressForHUD(float delay)
	{
		UserStateHUD.GetInstance().SetMissionUpdateDelay(delay);
		UserStateHUD.GetInstance().UpdateMission();
	}

	public void UpdateQuestProgressForHUD()
	{
		UserStateHUD.GetInstance().SetMissionUpdateDelay(2f);
		UserStateHUD.GetInstance().UpdateMission();
	}

	public short GetQuestMark()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		Lobby.GetInstance().SetCurrentMarkQuest(currentQuest);
		return Lobby.GetInstance().GetCurrentMarkQuest();
	}

	public Dictionary<string, QuestPoint> GetCurrentQuestPoint()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		return GetQuestPoint(currentQuest);
	}

	public Dictionary<string, QuestPoint> GetMarkQuestPoint()
	{
		Dictionary<string, QuestPoint> dictionary = null;
		short currentMarkQuest = Lobby.GetInstance().GetCurrentMarkQuest();
		if (currentMarkQuest == 0)
		{
			return null;
		}
		return GetQuestPointForNet(currentMarkQuest);
	}

	private Dictionary<string, QuestPoint> GetQuestPoint(short commonid)
	{
		Dictionary<string, QuestPoint> dictionary = new Dictionary<string, QuestPoint>();
		if (commonid != 0)
		{
			Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
			if (commonQuestLst.ContainsKey(commonid))
			{
				List<Quest> list = commonQuestLst[commonid];
				foreach (Quest item in list)
				{
					QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
					if (objFromAccStateLst == null)
					{
						continue;
					}
					if (objFromAccStateLst.IsCompletedQuest())
					{
						QuestPoint submitPoint = item.GetSubmitPoint();
						if (submitPoint != null)
						{
							dictionary.Add(item.m_finQuestNpcName, submitPoint);
						}
						break;
					}
					return objFromAccStateLst.GetQuestPoint();
				}
			}
		}
		return dictionary;
	}

	private Dictionary<string, QuestPoint> GetQuestPointForNet(short commonid)
	{
		Dictionary<string, QuestPoint> dictionary = new Dictionary<string, QuestPoint>();
		if (commonid != 0)
		{
			Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
			if (commonQuestLst.ContainsKey(commonid))
			{
				List<Quest> list = commonQuestLst[commonid];
				foreach (Quest item in list)
				{
					QuestState objFromTeamAccStateLst = GetObjFromTeamAccStateLst(item.m_id);
					if (objFromTeamAccStateLst == null || CheckSubQuestCompleted(item.m_id))
					{
						continue;
					}
					if (objFromTeamAccStateLst.IsCompletedQuest())
					{
						QuestPoint submitPoint = item.GetSubmitPoint();
						if (submitPoint != null)
						{
							dictionary.Add(item.m_finQuestNpcName, submitPoint);
						}
						break;
					}
					return objFromTeamAccStateLst.GetQuestPoint();
				}
			}
		}
		return dictionary;
	}

	public string GetCurrentQuestProgress()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		return GetQuestProgress(currentQuest);
	}

	public string GetMarkQuestProgress()
	{
		short currentMarkQuest = Lobby.GetInstance().GetCurrentMarkQuest();
		string empty = string.Empty;
		if (currentMarkQuest == 0)
		{
			return empty;
		}
		if (!CheckHasBeenAcceptedWithCommonID(currentMarkQuest))
		{
			return LocalizationManager.GetInstance().GetString("LOC_QUEST_FOLLOW_HOST");
		}
		return GetQuestProgress(currentMarkQuest);
	}

	public QuestAttr GetCurrentQuestAttr()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		return GetQuestAttr(currentQuest);
	}

	private bool CanBeSubmittedProgress(short commonId)
	{
		List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[commonId];
		foreach (Quest item in list)
		{
			QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
			if (objFromAccStateLst == null)
			{
				if (GetObjFromCompletedQuestLst(item.m_id) == null)
				{
					return false;
				}
			}
			else if (objFromAccStateLst.m_status != QuestPhase.Submit)
			{
				return false;
			}
		}
		return true;
	}

	public short SwitchQuestProgress(short currentCommId)
	{
		List<short> list = new List<short>();
		foreach (QuestState item in m_accStateLst)
		{
			if (!list.Contains(item.m_quest.m_commonId))
			{
				list.Add(item.m_quest.m_commonId);
			}
		}
		if (list.Count <= 1)
		{
			return currentCommId;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (currentCommId == list[i])
			{
				num = i;
				break;
			}
		}
		num++;
		if (num >= list.Count)
		{
			num = 0;
		}
		return list[num];
	}

	public bool SwitchCurrentQuestProgress()
	{
		if (m_accStateLst.Count <= 1)
		{
			return false;
		}
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		short num = SwitchQuestProgress(currentQuest);
		if (num != currentQuest)
		{
			GameApp.GetInstance().GetUserState().SetCurrentQuest(num);
			return true;
		}
		return false;
	}

	public bool SwitchMarkQuestProgress()
	{
		if (m_accStateLst.Count <= 1 || !Lobby.GetInstance().IsMasterPlayer)
		{
			return false;
		}
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		short num = SwitchQuestProgress(currentQuest);
		if (num != currentQuest)
		{
			GameApp.GetInstance().GetUserState().SetCurrentQuest(num);
			Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
			UploadMarkQuest();
			return true;
		}
		return false;
	}

	public bool CanBeSubmittedCurrentQuestProgress()
	{
		foreach (QuestState item in m_accStateLst)
		{
			if (!item.m_animFlag)
			{
				List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[item.m_quest.m_commonId];
				if (CanBeSubmittedProgress(item.m_quest.m_commonId))
				{
					item.m_animFlag = true;
					return true;
				}
			}
		}
		return false;
	}

	public bool CanBeSubmittedMarkQuestProgress()
	{
		foreach (QuestState item in m_accStateLst)
		{
			if (!item.m_animFlag)
			{
				List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[item.m_quest.m_commonId];
				if (CanBeSubmittedProgress(item.m_quest.m_commonId))
				{
					item.m_animFlag = true;
					return true;
				}
			}
		}
		return false;
	}

	public QuestAttr GetMarkQuestAttr()
	{
		short currentMarkQuest = Lobby.GetInstance().GetCurrentMarkQuest();
		return GetQuestAttr(currentMarkQuest);
	}

	public QuestAttr GetQuestAttr(short commonid)
	{
		Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
		if (commonQuestLst.ContainsKey(commonid))
		{
			List<Quest> list = commonQuestLst[commonid];
			using (List<Quest>.Enumerator enumerator = list.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Quest current = enumerator.Current;
					return current.m_attr;
				}
			}
		}
		return QuestAttr.SUB_QUEST;
	}

	public string GetQuestProgress(short commonid)
	{
		string text = string.Empty;
		if (commonid != 0)
		{
			Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
			if (commonQuestLst.ContainsKey(commonid))
			{
				List<Quest> list = commonQuestLst[commonid];
				foreach (Quest item in list)
				{
					if (CheckHasBeenAccepted(item.m_id))
					{
						QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
						text = ((!objFromAccStateLst.IsCompletedQuest()) ? (text + " " + objFromAccStateLst.GetAccQuestDesc()) : (text + " " + objFromAccStateLst.GetFinQuestDesc()));
						break;
					}
				}
			}
		}
		return text;
	}

	public Dictionary<string, QuestPoint> GetCurrentQuestPointForPortal()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		return GetQuestPoint(currentQuest);
	}

	public Dictionary<string, QuestPoint> GetMarkQuestPointForPortal()
	{
		short currentMarkQuest = Lobby.GetInstance().GetCurrentMarkQuest();
		return GetQuestPoint(currentMarkQuest);
	}

	private Dictionary<string, QuestPoint> GetQuestPointForPortal(short commonid)
	{
		if (commonid != 0)
		{
			Dictionary<short, List<Quest>> commonQuestLst = QuestManager.GetInstance().m_commonQuestLst;
			if (commonQuestLst.ContainsKey(commonid))
			{
				List<Quest> list = commonQuestLst[commonid];
				foreach (Quest item in list)
				{
					if (CheckHasBeenAccepted(item.m_id))
					{
						QuestState objFromAccStateLst = GetObjFromAccStateLst(item.m_id);
						if (!objFromAccStateLst.IsCompletedQuest())
						{
							return objFromAccStateLst.GetQuestPoint();
						}
						break;
					}
				}
			}
		}
		return null;
	}

	private bool IsTriggerForRespawnEnemy(int questId, List<QuestState> accStateLst)
	{
		int count = accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			Quest quest = accStateLst[i].m_quest;
			if (quest.m_id == questId && (quest.m_questType == QuestType.Monster || quest.m_questType == QuestType.Defend || quest.m_questType == QuestType.Collection || quest.m_questType == QuestType.Discovery) && accStateLst[i].m_status == QuestPhase.Active)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsTriggerForRespawnEnemy(int questId)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (IsTriggerForRespawnEnemy(questId, m_accStateLst))
			{
				return true;
			}
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null && item.GetUserState() != null && IsTriggerForRespawnEnemy(questId, item.GetUserState().m_questStateContainer.m_accStateLst))
				{
					return true;
				}
			}
		}
		else if (IsTriggerForRespawnEnemy(questId, m_accStateLst))
		{
			return true;
		}
		return false;
	}

	public bool IsAciveQuest(int questId)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (IsAciveForQuest(questId, m_accStateLst))
			{
				return true;
			}
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null && item.GetUserState() != null && IsAciveForQuest(questId, item.GetUserState().m_questStateContainer.m_accStateLst))
				{
					return true;
				}
			}
		}
		else if (IsAciveForQuest(questId, m_accStateLst))
		{
			return true;
		}
		return false;
	}

	private bool IsAciveForQuest(int questId, List<QuestState> accStateLst)
	{
		int count = accStateLst.Count;
		for (int i = 0; i < count; i++)
		{
			Quest quest = accStateLst[i].m_quest;
			if (quest.m_id == questId && quest.m_questType == QuestType.Collection && accStateLst[i].m_status == QuestPhase.Active)
			{
				return true;
			}
		}
		return false;
	}
}
