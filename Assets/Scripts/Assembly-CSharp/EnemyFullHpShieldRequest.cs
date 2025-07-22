public class EnemyFullHpShieldRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	public EnemyFullHpShieldRequest(byte pointID, byte enemyID)
	{
		requestID = 187;
		mPointID = pointID;
		mEnemyID = enemyID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		return bytesBuffer.GetBytes();
	}
}
