internal class PlayerUseItemResponse : Response
{
	protected int playerID;

	protected byte itemID;

	protected byte buffValue;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		itemID = bytesBuffer.ReadByte();
		buffValue = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		float hpRate = (float)(int)buffValue * 0.01f;
		if (playerID == channelID)
		{
			localPlayer.UseItemByItemID(itemID, hpRate);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.UseItemByItemID(itemID, hpRate);
		}
	}
}
