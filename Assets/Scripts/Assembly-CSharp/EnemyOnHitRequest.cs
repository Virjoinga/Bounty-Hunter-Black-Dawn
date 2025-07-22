public class EnemyOnHitRequest : Request
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected int mDamage;

	protected bool mCriticalAttack;

	protected byte mElementType;

	protected bool mIsPenetration;

	protected byte mWeaponType;

	protected byte mAttackType;

	public EnemyOnHitRequest(byte pointID, byte enemyID, int damage, bool criticalAttack, byte elementType, bool isPenetration, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		requestID = 108;
		mPointID = pointID;
		mEnemyID = enemyID;
		mDamage = damage;
		mCriticalAttack = criticalAttack;
		mElementType = elementType;
		mIsPenetration = isPenetration;
		mWeaponType = (byte)weaponType;
		mAttackType = (byte)attackerType;
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(11);
		bytesBuffer.AddByte(mPointID);
		bytesBuffer.AddByte(mEnemyID);
		bytesBuffer.AddInt(mDamage);
		bytesBuffer.AddBool(mCriticalAttack);
		bytesBuffer.AddByte(mElementType);
		bytesBuffer.AddBool(mIsPenetration);
		bytesBuffer.AddByte(mWeaponType);
		bytesBuffer.AddByte(mAttackType);
		return bytesBuffer.GetBytes();
	}
}
