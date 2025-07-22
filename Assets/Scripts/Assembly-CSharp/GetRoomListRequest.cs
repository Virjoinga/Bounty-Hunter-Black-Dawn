public class GetRoomListRequest : Request
{
	public byte m_roomType;

	public short m_level;

	public byte m_unlockCity;

	public short m_currentQuest;

	public GetRoomListRequest(byte roomType, short level, byte unlockCity, short currentQuest)
	{
		requestID = 7;
		m_roomType = roomType;
		m_level = level;
		m_unlockCity = unlockCity;
		m_currentQuest = currentQuest;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte(m_roomType);
		bytesBuffer.AddShort(m_level);
		bytesBuffer.AddByte(m_unlockCity);
		bytesBuffer.AddShort(m_currentQuest);
		return bytesBuffer.GetBytes();
	}
}
