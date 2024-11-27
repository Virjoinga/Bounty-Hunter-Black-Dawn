public class LeaveRoomRequest : Request
{
	public LeaveRoomRequest()
	{
		requestID = 6;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
