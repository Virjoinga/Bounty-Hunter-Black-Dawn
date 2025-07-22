using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameEntity
{
	protected const float FIRE_LINE_HALF_LENGTH = 2.5f;

	protected const float FPS_FIRE_LINE_HALF_LENGTH = 5f;

	private static float SCOPE_FOV_40 = 21.4f;

	private static float SCOPE_FOV_80 = 16.7f;

	private static float SCOPE_FOV_120 = 13f;

	private static float SCOPE_FOV_160 = 10f;

	protected bool attacked;

	protected float lastAttackTime;

	protected GameObject hitParticles;

	protected GameObject projectile;

	protected Camera cameraComponent;

	protected Transform cameraTransform;

	protected FirstPersonCameraScript gameCamera;

	protected Transform gunfire;

	protected GameObject gunfireObj;

	protected GameObject gunfireObj_Fire;

	protected GameObject gunfireObj_Shock;

	protected GameObject gunfireObj_Corrosive;

	protected Transform weaponBoneTrans;

	protected AudioSource shootAudio;

	protected Player player;

	protected Vector3 aimTarget;

	protected int gunID = 1;

	protected bool isCDing;

	protected float hitForce;

	protected float range;

	protected float rangeInit;

	protected float bombRange;

	protected int maxTotalBullet;

	protected int bulletInGun;

	protected int gunCapacity;

	protected int gunCapacityInit;

	protected int totalBullet;

	protected float maxDeflection;

	protected Vector2 deflection;

	protected float damage;

	protected float damageInit;

	protected float splashDamage;

	protected float splashDamageInit;

	protected float splashDuration;

	protected float bombRangeInit;

	protected float attackFrenquency;

	protected float attackFrenquencyInit;

	protected float accuracy;

	protected float accuracyInit;

	protected int enegyConsume = 5;

	protected float reloadTime;

	protected float speedDrag;

	protected Vector3 lastHitPosition;

	protected int price;

	protected int mithril;

	protected byte unlockLevel;

	protected byte weaponNameNumber;

	protected float notUseBulletRate;

	protected float notUseBulletRateInit;

	protected float reloadTimeScale = 1f;

	protected float reloadTimeScaleInit = 1f;

	protected byte elementFirePara;

	protected byte elementShockPara;

	protected byte elementCorrosivePara;

	protected int elementDotTriggerTimes;

	protected float elementDotTriggerBase;

	protected float elementDotTriggerScale;

	protected int elementLastFrameCount;

	public ElementType mCurrentElementType;

	protected byte mCurrentElementPara;

	protected int mElementTypeLastFrameCount;

	protected float criticalRate;

	protected float criticalRateInit;

	protected float criticalDamage = 1.5f;

	protected float criticalDamageInit = 1.5f;

	protected bool hasScope;

	protected short scopeRate;

	protected byte recoil;

	protected byte recoilInit;

	protected float meleeDamage;

	protected float meleeDamageInit;

	protected bool mPenetration;

	protected Timer Achieve001_StopTimer = new Timer();

	protected int hitAchievement_HitCount;

	protected byte level;

	protected Vector3 center;

	protected Timer playSoundTimer = new Timer();

	protected bool waitForAttackAnimationStop = true;

	public Renderer[] gunRenderers;

	public int DamageLevel { get; set; }

	public int FrequencyLevel { get; set; }

	public int AccuracyLevel { get; set; }

	public float ReloadTime
	{
		get
		{
			return reloadTime;
		}
	}

	public WeaponConfig WConf { get; set; }

	public bool IsSelectedForBattle { get; set; }

	public float AimOffset { get; set; }

	public float MeleeDamage
	{
		get
		{
			return meleeDamage;
		}
	}

	public float MeleeDamageInit
	{
		get
		{
			return meleeDamageInit;
		}
	}

	public float CriticalRate
	{
		get
		{
			return criticalRate;
		}
	}

	public float CriticalRateInit
	{
		get
		{
			return criticalRateInit;
		}
	}

	public float CriticalDamage
	{
		get
		{
			return criticalDamage;
		}
	}

	public float CriticalDamageInit
	{
		get
		{
			return criticalDamageInit;
		}
	}

	public bool HasScope
	{
		get
		{
			return hasScope;
		}
	}

	public float ElementFirePara
	{
		get
		{
			return (int)elementFirePara;
		}
	}

	public float ElementShockPara
	{
		get
		{
			return (int)elementShockPara;
		}
	}

	public float ElementCorrosivePara
	{
		get
		{
			return (int)elementCorrosivePara;
		}
	}

	public float ExplosionRangeInit
	{
		get
		{
			return bombRangeInit;
		}
	}

	public float ScopeRate
	{
		get
		{
			return (float)(scopeRate + 100) / 100f;
		}
	}

	public float RecoilInit
	{
		get
		{
			return (int)recoilInit;
		}
	}

	public float AttackFrequencyT { get; set; }

	public WeaponAdjuster Adjuster { get; set; }

	public string FireSoundName { get; set; }

	public string ExplodeSoundName { get; set; }

	public int AimID { get; set; }

	public int GunID
	{
		get
		{
			return gunID;
		}
		set
		{
			gunID = value;
		}
	}

	public byte WeaponNameNumber
	{
		get
		{
			return weaponNameNumber;
		}
		set
		{
			weaponNameNumber = value;
		}
	}

	public string Info
	{
		get
		{
			return Name;
		}
	}

	public new string Name { get; set; }

	public string PrefabName { get; set; }

	public int Price
	{
		get
		{
			return price;
		}
	}

	public int Mithril
	{
		get
		{
			return mithril;
		}
	}

	public byte Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public float Accuracy
	{
		get
		{
			return accuracy;
		}
		set
		{
			accuracy = value;
		}
	}

	public float AccuracyInit
	{
		get
		{
			return accuracyInit;
		}
	}

	public GameObject Gun
	{
		get
		{
			return entityObject;
		}
	}

	public Vector3 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public int GunCapacity
	{
		get
		{
			return gunCapacity;
		}
	}

	public int TotalBullet
	{
		get
		{
			return totalBullet;
		}
	}

	public float Damage
	{
		get
		{
			return damage;
		}
		set
		{
			damage = value;
		}
	}

	public float DamageInit
	{
		get
		{
			return damageInit;
		}
	}

	public float AttackFrequency
	{
		get
		{
			return attackFrenquency;
		}
		set
		{
			attackFrenquency = value;
		}
	}

	public float AttackFrenquencyInit
	{
		get
		{
			return attackFrenquencyInit;
		}
	}

	public int EnegyConsume
	{
		get
		{
			return enegyConsume;
		}
		set
		{
			enegyConsume = value;
		}
	}

	public float ReloadTimeScale
	{
		get
		{
			return reloadTimeScale;
		}
	}

	public virtual int BulletCountInGun
	{
		get
		{
			return bulletInGun;
		}
		set
		{
			bulletInGun = value;
		}
	}

	public Vector2 Deflection
	{
		get
		{
			return deflection;
		}
	}

	public byte UnlockLevel
	{
		get
		{
			return unlockLevel;
		}
	}

	public virtual void Attack(float deltaTime)
	{
		hitAchievement_HitCount = 0;
		UseBullet(1);
		AttackByRaycast();
		SetRecoilToCamera();
		CheckEnemySoundDetection();
		attacked = false;
		lastAttackTime = Time.time;
		if (hitAchievement_HitCount == 0)
		{
			if (GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Boss)
			{
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Start);
				AchievementManager.GetInstance().Trigger(trigger);
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
			}
		}
		else
		{
			AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger2);
		}
	}

	public virtual void StopFire()
	{
	}

	public virtual WeaponType GetWeaponType()
	{
		return WeaponType.NoGun;
	}

	public bool GetWaitForAttackAnimationStop()
	{
		return waitForAttackAnimationStop;
	}

	public virtual bool IsTypeOfLoopShootingWeapon()
	{
		return true;
	}

	public void EnableGunObject(bool bEnable)
	{
		if (!bEnable)
		{
			if (gunRenderers != null)
			{
				Renderer[] array = gunRenderers;
				foreach (Renderer renderer in array)
				{
					renderer.enabled = false;
				}
			}
			if (gunfire != null && gunfire.gameObject != null)
			{
				gunfire.gameObject.SetActive(false);
			}
			return;
		}
		if (gunRenderers != null)
		{
			Renderer[] array2 = gunRenderers;
			foreach (Renderer renderer2 in array2)
			{
				renderer2.enabled = true;
			}
		}
		if (gunfire != null && gunfire.gameObject != null)
		{
			gunfire.gameObject.SetActive(true);
		}
	}

	public virtual string GetAnimationSuffix()
	{
		return string.Empty;
	}

	public virtual string GetReloadAnimationSuffix()
	{
		return string.Empty;
	}

	public virtual float GetReloadAnimationSpeed()
	{
		return 1f;
	}

	public void GeneratePrefabName()
	{
		PrefabName = "Weapon/" + LootManager.GetPrefabNameByItemClassAndNumber((ItemClasses)GetWeaponType(), WeaponNameNumber);
	}

	public void GenerateThirdPersonPrefabName()
	{
		PrefabName = "WeaponL/";
		switch (GetWeaponType())
		{
		case WeaponType.SubMachineGun:
			PrefabName += "SMG";
			break;
		case WeaponType.AssaultRifle:
			PrefabName += "assault";
			break;
		case WeaponType.Pistol:
			PrefabName += "pistol";
			break;
		case WeaponType.Revolver:
			PrefabName += "revolver";
			break;
		case WeaponType.ShotGun:
			PrefabName += "shotgun";
			break;
		case WeaponType.Sniper:
			PrefabName += "sniper";
			break;
		case WeaponType.RPG:
			PrefabName += "RPG";
			break;
		default:
			PrefabName += "SMG";
			break;
		}
		PrefabName += string.Format("{0:D2}", weaponNameNumber);
		PrefabName += "_l";
	}

	public static string CreatePrefabName(WeaponType wType, int number)
	{
		string empty = string.Empty;
		return "Weapon/" + LootManager.GetPrefabNameByItemClassAndNumber((ItemClasses)wType, number);
	}

	public virtual void DoLogic()
	{
	}

	public void CreateGun()
	{
		Debug.Log(PrefabName);
		UnityEngine.Object original = Resources.Load(PrefabName);
		entityObject = (GameObject)UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity);
		animationObject = entityObject;
		gunRenderers = WeaponResourceConfig.GetWeaponRenderers(entityObject, gunID);
	}

	public float GetSpeedDrag()
	{
		return speedDrag;
	}

	public int GetGunID()
	{
		return gunID;
	}

	public void Upgrade()
	{
	}

	public float GetNextLevelDamage()
	{
		return damage + damageInit * 1f;
	}

	public int GetDamageUpgradePrice()
	{
		return 0;
	}

	public virtual void Init(Player player)
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		gameCamera = Camera.main.GetComponent<FirstPersonCameraScript>();
		if (gameCamera != null)
		{
			cameraComponent = gameCamera.GetComponent<Camera>();
			cameraTransform = gameCamera.CameraTransform;
		}
		this.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.Weapon);
		if (weaponBoneTrans == null)
		{
			Debug.Log("could not find weapon point!");
		}
		if (player.IsLocal())
		{
			GeneratePrefabName();
		}
		else
		{
			GenerateThirdPersonPrefabName();
		}
		CreateGun();
		AddGunToBone();
		shootAudio = entityObject.GetComponent<AudioSource>();
		if (shootAudio == null)
		{
		}
		gunfire = WeaponResourceConfig.GetWeaponGunFire(entityObject, gunID);
		GunOff();
		Achieve001_StopTimer.SetTimer(1.5f, true);
		GetElementTypeAndPara();
	}

	public void AddGunToBone()
	{
		entityObject.transform.position = weaponBoneTrans.position;
		entityObject.transform.rotation = weaponBoneTrans.rotation;
		entityObject.transform.parent = weaponBoneTrans;
	}

	public void DamageBonus(float bonusAll, float bonusWeapon)
	{
		damage = damageInit;
		damage *= 1f + bonusAll + bonusWeapon;
	}

	public void AttackFrequencyBonus(float bonusAll, float bonusWeapon)
	{
		attackFrenquency = attackFrenquencyInit * 1f / (1f + bonusAll + bonusWeapon);
	}

	public void RangeBonus(float bonusAll, float bonusWeapon)
	{
		range = rangeInit + bonusAll + bonusWeapon;
	}

	public void ExplosionRangeBonus(float bonus)
	{
		bombRange = bombRangeInit * (1f + bonus);
	}

	public void AccuracyBonus(float bonusAll, float bonusWeapon)
	{
		accuracy = 100f - (100f - accuracyInit) / (1f + bonusAll + bonusWeapon);
		accuracy = Mathf.Clamp(accuracy, 0f, 100f);
	}

	public void RecoilBonus(float bonusAll, float bonusWeapon)
	{
		float num = (float)(int)recoilInit * (1f + bonusAll + bonusWeapon);
		Mathf.Clamp(num, 0f, 100f);
		recoil = (byte)num;
	}

	public void NotUseBulletRateBonus(float bonusAll, float bonusWeapon)
	{
		notUseBulletRate = notUseBulletRateInit * (1f + bonusAll + bonusWeapon);
	}

	public void GunCapacityBonus(float bonusAll, float bonusWeapon, int bonusValueAll, int bonusWeaponValue)
	{
		gunCapacity = Mathf.CeilToInt((float)gunCapacityInit * (1f + bonusAll + bonusWeapon)) + bonusValueAll + bonusWeaponValue;
	}

	public void ReloadTimeBonus(float bonusAll, float bonusWeapon)
	{
		reloadTimeScale = reloadTimeScaleInit + bonusAll + bonusWeapon;
	}

	public void CriticalRateBonus(float bonusAll, float bonusWeapon)
	{
		criticalRate = criticalRateInit + bonusAll + bonusWeapon;
	}

	public void CriticalDamageBonus(float bonusAll, float bonusWeapon)
	{
		criticalDamage = criticalDamageInit + bonusAll + bonusWeapon;
	}

	public void PenetrationBonus(bool bonusAll, bool bonusWeapon)
	{
		mPenetration = bonusAll || bonusWeapon;
	}

	public virtual float SimpleDamage()
	{
		return damageInit;
	}

	public virtual float SimpleDamage(int gunLevel)
	{
		return damageInit;
	}

	public virtual void Loop(float deltaTime)
	{
	}

	public virtual void AutoDestructEffect()
	{
	}

	public virtual void AutoAim(float deltaTime)
	{
	}

	public virtual void PlaySound()
	{
	}

	public virtual void Reload()
	{
		if (animationObject != null && animationObject.GetComponent<Animation>() != null && animationObject.GetComponent<Animation>()["reload"] != null)
		{
			animationObject.GetComponent<Animation>()["reload"].time = 0f;
		}
	}

	public void StopReload()
	{
		if (animationObject.GetComponent<Animation>() != null && animationObject.GetComponent<Animation>()["reload"] != null && animationObject.GetComponent<Animation>()["reload"].clip != null && animationObject.GetComponent<Animation>().IsPlaying("reload"))
		{
			animationObject.GetComponent<Animation>()["reload"].time = animationObject.GetComponent<Animation>()["reload"].clip.length;
		}
	}

	public void ReloadComplete()
	{
		int bulletByWeaponType = GameApp.GetInstance().GetUserState().GetBulletByWeaponType(GetWeaponType());
		if (bulletByWeaponType > 0)
		{
			int num = gunCapacity - BulletCountInGun;
			if (num > bulletByWeaponType)
			{
				num = bulletByWeaponType;
			}
			BulletCountInGun += num;
			ItemInfiniteBullet itemInfiniteBullet = (ItemInfiniteBullet)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
				.GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
			if (!itemInfiniteBullet.IsUnlimitedBullet(GetWeaponType()))
			{
				GameApp.GetInstance().GetUserState().AddBulletByWeaponType(GetWeaponType(), (short)(-num));
			}
			if (player != null && player.IsLocal())
			{
				(player as LocalPlayer).UsedBulletWithoutReload = 0;
			}
		}
	}

	public void UseBullet(int num)
	{
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.The_Arms_Dealer, AchievementTrigger.Type.Data);
		achievementTrigger.PutData(num);
		AchievementManager.GetInstance().Trigger(achievementTrigger);
		float num2 = UnityEngine.Random.Range(0f, 1f);
		if (!(num2 <= notUseBulletRate))
		{
			if (BulletCountInGun > 0)
			{
				BulletCountInGun--;
			}
			if (player != null && player.IsLocal())
			{
				(player as LocalPlayer).UsedBulletWithoutReload++;
			}
		}
	}

	public virtual void AddBullets()
	{
		int bulletByWeaponType = player.GetUserState().GetBulletByWeaponType(GetWeaponType());
		bulletByWeaponType += gunCapacity;
	}

	public void AddBullets(int num)
	{
		BulletCountInGun += num;
		BulletCountInGun = Mathf.Clamp(BulletCountInGun, 0, gunCapacity);
	}

	public virtual void GunOn()
	{
		if (entityObject != null && gunRenderers != null)
		{
			Renderer[] array = gunRenderers;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = true;
			}
		}
	}

	public bool CheckBullets()
	{
		if (player.IsLocal())
		{
			if (player.GetUserState().GetBulletByWeaponType(GetWeaponType()) == 0 && BulletCountInGun == 0)
			{
				StopFire();
				return false;
			}
			return true;
		}
		return true;
	}

	public virtual bool HaveBullets()
	{
		if (player == null)
		{
			return false;
		}
		if (player.IsLocal())
		{
			if (player.GetUserState().GetBulletByWeaponType(GetWeaponType()) == 0 && BulletCountInGun == 0)
			{
				PlayBlankShootSound();
				StopFire();
				return false;
			}
			return true;
		}
		return true;
	}

	public bool NoBulletsInGun()
	{
		if (BulletCountInGun == 0)
		{
			return true;
		}
		return false;
	}

	public virtual void PlayBlankShootSound()
	{
		AudioManager.GetInstance().PlaySoundSingle("Audio/blank/blank_shot01");
	}

	public virtual void CreateTrajectory()
	{
		lastAttackTime = Time.time;
	}

	public virtual bool CoolDown()
	{
		if (Time.time - lastAttackTime > ((!player.IsLocal()) ? AttackFrequencyT : attackFrenquency))
		{
			return true;
		}
		return false;
	}

	public void SetWeaponShootSpeed(float speed)
	{
		Animation animation = player.GetObject().transform.Find("Entity").gameObject.GetComponent<Animation>();
		if (animation[GetAnimationSuffix() + AnimationString.Attack] != null)
		{
			if (player.IsLocal())
			{
				animation[GetAnimationSuffix() + AnimationString.Attack].speed = Mathf.Max(1f, speed / attackFrenquency);
			}
			else
			{
				animation[GetAnimationSuffix() + AnimationString.Attack].speed = Mathf.Max(1f, speed / AttackFrequencyT);
			}
		}
		if (player.IsLocal() && animation[GetAnimationSuffix() + AnimationString.AimAttack] != null)
		{
			animation[GetAnimationSuffix() + AnimationString.AimAttack].speed = Mathf.Max(1f, speed / attackFrenquency);
		}
	}

	public virtual void GunOff()
	{
		if (entityObject != null && gunRenderers != null)
		{
			Renderer[] array = gunRenderers;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = false;
			}
		}
		StopFire();
	}

	protected void CheckEnemyBulletDetection(Vector3 hitPosition)
	{
		Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value in enemies.Values)
		{
			value.DetectBullet(hitPosition);
		}
	}

	protected void CheckEnemySoundDetection()
	{
		Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (Enemy value in enemies.Values)
		{
			value.DetectWeaponSound();
		}
	}

	public void SetWeaponPropertyWithNGUIBaseItem(NGUIBaseItem item)
	{
		if (item.itemStats.Count < 3)
		{
			Debug.Log("No stats!?");
			return;
		}
		level = item.ItemLevel;
		Name = item.name;
		damageInit = item.itemStats[0].statValue;
		damage = damageInit;
		accuracyInit = item.itemStats[1].statValue;
		accuracy = accuracyInit;
		attackFrenquencyInit = item.itemStats[2].statValue;
		attackFrenquency = attackFrenquencyInit;
		reloadTime = item.itemStats[3].statValue;
		gunCapacity = (int)item.itemStats[4].statValue;
		gunCapacityInit = gunCapacity;
		ReloadComplete();
		recoilInit = (byte)item.itemStats[5].statValue;
		recoil = recoilInit;
		meleeDamageInit = item.itemStats[6].statValue;
		meleeDamage = meleeDamageInit;
		criticalRateInit = item.itemStats[7].statValue;
		criticalRate = criticalRateInit;
		criticalDamageInit = item.itemStats[8].statValue;
		criticalDamage = criticalDamageInit;
		hasScope = item.itemStats[9].statValue != 0f;
		scopeRate = (short)item.itemStats[14].statValue;
		bombRangeInit = item.itemStats[10].statValue;
		bombRange = bombRangeInit;
		elementFirePara = (byte)item.itemStats[11].statValue;
		elementShockPara = (byte)item.itemStats[12].statValue;
		elementCorrosivePara = (byte)item.itemStats[13].statValue;
		Adjuster = new WeaponAdjuster(10f, 0f, 0.8f, 0.5f);
	}

	protected virtual void DestroyEffect()
	{
	}

	public virtual void DestroyWeapon()
	{
		DestroyEffect();
		UnityEngine.Object.Destroy(hitParticles);
		UnityEngine.Object.Destroy(projectile);
		UnityEngine.Object.Destroy(gunfireObj);
		UnityEngine.Object.Destroy(gunfireObj_Fire);
		UnityEngine.Object.Destroy(gunfireObj_Shock);
		UnityEngine.Object.Destroy(gunfireObj_Corrosive);
		UnityEngine.Object.Destroy(entityObject);
		gunRenderers = null;
	}

	public virtual float CalculateMinAccurancyFactor()
	{
		return GetCameraRange() * (100f - accuracy) / (float)(GetLongestScope() * 100);
	}

	public virtual float CalculateMaxAccurancyFactor()
	{
		return GetCameraRange() / (float)GetLongestScope();
	}

	protected virtual int GetLongestScope()
	{
		int result = 0;
		switch (GetWeaponType())
		{
		case WeaponType.SubMachineGun:
			result = 3;
			break;
		case WeaponType.AssaultRifle:
			result = 3;
			break;
		case WeaponType.Pistol:
			result = 2;
			break;
		case WeaponType.Revolver:
			result = 2;
			break;
		case WeaponType.ShotGun:
			result = 2;
			break;
		case WeaponType.Sniper:
			result = 5;
			break;
		case WeaponType.RPG:
			result = 4;
			break;
		}
		return result;
	}

	protected virtual float GetCameraRange()
	{
		float result = 0f;
		switch (GetWeaponType())
		{
		case WeaponType.SubMachineGun:
			result = 0.4f;
			break;
		case WeaponType.AssaultRifle:
			result = 0.6f;
			break;
		case WeaponType.Pistol:
			result = 0.2f;
			break;
		case WeaponType.Revolver:
			result = 0.2f;
			break;
		case WeaponType.ShotGun:
			result = 0.8f;
			break;
		case WeaponType.Sniper:
			result = 1.3f;
			break;
		case WeaponType.RPG:
			result = 1.2f;
			break;
		}
		return result;
	}

	protected virtual int GetRecoilRecover()
	{
		int result = 0;
		switch (GetWeaponType())
		{
		case WeaponType.SubMachineGun:
			result = 20;
			break;
		case WeaponType.AssaultRifle:
			result = 30;
			break;
		case WeaponType.Pistol:
			result = 20;
			break;
		case WeaponType.Revolver:
			result = 20;
			break;
		case WeaponType.ShotGun:
			result = 20;
			break;
		case WeaponType.Sniper:
			result = 20;
			break;
		case WeaponType.RPG:
			result = 20;
			break;
		}
		return result;
	}

	protected virtual void SetRecoilToCamera()
	{
		AimOffset += (int)recoil;
		FirstPersonCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
		float num = CalculateMaxAccurancyFactor() * camera.GetComponent<Camera>().pixelHeight * 0.5f;
		if (AimOffset > num)
		{
			AimOffset = num;
		}
		camera.AngelV += (float)(int)recoil * 0.05f;
		camera.recoilAngleV += (float)(int)recoil * 0.05f;
	}

	public virtual void RecoverRecoil(float deltaTime)
	{
		AimOffset -= deltaTime * (float)GetRecoilRecover();
		FirstPersonCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
		float num = CalculateMinAccurancyFactor() * camera.GetComponent<Camera>().pixelHeight * 0.5f;
		if (AimOffset < num)
		{
			AimOffset = num;
		}
	}

	public virtual float GetScopeFOV()
	{
		if (scopeRate == 40)
		{
			return SCOPE_FOV_40;
		}
		if (scopeRate == 80)
		{
			return SCOPE_FOV_80;
		}
		if (scopeRate == 120)
		{
			return SCOPE_FOV_120;
		}
		if (scopeRate == 160)
		{
			return SCOPE_FOV_160;
		}
		return SCOPE_FOV_40;
	}

	public virtual void AttackByRaycast()
	{
		AttackByRaycast(true, true);
	}

	protected virtual void PlayGunFireEffect(Vector3 direction)
	{
	}

	public virtual void AttackByRaycast(bool needFireLine, bool needBulletScar)
	{
		cameraComponent = Camera.main;
		cameraTransform = cameraComponent.transform;
		gameCamera = Camera.main.GetComponent<FirstPersonCameraScript>();
		Ray ray = default(Ray);
		float num = 0f;
		float num2 = 0f;
		float num3 = AimOffset - 7f;
		float num4 = UnityEngine.Random.Range(0f, 1f);
		if (num4 < (float)(int)recoil * 0.01f)
		{
			num3 *= 0.5f;
		}
		num += UnityEngine.Random.Range(0f - num3, num3);
		num2 += UnityEngine.Random.Range(0f - num3, num3);
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x + num, (float)Screen.height - gameCamera.ReticlePosition.y + num2, 0.1f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		ray = new Ray(cameraTransform.position + normalized, normalized);
		PlayGunFireEffect(normalized);
		RaycastHit hit = default(RaycastHit);
		hit.point = cameraTransform.position + (vector - cameraTransform.position).normalized * 80f;
		float num5 = Mathf.Tan((float)Math.PI / 3f);
		RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED));
		GetElementTypeAndPara();
		if (!mPenetration)
		{
			StopAchievement023Trigger();
			HitDetectionNormal(ray, hits, hit, needBulletScar);
		}
		else
		{
			StopAchievement023Trigger();
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.No_survival, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
			HitDetectionPenetrantion(ray, hits, hit, needBulletScar);
		}
		PlayElementGunfire();
		aimTarget = hit.point;
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		if (needFireLine)
		{
			PlayElementBulletTrail(hit.point, normalized2);
		}
	}

	public virtual void HitDetectionNormal(Ray ray, RaycastHit[] hits, RaycastHit hit, bool needBulletScar)
	{
		float num = 1000000f;
		for (int i = 0; i < hits.Length; i++)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && hits[i].collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(hits[i].collider.gameObject.transform.parent.name);
				Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			Vector3 zero = Vector3.zero;
			if (hits[i].collider.gameObject.layer == PhysicsLayer.ENEMY || hits[i].collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
			{
				zero = gunfire.transform.InverseTransformPoint(hits[i].collider.transform.position);
			}
			else
			{
				zero = gunfire.transform.InverseTransformPoint(hits[i].point);
			}
			float sqrMagnitude = (hits[i].point - ray.origin).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				hit = hits[i];
				num = sqrMagnitude;
			}
		}
		Hit(ray, hit, false, needBulletScar);
	}

	public virtual void HitDetectionPenetrantion(Ray ray, RaycastHit[] hits, RaycastHit hit, bool needBulletScar)
	{
		float num = 1000f;
		for (int i = 0; i < hits.Length; i++)
		{
			if ((hits[i].collider.gameObject.layer == PhysicsLayer.WALL || hits[i].collider.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL) && hits[i].distance < num)
			{
				num = hits[i].distance;
			}
		}
		for (int j = 0; j < hits.Length; j++)
		{
			Vector3 zero = Vector3.zero;
			if (hits[j].distance > num)
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameMode().IsTeamMode() && hits[j].collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
			{
				int userID = int.Parse(hits[j].collider.gameObject.transform.parent.name);
				Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
				{
					continue;
				}
			}
			if (hits[j].collider.gameObject.layer == PhysicsLayer.ENEMY || hits[j].collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
			{
				zero = gunfire.transform.InverseTransformPoint(hits[j].collider.transform.position);
			}
			else
			{
				zero = gunfire.transform.InverseTransformPoint(hits[j].point);
			}
			hit = hits[j];
			Hit(ray, hit, true, needBulletScar);
		}
	}

	protected virtual void PlayBulletFloorEffect(RaycastHit hit)
	{
		if (!GameApp.GetInstance().GetGameScene().PlayOnHitElementEffect(hit.point, mCurrentElementType))
		{
			GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.BULLET_FLOOR_SPARK)
				.CreateObject(hit.point, hit.normal, Quaternion.identity);
		}
	}

	protected virtual void PlayBulletWallEffect(RaycastHit hit)
	{
		if (!GameApp.GetInstance().GetGameScene().PlayOnHitElementEffect(hit.point, mCurrentElementType))
		{
			GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.BULLET_WALL_SPARK)
				.CreateObject(hit.point, Vector3.zero, Quaternion.identity);
		}
	}

	public void Hit(Ray ray, RaycastHit hit, bool penetration, bool needBulletScar)
	{
		if (!(hit.collider != null))
		{
			return;
		}
		if (needBulletScar)
		{
			if (hit.collider.gameObject.layer == PhysicsLayer.FLOOR)
			{
				PlayBulletFloorEffect(hit);
			}
			else if (hit.collider.gameObject.layer == PhysicsLayer.WALL)
			{
				PlayBulletWallEffect(hit);
			}
		}
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.hitForce = ray.direction * 2f;
		damageProperty.damage = (int)damage;
		damageProperty.hitpoint = hit.point;
		damageProperty.isLocal = true;
		damageProperty.wType = GetWeaponType();
		damageProperty.isPenetration = penetration;
		damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		damageProperty.weaponLevel = level;
		if (GetWeaponType() == WeaponType.Sniper && player.InAimState && (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || !GameApp.GetInstance().GetGameMode().IsVSMode()))
		{
			damageProperty.damage = Mathf.CeilToInt((float)damageProperty.damage * 1.5f);
		}
		if (hit.collider.gameObject.layer == PhysicsLayer.ENEMY || hit.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
		{
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(hit.collider);
			Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
			if (enemyByID.InPlayingState())
			{
				hitAchievement_HitCount++;
				bool flag = false;
				if (hit.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
				{
					flag = true;
					if (player.IsLocal())
					{
						float num = Vector3.Distance(hit.collider.transform.position, player.GetPosition());
						AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Safe_Zone, AchievementTrigger.Type.Data);
						achievementTrigger.PutData((int)num);
						AchievementManager.GetInstance().Trigger(achievementTrigger);
					}
				}
				else
				{
					float num2 = UnityEngine.Random.Range(0f, 1f);
					if (num2 < CriticalRate)
					{
						flag = true;
					}
				}
				if (flag)
				{
					damageProperty.damage = (int)((float)damageProperty.damage * criticalDamage);
				}
				damageProperty.criticalAttack = flag;
				CalculateElement(damageProperty, enemyByID, true);
				enemyByID.HitEnemy(damageProperty);
			}
		}
		else if (hit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				int userID = int.Parse(hit.collider.transform.parent.name);
				bool flag2 = false;
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID != null && !remotePlayerByUserID.IsSameTeam(player))
				{
					float num3 = UnityEngine.Random.Range(0f, 1f);
					if (num3 < CriticalRate)
					{
						flag2 = true;
					}
					if (flag2)
					{
						damageProperty.damage = (int)((float)damageProperty.damage * criticalDamage);
					}
					damageProperty.criticalAttack = flag2;
					CalculateElement(damageProperty, remotePlayerByUserID, true);
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(player.GetUserID(), (short)damageProperty.damage, int.Parse(hit.collider.transform.parent.name), penetration, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, DamageProperty.AttackerType._PlayerOrEnemy);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else if (hit.collider.gameObject.layer == PhysicsLayer.SUMMONED && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(hit.collider);
			if (controllableByCollider != null)
			{
				foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
				{
					if (remotePlayer == null)
					{
						continue;
					}
					SummonedItem summonedByName = remotePlayer.GetSummonedByName(controllableByCollider.name);
					if (summonedByName != null && !summonedByName.IsSameTeam())
					{
						bool flag3 = false;
						float num4 = UnityEngine.Random.Range(0f, 1f);
						if (num4 < CriticalRate)
						{
							flag3 = true;
						}
						if (flag3)
						{
							damageProperty.damage = (int)((float)damageProperty.damage * criticalDamage);
						}
						damageProperty.criticalAttack = flag3;
						ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summonedByName.ControllableType, summonedByName.ID, damageProperty.damage);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						break;
					}
				}
			}
		}
		CheckEnemyBulletDetection(hit.point);
	}

	public void CalculateElement(DamageProperty damageProperty, GameUnit target, bool getTypePara)
	{
		if (getTypePara)
		{
			GetElementTypeAndPara();
		}
		damageProperty.elementType = mCurrentElementType;
		if (elementDotTriggerTimes > 0 && (mCurrentElementType == ElementType.Fire || mCurrentElementType == ElementType.Shock || mCurrentElementType == ElementType.Corrosive) && SCalculateElement(mCurrentElementPara, elementDotTriggerBase, elementDotTriggerScale, mCurrentElementType, damageProperty, target) && elementLastFrameCount != Time.frameCount)
		{
			elementDotTriggerTimes--;
			elementLastFrameCount = Time.frameCount;
		}
	}

	public static bool SCalculateElement(byte elementPara, float elementDotTriggerBase, float elementDotTriggerScale, ElementType elementType, DamageProperty damageProperty, GameUnit target)
	{
		float num = elementDotTriggerBase + elementDotTriggerScale * (float)(int)elementPara;
		if (num > 1f)
		{
			num = 1f;
		}
		bool flag = false;
		if (target.Shield > 0 && elementType == ElementType.Shock)
		{
			flag = true;
		}
		else if (target.Shield <= 0)
		{
			if ((elementType == ElementType.Corrosive && target.ShieldType == ShieldType.MECHANICAL) || (elementType == ElementType.Fire && target.ShieldType == ShieldType.FLESH) || (elementType == ElementType.Shock && target.ShieldType == ShieldType.FLESH) || (elementType == ElementType.Corrosive && target.ShieldType == ShieldType.FLESH))
			{
				flag = true;
			}
		}
		else
		{
			damageProperty.isTriggerDlementDot = false;
		}
		if (flag)
		{
			float num2 = UnityEngine.Random.Range(0f, 1f);
			if (num2 <= num)
			{
				damageProperty.isTriggerDlementDot = true;
				switch (damageProperty.elementType)
				{
				case ElementType.Fire:
					damageProperty.elementDotTime = ElementWeaponConfig.FireDotTime[elementPara];
					damageProperty.elementDotDamage = ElementWeaponConfig.FireDotDamage[elementPara];
					break;
				case ElementType.Shock:
					damageProperty.elementDotTime = ElementWeaponConfig.ShockDotTime[elementPara];
					damageProperty.elementDotDamage = ElementWeaponConfig.ShockDotDamage[elementPara];
					break;
				case ElementType.Corrosive:
					damageProperty.elementDotTime = ElementWeaponConfig.CorrosiveDotTime[elementPara];
					damageProperty.elementDotDamage = ElementWeaponConfig.CorrosiveDotDamage[elementPara];
					break;
				}
				return true;
			}
		}
		return false;
	}

	public void GetElementTypeAndPara()
	{
		if (mElementTypeLastFrameCount != Time.frameCount)
		{
			if (elementFirePara != 0 && elementShockPara != 0 && elementCorrosivePara != 0)
			{
				mCurrentElementType = (ElementType)UnityEngine.Random.Range(2, 5);
				mCurrentElementPara = elementFirePara;
			}
			else if (elementFirePara != 0)
			{
				mCurrentElementType = ElementType.Fire;
				mCurrentElementPara = elementFirePara;
			}
			else if (elementShockPara != 0)
			{
				mCurrentElementType = ElementType.Shock;
				mCurrentElementPara = elementShockPara;
			}
			else if (elementCorrosivePara != 0)
			{
				mCurrentElementType = ElementType.Corrosive;
				mCurrentElementPara = elementCorrosivePara;
			}
			mElementTypeLastFrameCount = Time.frameCount;
		}
	}

	public bool IsAllElement()
	{
		if (elementFirePara != 0 && elementShockPara != 0 && elementCorrosivePara != 0)
		{
			return true;
		}
		return false;
	}

	public void SetAllElementParaForRemotePlayer()
	{
		if (!player.IsLocal())
		{
			elementFirePara = 1;
			elementShockPara = 1;
			elementCorrosivePara = 1;
		}
	}

	protected void StopAchievement023Trigger()
	{
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.No_survival, AchievementTrigger.Type.Stop);
		AchievementManager.GetInstance().Trigger(trigger);
	}

	protected void PlayElementGunfire()
	{
		bool flag = false;
		switch (mCurrentElementType)
		{
		case ElementType.Fire:
			flag = true;
			if (gunfire != null && gunfireObj_Fire != null)
			{
				gunfireObj_Fire.SetActive(true);
			}
			break;
		case ElementType.Shock:
			flag = true;
			if (gunfire != null && gunfireObj_Shock != null)
			{
				gunfireObj_Shock.SetActive(true);
			}
			break;
		case ElementType.Corrosive:
			flag = true;
			if (gunfire != null && gunfireObj_Corrosive != null)
			{
				gunfireObj_Corrosive.SetActive(true);
			}
			break;
		}
		if (!flag && gunfire != null && gunfireObj != null)
		{
			gunfireObj.SetActive(true);
		}
	}

	protected void PlayElementBulletTrail(Vector3 hitPosition, Vector3 dir)
	{
		GameObject gameObject = null;
		bool flag = false;
		switch (mCurrentElementType)
		{
		case ElementType.Fire:
			flag = true;
			gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.FIRE_BULLET_TRAIL)
				.CreateObject(gunfire.position, dir, Quaternion.identity);
			break;
		case ElementType.Shock:
			flag = true;
			gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.SHOCK_BULLET_TRAIL)
				.CreateObject(gunfire.position, dir, Quaternion.identity);
			break;
		case ElementType.Corrosive:
			flag = true;
			gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.CORROSIVE_BULLET_TRAIL)
				.CreateObject(gunfire.position, dir, Quaternion.identity);
			break;
		}
		if (!flag)
		{
			gameObject = GameApp.GetInstance().GetGameScene().GetEffectPool(EffectPoolType.BULLET_TRAIL_FPS)
				.CreateObject(gunfire.position + 5f * dir, dir, Quaternion.identity);
		}
		if (!(gameObject == null))
		{
			BulletTrailScript component = gameObject.GetComponent<BulletTrailScript>();
			component.beginPos = gunfire.position + 5f * dir;
			if (player.IsLocal())
			{
				component.endPos = hitPosition - 5f * dir;
			}
			else
			{
				component.endPos = gunfire.position + dir * 100f;
			}
			component.isActive = true;
		}
	}

	protected bool PlayElementSound()
	{
		bool result = false;
		switch (mCurrentElementType)
		{
		case ElementType.Fire:
			result = true;
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/element/fire_rad", player.GetTransform().position);
			break;
		case ElementType.Shock:
			result = true;
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/element/fire_nano", player.GetTransform().position);
			break;
		case ElementType.Corrosive:
			result = true;
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/element/fire_poison", player.GetTransform().position);
			break;
		}
		return result;
	}
}
