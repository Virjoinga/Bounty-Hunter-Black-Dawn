public class PlayerJoinTeamStartGameRequest : Request
{
	public PlayerJoinTeamStartGameRequest()
	{
		requestID = 19;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(0);
		return bytesBuffer.GetBytes();
	}
}
