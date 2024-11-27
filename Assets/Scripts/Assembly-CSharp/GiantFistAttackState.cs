public class GiantFistAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Giant giant = enemy as Giant;
		if (giant != null)
		{
			giant.DoGiantFistAttack();
		}
	}
}
