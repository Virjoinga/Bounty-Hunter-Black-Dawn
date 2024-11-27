using UnityEngine;

public class MobRevolver : EnemyHuman
{
	public override void Init()
	{
		base.Init();
		if (mEnemyType == EnemyType.ELITE_MOB_REVOLVER)
		{
			mWeaponPrefabPath = "WeaponL/revolver02_l";
		}
		else
		{
			mWeaponPrefabPath = "WeaponL/revolver01_l";
		}
		mLeanRightPosition = new Vector3(0.52f, 1.5f, 0f);
		mLeanLeftPosition = new Vector3(-0.52f, 1.5f, 0f);
		mHitCheckHeight = 1.6f;
		mFireAnimationLength = 14f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.MobMesh;
	}

	protected override void PlayShootingSound()
	{
		PlaySound("RPG_Audio/Weapon/revolver/revolver_fire");
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/mob_attacked");
	}
}
