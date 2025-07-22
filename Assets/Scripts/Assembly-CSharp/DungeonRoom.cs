using UnityEngine;

public class DungeonRoom
{
	public enum DungeonCellType
	{
		EMPTY = 0,
		OUTER_WALL = 1,
		DOOR = 2,
		FLOOR = 3,
		WALL_NORTH = 4,
		WALL_NORTHEAST = 5,
		WALL_EAST = 6,
		WALL_SOUTHEAST = 7,
		WALL_SOUTH = 8,
		WALL_SOUTHWEST = 9,
		WALL_WEST = 10,
		WALL_NORTHWEST = 11
	}

	private int mRoomId;

	private int mBlockNumX;

	private int mBlockNumY;

	private byte[,] mBlocks;

	private GameObject mRoomObject;

	public byte this[int x, int y]
	{
		get
		{
			return mBlocks[x, y];
		}
		set
		{
			mBlocks[x, y] = value;
		}
	}

	public int LengthX
	{
		get
		{
			return mBlockNumX;
		}
	}

	public int LengthY
	{
		get
		{
			return mBlockNumY;
		}
	}

	public int MinX { get; set; }

	public int MaxX { get; set; }

	public int MinY { get; set; }

	public int MaxY { get; set; }

	public DungeonRoom(int id, int x, int y)
	{
		mRoomId = id;
		mBlockNumX = x;
		mBlockNumY = y;
	}

	public bool Intersect(DungeonRoom otherRoom)
	{
		return IntersectX(otherRoom, 0) && IntersectY(otherRoom, 0);
	}

	public bool IntersectX(DungeonRoom otherRoom, int length)
	{
		return Mathf.Max(MaxX - otherRoom.MinX, otherRoom.MaxX - MinX) + 1 < LengthX + otherRoom.LengthX - length;
	}

	public bool IntersectY(DungeonRoom otherRoom, int length)
	{
		return Mathf.Max(MaxY - otherRoom.MinY, otherRoom.MaxY - MinY) + 1 < LengthY + otherRoom.LengthY - length;
	}

	public void Create()
	{
		mBlocks = new byte[mBlockNumX, mBlockNumY];
		mBlocks[0, 0] = DungeonBlockType.WALL_SOUTHWEST;
		mBlocks[0, mBlockNumY - 1] = DungeonBlockType.WALL_NORTHWEST;
		mBlocks[mBlockNumX - 1, 0] = DungeonBlockType.WALL_SOUTHEAST;
		mBlocks[mBlockNumX - 1, mBlockNumY - 1] = DungeonBlockType.WALL_NORTHEAST;
		for (int i = 1; i < mBlockNumX - 1; i++)
		{
			for (int j = 1; j < mBlockNumY - 1; j++)
			{
				mBlocks[i, j] = DungeonBlockType.FLOOR;
			}
			mBlocks[i, 0] = DungeonBlockType.WALL_SOUTH;
			mBlocks[i, mBlockNumY - 1] = DungeonBlockType.WALL_NORTH;
		}
		for (int k = 1; k < mBlockNumY - 1; k++)
		{
			mBlocks[0, k] = DungeonBlockType.WALL_WEST;
			mBlocks[mBlockNumX - 1, k] = DungeonBlockType.WALL_EAST;
		}
	}

	public void SetParentTransform(Transform parent)
	{
		mRoomObject = new GameObject("DungeonRoom" + mRoomId);
		mRoomObject.transform.parent = parent;
	}

	public void SetLocalPosition(Vector3 pos)
	{
		mRoomObject.transform.localPosition = pos;
	}

	public void Load()
	{
		for (int i = 0; i < mBlockNumX; i++)
		{
			for (int j = 0; j < mBlockNumY; j++)
			{
				LoadBlock(i, j);
			}
		}
	}

	public void LoadBlock(int x, int y)
	{
		GameObject gameObject = Object.Instantiate(DungeonResource.GetInstance().GetPrefab(mBlocks[x, y])) as GameObject;
		gameObject.name = "DungeonBlock_" + x + "_" + y;
		gameObject.transform.parent = mRoomObject.transform;
		gameObject.transform.localPosition = new Vector3(2.5f + 5f * (float)x, 0f, 2.5f + 5f * (float)y);
	}
}
