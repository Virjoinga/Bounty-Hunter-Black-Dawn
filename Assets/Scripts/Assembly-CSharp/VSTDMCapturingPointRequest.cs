public class VSTDMCapturingPointRequest : Request
{
	protected short mPointID;

	public VSTDMCapturingPointRequest(short pointID)
	{
		requestID = 301;
		mPointID = pointID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(mPointID);
		return bytesBuffer.GetBytes();
	}
}
