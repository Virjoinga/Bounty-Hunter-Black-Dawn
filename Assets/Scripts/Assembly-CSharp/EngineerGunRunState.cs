public class EngineerGunRunState : ControllableUnitState
{
	public override void NextState(ControllableUnit summoned)
	{
		if (summoned.Hp <= 0)
		{
			summoned.StartDead();
			return;
		}
		EngineerGun engineerGun = summoned as EngineerGun;
		if (engineerGun != null)
		{
			engineerGun.DoEngineerGunRun();
		}
	}
}
