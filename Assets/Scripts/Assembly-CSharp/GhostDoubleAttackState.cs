public class GhostDoubleAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Ghost ghost = enemy as Ghost;
		if (ghost != null)
		{
			ghost.DoGhostDoubleAttack();
		}
	}
}
