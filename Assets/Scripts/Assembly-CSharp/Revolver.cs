using UnityEngine;

public class Revolver : Weapon
{
	public override WeaponType GetWeaponType()
	{
		return WeaponType.Revolver;
	}

	public override string GetAnimationSuffix()
	{
		return "revolver01_";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "revolver01_";
	}

	public override float GetReloadAnimationSpeed()
	{
		return 2.5f * base.ReloadTimeScale / reloadTime;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		base.AttackFrequencyT = 0.25f;
		SetWeaponShootSpeed(0.5f);
		playSoundTimer.SetTimer(attackFrenquency / 2f, true);
		elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[1];
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[1];
		elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[1];
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
	}

	public override void AutoDestructEffect()
	{
	}

	public override void GunOff()
	{
		base.GunOff();
	}

	public override void PlaySound()
	{
		if (!PlayElementSound())
		{
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/revolver/revolver_fire", player.GetTransform().position);
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		if (!player.IsLocal())
		{
			GetElementTypeAndPara();
		}
		PlaySound();
		PlayGunFireEffect(gunfire.transform.forward);
		Vector3 normalized = player.GetTransform().forward.normalized;
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		PlayElementBulletTrail(gunfire.position, normalized);
	}

	protected override void DestroyEffect()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			ObjectPool effectPool = gameScene.GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_SPARK);
			if (effectPool != null)
			{
				effectPool.DestructAll();
			}
			ObjectPool effectPool2 = gameScene.GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_FIRE_SPARK);
			if (effectPool != null)
			{
				effectPool.DestructAll();
			}
			ObjectPool effectPool3 = gameScene.GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_SHOCK_SPARK);
			if (effectPool != null)
			{
				effectPool.DestructAll();
			}
			ObjectPool effectPool4 = gameScene.GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_CORROSIVE_SPARK);
			if (effectPool != null)
			{
				effectPool.DestructAll();
			}
		}
	}

	protected override void PlayGunFireEffect(Vector3 direction)
	{
		GameObject gameObject = null;
		switch (mCurrentElementType)
		{
		case ElementType.Fire:
			if (gunfire != null)
			{
				gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_FIRE_SPARK)
					.CreateObject(gunfire.position, direction, Quaternion.identity);
			}
			break;
		case ElementType.Shock:
			if (gunfire != null)
			{
				gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_SHOCK_SPARK)
					.CreateObject(gunfire.position, direction, Quaternion.identity);
			}
			break;
		case ElementType.Corrosive:
			if (gunfire != null)
			{
				gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_CORROSIVE_SPARK)
					.CreateObject(gunfire.position, direction, Quaternion.identity);
			}
			break;
		}
		if (gameObject == null)
		{
			gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.UN_LOOP_WEAPON_SPARK)
				.CreateObject(gunfire.position, direction, Quaternion.identity);
		}
		gameObject.transform.parent = gunfire;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	public override void Attack(float deltaTime)
	{
		base.Attack(deltaTime);
		PlaySound();
	}

	public override void Reload()
	{
		base.Reload();
		PlayAnimation("reload", WrapMode.Once, GetReloadAnimationSpeed());
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/revolver/revolver_reload", player.GetTransform().position);
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[1];
	}
}
