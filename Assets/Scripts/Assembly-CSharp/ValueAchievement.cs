public class ValueAchievement : Achievement
{
	public ValueAchievement(int id, string nameEN, string infoEN, byte icon, string reward, string targetNum, string additionalPart, bool active)
		: base(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active)
	{
	}

	protected override void OnDataChange(int data, int additionalPart)
	{
		bool flag = true;
		int num = mTargetNum.Length;
		for (int i = 0; i < num; i++)
		{
			if (mAdditionalPart[i] == 0 || mAdditionalPart[i] == additionalPart)
			{
				if (data < mTargetNum[i])
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			DoFinish();
		}
	}

	protected override float GetAchievementPercent()
	{
		return base.Finish ? 100 : 0;
	}
}
