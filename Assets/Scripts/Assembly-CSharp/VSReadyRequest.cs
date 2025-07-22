public class VSReadyRequest : Request
{
	public enum PVPMode
	{
		m1v1 = 0,
		m4v4 = 1
	}

	private int m_UserID;

	private PVPMode m_Mode;

	public VSReadyRequest(int userID)
	{
		requestID = 193;
		m_UserID = userID;
	}

	public override byte[] GetBody()
	{
		int num = 4;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddInt(m_UserID);
		return bytesBuffer.GetBytes();
	}
}
