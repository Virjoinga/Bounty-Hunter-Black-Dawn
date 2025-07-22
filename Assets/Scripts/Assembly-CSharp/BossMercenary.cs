public class BossMercenary : MercenaryBoss
{
	public override bool IsBoss()
	{
		return false;
	}

	public override bool IsBossRush()
	{
		return true;
	}

	public override bool NeedEliteImg()
	{
		return false;
	}
}
