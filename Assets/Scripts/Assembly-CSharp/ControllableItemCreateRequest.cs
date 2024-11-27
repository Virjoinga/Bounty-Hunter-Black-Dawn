using UnityEngine;

public class ControllableItemCreateRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected byte mLevel;

	protected byte mSubType;

	protected Vector3 mPosition;

	protected Quaternion mRotation;

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

	public ControllableItemCreateRequest(EControllableType type, byte id, byte level, byte subType, Vector3 position, Quaternion rotation, short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
		requestID = 171;
		mType = type;
		mID = id;
		mLevel = level;
		mSubType = subType;
		mPosition = position;
		mRotation = rotation;
		mPara1 = para1;
		mPara2 = para2;
		mPara3 = para3;
		mPara4 = para4;
		mPara5 = para5;
		mPara6 = para6;
		mPara7 = para7;
		mPara8 = para8;
		mPara9 = para9;
		mPara10 = para10;
		mPara11 = para11;
		mPara12 = para12;
		mPara13 = para13;
		mPara14 = para14;
		mPara15 = para15;
		mPara16 = para16;
	}

	public override byte[] GetBody()
	{
		int num = 48;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddByte(mLevel);
		bytesBuffer.AddByte(mSubType);
		bytesBuffer.AddShort((short)(mPosition.x * 10f));
		bytesBuffer.AddShort((short)(mPosition.y * 10f));
		bytesBuffer.AddShort((short)(mPosition.z * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.x * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.y * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.z * 10f));
		bytesBuffer.AddShort(mPara1);
		bytesBuffer.AddShort(mPara2);
		bytesBuffer.AddShort(mPara3);
		bytesBuffer.AddShort(mPara4);
		bytesBuffer.AddShort(mPara5);
		bytesBuffer.AddShort(mPara6);
		bytesBuffer.AddShort(mPara7);
		bytesBuffer.AddShort(mPara8);
		bytesBuffer.AddShort(mPara9);
		bytesBuffer.AddShort(mPara10);
		bytesBuffer.AddShort(mPara11);
		bytesBuffer.AddShort(mPara12);
		bytesBuffer.AddShort(mPara13);
		bytesBuffer.AddShort(mPara14);
		bytesBuffer.AddShort(mPara15);
		bytesBuffer.AddShort(mPara16);
		return bytesBuffer.GetBytes();
	}
}
