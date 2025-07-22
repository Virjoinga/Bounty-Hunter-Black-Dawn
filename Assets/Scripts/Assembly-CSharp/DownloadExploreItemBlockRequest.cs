public class DownloadExploreItemBlockRequest : Request
{
	private byte mBlockID;

	public DownloadExploreItemBlockRequest(byte ID)
	{
		requestID = 166;
		mBlockID = ID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mBlockID);
		return bytesBuffer.GetBytes();
	}
}
