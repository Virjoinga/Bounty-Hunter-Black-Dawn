internal class SendPlayerInputResponse : Response
{
	public int playerID;

	public bool fire;

	public bool isMoving;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		fire = bytesBuffer.ReadBool();
		isMoving = bytesBuffer.ReadBool();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.inputController.inputInfo.fire = fire;
			}
		}
	}
}
