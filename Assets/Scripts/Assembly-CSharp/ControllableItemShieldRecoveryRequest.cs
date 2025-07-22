public class ControllableItemShieldRecoveryRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected int mDeltaShield;

	public ControllableItemShieldRecoveryRequest(EControllableType type, byte id, int deltaShield)
	{
		requestID = 176;
		mType = type;
		mID = id;
		mDeltaShield = deltaShield;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddInt(mDeltaShield);
		return bytesBuffer.GetBytes();
	}
}
