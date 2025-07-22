using System;
using System.Collections.Generic;
using UnityEngine;

public class UIBossRushRoomList : MonoBehaviour, InGameMenuListener, UIMsgListener
{
	private const int REFRASH_ROOM_DURATION = 60;

	public GameObject m_roomTemplate;

	public GameObject m_selectionTemplate;

	public GameObject m_commentTemplate;

	public GameObject m_playerInfoTemplate;

	public GameObject m_questInfoTemplate;

	public GameObject m_roomInfoPanelTemplate;

	public GameObject m_selectionBottomTemplate;

	public GameObject m_refreshBarTemplate;

	public GameObject m_seatInfoTemplate;

	public GameObject m_container;

	public UILabel m_search;

	public GameObject m_createRoomBtn;

	public GameObject m_cancel;

	public GameObject m_join;

	public GameObject m_searchBtn;

	protected GameObject m_selected;

	protected GameObject m_refresh;

	protected List<Room> m_roomList = new List<Room>();

	public int m_currRoomId;

	private DateTime m_start;

	protected Dictionary<int, GameObject> m_rooms = new Dictionary<int, GameObject>();

	protected DateTime m_refreshTime;

	protected Vector3 m_initContainer;

	protected bool m_canRefresh = true;

	protected bool m_updateRooms;

	private int m_recommendRoomNumber;

	private bool m_pulldownUpdate;

	private UIDraggablePanelAlign m_dragPanel;

	private void Awake()
	{
		NGUITools.SetActive(m_join, false);
		m_start = DateTime.Now;
		m_currRoomId = -1;
	}

