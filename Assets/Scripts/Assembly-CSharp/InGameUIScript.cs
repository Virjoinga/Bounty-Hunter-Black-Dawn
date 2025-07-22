using System;
using System.Collections;
using UnityEngine;

public class InGameUIScript : UIStateManager, EffectsCameraListener, UIMsgListener
{
	public static bool bInited;

	public NetworkManager networkMgr;

	private int dataIndex;

	private byte LoadingState;

	private static byte LOADING_SUBSTATE_START;

	private static byte LOADING_SUBSTATE_RES = 1;

	private static byte LOADING_SUBSTATE_CAMERA = 2;

	private static byte LOADING_SUBSTATE_MAINSCRIPT = 3;

	private static byte LOADING_SUBSTATE_PREFAB = 4;

	private static byte LOADING_SUBSTATE_COMPLETED = 5;

	protected GameObject m_DropItemUI;

	protected GameObject m_loadingUI;

	protected GameObject m_HUDUI;

	protected GameObject m_bubbleUI;

	protected GameObject m_questUI;

	protected GameObject m_teamUI;

	protected GameObject m_bagUI;

	protected GameObject m_skillTreeUI;

	protected GameObject m_MiniMapUI;

	protected GameObject m_OptionUI;

	protected GameObject m_InGameUI;

	protected GameObject m_EffectsCamera;

	protected GameObject m_Entrance;

	protected GameObject m_SettingUI;

	protected GameObject m_ShopUI;

	protected GameObject m_AchievementUI;

	protected GameObject m_ArenaUI;

	protected GameObject m_GambleUI;

	protected GameObject m_gameMian;

	protected GameObject m_AdsVideoUI;

	protected GameObject m_AdsNoVideoUI;

	protected GameObject m_VSUI;

	protected GameObject m_EquipmentUpgradeUI;

	protected GameObject m_vsTeamUI;

	protected GameObject m_BossRushUI;

	protected GameObject m_BossRushRoomUI;

	protected Timer m_VarifyHackPlayerTimer = new Timer();

	private byte m_sceneId;

	private void Awake()
	{
		bInited = false;
	}

	private void Start()
	{
		LoadingState = LOADING_SUBSTATE_START;
		GameApp.GetInstance().SetUIStateManager(this);
		FrGoToPhase(0, false, true, true);
		Time.timeScale = 1f;
		if ((DateTime.Now - GameApp.UploadAnalysis).TotalHours >= 24.0)
		{
			Debug.Log("UploadAnalysis...");
			GameApp.UploadAnalysis = DateTime.Now;
			if (GameApp.GetInstance().IsConnectedToInternet())
			{
				StartCoroutine(UploadAnalysis());
			}
		}
		m_VarifyHackPlayerTimer.SetTimer(Global.VARIFY_HACK_PLAYER_TIME_INTERVAL, false);
	}

	public GameObject InitQuestUI()
	{
		if (m_questUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Quest", "QuestUI");
			m_questUI = UnityEngine.Object.Instantiate(original) as GameObject;
		}
		return m_questUI;
	}

	public GameObject InitBubbleUI()
	{
		if (m_bubbleUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Bubble", "BubbleUI");
			m_bubbleUI = UnityEngine.Object.Instantiate(original) as GameObject;
		}
		return m_bubbleUI;
	}

	public void DestroyBubbleUI()
	{
		if (m_bubbleUI != null)
		{
			UnityEngine.Object.Destroy(m_bubbleUI);
			m_bubbleUI = null;
		}
	}

	public GameObject GetBubbleUI()
	{
		return m_bubbleUI;
	}

	public void InitQuestLogsUI()
	{
		if (m_questUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Quest", "QuestUI");
			m_questUI = UnityEngine.Object.Instantiate(original) as GameObject;
			UIQuest component = m_questUI.GetComponent<UIQuest>();
			component.ShowLogs();
		}
	}

	public void DestroyQuestUI()
	{
		if (m_questUI != null)
		{
			MemoryManager.FreeNGUI(m_questUI);
			m_questUI = null;
		}
	}

