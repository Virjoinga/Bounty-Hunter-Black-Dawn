public class CreateExtraShieldRequest : Request
{
	public int extraShield;

	public CreateExtraShieldRequest(int extraShield)
	{
		requestID = 180;
		this.extraShield = extraShield;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddInt(extraShield);
		return bytesBuffer.GetBytes();
	}
}
