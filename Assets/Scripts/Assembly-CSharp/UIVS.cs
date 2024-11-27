using UnityEngine;

public abstract class UIVS : UIGameMenu
{
	public enum Mode
	{
		CaptureHold_1v1 = 0,
		CaptureHold_4v4 = 1
	}

	public static UIVS instance;

	public UIVSUserInfoManager teamInfoManager;

	private float mLastUpdateTime;

	private static byte prevState;

	protected bool mCanSendRequest;

	protected float lastTimeSendRequest;

	protected byte round;

	protected float mSendRequestInterval = 10f;

	protected override byte InitMask()
	{
		return 2;
	}

	protected override bool PauseGame()
	{
		return false;
	}

	protected override void Awake()
	{
		base.Awake();
		instance = this;
		SetMenuCloseOnDestroy(true);
		mCanSendRequest = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		instance = null;
	}

	protected virtual void Update()
	{
		if (Time.time - mLastUpdateTime > 1.5f)
		{
			mLastUpdateTime = Time.time;
			teamInfoManager.UpdatePlayer(UserStateHUD.GetInstance().GetAllPlayerList());
		}
	}

	protected void SendQuickMatchRequest(Mode mode)
	{
		lastTimeSendRequest = Time.time;
		mCanSendRequest = false;
		VSQuickMatchRequest request = new VSQuickMatchRequest(mode, round);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}

	public virtual void NotifyLoginSuccess()
	{
		Debug.Log("LoginSuccess");
	}

	public virtual void NotifyAllReady()
	{
		Debug.Log("AllReady");
	}

	public virtual void NotifyGetRankSuccess()
	{
		Debug.Log("GetRankSuccess");
		teamInfoManager.UpdatePlayer(UserStateHUD.GetInstance().GetAllPlayerList());
	}

	public virtual void NotifyMatch1v1Fail(byte interval)
	{
		Debug.Log("Match 1v1 fail");
		mCanSendRequest = true;
		lastTimeSendRequest = Time.time;
		mSendRequestInterval = (int)interval;
		round++;
	}

	public virtual void NotifyStopMatch()
	{
		Debug.Log("Stop Match 1v1");
		round = 0;
	}

	public virtual void NotifyMatchSuccess()
	{
	}

	public static void Show()
	{
		if (GameApp.GetInstance().GetGameMode().TypeOfNetwork == NetworkType.Single)
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(37, false, false, true);
		}
		else
		{
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(38, false, false, true);
		}
	}

	public static bool IsShow()
	{
		return instance != null;
	}

	public static void Close()
	{
		GameApp.GetInstance().CloseConnectionGameServer();
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
	}
}
