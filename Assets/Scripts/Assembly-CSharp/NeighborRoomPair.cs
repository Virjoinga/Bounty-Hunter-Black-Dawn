internal class NeighborRoomPair
{
	private DungeonRoom mFirstRoom;

	private DungeonRoom mSecondRoom;

	private int mStart;

	private int mLength;

	public NeighborRoomPair(DungeonRoom firstRoom, DungeonRoom secondRoom, int start, int length)
	{
		mFirstRoom = firstRoom;
		mSecondRoom = secondRoom;
		mStart = start;
		mLength = length;
	}

	public void CreateVerticalPath()
	{
		int num = mStart - mFirstRoom.MinX;
		if (num == 0)
		{
			if (mFirstRoom[num, mFirstRoom.LengthY - 1] == DungeonBlockType.WALL_NORTH)
			{
				mFirstRoom[num, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_SOUTHEAST;
			}
			else
			{
				mFirstRoom[num, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_WEST;
			}
		}
		else
		{
			mFirstRoom[num, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_SOUTHEAST;
		}
		if (num + mLength == mFirstRoom.LengthX)
		{
			if (mFirstRoom[num + mLength - 1, mFirstRoom.LengthY - 1] == DungeonBlockType.WALL_NORTH)
			{
				mFirstRoom[num + mLength - 1, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_SOUTHWEST;
			}
			else
			{
				mFirstRoom[num + mLength - 1, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_EAST;
			}
		}
		else
		{
			mFirstRoom[num + mLength - 1, mFirstRoom.LengthY - 1] = DungeonBlockType.WALL_SOUTHWEST;
		}
		for (int i = num + 1; i < num + mLength - 1; i++)
		{
			mFirstRoom[i, mFirstRoom.LengthY - 1] = DungeonBlockType.FLOOR;
		}
		num = mStart - mSecondRoom.MinX;
		if (num == 0)
		{
			if (mSecondRoom[num, 0] == DungeonBlockType.WALL_SOUTH)
			{
				mSecondRoom[num, 0] = DungeonBlockType.WALL_NORTHEAST;
			}
			else
			{
				mSecondRoom[num, 0] = DungeonBlockType.WALL_WEST;
			}
		}
		else
		{
			mSecondRoom[num, 0] = DungeonBlockType.WALL_NORTHEAST;
		}
		if (num + mLength == mSecondRoom.LengthX)
		{
			if (mSecondRoom[num + mLength - 1, 0] == DungeonBlockType.WALL_SOUTH)
			{
				mSecondRoom[num + mLength - 1, 0] = DungeonBlockType.WALL_NORTHWEST;
			}
			else
			{
				mSecondRoom[num + mLength - 1, 0] = DungeonBlockType.WALL_EAST;
			}
		}
		else
		{
			mSecondRoom[num + mLength - 1, 0] = DungeonBlockType.WALL_NORTHWEST;
		}
		for (int j = num + 1; j < num + mLength - 1; j++)
		{
			mSecondRoom[j, 0] = DungeonBlockType.FLOOR;
		}
	}

	public void CreateHorizontalPath()
	{
		int num = mStart - mFirstRoom.MinY;
		if (num == 0)
		{
			if (mFirstRoom[mFirstRoom.LengthX - 1, num] == DungeonBlockType.WALL_EAST)
			{
				mFirstRoom[mFirstRoom.LengthX - 1, num] = DungeonBlockType.WALL_NORTHWEST;
			}
			else
			{
				mFirstRoom[mFirstRoom.LengthX - 1, num] = DungeonBlockType.WALL_SOUTH;
			}
		}
		else
		{
			mFirstRoom[mFirstRoom.LengthX - 1, num] = DungeonBlockType.WALL_NORTHWEST;
		}
		if (num + mLength == mFirstRoom.LengthY)
		{
			if (mFirstRoom[mFirstRoom.LengthX - 1, num + mLength - 1] == DungeonBlockType.WALL_EAST)
			{
				mFirstRoom[mFirstRoom.LengthX - 1, num + mLength - 1] = DungeonBlockType.WALL_SOUTHWEST;
			}
			else
			{
				mFirstRoom[mFirstRoom.LengthX - 1, num + mLength - 1] = DungeonBlockType.WALL_NORTH;
			}
		}
		else
		{
			mFirstRoom[mFirstRoom.LengthX - 1, num + mLength - 1] = DungeonBlockType.WALL_SOUTHWEST;
		}
		for (int i = num + 1; i < num + mLength - 1; i++)
		{
			mFirstRoom[mFirstRoom.LengthX - 1, i] = DungeonBlockType.FLOOR;
		}
		num = mStart - mSecondRoom.MinY;
		if (num == 0)
		{
			if (mSecondRoom[0, num] == DungeonBlockType.WALL_WEST)
			{
				mSecondRoom[0, num] = DungeonBlockType.WALL_NORTHEAST;
			}
			else
			{
				mSecondRoom[0, num] = DungeonBlockType.WALL_SOUTH;
			}
		}
		else
		{
			mSecondRoom[0, num] = DungeonBlockType.WALL_NORTHEAST;
		}
		if (num + mLength == mSecondRoom.LengthY)
		{
			if (mSecondRoom[0, num + mLength - 1] == DungeonBlockType.WALL_WEST)
			{
				mSecondRoom[0, num + mLength - 1] = DungeonBlockType.WALL_SOUTHEAST;
			}
			else
			{
				mSecondRoom[0, num + mLength - 1] = DungeonBlockType.WALL_NORTH;
			}
		}
		else
		{
			mSecondRoom[0, num + mLength - 1] = DungeonBlockType.WALL_SOUTHEAST;
		}
		for (int j = num + 1; j < num + mLength - 1; j++)
		{
			mSecondRoom[0, j] = DungeonBlockType.FLOOR;
		}
	}
}
