using UnityEngine;

public class RocketLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 20f;

	protected static int sbulletCount;

	protected AlphaAnimationScript aas;

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

	public RocketLauncher()
	{
		mCurrentElementType = ElementType.Explosion;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.RPG;
	}

	public override string GetAnimationSuffix()
	{
		return "rpg01_";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "rpg01_";
	}

	public override float GetReloadAnimationSpeed()
	{
		return 3.53f * base.ReloadTimeScale / reloadTime;
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		base.AttackFrequencyT = 0.5f;
		SetWeaponShootSpeed(1f);
		aas = entityObject.GetComponent<AlphaAnimationScript>();
		base.FireSoundName = "RPG_Audio/Weapon/rpg/rpg_fire";
		base.ExplodeSoundName = "RPG_Audio/Weapon/rpg/rpg_boom";
		elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[3];
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[3];
		elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[3];
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		PlaySound();
	}

	public override void Attack(float deltaTime)
	{
		UseBullet(1);
		lastAttackTime = Time.time;
		PlaySound();
		CheckEnemySoundDetection();
		cameraComponent = Camera.main;
		cameraTransform = cameraComponent.transform;
		gameCamera = Camera.main.GetComponent<FirstPersonCameraScript>();
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		if (aas != null)
		{
			aas.StartAnimation();
		}
		Ray ray = default(Ray);
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		string path = "RPG_effect/RPG_RPG01_Projectile";
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized2;
		component.flySpeed = rocketFlySpeed;
		component.explodeRadius = bombRange;
		component.hitForce = hitForce;
		component.life = 12f;
		component.damage = (int)damage;
		component.GunType = WeaponType.RPG;
		component.targetPos = aimTarget;
		component.bagIndex = 0;
		component.weapon = this;
		GetElementTypeAndPara();
		component.elementType = mCurrentElementType;
		component.level = level;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(7, gunfire.position, normalized2, mCurrentElementType);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		SetRecoilToCamera();
	}

	public override void StopFire()
	{
	}

	public override void Reload()
	{
		base.Reload();
		PlayAnimation("reload", WrapMode.Once, GetReloadAnimationSpeed());
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/rpg/rpg_reload", player.GetTransform().position);
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[3];
	}

	public override void PlaySound()
	{
		AudioManager.GetInstance().PlaySoundAt(base.FireSoundName, player.GetTransform().position);
	}
}
