using UnityEngine;

public class Sniper : Weapon
{
	private static float SNIPER_SCOPE_FOV_40 = 21.4f;

	private static float SNIPER_SCOPE_FOV_80 = 16.7f;

	private static float SNIPER_SCOPE_FOV_120 = 13f;

	private static float SNIPER_SCOPE_FOV_160 = 10f;

	protected ObjectPool firelineObjectPool;

	protected ObjectPool sparksObjectPool;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.Sniper;
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override string GetAnimationSuffix()
	{
		return "sniper_rifle_";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "sniper_rifle_";
	}

	public override float GetReloadAnimationSpeed()
	{
		return 2.37f * base.ReloadTimeScale / reloadTime;
	}

	public override float GetScopeFOV()
	{
		if (scopeRate == 40)
		{
			return SNIPER_SCOPE_FOV_40;
		}
		if (scopeRate == 80)
		{
			return SNIPER_SCOPE_FOV_80;
		}
		if (scopeRate == 120)
		{
			return SNIPER_SCOPE_FOV_120;
		}
		if (scopeRate == 160)
		{
			return SNIPER_SCOPE_FOV_160;
		}
		return SNIPER_SCOPE_FOV_40;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		base.AttackFrequencyT = 0.5f;
		SetWeaponShootSpeed(1f);
		Object.DontDestroyOnLoad(gunfire);
		elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[4];
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[4];
		elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[4];
	}

	public override void Loop(float deltaTime)
	{
		if (Achieve001_StopTimer.Ready())
		{
			StopAchievement023Trigger();
			Achieve001_StopTimer.Do();
		}
	}

	public override void AutoDestructEffect()
	{
	}

	public override void StopFire()
	{
		if (gunfireObj != null)
		{
			gunfireObj.SetActive(false);
		}
		if (gunfireObj_Fire != null)
		{
			gunfireObj_Fire.SetActive(false);
		}
		if (gunfireObj_Shock != null)
		{
			gunfireObj_Shock.SetActive(false);
		}
		if (gunfireObj_Corrosive != null)
		{
			gunfireObj_Corrosive.SetActive(false);
		}
	}

	public override void GunOff()
	{
		base.GunOff();
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
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Yakitoui, AchievementTrigger.Type.Data);
		achievementTrigger.PutData(hitAchievement_HitCount);
		AchievementManager.GetInstance().Trigger(achievementTrigger);
	}

	public override void Reload()
	{
		base.Reload();
		PlayAnimation("reload", WrapMode.Once, GetReloadAnimationSpeed());
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/sniper/sniper_reload", player.GetTransform().position);
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[4];
	}

	public override void PlaySound()
	{
		if (!PlayElementSound())
		{
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/sniper/sniper_fire", player.GetTransform().position);
		}
	}
}
