public class RemotePlayerHpRecoveryRequest : Request
{
	protected int userID;

	protected short point;

	public RemotePlayerHpRecoveryRequest(int id, short point)
	{
		requestID = 178;
		userID = id;
		this.point = point;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddInt(userID);
		bytesBuffer.AddShort(point);
		return bytesBuffer.GetBytes();
	}
}
