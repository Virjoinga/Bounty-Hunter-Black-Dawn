using System;
using UnityEngine;

public class UIVSPassword : MonoBehaviour, UIMsgListener
{
	public UILabel m_roomName;

	public UILabel m_password;

	public GameObject m_confirm;

	public GameObject m_cancel;

	public GameObject m_collider;

	public UILabel m_wrongPrompt;

	protected Room m_room;

	private void Awake()
	{
		BoxCollider component = m_collider.GetComponent<BoxCollider>();
		component.center = Vector3.zero;
		component.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(m_confirm);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickConfirm));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_cancel);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCancel));
	}

	private void OnEnable()
	{
		if (UIVSTeam.m_instance != null)
		{
			Room curRoom = UIVSTeam.m_instance.m_roomList.GetCurRoom();
			if (curRoom == null)
			{
				OnClickCancel(null);
				return;
			}
			UIVSTeam.m_instance.m_roomList.SetRefreshFlag(false);
			SetRoom(curRoom);
		}
	}

	private void OnDisable()
	{
		if (UIVSTeam.m_instance != null)
		{
			UIVSTeam.m_instance.m_roomList.SetRefreshFlag(true);
		}
	}

	public void SetRoom(Room room)
	{
		m_room = room;
		m_roomName.text = room.getRoomName();
		m_wrongPrompt.text = string.Empty;
	}

	private void OnClickConfirm(GameObject go)
	{
		if (!string.IsNullOrEmpty(m_password.text))
		{
			short num = Convert.ToInt16(m_password.text);
			if (m_room.getRoomPassword() == num)
			{
				GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
				UIVSTeam.m_instance.m_roomList.JoinVSRoom(UIVSTeam.m_instance.m_roomList.GetCurRoom().getRoomID());
				base.gameObject.SetActive(false);
				UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 10f, 18, this);
			}
			else
			{
				m_wrongPrompt.text = LocalizationManager.GetInstance().GetString("MSG_NET_WRONG_PASSWORD");
			}
		}
	}

	private void OnClickCancel(GameObject go)
	{
		base.gameObject.SetActive(false);
		UIVSTeam.m_instance.m_roomList.m_currRoomId = -1;
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
