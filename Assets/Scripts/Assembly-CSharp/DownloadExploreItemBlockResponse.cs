using System.Collections.Generic;

internal class DownloadExploreItemBlockResponse : Response
{
	private byte mBlockID;

	private List<byte> mItemIDinBlock = new List<byte>();

	private List<ExploreItemStatesInfo> mExplorableStatesInfo = new List<ExploreItemStatesInfo>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBlockID = bytesBuffer.ReadByte();
		byte b = bytesBuffer.ReadByte();
		for (byte b2 = 0; b2 < b; b2++)
		{
			byte item = bytesBuffer.ReadByte();
			short mQuestID = bytesBuffer.ReadShort();
			byte mState = bytesBuffer.ReadByte();
			mItemIDinBlock.Add(item);
			ExploreItemStatesInfo exploreItemStatesInfo = new ExploreItemStatesInfo();
			exploreItemStatesInfo.mQuestID = mQuestID;
			exploreItemStatesInfo.mState = (ExploreItemStates)mState;
			mExplorableStatesInfo.Add(exploreItemStatesInfo);
		}
	}

	public override void ProcessLogic()
	{
		GameApp.GetInstance().GetGameWorld().DownloadExploreItemBlock(mBlockID, mItemIDinBlock, mExplorableStatesInfo);
	}
}
