public class ClearRemotePlayerBuffEffectRequest : Request
{
	protected byte mEffectType;

	public ClearRemotePlayerBuffEffectRequest(byte effectType)
	{
		requestID = 196;
		mEffectType = effectType;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mEffectType);
		return bytesBuffer.GetBytes();
	}
}
