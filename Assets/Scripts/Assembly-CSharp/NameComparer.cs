using System.Collections.Generic;
using UnityEngine;

public class NameComparer : IComparer<GameObject>
{
	public int Compare(GameObject x, GameObject y)
	{
		if (x == null)
		{
			if (y == null)
			{
				return 0;
			}
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		return x.name.CompareTo(y.name);
	}
}
