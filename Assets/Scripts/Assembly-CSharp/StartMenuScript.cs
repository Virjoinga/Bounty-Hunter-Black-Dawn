using System;
using System.Collections;
using UnityEngine;

public class StartMenuScript : UIStateManager, UIMsgListener
{
	private float startTime;

	public NetworkManager networkMgr;

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_START;

	private static byte LOADING_SUBSTATE_RES = 1;

	private static byte LOADING_SUBSTATE_ICLOUD = 2;

	private static byte LOADING_SUBSTATE_USERSTATE = 3;

	private static byte LOADING_SUBSTATE_AVATAR = 4;

	private bool bGiveRewards;

	public static bool bFirstLaunch = true;

	private GameObject sceneObj;

	protected GameObject m_StartUI;

	protected GameObject m_CreditUI;

	protected GameObject m_SettingUI;

	protected GameObject m_loadingUI;

	protected GameObject m_CharacterUI;

	protected GameObject m_AvatarShopUI;

	protected GameObject m_vsTeamUI;

	protected GameObject m_BossRushUI;

	protected GameObject m_BossRushRoomUI;

	private void Awake()
	{
	}

	private void Start()
	{
		Res2DManager.GetInstance().Init();
		LoadingState = LOADING_SUBSTATE_RES;
		GameApp.GetInstance().SetUIStateManager(this);
		GameApp.GetInstance().GetGameMode().TypeOfNetwork = NetworkType.Single;
		FrGoToPhase(0, false, true, true);
		TimeManager.GetInstance().Init();
		TimeManager.GetInstance().setMaxLoopTimes(-1);
		TimeManager.GetInstance().setPeriod(3f);
		if (string.IsNullOrEmpty(GameApp.GetInstance().UUID))
		{
			GameApp.GetInstance().UUID = AndroidPluginScript.GetAndroidId();
		}
		Debug.Log("UDID = " + GameApp.GetInstance().UUID);
		GameApp.GetInstance().StartHttpRequestThread();
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			StartCoroutine(SetAds());
		}
	}

	private void InitGlobalState()
	{
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		if (!globalState.bInit)
		{
			Debug.Log("InitGlobalState...");
			globalState.LoadConfig();
			GameApp.loadRMS loadRMS = GameApp.GetInstance().Load();
			switch (loadRMS)
			{
			case GameApp.loadRMS.RMSEmpty:
				Debug.Log("no RMS....." + loadRMS);
				globalState.Init();
				break;
			case GameApp.loadRMS.RMSException:
			case GameApp.loadRMS.RMSMismatch:
				Debug.Log("exception when user is loading RMS....." + loadRMS);
				break;
			}
			globalState.bInit = true;
		}
	}

	public override void FrFree()
	{
	}

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
		{
			GlobalState globalState = GameApp.GetInstance().GetGlobalState();
			if (!globalState.bInit)
			{
				Res2DManager.GetInstance().SetResData(0);
				Res2DManager.GetInstance().SetResData(1);
				Res2DManager.GetInstance().SetResData(2);
				Res2DManager.GetInstance().SetResData(3);
				Res2DManager.GetInstance().SetResData(4);
				Res2DManager.GetInstance().SetResData(5);
				Res2DManager.GetInstance().SetResData(6);
				Res2DManager.GetInstance().SetResData(7);
				Res2DManager.GetInstance().SetResData(8);
				Res2DManager.GetInstance().SetResData(9);
				Res2DManager.GetInstance().SetResData(10);
				Res2DManager.GetInstance().SetResData(11);
				Res2DManager.GetInstance().SetResData(12);
				Res2DManager.GetInstance().SetResData(13);
				Res2DManager.GetInstance().SetResData(14);
				Res2DManager.GetInstance().SetResData(15);
				Res2DManager.GetInstance().SetResData(16);
				Res2DManager.GetInstance().SetResData(17);
				Res2DManager.GetInstance().SetResData(18);
				Res2DManager.GetInstance().SetResData(19);
				Res2DManager.GetInstance().SetResData(20);
				Res2DManager.GetInstance().SetResData(21);
				Res2DManager.GetInstance().SetResData(22);
				Res2DManager.GetInstance().SetResData(23);
				Res2DManager.GetInstance().SetResData(24);
				Res2DManager.GetInstance().SetResData(25);
				Res2DManager.GetInstance().SetResData(26);
				Res2DManager.GetInstance().SetResData(27);
				Res2DManager.GetInstance().SetResData(28);
				Res2DManager.GetInstance().SetResData(43);
				Res2DManager.GetInstance().SetResData(73);
				Res2DManager.GetInstance().SetResData(40);
				Res2DManager.GetInstance().SetResData(41);
				Res2DManager.GetInstance().SetResData(42);
				Res2DManager.GetInstance().SetResData(44);
				Res2DManager.GetInstance().SetResData(45);
				Res2DManager.GetInstance().SetResData(52);
				Res2DManager.GetInstance().SetResData(46);
				Res2DManager.GetInstance().SetResData(47);
				Res2DManager.GetInstance().SetResData(48);
				Res2DManager.GetInstance().SetResData(50);
				Res2DManager.GetInstance().SetResData(49);
				Res2DManager.GetInstance().SetResData(38);
				Res2DManager.GetInstance().SetResData(51);
				Res2DManager.GetInstance().SetResData(37);
				Res2DManager.GetInstance().SetResData(29);
				Res2DManager.GetInstance().SetResData(30, 36);
				Res2DManager.GetInstance().SetResData(53, 59);
				Res2DManager.GetInstance().SetResData(74);
				Res2DManager.GetInstance().SetResData(75);
			}
			StartLoading(0, 50);
			break;
		}
		case 1:
			if (m_loadingUI == null)
			{
				GameObject original6 = ResourceLoad.GetInstance().LoadUI("Loading", "LoadingUI");
				m_loadingUI = UnityEngine.Object.Instantiate(original6) as GameObject;
				LoadingState = LOADING_SUBSTATE_START;
			}
			break;
		case 2:
			if (m_StartUI == null)
			{
				GameObject original10 = ResourceLoad.GetInstance().LoadUI("MainMenuX", "StartUI");
				m_StartUI = UnityEngine.Object.Instantiate(original10) as GameObject;
			}
			else
			{
				m_StartUI.SetActive(true);
			}
			break;
		case 18:
			if (m_CreditUI == null)
			{
				GameObject original4 = ResourceLoad.GetInstance().LoadUI("MainMenuX", "CreditUI");
				m_CreditUI = UnityEngine.Object.Instantiate(original4) as GameObject;
			}
			else
			{
				m_CreditUI.SetActive(true);
			}
			break;
		case 19:
			if (m_SettingUI == null)
			{
				GameObject original8 = ResourceLoad.GetInstance().LoadUI("MainMenuX", "SettingUI");
				m_SettingUI = UnityEngine.Object.Instantiate(original8) as GameObject;
			}
			else
			{
				m_SettingUI.SetActive(true);
			}
			break;
		case 20:
			if (m_CharacterUI == null)
			{
				GameObject original2 = ResourceLoad.GetInstance().LoadUI("MainMenuX", "CharacterUI");
				m_CharacterUI = UnityEngine.Object.Instantiate(original2) as GameObject;
			}
			else
			{
				m_CharacterUI.SetActive(true);
			}
			break;
		case 39:
			if (m_AvatarShopUI == null)
			{
				GameObject original9 = ResourceLoad.GetInstance().LoadUI("AvatarShop", "AvatarShopUI");
				m_AvatarShopUI = UnityEngine.Object.Instantiate(original9) as GameObject;
			}
			break;
		case 3:
			if (m_StartUI == null)
			{
				GameObject original7 = ResourceLoad.GetInstance().LoadUI("MainMenuX", "StartUI");
				m_StartUI = UnityEngine.Object.Instantiate(original7) as GameObject;
			}
			else
			{
				m_StartUI.SetActive(true);
			}
			break;
		case 41:
			if (m_vsTeamUI == null)
			{
				MainScriptInStartMenu component2 = base.gameObject.GetComponent<MainScriptInStartMenu>();
				component2.Init();
				GameObject original5 = ResourceLoad.GetInstance().LoadUI("VS", "VSStartUITeam");
				m_vsTeamUI = UnityEngine.Object.Instantiate(original5) as GameObject;
				UIVSTeam componentInChildren = m_vsTeamUI.GetComponentInChildren<UIVSTeam>();
				componentInChildren.ShowRoomList();
				InGameMenuManager.GetInstance().HideHUD();
				Time.timeScale = 1f;
			}
			break;
		case 42:
			if (m_BossRushUI == null)
			{
				GameObject original3 = ResourceLoad.GetInstance().LoadUI("BossRush", "BossRushUI");
				m_BossRushUI = UnityEngine.Object.Instantiate(original3) as GameObject;
			}
			break;
		case 43:
			if (m_BossRushRoomUI == null)
			{
				GameObject original = ResourceLoad.GetInstance().LoadUI("BossRush", "BossRushRoomUI");
				m_BossRushRoomUI = UnityEngine.Object.Instantiate(original) as GameObject;
				UIBossRushTeam component = m_BossRushRoomUI.GetComponent<UIBossRushTeam>();
				component.ShowRoomList();
				InGameMenuManager.GetInstance().HideHUD();
				Time.timeScale = 1f;
			}
			break;
		}
	}

	protected override void FrClose(int phase)
	{
		switch (phase)
		{
		case 1:
			MemoryManager.FreeNGUI(m_loadingUI);
			m_loadingUI = null;
			break;
		case 2:
			m_StartUI.SetActive(false);
			break;
		case 18:
			m_CreditUI.SetActive(false);
			break;
		case 19:
			m_SettingUI.SetActive(false);
			break;
		case 20:
			m_CharacterUI.SetActive(false);
			break;
		case 39:
			if (m_AvatarShopUI != null)
			{
				MemoryManager.FreeNGUI(m_AvatarShopUI);
				m_AvatarShopUI = null;
			}
			break;
		case 41:
			MemoryManager.FreeNGUI(m_vsTeamUI);
			m_vsTeamUI = null;
			break;
		case 42:
			MemoryManager.FreeNGUI(m_BossRushUI);
			m_BossRushUI = null;
			break;
		case 43:
			MemoryManager.FreeNGUI(m_BossRushRoomUI);
			m_BossRushRoomUI = null;
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		switch (phase)
		{
		case 0:
			FrGoToPhase(2, false, false, false);
			break;
		case 1:
			if (FrGetPreviousPhase() != 0)
			{
				break;
			}
			if (LoadingState == LOADING_SUBSTATE_START)
			{
				UILoading.m_instance.PlayAnimation("RPG_anim_150", WrapMode.ClampForever, 0f);
				LoadingState = LOADING_SUBSTATE_RES;
			}
			else if (LoadingState == LOADING_SUBSTATE_RES)
			{
				if (Res2DManager.GetInstance().LoadResPro())
				{
					LoadingState = LOADING_SUBSTATE_AVATAR;
				}
				UILoading.m_instance.SetAnimPercent((float)(int)Res2DManager.GetInstance().byLoadPercent / 100f);
			}
			else if (LoadingState != LOADING_SUBSTATE_ICLOUD && LoadingState == LOADING_SUBSTATE_AVATAR)
			{
				UILoading.m_instance.SetAnimPercent(1f);
				if (UILoading.m_instance.IsCompleted())
				{
					Resources.UnloadUnusedAssets();
					InitGlobalState();
					AudioManager.GetInstance().PlayMusic("RPG_Audio/BGM/BGM_c0_c3_menu");
					FrGoToPhase(FrGetPreviousPhase(), true, false, false);
				}
			}
			break;
		case 2:
			break;
		case 18:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 10:
			break;
		case 8:
			break;
		case 11:
			break;
		case 6:
		case 7:
		case 9:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
			break;
		}
	}

	public void UpLoadData()
	{
		UploadDataRequest request = new UploadDataRequest();
		networkMgr.SendRequest(request);
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton eventID)
	{
		if (whichMsg.EventId == 57)
		{
			UIMsgBox.instance.CloseMessage();
			GameApp.GetInstance().GetGlobalState().Init();
		}
	}

	private IEnumerator CheckAdColony()
	{
		string deviceid = GameApp.GetInstance().UUID;
		string productid = "486314228";
		string apikey = "ff12c9401a731df69a9190229eac27ea";
		string checkurl = "http://cpa.adtilt.com/has_user_action?api_key=" + apikey + "&product_id=" + productid + "&device_id=" + deviceid;
		string checking = string.Empty;
		WWW checkingad2 = new WWW(checkurl);
		while (!checkingad2.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (checkingad2 != null && checkingad2.error == null && checkingad2.bytes != null && checkingad2.bytes.Length > 0)
			{
				string checkresponse = checkingad2.text;
				Hashtable checkres = checkresponse.hashtableFromJson();
				checking = checkres["status"].ToString();
			}
		}
		catch (Exception ex3)
		{
			Exception ex2 = ex3;
			Debug.Log(ex2.Message);
		}
		finally
		{
			if (checkingad2 != null)
			{
				checkingad2.Dispose();
				checkingad2 = null;
			}
		}
		if (checking.Equals("True"))
		{
			yield break;
		}
		string reporturl = "http://cpa.adtilt.com/on_user_action?api_key=" + apikey + "&product_id=" + productid + "&raw_udid=" + deviceid + "&odin1=" + deviceid;
		WWW reportingad2 = new WWW(reporturl);
		while (!reportingad2.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (reportingad2 != null && reportingad2.error == null && reportingad2.bytes != null && reportingad2.bytes.Length > 0)
			{
				string reportresponse = reportingad2.text;
				Hashtable reportres = reportresponse.hashtableFromJson();
				string bolreport = (string)reportres["status"];
			}
		}
		catch (Exception ex4)
		{
			Exception ex = ex4;
			Debug.Log(ex.Message);
		}
		finally
		{
			if (reportingad2 != null)
			{
				reportingad2.Dispose();
				reportingad2 = null;
			}
		}
	}

	private IEnumerator SetAds()
	{
		WWW getAppStatusWWW2 = null;
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			getAppStatusWWW2 = new WWW("http://174.36.196.91:8088/AdvertServer/GetAppStatus?appcode=ag003&v=126");
			break;
		case AndroidConstant.Version.Kindle:
			getAppStatusWWW2 = new WWW("http://174.36.196.91:8088/AdvertServer/GetAppStatus?appcode=ag003&v=126");
			break;
		}
		while (!getAppStatusWWW2.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (getAppStatusWWW2 != null && getAppStatusWWW2.error == null && getAppStatusWWW2.bytes != null && getAppStatusWWW2.bytes.Length > 0)
			{
				byte[] appStatusBytes = getAppStatusWWW2.bytes;
				BytesBuffer bb = new BytesBuffer(appStatusBytes);
				AdsManager.GetInstance().DownLoadAdsStatus(bb);
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(ex.Message);
		}
		finally
		{
			if (getAppStatusWWW2 != null)
			{
				getAppStatusWWW2.Dispose();
				getAppStatusWWW2 = null;
			}
		}
	}

	private IEnumerator IAPChecker()
	{
		WWW getAppStatusWWW = new WWW("http://174.36.196.91:8088/AdvertServer/GetAppStatus?appcode=ig003");
		while (!getAppStatusWWW.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (getAppStatusWWW != null && getAppStatusWWW.error == null && getAppStatusWWW.bytes != null && getAppStatusWWW.bytes.Length > 0)
			{
				byte[] appStatusBytes = getAppStatusWWW.bytes;
				BytesBuffer bb = new BytesBuffer(appStatusBytes);
				if (bb.ReadShort() == 0)
				{
					byte status = bb.ReadByte();
					GameApp.GetInstance().AppStatus = status;
					Debug.Log("appstatus:" + status);
				}
			}
		}
		catch (Exception ex3)
		{
			Exception ex2 = ex3;
			Debug.Log(ex2.Message);
		}
		finally
		{
			if (getAppStatusWWW != null)
			{
				getAppStatusWWW.Dispose();
				getAppStatusWWW = null;
			}
		}
		WWW getBase64MinLengthWWW2 = new WWW("http://174.36.196.91:7671/IapStatServer/GetVerifyLen?appcode=ig003");
		while (!getBase64MinLengthWWW2.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			if (getBase64MinLengthWWW2 != null && getBase64MinLengthWWW2.error == null && getBase64MinLengthWWW2.bytes != null && getBase64MinLengthWWW2.bytes.Length > 0)
			{
				GameApp.GetInstance().Base64MinLength = int.Parse(getBase64MinLengthWWW2.text);
				Debug.Log("Base64MinLength:" + GameApp.GetInstance().Base64MinLength);
			}
		}
		catch (Exception ex4)
		{
			Exception ex = ex4;
			Debug.Log(ex.Message);
		}
		finally
		{
			if (getBase64MinLengthWWW2 != null)
			{
				getBase64MinLengthWWW2.Dispose();
				getBase64MinLengthWWW2 = null;
			}
		}
	}
}
