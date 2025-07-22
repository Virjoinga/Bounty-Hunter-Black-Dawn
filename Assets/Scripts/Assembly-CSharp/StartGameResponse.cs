using UnityEngine;

internal class StartGameResponse : Response
{
	public short roomID;

	public byte gameMode;

	public EBossState bossState;

	public short portalID;

	public byte masterSceneID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		gameMode = bytesBuffer.ReadByte();
		bossState = (EBossState)bytesBuffer.ReadByte();
		portalID = bytesBuffer.ReadShort();
		masterSceneID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Debug.Log("StartGame!!!");
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		GameApp.GetInstance().GetGameMode().ModePlay = (Mode)gameMode;
		GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.Playing;
		Time.timeScale = 1f;
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		inGameUIScript.FrGoToPhase(6, true, false, false);
		gameWorld.CurrentSceneID = masterSceneID;
		UserState userState = GameApp.GetInstance().GetUserState();
		Debug.Log("masterSceneID = " + masterSceneID);
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(masterSceneID);
		userState.SetCurrentCityID(sceneConfig.AreaID);
		Debug.Log("masterCityID = " + sceneConfig.AreaID + ", masterSceneID = " + masterSceneID);
		gameWorld.GetLocalPlayer().SetState(Player.IDLE_STATE);
		gameWorld.GetLocalPlayer().SetSeatID(Lobby.GetInstance().CurrentSeatID);
		Debug.Log("Lobby.GetInstance().CurrentSeatID: " + Lobby.GetInstance().CurrentSeatID);
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			gameWorld.GetLocalPlayer().Team = (TeamName)(gameWorld.GetLocalPlayer().GetSeatID() / 4);
		}
		Debug.Log("Lobby.GetInstance().CurrentSeatID: " + Lobby.GetInstance().CurrentSeatID);
		GameWorld.NeedGetSeneceState = true;
		TimeManager.GetInstance().SynStart();
		Object.DestroyImmediate(HUDManager.instance.gameObject);
		SceneConfig sceneConfig2 = GameApp.GetInstance().GetGameWorld().GetSceneConfig(gameWorld.CurrentSceneID);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			gameScene.LeaveScene();
		}
		GameApp.GetInstance().GetGameWorld().BossState = bossState;
		GameApp.GetInstance().GetGameWorld().PortalID = portalID;
		Application.LoadLevel(sceneConfig2.SceneFileName);
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		robot.GetGameApp().GetGameMode().ModePlay = (Mode)gameMode;
		robot.GetGameApp().GetGameMode().PlayerStatus = PlayerStateNetwork.Playing;
		robot.Notify(RobotStateEvent.StartGame);
		robot.GetGameScene().SetPortalID(portalID);
		robot.GetGameScene().SetScene((ESceneID)masterSceneID);
		GetSceneStateRequest request = new GetSceneStateRequest(robot.GetGameScene().mCityID, robot.GetGameScene().mSceneID, robot.Hp, robot.Shield, 0, robot.GetWeaponList(), 0, ElementType.NoElement);
		robot.GetNetworkManager().SendRequestAsRobot(request, robot);
		robot.GetTimeManager().SynStart();
		Debug.Log("robot start game ..................");
	}
}
