public class CreateRoomRequest : Request
{
	public string m_roomName;

	public short m_passWord;

	public byte m_maxPlayer;

	public bool m_hasPass;

	public byte m_mode;

	public byte m_mapID;

	public short m_level;

	public short m_ping;

	public short m_minJoinLevel;

	public short m_maxJoinLevel;

	public short m_questMark;

	public string m_comment;

	public byte m_cityID;

	public byte m_sceneID;

	public CreateRoomRequest(string name, short pass, byte playerNum, bool hasPassword, byte mode, byte mapID, short level, short ping, short minJoinLevel, short maxJoinLevel, short questMark, string comment, byte cityId, byte sceneId)
	{
		requestID = 4;
		m_roomName = name;
		m_passWord = -1;
		m_passWord = pass;
		m_maxPlayer = playerNum;
		m_hasPass = hasPassword;
		m_mode = mode;
		m_mapID = mapID;
		m_level = level;
		m_ping = ping;
		m_minJoinLevel = minJoinLevel;
		m_maxJoinLevel = maxJoinLevel;
		m_questMark = questMark;
		m_comment = comment;
		m_cityID = cityId;
		m_sceneID = sceneId;
	}

	public override byte[] GetBody()
	{
		short stringShortLength = BytesBuffer.GetStringShortLength(m_roomName);
		short stringShortLength2 = BytesBuffer.GetStringShortLength(m_comment);
		int num = 12 + stringShortLength + 2 + stringShortLength2 + 2;
		if (m_hasPass)
		{
			num += 2;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddBool(m_hasPass);
		bytesBuffer.AddByte(m_maxPlayer);
		bytesBuffer.AddByte(m_mode);
		bytesBuffer.AddByte(m_mapID);
		bytesBuffer.AddShort(m_level);
		bytesBuffer.AddShort(m_ping);
		bytesBuffer.AddShort(m_minJoinLevel);
		bytesBuffer.AddShort(m_maxJoinLevel);
		bytesBuffer.AddStringShortLength(m_roomName);
		bytesBuffer.AddShort(m_questMark);
		bytesBuffer.AddStringShortLength(m_comment);
		if (m_hasPass)
		{
			bytesBuffer.AddShort(m_passWord);
		}
		bytesBuffer.AddByte(m_cityID);
		bytesBuffer.AddByte(m_sceneID);
		return bytesBuffer.GetBytes();
	}
}
