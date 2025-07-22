using UnityEngine;

public class ItemSpawnRequest : Request
{
	protected short px;

	protected short py;

	protected short pz;

	protected short fx;

	protected short fy;

	protected short fz;

	protected short specialID;

	public ItemSpawnRequest(short specialID, Vector3 pos, Vector3 force)
	{
		requestID = 119;
		px = (short)(pos.x * 10f);
		py = (short)(pos.y * 10f);
		pz = (short)(pos.z * 10f);
		fx = (short)(force.x * 10f);
		fy = (short)(force.y * 10f);
		fz = (short)(force.z * 10f);
		this.specialID = specialID;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(14);
		bytesBuffer.AddShort(px);
		bytesBuffer.AddShort(py);
		bytesBuffer.AddShort(pz);
		bytesBuffer.AddShort(fx);
		bytesBuffer.AddShort(fy);
		bytesBuffer.AddShort(fz);
		bytesBuffer.AddShort(specialID);
		return bytesBuffer.GetBytes();
	}
}
