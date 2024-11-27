public class TimeSynchronizeRequest : Request
{
	private byte mId;

	private long mTime;

	public TimeSynchronizeRequest(byte id, long mTime)
	{
		requestID = 100;
		mId = id;
		this.mTime = mTime;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(9);
		bytesBuffer.AddByte(mId);
		bytesBuffer.AddLong(mTime);
		return bytesBuffer.GetBytes();
	}
}
