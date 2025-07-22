public class ControllableItemDisappearRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	public ControllableItemDisappearRequest(EControllableType type, byte id)
	{
		requestID = 175;
		mType = type;
		mID = id;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		return bytesBuffer.GetBytes();
	}
}
