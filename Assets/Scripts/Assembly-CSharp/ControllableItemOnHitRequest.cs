using UnityEngine;

public class ControllableItemOnHitRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected int mDamage;

	public ControllableItemOnHitRequest(EControllableType type, byte id, int damage)
	{
		requestID = 173;
		mType = type;
		mID = id;
		mDamage = damage;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			mDamage = (short)Mathf.CeilToInt((float)mDamage * 0.4f);
		}
	}

	public override byte[] GetBody()
	{
		int num = 6;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddInt(mDamage);
		return bytesBuffer.GetBytes();
	}
}
