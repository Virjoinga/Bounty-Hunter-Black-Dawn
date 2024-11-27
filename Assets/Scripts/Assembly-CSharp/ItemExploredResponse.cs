public class ItemExploredResponse : Response
{
	private byte mBlockID;

	private byte mStateID;

	private short mQuestID;

	private byte mState;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBlockID = bytesBuffer.ReadByte();
		mStateID = bytesBuffer.ReadByte();
		mQuestID = bytesBuffer.ReadShort();
		mState = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		ExploreItemStatesInfo exploreItemStatesInfo = new ExploreItemStatesInfo();
		exploreItemStatesInfo.mQuestID = mQuestID;
		exploreItemStatesInfo.mState = (ExploreItemStates)mState;
		GameApp.GetInstance().GetGameWorld().UpdateExploreItem(mBlockID, mStateID, exploreItemStatesInfo);
	}
}
