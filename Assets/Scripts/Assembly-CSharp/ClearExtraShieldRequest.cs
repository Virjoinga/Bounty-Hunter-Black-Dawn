public class ClearExtraShieldRequest : Request
{
	public int extraShield;

	public ClearExtraShieldRequest()
	{
		requestID = 181;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
