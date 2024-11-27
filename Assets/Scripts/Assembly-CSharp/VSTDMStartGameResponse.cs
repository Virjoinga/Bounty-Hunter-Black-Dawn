using UnityEngine;

internal class VSTDMStartGameResponse : Response
{
	public short mRoomID;

	public byte mCityID;

	public byte mSceneID;

	public byte mGameMode;

	public byte mSubGameMode;

	public byte mSeatID;

	public bool mIsMaster;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mRoomID = bytesBuffer.ReadShort();
		mCityID = bytesBuffer.ReadByte();
		mSceneID = bytesBuffer.ReadByte();
		mGameMode = bytesBuffer.ReadByte();
		mSubGameMode = bytesBuffer.ReadByte();
		mSeatID = bytesBuffer.ReadByte();
		mIsMaster = bytesBuffer.ReadBool();
	}

	public override void ProcessLogic()
	{
		Debug.Log("StartGame!!!");
		if (UIVS.instance != null)
		{
			UIVS.instance.NotifyMatchSuccess();
		}
		if (UIVSTeam.m_instance != null)
		{
			UIVSTeam.m_instance.m_createRoom.NotifyCreateRoomSuccess();
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		gameWorld.RemoveAllRemotePlayers();
		GameApp.GetInstance().GetGameMode().ModePlay = (Mode)mGameMode;
		GameApp.GetInstance().CreateVSManager();
		GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.Playing;
		Time.timeScale = 1f;
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript != null)
		{
			inGameUIScript.FrGoToPhase(6, true, false, false);
		}
		gameWorld.CurrentSceneID = mSceneID;
		UserState userState = GameApp.GetInstance().GetUserState();
		Debug.Log("masterSceneID = " + mSceneID);
		userState.SetCurrentCityID(mCityID);
		Lobby.GetInstance().SetCurrentRoomID(mRoomID);
		Lobby.GetInstance().IsMasterPlayer = mIsMaster;
		gameWorld.GetLocalPlayer().SetState(Player.IDLE_STATE);
		gameWorld.GetLocalPlayer().SetSeatID(mSeatID);
		Debug.Log("mSeatID: " + mSeatID);
		gameWorld.GetLocalPlayer().Team = (TeamName)(gameWorld.GetLocalPlayer().GetSeatID() / 4);
		GameWorld.NeedGetSeneceState = true;
		TimeManager.GetInstance().SynStart();
		if (HUDManager.instance != null)
		{
			Object.DestroyImmediate(HUDManager.instance.gameObject);
		}
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(gameWorld.CurrentSceneID);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			gameScene.LeaveScene();
		}
		Application.LoadLevel(sceneConfig.SceneFileName);
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		robot.GetGameApp().GetGameMode().ModePlay = (Mode)mGameMode;
		robot.GetGameApp().GetGameMode().PlayerStatus = PlayerStateNetwork.Playing;
		if (mRoomID == -1)
		{
			robot.GetRobotRoom().CreateRoomFailed();
		}
		else
		{
			robot.GetLobby().SetCurrentRoomID(mRoomID);
			robot.Notify(RobotStateEvent.StartVSGame);
		}
		Debug.Log("robot start VS TDM game ..................");
	}
}
