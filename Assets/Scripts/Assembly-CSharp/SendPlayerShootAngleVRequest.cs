public class SendPlayerShootAngleVRequest : Request
{
	protected short m_angleV;

	public SendPlayerShootAngleVRequest(short angleV)
	{
		requestID = 124;
		m_angleV = angleV;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(m_angleV);
		return bytesBuffer.GetBytes();
	}
}
