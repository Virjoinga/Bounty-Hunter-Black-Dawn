public class DownloadQuestsRequest : Request
{
	public DownloadQuestsRequest()
	{
		requestID = 151;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
