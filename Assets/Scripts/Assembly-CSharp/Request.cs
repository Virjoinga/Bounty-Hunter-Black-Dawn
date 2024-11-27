public abstract class Request
{
	public short requestID;

	public short length;

	public abstract byte[] GetBody();

	public byte[] GetBytes()
	{
		byte[] body = GetBody();
		BytesBuffer bytesBuffer = new BytesBuffer(4 + body.Length);
		bytesBuffer.AddShort(requestID);
		bytesBuffer.AddShort((short)body.Length);
		bytesBuffer.AddBytes(body);
		return bytesBuffer.GetBytes();
	}
}
