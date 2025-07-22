internal class ControllableItemChangePVPTargetResponse : Response
{
	protected bool mIsTargetPlayer;

	protected int mTargetID;

	protected EControllableType mType;

	protected byte mSelfID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mIsTargetPlayer = bytesBuffer.ReadBool();
		mTargetID = bytesBuffer.ReadInt();
		mType = (EControllableType)bytesBuffer.ReadByte();
		mSelfID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (mType != 0)
		{
			return;
		}
		SummonedItem summonedByID = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mSelfID);
		if (summonedByID != null)
		{
			EngineerGun engineerGun = summonedByID as EngineerGun;
			if (engineerGun != null)
			{
				GameUnit gameUnit = null;
				gameUnit = ((!mIsTargetPlayer) ? ((GameUnit)GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + (byte)mTargetID)) : ((GameUnit)((Lobby.GetInstance().GetChannelID() != mTargetID) ? ((Player)GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mTargetID)) : ((Player)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()))));
				engineerGun.ChangeTargetUnit(gameUnit);
			}
		}
	}
}
