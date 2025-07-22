public class RemoveSpawnPointRequest : Request
{
	private byte mPointID;

	private bool mIsForceRemove;

	public RemoveSpawnPointRequest(byte pointID, bool isForceRemove)
	{
		requestID = 177;
		mPointID = pointID;
		mIsForceRemove = isForceRemove;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddBool(mIsForceRemove);
		return bytesBuffer.GetBytes();
	}
}
