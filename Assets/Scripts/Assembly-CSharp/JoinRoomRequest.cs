public class JoinRoomRequest : Request
{
	public short m_roomId;

	public short m_level;

	public short m_ping;

	public JoinRoomRequest(short id, short level, short ping)
	{
		requestID = 5;
		m_roomId = id;
		m_level = level;
		m_ping = ping;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddShort(m_roomId);
		bytesBuffer.AddShort(m_level);
		bytesBuffer.AddShort(m_ping);
		return bytesBuffer.GetBytes();
	}
}
