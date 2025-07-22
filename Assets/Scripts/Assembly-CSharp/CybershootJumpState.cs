public class CybershootJumpState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Cybershoot cybershoot = enemy as Cybershoot;
		if (cybershoot != null)
		{
			cybershoot.DoCybershootJump();
		}
	}
}
