public class VSTDMCreateTargetPointInfoRequest : Request
{
	public VSTDMCreateTargetPointInfoRequest()
	{
		requestID = 303;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
