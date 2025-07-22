public class UnitDetailedProperties
{
	public float DamageBonusAll;

	public float[] DamageBonus = new float[8];

	public float AccuracyBonusAll;

	public float[] AccuracyBonus = new float[8];

	public float AttackIntervalPercentageBonusAll;

	public float[] AttackIntervalPercentageBonus = new float[8];

	public float ReloadTimeBonusAll;

	public float[] ReloadTimeBonus = new float[8];

	public float MagsBonusAll;

	public float[] MagsBonus = new float[8];

	public int MagsBonusValueAll;

	public int[] MagsBonusValue = new int[8];

	public float RecoilBonusAll;

	public float[] RecoilBonus = new float[8];

	public int AmmoRecoverBonusAll;

	public int[] AmmoRecoverBonus = new int[8];

	public float CriticalRateBonusAll;

	public float[] CriticalRateBonus = new float[8];

	public float CriticalDamageBonusAll;

	public float[] CriticalDamageBonus = new float[8];

	public float MeleeDamageBonus;

	public int MaxShieldBonus;

	public float MaxShieldPercentageBonus;

	public int ShieldRecoveryBonus;

	public float ShieldRecoveryPercentageBonus;

	public int[] ElementsResistanceBonus = new int[3];

	public int HpBonus;

	public float HpPercentageBonus;

	public float HpRecoveryBonus;

	public float HpRecoveryPercentageBonus;

	public float ExplosionRangeBonus;

	public float SpeedBonus;

	public int DamageReductionBonus;

	public int DamageToHealthBonus;

	public int ExplosionDamageReductionBonus;

	public float DamageImmunityRateBonus;

	public float DropRateBonus;

	public float ShieldRecoverDelayBonus;

	public float ShieldRecoverDelayBonusPercentage;

	public float DeathTimeBonus;

	public float RebirthHealthBonus;

	public float TherapeuticGainBonus;

	public float CapacitToHealthBonus;

	public bool PenetrationBonusAll;

	public bool[] PenetrationBonus = new bool[8];

	public int ShotgunBulletBonus;

	public float NotUseBulletRateBonus;

	public void Reset()
	{
		DamageBonusAll = 0f;
		AccuracyBonusAll = 0f;
		AttackIntervalPercentageBonusAll = 0f;
		ReloadTimeBonusAll = 0f;
		MagsBonusAll = 0f;
		MagsBonusValueAll = 0;
		RecoilBonusAll = 0f;
		AmmoRecoverBonusAll = 0;
		CriticalRateBonusAll = 0f;
		CriticalDamageBonusAll = 0f;
		PenetrationBonusAll = false;
		for (int i = 0; i < 8; i++)
		{
			DamageBonus[i] = 0f;
			AccuracyBonus[i] = 0f;
			AttackIntervalPercentageBonus[i] = 0f;
			ReloadTimeBonus[i] = 0f;
			MagsBonus[i] = 0f;
			MagsBonusValue[i] = 0;
			RecoilBonus[i] = 0f;
			AmmoRecoverBonus[i] = 0;
			CriticalRateBonus[i] = 0f;
			CriticalDamageBonus[i] = 0f;
			PenetrationBonus[i] = false;
		}
		MeleeDamageBonus = 0f;
		MaxShieldBonus = 0;
		MaxShieldPercentageBonus = 0f;
		ShieldRecoveryBonus = 0;
		ShieldRecoveryPercentageBonus = 0f;
		ElementsResistanceBonus[0] = 0;
		ElementsResistanceBonus[1] = 0;
		ElementsResistanceBonus[2] = 0;
		HpBonus = 0;
		HpPercentageBonus = 0f;
		HpRecoveryBonus = 0f;
		HpRecoveryPercentageBonus = 0f;
		ExplosionRangeBonus = 0f;
		SpeedBonus = 0f;
		DamageReductionBonus = 0;
		DamageToHealthBonus = 0;
		ExplosionDamageReductionBonus = 0;
		DamageImmunityRateBonus = 0f;
		DropRateBonus = 0f;
		ShotgunBulletBonus = 0;
		NotUseBulletRateBonus = 0f;
		ShieldRecoverDelayBonus = 0f;
		ShieldRecoverDelayBonusPercentage = 0f;
		DeathTimeBonus = 0f;
		RebirthHealthBonus = 0f;
		CapacitToHealthBonus = 0f;
	}
}
