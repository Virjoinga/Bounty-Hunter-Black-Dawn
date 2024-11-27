using UnityEngine;

public class PlayerFireRocketRequest : Request
{
	protected byte type;

	protected short x;

	protected short y;

	protected short z;

	protected short dx;

	protected short dy;

	protected short dz;

	protected byte elementType;

	public PlayerFireRocketRequest(byte type, Vector3 pos, Vector3 dir, ElementType _elementType)
	{
		requestID = 114;
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		elementType = (byte)_elementType;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(14);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(dx);
		bytesBuffer.AddShort(dy);
		bytesBuffer.AddShort(dz);
		bytesBuffer.AddByte(elementType);
		return bytesBuffer.GetBytes();
	}
}
