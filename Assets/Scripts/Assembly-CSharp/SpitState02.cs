public class SpitState02 : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Spit spit = enemy as Spit;
		if (spit != null)
		{
			spit.DoSpit02();
		}
	}
}
