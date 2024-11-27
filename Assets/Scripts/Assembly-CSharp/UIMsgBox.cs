using UnityEngine;

public class UIMsgBox : MonoBehaviour
{
	public const byte EVENT_TYPE_NOT_DEFINED = 0;

	public const byte EVENT_TYPE_DISCONNECTION = 1;

	public const byte EVENT_CREATE_CHARACTER = 2;

	public const byte EVENT_CREATE_CHARACTER_FAIL = 3;

	public const byte EVENT_INVITATION = 4;

	public const byte EVENT_CONFIRM_TO_USE_MITHRIL = 5;

	public const byte EVENT_RETURN_TO_CITY_FREE = 6;

	public const byte EVENT_RETURN_TO_CITY_MITHRIL = 7;

	public const byte EVENT_RESET_GAMBLE = 8;

	public const byte EVENT_MITHRIL_NOT_ENOUGH = 9;

	public const byte EVENT_SKILL_POINT_NOT_ENOUGH = 10;

	public const byte EVENT_GOLD_NOT_ENOUGH = 99;

	public const byte EVENT_CAN_NOT_ACCESS_SERVER = 11;

	public const byte EVENT_NET_TIMEOUT = 12;

	public const byte EVENT_JOIN_ROOM_FAILED = 13;

	public const byte EVENT_SERVER_MAINTAINEANCE = 14;

	public const byte EVENT_ACCOUNT_LOCKED = 15;

	public const byte EVENT_VERSION_MISMATCH = 16;

	public const byte EVENT_LOGIN_SERVER_FAILED = 17;

	public const byte EVENT_LOGIN_TIMEOUT = 18;

	public const byte EVENT_CREATE_ROOM_FAILED = 19;

	public const byte EVENT_CREATE_ROOM_LEVEL_MISMATCH = 20;

	public const byte EVENT_SINGLE_PVP_MISMATCH = 52;

	public const byte EVENT_UNLOCK_FIRST_WEAPON_SLOT = 21;

	public const byte EVENT_UNLOCK_SECOND_WEAPON_SLOT = 22;

	public const byte EVENT_UNLOCK_FIRST_CHIP_SLOT = 23;

	public const byte EVENT_UNLOCK_SECOND_CHIP_SLOT = 24;

	public const byte EVENT_UNLOCK_THIRD_CHIP_SLOT = 25;

	public const byte EVENT_REFRESH_SHOP = 26;

	public const byte EVENT_UPGRADE_BULLET_BY_GOLD = 27;

	public const byte EVENT_UPGRADE_BULLET_BY_MITHRIL = 28;

	public const byte EVENT_IAP_SERVER_TIME_OUT = 29;

	public const byte EVENT_BAG_IS_FULL = 30;

	public const byte EVENT_CLEAR_SKILL_POINTS = 31;

	public const byte EVENT_CHIP_CONFLICT_WARNING = 32;

	public const byte EVENT_INVITAION_IN_MENU = 33;

	public const byte EVENT_BUY_SKILL_POINTS = 34;

	public const byte EVENT_ENTER_BLACK_MARKET = 35;

	public const byte EVENT_EXTEND_BAG = 36;

	public const byte EVENT_QUEST_ABANDON = 41;

	public const byte EVENT_MITHRIL_REWARDS = 46;

	public const byte EVENT_CONFIRM_TO_DISCONNECT = 47;

	public const byte EVENT_PLEASE_RATE_US = 48;

	public const byte EVENT_NO_VIDEO_TO_WATCH = 49;

	public const byte EVENT_FINAL_MIAN_QUEST = 50;

	public const byte EVENT_GIFT_MITHRIL = 51;

	public const byte EVENT_FIRST_HUNTER = 52;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_QUERY = 42;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_TIMEOUT = 43;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_NO_DATA = 44;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_FAILED = 45;

	public const byte EVENT_RMS_ICLOUD_UNAVAILABLE = 53;

	public const byte EVENT_RMS_ICLOUD_UPLOAD_FAILED = 54;

	public const byte EVENT_RMS_ICLOUD_UPLOAD_SUCCESS = 55;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_SUCCESS = 56;

	public const byte EVENT_RMS_LOAD_FAILED_PROMPT = 57;

	public const byte EVENT_RMS_LOAD_ROLE_FAILED_PROMPT = 58;

	public const byte EVENT_RMS_LOAD_FAILED = 59;

	public const byte EVENT_RMS_ICLOUD_DOWNLOAD_VER_MISMATCH = 60;

	public const byte EVENT_TUTORIAL_UPGRADE = 61;

	public const byte EVENT_PAY_FOR_RESPAWN = 62;

