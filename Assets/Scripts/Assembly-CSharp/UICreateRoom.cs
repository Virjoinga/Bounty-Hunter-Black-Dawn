using System;
using UnityEngine;

public class UICreateRoom : MonoBehaviour, UIMsgListener
{
	public UILabel m_name;

	public UILabel m_comment;

	public UILabel m_requireLv;

	public UIInput m_passwordInput;

	public UILabel m_password;

	public UISprite m_passwordBG;

	public GameObject m_cancel;

	public GameObject m_create;

	public UILabel m_changed;

	protected DateTime m_waitTimer;

	protected bool m_beginChange;

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
		if (UITeam.m_instance != null)
		{
			InitRoom();
		}
	}

	private void Update()
	{
		if (m_beginChange)
		{
			if ((DateTime.Now - m_waitTimer).TotalSeconds >= 2.0)
			{
				UILoadingNet.m_instance.Hide();
				NGUITools.SetActive(base.gameObject, false);
				Debug.Log("start game...");
				StartGameRequest request = new StartGameRequest();
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			m_changed.text = LocalizationManager.GetInstance().GetString("LOC_ROOM_NAME_CHANGED_PROMPT");
		}
	}

	public void OnSubmit()
	{
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
		m_beginChange = true;
		m_waitTimer = DateTime.Now;
		m_name.text = Lobby.GetInstance().GetUserName();
	}

	private void InitRoom()
	{
		m_name.text = GameApp.GetInstance().GetUserState().GetRoleName();
		OnActivate(false);
		m_changed.text = string.Empty;
		m_beginChange = false;
	}

	public void OnClickCancel(GameObject go)
	{
		UITeam.m_instance.m_createRoom.SetActive(false);
		UITeam.m_instance.m_roomList.gameObject.SetActive(true);
	}

	public void OnClickCreate(GameObject go)
	{
		short pass = 0;
		if (!string.IsNullOrEmpty(m_password.text))
		{
			pass = Convert.ToInt16(m_password.text);
		}
		if (VerifyCreateRoom())
		{
			short num = Convert.ToInt16(m_requireLv.text);
			num = (short)((num <= 0) ? 1 : num);
			GameApp.GetInstance().GetGameWorld().LeaveCurrentRoom();
			CreateRoomRequest request = new CreateRoomRequest(m_name.text, pass, 4, m_hasPassword, 0, GameApp.GetInstance().GetUserState().GetCurrentCityID(), GameApp.GetInstance().GetUserState().GetCharLevel(), TimeManager.GetInstance().Ping, num, 999, GameApp.GetInstance().GetUserState().m_questStateContainer.GetQuestMark(), m_comment.text, GameApp.GetInstance().GetUserState().GetCurrentCityID(), GameApp.GetInstance().GetGameWorld().CurrentSceneID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			UILoadingNet.m_instance.Show(LocalizationManager.GetInstance().GetString("MSG_NET_TIME_OUT"), 10f, 12, this);
		}
	}

	private bool VerifyCreateRoom()
	{
		if (Convert.ToInt16(m_requireLv.text) > GameApp.GetInstance().GetUserState().GetCharLevel())
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_NET_CREATE_ROOM_DISMATCH_LEVEL"), 2, 20);
			return false;
		}
		return true;
	}

	private void OnDisable()
	{
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
}
