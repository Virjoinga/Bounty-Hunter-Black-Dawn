public class VSChangeSubModeRequest : Request
{
	private UIVS.Mode mMode;

	public VSChangeSubModeRequest(UIVS.Mode mode)
	{
		requestID = 29;
		mMode = mode;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte((byte)mMode);
		return bytesBuffer.GetBytes();
	}
}
