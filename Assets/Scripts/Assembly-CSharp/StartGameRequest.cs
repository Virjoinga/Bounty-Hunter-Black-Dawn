public class StartGameRequest : Request
{
	public StartGameRequest()
	{
		requestID = 9;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
