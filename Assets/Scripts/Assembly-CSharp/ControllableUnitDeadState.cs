public class ControllableUnitDeadState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		summoned.DoDead();
	}
}
