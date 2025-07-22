public class RoleLoginRequest : Request
{
	protected string userName;

	protected int userID;

	protected byte userType;

	protected string gcName;

	public RoleLoginRequest(string userName, int userID, byte userType)
	{
		requestID = 12;
		this.userName = userName;
		this.userID = userID;
		this.userType = userType;
		gcName = "Guest";
	}

	public override byte[] GetBody()
	{
		byte b = (byte)(BytesBuffer.GetStringByteLength(userName) + BytesBuffer.GetStringByteLength(gcName) + 5);
		BytesBuffer bytesBuffer = new BytesBuffer(b);
		bytesBuffer.AddInt(userID);
		bytesBuffer.AddString(userName);
		bytesBuffer.AddString(gcName);
		bytesBuffer.AddByte(userType);
		return bytesBuffer.GetBytes();
	}
}