	private void Start()
	{
		m_initContainer = m_container.transform.parent.localPosition;
		m_dragPanel = m_container.transform.parent.GetComponent<UIDraggablePanelAlign>();
		UIEventListener uIEventListener = UIEventListener.Get(m_createRoomBtn);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCreateRoom));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_cancel);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCancel));
		UIEventListener uIEventListener3 = UIEventListener.Get(m_join);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickJoin));
		UIEventListener uIEventListener4 = UIEventListener.Get(m_searchBtn);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickSearch));
		InGameMenuManager.GetInstance().SetListener(this);
	}

	private void OnEnable()
	{
		if (!(UIBossRushTeam.m_instance != null))
		{
			return;
		}
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			SetRefreshFlag(true);
			m_refreshTime = DateTime.Now;
			if (GameApp.GetInstance().GetGameMode().TypeOfNetwork == NetworkType.Single)
			{
				GameApp.GetInstance().GetUserState().LoginAsUser();
				UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_LOGIN_TIMEOUT"), 25f, 18, this);
				Debug.Log("Login...");
			}
			else
			{
				SendUpdateRoomsRequest(true);
			}
		}
		else
		{
			UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_CAN_NOT_ACCESS_SERVER"), 1f, 11, this);
		}
	}

	public void LoginSuccess()
	{
		if (UIBossRushTeam.giftDaily)
		{
			UIBossRushTeam.m_instance.m_rewards.SetActive(true);
		}
		SendUpdateRoomsRequest(true);
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
		DestoryRooms();
		InGameMenuManager.GetInstance().RemoveListener(this);
	}

	private void Update()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			SendUpdateRoomsRequest(false);
		}
		if (m_refresh != null)
		{
			float num = 50f;
			if (m_container.transform.parent.localPosition.y < m_initContainer.y - num)
			{
				UISprite component = m_refresh.transform.Find("Sprite").GetComponent<UISprite>();
				if (!component.spriteName.Equals("update"))
				{
					component.spriteName = "update";
					component.MakePixelPerfect();
				}
				m_pulldownUpdate = true;
			}
			else if (Mathf.Abs(m_container.transform.parent.localPosition.y - m_initContainer.y) <= 10f)
			{
				UISprite component2 = m_refresh.transform.Find("Sprite").GetComponent<UISprite>();
				if (!component2.spriteName.Equals("pulldown"))
				{
					component2.spriteName = "pulldown";
					component2.MakePixelPerfect();
				}
				if (m_dragPanel != null && !m_dragPanel.IsMoving() && m_pulldownUpdate)
				{
					Debug.Log("UpdateRoomsImmediate--------------");
					UpdateRoomsImmediate();
					m_pulldownUpdate = false;
				}
			}
		}
		CloseOthersRoom(m_currRoomId);
	}

	private void LateUpdate()
	{
		if (!m_updateRooms)
		{
			return;
		}
		foreach (KeyValuePair<int, GameObject> room in m_rooms)
		{
			UIRoom component = room.Value.GetComponent<UIRoom>();
			if (component.IsPlayingAnimation())
			{
				return;
			}
		}
		m_updateRooms = false;
		UpdateRooms();
	}

	public void SendGetRoomData(int id)
	{
		GetRoomDataRequest request = new GetRoomDataRequest((short)id);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		Debug.Log("get room data......................");
	}

	public void ActiveRoom(GameObject go)
	{
	}

	public void JoinRoomSuccess()
	{
		UIBossRushTeam.m_instance.SetMenuClose(false);
		UILoadingNet.m_instance.Hide();
	}

	public void JoinRoomFailed()
	{
		if (UIBossRushTeam.m_instance.m_password.activeSelf)
		{
			UIBossRushTeam.m_instance.m_password.SetActive(false);
		}
		UILoadingNet.m_instance.Hide();
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NET_JOIN_FAILED"), 2, 13);
	}

	public void UpdateRoomsImmediate()
	{
		if ((DateTime.Now - m_refreshTime).TotalSeconds >= 2.0)
		{
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 3f, 12, this);
			SendUpdateRoomsRequest(true);
			m_refreshTime = DateTime.Now;
			Debug.Log("send update..");
		}
	}

	public void SendUpdateRoomsRequest(bool immediate)
	{
		if (!m_canRefresh)
		{
			m_start = DateTime.Now;
		}
		if (immediate)
		{
			m_start = DateTime.Now.AddSeconds(-61.0);
		}
		if ((DateTime.Now - m_start).TotalSeconds >= 60.0)
		{
			m_start = DateTime.Now;
			string text = m_search.text.ToLower().Trim();
			if (string.IsNullOrEmpty(text))
			{
				short charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
				byte maxUnlockCity = GameApp.GetInstance().GetUserState().GetMaxUnlockCity();
				short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
				GetRoomListRequest request = new GetRoomListRequest((byte)UIBossRushTeam.BossRushMode, charLevel, maxUnlockCity, currentQuest);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				Debug.Log("lowerText:" + text);
				SearchRoomRequest request2 = new SearchRoomRequest(UIBossRushTeam.BossRushMode, text);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void Selection(int roomId)
	{
		if (m_rooms.ContainsKey(roomId))
		{
			GameObject gameObject = m_rooms[roomId];
			m_currRoomId = roomId;
		}
	}

	public void CloseOthersRoom(int exceptRoomId)
	{
		foreach (KeyValuePair<int, GameObject> room in m_rooms)
		{
			if (room.Value != null && room.Key != exceptRoomId)
			{
				UIRoom component = room.Value.GetComponent<UIRoom>();
				if (component.m_state == UIRoom.RoomState.Opened && !component.IsPlayingAnimation())
				{
					Debug.Log("roomScript.m_roomName: " + component.m_roomName.text);
					component.DisableRoom();
				}
			}
		}
	}

	public void BeginUpdateRooms(List<Room> roomList, int recommendRoomNumber)
	{
		UILoadingNet.m_instance.Hide();
		m_roomList.Clear();
		foreach (Room room in roomList)
		{
			m_roomList.Add(room);
		}
		DestoryRooms();
		m_recommendRoomNumber = recommendRoomNumber;
		Debug.Log("roomList: " + m_roomList.Count);
		m_updateRooms = true;
	}

	private void UpdateRooms()
	{
		NGUITools.SetActive(m_join, false);
		UserState userState = GameApp.GetInstance().GetUserState();
		Transform transform = m_container.transform;
		int count = m_roomList.Count;
		m_refresh = NGUITools.AddChild(m_container, m_refreshBarTemplate);
		m_refresh.name = Convert.ToString(0);
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = NGUITools.AddChild(m_container, m_roomTemplate);
			gameObject.name = Convert.ToString(i + 1);
			Room room = m_roomList[i];
			Transform trans = gameObject.transform;
			UIRoom component = gameObject.GetComponent<UIRoom>();
			component.m_canExpand = false;
			component.m_state = UIRoom.RoomState.Closed;
			bool bRecommend = ((i < m_recommendRoomNumber) ? true : false);
			component.SetRoom(room, bRecommend);
			component.m_id = room.getRoomID();
			BoxCollider boxCollider = gameObject.GetComponent<Collider>() as BoxCollider;
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(trans);
			boxCollider.center = new Vector3(bounds.center.x, bounds.center.y, 0f);
			boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
			m_rooms.Add(room.getRoomID(), gameObject);
			if (room.getRoomID() == m_currRoomId)
			{
				SendGetRoomData(m_roomList[0].getRoomID());
			}
		}
		if (m_currRoomId == -1 && m_roomList.Count > 0)
		{
			Selection(m_roomList[0].getRoomID());
			SendGetRoomData(m_roomList[0].getRoomID());
		}
		UITableSort component2 = m_container.GetComponent<UITableSort>();
		component2.repositionNow = true;
		Debug.Log("UIRoomList: OnUIRoomList");
	}

	public void UpdateRoomData(Room room)
	{
		if (room == null)
		{
			return;
		}
		int roomID = room.getRoomID();
		foreach (Room room2 in m_roomList)
		{
			if (room2.getRoomID() != roomID || !m_rooms.ContainsKey(roomID))
			{
				continue;
			}
			room2.setJoinedPlayer(room.getJoinedPlayer());
			room2.setAllPlayer(room.GetAllPlayers());
			UIRoom component = m_rooms[roomID].GetComponent<UIRoom>();
			component.m_canExpand = true;
			component.m_playerNum.text = Convert.ToString(room2.getJoinedPlayer()) + " " + LocalizationManager.GetInstance().GetString("MENU_ROOM_PLAYER_NUM");
			Transform transform = component.m_tween.transform;
			foreach (Transform item in transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(m_roomInfoPanelTemplate) as GameObject;
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			float num = 0f;
			float num2 = 10f;
			UIBackgroundInfo component2 = gameObject.GetComponent<UIBackgroundInfo>();
			num = AddQuestInfo(room2.getQuestMark(), gameObject.transform, new Vector3(30f, 0f - num2, 0f)) + num2;
			num += AddPlayerInfo(room2.GetAllPlayers(), room2.getPlayerNum(), gameObject.transform, new Vector3(30f, 0f - (num + num2), 0f)) + num2;
			num += AddComment(room2.getComment(), gameObject.transform, new Vector3(30f, 0f - (num + num2), 0f)) + num2;
			num += 20f;
			component2.m_background.transform.localPosition = new Vector3(20f, 0f, 0f);
			component2.m_background.transform.localScale = new Vector3(component.GetUpperWidth() - 40f, num, 0f);
			component.SetUnderBackgroundY(num);
			if (m_currRoomId == roomID)
			{
				if (component.m_state == UIRoom.RoomState.Closed)
				{
					component.ActiveRoom();
				}
				if (Lobby.GetInstance().GetCurrentRoomID() != m_currRoomId)
				{
					NGUITools.SetActive(m_join, true);
				}
			}
			Debug.Log("ready play :" + component.m_roomName.text + ": " + component.m_canExpand);
		}
	}

	public float AddQuestInfo(short markQuest, Transform parent, Vector3 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_questInfoTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = "0";
		UIQuestInfo component = gameObject.GetComponent<UIQuestInfo>();
		component.SetQuestInfo(markQuest, GameApp.GetInstance().GetUserState().m_questStateContainer.GetName(markQuest), GameApp.GetInstance().GetUserState().m_questStateContainer.GetLevel(markQuest));
		gameObject.transform.localPosition = offset;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		GameObject gameObject2 = gameObject.transform.Find("Sprite").gameObject;
		return gameObject2.transform.localScale.y;
	}

	public float AddComment(string comment, Transform parent, Vector3 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_commentTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = "4";
		GameObject gameObject2 = gameObject.transform.Find("Label").gameObject;
		UILabel component = gameObject2.GetComponent<UILabel>();
		component.text = comment;
		gameObject.transform.localPosition = offset;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		GameObject gameObject3 = gameObject.transform.Find("Background").gameObject;
		return gameObject3.transform.localScale.y;
	}

	public float AddPlayerInfo(RoomPlayer[] players, int playerCount, Transform parent, Vector3 offset)
	{
		Debug.Log("players count: " + players.Length);
		float num = 65f;
		List<GameObject> list = new List<GameObject>();
		int num2 = (playerCount + 1) / 2;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_seatInfoTemplate) as GameObject;
			gameObject.SetActive(true);
			gameObject.transform.parent = parent;
			gameObject.name = Convert.ToString(1 + i);
			gameObject.transform.localPosition = new Vector3(offset.x, offset.y - num * (float)i, offset.z);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			list.Add(gameObject);
		}
		int num3 = 0;
		for (int j = 0; j < players.Length; j++)
		{
			RoomPlayer roomPlayer = players[j];
			if (roomPlayer != null)
			{
				Debug.Log("players : " + j + " " + roomPlayer);
				GameObject gameObject2 = UnityEngine.Object.Instantiate(m_playerInfoTemplate) as GameObject;
				Transform transform = null;
				transform = ((num3 % 2 != 0) ? list[num3 / 2].transform.Find("Right") : list[num3 / 2].transform.Find("Left"));
				gameObject2.transform.parent = transform;
				gameObject2.name = roomPlayer.getPlayerName();
				UIPlayerInfo component = gameObject2.GetComponent<UIPlayerInfo>();
				component.SetPlayerInfo(roomPlayer.getPlayerName(), roomPlayer.Level, roomPlayer.Role);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
				num3++;
			}
		}
		return num * (float)num2;
	}

	public void OnClickCreateRoom(GameObject go)
	{
		UIBossRushTeam.m_instance.m_roomList.gameObject.SetActive(false);
		UIBossRushTeam.m_instance.m_createRoom.SetActive(true);
	}

	public void OnClickCancel(GameObject go)
	{
		Debug.Log("OnClickCancel..." + Lobby.GetInstance().GetCurrentRoomID());
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		if (Lobby.GetInstance().GetCurrentRoomID() == -1)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
		}
	}

	public Room GetCurRoom()
	{
		foreach (Room room in m_roomList)
		{
			if (room.getRoomID() == UIBossRushTeam.m_instance.m_roomList.m_currRoomId)
			{
				return room;
			}
		}
		return null;
	}

	public void OnClickJoin(GameObject go)
	{
		Room curRoom = GetCurRoom();
		if (curRoom != null)
		{
			if (GameApp.GetInstance().GetUserState().GetCharLevel() < curRoom.getMinJoinLevel())
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NET_LEVEL_MISMATCH_JOIN_FAILED"), 2, 13);
				return;
			}
			if (curRoom.isHasPassword())
			{
				SetRefreshFlag(false);
				UIBossRushTeam.m_instance.m_password.SetActive(true);
				return;
			}
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			JoinRoomRequest request = new JoinRoomRequest((short)UIBossRushTeam.m_instance.m_roomList.m_currRoomId, GameApp.GetInstance().GetUserState().GetCharLevel(), TimeManager.GetInstance().Ping);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT_JOIN_FAILED"), 5f, 12, UIBossRushTeam.m_instance.m_roomList);
		}
	}

	public void OnClickSearch(GameObject go)
	{
		Debug.Log("searching....");
		UpdateRoomsImmediate();
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 17 || whichMsg.EventId == 18)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
		else if (whichMsg.EventId == 11)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
		else if (whichMsg.EventId == 19)
		{
			UIMsgBox.instance.CloseMessage();
		}
		else if (whichMsg.EventId == 13)
		{
			UIMsgBox.instance.CloseMessage();
			UpdateRoomsImmediate();
		}
		else if (whichMsg.EventId == 12)
		{
			UIMsgBox.instance.CloseMessage();
		}
		else if (whichMsg.EventId == 15)
		{
			UILoadingNet.m_instance.Hide();
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
		else if (whichMsg.EventId == 16)
		{
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
			Application.OpenURL(UIConstant.FULL_VERSION_URL);
		}
		else if (whichMsg.EventId == 14)
		{
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
	}

	public void SetRefreshFlag(bool bRefresh)
	{
		m_canRefresh = bRefresh;
	}

	public void OnCloseButtonClick()
	{
		Debug.Log("OnCloseButtonClick..." + Lobby.GetInstance().GetCurrentRoomID());
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		if (Lobby.GetInstance().GetCurrentRoomID() == -1)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
		}
	}

	public void DestoryRooms()
	{
		foreach (int key in m_rooms.Keys)
		{
			UnityEngine.Object.Destroy(m_rooms[key]);
		}
		m_rooms.Clear();
		NGUITools.Destroy(m_refresh);
	}
}
