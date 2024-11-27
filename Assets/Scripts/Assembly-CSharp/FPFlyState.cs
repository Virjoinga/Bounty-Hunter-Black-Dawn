public class FPFlyState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		FloatProtector floatProtector = enemy as FloatProtector;
		if (floatProtector != null)
		{
			floatProtector.DoFPFlyAround();
		}
	}
}
