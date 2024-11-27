public class FCLaserLockRageState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		FloatControler floatControler = enemy as FloatControler;
		if (floatControler != null)
		{
			floatControler.DoFCLaserLockRage();
		}
	}
}
