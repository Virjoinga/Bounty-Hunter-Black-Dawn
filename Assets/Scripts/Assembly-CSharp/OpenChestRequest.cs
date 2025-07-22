public class OpenChestRequest : Request
{
	protected short chestID;

	public OpenChestRequest(short chestID)
	{
		requestID = 185;
		this.chestID = chestID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(chestID);
		return bytesBuffer.GetBytes();
	}
}
