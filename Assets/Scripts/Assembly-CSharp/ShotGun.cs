using UnityEngine;

public class ShotGun : Weapon
{
	protected static int sbulletCount;

	protected Timer shotgunFireTimer;

	protected bool readyforCock;

	private float damageDeductRate = 0.08f;

	protected int reticleWidth;

	protected int reticleHeight;

	public override int BulletCountInGun
	{
		get
		{
			return sbulletCount;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.ShotGun;
	}

	public override string GetAnimationSuffix()
	{
		return "shootgun01_";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "shootgun01_";
	}

	public override float GetReloadAnimationSpeed()
	{
		return 2.6f * base.ReloadTimeScale / reloadTime;
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		PlaySound();
		GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.SHOTGUN_SPARK)
			.CreateObject(gunfire.position, gunfire.transform.forward, Quaternion.identity);
	}

	public override void Init(Player player)
	{
		base.Init(player);
		base.AttackFrequencyT = 0.25f;
		SetWeaponShootSpeed(1f);
		Object.DontDestroyOnLoad(gunfire);
		elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[0];
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[0];
		elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[0];
	}

	public void PlayPumpAnimation()
	{
	}

	public override void PlayBlankShootSound()
	{
		AudioManager.GetInstance().PlaySoundSingle("Audio/blank/blank_shot02");
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
	}

	protected override void PlayBulletFloorEffect(RaycastHit hit)
	{
		GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.SHOTGUN_FLOOR_SPARK)
			.CreateObject(hit.point, hit.normal, Quaternion.identity);
	}

	protected override void PlayBulletWallEffect(RaycastHit hit)
	{
		GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.SHOTGUN_WALL_SPARK)
			.CreateObject(hit.point, Vector3.zero, Quaternion.identity);
	}

	public override void Attack(float deltaTime)
	{
		hitAchievement_HitCount = 0;
		UseBullet(1);
		PlaySound();
		CheckEnemySoundDetection();
		cameraComponent = Camera.main;
		cameraTransform = cameraComponent.transform;
		aimTarget = cameraTransform.TransformPoint(0f, 0f, 100f);
		Vector3 normalized = (aimTarget - gunfire.position).normalized;
		GameObject gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.SHOTGUN_SPARK)
			.CreateObject(gunfire.position, normalized, Quaternion.identity);
		gameObject.transform.parent = gunfire;
		AttackByRaycast(false, true);
		for (int i = 0; i < 6; i++)
		{
			AttackByRaycast(false, false);
		}
		SetRecoilToCamera();
		attacked = false;
		lastAttackTime = Time.time;
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Yakitoui, AchievementTrigger.Type.Data);
		achievementTrigger.PutData(hitAchievement_HitCount);
		AchievementManager.GetInstance().Trigger(achievementTrigger);
		if (hitAchievement_HitCount == 0)
		{
			if (GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Boss)
			{
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Start);
				AchievementManager.GetInstance().Trigger(trigger);
				AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Data);
				achievementTrigger2.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger2);
			}
		}
		else
		{
			AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger2);
		}
	}

	protected override void DestroyEffect()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			ObjectPool effectPool = gameScene.GetEffectPool(EffectPoolType.SHOTGUN_SPARK);
			if (effectPool != null)
			{
				effectPool.DestructAll();
			}
		}
	}

	public override void GunOff()
	{
		base.GunOff();
		readyforCock = false;
	}

	public override void Reload()
	{
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/shotgun/shotgun_reload", player.GetTransform().position);
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[0];
	}

	public override void PlaySound()
	{
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/shotgun/shotgun_fire", player.GetTransform().position);
	}
}
