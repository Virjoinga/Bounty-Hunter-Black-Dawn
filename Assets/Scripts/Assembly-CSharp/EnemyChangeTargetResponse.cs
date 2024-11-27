internal class EnemyChangeTargetResponse : Response
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected EGameUnitType mGameUnitType;

	protected int mPlayerID;

	protected EControllableType mControllableType;

	protected byte mControllableID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mEnemyID = bytesBuffer.ReadByte();
		mGameUnitType = (EGameUnitType)bytesBuffer.ReadByte();
		if (mGameUnitType == EGameUnitType.PLAYER)
		{
			mPlayerID = bytesBuffer.ReadInt();
		}
		else if (mGameUnitType == EGameUnitType.CONTROLLABLE_ITEM)
		{
			mControllableType = (EControllableType)bytesBuffer.ReadByte();
			mControllableID = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(mPointID, mEnemyID);
		if (enemy != null && enemy.InPlayingState())
		{
			GameUnit target = null;
			if (mGameUnitType == EGameUnitType.PLAYER)
			{
				target = ((mPlayerID != GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID()) ? ((Player)GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mPlayerID)) : ((Player)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()));
			}
			else if (mGameUnitType == EGameUnitType.CONTROLLABLE_ITEM && mControllableType == EControllableType.SUMMONED)
			{
				target = GameApp.GetInstance().GetGameScene().GetSummonedByID("S_" + mControllableID);
			}
			enemy.ChangeTarget(target);
		}
	}
}
