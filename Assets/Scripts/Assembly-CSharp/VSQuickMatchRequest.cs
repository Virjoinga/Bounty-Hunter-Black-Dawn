public class VSQuickMatchRequest : Request
{
	private UIVS.Mode mMode;

	private byte mRound;

	public VSQuickMatchRequest(UIVS.Mode mode, byte round)
	{
		requestID = 30;
		mMode = mode;
		mRound = round;
	}

	public override byte[] GetBody()
	{
		int num = 1;
		if (mMode == UIVS.Mode.CaptureHold_1v1)
		{
			num++;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mMode);
		if (mMode == UIVS.Mode.CaptureHold_1v1)
		{
			bytesBuffer.AddByte(mRound);
		}
		return bytesBuffer.GetBytes();
	}
}
