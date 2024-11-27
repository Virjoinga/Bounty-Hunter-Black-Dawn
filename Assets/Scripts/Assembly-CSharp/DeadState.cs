public class DeadState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		enemy.DoDead();
	}
}
