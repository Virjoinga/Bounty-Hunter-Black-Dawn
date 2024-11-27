public class EnemyIdleState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
		}
		else
		{
			enemy.DoEnemyIdle();
		}
	}
}
