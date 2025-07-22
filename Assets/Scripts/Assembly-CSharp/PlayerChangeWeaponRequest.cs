public class PlayerChangeWeaponRequest : Request
{
	protected byte bagIndex;

	protected ElementType elementType;

	public PlayerChangeWeaponRequest(byte bagIndex, ElementType element)
	{
		requestID = 111;
		this.bagIndex = bagIndex;
		elementType = element;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(bagIndex);
		bytesBuffer.AddByte((byte)elementType);
		return bytesBuffer.GetBytes();
	}
}
