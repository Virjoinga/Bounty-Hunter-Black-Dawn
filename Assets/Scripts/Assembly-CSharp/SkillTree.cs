public class SkillTree
{
	protected const int LAYER = 4;

	protected const int COLS = 2;

	protected const int POINT_NEEDED_UNLOCK_NEXT_LAYER = 5;

	protected const int MAX_SKILL_LEVEL = 5;

	protected const int FULL_UPGRADE_POINT = 31;

	protected int[,] skillLevel = new int[4, 2];

	protected int TreeID = -1;

	public void Init(int treeID)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				skillLevel[i, j] = 0;
			}
		}
		TreeID = treeID;
	}

	public int GetLayerPoints(int layer)
	{
		int num = 0;
		for (int i = 0; i < 2; i++)
		{
			num += skillLevel[layer, i];
		}
		return num;
	}

	public int GetTotalPoints()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				num += skillLevel[i, j];
			}
		}
		return num;
	}

	public bool GivePoint(int layer, int col)
	{
		if (layer == 0 || GetTotalPoints() >= 5 * layer)
		{
			if (layer != 3)
			{
				if (skillLevel[layer, col] < 5)
				{
					skillLevel[layer, col]++;
					return true;
				}
			}
			else if (skillLevel[layer, col] < 1)
			{
				skillLevel[layer, col]++;
				return true;
			}
		}
		return false;
	}

	public int GetSkillLevel(int layer, int col)
	{
		return skillLevel[layer, col];
	}

	public void SetSkillLevel(int layer, int col, int level)
	{
		skillLevel[layer, col] = level;
	}

	public bool CanGivePoint(int layer, int col, int givePoint)
	{
		return false;
	}

	public bool IsFullyUpgraded()
	{
		return GetTotalPoints() == 31;
	}
}
