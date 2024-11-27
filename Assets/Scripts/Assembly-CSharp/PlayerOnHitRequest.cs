public class PlayerOnHitRequest : Request
{
	protected short damage;

	protected bool hasPlayerID;

	protected int playerID;

	public PlayerOnHitRequest(short damage, bool hasPlayerID, int playerID)
	{
		requestID = 110;
		this.damage = damage;
		this.hasPlayerID = hasPlayerID;
		this.playerID = playerID;
	}

	public override byte[] GetBody()
	{
		byte b = 3;
		if (hasPlayerID)
		{
			b += 4;
		}
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		bytesBuffer.AddBool(hasPlayerID);
		bytesBuffer.AddShort(damage);
		if (hasPlayerID)
		{
			bytesBuffer.AddInt(playerID);
		}
		return bytesBuffer.GetBytes();
	}
}
