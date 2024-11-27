public class RequireExploreItemBlockRequest : Request
{
	private byte mBlockID;

	public RequireExploreItemBlockRequest(byte ID)
	{
		requestID = 164;
		mBlockID = ID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mBlockID);
		return bytesBuffer.GetBytes();
	}
}
