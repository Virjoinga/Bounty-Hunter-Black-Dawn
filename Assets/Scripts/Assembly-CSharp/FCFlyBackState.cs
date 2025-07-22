public class FCFlyBackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		FloatControler floatControler = enemy as FloatControler;
		if (floatControler != null)
		{
			floatControler.DoFCFlyBack();
		}
	}
}
