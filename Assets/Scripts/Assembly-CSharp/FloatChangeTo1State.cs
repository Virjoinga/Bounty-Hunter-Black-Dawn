public class FloatChangeTo1State : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		FloatCore floatCore = enemy as FloatCore;
		if (floatCore != null)
		{
			floatCore.DoFloatChangeTo1();
		}
	}
}
