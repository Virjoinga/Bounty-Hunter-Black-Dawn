public class PickUpQuestItemRequest : Request
{
	protected short specialID;

	public PickUpQuestItemRequest(short specialID)
	{
		requestID = 169;
		this.specialID = specialID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(specialID);
		return bytesBuffer.GetBytes();
	}
}
