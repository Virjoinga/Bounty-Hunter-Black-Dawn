public class PlayerRecoverFromDyingRequest : Request
{
	protected byte type;

	protected short point;

	public PlayerRecoverFromDyingRequest(short point)
	{
		requestID = 157;
		this.point = point;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(point);
		return bytesBuffer.GetBytes();
	}
}
