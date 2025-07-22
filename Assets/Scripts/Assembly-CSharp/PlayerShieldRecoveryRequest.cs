public class PlayerShieldRecoveryRequest : Request
{
	protected byte type;

	protected short point;

	protected short maxShield;

	public PlayerShieldRecoveryRequest(byte type, short point, short maxShield)
	{
		requestID = 161;
		this.type = type;
		this.point = point;
		this.maxShield = maxShield;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(point);
		bytesBuffer.AddShort(maxShield);
		return bytesBuffer.GetBytes();
	}
}
