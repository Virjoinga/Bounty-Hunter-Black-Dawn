public class TestRequest : Request
{
	public TestRequest()
	{
		requestID = 50;
	}

	public override byte[] GetBody()
	{
		byte b = 20;
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		for (int i = 0; i < 5; i++)
		{
			bytesBuffer.AddInt(1);
		}
		return bytesBuffer.GetBytes();
	}
}
