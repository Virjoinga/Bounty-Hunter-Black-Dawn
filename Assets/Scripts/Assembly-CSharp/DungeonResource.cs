using UnityEngine;

public class DungeonResource
{
	private static DungeonResource mInstance;

	private GameObject[] mPrefabs;

	private DungeonResource()
	{
		mPrefabs = new GameObject[DungeonBlockType.TYPE_NUM];
	}

	public static DungeonResource GetInstance()
	{
		if (mInstance == null)
		{
			mInstance = new DungeonResource();
		}
		return mInstance;
	}

	public void LoadAllResource()
	{
	}

	public GameObject GetPrefab(byte type)
	{
		if (type < DungeonBlockType.TYPE_NUM)
		{
			return mPrefabs[type];
		}
		return null;
	}
}
