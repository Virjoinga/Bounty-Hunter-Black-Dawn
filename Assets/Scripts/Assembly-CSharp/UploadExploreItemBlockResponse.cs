internal class UploadExploreItemBlockResponse : Response
{
	private byte mBlockID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBlockID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameApp.GetInstance().GetGameWorld().RefreshEploreItemBlock(mBlockID);
	}
}
