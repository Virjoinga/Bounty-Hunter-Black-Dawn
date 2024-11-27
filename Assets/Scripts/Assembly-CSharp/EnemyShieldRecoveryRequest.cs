public class EnemyShieldRecoveryRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected int mDeltaShield;

	public EnemyShieldRecoveryRequest(byte pointID, byte enemyID, int deltaShield)
	{
		requestID = 135;
		mPointID = pointID;
		mEnemyID = enemyID;
		mDeltaShield = deltaShield;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		bytesBuffer.AddInt(mDeltaShield);
		return bytesBuffer.GetBytes();
	}
}
