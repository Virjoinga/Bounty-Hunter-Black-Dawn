public class ControllableItemChangePVPTargetRequest : Request
{
	protected bool mIsTargetPlayer;

	protected int mTargetID;

	protected EControllableType mType;

	protected byte mSelfID;

	public ControllableItemChangePVPTargetRequest(bool isTargetPlayer, int id, EControllableType eType, byte selfID)
	{
		requestID = 197;
		mIsTargetPlayer = isTargetPlayer;
		mTargetID = id;
		mType = eType;
		mSelfID = selfID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(7);
		bytesBuffer.AddBool(mIsTargetPlayer);
		bytesBuffer.AddInt(mTargetID);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mSelfID);
		return bytesBuffer.GetBytes();
	}
}
