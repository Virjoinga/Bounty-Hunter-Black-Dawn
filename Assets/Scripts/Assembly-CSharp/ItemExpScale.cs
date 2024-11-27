using System.IO;
using UnityEngine;

public class ItemExpScale : GlobalIAPItem
{
	private float expScale = 1f;

	public void ReadData(BinaryReader br)
	{
		expScale = (float)br.ReadInt32() / 100f;
	}

	public void WriteData(BinaryWriter bw)
	{
		bw.Write((int)(expScale * 100f));
	}

	public void Resume()
	{
		Debug.Log("expScale : " + expScale);
		expScale = 2f;
	}

	public float GetScale()
	{
		return expScale;
	}
}
