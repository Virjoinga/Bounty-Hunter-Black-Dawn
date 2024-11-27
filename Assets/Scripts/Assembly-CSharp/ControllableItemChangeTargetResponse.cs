internal class ControllableItemChangeTargetResponse : Response
{
	protected EControllableType mType;

	protected byte mId;

	protected byte mPointId;

	protected byte mEnemyId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mType = (EControllableType)bytesBuffer.ReadByte();
		mId = bytesBuffer.ReadByte();
		mPointId = bytesBuffer.ReadByte();
		mEnemyId = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (mType != 0)
		{
			return;
		}
		SummonedItem summonedByID = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mId);
		if (summonedByID != null)
		{
			EngineerGun engineerGun = summonedByID as EngineerGun;
			if (engineerGun != null)
			{
				engineerGun.ChangeTargetUnit(mPointId, mEnemyId);
			}
		}
	}
}
