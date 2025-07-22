public class EnemySpeedDownOverRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	public EnemySpeedDownOverRequest(byte pointID, byte enemyID)
	{
		requestID = 183;
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
