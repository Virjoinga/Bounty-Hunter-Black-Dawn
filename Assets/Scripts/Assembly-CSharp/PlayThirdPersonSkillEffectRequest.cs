public class PlayThirdPersonSkillEffectRequest : Request
{
	public enum SkillEffectType
	{
		HealingWave = 0
	}

	protected SkillEffectType mEffectType;

	public PlayThirdPersonSkillEffectRequest(SkillEffectType effectType)
	{
		requestID = 186;
		mEffectType = effectType;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(1);
		bytesBuffer.AddByte((byte)mEffectType);
		return bytesBuffer.GetBytes();
	}
}
