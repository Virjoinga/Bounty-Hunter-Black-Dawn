internal class ControllableItemOnHitResponse : Response
{
	protected EControllableType mType;

	protected byte mId;

	protected int mHp;

	protected int mShield;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mType = (EControllableType)bytesBuffer.ReadByte();
		mId = bytesBuffer.ReadByte();
		mHp = bytesBuffer.ReadInt();
		mShield = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		if (mType == EControllableType.SUMMONED)
		{
			SummonedItem summonedByID = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mId);
			if (summonedByID != null)
			{
				summonedByID.OnHitResponse(mHp, mShield);
			}
		}
	}
}
