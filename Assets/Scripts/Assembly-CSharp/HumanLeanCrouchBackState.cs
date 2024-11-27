public class HumanLeanCrouchBackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		EnemyHuman enemyHuman = enemy as EnemyHuman;
		if (enemyHuman != null)
		{
			enemyHuman.DoHumanLeanCrouchBack();
		}
	}
}
