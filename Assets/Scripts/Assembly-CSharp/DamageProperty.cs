using UnityEngine;

public class DamageProperty
{
	public enum AttackerType
	{
		_PlayerOrEnemy = 0,
		_EngineerGun = 1,
		_HealingStation = 2,
		_Traps = 3,
		_Dot = 4
	}

	public Vector3 hitForce;

	public int damage;

	public WeaponType wType;

	public Vector3 hitpoint;

	public bool criticalAttack;

	public bool isLocal;

	public bool isPenetration;

	public ElementType elementType;

	public bool isTriggerDlementDot;

	public float elementDotDamage;

	public int elementDotTime;

	public int unitLevel;

	public int weaponLevel;

	public AttackerType attackerType;
}
