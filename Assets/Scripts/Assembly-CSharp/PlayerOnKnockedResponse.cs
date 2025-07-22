internal class PlayerOnKnockedResponse : Response
{
	private int playerID;

	private short speed;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		speed = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID != channelID)
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.StartKnocked((float)speed / 100f);
			}
		}
	}
}
