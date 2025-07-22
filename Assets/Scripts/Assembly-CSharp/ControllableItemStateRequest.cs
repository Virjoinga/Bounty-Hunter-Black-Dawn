using UnityEngine;

public class ControllableItemStateRequest : Request
{
	protected EControllableType mType;

	protected byte mID;

	protected ControllableStateConst mStateId;

	protected Vector3 mPosition;

	public ControllableItemStateRequest(EControllableType type, byte id, ControllableStateConst stateId, Vector3 position)
	{
		requestID = 172;
		mType = type;
		mID = id;
		mStateId = stateId;
		mPosition = position;
	}

	public override byte[] GetBody()
	{
		int num = 9;
		BytesBuffer bytesBuffer = new BytesBuffer(num);
		bytesBuffer.AddByte((byte)mType);
		bytesBuffer.AddByte(mID);
		bytesBuffer.AddByte((byte)mStateId);
		bytesBuffer.AddShort((short)(mPosition.x * 10f));
		bytesBuffer.AddShort((short)(mPosition.y * 10f));
		bytesBuffer.AddShort((short)(mPosition.z * 10f));
		return bytesBuffer.GetBytes();
	}
}
