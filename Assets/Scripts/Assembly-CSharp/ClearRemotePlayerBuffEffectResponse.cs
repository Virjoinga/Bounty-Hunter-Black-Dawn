internal class ClearRemotePlayerBuffEffectResponse : Response
{
	protected int playerID;

	protected byte effectType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		effectType = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			if (effectType == 8)
			{
				localPlayer.ClearSpeedDownEffect();
			}
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null && effectType == 8)
		{
			remotePlayerByUserID.ClearSpeedDownEffect();
		}
	}
}
