public class FloatCautionEndState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		FloatCore floatCore = enemy as FloatCore;
		if (floatCore != null)
		{
			floatCore.DoFloatCautionEnd();
		}
	}
}
