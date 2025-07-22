internal class SendPlayerShootAngleVResponse : Response
{
	public int playerID;

	public short angleV;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		angleV = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.TargetAngleV = angleV;
			}
		}
	}
}
