public class GetVSTDMRankRequest : Request
{
	private int m_statsId;

	public GetVSTDMRankRequest(int statsId)
	{
		requestID = 27;
		m_statsId = statsId;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddInt(m_statsId);
		return bytesBuffer.GetBytes();
	}
}
