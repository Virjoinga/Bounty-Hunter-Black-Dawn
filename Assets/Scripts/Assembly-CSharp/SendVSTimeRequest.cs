public class SendVSTimeRequest : Request
{
	protected int channelID;

	protected float time;

	public SendVSTimeRequest(int channelID, float time)
	{
		requestID = 133;
		this.channelID = channelID;
		this.time = time;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(8);
		bytesBuffer.AddInt(channelID);
		bytesBuffer.AddFloat(time);
		return bytesBuffer.GetBytes();
	}
}
