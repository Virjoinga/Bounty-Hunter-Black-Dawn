using UnityEngine;

public class MercenaryShotgun : EnemyHuman
{
	protected bool mNeedSparkEffect;

	public override void Init()
	{
		base.Init();
		if (mEnemyType == EnemyType.ELITE_MERCENARY_SHOTGUN)
		{
			mWeaponPrefabPath = "WeaponL/shotgun02_l";
		}
		else
		{
			mWeaponPrefabPath = "WeaponL/shotgun01_l";
		}
		mLeanRightPosition = new Vector3(0.52f, 1.5f, 0f);
		mLeanLeftPosition = new Vector3(-0.52f, 1.5f, 0f);
		mHitCheckHeight = 1.6f;
		mFireAnimationLength = 24f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.MercenaryMesh;
	}

	protected override void Shoot(string shootAnimation)
	{
		if (!mIsShoot)
		{
			mNeedSparkEffect = true;
			for (int i = 0; i < 4; i++)
			{
				CheckShoot();
				mNeedSparkEffect = false;
			}
			mIsShoot = true;
		}
		if (mIsShoot && animation[shootAnimation].time > animation[shootAnimation].clip.length)
		{
			animation[shootAnimation].time -= animation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
		}
	}

	protected override void CreateWallSpark()
	{
		if (mNeedSparkEffect)
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_WALL_SPARK).CreateObject(mRaycastHit.point, Vector3.zero, Quaternion.identity);
		}
	}

	protected override void CreateFloorSpark()
	{
		if (mNeedSparkEffect)
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_FLOOR_SPARK).CreateObject(mRaycastHit.point, mRaycastHit.normal, Quaternion.identity);
		}
	}

	protected override void CreateGunFire(Vector3 direction)
	{
		if (mNeedSparkEffect)
		{
			gameScene.GetEffectPool(EffectPoolType.SHOTGUN_SPARK_T).CreateObject(mWeaponFireTransform.position, direction, Quaternion.identity);
		}
	}

	protected override void CreateBulletTrail(bool hitCollider, bool hitLocalPlayer, Vector3 direction)
	{
	}

	protected override void PlayShootingSound()
	{
		PlaySound("RPG_Audio/Weapon/shotgun/shotgun_fire");
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/mercenary_attacked");
	}
}
