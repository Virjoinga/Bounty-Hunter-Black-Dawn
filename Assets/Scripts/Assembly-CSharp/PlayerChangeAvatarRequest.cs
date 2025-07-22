public class PlayerChangeAvatarRequest : Request
{
	protected UserState userState;

	public PlayerChangeAvatarRequest(UserState userState)
	{
		requestID = 170;
		this.userState = userState;
	}

	public override byte[] GetBody()
	{
		byte b = (byte)Global.DECORATION_PART_NUM;
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		userState.WriteDecoration(bytesBuffer);
		return bytesBuffer.GetBytes();
	}
}
