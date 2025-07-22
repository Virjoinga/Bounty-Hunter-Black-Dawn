public class SearchRoomAdvancedRequest : Request
{
	private byte mOnlineMode;

	private byte mPlayerRankID;

	private byte mLowerRankID;

	private byte mUpperRankID;

	private byte mGameMode;

	private byte mWinCondition;

	private short mWinValue;

	private bool mIsAutoBalance;

	private byte mPlayerNumber;

	public SearchRoomAdvancedRequest(byte onlineMode, byte playerRankID, byte lowerRankID, byte upperRankID, byte gameMode, byte winCondition, short winValue, bool autoBalance, byte playerNumber)
	{
		requestID = 21;
		mOnlineMode = onlineMode;
		mPlayerRankID = playerRankID;
		mLowerRankID = lowerRankID;
		mUpperRankID = upperRankID;
		mGameMode = gameMode;
		mWinCondition = winCondition;
		mWinValue = winValue;
		mIsAutoBalance = autoBalance;
		mPlayerNumber = playerNumber;
	}

	public override byte[] GetBody()
	{
		int num = 10;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte(mOnlineMode);
		bytesBuffer.AddByte(mPlayerRankID);
		bytesBuffer.AddByte(mLowerRankID);
		bytesBuffer.AddByte(mUpperRankID);
		bytesBuffer.AddByte(mGameMode);
		bytesBuffer.AddByte(mWinCondition);
		bytesBuffer.AddShort(mWinValue);
		bytesBuffer.AddBool(mIsAutoBalance);
		bytesBuffer.AddByte(mPlayerNumber);
		return bytesBuffer.GetBytes();
	}
}