	public void InitTeamQuestUI()
	{
		if (m_questUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Quest", "QuestUI");
			m_questUI = UnityEngine.Object.Instantiate(original) as GameObject;
			UIQuest component = m_questUI.GetComponent<UIQuest>();
			component.ShowTeamQuest();
		}
	}

	public GameObject InitBagUI()
	{
		if (m_bagUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Bag", "BagUI");
			m_bagUI = UnityEngine.Object.Instantiate(original, Vector3.up * -1000f, Quaternion.identity) as GameObject;
		}
		return m_bagUI;
	}

	public void DestroyBagUI()
	{
		if (m_bagUI != null)
		{
			MemoryManager.FreeNGUI(m_bagUI, false);
			m_bagUI = null;
		}
	}

	public GameObject InitSkillTreeUI()
	{
		if (m_skillTreeUI == null)
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("SkillTree", "SkillTreeUI");
			m_skillTreeUI = UnityEngine.Object.Instantiate(original, Vector3.up * -1000f, Quaternion.identity) as GameObject;
		}
		return m_skillTreeUI;
	}

	public void DestroySkillTreeUI()
	{
		if (m_skillTreeUI != null)
		{
			MemoryManager.FreeNGUI(m_skillTreeUI);
			m_skillTreeUI = null;
		}
	}

	protected override void FrInit(int phase)
	{
		switch (phase)
		{
		case 0:
			Res2DManager.GetInstance().SetResData(39);
			StartLoading(0, 50);
			break;
		case 1:
			if (m_EffectsCamera == null)
			{
				GameObject original13 = ResourceLoad.GetInstance().LoadUI("EffectsCamera", "EffectsCamera");
				m_EffectsCamera = UnityEngine.Object.Instantiate(original13) as GameObject;
				EffectsCamera.instance.m_bManual = true;
				LoadingState = LOADING_SUBSTATE_START;
			}
			break;
		case 6:
			if (TutorialManager.GetInstance().IsCanCreateTutorial())
			{
				TutorialManager.GetInstance().CreateTutorialPrefab();
			}
			break;
		case 17:
		{
			InitQuestUI();
			UIQuest component5 = m_questUI.GetComponent<UIQuest>();
			if (component5 != null)
			{
				component5.SetQuestNpc(UIQuest.m_npcId);
				component5.ShowQuests();
				InGameMenuManager.GetInstance().HideHUD();
			}
			LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer2.inputController.Block = true;
			localPlayer2.SetState(Player.TALK_STATE);
			break;
		}
		case 24:
			InitQuestUI();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				UIQuest.m_instance.ShowTeamQuest();
			}
			else
			{
				UIQuest.m_instance.ShowLogs();
			}
			break;
		case 21:
		{
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			if (gameWorld != null && gameWorld.GetLocalPlayer() != null)
			{
				gameWorld.GetLocalPlayer().StopSpecialAction();
			}
			InitBubbleUI();
			UIBubble component2 = m_bubbleUI.GetComponent<UIBubble>();
			if (component2 != null)
			{
				if (!string.IsNullOrEmpty(UIBubble.m_text))
				{
					component2.m_normal.SetActive(true);
				}
				else
				{
					component2.Init();
					component2.Logic();
				}
			}
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer.inputController.Block = true;
			localPlayer.SetState(Player.TALK_STATE);
			break;
		}
		case 41:
			if (m_vsTeamUI == null)
			{
				GameObject original17 = ResourceLoad.GetInstance().LoadUI("VS", "VSUITeam");
				m_vsTeamUI = UnityEngine.Object.Instantiate(original17) as GameObject;
				UIVSTeam component4 = m_vsTeamUI.GetComponent<UIVSTeam>();
				component4.ShowRoomList();
				InGameMenuManager.GetInstance().HideHUD();
				Time.timeScale = 1f;
			}
			break;
		case 9:
			if (m_teamUI == null)
			{
				GameObject original15 = ResourceLoad.GetInstance().LoadUI("Team", "TeamUI");
				m_teamUI = UnityEngine.Object.Instantiate(original15) as GameObject;
				UITeam component3 = m_teamUI.GetComponent<UITeam>();
				component3.ShowRoomList();
				InGameMenuManager.GetInstance().HideHUD();
				Time.timeScale = 1f;
			}
			break;
		case 7:
			break;
		case 16:
			break;
		case 14:
			break;
		case 15:
			if (m_OptionUI == null)
			{
				GameObject original11 = ResourceLoad.GetInstance().LoadUI("Option", "OptionUI");
				m_OptionUI = UnityEngine.Object.Instantiate(original11) as GameObject;
			}
			break;
		case 22:
			InitBagUI();
			break;
		case 23:
			InitSkillTreeUI();
			break;
		case 25:
			if (m_MiniMapUI == null)
			{
				GameObject original6 = ResourceLoad.GetInstance().LoadUI("MiniMapX", "MiniMapUI");
				m_MiniMapUI = UnityEngine.Object.Instantiate(original6) as GameObject;
			}
			break;
		case 26:
			if (m_AchievementUI == null)
			{
				GameObject original3 = ResourceLoad.GetInstance().LoadUI("Achievement", "AchievementUI");
				m_AchievementUI = UnityEngine.Object.Instantiate(original3) as GameObject;
			}
			break;
		case 27:
			if (m_Entrance == null)
			{
				GameObject original18 = ResourceLoad.GetInstance().LoadUI("Portal", "PortalUI");
				m_Entrance = UnityEngine.Object.Instantiate(original18) as GameObject;
			}
			break;
		case 19:
			if (m_SettingUI == null)
			{
				GameObject original16 = ResourceLoad.GetInstance().LoadUI("InGameMenu", "InGameSetting");
				m_SettingUI = UnityEngine.Object.Instantiate(original16) as GameObject;
			}
			else
			{
				m_SettingUI.SetActiveRecursively(true);
			}
			SetInGameMenuVisible(false);
			break;
		case 28:
			if (m_ShopUI == null)
			{
				GameObject original14 = ResourceLoad.GetInstance().LoadUI("ShopUI", "ShopUI");
				m_ShopUI = UnityEngine.Object.Instantiate(original14, Vector3.up * -1000f, Quaternion.identity) as GameObject;
			}
			break;
		case 29:
			if (m_ArenaUI == null)
			{
				GameObject original12 = ResourceLoad.GetInstance().LoadUI("Arena", "ArenaUI");
				m_ArenaUI = UnityEngine.Object.Instantiate(original12) as GameObject;
			}
			break;
		case 34:
			if (m_GambleUI == null)
			{
				GameObject original10 = ResourceLoad.GetInstance().LoadUI("FruitMachine", "FruitMachineUI");
				m_GambleUI = UnityEngine.Object.Instantiate(original10) as GameObject;
			}
			break;
		case 35:
			if (m_AdsVideoUI == null)
			{
				GameObject original9 = ResourceLoad.GetInstance().LoadUI("Ads", "AdsVideoUI");
				m_AdsVideoUI = UnityEngine.Object.Instantiate(original9) as GameObject;
			}
			break;
		case 36:
			if (m_AdsNoVideoUI == null)
			{
				GameObject original8 = ResourceLoad.GetInstance().LoadUI("Ads", "AdsNoVideoUI");
				m_AdsNoVideoUI = UnityEngine.Object.Instantiate(original8) as GameObject;
			}
			break;
		case 37:
			if (m_VSUI == null)
			{
				GameObject original7 = ResourceLoad.GetInstance().LoadUI("VS", "VSUISingle");
				m_VSUI = UnityEngine.Object.Instantiate(original7) as GameObject;
			}
			break;
		case 38:
			if (m_VSUI == null)
			{
				GameObject original5 = ResourceLoad.GetInstance().LoadUI("VS", "VSUICoop");
				m_VSUI = UnityEngine.Object.Instantiate(original5) as GameObject;
			}
			break;
		case 40:
			if (m_EquipmentUpgradeUI == null)
			{
				GameObject original4 = ResourceLoad.GetInstance().LoadUI("EquipmentUpgrade", "EquipmentUpgradeUI");
				m_EquipmentUpgradeUI = UnityEngine.Object.Instantiate(original4) as GameObject;
			}
			break;
		case 42:
			if (m_BossRushUI == null)
			{
				GameObject original2 = ResourceLoad.GetInstance().LoadUI("BossRush", "BossRushUI");
				m_BossRushUI = UnityEngine.Object.Instantiate(original2) as GameObject;
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
		case 2:
		case 3:
		case 4:
		case 5:
		case 8:
		case 10:
		case 11:
		case 12:
		case 13:
		case 18:
		case 20:
		case 30:
		case 31:
		case 32:
		case 33:
		case 39:
			break;
		}
	}

	private void SetInGameMenuVisible(bool visible)
	{
		if (m_InGameUI != null)
		{
			m_InGameUI.SetActiveRecursively(visible);
		}
	}

	protected override void FrClose(int phase)
	{
		Debug.Log("close");
		Debug.Log("phase : " + phase);
		switch (phase)
		{
		case 0:
			break;
		case 1:
			break;
		case 6:
			TutorialManager.GetInstance().DestroyTutorialPrefab();
			break;
		case 17:
		{
			LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer2.SetState(Player.IDLE_STATE);
			localPlayer2.inputController.Block = false;
			MemoryManager.FreeNGUI(m_questUI);
			m_questUI = null;
			break;
		}
		case 24:
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && Lobby.GetInstance().IsMasterPlayer)
			{
				ChangeQuestMarkRequest request = new ChangeQuestMarkRequest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			MemoryManager.FreeNGUI(m_questUI);
			m_questUI = null;
			Debug.Log("DestroyQuestUI");
			break;
		case 21:
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer.SetState(Player.IDLE_STATE);
			localPlayer.inputController.Block = false;
			MemoryManager.FreeNGUI(m_bubbleUI);
			m_bubbleUI = null;
			break;
		}
		case 9:
			MemoryManager.FreeNGUI(m_teamUI);
			m_teamUI = null;
			break;
		case 41:
			MemoryManager.FreeNGUI(m_vsTeamUI);
			m_vsTeamUI = null;
			break;
		case 7:
			break;
		case 16:
			break;
		case 14:
			break;
		case 15:
			MemoryManager.FreeNGUI(m_OptionUI, false);
			m_OptionUI = null;
			break;
		case 22:
			DestroyBagUI();
			break;
		case 23:
			DestroySkillTreeUI();
			break;
		case 25:
			MemoryManager.FreeNGUI(m_MiniMapUI);
			m_MiniMapUI = null;
			break;
		case 27:
			MemoryManager.FreeNGUI(m_Entrance);
			break;
		case 19:
			MemoryManager.FreeNGUI(m_SettingUI);
			m_SettingUI = null;
			SetInGameMenuVisible(true);
			break;
		case 28:
			MemoryManager.FreeNGUI(m_ShopUI, false);
			m_ShopUI = null;
			break;
		case 26:
			MemoryManager.FreeNGUI(m_AchievementUI);
			m_AchievementUI = null;
			break;
		case 29:
			MemoryManager.FreeNGUI(m_ArenaUI);
			m_ArenaUI = null;
			break;
		case 34:
			MemoryManager.FreeNGUI(m_GambleUI);
			m_GambleUI = null;
			break;
		case 35:
			MemoryManager.FreeNGUI(m_AdsVideoUI);
			m_AdsVideoUI = null;
			break;
		case 36:
			MemoryManager.FreeNGUI(m_AdsNoVideoUI);
			m_AdsNoVideoUI = null;
			break;
		case 37:
		case 38:
			MemoryManager.FreeNGUI(m_VSUI);
			m_VSUI = null;
			break;
		case 40:
			MemoryManager.FreeNGUI(m_EquipmentUpgradeUI);
			m_EquipmentUpgradeUI = null;
			break;
		case 42:
			MemoryManager.FreeNGUI(m_BossRushUI);
			m_BossRushUI = null;
			break;
		case 43:
			MemoryManager.FreeNGUI(m_BossRushRoomUI);
			m_BossRushRoomUI = null;
			break;
		case 2:
		case 3:
		case 4:
		case 5:
		case 8:
		case 10:
		case 11:
		case 12:
		case 13:
		case 18:
		case 20:
		case 30:
		case 31:
		case 32:
		case 33:
		case 39:
			break;
		}
	}

	protected override void FrUpdate(int phase)
	{
		if (GameApp.GetInstance().GetGameMode().TypeOfNetwork == NetworkType.MultiPlayer_Internet && UpdateNetwork())
		{
			if (EnemySpawnScript.GetInstance() != null && EnemySpawnScript.GetInstance().IsDisconnectedInMultiplay)
			{
				TimeManager.GetInstance().SynStop();
				Time.timeScale = 0f;
				return;
			}
			if (UILoadingNet.m_instance != null)
			{
				UILoadingNet.m_instance.Hide();
			}
			UIMsgBox.instance.ShowSystemMessage(this, LocalizationManager.GetInstance().GetString("MSG_DISCONNECTION"), 2, 1);
			TimeManager.GetInstance().SynStop();
			Time.timeScale = 0f;
			return;
		}
		switch (phase)
		{
		case 0:
			FrGoToPhase(6, false, false, false);
			break;
		case 1:
			if (FrGetPreviousPhase() != 0)
			{
				break;
			}
			if (LoadingState == LOADING_SUBSTATE_START)
			{
				LoadingState = LOADING_SUBSTATE_RES;
			}
			else if (LoadingState == LOADING_SUBSTATE_RES)
			{
				if (Res2DManager.GetInstance().LoadResPro())
				{
					LoadingState = LOADING_SUBSTATE_CAMERA;
				}
			}
			else if (LoadingState == LOADING_SUBSTATE_CAMERA)
			{
				GameObject original = Resources.Load("Camera/FPSCamera") as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
				gameObject.name = "FPSCamera";
				GameObject original2 = ResourceLoad.GetInstance().LoadUI("HUD", "HUD");
				m_HUDUI = UnityEngine.Object.Instantiate(original2) as GameObject;
				Debug.Log("Load HUD....");
				LoadingState = LOADING_SUBSTATE_MAINSCRIPT;
			}
			else if (LoadingState == LOADING_SUBSTATE_MAINSCRIPT)
			{
				bool flag = false;
				GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
				if (gameWorld == null)
				{
					flag = true;
				}
				else
				{
					GameObject playerSpawnPoint = GameApp.GetInstance().GetGameWorld().GetPlayerSpawnPoint();
					if (null != playerSpawnPoint)
					{
						FirstSceneStreamingScript component = playerSpawnPoint.GetComponent<FirstSceneStreamingScript>();
						flag = !(null != component) || !component.enabled || component.FirstVolumeLoaded;
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					GameObject original3 = Resources.Load("Game/GameMain") as GameObject;
					m_gameMian = UnityEngine.Object.Instantiate(original3) as GameObject;
					MainScript component2 = m_gameMian.GetComponent<MainScript>();
					component2.Init();
					HUDManager.instance.LoadHUD(HUDManager.Type.Battle);
					LoadingState = LOADING_SUBSTATE_PREFAB;
				}
			}
			else if (LoadingState == LOADING_SUBSTATE_PREFAB)
			{
				GameObject original4 = ResourceLoad.GetInstance().LoadUI("HUD", "ItemPopMenu");
				m_DropItemUI = UnityEngine.Object.Instantiate(original4) as GameObject;
				ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
				if (itemInfoData.IsFirstTimeToRefreshShop)
				{
					itemInfoData.RefreshShopItems();
					GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.SetTimer(120f, false);
					BlackMarketIcon.BlackMarketCD = 120f;
					BlackMarketIcon.BlackMarketTimeToReady = 120f;
					itemInfoData.IsFirstTimeToRefreshShop = false;
				}
				else
				{
					GameApp.GetInstance().GetGameWorld().mShopItemRefreshTimer.SetTimer(itemInfoData.TimeToShopRefresh, false);
				}
				GameApp.GetInstance().GetGameWorld().mBlackMarketRefreshTimer.SetTimer(BlackMarketIcon.BlackMarketTimeToReady, false);
				GameApp.GetInstance().GetLootManager().CheckAndSpawnAreanaReward();
				LoadingState = LOADING_SUBSTATE_COMPLETED;
			}
			else if (LoadingState == LOADING_SUBSTATE_COMPLETED)
			{
				if (GameApp.GetInstance().GetGameScene().mapType == MapType.City)
				{
					GameApp.GetInstance().Save();
					Debug.Log("save data....");
				}
				Resources.UnloadUnusedAssets();
				bInited = true;
				EffectsCamera.instance.m_bManual = false;
				Debug.Log("bInited:" + bInited);
				FrGoToPhase(FrGetPreviousPhase(), true, false, false);
			}
			break;
		case 6:
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (GameApp.GetInstance().GetGameMode().PlayerStatus == PlayerStateNetwork.Playing)
				{
					VarifyHackPlayer();
				}
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					if (HUDManager.instance != null && HUDManager.instance.GetRunningHUDType() != HUDManager.Type.VSTDMBattle)
					{
						HUDManager.instance.LoadHUD(HUDManager.Type.VSTDMBattle);
					}
				}
				else if (HUDManager.instance != null && HUDManager.instance.GetRunningHUDType() != HUDManager.Type.CoopBattle)
				{
					HUDManager.instance.LoadHUD(HUDManager.Type.CoopBattle);
				}
			}
			else if (HUDManager.instance != null && HUDManager.instance.GetRunningHUDType() != HUDManager.Type.Battle)
			{
				HUDManager.instance.LoadHUD(HUDManager.Type.Battle);
			}
			break;
		}
	}

	public void RestartGame()
	{
		FrGoToPhase(6, false, false, false);
	}

	public bool UpdateNetwork()
	{
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		if (networkMgr != null)
		{
			if (networkMgr.IsDisconnected || !networkMgr.IsConnected())
			{
				return true;
			}
			if (networkMgr.IsDisconnected || networkMgr.IsDisplayErrorBox)
			{
				return true;
			}
		}
		return false;
	}

	public override void FrFree()
	{
		Res2DManager.GetInstance().FreeDataTable(39);
	}

	public void StartLoading(byte mode, int nMax)
	{
		Res2DManager.GetInstance().LoadResInit(mode, nMax);
		FrGoToPhase(1, false, true, false);
	}

	public bool bInitCompleted()
	{
		if (m_gameMian != null)
		{
			MainScript component = m_gameMian.GetComponent<MainScript>();
			if (component.bInit)
			{
				return true;
			}
		}
		return false;
	}

	public bool VerifyEnter()
	{
		if (FrGetCurrentPhase() != 6 && FrGetCurrentPhase() != 27)
		{
			return false;
		}
		if (UIMsgBox.instance.IsMessageShow())
		{
			return false;
		}
		return true;
	}

	public bool Enter(byte sceneId)
	{
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(sceneId);
		if (sceneConfig == null)
		{
			return false;
		}
		EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
		m_sceneId = sceneId;
		return true;
	}

	public void ShowCompenstaeForNet(int mithril)
	{
		if (UIMsgBox.instance != null && !UIMsgBox.instance.IsMessageShow())
		{
			string info = LocalizationManager.GetInstance().GetString("MSG_COMPENSTAE").Replace("%d", mithril.ToString());
			UIMsgBox.instance.ShowMessage(this, info, 2, 51);
		}
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		EffectsCamera.instance.RemoveListener();
		GameApp.GetInstance().GetGameMode().SubModePlay = SubMode.Story;
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		gameScene.LeaveScene();
		Debug.Log("citySceneID : " + m_sceneId);
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(m_sceneId);
		if (m_sceneId < 0)
		{
			Debug.Log("Error SceneID : " + m_sceneId);
		}
		Application.LoadLevel(sceneConfig.SceneFileName);
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		Debug.Log("onButton....");
		if (whichMsg.EventId == 1)
		{
			if (Arena.GetInstance().IsCurrentSceneArena())
			{
				UIMsgBox.instance.CloseMessage();
				EnemySpawnScript.GetInstance().DisconnectMulitplay();
				return;
			}
			Time.timeScale = 1f;
			GameApp.GetInstance().CloseConnectionGameServer();
			UIMsgBox.instance.CloseMessage();
			if (FrGetCurrentPhase() != 6)
			{
				InGameMenuManager.GetInstance().Close();
			}
			FrGoToPhase(6, true, false, false);
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			if (gameWorld != null)
			{
				gameWorld.ExitMultiplayerMode();
			}
		}
		else if (whichMsg.EventId == 51)
		{
			UIMsgBox.instance.CloseMessage();
		}
	}

	private IEnumerator UploadAnalysis()
	{
		string url = "http://174.36.196.91:19001/blackdawnAnalysis/UserBehaServlet";
		WWW www2 = new WWW(url, GameApp.GetInstance().GetUserState().OperInfo.WriteToBuffer());
		DateTime startTime = DateTime.Now;
		bool timeout = false;
		while (!www2.isDone)
		{
			Debug.Log("www.isDone: " + www2.isDone);
			if ((DateTime.Now - startTime).TotalSeconds > 15.0)
			{
				timeout = true;
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			Debug.Log("www.error: " + www2.error);
			if (!timeout && www2 != null && www2.error == null)
			{
				if (www2.bytes != null && www2.bytes.Length > 0)
				{
					byte[] indexBytes = www2.bytes;
					BytesBuffer bb = new BytesBuffer(indexBytes);
					int id = bb.ReadInt();
					GameApp.GetInstance().GetUserState().OperInfo.mIndex = id;
				}
				GameApp.GetInstance().GetUserState().OperInfo.AfterUpload();
				Debug.Log("upload analysis......: " + GameApp.GetInstance().GetUserState().OperInfo.mIndex);
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(ex.Message);
		}
		finally
		{
			if (www2 != null)
			{
				www2.Dispose();
				www2 = null;
			}
		}
	}

	private IEnumerator UploadAnalysisForQuest()
	{
		string url = "http://174.36.196.91:19001/blackdawnAnalysis/QuestLogServlet";
		WWW www2 = new WWW(url, GameApp.GetInstance().GetUserState().QuestInfo.WriteToBuffer());
		DateTime startTime = DateTime.Now;
		bool timeout = false;
		while (!www2.isDone)
		{
			Debug.Log("www.isDone: " + www2.isDone);
			if ((DateTime.Now - startTime).TotalSeconds > 15.0)
			{
				timeout = true;
				break;
			}
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			Debug.Log("www.error: " + www2.error);
			if (!timeout && www2 != null && www2.error == null)
			{
				GameApp.GetInstance().GetUserState().QuestInfo.AfterUpload();
				Debug.Log("upload analysis for quest...... ");
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.Log(ex.Message);
		}
		finally
		{
			if (www2 != null)
			{
				www2.Dispose();
				www2 = null;
			}
		}
	}

	protected void VarifyHackPlayer()
	{
		if (m_VarifyHackPlayerTimer.Ready())
		{
			m_VarifyHackPlayerTimer.Do();
			if (GameApp.GetInstance().GetGameWorld() != null)
			{
				VarifyHackPlayerRequest request = new VarifyHackPlayerRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}
}
