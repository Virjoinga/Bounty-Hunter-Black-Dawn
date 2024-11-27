public class BossInitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		EnemyBoss enemyBoss = enemy as EnemyBoss;
		if (enemyBoss != null)
		{
			enemyBoss.DoBossInit();
		}
	}
}
