public class FPReadyChangeState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		FloatProtector floatProtector = enemy as FloatProtector;
		if (floatProtector != null)
		{
			floatProtector.DoFPReadyChange();
		}
	}
}
