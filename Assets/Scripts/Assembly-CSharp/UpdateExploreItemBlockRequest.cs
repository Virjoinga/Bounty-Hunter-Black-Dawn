public class UpdateExploreItemBlockRequest : Request
{
	private short mQuestID;

	public UpdateExploreItemBlockRequest(short questID)
	{
		requestID = 191;
		mQuestID = questID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(mQuestID);
		return bytesBuffer.GetBytes();
	}
}
