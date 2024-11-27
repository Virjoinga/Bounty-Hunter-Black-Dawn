public class PickUpItemRequest : Request
{
	protected short sequenceID;

	public PickUpItemRequest(short sequenceID)
	{
		requestID = 120;
		this.sequenceID = sequenceID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(sequenceID);
		return bytesBuffer.GetBytes();
	}
}
