using UnityEngine;

public class MercenaryAssaultRifle : EnemyHuman
{
	public override void Init()
	{
		base.Init();
		if (mEnemyType == EnemyType.ELITE_MERCENARY_ASSAULT_RIFLE)
		{
			mWeaponPrefabPath = "WeaponL/assault03_l";
		}
		else
		{
			mWeaponPrefabPath = "WeaponL/assault01_l";
		}
		mLeanRightPosition = new Vector3(0.52f, 1.5f, 0f);
		mLeanLeftPosition = new Vector3(-0.52f, 1.5f, 0f);
		mHitCheckHeight = 1.6f;
		mFireAnimationLength = 4f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.MercenaryMesh;
	}

	protected override void PlayShootingSound()
	{
		PlaySound("RPG_Audio/Weapon/assault_rifle/assault_fire");
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/mercenary_attacked");
	}
}
