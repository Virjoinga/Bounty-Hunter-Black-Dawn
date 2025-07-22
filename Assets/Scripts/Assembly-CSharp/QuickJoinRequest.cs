public class QuickJoinRequest : Request
{
	private byte mRankId;

	private short ping;

	public QuickJoinRequest(byte rankId, short ping)
	{
		requestID = 11;
		mRankId = rankId;
		this.ping = ping;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(mRankId);
		bytesBuffer.AddShort(ping);
		return bytesBuffer.GetBytes();
	}
}
