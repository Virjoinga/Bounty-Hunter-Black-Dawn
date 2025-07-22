public class WormDrillIdleState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		Worm worm = enemy as Worm;
		if (worm != null)
		{
			worm.DoDrillIdle();
			worm.LookAtTargetHorizontal();
		}
	}
}
