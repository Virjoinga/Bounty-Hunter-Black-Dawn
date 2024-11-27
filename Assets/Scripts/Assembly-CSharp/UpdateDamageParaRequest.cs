public class UpdateDamageParaRequest : Request
{
	protected byte mDamageParaEnum;

	protected int mValue;

	public UpdateDamageParaRequest(LocalPlayer.DamagePara paraEnum, int value)
	{
		requestID = 194;
		mDamageParaEnum = (byte)paraEnum;
		mValue = value;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(mDamageParaEnum);
		bytesBuffer.AddInt(mValue);
		return bytesBuffer.GetBytes();
	}
}
