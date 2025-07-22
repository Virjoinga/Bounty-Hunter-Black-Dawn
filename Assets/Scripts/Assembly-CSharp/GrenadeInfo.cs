public class GrenadeInfo
{
	public string Name;

	public int Damage;

	public int DamageInit;

	public float ExplosionRange;

	public float ExplosionRangeInit;

	public ElementType elementType;

	public byte elementPara = 1;

	public float elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[7];

	public float elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[7];

	public byte Level;

	public float CriticalRate;

	public float CriticalRateInit;

	public float CriticalDamage = 1.5f;

	public float CriticalDamageInit = 1.5f;
}
