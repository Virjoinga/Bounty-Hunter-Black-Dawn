public class ControllableUnitDisappearState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		summoned.DoDisappear();
	}
}
