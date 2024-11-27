public class ChangeQuestMarkRequest : Request
{
	public short m_questCommonId;

	public ChangeQuestMarkRequest(short questCommonId)
	{
		requestID = 160;
		m_questCommonId = questCommonId;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(m_questCommonId);
		return bytesBuffer.GetBytes();
	}
}
