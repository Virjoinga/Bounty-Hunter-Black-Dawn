public class InvitaionFailMessageRequest : Request
{
	public enum Type
	{
		InMenu = 0,
		Dying = 1,
		NotInSameMode = 2,
		Reject = 3
	}

	private int m_ID;

	private Type m_Type;

	public InvitaionFailMessageRequest(int _id, Type _type)
	{
		requestID = 190;
		m_ID = _id;
		m_Type = _type;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddInt(m_ID);
		bytesBuffer.AddByte((byte)m_Type);
		return bytesBuffer.GetBytes();
	}
}
