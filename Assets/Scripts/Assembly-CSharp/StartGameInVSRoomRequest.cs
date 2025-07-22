public class StartGameInVSRoomRequest : Request
{
	public short m_roomId;

	public StartGameInVSRoomRequest(short id)
	{
		requestID = 32;
		m_roomId = id;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(m_roomId);
		return bytesBuffer.GetBytes();
	}
}
