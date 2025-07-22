internal class PlayerLeaveGameResponse : Response
{
	protected int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			GameApp.GetInstance().GetGameWorld().RemoveRemotePlayer(playerID);
			GameApp.GetInstance().GetGameWorld().RecalculateEnemyAbility();
		}
	}
}
