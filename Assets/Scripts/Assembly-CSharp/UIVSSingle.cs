using UnityEngine;

public class UIVSSingle : UIVS, UIMsgListener
{
	public GameObject startButton;

	public GameObject rankBackButton;

	public GameObject rankButton;

	public GameObject uiRank;

	public GameObject uiReady;

	public GameObject uiRewards;

	private Mode curMode;

	private bool bClick;

	protected override void Awake()
	{
		base.Awake();
		AddDelegate(startButton);
		AddDelegate(rankBackButton);
		AddDelegate(rankButton);
		bClick = false;
		round = 0;
		uiRank.SetActive(false);
		uiRewards.SetActive(false);
		uiReady.SetActive(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			GameApp.GetInstance().GetUserState().LoginAsUser();
			UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_LOGIN_TIMEOUT"), 25f, 18, this);
			Debug.Log("Login...");
		}
		else
		{
			UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_NET_CAN_NOT_ACCESS_SERVER"), 1f, 11, this);
		}
	}

	protected override void Update()
	{
		base.Update();
		if ((UIVSTeam.giftDaily || UITeam.giftDaily || UIBossRushTeam.giftDaily) && !uiRewards.activeSelf)
		{
			uiRewards.SetActive(true);
		}
		if (bClick && mCanSendRequest && Time.time - lastTimeSendRequest > mSendRequestInterval)
		{
			SendQuickMatchRequest(curMode);
		}
	}

	private void On1v1Select(bool select)
	{
		if (select)
		{
			curMode = Mode.CaptureHold_1v1;
		}
	}

	private void On4v4Select(bool select)
	{
		if (select)
		{
			curMode = Mode.CaptureHold_4v4;
		}
	}

	public override void NotifyStopMatch()
	{
		base.NotifyStopMatch();
		UILoadingNet.m_instance.Hide();
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_PVP_NO_ROOM"), 2, 52);
	}

	public override void NotifyLoginSuccess()
	{
		base.NotifyLoginSuccess();
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null)
		{
			GetVSTDMRankRequest request = new GetVSTDMRankRequest(userState.GetVSTDMStatsId());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void NotifyGetRankSuccess()
	{
		base.NotifyGetRankSuccess();
		UILoadingNet.m_instance.Hide();
	}

	public override void NotifyMatchSuccess()
	{
		base.NotifyStopMatch();
		UILoadingNet.m_instance.Hide();
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (!bClick)
		{
			if (go.Equals(startButton))
			{
				bClick = true;
				Debug.Log("Start PVP : " + curMode);
				UILoadingNet.m_instance.ShowSystem(LocalizationManager.GetInstance().GetString("MSG_PVP_NO_ROOM"), 1000f, 52, this);
				SendQuickMatchRequest(curMode);
			}
			else if (go.Equals(rankButton))
			{
				Debug.Log("Show Rank");
				uiRank.SetActive(true);
				uiReady.SetActive(false);
			}
			else if (go.Equals(rankBackButton))
			{
				Debug.Log("Show Ready");
				uiRank.SetActive(false);
				uiReady.SetActive(true);
			}
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (whichMsg.EventId == 17 || whichMsg.EventId == 18 || whichMsg.EventId == 11)
		{
			UIMsgBox.instance.CloseMessage();
			UIVS.Close();
		}
		else if (whichMsg.EventId == 52)
		{
			UIMsgBox.instance.CloseMessage();
			bClick = false;
		}
	}
}
