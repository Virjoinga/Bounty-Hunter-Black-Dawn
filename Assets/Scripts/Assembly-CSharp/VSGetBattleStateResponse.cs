internal class VSGetBattleStateResponse : Response
{
	protected int mPlayerID;

	protected IBattleState mBattleState;

	protected byte mVSMode;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPlayerID = bytesBuffer.ReadInt();
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
		{
			mVSMode = 1;
		}
		mBattleState = BattleStateFactory.GetInstance().Create(GMBattleState.GM_TDM_STATE);
		if (mBattleState != null)
		{
			mBattleState.ReadFromBuffer(bytesBuffer);
		}
	}

	public override void ProcessLogic()
	{
		if (mBattleState == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (mPlayerID == channelID)
		{
			localPlayer.GetUserState().GetBattleStates()[mVSMode].SetState(mBattleState);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mPlayerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.GetUserState().GetBattleStates()[mVSMode].SetState(mBattleState);
		}
	}
}
