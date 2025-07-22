public class EnemyChangeTargetRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected EGameUnitType mGameUnitType;

	protected int mPlayerID;

	protected EControllableType mControllableType;

	protected byte mControllableID;

	public EnemyChangeTargetRequest(byte pointID, byte enemyID, GameUnit target)
	{
		requestID = 105;
		mPointID = pointID;
		mEnemyID = enemyID;
		mGameUnitType = target.GameUnitType;
		if (mGameUnitType == EGameUnitType.PLAYER)
		{
			Player player = target as Player;
			if (player != null)
			{
				mPlayerID = player.GetUserID();
			}
		}
		else if (mGameUnitType == EGameUnitType.CONTROLLABLE_ITEM)
		{
			ControllableUnit controllableUnit = target as ControllableUnit;
			if (controllableUnit != null)
			{
				mControllableType = controllableUnit.ControllableType;
				mControllableID = controllableUnit.ID;
			}
		}
	}

	public override byte[] GetBody()
	{
		int num = 3;
		if (mGameUnitType == EGameUnitType.PLAYER)
		{
			num += 4;
		}
		else if (mGameUnitType == EGameUnitType.CONTROLLABLE_ITEM)
		{
			num += 2;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		bytesBuffer.AddByte((byte)mGameUnitType);
		if (mGameUnitType == EGameUnitType.PLAYER)
		{
			bytesBuffer.AddInt(mPlayerID);
		}
		else if (mGameUnitType == EGameUnitType.CONTROLLABLE_ITEM)
		{
			bytesBuffer.AddByte((byte)mControllableType);
			bytesBuffer.AddByte(mControllableID);
		}
		return bytesBuffer.GetBytes();
	}
}
