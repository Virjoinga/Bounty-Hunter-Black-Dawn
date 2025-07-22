public class PlayRemotePlayerBuffEffectRequest : Request
{
	protected byte mEffectType;

	public PlayRemotePlayerBuffEffectRequest(byte effectType)
	{
		requestID = 195;
		mEffectType = effectType;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte(mEffectType);
		return bytesBuffer.GetBytes();
	}
}
