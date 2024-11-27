using System.Collections.Generic;
using UnityEngine;

public class RobotPosition
{
	public Vector3 mPosition;

	public List<byte> mPointIDList;

	public RobotPosition(Vector3 pos, byte[] array)
	{
		mPosition = pos;
		mPointIDList = new List<byte>();
		foreach (byte item in array)
		{
			mPointIDList.Add(item);
		}
	}
}
