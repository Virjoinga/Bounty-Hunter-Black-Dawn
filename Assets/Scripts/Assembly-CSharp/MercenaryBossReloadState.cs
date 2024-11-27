public class MercenaryBossReloadState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		MercenaryBoss mercenaryBoss = enemy as MercenaryBoss;
		if (mercenaryBoss != null)
		{
			mercenaryBoss.DoMercenaryBossReload();
		}
	}
}
