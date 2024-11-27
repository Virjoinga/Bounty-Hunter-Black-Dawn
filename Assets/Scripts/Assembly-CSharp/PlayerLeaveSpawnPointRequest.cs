public class PlayerLeaveSpawnPointRequest : Request
{
	private byte mPointID;

	public PlayerLeaveSpawnPointRequest(byte pointID)
	{
		requestID = 141;
		mPointID = pointID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mPointID);
		return bytesBuffer.GetBytes();
	}
}
