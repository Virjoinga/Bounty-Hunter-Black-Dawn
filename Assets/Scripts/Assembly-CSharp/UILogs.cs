using System;
using System.Collections.Generic;
using UnityEngine;

public class UILogs : MonoBehaviour, UIMsgListener
{
	protected enum QuestSheet
	{
		Accepted = 0,
		Completed = 1
	}

	public GameObject m_template;

	public GameObject m_selectionTemplate;

	public GameObject m_markTemplate;

	public GameObject m_accContainer;

	public GameObject m_compContainer;

	public GameObject m_acceptedSheet;

	public GameObject m_completedSheet;

	public GameObject m_accScroll;

	public GameObject m_compScroll;

	public GameObject m_descriptionContainer;

	private GameObject m_accSheetActived;

	private GameObject m_accSheetInactivated;

	private GameObject m_compSheetActived;

	private GameObject m_compSheetInactivated;

	public GameObject m_abandonBtn;

	public GameObject m_backBtn;

	protected List<List<GameObject>> m_logs = new List<List<GameObject>>();

	protected Dictionary<int, List<QuestState>> m_accStates;

	protected Dictionary<int, List<QuestCompleted>> m_compStates;

	protected GameObject m_selection;

	protected GameObject m_mark;

	protected QuestSheet m_curSheet;

	protected int m_selId;

	protected int m_curIdCompleted;

