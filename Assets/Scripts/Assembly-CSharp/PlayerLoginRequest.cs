public class PlayerLoginRequest : Request
{
	public string userName;

	public string passWord;

	public string version;

	public PlayerLoginRequest()
	{
		requestID = 1;
	}

	public override byte[] GetBody()
	{
		byte b = (byte)(BytesBuffer.GetStringByteLength(userName) + BytesBuffer.GetStringByteLength(passWord) + BytesBuffer.GetStringByteLength(version) + 4);
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		bytesBuffer.AddString(userName);
		bytesBuffer.AddString(passWord);
		bytesBuffer.AddString(version);
		bytesBuffer.AddInt(GlobalState.user_id);
		return bytesBuffer.GetBytes();
	}
}
