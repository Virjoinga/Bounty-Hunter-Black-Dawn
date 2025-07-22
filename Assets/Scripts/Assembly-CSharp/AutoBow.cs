using UnityEngine;

public class AutoBow : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 22f;

	protected static int sbulletCount;

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

	public AutoBow()
	{
		maxTotalBullet = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AutoBow;
	}

	public override string GetAnimationSuffix()
	{
		return "_bow";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "_bow";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void Init(Player player)
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		gameCamera = Camera.main.GetComponent<FirstPersonCameraScript>();
		cameraComponent = gameCamera.GetComponent<Camera>();
		cameraTransform = gameCamera.CameraTransform;
		base.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.WeaponL);
		CreateGun();
		entityObject.transform.parent = weaponBoneTrans;
		shootAudio = entityObject.GetComponent<AudioSource>();
		if (shootAudio == null)
		{
		}
		GunOff();
		gunfire = WeaponResourceConfig.GetWeaponGunFire(entityObject, gunID);
		SetWeaponShootSpeed(1.4f);
	}

	public override void Loop(float deltaTime)
	{
		if (!player.IsLocal())
		{
			return;
		}
		string attack = AnimationString.Attack;
		if (attacked || (!player.AnimationPlayed(attack + GetAnimationSuffix(), 0.5f) && !player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.5f)))
		{
			return;
		}
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
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
		AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/lightbow_shot", player.GetTransform().position);
		for (int i = 0; i < 3; i++)
		{
			Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
			GameObject original = Resources.Load("Effect/Trinity/Bow_Shot") as GameObject;
			GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
			gameObject.transform.rotation = Quaternion.AngleAxis(5f - 5f * (float)i, Vector3.up) * gameObject.transform.rotation;
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = gameObject.transform.forward;
			component.flySpeed = rocketFlySpeed;
			component.explodeRadius = bombRange;
			component.hitForce = hitForce;
			component.life = 8f;
			component.damage = (int)damage;
			component.GunType = WeaponType.AutoBow;
			component.targetPos = aimTarget;
			component.bagIndex = 0;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(18, gunfire.position, component.dir, ElementType.NoElement);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		attacked = true;
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		attacked = false;
	}

	public override void StopFire()
	{
	}
}
