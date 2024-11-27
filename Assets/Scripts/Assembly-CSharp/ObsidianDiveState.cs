public class ObsidianDiveState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Obsidian obsidian = enemy as Obsidian;
		if (obsidian != null)
		{
			obsidian.DoObsidianDive();
		}
	}
}
