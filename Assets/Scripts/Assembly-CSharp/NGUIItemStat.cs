public class NGUIItemStat
{
	public enum StatType
	{
		Damage = 0,
		Accurancy = 1,
		AttackInterval = 2,
		ReloadTime = 3,
		Mags = 4,
		Recoil = 5,
		MeleeDamage = 6,
		CriticalRate = 7,
		CriticalDamage = 8,
		HasScope = 9,
		ScopeRate = 10,
		ExplosionRange = 11,
		ElementType = 12,
		ElementPara = 13,
		ExplosionFadeRange = 14,
		ZoomRate = 15,
		FadeRange = 16,
		ShieldCapacity = 17,
		RecoverySpeed = 18,
		RecoveryDelay = 19,
		ElementResistance = 20,
		HPRecovery = 21,
		Superposition = 22,
		Other = 23
	}

	public StatType statType;

	public float statValue;

	public static string GetName(StatType i)
	{
		return i.ToString();
	}
}
