public class ControllableItemChangeTargetRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected byte mPointID;

	protected byte mEnemyID;

	public ControllableItemChangeTargetRequest(EControllableType type, byte id, byte pointId, byte enemyId)
	{
		requestID = 174;
		mType = type;
		mID = id;
		mPointID = pointId;
		mEnemyID = enemyId;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		return bytesBuffer.GetBytes();
	}
}
