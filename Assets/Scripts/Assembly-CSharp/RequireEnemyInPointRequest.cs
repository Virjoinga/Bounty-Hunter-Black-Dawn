public class RequireEnemyInPointRequest : Request
{
	private byte mPointID;

	private bool mOneEnemy;

	private byte mEnemyID;

	public RequireEnemyInPointRequest(byte pointID)
	{
		requestID = 139;
		mPointID = pointID;
		mOneEnemy = false;
	}

	public RequireEnemyInPointRequest(byte pointID, byte enemyID)
	{
		requestID = 139;
		mPointID = pointID;
		mOneEnemy = true;
		mEnemyID = enemyID;
	}

	public override byte[] GetBody()
	{
		int num = 2;
		if (mOneEnemy)
		{
			num++;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddBool(mOneEnemy);
		if (mOneEnemy)
		{
			bytesBuffer.AddByte(mEnemyID);
		}
		return bytesBuffer.GetBytes();
	}
}
