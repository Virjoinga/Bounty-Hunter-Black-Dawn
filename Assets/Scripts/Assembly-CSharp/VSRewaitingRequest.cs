public class VSRewaitingRequest : Request
{
	private UIVS.Mode mMode;

	public VSRewaitingRequest(UIVS.Mode mode)
	{
		requestID = 198;
		mMode = mode;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte((byte)mMode);
		return bytesBuffer.GetBytes();
	}
}
