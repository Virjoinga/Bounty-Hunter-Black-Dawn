using System;
using UnityEngine;

public class LaserCannon : Weapon
{
	protected float flySpeed;

	protected static float sbulletCount;

	private GameObject laserObj;

	protected Vector3 laserStartScale;

	protected float lastLaserHitInitiatTime;

	protected float startFireTime;

	protected float chargeEnegy = 100f;

	protected bool overHeat;

	protected GameObject chargeObj;

	protected float lastChargeTime;

	public bool IsOverHeat
	{
		get
		{
			return overHeat;
		}
	}

	public float GetChargeEnegy()
	{
		return chargeEnegy;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		attackFrenquency = 0.2f;
		UnityEngine.Object original = Resources.Load("Effect/JIGUANG_fire");
		UnityEngine.Object original2 = Resources.Load("Effect/GunFire_LaserCannon2");
		gunfireObj = (GameObject)UnityEngine.Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gunfireObj.transform.parent = gunfire;
		chargeObj = (GameObject)UnityEngine.Object.Instantiate(original2, gunfire.position, Quaternion.identity);
		chargeObj.transform.parent = gunfire;
		waitForAttackAnimationStop = false;
		StopFire();
	}

	public override string GetAnimationSuffix()
	{
		return "_laser";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "_rifle";
	}

	public void SetShootTimeNow()
	{
		lastAttackTime = Time.time;
	}

	public override bool CoolDown()
	{
		if (Time.time - lastAttackTime > attackFrenquency && !overHeat)
		{
			return true;
		}
		if (player.IsLocal() && laserObj == null)
		{
			Charge();
			player.SetState(Player.IDLE_STATE);
			player.StopAnimation(AnimationString.RunAttack + GetAnimationSuffix());
		}
		return false;
	}

