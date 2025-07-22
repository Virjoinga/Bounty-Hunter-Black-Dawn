using System;
using UnityEngine;

public class UIVSCreateRoom : MonoBehaviour, UIMsgListener
{
	public UILabel m_name;

	public UILabel m_requireLowerLv;

	public UILabel m_requireHigherLv;

	public UIInput m_passwordInput;

	public UILabel m_password;

	public UISprite m_passwordBG;

	public GameObject m_cancel;

	public GameObject m_create;

	public UILabel m_changed;

	protected DateTime m_waitTimer;

	protected bool m_hasPassword;

	private void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(m_cancel);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCancel));
		UIEventListener uIEventListener2 = UIEventListener.Get(m_create);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCreate));
	}

	private void OnEnable()
	{
		if (UIVSTeam.m_instance != null)
		{
			InitRoom();
		}
	}

	public void OnActivate(bool isChecked)
	{
		if (isChecked)
		{
			UISprite component = m_passwordBG.GetComponent<UISprite>();
			component.spriteName = "number_n";
			component.MakePixelPerfect();
			m_passwordInput.enabled = true;
			m_hasPassword = true;
		}
		else
		{
			UISprite component2 = m_passwordBG.GetComponent<UISprite>();
			component2.spriteName = "number_b";
			component2.MakePixelPerfect();
			m_passwordInput.enabled = false;
			m_hasPassword = false;
		}
	}

	public void ChangeRoomName(string name)
	{
		m_waitTimer = DateTime.Now;
		m_name.text = Lobby.GetInstance().GetUserName();
	}

	private void InitRoom()
	{
		m_name.text = GameApp.GetInstance().GetUserState().GetRoleName();
		OnActivate(false);
		m_changed.text = string.Empty;
		m_requireLowerLv.text = "1";
		m_requireHigherLv.text = string.Empty + GameApp.GetInstance().GetUserState().GetCharLevel();
	}

	public void OnClickCancel(GameObject go)
	{
		UIVSTeam.m_instance.m_createRoom.gameObject.SetActive(false);
	}

	public void OnClickCreate(GameObject go)
	{
		short pass = 0;
		if (!string.IsNullOrEmpty(m_password.text))
		{
			pass = Convert.ToInt16(m_password.text);
		}
		else
		{
			m_hasPassword = false;
		}
		if (VerifyCreateRoom())
		{
			short num = Convert.ToInt16(m_requireLowerLv.text);
			short num2 = Convert.ToInt16(m_requireHigherLv.text);
			num = (short)((num <= 0) ? 1 : num);
			num2 = ((num2 >= Global.MAX_CHAR_LEVEL + 1) ? Global.MAX_CHAR_LEVEL : num2);
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			CreateVSRoomRequest request = new CreateVSRoomRequest(m_name.text, pass, m_hasPassword, TimeManager.GetInstance().Ping, num, num2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 10f, 12, this);
		}
	}

	private bool VerifyCreateRoom()
	{
		if (Convert.ToInt16(m_requireLowerLv.text) > GameApp.GetInstance().GetUserState().GetCharLevel() || Convert.ToInt16(m_requireHigherLv.text) < GameApp.GetInstance().GetUserState().GetCharLevel() || Convert.ToInt16(m_requireLowerLv.text) > Convert.ToInt16(m_requireHigherLv.text))
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NET_CREATE_ROOM_DISMATCH_LEVEL"), 2, 20);
			return false;
		}
		return true;
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 12 || whichMsg.EventId == 19)
		{
			UIMsgBox.instance.CloseMessage();
		}
		else if (whichMsg.EventId == 20)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}

	public void NotifyCreateRoomSuccess()
	{
		UILoadingNet.m_instance.Hide();
	}
}
