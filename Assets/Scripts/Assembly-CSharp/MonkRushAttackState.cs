public class MonkRushAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Monk monk = enemy as Monk;
		if (monk != null)
		{
			monk.DoRushAttack();
		}
	}
}