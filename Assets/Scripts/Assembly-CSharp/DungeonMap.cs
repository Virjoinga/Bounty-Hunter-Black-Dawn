using UnityEngine;

public class DungeonMap
{
	public enum Direction
	{
		EAST = 0,
		SOUTH = 1,
		WEST = 2,
		NORTH = 3,
		NUM = 4
	}

	private int mMapId;

	private double mRandomSeed;

	private int mRoomNum;

	private int mSizeX;

	private int mSizeY;

	private DungeonRoom[] mRooms;

	private GameObject mMapObject;

	private bool isLoad;

	private Direction mEnterDirection;

	private Direction mExitDirection;

	private DungeonRoom[] mBorderRooms;

	private DungeonRoom mEnterRoom;

	private DungeonRoom mExitRoom;

	private DungeonStreamingTriggerScript mTriggerScript;

	public int StartX { get; set; }

	public int StartY { get; set; }

	public int SizeX
	{
		get
		{
			return mSizeX;
		}
	}

	public int SizeY
	{
		get
		{
			return mSizeY;
		}
	}

	public DungeonRoom EnterRoom
	{
		get
		{
			return mEnterRoom;
		}
	}

	public DungeonRoom ExitRoom
	{
		get
		{
			return mExitRoom;
		}
	}

	public DungeonRoom this[int i]
	{
		get
		{
			if (i >= 0 && i < mRoomNum)
			{
				return mRooms[i];
			}
			return null;
		}
	}

	public GameObject MapObject
	{
		get
		{
			return mMapObject;
		}
	}

	public int RoomNum
	{
		get
		{
			return mRoomNum;
		}
	}

	public DungeonStreamingTriggerScript TriggerScript
	{
		get
		{
			return mTriggerScript;
		}
	}

	public DungeonMap(int id, double seed, int roomNum)
	{
		mMapId = id;
		mRandomSeed = seed;
		mRoomNum = roomNum;
		mRooms = new DungeonRoom[mRoomNum];
		for (int i = 0; i < mRoomNum; i++)
		{
			mRooms[i] = null;
		}
		mBorderRooms = new DungeonRoom[4];
		for (int j = 0; j < 4; j++)
		{
			mBorderRooms[j] = null;
		}
	}

