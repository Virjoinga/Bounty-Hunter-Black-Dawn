using UnityEngine;

public class ControllableItemSendTransformRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected Vector3 mPosition;

	protected Quaternion mRotation;

	public ControllableItemSendTransformRequest(EControllableType type, byte id, Vector3 position, Quaternion rotation)
	{
		requestID = 184;
		mType = type;
		mID = id;
		mPosition = position;
		mRotation = rotation;
	}

	public override byte[] GetBody()
	{
		int num = 14;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddShort((short)(mPosition.x * 10f));
		bytesBuffer.AddShort((short)(mPosition.y * 10f));
		bytesBuffer.AddShort((short)(mPosition.z * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.x * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.y * 10f));
		bytesBuffer.AddShort((short)(mRotation.eulerAngles.z * 10f));
		return bytesBuffer.GetBytes();
	}
}