	public const byte EVENT_ADS_MITHRIL_REWARDS_CONFIRM = 63;

	public const byte EVENT_EXIT_GAME_QUERY = 64;

	public const byte EVENT_UNLOCK_FIRST_GREEN = 65;

	public const byte EVENT_UNLOCK_SECOND_GREEN = 66;

	public const byte EVENT_UNLOCK_THIRD_GREEN = 67;

	public const byte TYPE_UNKNOWN = 0;

	public const byte TYPE_DIALOG = 1;

	public const byte TYPE_CONFIRM = 2;

	public const byte TYPE_QUERY = 3;

	public const byte TYPE_WAIT = 4;

	public const byte TYPE_TIMED_CONFIRM = 5;

	public const byte TYPE_TIMED_QUERY = 6;

	public const byte TYPE_TIMED_WAIT = 7;

	public static UIMsgBox instance;

	public GameObject m_Anchor;

	public GameObject m_Camera;

	public UIMsg[] m_Msg;

	private UIMsg m_CurrentMsg;

	private void Awake()
	{
		instance = this;
		InactivateSelf();
		UIMsg[] msg = m_Msg;
		foreach (UIMsg uIMsg in msg)
		{
			uIMsg.gameObject.SetActive(false);
		}
	}

	private void Destroy()
	{
		instance = null;
	}

	public UIMsg ShowSystemMessage(UIMsgListener listener, string info, byte type, byte eventId, float time)
	{
		return ShowMessage(listener, info, type, eventId, true, time);
	}

	public UIMsg ShowSystemMessage(UIMsgListener listener, string info, byte type, byte eventId)
	{
		return ShowMessage(listener, info, type, eventId, true, -1f);
	}

	public UIMsg ShowMessage(UIMsgListener listener, string info, byte type, byte eventId, float time)
	{
		return ShowMessage(listener, info, type, eventId, false, time);
	}

	public UIMsg ShowMessage(UIMsgListener listener, string info, byte type, byte eventId)
	{
		return ShowMessage(listener, info, type, eventId, false, -1f);
	}

	public UIMsg ShowMessage(UIMsgListener listener, string info, byte type)
	{
		return ShowMessage(listener, info, type, 0, false, -1f);
	}

	public UIMsg ShowMessage(UIMsgListener listener, string info, byte type, float time)
	{
		return ShowMessage(listener, info, type, 0, false, time);
	}

	public UIMsg ShowMessage(UIMsgListener listener, string info, byte type, byte eventId, bool ignoreOthers, float time)
	{
		if (m_CurrentMsg == null || !m_CurrentMsg.IgnoreOthers)
		{
			if (m_CurrentMsg != null)
			{
				m_CurrentMsg.gameObject.SetActive(false);
			}
			if (type < 1 || type > m_Msg.Length)
			{
				m_CurrentMsg = null;
			}
			else
			{
				m_CurrentMsg = m_Msg[type - 1];
			}
			if (time > 0f)
			{
				UIProgressBar componentInChildren = m_CurrentMsg.GetComponentInChildren<UIProgressBar>();
				if (componentInChildren != null)
				{
					componentInChildren.time = time;
				}
			}
			if (m_CurrentMsg != null)
			{
				ActivateSelf();
				m_CurrentMsg.m_Information.text = info;
				m_CurrentMsg.EventId = eventId;
				m_CurrentMsg.Type = type;
				m_CurrentMsg.IgnoreOthers = ignoreOthers;
				m_CurrentMsg.SetListener(listener);
				m_CurrentMsg.gameObject.SetActive(true);
				if (Camera.main != null)
				{
					AudioManager.GetInstance().PlaySound("RPG_Audio/Menu/menu_popup");
				}
			}
		}
		return m_CurrentMsg;
	}

	public void CloseMessage()
	{
		if (m_CurrentMsg != null)
		{
			m_CurrentMsg.gameObject.SetActive(false);
			m_CurrentMsg = null;
			InactivateSelf();
		}
	}

	public byte GetCurrentMessageEventID()
	{
		if (m_CurrentMsg != null)
		{
			return m_CurrentMsg.EventId;
		}
		return 0;
	}

	public byte GetCurrentMessageType()
	{
		if (m_CurrentMsg != null)
		{
			return m_CurrentMsg.Type;
		}
		return 0;
	}

	public bool IsMessageShow()
	{
		return m_CurrentMsg != null;
	}

	private void ActivateSelf()
	{
		base.gameObject.SetActive(true);
	}

	private void InactivateSelf()
	{
		base.gameObject.SetActive(false);
	}
}
