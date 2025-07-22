internal class EnemyOnHitResponse : Response
{
	protected byte mPointID;

	protected byte mEnemyID;

	protected int mDamage;

	protected int mHp;

	protected int mShield;

	protected int mKillerID;

	protected bool mCriticalAttack;

	protected byte mElementType;

	protected byte mWeaponType;

	protected byte mAttackerType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mPointID = bytesBuffer.ReadByte();
		mEnemyID = bytesBuffer.ReadByte();
		mDamage = bytesBuffer.ReadInt();
		mHp = bytesBuffer.ReadInt();
		mShield = bytesBuffer.ReadInt();
		mKillerID = bytesBuffer.ReadInt();
		mCriticalAttack = bytesBuffer.ReadBool();
		mElementType = bytesBuffer.ReadByte();
		mWeaponType = bytesBuffer.ReadByte();
		mAttackerType = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(mPointID, mEnemyID);
		if (enemy != null && enemy.InPlayingState())
		{
			enemy.OnHitResponse(mKillerID, mDamage, mShield, mHp, mCriticalAttack, mElementType, (WeaponType)mWeaponType, (DamageProperty.AttackerType)mAttackerType);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
	}
}
