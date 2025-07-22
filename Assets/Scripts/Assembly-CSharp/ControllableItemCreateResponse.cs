using UnityEngine;

internal class ControllableItemCreateResponse : Response
{
	protected int mPlayerID;

	protected byte mId;

	protected byte mLevel;

	protected byte mType;

	protected byte mSubType;

	protected int mHp;

	protected int mShield;

	protected short mPositionX;

	protected short mPositionY;

	protected short mPositionZ;

	protected short mRotationX;

	protected short mRotationY;

	protected short mRotationZ;

	protected short mPara1;

	protected short mPara2;

	protected short mPara3;

	protected short mPara4;

	protected short mPara5;

	protected short mPara6;

	protected short mPara7;

	protected short mPara8;

	protected short mPara9;

	protected short mPara10;

	protected short mPara11;

	protected short mPara12;

	protected short mPara13;

	protected short mPara14;

	protected short mPara15;

	protected short mPara16;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPlayerID = bytesBuffer.ReadInt();
		mType = bytesBuffer.ReadByte();
		mId = bytesBuffer.ReadByte();
		mLevel = bytesBuffer.ReadByte();
		mSubType = bytesBuffer.ReadByte();
		mHp = bytesBuffer.ReadInt();
		mShield = bytesBuffer.ReadInt();
		mPositionX = bytesBuffer.ReadShort();
		mPositionY = bytesBuffer.ReadShort();
		mPositionZ = bytesBuffer.ReadShort();
		mRotationX = bytesBuffer.ReadShort();
		mRotationY = bytesBuffer.ReadShort();
		mRotationZ = bytesBuffer.ReadShort();
		mPara1 = bytesBuffer.ReadShort();
		mPara2 = bytesBuffer.ReadShort();
		mPara3 = bytesBuffer.ReadShort();
		mPara4 = bytesBuffer.ReadShort();
		mPara5 = bytesBuffer.ReadShort();
		mPara6 = bytesBuffer.ReadShort();
		mPara7 = bytesBuffer.ReadShort();
		mPara8 = bytesBuffer.ReadShort();
		mPara9 = bytesBuffer.ReadShort();
		mPara10 = bytesBuffer.ReadShort();
		mPara11 = bytesBuffer.ReadShort();
		mPara12 = bytesBuffer.ReadShort();
		mPara13 = bytesBuffer.ReadShort();
		mPara14 = bytesBuffer.ReadShort();
		mPara15 = bytesBuffer.ReadShort();
		mPara16 = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(mPlayerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.InitControllableItem(mId, mLevel, mType, mSubType, mHp, mShield, new Vector3((float)mPositionX / 10f, (float)mPositionY / 10f, (float)mPositionZ / 10f), Quaternion.Euler((float)mRotationX / 10f, (float)mRotationY / 10f, (float)mRotationZ / 10f), mPara1, mPara2, mPara3, mPara4, mPara5, mPara6, mPara7, mPara8, mPara9, mPara10, mPara11, mPara12, mPara13, mPara14, mPara15, mPara16);
		}
	}
}
