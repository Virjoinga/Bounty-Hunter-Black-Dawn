public class UpdatePlayerAccQuestSubStateRequest : Request
{
	protected short m_questId;

	protected byte m_questSubState;

	public UpdatePlayerAccQuestSubStateRequest(short questId, byte questSubState)
	{
		requestID = 154;
		m_questId = questId;
		m_questSubState = questSubState;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(7);
		bytesBuffer.AddInt(Lobby.GetInstance().GetChannelID());
		bytesBuffer.AddShort(m_questId);
		bytesBuffer.AddByte(m_questSubState);
		return bytesBuffer.GetBytes();
	}
}
