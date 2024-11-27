using System;
using System.Collections.Generic;
using UnityEngine;

public class UIVSRoomList : MonoBehaviour, InGameMenuListener, UIMsgListener
{
	private const int REFRASH_ROOM_DURATION = 60;

	public GameObject m_roomTemplate;

	public GameObject m_selectionTemplate;

	public GameObject m_playerInfoTemplate;

	public GameObject m_roomInfoPanelTemplate;

	public GameObject m_selectionBottomTemplate;

	public GameObject m_refreshBarTemplate;

	public GameObject m_seatInfoTemplate;

	public GameObject m_vsLogoTemplate;

	public GameObject m_vsCountDownTemplate;

	public GameObject m_container;

	public UILabel m_search;

	public GameObject m_createRoomBtn;

	public GameObject m_quickMatchBtn;

	public GameObject m_rankBtn;

	public GameObject m_rankBackBtn;

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
		UIEventListener uIEventListener2 = UIEventListener.Get(m_quickMatchBtn);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickQuickMatch));
		UIEventListener uIEventListener3 = UIEventListener.Get(m_join);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickJoin));
		UIEventListener uIEventListener4 = UIEventListener.Get(m_searchBtn);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickSearch));
		UIEventListener uIEventListener5 = UIEventListener.Get(m_rankBtn);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnClickRank));
		UIEventListener uIEventListener6 = UIEventListener.Get(m_rankBackBtn);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnClickRankBack));
		InGameMenuManager.GetInstance().SetListener(this);
	}

	private void OnEnable()
	{
		if (!(UIVSTeam.m_instance != null))
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
		if (UIVSTeam.giftDaily)
		{
			UIVSTeam.m_instance.m_rewards.SetActive(true);
		}
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null)
		{
			GetVSTDMRankRequest request = new GetVSTDMRankRequest(userState.GetVSTDMStatsId());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
			UIVSRoom component = room.Value.GetComponent<UIVSRoom>();
			if (component.IsPlayingAnimation())
			{
				return;
			}
		}
		m_updateRooms = false;
		UpdateRooms();
	}

	public void JoinVSRoom(int id)
	{
		JoinVSRoomRequest request = new JoinVSRoomRequest((short)id, TimeManager.GetInstance().Ping);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
		UIVSTeam.m_instance.SetMenuClose(false);
		UILoadingNet.m_instance.Hide();
	}

	public void JoinRoomFailed()
	{
		if (UIVSTeam.m_instance.m_password.activeSelf)
		{
			UIVSTeam.m_instance.m_password.SetActive(false);
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
				GetRoomListRequest request = new GetRoomListRequest(4, charLevel, maxUnlockCity, currentQuest);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				Debug.Log("lowerText:" + text);
				SearchRoomRequest request2 = new SearchRoomRequest(Mode.VS_TDM, text);
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
				UIVSRoom component = room.Value.GetComponent<UIVSRoom>();
				if (component.m_state == UIVSRoom.RoomState.Opened && !component.IsPlayingAnimation())
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
			UIVSRoom component = gameObject.GetComponent<UIVSRoom>();
			component.m_canExpand = false;
			component.m_state = UIVSRoom.RoomState.Closed;
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
				SendGetRoomData(m_roomList[i].getRoomID());
			}
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
			UIVSRoom component = m_rooms[roomID].GetComponent<UIVSRoom>();
			component.m_canExpand = true;
			component.m_playerNum.text = room2.getJoinedPlayer() + "/" + room2.getMaxPlayer() + " " + LocalizationManager.GetInstance().GetString("MENU_ROOM_PLAYER_NUM");
			component.SetSceneIcon(room2.getMapID());
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
			float num2 = 0f;
			UIBackgroundInfo component2 = gameObject.GetComponent<UIBackgroundInfo>();
			num = AddVSLogo(gameObject.transform, new Vector3(30f, 0f - num2, 0f)) + num2;
			num += AddVSCountDown(gameObject.transform, new Vector3(255f, 0f - (num + num2), 0f)) + num2;
			num += AddPlayerInfo(room2.GetAllPlayers(), room2.getPlayerNum(), gameObject.transform, new Vector3(75f, 0f - (num + num2), 0f)) + num2;
			num += 20f;
			component2.m_background.transform.localPosition = new Vector3(20f, 0f, 0f);
			component2.m_background.transform.localScale = new Vector3(component.GetUpperWidth() - 40f, num, 0f);
			component.SetUnderBackgroundY(num);
			if (m_currRoomId == roomID)
			{
				if (component.m_state == UIVSRoom.RoomState.Closed)
				{
					component.ActiveRoom();
				}
				NGUITools.SetActive(m_join, true);
			}
			Debug.Log("ready play :" + component.m_roomName.text + ": " + component.m_canExpand);
		}
	}

	public float AddVSLogo(Transform parent, Vector3 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_vsLogoTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = "4";
		gameObject.transform.localPosition = offset;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		GameObject gameObject2 = gameObject.transform.Find("Background").gameObject;
		return gameObject2.transform.localScale.y - 10f;
	}

	public float AddVSCountDown(Transform parent, Vector3 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(m_vsCountDownTemplate) as GameObject;
		gameObject.transform.parent = parent;
		gameObject.name = "5";
		gameObject.transform.localPosition = offset;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		UILabel label = gameObject.GetComponentsInChildren<UILabel>(true)[0];
		UICountDownButton uICountDownButton = m_join.GetComponentsInChildren<UICountDownButton>(true)[0];
		uICountDownButton.label = label;
		uICountDownButton.receiver = m_join;
		uICountDownButton.functionName = "OnClick";
		GameObject gameObject2 = gameObject.transform.Find("Background").gameObject;
		return gameObject2.transform.localScale.y;
	}

	public float AddPlayerInfo(RoomPlayer[] players, int playerCount, Transform parent, Vector3 offset)
	{
		Debug.Log("********************************************************");
		GameObject gameObject = UnityEngine.Object.Instantiate(m_seatInfoTemplate) as GameObject;
		gameObject.SetActive(true);
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = offset;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		UIVSSeat[] componentsInChildren = gameObject.GetComponentsInChildren<UIVSSeat>(true);
		foreach (RoomPlayer roomPlayer in players)
		{
			if (roomPlayer == null)
			{
				continue;
			}
			UIVSSeat[] array = componentsInChildren;
			foreach (UIVSSeat uIVSSeat in array)
			{
				if (uIVSSeat.m_Seat == roomPlayer.SeatID)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(m_playerInfoTemplate) as GameObject;
					gameObject2.name = roomPlayer.getPlayerName();
					UIPlayerInfo component = gameObject2.GetComponent<UIPlayerInfo>();
					component.SetPlayerInfo(roomPlayer.getPlayerName(), roomPlayer.Level, roomPlayer.Role, roomPlayer.IsLocalPlayer);
					uIVSSeat.SetPlayer(gameObject2);
				}
			}
			Debug.Log("RoomPlayer : " + roomPlayer.SeatID);
		}
		return 185f;
	}

	public void OnClickCreateRoom(GameObject go)
	{
		if (m_currRoomId == -1)
		{
			UIVSTeam.m_instance.m_createRoom.gameObject.SetActive(true);
		}
	}

	public void OnClickCancel(GameObject go)
	{
		Debug.Log("OnClickCancel..." + Lobby.GetInstance().GetCurrentRoomID());
		CloseRoomList();
		if (Lobby.GetInstance().GetCurrentRoomID() == -1)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
		}
	}

	public Room GetCurRoom()
	{
		foreach (Room room in m_roomList)
		{
			if (room.getRoomID() == UIVSTeam.m_instance.m_roomList.m_currRoomId)
			{
				return room;
			}
		}
		return null;
	}

	public void OnClickJoin(GameObject go)
	{
		StartGameInVSRoomRequest request = new StartGameInVSRoomRequest((short)UIVSTeam.m_instance.m_roomList.m_currRoomId);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void OnClickSearch(GameObject go)
	{
		if (m_currRoomId == -1)
		{
			Debug.Log("searching....");
			UpdateRoomsImmediate();
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 17 || whichMsg.EventId == 18)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
			UIMsgBox.instance.CloseMessage();
			CloseRoomList();
		}
		else if (whichMsg.EventId == 11)
		{
			GameApp.GetInstance().CloseConnectionGameServer();
			UIMsgBox.instance.CloseMessage();
			CloseRoomList();
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
			CloseRoomList();
		}
		else if (whichMsg.EventId == 16)
		{
			UIMsgBox.instance.CloseMessage();
			CloseRoomList();
			Application.OpenURL(UIConstant.FULL_VERSION_URL);
		}
		else if (whichMsg.EventId == 14)
		{
			UIMsgBox.instance.CloseMessage();
			CloseRoomList();
		}
	}

	public void SetRefreshFlag(bool bRefresh)
	{
		m_canRefresh = bRefresh;
	}

	public void OnCloseButtonClick()
	{
		Debug.Log("OnCloseButtonClick..." + Lobby.GetInstance().GetCurrentRoomID());
		CloseRoomList();
		GameApp.GetInstance().CloseConnectionGameServer();
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

	public void ChangeSeat(int seatID)
	{
		ChangeSeatRequest request = new ChangeSeatRequest((byte)seatID);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void OnClickQuickMatch(GameObject go)
	{
		if (m_currRoomId == -1)
		{
			QuickJoinRequest request = new QuickJoinRequest((byte)GameApp.GetInstance().GetUserState().GetCharLevel(), TimeManager.GetInstance().Ping);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 10f, 18, this);
		}
	}

	public void OnClickRank(GameObject go)
	{
		if (m_currRoomId == -1)
		{
			UIVSTeam.m_instance.m_rank.SetActive(true);
			UIVSTeam.m_instance.m_roomList.gameObject.SetActive(false);
		}
	}

	public void OnClickRankBack(GameObject go)
	{
		UIVSTeam.m_instance.m_rank.SetActive(false);
		UIVSTeam.m_instance.m_roomList.gameObject.SetActive(true);
	}

	public bool CheckRoomLevel(int roomID)
	{
		Room room = null;
		foreach (Room room2 in m_roomList)
		{
			if (room2.getRoomID() == roomID)
			{
				room = room2;
			}
		}
		if (room == null)
		{
			return false;
		}
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		if (charLevel < room.getMinJoinLevel() || charLevel > room.getMaxJoinLevel())
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NET_PVP_LEVEL_MISMATCH_JOIN_FAILED"), 2, 13);
			return false;
		}
		return true;
	}

	private void CloseRoomList()
	{
		if (UIStart.STARTMODE == UIStart.StartMode.Pvp)
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(20, false, false, false);
		}
		else
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
		}
	}
}
