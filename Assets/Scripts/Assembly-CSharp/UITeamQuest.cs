using System;
using System.Collections.Generic;
using UnityEngine;

public class UITeamQuest : MonoBehaviour
{
	public GameObject m_questTemplate;

	public GameObject m_playerInfoTemplate;

	public GameObject m_markTemplate;

	public GameObject m_selectionTemplate;

	public GameObject m_separatorTemplate;

	public GameObject m_accContainer;

	public GameObject m_playerInfoContainer;

	public GameObject m_descriptionContainer;

	public GameObject m_detailChk;

	public GameObject m_playersChk;

	public GameObject m_backBtn;

	public GameObject m_descRoot;

	public GameObject m_playerInfoRoot;

	protected Dictionary<int, GameObject> m_teamQuest = new Dictionary<int, GameObject>();

	protected List<GameObject> m_playerInfo = new List<GameObject>();

	protected GameObject m_mark;

	protected GameObject m_selection;

	protected GameObject m_separator;

	protected int m_selectedQuestID;

	protected int m_sortIdAccepted;

	protected int m_sortIdNotAccepted;

	public List<GameObject> m_seat = new List<GameObject>();

	protected bool m_bUpdateAccContainer;

	private void Awake()
	{
		m_selectedQuestID = 0;
		m_sortIdAccepted = 0;
		m_sortIdNotAccepted = 1001;
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(m_backBtn);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBack));
	}

	private void OnEnable()
	{
		if (UIQuest.m_instance != null)
		{
			InitTeamQuests();
		}
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (m_bUpdateAccContainer)
		{
			UITableSort component = m_accContainer.GetComponent<UITableSort>();
			component.repositionNow = true;
			m_bUpdateAccContainer = false;
		}
	}

	private void OnDisable()
	{
	}

	private void DestoryTeamLogs()
	{
		foreach (int key in m_teamQuest.Keys)
		{
			UnityEngine.Object.Destroy(m_teamQuest[key]);
		}
		m_teamQuest.Clear();
		foreach (GameObject item in m_playerInfo)
		{
			UnityEngine.Object.Destroy(item);
		}
		m_playerInfo.Clear();
	}

	public void InitTeamQuests()
	{
		InitAccepted();
		InitNotAccepted();
		if (m_teamQuest.Count > 0)
		{
			Debug.Log("GameApp.GetInstance().GetUserState().GetCurrentQuest():" + GameApp.GetInstance().GetUserState().GetCurrentQuest());
			SetSelectedQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
		}
		m_bUpdateAccContainer = true;
	}

	public void InitAccepted()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item == null)
				{
					continue;
				}
				UserState userState2 = item.GetUserState();
				Dictionary<int, List<QuestState>> dictionary = userState2.m_questStateContainer.MergeAccQuests();
				foreach (KeyValuePair<int, List<QuestState>> item2 in dictionary)
				{
					if (userState.m_questStateContainer.CheckHasBeenAccepted(item2.Value[0].m_id) && !m_teamQuest.ContainsKey(item2.Key))
					{
						AddTeamQuestForAccepted(item2.Key, item2.Value[0]);
					}
				}
			}
		}
		UserState userState3 = GameApp.GetInstance().GetUserState();
		Dictionary<int, List<QuestState>> dictionary2 = userState3.m_questStateContainer.MergeAccQuests();
		foreach (KeyValuePair<int, List<QuestState>> item3 in dictionary2)
		{
			if (userState.m_questStateContainer.CheckHasBeenAccepted(item3.Value[0].m_id) && !m_teamQuest.ContainsKey(item3.Key))
			{
				AddTeamQuestForAccepted(item3.Key, item3.Value[0]);
			}
		}
	}

	public void InitNotAccepted()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		bool flag = true;
		int count = m_teamQuest.Count;
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item == null)
				{
					continue;
				}
				UserState userState2 = item.GetUserState();
				Dictionary<int, List<QuestState>> dictionary = userState2.m_questStateContainer.MergeAccQuests();
				foreach (KeyValuePair<int, List<QuestState>> item2 in dictionary)
				{
					if (!userState.m_questStateContainer.CheckHasBeenAccepted(item2.Value[0].m_id) && !m_teamQuest.ContainsKey(item2.Key))
					{
						AddTeamQuestForNotAccepted(item2.Key, item2.Value[0]);
						if (flag && count > 0)
						{
							m_separator = NGUITools.AddChild(m_accContainer, m_separatorTemplate);
							m_separator.name = "1000";
							flag = false;
						}
					}
				}
			}
		}
		UserState userState3 = GameApp.GetInstance().GetUserState();
		Dictionary<int, List<QuestState>> dictionary2 = userState3.m_questStateContainer.MergeAccQuests();
		foreach (KeyValuePair<int, List<QuestState>> item3 in dictionary2)
		{
			if (!userState.m_questStateContainer.CheckHasBeenAccepted(item3.Value[0].m_id) && !m_teamQuest.ContainsKey(item3.Key))
			{
				AddTeamQuestForNotAccepted(item3.Key, item3.Value[0]);
				if (flag && count > 0)
				{
					m_separator = NGUITools.AddChild(m_accContainer, m_separatorTemplate);
					m_separator.name = "1000";
					flag = false;
				}
			}
		}
	}

	public void SetSelectedQuest(int commonId)
	{
		Debug.Log("m_selectedQuestID: " + m_selectedQuestID);
		if (m_selectedQuestID == commonId)
		{
			return;
		}
		if (m_teamQuest.ContainsKey(m_selectedQuestID))
		{
			UITeamQuestActive component = m_teamQuest[m_selectedQuestID].GetComponent<UITeamQuestActive>();
			NGUITools.SetActive(component.m_owner, false);
		}
		NGUITools.Destroy(m_selection);
		m_selectedQuestID = commonId;
		if (m_selectedQuestID == 0 || !m_teamQuest.ContainsKey(commonId))
		{
			return;
		}
		ResetPlayerInfo();
		ResetQuestDescription();
		UITeamQuestActive component2 = m_teamQuest[m_selectedQuestID].GetComponent<UITeamQuestActive>();
		m_selection = NGUITools.AddChild(m_teamQuest[m_selectedQuestID], m_selectionTemplate);
		NGUITools.SetActive(component2.m_owner, true);
		if (GameApp.GetInstance().GetGameMode().IsCoopMode() && GameApp.GetInstance().GetUserState().m_questStateContainer.CheckHasBeenAcceptedWithCommonID((short)commonId))
		{
			GameApp.GetInstance().GetUserState().SetCurrentQuest((short)commonId);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
			{
				Lobby.GetInstance().SetCurrentMarkQuest((short)commonId);
			}
			SetMarkQuest(Lobby.GetInstance().GetCurrentMarkQuest());
		}
		m_bUpdateAccContainer = true;
		UICheckbox component3 = m_detailChk.GetComponent<UICheckbox>();
		if (component3.isChecked)
		{
			OnActivateDetails(true);
		}
		else
		{
			OnActivatePlayerInfo(true);
		}
	}

	public void SetMarkQuest(int commonId)
	{
		NGUITools.Destroy(m_mark);
		if (commonId != 0 && m_teamQuest.ContainsKey(commonId))
		{
			UITeamQuestActive component = m_teamQuest[commonId].GetComponent<UITeamQuestActive>();
			m_mark = NGUITools.AddChild(m_teamQuest[commonId].transform.gameObject, m_markTemplate);
			m_mark.transform.localPosition = Vector3.zero;
		}
	}

	public void UpdateMarkQuest(int commonId)
	{
		Debug.Log("net commonId: " + commonId);
		SetMarkQuest(commonId);
		m_bUpdateAccContainer = true;
	}

	public void UpdateCurrentQuest()
	{
		if (m_teamQuest.ContainsKey(m_selectedQuestID))
		{
			UITeamQuestActive component = m_teamQuest[m_selectedQuestID].GetComponent<UITeamQuestActive>();
			SetOwner(component, component.m_id);
			ResetPlayerInfo();
			ResetQuestDescription();
			NGUITools.SetActive(component.m_owner, true);
			UICheckbox component2 = m_detailChk.GetComponent<UICheckbox>();
			if (component2.isChecked)
			{
				OnActivateDetails(true);
			}
			else
			{
				OnActivatePlayerInfo(true);
			}
		}
	}

	public void ResetQuestDescription()
	{
		foreach (Transform item in m_descriptionContainer.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		if (m_selectedQuestID != 0)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			QuestState objFromAccStateLstWithCommonId = userState.m_questStateContainer.GetObjFromAccStateLstWithCommonId(m_selectedQuestID);
			Transform parent = m_descriptionContainer.transform;
			if (objFromAccStateLstWithCommonId != null)
			{
				string detail = GameApp.GetInstance().GetUserState().m_questStateContainer.GetDetail(objFromAccStateLstWithCommonId.m_quest.m_commonId);
				string progress = GameApp.GetInstance().GetUserState().m_questStateContainer.GetProgress(objFromAccStateLstWithCommonId.m_quest.m_commonId);
				UIQuest.m_instance.AddRewards(objFromAccStateLstWithCommonId.m_quest, parent);
				UIQuest.m_instance.AddDetails(detail, parent);
				UIQuest.m_instance.AddObjective(progress, parent);
			}
			else
			{
				List<Quest> list = QuestManager.GetInstance().m_commonQuestLst[(short)m_selectedQuestID];
				string @string = LocalizationManager.GetInstance().GetString("LOC_QUEST_FOLLOW_HOST_DETAILS");
				UIQuest.m_instance.AddDetails(@string, parent);
			}
			UITableSort component = m_descriptionContainer.GetComponent<UITableSort>();
			component.repositionNow = true;
		}
	}

	public void ResetPlayerInfo()
	{
		foreach (GameObject item in m_playerInfo)
		{
			UnityEngine.Object.Destroy(item);
		}
		m_playerInfo.Clear();
		if (m_selectedQuestID != 0)
		{
			Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			AddPlayerInfo(localPlayer);
			Debug.Log("player.seat: " + localPlayer.GetSeatID());
			if (GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
				foreach (RemotePlayer item2 in remotePlayers)
				{
					if (item2 != null)
					{
						AddPlayerInfo(item2);
						Debug.Log("p.seat: " + item2.GetSeatID());
					}
				}
			}
		}
		UIGridX component = m_playerInfoContainer.GetComponent<UIGridX>();
		component.repositionNow = true;
	}

	public void AddPlayerInfo(Player player)
	{
		Transform parent = m_playerInfoContainer.transform;
		GameObject gameObject = UnityEngine.Object.Instantiate(m_playerInfoTemplate) as GameObject;
		Transform transform = gameObject.transform;
		UIPlayerQuestInfo component = gameObject.GetComponent<UIPlayerQuestInfo>();
		if (player.IsLocal())
		{
			gameObject.name = "0";
		}
		else
		{
			gameObject.name = Convert.ToString(player.GetSeatID() + 1);
		}
		UserState userState = player.GetUserState();
		component.SetPlayerInfo(player.GetUserID(), player.GetDisplayName(), userState.GetCharLevel(), (int)userState.GetCharacterClass(), player.GetSeatID());
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		m_playerInfo.Add(gameObject);
	}

	public void AddTeamQuestForAccepted(int id, QuestState quest)
	{
		Transform parent = m_accContainer.transform;
		GameObject gameObject = UnityEngine.Object.Instantiate(m_questTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = Convert.ToString(m_sortIdAccepted);
		Transform transform = gameObject.transform;
		UITeamQuestActive component = gameObject.GetComponent<UITeamQuestActive>();
		component.m_id = quest.m_quest.m_commonId;
		component.SetText(LocalizationManager.GetInstance().GetString(quest.m_quest.m_name));
		component.SetBackground(UIConstant.QUEST_ATTR_SPRITE[(int)quest.m_quest.m_attr]);
		component.SetLevel("Lv." + quest.m_quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(quest.m_quest.m_difficult));
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckCanBeSubmittedWithCommonId(quest.m_quest.m_commonId))
		{
			component.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_CAN_SUBMIT]);
		}
		else
		{
			component.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_HAVE_ACCEPTED]);
		}
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		SetOwner(component, quest.m_quest.m_commonId);
		m_teamQuest.Add(id, gameObject);
		m_sortIdAccepted++;
	}

	public void AddTeamQuestForNotAccepted(int id, QuestState quest)
	{
		Transform parent = m_accContainer.transform;
		GameObject gameObject = UnityEngine.Object.Instantiate(m_questTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = Convert.ToString(m_sortIdNotAccepted);
		Transform transform = gameObject.transform;
		UITeamQuestActive component = gameObject.GetComponent<UITeamQuestActive>();
		component.m_id = quest.m_quest.m_commonId;
		component.SetText(LocalizationManager.GetInstance().GetString(quest.m_quest.m_name));
		component.SetBackground(UIConstant.QUEST_ATTR_SPRITE[(int)quest.m_quest.m_attr]);
		component.SetState(UIConstant.QUEST_STATE_SPRITE[UIConstant.QUEST_STATE_CAN_ACCEPT]);
		component.SetLevel("Lv." + quest.m_quest.m_difficult, UIQuest.m_instance.GetColorForQuestDifficulty(quest.m_quest.m_difficult));
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		SetOwner(component, quest.m_quest.m_commonId);
		m_teamQuest.Add(id, gameObject);
		m_sortIdNotAccepted++;
	}

	public void AddTeamQuest(int id, QuestState quest, bool canBeAccepted)
	{
		if (m_teamQuest.ContainsKey(id))
		{
			return;
		}
		if (canBeAccepted)
		{
			AddTeamQuestForAccepted(id, quest);
			if (m_sortIdNotAccepted > 1001 && m_separator == null)
			{
				m_separator = NGUITools.AddChild(m_accContainer, m_separatorTemplate);
				m_separator.name = "1000";
			}
		}
		else
		{
			if (m_sortIdAccepted > 0 && m_separator == null)
			{
				m_separator = NGUITools.AddChild(m_accContainer, m_separatorTemplate);
				m_separator.name = "1000";
			}
			AddTeamQuestForNotAccepted(id, quest);
		}
		if (m_selectedQuestID == 0)
		{
			SetSelectedQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
		}
		m_bUpdateAccContainer = true;
	}

	public void SetOwner(UITeamQuestActive active, int commonId)
	{
		active.ClearOwner();
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			return;
		}
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		UserState userState = localPlayer.GetUserState();
		GameObject gameObject = UnityEngine.Object.Instantiate(m_seat[localPlayer.GetSeatID()]) as GameObject;
		gameObject.name = "0";
		gameObject.transform.localPosition = new Vector3(0f, 0f, -4f);
		active.AddOwner(gameObject);
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null)
			{
				UserState userState2 = item.GetUserState();
				if (userState2.m_questStateContainer.CheckHasBeenAcceptedWithCommonID(commonId))
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(m_seat[item.GetSeatID()]) as GameObject;
					gameObject2.name = Convert.ToString(item.GetSeatID() + 1);
					gameObject2.transform.localPosition = new Vector3(0f, 0f, -4 + item.GetSeatID() + 1);
					active.AddOwner(gameObject2);
				}
			}
		}
		NGUITools.SetActive(active.m_owner, false);
	}

	private void OnActivateDetails(bool isChecked)
	{
		if (isChecked)
		{
			NGUITools.SetActive(m_descRoot, true);
			NGUITools.SetActive(m_playerInfoRoot, false);
		}
	}

	private void OnActivatePlayerInfo(bool isChecked)
	{
		if (isChecked)
		{
			NGUITools.SetActive(m_descRoot, false);
			NGUITools.SetActive(m_playerInfoRoot, true);
		}
	}

	private void OnClickBack(GameObject go)
	{
		InGameMenuManager.GetInstance().Close();
	}

	private void OnDestory()
	{
		DestoryTeamLogs();
	}
}
