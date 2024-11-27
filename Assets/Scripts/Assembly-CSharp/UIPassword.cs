using System;
using UnityEngine;

public class UIPassword : MonoBehaviour
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
		if (UITeam.m_instance != null)
		{
			Room curRoom = UITeam.m_instance.m_roomList.GetCurRoom();
			if (curRoom == null)
			{
				OnClickCancel(null);
			}
			else
			{
				SetRoom(curRoom);
			}
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
		short num = Convert.ToInt16(m_password.text);
		if (m_room.getRoomPassword() == num)
		{
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			JoinRoomRequest request = new JoinRoomRequest((short)UITeam.m_instance.m_roomList.m_currRoomId, GameApp.GetInstance().GetUserState().GetCharLevel(), TimeManager.GetInstance().Ping);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT_JOIN_FAILED"), 5f, 12, UITeam.m_instance.m_roomList);
		}
		else
		{
			m_wrongPrompt.text = LocalizationManager.GetInstance().GetString("MSG_NET_WRONG_PASSWORD");
		}
	}

	private void OnClickCancel(GameObject go)
	{
		UITeam.m_instance.m_roomList.SetRefreshFlag(true);
		base.gameObject.SetActive(false);
	}
}
