public class ClearExtraShieldResponse : Response
{
	protected int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			localPlayer.ClearExtraShieldWithEffect();
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.ClearExtraShieldWithEffect();
		}
	}
}
