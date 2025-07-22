public class ShellSpinStartState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime)
	{
		if (enemy.Hp <= 0)
		{
			enemy.StartDead();
			return;
		}
		Shell shell = enemy as Shell;
		if (shell != null)
		{
			shell.DoShellSpinStart();
		}
	}
}
