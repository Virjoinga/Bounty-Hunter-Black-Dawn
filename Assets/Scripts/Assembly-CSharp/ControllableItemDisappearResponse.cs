internal class ControllableItemDisappearResponse : Response
{
	protected EControllableType mType;

	protected byte mId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mType = (EControllableType)bytesBuffer.ReadByte();
		mId = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (mType == EControllableType.SUMMONED)
		{
			SummonedItem summonedByID = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mId);
			if (summonedByID != null)
			{
				summonedByID.EndCurrentState();
				summonedByID.StartDisappear();
			}
		}
	}
}
