public class UploadOperatingInfo : Request
{
	private int mithrilRebirthTime;

	public UploadOperatingInfo(int rebirthTime)
	{
		requestID = 23;
		mithrilRebirthTime = rebirthTime;
	}

	public override byte[] GetBody()
	{
		byte b = 4;
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		bytesBuffer.AddInt(mithrilRebirthTime);
		return bytesBuffer.GetBytes();
	}
}
