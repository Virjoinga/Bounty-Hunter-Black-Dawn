public class ChangeSeatRequest : Request
{
	public byte seatID;

	public ChangeSeatRequest(byte seatId)
	{
		requestID = 20;
		seatID = seatId;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(seatID);
		return bytesBuffer.GetBytes();
	}
}
