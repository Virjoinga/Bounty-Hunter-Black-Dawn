public class SearchRoomRequest : Request
{
	public string mSearchText;

	private Mode mMode;

	public SearchRoomRequest(Mode mode, string text)
	{
		requestID = 10;
		mSearchText = text;
		mMode = mode;
	}

	public override byte[] GetBody()
	{
		byte stringByteLength = BytesBuffer.GetStringByteLength(mSearchText);
		int num = stringByteLength + 1;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mMode);
		bytesBuffer.AddString(mSearchText);
		return bytesBuffer.GetBytes();
	}
}
