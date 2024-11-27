public class DownloadEnemyInPointRequest : Request
{
	private byte mPointID;

	private bool mExistInGameWorld;

	public DownloadEnemyInPointRequest(byte pointID, bool existInGameWorld)
	{
		requestID = 138;
		mPointID = pointID;
		mExistInGameWorld = existInGameWorld;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddBool(mExistInGameWorld);
		return bytesBuffer.GetBytes();
	}
}
