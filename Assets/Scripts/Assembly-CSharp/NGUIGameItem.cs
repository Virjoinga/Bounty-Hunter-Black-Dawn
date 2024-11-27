using System;
using UnityEngine;

[Serializable]
public class NGUIGameItem
{
	[SerializeField]
	private int mBaseItemID;

	public int itemLevel = 1;

	private NGUIBaseItem mBaseItem;

	public int baseItemID
	{
		get
		{
			return mBaseItemID;
		}
	}

	public NGUIBaseItem baseItem
	{
		get
		{
			return mBaseItem;
		}
	}

	public string name
	{
		get
		{
			if (baseItem == null)
			{
				return null;
			}
			return baseItem.name;
		}
	}

	public NGUIGameItem(int id)
	{
		mBaseItemID = id;
	}

	public NGUIGameItem(int id, NGUIBaseItem bi)
	{
		mBaseItemID = id;
		mBaseItem = bi;
	}
}
