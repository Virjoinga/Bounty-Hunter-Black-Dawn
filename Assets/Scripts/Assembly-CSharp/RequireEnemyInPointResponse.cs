internal class RequireEnemyInPointResponse : Response
{
	private byte mPointID;

	private bool mOneEnemy;

	private byte mEnemyID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mOneEnemy = bytesBuffer.ReadBool();
		if (mOneEnemy)
		{
			mEnemyID = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		UploadEnemyInPointRequest request = new UploadEnemyInPointRequest(mPointID, mOneEnemy, mEnemyID);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		robot.GetGameScene().UploadEnemy(mPointID);
	}
}
