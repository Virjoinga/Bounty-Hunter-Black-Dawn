public class ChatRequest : Request
{
	private int m_hostID;

	private string m_name;

	private string m_text;

	public ChatRequest(int hostID, string name, string text)
	{
		requestID = 159;
		m_hostID = hostID;
		m_name = name;
		m_text = text;
	}

	public override byte[] GetBody()
	{
		byte stringByteLength = BytesBuffer.GetStringByteLength(m_name);
		byte stringByteLength2 = BytesBuffer.GetStringByteLength(m_text);
		int num = 4 + stringByteLength + stringByteLength2;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddInt(m_hostID);
		bytesBuffer.AddString(m_name);
		bytesBuffer.AddString(m_text);
		return bytesBuffer.GetBytes();
	}
}
