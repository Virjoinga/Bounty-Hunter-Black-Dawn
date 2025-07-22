using UnityEngine;

public class UIVSRoom : MonoBehaviour, UIMsgListener
{
	public enum RoomState
	{
		Opened = 0,
		Closed = 1
	}

	public GameObject m_light;

	public UILabel m_roomName;

	public UILabel m_requireLv;

	public UILabel m_playerNum;

	public GameObject m_tween;

	public GameObject m_lock;

	public UISprite m_ping;

	public UISprite m_SceneIcon;

	public int m_id;

	public RoomState m_state;

	public bool m_canExpand;

	public UITweenX m_Tween2;

	public UITweenX m_Tween1;

	public GameObject m_under;

	public GameObject m_upper;

	public GameObject m_recommend;

	private Vector3 m_size;

	private bool isPlayingAnimation;

	private float m_upperWidth;

	private float m_upperHeight;

	private float m_underBackgroundY;

	private BoxCollider m_boxCollider;

	private void Awake()
	{
		Transform transform = m_under.transform.Find("Background");
		if (transform != null)
		{
			m_upperWidth = transform.localScale.x;
			m_upperHeight = transform.localScale.y;
		}
		m_boxCollider = base.gameObject.GetComponent<Collider>() as BoxCollider;
	}

	public void SetRoom(Room room, bool bRecommend)
	{
		m_roomName.text = room.getRoomName();
		if (room.getRoomID() == Lobby.GetInstance().GetCurrentRoomID())
		{
			m_roomName.color = new Color(0f, 1f, 0.23529412f);
		}
		m_requireLv.text = LocalizationManager.GetInstance().GetString("MSG_REQUIRE_LEVEL") + room.getMinJoinLevel() + "~" + room.getMaxJoinLevel();
		m_playerNum.text = room.getJoinedPlayer() + "/" + room.getMaxPlayer() + " " + LocalizationManager.GetInstance().GetString("MENU_ROOM_PLAYER_NUM");
		SetSceneIcon(room.getMapID());
		if (room.getPing() <= 50)
		{
			m_ping.spriteName = "net_4";
		}
		else if (room.getPing() <= 100)
		{
			m_ping.spriteName = "net_3";
		}
		else if (room.getPing() <= 300)
		{
			m_ping.spriteName = "net_2";
		}
		else
		{
			m_ping.spriteName = "net_1";
		}
		m_ping.MakePixelPerfect();
		if (!room.isHasPassword())
		{
			NGUITools.SetActive(m_lock, false);
		}
		if (bRecommend)
		{
			m_recommend.SetActive(true);
		}
		else
		{
			m_recommend.SetActive(false);
		}
	}

	public float GetUpperWidth()
	{
		return m_upperWidth;
	}

	public void SetUnderBackgroundY(float posy)
	{
		m_underBackgroundY = posy;
	}

	public bool IsPlayingAnimation()
	{
		return isPlayingAnimation;
	}

	public void SetSceneIcon(int mapID)
	{
		m_SceneIcon.spriteName = "map0" + (mapID + 1);
	}

	private void OnClick()
	{
		Debug.Log("OnClick: " + isPlayingAnimation);
		if (isPlayingAnimation)
		{
			return;
		}
		if (m_state == RoomState.Closed)
		{
			if (!UIVSTeam.m_instance.m_roomList.CheckRoomLevel(m_id))
			{
				return;
			}
			UIVSTeam.m_instance.m_roomList.Selection(m_id);
			if (UIVSTeam.m_instance.m_roomList.GetCurRoom().isHasPassword())
			{
				UIVSTeam.m_instance.m_password.SetActive(true);
			}
			else
			{
				GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
				UIVSTeam.m_instance.m_roomList.JoinVSRoom(m_id);
				UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 10f, 18, this);
			}
		}
		else
		{
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			UIVSTeam.m_instance.m_roomList.SendGetRoomData(m_id);
			UIVSTeam.m_instance.m_roomList.m_currRoomId = -1;
		}
		NGUITools.SetActive(UIVSTeam.m_instance.m_roomList.m_join, false);
	}

	private void Update()
	{
		float y = m_upper.transform.localPosition.y;
		m_under.transform.localPosition = new Vector3(m_under.transform.localPosition.x, y - m_underBackgroundY * m_tween.transform.localScale.y, 0f);
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
		m_boxCollider.center = new Vector3(bounds.center.x, bounds.center.y, 0f);
		m_boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
		if (Input.GetKeyDown(KeyCode.C))
		{
			m_Tween1.PlayForward(PlayForwardTween2);
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			m_Tween2.PlayReverse(PlayReverseTween1);
		}
	}

	public void Select()
	{
		if (m_canExpand && !isPlayingAnimation)
		{
			isPlayingAnimation = true;
			if (m_state == RoomState.Closed)
			{
				m_Tween1.PlayForward(PlayForwardTween2);
			}
			else
			{
				m_Tween2.PlayReverse(PlayReverseTween1);
			}
		}
	}

	private void PlayForwardTween2()
	{
		m_light.SetActive(false);
		m_Tween2.PlayForward(PlayForwardOver);
	}

	private void PlayForwardOver()
	{
		m_state = RoomState.Opened;
		isPlayingAnimation = false;
		Debug.Log("PlayReverseOver Opened....");
	}

	private void PlayReverseTween1()
	{
		m_light.SetActive(true);
		m_Tween1.PlayReverse(PlayReverseOver);
	}

	private void PlayReverseOver()
	{
		m_state = RoomState.Closed;
		isPlayingAnimation = false;
		Debug.Log("PlayReverseOver Closed....");
	}

	public void ActiveRoom()
	{
		isPlayingAnimation = true;
		m_Tween1.PlayForward(PlayForwardTween2);
	}

	public void DisableRoom()
	{
		isPlayingAnimation = true;
		m_Tween2.PlayReverse(PlayReverseTween1);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 12)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}
}
