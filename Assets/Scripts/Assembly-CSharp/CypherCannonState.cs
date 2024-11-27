public class CypherCannonState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Cypher cypher = enemy as Cypher;
		if (cypher != null)
		{
			cypher.DoCypherCannon();
		}
	}
}
