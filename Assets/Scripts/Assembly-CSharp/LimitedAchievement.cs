public class LimitedAchievement : Achievement
{
	private int[] currNum;

	private bool start;

	public LimitedAchievement(int id, string nameEN, string infoEN, byte icon, string reward, string targetNum, string additionalPart, bool active)
		: base(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active)
	{
		currNum = new int[mTargetNum.Length];
		start = false;
	}

	protected override void OnDataChange(int data, int additionalPart)
	{
		if (!start)
		{
			return;
		}
		int num = mTargetNum.Length;
		for (int i = 0; i < num; i++)
		{
			if (mAdditionalPart[i] == 0 || mAdditionalPart[i] == additionalPart)
			{
				currNum[i] += data;
			}
		}
		CheckResult();
	}

	private void CheckResult()
	{
		int num = mTargetNum.Length;
		bool flag = true;
		for (int i = 0; i < num; i++)
		{
			if (currNum[i] < mTargetNum[i])
			{
				flag = false;
			}
		}
		if (flag)
		{
			DoFinish();
		}
	}

	protected override void OnStart()
	{
		if (!start)
		{
			start = true;
			for (int i = 0; i < currNum.Length; i++)
			{
				currNum[i] = 0;
			}
		}
	}

	protected override void OnStop()
	{
		if (start)
		{
			start = false;
			CheckResult();
		}
	}

	protected override void OnReset()
	{
		start = false;
		for (int i = 0; i < currNum.Length; i++)
		{
			currNum[i] = 0;
		}
	}

	protected override float GetAchievementPercent()
	{
		return base.Finish ? 100 : 0;
	}
}
