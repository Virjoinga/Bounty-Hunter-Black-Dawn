using System;
using System.Collections.Generic;
using UnityEngine;

public class UIQuestEntities : MonoBehaviour, InGameMenuListener, UIMsgListener
{
	public GameObject m_questTemplate;

	public GameObject m_separatorTemplate;

	public GameObject m_selectionTemplate;

	public GameObject m_questsContainer;

	public GameObject m_descriptionContainer;

	public GameObject m_acceptBtn;

	public GameObject m_submitBtn;

	public GameObject m_hurryBtn;

	protected GameObject m_separator;

	protected GameObject m_separator2;

	protected GameObject m_selection;

	protected Dictionary<int, GameObject> m_quests = new Dictionary<int, GameObject>();

	protected int m_selectedQuestID;

	protected int m_sortId;

	private void Awake()
	{
		m_selectedQuestID = 0;
		m_sortId = 0;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(m_acceptBtn);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickAccept));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_hurryBtn);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickHurry));
		UIEventListener uIEventListener3 = UIEventListener.Get(m_submitBtn);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickSubmit));
		InGameMenuManager.GetInstance().SetListener(this);
	}

	private void OnEnable()
	{
		if (!(UIQuest.m_instance != null))
		{
			return;
		}
		InitQuests();
		if (m_quests.Count <= 0)
		{
			return;
		}
		using (Dictionary<int, GameObject>.KeyCollection.Enumerator enumerator = m_quests.Keys.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				Selection(current, false);
			}
		}
	}

	public void InitQuests()
	{
		InitCanBeSubmitted();
		InitCanBeAccepted();
		InitHasBeenAccepted();
	}

	private void InitCanBeSubmitted()
	{
		List<QuestScript.QuestData> questData = UIQuest.m_instance.GetQuestData();
		UserState userState = GameApp.GetInstance().GetUserState();
		int count = questData.Count;
		Transform parent = m_questsContainer.transform;
		for (int i = 0; i < count; i++)
		{
			int id = questData[i].m_id;
			Quest quest = QuestManager.GetInstance().m_questLst[id];
			if ((questData[i].m_type != QuestScript.QuestAccType.Submit && questData[i].m_type != QuestScript.QuestAccType.AssignAndSubmit) || !userState.m_questStateContainer.CheckCanBeSubmitted(id))
			{
				continue;
			}
			if (m_quests.ContainsKey(quest.m_commonId))
			{
				UIQuestEntity component = m_quests[quest.m_commonId].GetComponent<UIQuestEntity>();
				if (component.GetPhase() == QuestPhase.Submit)
				{
					component.AddQuest((short)id);
				}
				continue;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(m_questTemplate) as GameObject;
			gameObject.transform.parent = parent;
			gameObject.name = Convert.ToString(m_sortId);
			Transform transform = gameObject.transform;
			UIQuestEntity component2 = gameObject.GetComponent<UIQuestEntity>();
			component2.SetId(quest.m_commonId);
			component2.SetPhase(QuestPhase.Submit);
			component2.ClearQuest();
			component2.AddQuest((short)id);
			component2.SetFlag(UIConstant.QUEST_ATTR_SPRITE[(int)quest.m_attr]);
			component2.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_CAN_SUBMIT]);
			component2.SetText(LocalizationManager.GetInstance().GetString(quest.m_name));
			component2.SetLevel("Lv." + quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(quest.m_difficult));
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			m_quests.Add(quest.m_commonId, gameObject);
			m_sortId++;
		}
	}

	private void InitCanBeAccepted()
	{
		List<QuestScript.QuestData> questData = UIQuest.m_instance.GetQuestData();
		UserState userState = GameApp.GetInstance().GetUserState();
		int count = questData.Count;
		Transform parent = m_questsContainer.transform;
		bool flag = true;
		int count2 = m_quests.Count;
		for (int i = 0; i < count; i++)
		{
			int id = questData[i].m_id;
			Quest quest = QuestManager.GetInstance().m_questLst[id];
			if ((questData[i].m_type != QuestScript.QuestAccType.Assign && questData[i].m_type != QuestScript.QuestAccType.AssignAndSubmit) || !userState.m_questStateContainer.CheckCanBeAccepted(id))
			{
				continue;
			}
			if (m_quests.ContainsKey(quest.m_commonId))
			{
				UIQuestEntity component = m_quests[quest.m_commonId].GetComponent<UIQuestEntity>();
				if (component.GetPhase() == QuestPhase.Inactive)
				{
					component.AddQuest((short)id);
				}
				continue;
			}
			if (flag && count2 > 0)
			{
				NGUITools.Destroy(m_separator);
				m_separator = NGUITools.AddChild(m_questsContainer, m_separatorTemplate);
				m_sortId = 500;
				m_separator.name = Convert.ToString(m_sortId);
				m_sortId++;
				flag = false;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(m_questTemplate) as GameObject;
			gameObject.transform.parent = parent;
			gameObject.name = Convert.ToString(m_sortId);
			Transform transform = gameObject.transform;
			UIQuestEntity component2 = gameObject.GetComponent<UIQuestEntity>();
			component2.SetId(quest.m_commonId);
			component2.SetPhase(QuestPhase.Inactive);
			Debug.Log("id12: " + id);
			component2.ClearQuest();
			component2.AddQuest((short)id);
			component2.SetFlag(UIConstant.QUEST_ATTR_SPRITE[(int)quest.m_attr]);
			component2.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_CAN_ACCEPT]);
			component2.SetText(LocalizationManager.GetInstance().GetString(quest.m_name));
			component2.SetLevel("Lv." + quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(quest.m_difficult));
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			m_quests.Add(quest.m_commonId, gameObject);
			m_sortId++;
		}
	}

	private void InitHasBeenAccepted()
	{
		List<QuestScript.QuestData> questData = UIQuest.m_instance.GetQuestData();
		UserState userState = GameApp.GetInstance().GetUserState();
		int count = questData.Count;
		Transform parent = m_questsContainer.transform;
		bool flag = true;
		Debug.Log("HasBeenAccepted Count " + count);
		int count2 = m_quests.Count;
		for (int i = 0; i < count; i++)
		{
			int id = questData[i].m_id;
			Quest quest = QuestManager.GetInstance().m_questLst[id];
			if ((questData[i].m_type != QuestScript.QuestAccType.Submit && questData[i].m_type != QuestScript.QuestAccType.AssignAndSubmit && !IsAutoSubmit(quest)) || !userState.m_questStateContainer.CheckHasBeenAcceptedWithCommonID(quest.m_commonId) || userState.m_questStateContainer.CheckCanBeSubmitted(id))
			{
				continue;
			}
			Debug.Log("Has Been Accepted....");
			if (m_quests.ContainsKey(quest.m_commonId))
			{
				UIQuestEntity component = m_quests[quest.m_commonId].GetComponent<UIQuestEntity>();
				if (component.GetPhase() == QuestPhase.Active)
				{
					component.AddQuest((short)id);
				}
				continue;
			}
			if (flag && count2 > 0)
			{
				NGUITools.Destroy(m_separator2);
				m_separator2 = NGUITools.AddChild(m_questsContainer, m_separatorTemplate);
				m_sortId = 1000;
				m_separator2.name = Convert.ToString(m_sortId);
				m_sortId++;
				flag = false;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(m_questTemplate) as GameObject;
			gameObject.transform.parent = parent;
			Transform transform = gameObject.transform;
			gameObject.name = Convert.ToString(m_sortId);
			UIQuestEntity component2 = gameObject.GetComponent<UIQuestEntity>();
			component2.SetId(quest.m_commonId);
			component2.SetPhase(QuestPhase.Active);
			component2.ClearQuest();
			component2.AddQuest((short)id);
			component2.SetFlag(UIConstant.QUEST_ATTR_SPRITE[(int)quest.m_attr]);
			component2.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_HAVE_ACCEPTED]);
			component2.SetText(LocalizationManager.GetInstance().GetString(quest.m_name));
			component2.SetLevel("Lv." + quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(quest.m_difficult));
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			m_quests.Add(quest.m_commonId, gameObject);
			m_sortId++;
		}
	}

	private bool IsAutoSubmit(Quest quest)
	{
		if (quest.m_subAttr == 1 && quest.m_finQuestNpcId == 0)
		{
			return true;
		}
		return false;
	}

	public void Selection(int commonId, bool immediate)
	{
		if ((commonId == m_selectedQuestID && !immediate) || (m_quests != null && !m_quests.ContainsKey(commonId)))
		{
			return;
		}
		m_selectedQuestID = commonId;
		foreach (Transform item in m_descriptionContainer.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		NGUITools.Destroy(m_selection);
		Debug.Log("m_selectedQuestID: " + m_selectedQuestID);
		m_selection = NGUITools.AddChild(m_quests[m_selectedQuestID], m_selectionTemplate);
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		UserState userState = GameApp.GetInstance().GetUserState();
		Transform parent = m_descriptionContainer.transform;
		if (component.GetPhase() == QuestPhase.Inactive)
		{
			Debug.Log("entity.GetQuests()[0]: " + component.GetQuests().Count);
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(component.GetQuests()[0]);
			string detail = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDetail(objFromQuestLst.m_commonId);
			UIQuest.m_instance.AddDetails(detail, parent);
			UIQuest.m_instance.AddRewards(objFromQuestLst, parent);
			NGUITools.SetActive(m_acceptBtn, true);
			NGUITools.SetActive(m_hurryBtn, false);
			NGUITools.SetActive(m_submitBtn, false);
		}
		else if (component.GetPhase() == QuestPhase.Active)
		{
			Quest objFromQuestLst2 = userState.m_questStateContainer.GetObjFromQuestLst(component.GetQuests()[0]);
			string progress = GameApp.GetInstance().GetUserState().m_questStateContainer.GetProgress(objFromQuestLst2.m_commonId);
			string detail2 = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDetail(objFromQuestLst2.m_commonId);
			UIQuest.m_instance.AddDetails(detail2, parent);
			UIQuest.m_instance.AddObjective(progress, parent);
			UIQuest.m_instance.AddRewards(objFromQuestLst2, parent);
			NGUITools.SetActive(m_acceptBtn, false);
			NGUITools.SetActive(m_hurryBtn, true);
			NGUITools.SetActive(m_submitBtn, false);
		}
		else
		{
			Quest objFromQuestLst3 = userState.m_questStateContainer.GetObjFromQuestLst(component.GetQuests()[0]);
			string progress2 = GameApp.GetInstance().GetUserState().m_questStateContainer.GetProgress(objFromQuestLst3.m_commonId);
			string detail3 = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDetail(objFromQuestLst3.m_commonId);
			UIQuest.m_instance.AddRewards(objFromQuestLst3, parent);
			UIQuest.m_instance.AddDetails(detail3, parent);
			UIQuest.m_instance.AddObjective(progress2, parent);
			NGUITools.SetActive(m_acceptBtn, false);
			NGUITools.SetActive(m_hurryBtn, false);
			NGUITools.SetActive(m_submitBtn, true);
		}
		UITableSort component2 = m_questsContainer.GetComponent<UITableSort>();
		component2.repositionNow = true;
		UITableSort component3 = m_descriptionContainer.GetComponent<UITableSort>();
		component3.repositionNow = true;
	}

	private void DestoryQuests()
	{
		foreach (int key in m_quests.Keys)
		{
			UnityEngine.Object.Destroy(m_quests[key]);
		}
		m_quests.Clear();
	}

	private bool CanBeExit(int filterId)
	{
		foreach (int key in m_quests.Keys)
		{
			if (filterId != key)
			{
				UIQuestEntity component = m_quests[key].GetComponent<UIQuestEntity>();
				if (component.GetPhase() != QuestPhase.Active)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void PrioritySelection()
	{
		foreach (int key in m_quests.Keys)
		{
			UIQuestEntity component = m_quests[key].GetComponent<UIQuestEntity>();
			if (component.GetPhase() == QuestPhase.Submit)
			{
				m_selectedQuestID = key;
				return;
			}
		}
		foreach (int key2 in m_quests.Keys)
		{
			UIQuestEntity component2 = m_quests[key2].GetComponent<UIQuestEntity>();
			if (component2.GetPhase() == QuestPhase.Inactive)
			{
				m_selectedQuestID = key2;
				break;
			}
		}
	}

	private void OnClickAccept(GameObject go)
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		if (component.GetPhase() != 0)
		{
			return;
		}
		UserState userState = GameApp.GetInstance().GetUserState();
		List<short> quests = component.GetQuests();
		for (int i = 0; i < quests.Count; i++)
		{
			if (userState.m_questStateContainer.AcceptQuest(quests[i]) != null && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				UpdatePlayerAccQuestsRequest request = new UpdatePlayerAccQuestsRequest(quests[i]);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		DestoryQuests();
		InitQuests();
		if (!CanBeExit(m_selectedQuestID))
		{
			PrioritySelection();
			Selection(m_selectedQuestID, true);
			return;
		}
		NGUITools.SetActive(UIQuest.m_instance.m_quests, false);
		for (int j = 0; j < quests.Count; j++)
		{
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(quests[j]);
			if (!string.IsNullOrEmpty(objFromQuestLst.m_accQuestNpcTxt))
			{
				UIBubble.m_text = LocalizationManager.GetInstance().GetString(objFromQuestLst.m_accQuestNpcTxt);
				break;
			}
		}
		if (UIQuest.m_instance.GetAcceptFrom() == QuestScript.AcceptFrom.Board || string.IsNullOrEmpty(UIBubble.m_text))
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
			UIQuest.m_instance.UpdateFlag();
		}
		else
		{
			UIBubble.m_npcId = UIQuest.m_npcId;
			UIBubble.m_bubbleState = NpcBubbleState.ASSIGNABLE_STATE;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(21, false, false, false);
		}
		InGameMenuManager.GetInstance().RemoveListener(this);
		InGameMenuManager.GetInstance().CloseInstantly();
		GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
	}

	private void OnClickHurry(GameObject go)
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		if (component.GetPhase() != QuestPhase.Active)
		{
			return;
		}
		UserState userState = GameApp.GetInstance().GetUserState();
		List<short> quests = component.GetQuests();
		NGUITools.SetActive(UIQuest.m_instance.m_quests, false);
		for (int i = 0; i < quests.Count; i++)
		{
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(quests[i]);
			if (!string.IsNullOrEmpty(objFromQuestLst.m_unfinQuestNpcTxt))
			{
				UIBubble.m_text = LocalizationManager.GetInstance().GetString(objFromQuestLst.m_unfinQuestNpcTxt);
				break;
			}
		}
		if (UIQuest.m_instance.GetAcceptFrom() == QuestScript.AcceptFrom.Board || string.IsNullOrEmpty(UIBubble.m_text))
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
			UIQuest.m_instance.UpdateFlag();
		}
		else
		{
			UIBubble.m_npcId = UIQuest.m_npcId;
			UIBubble.m_bubbleState = NpcBubbleState.TALK_STATE;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(21, false, false, false);
		}
		InGameMenuManager.GetInstance().RemoveListener(this);
		InGameMenuManager.GetInstance().CloseInstantly();
		GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
	}

	private void ShowBonus()
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		UserState userState = GameApp.GetInstance().GetUserState();
		UIQuestBonus component2 = UIQuest.m_instance.m_bonus.GetComponent<UIQuestBonus>();
		List<short> quests = component.GetQuests();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (num4 < quests.Count)
		{
			Debug.Log("quest id: " + quests[num4]);
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(quests[num4]);
			List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[objFromQuestLst.m_commonId];
			foreach (Quest item in list)
			{
				QuestAward questAward = (QuestAward)item.m_award;
				num += questAward.m_exp;
				num2 += questAward.m_cash;
				num3 += questAward.m_mithril;
				foreach (QuestAward.ItemAward item2 in questAward.m_itemAwardLst)
				{
					if (item2.m_itemNum > 0)
					{
						GameObject iconForSpecialItem = GameApp.GetInstance().GetLootManager().GetIconForSpecialItem(item2.m_itemId, item2.m_itemNameNum);
						component2.AddItem(iconForSpecialItem);
					}
				}
			}
		}
		component2.SetBonus(num2, num, num3);
		UIQuest.m_instance.m_bonus.SetActive(true);
	}

	private void OnClickSubmit(GameObject go)
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		if (component.GetPhase() == QuestPhase.Submit)
		{
			ShowBonus();
		}
	}

	public void Submit()
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		UserState userState = GameApp.GetInstance().GetUserState();
		List<short> quests = component.GetQuests();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			UpdatePlayerCmpQuestsRequest request = new UpdatePlayerCmpQuestsRequest(quests);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		for (int i = 0; i < quests.Count; i++)
		{
			Debug.Log("quest id: " + quests[i]);
			userState.m_questStateContainer.CompletedQuest(quests[i]);
			userState.m_questStateContainer.AutoAcceptSubQuest(quests[i]);
		}
		if (quests.Count > 0)
		{
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(quests[0]);
			userState.QuestInfo.SubmitQuest(objFromQuestLst.m_commonId);
			if (userState.m_questStateContainer.CheckQuestCompleted(objFromQuestLst))
			{
				AchievementTrigger achievementTrigger = null;
				if (objFromQuestLst.m_attr == QuestAttr.SUB_QUEST)
				{
					achievementTrigger = AchievementTrigger.Create(AchievementID.Anyone_need_help);
					achievementTrigger.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger);
				}
				achievementTrigger = AchievementTrigger.Create(AchievementID.Easy_Task, AchievementTrigger.Type.Start);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
				achievementTrigger = AchievementTrigger.Create(AchievementID.Easy_Task);
				achievementTrigger.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
			}
			if (GameApp.GetInstance().GetGameWorld().CheckFinalMainQuest(objFromQuestLst))
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GAME_FINISHED"), 2, 50);
				return;
			}
		}
		OnEndSubmitQuest();
	}

	private void OnEndSubmitQuest()
	{
		UIQuestEntity component = m_quests[m_selectedQuestID].GetComponent<UIQuestEntity>();
		UserState userState = GameApp.GetInstance().GetUserState();
		List<short> quests = component.GetQuests();
		DestoryQuests();
		InitQuests();
		if (!CanBeExit(m_selectedQuestID))
		{
			PrioritySelection();
			Selection(m_selectedQuestID, true);
			return;
		}
		NGUITools.SetActive(UIQuest.m_instance.m_quests, false);
		for (int i = 0; i < quests.Count; i++)
		{
			Quest objFromQuestLst = userState.m_questStateContainer.GetObjFromQuestLst(quests[i]);
			if (!string.IsNullOrEmpty(objFromQuestLst.m_finQuestNpcTxt))
			{
				UIBubble.m_text = LocalizationManager.GetInstance().GetString(objFromQuestLst.m_finQuestNpcTxt);
				break;
			}
		}
		if (UIQuest.m_instance.GetAcceptFrom() == QuestScript.AcceptFrom.Board || string.IsNullOrEmpty(UIBubble.m_text))
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
			UIQuest.m_instance.UpdateFlag();
		}
		else
		{
			UIBubble.m_npcId = UIQuest.m_npcId;
			UIBubble.m_bubbleState = NpcBubbleState.ASSIGNED_STATE;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(21, false, false, false);
		}
		InGameMenuManager.GetInstance().RemoveListener(this);
		InGameMenuManager.GetInstance().CloseInstantly();
		GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
		GameApp.GetInstance().Save();
	}

	public void OnCloseButtonClick()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			GameApp.GetInstance().GetUserState().m_questStateContainer.AutoAcceptSubQuestForNet();
		}
		GameApp.GetInstance().Save();
		GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 30)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 50)
		{
			UIMsgBox.instance.CloseMessage();
			OnEndSubmitQuest();
		}
	}
}
