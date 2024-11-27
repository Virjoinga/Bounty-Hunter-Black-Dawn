public class PlayerLeaveExploreItemBlockRequest : Request
{
	private byte mBlockID;

	public PlayerLeaveExploreItemBlockRequest(byte blockID)
	{
		requestID = 167;
		mBlockID = blockID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mBlockID);
		return bytesBuffer.GetBytes();
	}
}
