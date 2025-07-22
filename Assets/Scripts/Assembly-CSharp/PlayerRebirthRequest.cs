public class PlayerRebirthRequest : Request
{
	protected short spawnPointIndex;

	public PlayerRebirthRequest(short spawnPointIndex)
	{
		requestID = 125;
		this.spawnPointIndex = spawnPointIndex;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddShort(spawnPointIndex);
		return bytesBuffer.GetBytes();
	}
}
