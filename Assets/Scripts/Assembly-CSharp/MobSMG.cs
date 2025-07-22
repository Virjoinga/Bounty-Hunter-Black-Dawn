using UnityEngine;

public class MobSMG : EnemyHuman
{
	public override void Init()
	{
		base.Init();
		if (mEnemyType == EnemyType.ELITE_MOB_REVOLVER)
		{
			mWeaponPrefabPath = "WeaponL/SMG02_l";
		}
		else
		{
			mWeaponPrefabPath = "WeaponL/SMG01_l";
		}
		mLeanRightPosition = new Vector3(0.52f, 1.5f, 0f);
		mLeanLeftPosition = new Vector3(-0.52f, 1.5f, 0f);
		mHitCheckHeight = 1.6f;
		mFireAnimationLength = 4f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.MobMesh;
	}

	protected override void PlayShootingSound()
	{
		PlaySound("RPG_Audio/Weapon/smg/SMG01_fire");
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/mob_attacked");
	}
}
