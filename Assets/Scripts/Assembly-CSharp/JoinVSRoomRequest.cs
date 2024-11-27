public class JoinVSRoomRequest : Request
{
	public short m_roomId;

	public short m_ping;

	public JoinVSRoomRequest(short id, short ping)
	{
		requestID = 31;
		m_roomId = id;
		m_ping = ping;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddShort(m_roomId);
		bytesBuffer.AddShort(m_ping);
		return bytesBuffer.GetBytes();
	}
}