	public void Create()
	{
		int num = Random.Range(4, 8);
		int num2 = Random.Range(4, 8);
		mRooms[0] = new DungeonRoom(0, num, num2);
		mRooms[0].MinX = 0;
		mRooms[0].MinY = 0;
		mRooms[0].MaxX = num - 1;
		mRooms[0].MaxY = num2 - 1;
		mRooms[0].Create();
		for (int i = 1; i < mRoomNum; i++)
		{
			int num3 = 0;
			while (num3++ < 1000)
			{
				num = Random.Range(4, 8);
				num2 = Random.Range(4, 8);
				mRooms[i] = new DungeonRoom(i, num, num2);
				DungeonRoom dungeonRoom = mRooms[i];
				int num4 = Random.Range(0, i);
				DungeonRoom dungeonRoom2 = mRooms[num4];
				if (dungeonRoom2 == null)
				{
					continue;
				}
				switch ((Direction)Random.Range(0, 4))
				{
				case Direction.EAST:
				{
					dungeonRoom.MinX = dungeonRoom2.MaxX + 1;
					dungeonRoom.MaxX = dungeonRoom.MinX + dungeonRoom.LengthX - 1;
					int num8 = Random.Range(2, dungeonRoom2.LengthY - 2);
					dungeonRoom.MinY = dungeonRoom2.MinY + num8 - dungeonRoom.LengthY / 2;
					dungeonRoom.MaxY = dungeonRoom.MinY + dungeonRoom.LengthY - 1;
					break;
				}
				case Direction.SOUTH:
				{
					int num7 = Random.Range(2, dungeonRoom2.LengthX - 2);
					dungeonRoom.MinX = dungeonRoom2.MinX + num7 - dungeonRoom.LengthX / 2;
					dungeonRoom.MaxX = dungeonRoom.MinX + dungeonRoom.LengthX - 1;
					dungeonRoom.MaxY = dungeonRoom2.MinY - 1;
					dungeonRoom.MinY = dungeonRoom.MaxY - dungeonRoom.LengthY + 1;
					break;
				}
				case Direction.WEST:
				{
					dungeonRoom.MaxX = dungeonRoom2.MinX - 1;
					dungeonRoom.MinX = dungeonRoom.MaxX - dungeonRoom.LengthX + 1;
					int num6 = Random.Range(2, dungeonRoom2.LengthY - 2);
					dungeonRoom.MinY = dungeonRoom2.MinY + num6 - dungeonRoom.LengthY / 2;
					dungeonRoom.MaxY = dungeonRoom.MinY + dungeonRoom.LengthY - 1;
					break;
				}
				case Direction.NORTH:
				{
					int num5 = Random.Range(2, dungeonRoom2.LengthX - 2);
					dungeonRoom.MinX = dungeonRoom2.MinX + num5 - dungeonRoom.LengthX / 2;
					dungeonRoom.MaxX = dungeonRoom.MinX + dungeonRoom.LengthX - 1;
					dungeonRoom.MinY = dungeonRoom2.MaxY + 1;
					dungeonRoom.MaxY = dungeonRoom.MinY + dungeonRoom.LengthY - 1;
					break;
				}
				}
				bool flag = true;
				for (int j = 0; j < i; j++)
				{
					if (mRooms[j] != null && dungeonRoom.Intersect(mRooms[j]))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				dungeonRoom.Create();
				for (int k = 0; k < i; k++)
				{
					if (mRooms[k] != null)
					{
						if (mRooms[k].MinY == dungeonRoom.MaxY + 1 && dungeonRoom.IntersectX(mRooms[k], 3))
						{
							int num9 = Mathf.Min(dungeonRoom.MaxX, mRooms[k].MaxX) - Mathf.Max(dungeonRoom.MinX, mRooms[k].MinX) + 1;
							int num10 = num9 * Random.Range(50, 100) / 100 + 1;
							int num11 = Random.Range(0, num9 - num10);
							int start = Mathf.Max(dungeonRoom.MinX, mRooms[k].MinX) + num11;
							NeighborRoomPair neighborRoomPair = new NeighborRoomPair(dungeonRoom, mRooms[k], start, num10);
							neighborRoomPair.CreateVerticalPath();
						}
						else if (dungeonRoom.MinY == mRooms[k].MaxY + 1 && dungeonRoom.IntersectX(mRooms[k], 3))
						{
							int num12 = Mathf.Min(dungeonRoom.MaxX, mRooms[k].MaxX) - Mathf.Max(dungeonRoom.MinX, mRooms[k].MinX) + 1;
							int num13 = num12 * Random.Range(50, 100) / 100 + 1;
							int num14 = Random.Range(0, num12 - num13);
							int start2 = Mathf.Max(dungeonRoom.MinX, mRooms[k].MinX) + num14;
							NeighborRoomPair neighborRoomPair2 = new NeighborRoomPair(mRooms[k], dungeonRoom, start2, num13);
							neighborRoomPair2.CreateVerticalPath();
						}
						else if (mRooms[k].MinX == dungeonRoom.MaxX + 1 && dungeonRoom.IntersectY(mRooms[k], 3))
						{
							int num15 = Mathf.Min(dungeonRoom.MaxY, mRooms[k].MaxY) - Mathf.Max(dungeonRoom.MinY, mRooms[k].MinY) + 1;
							int num16 = num15 * Random.Range(50, 100) / 100 + 1;
							int num17 = Random.Range(0, num15 - num16);
							int start3 = Mathf.Max(dungeonRoom.MinY, mRooms[k].MinY) + num17;
							NeighborRoomPair neighborRoomPair3 = new NeighborRoomPair(dungeonRoom, mRooms[k], start3, num16);
							neighborRoomPair3.CreateHorizontalPath();
						}
						else if (dungeonRoom.MinX == mRooms[k].MaxX + 1 && dungeonRoom.IntersectY(mRooms[k], 3))
						{
							int num18 = Mathf.Min(dungeonRoom.MaxY, mRooms[k].MaxY) - Mathf.Max(dungeonRoom.MinY, mRooms[k].MinY) + 1;
							int num19 = num18 * Random.Range(50, 100) / 100 + 1;
							int num20 = Random.Range(0, num18 - num19);
							int start4 = Mathf.Max(dungeonRoom.MinY, mRooms[k].MinY) + num20;
							NeighborRoomPair neighborRoomPair4 = new NeighborRoomPair(mRooms[k], dungeonRoom, start4, num19);
							neighborRoomPair4.CreateHorizontalPath();
						}
					}
				}
				break;
			}
		}
		int num21 = 0;
		int num22 = 0;
		int num23 = 0;
		int num24 = 0;
		DungeonRoom[] array = mRooms;
		foreach (DungeonRoom dungeonRoom3 in array)
		{
			if (dungeonRoom3 != null)
			{
				if (dungeonRoom3.MinX < num21)
				{
					num21 = dungeonRoom3.MinX;
				}
				if (dungeonRoom3.MaxX > num22)
				{
					num22 = dungeonRoom3.MaxX;
				}
				if (dungeonRoom3.MinY < num23)
				{
					num23 = dungeonRoom3.MinY;
				}
				if (dungeonRoom3.MaxY > num24)
				{
					num24 = dungeonRoom3.MaxY;
				}
			}
		}
		mSizeX = num22 - num21 + 1;
		mSizeY = num24 - num23 + 1;
		DungeonRoom[] array2 = mRooms;
		foreach (DungeonRoom dungeonRoom4 in array2)
		{
			if (dungeonRoom4 != null)
			{
				if (dungeonRoom4.MinX == num21)
				{
					mBorderRooms[2] = dungeonRoom4;
				}
				if (dungeonRoom4.MaxX == num22)
				{
					mBorderRooms[0] = dungeonRoom4;
				}
				if (dungeonRoom4.MinY == num23)
				{
					mBorderRooms[1] = dungeonRoom4;
				}
				if (dungeonRoom4.MaxY == num24)
				{
					mBorderRooms[3] = dungeonRoom4;
				}
			}
		}
		DungeonRoom[] array3 = mRooms;
		foreach (DungeonRoom dungeonRoom5 in array3)
		{
			if (dungeonRoom5 != null)
			{
				dungeonRoom5.MinX -= num21;
				dungeonRoom5.MaxX -= num21;
				dungeonRoom5.MinY -= num23;
				dungeonRoom5.MaxY -= num23;
			}
		}
	}

	public void Load()
	{
		CreateMapObject();
		for (int i = 0; i < mRoomNum; i++)
		{
			if (mRooms[i] != null)
			{
				SetRoomPosition(i);
				mRooms[i].Load();
			}
		}
	}

	public void CreateMapObject()
	{
		mMapObject = new GameObject("DungeonMap" + mMapId);
		mMapObject.transform.position = new Vector3(5f * (float)StartX, 0f, 5f * (float)StartY);
		mMapObject.tag = TagName.LEVEL;
		mMapObject.isStatic = true;
		mMapObject.AddComponent<BoxCollider>();
		mMapObject.layer = PhysicsLayer.ITEMS;
		BoxCollider component = mMapObject.GetComponent<BoxCollider>();
		component.center = new Vector3((float)mSizeX * 2.5f, 2f, (float)mSizeY * 2.5f);
		component.size = new Vector3((float)mSizeX * 5f, 4f, (float)mSizeY * 5f);
		component.isTrigger = true;
		mMapObject.AddComponent<DungeonStreamingTriggerScript>();
		mTriggerScript = mMapObject.GetComponent<DungeonStreamingTriggerScript>();
		mTriggerScript.mCurrentMapId = mMapId;
	}

	public void SetRoomPosition(int i)
	{
		if (mRooms[i] != null)
		{
			mRooms[i].SetParentTransform(mMapObject.transform);
			mRooms[i].SetLocalPosition(new Vector3((float)mRooms[i].MinX * 5f, 0f, (float)mRooms[i].MinY * 5f));
		}
	}

	public void SetEnterDirection(Direction direction)
	{
		mEnterDirection = direction;
		mEnterRoom = mBorderRooms[(int)direction];
	}

	public Direction GetEnterDirection()
	{
		return mEnterDirection;
	}

	public void SetExitDirection(Direction direction)
	{
		mExitDirection = direction;
		mExitRoom = mBorderRooms[(int)direction];
	}

	public Direction GetExitDirection()
	{
		return mExitDirection;
	}

	public bool Intersect(DungeonMap otherMap)
	{
		return Mathf.Min(StartX + SizeX - 1, otherMap.StartX + otherMap.SizeX - 1) > Mathf.Max(StartX, otherMap.StartX) && Mathf.Min(StartY + SizeY - 1, otherMap.StartY + otherMap.SizeY - 1) > Mathf.Max(StartY, otherMap.StartY);
	}
}
