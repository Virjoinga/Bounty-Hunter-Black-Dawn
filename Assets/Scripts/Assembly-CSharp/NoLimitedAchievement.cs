public class NoLimitedAchievement : Achievement
{
	public NoLimitedAchievement(int id, string nameEN, string infoEN, byte icon, string reward, string targetNum, string additionalPart, bool active)
		: base(id, nameEN, infoEN, icon, reward, targetNum, additionalPart, active)
	{
	}

	protected override void OnDataChange(int data, int additionalPart)
	{
		DoFinish();
	}

	protected override float GetAchievementPercent()
	{
		return base.Finish ? 100 : 0;
	}
}
