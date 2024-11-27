public class BossTyler : Ghost
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
		return true;
	}
}