	public override void Loop(float deltaTime)
	{
		if (!(laserObj != null))
		{
			return;
		}
		AudioManager.GetInstance().PlaySoundSingle("Audio/lasergun/laser_fire");
		float num = 10f;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num = VSMath.CHARGE_CONSUME_FOR_LASERCANNON;
		}
		chargeEnegy -= num * Time.deltaTime;
		chargeEnegy = Mathf.Clamp(chargeEnegy, 0f, 100f);
		if (chargeEnegy == 0f)
		{
			overHeat = true;
		}
		if (player.IsLocal())
		{
			if (overHeat)
			{
				StopFire();
				return;
			}
			lastChargeTime = Time.time;
			GameApp.GetInstance().GetUserState().UseEnegy((int)((float)(enegyConsume * 5) * deltaTime));
			Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
			Vector3 normalized = (vector - cameraTransform.position).normalized;
			Ray ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR)))
			{
				aimTarget = hitInfo.point;
			}
			else
			{
				aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
			}
			Vector3 normalized2 = (aimTarget - gunfire.transform.position).normalized;
			float magnitude = (aimTarget - gunfire.transform.position).magnitude;
			laserObj.transform.position = gunfire.transform.position;
			laserObj.transform.LookAt(aimTarget);
			if (hitInfo.collider != null)
			{
				laserObj.transform.localScale = new Vector3(laserObj.transform.localScale.x, laserObj.transform.localScale.y, magnitude / 49f);
			}
			if (Time.time - lastLaserHitInitiatTime > 0.03f && (aimTarget - normalized2 - cameraTransform.position).sqrMagnitude > 9f)
			{
				lastLaserHitInitiatTime = Time.time;
			}
			GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, aimTarget - normalized2, Quaternion.identity) as GameObject;
		}
		else
		{
			float angle = base.Adjuster.angleOffsetH * ((float)Math.PI / 180f);
			player.GetTransform().RotateAround(-Vector3.up, angle);
			Vector3 normalized3 = player.GetTransform().forward.normalized;
			gunfire.transform.rotation = player.GetTransform().rotation;
			gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
			normalized3 = gunfire.transform.forward.normalized;
			Ray ray2 = new Ray(gunfire.transform.position, normalized3);
			RaycastHit hitInfo2;
			if (Physics.Raycast(ray2, out hitInfo2, 1000f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR)))
			{
				aimTarget = hitInfo2.point;
			}
			else
			{
				aimTarget = gunfire.transform.position + normalized3 * 200f;
			}
			float magnitude2 = (aimTarget - gunfire.transform.position).magnitude;
			laserObj.transform.position = gunfire.transform.position;
			laserObj.transform.LookAt(aimTarget);
			if (hitInfo2.collider != null)
			{
				laserObj.transform.localScale = new Vector3(laserObj.transform.localScale.x, laserObj.transform.localScale.y, magnitude2 / 49f);
			}
			GameObject original2 = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original2, aimTarget - normalized3, Quaternion.identity) as GameObject;
			player.GetTransform().RotateAround(Vector3.up, angle);
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		if (Time.time - startFireTime < 1f)
		{
			EnableChargeEffect(true);
			AudioManager.GetInstance().PlaySoundSingle("Audio/lasergun/laser_charge");
			Charge();
			return;
		}
		if (gunfireObj != null)
		{
			gunfireObj.SetActive(true);
		}
		Vector3 normalized = player.GetTransform().forward.normalized;
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		if (laserObj == null)
		{
			GameObject original = Resources.Load("Effect/Laser") as GameObject;
			laserObj = UnityEngine.Object.Instantiate(original, gunfire.transform.position, Quaternion.LookRotation(normalized)) as GameObject;
			laserStartScale = laserObj.transform.localScale;
			LaserShotScript component = laserObj.GetComponent<LaserShotScript>();
			component.isLocal = false;
		}
	}

	public void EnableChargeEffect(bool enable)
	{
		if (chargeObj != null)
		{
			if (enable)
			{
				chargeObj.SetActive(true);
			}
			else
			{
				chargeObj.SetActive(false);
			}
		}
	}

	public override void Attack(float deltaTime)
	{
		if (Time.time - startFireTime < 1f)
		{
			EnableChargeEffect(true);
			AudioManager.GetInstance().PlaySoundSingle("Audio/lasergun/laser_charge");
			Charge();
			return;
		}
		EnableChargeEffect(false);
		if (gunfireObj != null)
		{
			gunfireObj.SetActive(true);
		}
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		Ray ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		Vector3 normalized2 = (aimTarget - gunfire.transform.position).normalized;
		if (laserObj == null)
		{
			GameObject original = Resources.Load("Effect/Laser") as GameObject;
			laserObj = UnityEngine.Object.Instantiate(original, gunfire.transform.position, Quaternion.LookRotation(normalized2)) as GameObject;
			laserStartScale = laserObj.transform.localScale;
			LaserShotScript component = laserObj.GetComponent<LaserShotScript>();
			component.dir = normalized2;
			component.flySpeed = flySpeed;
			component.explodeRadius = 0f;
			component.hitForce = hitForce;
			component.life = 8f;
			component.damage = (int)damage;
			component.GunType = WeaponType.LaserGun;
			component.weaponBagIndex = 0;
			lastAttackTime = Time.time;
		}
	}

	public void Charge()
	{
		chargeEnegy += (Time.time - lastChargeTime) * 20f;
		lastChargeTime = Time.time;
		chargeEnegy = Mathf.Clamp(chargeEnegy, 0f, 100f);
		if (overHeat && chargeEnegy >= 100f)
		{
			overHeat = false;
		}
	}

	public override void StopFire()
	{
		if (laserObj != null)
		{
			UnityEngine.Object.Destroy(laserObj);
			laserObj = null;
		}
		if (gunfireObj != null)
		{
			gunfireObj.SetActive(false);
		}
		AudioManager.GetInstance().StopSound("laser_fire");
		startFireTime = Time.time;
		Charge();
		EnableChargeEffect(false);
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.LaserGun;
	}

	public override void GunOff()
	{
		base.GunOff();
	}
}
