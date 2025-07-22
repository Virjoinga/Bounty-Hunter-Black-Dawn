public class PlayerLevelUpRequest : Request
{
	protected short level;

	public PlayerLevelUpRequest(short level)
	{
		requestID = 189;
		this.level = level;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(level);
		return bytesBuffer.GetBytes();
	}
}
