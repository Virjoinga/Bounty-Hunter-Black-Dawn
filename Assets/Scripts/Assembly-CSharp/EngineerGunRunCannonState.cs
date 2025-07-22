public class EngineerGunRunCannonState : ControllableUnitState
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
			engineerGun.DoEngineerGunRunCannon();
		}
	}
}
