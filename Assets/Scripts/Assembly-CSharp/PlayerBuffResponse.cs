internal class PlayerBuffResponse : Response
{
	protected byte m_buff;

	protected int m_PlayerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_buff = bytesBuffer.ReadByte();
		m_PlayerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
	}
}
