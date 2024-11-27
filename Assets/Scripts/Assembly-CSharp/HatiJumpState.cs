public class HatiJumpState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Hati hati = enemy as Hati;
		if (hati != null)
		{
			hati.DoHatiJump();
		}
	}
}
