public class PlayerOnKnockedRequest : Request
{
	protected short speed;

	public PlayerOnKnockedRequest(float speed)
	{
		requestID = 121;
		this.speed = (short)(speed * 100f);
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(speed);
		return bytesBuffer.GetBytes();
	}
}
