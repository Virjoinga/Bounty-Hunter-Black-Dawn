public class ControllableUnitInitState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		if (summoned.Hp <= 0)
		{
			summoned.StartDead();
		}
		else
		{
			summoned.DoInit();
		}
	}
}
