internal class PlayerLeaveExploreItemBlockResponse : Response
{
	private byte mBlockID;

	private int mPlayerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBlockID = bytesBuffer.ReadByte();
		mPlayerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		ExploreItemBlockInfo explorItemBlock = GameApp.GetInstance().GetGameWorld().GetExplorItemBlock(mBlockID);
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mPlayerID);
		if (remotePlayerByUserID == null)
		{
		}
	}
}
