public class CreateVSRoomRequest : Request
{
	public string m_roomName;

	public short m_passWord;

	public bool m_hasPass;

	public short m_ping;

	public short m_minJoinLevel;

	public short m_maxJoinLevel;

	public CreateVSRoomRequest(string name, short pass, bool hasPassword, short ping, short minJoinLevel, short maxJoinLevel)
	{
		requestID = 18;
		m_roomName = name;
		m_passWord = pass;
		m_hasPass = hasPassword;
		m_ping = ping;
		m_minJoinLevel = minJoinLevel;
		m_maxJoinLevel = maxJoinLevel;
	}

	public override byte[] GetBody()
	{
		short stringShortLength = BytesBuffer.GetStringShortLength(m_roomName);
		int num = 7 + stringShortLength;
		if (m_hasPass)
		{
			num += 2;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddBool(m_hasPass);
		bytesBuffer.AddShort(m_ping);
		bytesBuffer.AddShort(m_minJoinLevel);
		bytesBuffer.AddShort(m_maxJoinLevel);
		bytesBuffer.AddStringShortLength(m_roomName);
		if (m_hasPass)
		{
			bytesBuffer.AddShort(m_passWord);
		}
		return bytesBuffer.GetBytes();
	}
}
