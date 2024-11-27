public class PlayerOnDeadRequest : Request
{
	public PlayerOnDeadRequest()
	{
		requestID = 158;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		int channelID = Lobby.GetInstance().GetChannelID();
		bytesBuffer.AddInt(channelID);
		return bytesBuffer.GetBytes();
	}
}
