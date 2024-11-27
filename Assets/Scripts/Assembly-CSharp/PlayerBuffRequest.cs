public class PlayerBuffRequest : Request
{
	protected byte buffID;

	public PlayerBuffRequest(byte buffID)
	{
		requestID = 123;
		this.buffID = buffID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(buffID);
		return bytesBuffer.GetBytes();
	}
}
