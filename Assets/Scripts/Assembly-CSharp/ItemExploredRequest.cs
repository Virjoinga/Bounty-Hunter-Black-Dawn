public class ItemExploredRequest : Request
{
	private byte mBlockID;

	private byte mStateID;

	private short mQuestID;

	private byte mState;

	public ItemExploredRequest(byte blockID, byte stateID, ExploreItemStates state, short questID)
	{
		requestID = 168;
		mBlockID = blockID;
		mStateID = stateID;
		mState = (byte)state;
		mQuestID = questID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(mBlockID);
		bytesBuffer.AddByte(mStateID);
		bytesBuffer.AddShort(mQuestID);
		bytesBuffer.AddByte(mState);
		return bytesBuffer.GetBytes();
	}
}
