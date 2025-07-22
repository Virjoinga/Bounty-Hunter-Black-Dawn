public class PlayerMeleeAttackRequest : Request
{
	public PlayerMeleeAttackRequest()
	{
		requestID = 200;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		int channelID = Lobby.GetInstance().GetChannelID();
		bytesBuffer.AddInt(channelID);
		return bytesBuffer.GetBytes();
	}
}
