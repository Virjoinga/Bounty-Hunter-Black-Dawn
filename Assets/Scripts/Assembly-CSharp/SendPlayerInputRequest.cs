public class SendPlayerInputRequest : Request
{
	protected bool m_Fire;

	protected bool m_IsMoving;

	public SendPlayerInputRequest(bool fire, bool isMoving)
	{
		requestID = 103;
		m_Fire = fire;
		m_IsMoving = isMoving;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddBool(m_Fire);
		bytesBuffer.AddBool(m_IsMoving);
		return bytesBuffer.GetBytes();
	}
}
