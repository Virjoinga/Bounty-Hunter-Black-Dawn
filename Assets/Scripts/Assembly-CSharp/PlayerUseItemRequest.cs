public class PlayerUseItemRequest : Request
{
	protected byte bagIndex;

	protected byte itemID;

	protected byte buffValue;

	public PlayerUseItemRequest(byte bagIndex, byte itemID, byte buffValue)
	{
		requestID = 117;
		this.bagIndex = bagIndex;
		this.itemID = itemID;
		this.buffValue = buffValue;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(bagIndex);
		bytesBuffer.AddByte(itemID);
		bytesBuffer.AddByte(buffValue);
		return bytesBuffer.GetBytes();
	}
}
