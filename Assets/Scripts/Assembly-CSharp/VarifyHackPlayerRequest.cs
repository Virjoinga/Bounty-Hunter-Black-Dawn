using UnityEngine;

public class VarifyHackPlayerRequest : Request
{
	protected int Analysis_ID;

	protected byte Level;

	protected int DPS;

	protected short FireRate;

	protected int CriticalDamage;

	protected int MeleeDamage;

	protected short DamageImmunityRate;

	protected short DamageReduction;

	protected short FireResistance;

	protected short ShockResistance;

	protected short CorrosiveResistance;

	protected short ExplosionResistance;

	protected int MaxHP;

	protected int MaxSheild;

	protected short ShieldRecoveryDelay;

	protected int ShieldRecovery;

	protected int MaxUsedBulletWithoutReload;

	public VarifyHackPlayerRequest(LocalPlayer player)
	{
		if (player != null)
		{
			requestID = 304;
			Analysis_ID = player.GetUserState().OperInfo.mIndex;
			Level = (byte)player.GetUserState().GetCharLevel();
			DPS = (int)player.GetWeapon().Damage;
			Debug.Log("DPS: " + DPS);
			FireRate = (short)player.GetWeapon().AttackFrequency;
			CriticalDamage = Mathf.CeilToInt(player.GetWeapon().CriticalDamage * 100f);
			MeleeDamage = player.MeleeATK;
			DamageImmunityRate = (short)Mathf.CeilToInt(player.DamageImmunityRateBonus * 100f);
			DamageReduction = (short)player.DamageReduction;
			FireResistance = (short)player.GetElementResistance(ElementType.Fire);
			ShockResistance = (short)player.GetElementResistance(ElementType.Shock);
			CorrosiveResistance = (short)player.GetElementResistance(ElementType.Corrosive);
			ExplosionResistance = (short)player.ExplosionDamageReduction;
			MaxHP = player.MaxHp;
			MaxSheild = player.MaxShield;
			ShieldRecoveryDelay = (short)(player.ShieldRecoveryDelay * 10f);
			ShieldRecovery = player.ShieldRecovery;
			MaxUsedBulletWithoutReload = player.MaxUsedBulletWithoutReload;
			Debug.Log("MaxUsedBulletWithoutReload = " + MaxUsedBulletWithoutReload);
		}
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(49);
		bytesBuffer.AddInt(Analysis_ID);
		bytesBuffer.AddByte(Level);
		bytesBuffer.AddInt(DPS);
		bytesBuffer.AddShort(FireRate);
		bytesBuffer.AddInt(CriticalDamage);
		bytesBuffer.AddInt(MeleeDamage);
		bytesBuffer.AddShort(DamageImmunityRate);
		bytesBuffer.AddShort(DamageReduction);
		bytesBuffer.AddShort(FireResistance);
		bytesBuffer.AddShort(ShockResistance);
		bytesBuffer.AddShort(CorrosiveResistance);
		bytesBuffer.AddShort(ExplosionResistance);
		bytesBuffer.AddInt(MaxHP);
		bytesBuffer.AddInt(MaxSheild);
		bytesBuffer.AddShort(ShieldRecoveryDelay);
		bytesBuffer.AddInt(ShieldRecovery);
		bytesBuffer.AddInt(MaxUsedBulletWithoutReload);
		return bytesBuffer.GetBytes();
	}
}
