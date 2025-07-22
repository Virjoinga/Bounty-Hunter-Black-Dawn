public class PlayerHpRecoveryRequest : Request
{
	protected byte type;

	protected short point;

	protected short maxHP;

	public PlayerHpRecoveryRequest(byte type, short point, short maxHP)
	{
		requestID = 118;
		this.type = type;
		this.point = point;
		this.maxHP = maxHP;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(point);
		bytesBuffer.AddShort(maxHP);
		return bytesBuffer.GetBytes();
	}
}
