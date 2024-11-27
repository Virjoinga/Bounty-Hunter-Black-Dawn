public class ControllableUnitIdleState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		if (summoned.Hp <= 0)
		{
			summoned.StartDead();
		}
		else
		{
			summoned.DoIdle();
		}
	}
}
