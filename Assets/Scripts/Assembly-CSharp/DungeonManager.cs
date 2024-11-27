using UnityEngine;

public class DungeonManager
{
	private static DungeonManager mInstance;

	private DungeonMap[] mMaps;

	private int mMapNum;

	public int CurrentMapId { get; set; }

	public DungeonMap CurrentMap
	{
		get
		{
			if (CurrentMapId >= 0 && CurrentMapId < mMapNum)
			{
				return mMaps[CurrentMapId];
			}
			return null;
		}
	}

	public DungeonMap this[int i]
	{
		get
		{
			if (i >= 0 && i < mMapNum)
			{
				return mMaps[i];
			}
			return null;
		}
	}

	private DungeonManager()
	{
	}

	public static DungeonManager GetInstance()
	{
		if (mInstance == null)
		{
			mInstance = new DungeonManager();
		}
		return mInstance;
	}

	public void Create()
	{
		mMapNum = 10;
		mMaps = new DungeonMap[mMapNum];
		int roomNum = Random.Range(10, 15);
		mMaps[0] = new DungeonMap(0, 0.0, roomNum);
		mMaps[0].Create();
		mMaps[0].StartX = 0;
		mMaps[0].StartY = 0;
		DungeonMap.Direction enterDirection = (DungeonMap.Direction)Random.Range(0, 4);
		mMaps[0].SetEnterDirection(enterDirection);
		for (int i = 1; i < mMapNum; i++)
		{
			roomNum = Random.Range(10, 15);
			mMaps[i] = new DungeonMap(i, 0.0, roomNum);
			mMaps[i].Create();
			bool flag = false;
			int num = Random.Range(0, 4);
			for (int j = num; j < 4; j++)
			{
				if (j == (int)mMaps[i - 1].GetEnterDirection())
				{
					continue;
				}
				mMaps[i - 1].SetExitDirection((DungeonMap.Direction)j);
				mMaps[i].SetEnterDirection(GetOppositeDirection(mMaps[i - 1].GetExitDirection()));
				CalculateEnterMapPosition(mMaps[i], mMaps[i - 1]);
				flag = true;
				for (int k = 0; k < i - 1; k++)
				{
					if (mMaps[k] != null && mMaps[i].Intersect(mMaps[k]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				for (int l = 0; l < num; l++)
				{
					if (l == (int)mMaps[i - 1].GetEnterDirection())
					{
						continue;
					}
					mMaps[i - 1].SetExitDirection((DungeonMap.Direction)l);
					mMaps[i].SetEnterDirection(GetOppositeDirection(mMaps[i - 1].GetExitDirection()));
					CalculateEnterMapPosition(mMaps[i], mMaps[i - 1]);
					flag = true;
					for (int m = 0; m < i - 1; m++)
					{
						if (mMaps[m] != null && mMaps[i].Intersect(mMaps[m]))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				NeighborRoomPair neighborRoomPair = null;
				int minX = mMaps[i].EnterRoom.MinX;
				int minY = mMaps[i].EnterRoom.MinY;
				int maxX = mMaps[i].EnterRoom.MaxX;
				int maxY = mMaps[i].EnterRoom.MaxY;
				mMaps[i].EnterRoom.MinX += mMaps[i].StartX - mMaps[i - 1].StartX;
				mMaps[i].EnterRoom.MinY += mMaps[i].StartY - mMaps[i - 1].StartY;
				mMaps[i].EnterRoom.MaxX += mMaps[i].StartX - mMaps[i - 1].StartX;
				mMaps[i].EnterRoom.MaxY += mMaps[i].StartY - mMaps[i - 1].StartY;
				switch (mMaps[i - 1].GetExitDirection())
				{
				case DungeonMap.Direction.EAST:
					num2 = Mathf.Min(mMaps[i].EnterRoom.MaxY, mMaps[i - 1].ExitRoom.MaxY) - Mathf.Max(mMaps[i].EnterRoom.MinY, mMaps[i - 1].ExitRoom.MinY) + 1;
					num3 = num2 * Random.Range(50, 100) / 100 + 1;
					num4 = Random.Range(0, num2 - num3);
					num5 = Mathf.Max(mMaps[i].EnterRoom.MinY, mMaps[i - 1].ExitRoom.MinY) + num4;
					neighborRoomPair = new NeighborRoomPair(mMaps[i - 1].ExitRoom, mMaps[i].EnterRoom, num5, num3);
					neighborRoomPair.CreateHorizontalPath();
					break;
				case DungeonMap.Direction.SOUTH:
					num2 = Mathf.Min(mMaps[i].EnterRoom.MaxX, mMaps[i - 1].ExitRoom.MaxX) - Mathf.Max(mMaps[i].EnterRoom.MinX, mMaps[i - 1].ExitRoom.MinX) + 1;
					num3 = num2 * Random.Range(50, 100) / 100 + 1;
					num4 = Random.Range(0, num2 - num3);
					num5 = Mathf.Max(mMaps[i].EnterRoom.MinX, mMaps[i - 1].ExitRoom.MinX) + num4;
					neighborRoomPair = new NeighborRoomPair(mMaps[i].EnterRoom, mMaps[i - 1].ExitRoom, num5, num3);
					neighborRoomPair.CreateVerticalPath();
					break;
				case DungeonMap.Direction.WEST:
					num2 = Mathf.Min(mMaps[i].EnterRoom.MaxY, mMaps[i - 1].ExitRoom.MaxY) - Mathf.Max(mMaps[i].EnterRoom.MinY, mMaps[i - 1].ExitRoom.MinY) + 1;
					num3 = num2 * Random.Range(50, 100) / 100 + 1;
					num4 = Random.Range(0, num2 - num3);
					num5 = Mathf.Max(mMaps[i].EnterRoom.MinY, mMaps[i - 1].ExitRoom.MinY) + num4;
					neighborRoomPair = new NeighborRoomPair(mMaps[i].EnterRoom, mMaps[i - 1].ExitRoom, num5, num3);
					neighborRoomPair.CreateHorizontalPath();
					break;
				case DungeonMap.Direction.NORTH:
					num2 = Mathf.Min(mMaps[i].EnterRoom.MaxX, mMaps[i - 1].ExitRoom.MaxX) - Mathf.Max(mMaps[i].EnterRoom.MinX, mMaps[i - 1].ExitRoom.MinX) + 1;
					num3 = num2 * Random.Range(50, 100) / 100 + 1;
					num4 = Random.Range(0, num2 - num3);
					num5 = Mathf.Max(mMaps[i].EnterRoom.MinX, mMaps[i - 1].ExitRoom.MinX) + num4;
					neighborRoomPair = new NeighborRoomPair(mMaps[i - 1].ExitRoom, mMaps[i].EnterRoom, num5, num3);
					neighborRoomPair.CreateVerticalPath();
					break;
				}
				mMaps[i].EnterRoom.MinX = minX;
				mMaps[i].EnterRoom.MinY = minY;
				mMaps[i].EnterRoom.MaxX = maxX;
				mMaps[i].EnterRoom.MaxY = maxY;
			}
		}
	}

	public void LoadFirstMap()
	{
		CurrentMapId = -1;
		mMaps[0].Load();
		mMaps[0].TriggerScript.mMapToUnload = null;
		mMaps[0].TriggerScript.mMapToLoad = mMaps[1];
		GameObject gameObject = new GameObject("StartPoint");
		gameObject.tag = TagName.RESPAWN;
		float num = 0f;
		Debug.Log("mMaps[0].GetEnterDirection() = " + mMaps[0].GetEnterDirection());
		switch (mMaps[0].GetEnterDirection())
		{
		case DungeonMap.Direction.EAST:
			num = (float)mMaps[0].EnterRoom.LengthY / 2f;
			gameObject.transform.position = new Vector3((float)(mMaps[0].EnterRoom.MaxX - 1) * 5f, 0.2f, ((float)mMaps[0].EnterRoom.MinY + num) * 5f);
			gameObject.transform.rotation = Quaternion.AngleAxis(270f, Vector3.up);
			break;
		case DungeonMap.Direction.SOUTH:
			num = (float)mMaps[0].EnterRoom.LengthX / 2f;
			gameObject.transform.position = new Vector3(((float)mMaps[0].EnterRoom.MinX + num) * 5f, 0.2f, (float)(mMaps[0].EnterRoom.MinY + 1) * 5f);
			gameObject.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
			break;
		case DungeonMap.Direction.WEST:
			num = (float)mMaps[0].EnterRoom.LengthY / 2f;
			gameObject.transform.position = new Vector3((float)(mMaps[0].EnterRoom.MinX + 1) * 5f, 0.2f, ((float)mMaps[0].EnterRoom.MinY + num) * 5f);
			gameObject.transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
			break;
		case DungeonMap.Direction.NORTH:
			num = (float)mMaps[0].EnterRoom.LengthX / 2f;
			gameObject.transform.position = new Vector3(((float)mMaps[0].EnterRoom.MinX + num) * 5f, 0.2f, (float)(mMaps[0].EnterRoom.MaxY - 1) * 5f);
			gameObject.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
			break;
		}
		for (int i = 0; i < 3; i++)
		{
			Object.Instantiate(gameObject);
		}
	}

	private void CalculateEnterMapPosition(DungeonMap enterMap, DungeonMap exitMap)
	{
		int num = 0;
		int num2 = 0;
		DungeonRoom enterRoom = enterMap.EnterRoom;
		DungeonRoom exitRoom = exitMap.ExitRoom;
		switch (enterMap.GetEnterDirection())
		{
		case DungeonMap.Direction.EAST:
			num = exitRoom.MinX - enterRoom.LengthX;
			num2 = exitRoom.MinY + (exitRoom.LengthY - enterRoom.LengthY) / 2;
			break;
		case DungeonMap.Direction.SOUTH:
			num = exitRoom.MinX + (exitRoom.LengthX - enterRoom.LengthX) / 2;
			num2 = exitRoom.MaxY + 1;
			break;
		case DungeonMap.Direction.WEST:
			num = exitRoom.MaxX + 1;
			num2 = exitRoom.MinY + (exitRoom.LengthY - enterRoom.LengthY) / 2;
			break;
		case DungeonMap.Direction.NORTH:
			num = exitRoom.MinX + (exitRoom.LengthX - enterRoom.LengthX) / 2;
			num2 = exitRoom.MinY - enterRoom.LengthY;
			break;
		}
		enterMap.StartX = exitMap.StartX + num - enterRoom.MinX;
		enterMap.StartY = exitMap.StartY + num2 - enterRoom.MinY;
	}

	private DungeonMap.Direction GetOppositeDirection(DungeonMap.Direction direction)
	{
		DungeonMap.Direction result = DungeonMap.Direction.EAST;
		switch (direction)
		{
		case DungeonMap.Direction.EAST:
			result = DungeonMap.Direction.WEST;
			break;
		case DungeonMap.Direction.SOUTH:
			result = DungeonMap.Direction.NORTH;
			break;
		case DungeonMap.Direction.WEST:
			result = DungeonMap.Direction.EAST;
			break;
		case DungeonMap.Direction.NORTH:
			result = DungeonMap.Direction.SOUTH;
			break;
		}
		return result;
	}
}
