public class WormDrillOutState01 : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Worm worm = enemy as Worm;
		if (worm != null)
		{
			worm.DoDrillOut01();
		}
	}
}
