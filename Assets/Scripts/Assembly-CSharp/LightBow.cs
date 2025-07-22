using UnityEngine;

public class LightBow : Weapon
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

	public LightBow()
	{
		maxTotalBullet = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.LightBow;
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

	public override void GunOn()
	{
		entityObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
		entityObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
		entityObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
	}

	public override void GunOff()
	{
		if (entityObject != null)
		{
			entityObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
			entityObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
			entityObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
		}
		StopFire();
	}

	public override void Loop(float deltaTime)
	{
		if (!player.IsLocal())
		{
			return;
		}
		string attack = AnimationString.Attack;
		if (!attacked && (player.AnimationPlayed(attack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.5f)))
		{
			AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/lightbow_shot", player.GetTransform().position);
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
			Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
			GameObject original = Resources.Load("Effect/LightBow_Shot") as GameObject;
			GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
			ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
			component.dir = normalized2;
			component.flySpeed = rocketFlySpeed;
			component.explodeRadius = bombRange;
			component.hitForce = hitForce;
			component.life = 8f;
			component.damage = (int)damage;
			component.GunType = WeaponType.LightBow;
			component.targetPos = aimTarget;
			component.bagIndex = 0;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(14, gunfire.position, normalized2, ElementType.NoElement);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			attacked = true;
		}
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
