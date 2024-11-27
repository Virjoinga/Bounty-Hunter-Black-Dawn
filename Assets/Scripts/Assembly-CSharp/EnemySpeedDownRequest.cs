public class EnemySpeedDownRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected short mSpeedDownSkillID;

	protected short mDuration;

	protected byte mSpeedDownRate;

	public EnemySpeedDownRequest(byte pointID, byte enemyID, short skillID, short duration, byte speedDownRate)
	{
		requestID = 182;
		mPointID = pointID;
		mEnemyID = enemyID;
		mSpeedDownSkillID = skillID;
		mDuration = duration;
		mSpeedDownRate = speedDownRate;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(7);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		bytesBuffer.AddShort(mSpeedDownSkillID);
		bytesBuffer.AddShort(mDuration);
		bytesBuffer.AddByte(mSpeedDownRate);
		return bytesBuffer.GetBytes();
	}
}
