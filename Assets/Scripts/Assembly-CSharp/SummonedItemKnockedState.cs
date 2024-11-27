public class SummonedItemKnockedState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		if (summoned.Hp <= 0)
		{
			summoned.StartDead();
			return;
		}
		SummonedItem summonedItem = summoned as SummonedItem;
		if (summonedItem != null)
		{
			summonedItem.DoKnocked();
		}
	}
}
