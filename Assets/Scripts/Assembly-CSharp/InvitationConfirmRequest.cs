public class InvitationConfirmRequest : Request
{
	private bool bEnter;

	private int m_UserID;

	public InvitationConfirmRequest(int userID, bool enter)
	{
		requestID = 163;
		m_UserID = userID;
		bEnter = enter;
	}

	public override byte[] GetBody()
	{
		int num = 5;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddInt(m_UserID);
		bytesBuffer.AddBool(bEnter);
		return bytesBuffer.GetBytes();
	}
}
