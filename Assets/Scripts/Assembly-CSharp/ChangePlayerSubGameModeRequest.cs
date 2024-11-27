public class ChangePlayerSubGameModeRequest : Request
{
	public byte m_subGameMode;

	public ChangePlayerSubGameModeRequest(byte subGameMode)
	{
		requestID = 188;
		m_subGameMode = subGameMode;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(m_subGameMode);
		return bytesBuffer.GetBytes();
	}
}
