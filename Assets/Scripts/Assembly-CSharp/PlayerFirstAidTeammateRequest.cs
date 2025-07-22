public class PlayerFirstAidTeammateRequest : Request
{
	private byte m_state;

	private int m_teammateId;

	public PlayerFirstAidTeammateRequest(int teammateId, FirstAidPhase state)
	{
		requestID = 156;
		m_teammateId = teammateId;
		m_state = (byte)state;
	}

	public override byte[] GetBody()
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		BytesBuffer bytesBuffer = new BytesBuffer(9);
		bytesBuffer.AddInt(channelID);
		bytesBuffer.AddInt(m_teammateId);
		bytesBuffer.AddByte(m_state);
		return bytesBuffer.GetBytes();
	}
}
