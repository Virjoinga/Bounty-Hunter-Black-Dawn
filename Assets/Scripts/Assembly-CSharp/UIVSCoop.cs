using UnityEngine;

public class UIVSCoop : UIVS, UIMsgListener
{
	public GameObject readyButton;

	public GameObject readyButtonLock;

	public UICheckbox m1v1CheckBox;

	public UICheckbox m4v4CheckBox;

	public GameObject rankBackButton;

	public GameObject rankButton;

	public GameObject uiRank;

	public GameObject uiReady;

	private Mode curMode;

	private float lastTimeClick;

	private bool bReady;

	private bool bClick;

	protected override void Awake()
	{
		base.Awake();
		AddDelegate(readyButton);
		AddDelegate(rankBackButton);
		AddDelegate(rankButton);
		readyButtonLock.SetActive(false);
		bReady = false;
		bClick = false;
		round = 0;
		mCanSendRequest = false;
		uiRank.SetActive(false);
		uiReady.SetActive(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		CheckPlayerNumber();
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null)
		{
			GetVSTDMRankRequest request = new GetVSTDMRankRequest(userState.GetVSTDMStatsId());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void CheckPlayerNumber()
	{
		if (GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
			.Count > 0)
		{
			if (m1v1CheckBox.gameObject.activeSelf)
			{
				m1v1CheckBox.gameObject.SetActive(false);
			}
			if (!m4v4CheckBox.isChecked)
			{
				m4v4CheckBox.isChecked = true;
			}
		}
		else if (!m1v1CheckBox.gameObject.activeSelf)
		{
			m1v1CheckBox.gameObject.SetActive(true);
		}
	}

	protected override void Update()
	{
		base.Update();
		CheckPlayerNumber();
		if (bReady && GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
			.Count == 0 && mCanSendRequest && Time.time - lastTimeSendRequest > mSendRequestInterval)
		{
			SendQuickMatchRequest(curMode);
		}
	}

	private void On1v1Select(bool select)
	{
		if (select)
		{
			curMode = Mode.CaptureHold_1v1;
			VSChangeSubModeRequest request = new VSChangeSubModeRequest(curMode);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void On4v4Select(bool select)
	{
		if (select)
		{
			curMode = Mode.CaptureHold_4v4;
			VSChangeSubModeRequest request = new VSChangeSubModeRequest(curMode);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void NotifyStopMatch()
	{
		base.NotifyStopMatch();
		UILoadingNet.m_instance.Hide();
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_PVP_NO_ROOM"), 2, 52);
	}

	public override void NotifyAllReady()
	{
		base.NotifyAllReady();
		bReady = true;
		readyButton.SetActive(false);
		readyButtonLock.SetActive(true);
	}

	public override void NotifyMatchSuccess()
	{
		UILoadingNet.m_instance.Hide();
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (!GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.IsPVPReady && Time.time - lastTimeClick > 5f)
		{
			if (go.Equals(readyButton))
			{
				bClick = true;
				lastTimeClick = Time.time;
				UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_PVP_NO_ROOM"), 1000f, 52, this);
				VSReadyRequest request = new VSReadyRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else if (go.Equals(rankButton) && !bClick)
			{
				Debug.Log("Show Rank");
				uiRank.SetActive(true);
				uiReady.SetActive(false);
			}
			else if (go.Equals(rankBackButton) && !bClick)
			{
				Debug.Log("Show Ready");
				uiRank.SetActive(false);
				uiReady.SetActive(true);
			}
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 52)
		{
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.IsPVPReady = false;
			bClick = false;
			SendRestartToReadyRequest();
		}
	}

	private void SendRestartToReadyRequest()
	{
	}
}
