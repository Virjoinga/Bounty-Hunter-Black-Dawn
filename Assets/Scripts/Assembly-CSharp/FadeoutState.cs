public class FadeoutState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		enemy.DoFadeout();
	}
}
