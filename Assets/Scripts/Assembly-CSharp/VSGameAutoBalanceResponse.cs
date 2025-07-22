internal class VSGameAutoBalanceResponse : Response
{
	protected int channelID;

	protected byte newSeatID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
		newSeatID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int num = Lobby.GetInstance().GetChannelID();
		if (localPlayer == null)
		{
			return;
		}
		if (num == channelID)
		{
			if (newSeatID != localPlayer.GetSeatID())
			{
				localPlayer.SetSeatID(newSeatID);
				localPlayer.Team = (TeamName)(localPlayer.GetSeatID() / 4);
			}
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(channelID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.SetSeatID(newSeatID);
			remotePlayerByUserID.Team = (TeamName)(remotePlayerByUserID.GetSeatID() / 4);
			remotePlayerByUserID.CreatePlayerSign();
		}
	}
}
