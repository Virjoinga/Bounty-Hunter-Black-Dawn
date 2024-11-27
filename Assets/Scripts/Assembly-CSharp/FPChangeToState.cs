public class FPChangeToState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		FloatProtector floatProtector = enemy as FloatProtector;
		if (floatProtector != null)
		{
			floatProtector.DoFPChangeTo();
		}
	}
}
