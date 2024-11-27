public class TerminatorElevateState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Terminator terminator = enemy as Terminator;
		if (terminator != null)
		{
			terminator.DoTerminatorElevate();
		}
	}
}