	private void Awake()
	{
		m_selId = -1;
		m_curIdCompleted = -1;
		m_curSheet = QuestSheet.Accepted;
		m_accSheetActived = m_acceptedSheet.transform.Find("Actived").gameObject;
		m_accSheetInactivated = m_acceptedSheet.transform.Find("Unactivated").gameObject;
		m_compSheetActived = m_completedSheet.transform.Find("Actived").gameObject;
		m_compSheetInactivated = m_completedSheet.transform.Find("Unactivated").gameObject;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(m_acceptedSheet);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickAccepted));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_completedSheet);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCompleted));
		UIEventListener uIEventListener3 = UIEventListener.Get(m_backBtn);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickBack));
		UIEventListener uIEventListener4 = UIEventListener.Get(m_abandonBtn);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickAbandon));
	}

	private void OnEnable()
	{
		if (UIQuest.m_instance != null)
		{
			CreateLogs();
		}
	}

	private void OnDisable()
	{
		DestoryLogs();
	}

	public void CreateLogs()
	{
		Debug.Log("m_selId: " + m_selId);
		if (!(m_template != null))
		{
			return;
		}
		m_curSheet = QuestSheet.Accepted;
		DestoryLogs();
		UserState userState = GameApp.GetInstance().GetUserState();
		m_accStates = userState.m_questStateContainer.MergeAccQuests();
		Debug.Log("states.Count: " + m_accStates.Count);
		Transform transform = m_accContainer.transform;
		List<GameObject> list = new List<GameObject>();
		foreach (KeyValuePair<int, List<QuestState>> accState in m_accStates)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_template) as GameObject;
			gameObject.transform.parent = transform;
			UIDragPanelContentsAlign component = gameObject.GetComponent<UIDragPanelContentsAlign>();
			Transform transform2 = gameObject.transform;
			component.draggablePanel = transform.parent.GetComponent<UIDraggablePanelAlign>();
			UILog component2 = gameObject.GetComponent<UILog>();
			component2.SetId(accState.Key);
			component2.SetFlag(UIConstant.QUEST_ATTR_SPRITE[(int)accState.Value[0].m_quest.m_attr]);
			if (userState.m_questStateContainer.CheckCanBeSubmittedWithCommonId(accState.Value[0].m_quest.m_commonId))
			{
				component2.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_CAN_SUBMIT]);
			}
			else
			{
				component2.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_HAVE_ACCEPTED]);
			}
			component2.SetLevel("Lv." + accState.Value[0].m_quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(accState.Value[0].m_quest.m_difficult));
			component2.SetText(LocalizationManager.GetInstance().GetString(accState.Value[0].m_quest.m_name));
			transform2.localPosition = Vector3.zero;
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			list.Insert(0, gameObject);
		}
		SortForName(list);
		m_logs.Add(list);
		m_compStates = userState.m_questStateContainer.MergeCompQuests();
		Transform transform3 = m_compContainer.transform;
		List<GameObject> list2 = new List<GameObject>();
		foreach (KeyValuePair<int, List<QuestCompleted>> compState in m_compStates)
		{
			Debug.Log("kvp.Value[0].Count: " + compState.Value.Count);
			Debug.Log("kvp.Value[0].m_quest: " + compState.Value[0].m_quest);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(m_template) as GameObject;
			gameObject2.transform.parent = transform3;
			UIDragPanelContentsAlign component3 = gameObject2.GetComponent<UIDragPanelContentsAlign>();
			Transform transform4 = gameObject2.transform;
			component3.draggablePanel = transform3.parent.GetComponent<UIDraggablePanelAlign>();
			UILog component4 = gameObject2.GetComponent<UILog>();
			component4.SetId(compState.Key);
			component4.SetFlag(UIConstant.QUEST_ATTR_SPRITE[(int)compState.Value[0].m_quest.m_attr]);
			component4.SetText(LocalizationManager.GetInstance().GetString(compState.Value[0].m_quest.m_name));
			component4.SetLevel("Lv." + compState.Value[0].m_quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(compState.Value[0].m_quest.m_difficult));
			component4.m_state.SetActive(false);
			transform4.localPosition = Vector3.zero;
			transform4.localRotation = Quaternion.identity;
			transform4.localScale = Vector3.one;
			list2.Insert(0, gameObject2);
		}
		SortForName(list2);
		m_logs.Add(list2);
		Selection(GameApp.GetInstance().GetUserState().GetCurrentQuest());
		if (m_logs[1].Count > 0)
		{
			List<GameObject> list3 = m_logs[1];
			UILog component5 = list3[0].GetComponent<UILog>();
			m_curIdCompleted = component5.GetId();
		}
		ShowAccSheet();
		HideCompSheet();
		UITableSort component6 = m_accContainer.GetComponent<UITableSort>();
		component6.repositionNow = true;
		UITableSort component7 = m_compContainer.GetComponent<UITableSort>();
		component7.repositionNow = true;
	}

	private void Update()
	{
	}

	private void SortForName(List<GameObject> lst)
	{
		int num = 0;
		foreach (GameObject item in lst)
		{
			item.name = Convert.ToString(num);
			num++;
		}
	}

	public void Selection(int commonId)
	{
		foreach (Transform item in m_descriptionContainer.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		NGUITools.Destroy(m_selection);
		NGUITools.Destroy(m_mark);
		int index = GetIndex(commonId);
		if (index >= m_logs[(int)m_curSheet].Count || index == -1)
		{
			NGUITools.SetActive(m_abandonBtn, false);
			NGUITools.SetActive(m_backBtn, false);
			UITableSort component = m_descriptionContainer.GetComponent<UITableSort>();
			component.repositionNow = true;
			return;
		}
		m_selId = index;
		List<GameObject> list = m_logs[(int)m_curSheet];
		UILog component2 = list[index].GetComponent<UILog>();
		m_selection = NGUITools.AddChild(list[index], m_selectionTemplate);
		Transform parent = m_descriptionContainer.transform;
		if (m_curSheet == QuestSheet.Accepted)
		{
			m_mark = NGUITools.AddChild(list[index], m_markTemplate);
			GameApp.GetInstance().GetUserState().SetCurrentQuest((short)commonId);
			List<QuestState> list2 = m_accStates[component2.GetId()];
			string detail = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDetail((short)commonId);
			string progress = GameApp.GetInstance().GetUserState().m_questStateContainer.GetProgress((short)commonId);
			UIQuest.m_instance.AddRewards(list2[0].m_quest, parent);
			UIQuest.m_instance.AddDetails(detail, parent);
			UIQuest.m_instance.AddObjective(progress, parent);
			NGUITools.SetActive(m_abandonBtn, true);
			NGUITools.SetActive(m_backBtn, false);
			UITableSort component3 = m_accContainer.GetComponent<UITableSort>();
			component3.repositionNow = true;
		}
		else
		{
			m_curIdCompleted = commonId;
			List<QuestCompleted> list3 = m_compStates[component2.GetId()];
			string descriptionForNotAccept = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDescriptionForNotAccept(list3[0].m_quest);
			UIQuest.m_instance.AddDetails(descriptionForNotAccept, parent);
			UIQuest.m_instance.AddRewards(list3[0].m_quest, parent);
			NGUITools.SetActive(m_abandonBtn, false);
			NGUITools.SetActive(m_backBtn, true);
			UITableSort component4 = m_compContainer.GetComponent<UITableSort>();
			component4.repositionNow = true;
		}
		UITableSort component5 = m_descriptionContainer.GetComponent<UITableSort>();
		component5.repositionNow = true;
	}

	public int GetIndex(int commonId)
	{
		List<GameObject> list = m_logs[(int)m_curSheet];
		for (int i = 0; i < list.Count; i++)
		{
			UILog component = list[i].GetComponent<UILog>();
			if (component.GetId() == commonId)
			{
				return i;
			}
		}
		return -1;
	}

	private void OnClickAccepted(GameObject go)
	{
		if (m_curSheet != 0)
		{
			m_curSheet = QuestSheet.Accepted;
			ShowAccSheet();
			HideCompSheet();
			Selection(GameApp.GetInstance().GetUserState().GetCurrentQuest());
			if (m_selId == -1 || m_selId >= m_logs[(int)m_curSheet].Count)
			{
				NGUITools.SetActive(m_backBtn, true);
				NGUITools.SetActive(m_abandonBtn, false);
			}
			else
			{
				NGUITools.SetActive(m_abandonBtn, true);
				NGUITools.SetActive(m_backBtn, false);
			}
			UITableSort component = m_accContainer.GetComponent<UITableSort>();
			component.repositionNow = true;
		}
	}

	private void OnClickBack(GameObject go)
	{
		InGameMenuManager.GetInstance().Close();
	}

	private void OnClickAbandon(GameObject go)
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MENU_MISSION_ABORT"), 3, 41);
	}

	private void OnClickCompleted(GameObject go)
	{
		if (m_curSheet != QuestSheet.Completed)
		{
			m_curSheet = QuestSheet.Completed;
			ShowCompSheet();
			HideAccSheet();
			Selection(m_curIdCompleted);
			NGUITools.SetActive(m_backBtn, true);
			NGUITools.SetActive(m_abandonBtn, false);
			UITableSort component = m_compContainer.GetComponent<UITableSort>();
			component.repositionNow = true;
		}
	}

	protected void ShowAccSheet()
	{
		NGUITools.SetActive(m_accContainer, true);
		NGUITools.SetActive(m_accSheetActived, true);
		NGUITools.SetActive(m_accSheetInactivated, false);
		m_accScroll.SetActive(true);
	}

	protected void ShowCompSheet()
	{
		NGUITools.SetActive(m_compContainer, true);
		NGUITools.SetActive(m_compSheetActived, true);
		NGUITools.SetActive(m_compSheetInactivated, false);
		m_compScroll.SetActive(true);
	}

	protected void HideAccSheet()
	{
		NGUITools.SetActive(m_accContainer, false);
		NGUITools.SetActive(m_accSheetActived, false);
		NGUITools.SetActive(m_accSheetInactivated, true);
		m_accScroll.SetActive(false);
	}

	protected void HideCompSheet()
	{
		NGUITools.SetActive(m_compContainer, false);
		NGUITools.SetActive(m_compSheetActived, false);
		NGUITools.SetActive(m_compSheetInactivated, true);
		m_compScroll.SetActive(false);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId != 41)
		{
			return;
		}
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			userState.m_questStateContainer.AbandonQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
			List<GameObject> list = m_logs[(int)m_curSheet];
			short currentQuest = userState.GetCurrentQuest();
			userState.QuestInfo.AbandonQuest(currentQuest);
			for (int i = 0; i < list.Count; i++)
			{
				UILog component = list[i].GetComponent<UILog>();
				if (component.GetId() == currentQuest)
				{
					UnityEngine.Object.Destroy(list[i]);
					list.RemoveAt(i);
					break;
				}
			}
			m_accStates.Remove(currentQuest);
			userState.SetCurrentQuest(0);
			if (userState.m_questStateContainer.m_accStateLst.Count > 0)
			{
				userState.SetCurrentQuest(userState.m_questStateContainer.m_accStateLst[0].m_quest.m_commonId);
				Selection(userState.GetCurrentQuest());
			}
			else
			{
				Selection(userState.GetCurrentQuest());
				NGUITools.SetActive(m_abandonBtn, false);
				NGUITools.SetActive(m_backBtn, true);
			}
			UITableSort component2 = m_accContainer.GetComponent<UITableSort>();
			component2.repositionNow = true;
			GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
		}
		UIMsgBox.instance.CloseMessage();
	}

	public void DestoryLogs()
	{
		foreach (List<GameObject> log in m_logs)
		{
			foreach (GameObject item in log)
			{
				UnityEngine.Object.Destroy(item);
			}
			log.Clear();
		}
		m_logs.Clear();
	}
}
