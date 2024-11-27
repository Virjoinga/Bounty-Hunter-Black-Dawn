using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
	protected Timer swordTimer = new Timer();

	protected int attackPhase = -1;

	protected float lastMakeOneHitTime;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.Sword;
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override string GetAnimationSuffix()
	{
		return "_jian";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "_jian";
	}

	public override void Init(Player player)
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		gameCamera = Camera.main.GetComponent<FirstPersonCameraScript>();
		cameraComponent = gameCamera.GetComponent<Camera>();
		cameraTransform = gameCamera.CameraTransform;
		base.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.Weapon);
		CreateGun();
		entityObject.transform.parent = weaponBoneTrans;
		shootAudio = entityObject.GetComponent<AudioSource>();
		if (shootAudio == null)
		{
		}
		GunOff();
		gunfire = WeaponResourceConfig.GetWeaponGunFire(entityObject, gunID);
		swordTimer.SetTimer(0.2f, true);
	}

	public void MakeOneHit(int index)
	{
		if (!(Time.time - lastMakeOneHitTime > 0.3f))
		{
			return;
		}
		lastMakeOneHitTime = Time.time;
		Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		bool flag = false;
		foreach (Enemy value in enemies.Values)
		{
			if (value.InPlayingState())
			{
				Vector3 vector = player.GetTransform().InverseTransformPoint(value.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, value.GetTransform().position) < 3.5f && vector.z > 0f)
				{
					DamageProperty damageProperty = new DamageProperty();
					damageProperty.damage = (int)damage;
					damageProperty.wType = WeaponType.Sword;
					damageProperty.hitpoint = value.GetTransform().position + Vector3.up;
					damageProperty.criticalAttack = true;
					damageProperty.isLocal = true;
					value.HitEnemy(damageProperty);
					flag = true;
				}
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (GameApp.GetInstance().GetGameMode().IsTeamMode())
				{
					Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(item.GetUserID());
					if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(player))
					{
						continue;
					}
				}
				Vector3 vector2 = player.GetTransform().InverseTransformPoint(item.GetTransform().position);
				if (Vector3.Distance(player.GetTransform().position, item.GetTransform().position) < 3.5f && vector2.z > 0f)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			int num = Random.Range(1, 4);
			AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/light_sword0" + num, entityObject.transform.position);
		}
		else
		{
			AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/light_sword_swing0" + index, entityObject.transform.position);
		}
		MakeExtraHit(index);
	}

	public virtual void MakeExtraHit(int index)
	{
	}

	protected virtual void CreateSwordTrail()
	{
	}

	public override void Loop(float deltaTime)
	{
		NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
		Transform transform = player.GetTransform();
		if (attackPhase == -1 && ((player.IsPlayingAnimation(AnimationString.Attack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.RunAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.FlyAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.2f)) || (player.IsPlayingAnimation(AnimationString.FlyRunAttack + GetAnimationSuffix()) && !player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.2f))))
		{
			attackPhase = 0;
		}
		else if (attackPhase == 0 && (player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.2f) || player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.2f)))
		{
			if (player.IsLocal())
			{
				MakeOneHit(1);
			}
			CreateSwordTrail();
			attackPhase = 1;
		}
		else if (attackPhase == 1 && (player.AnimationPlayed(AnimationString.Attack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.RunAttack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.FlyAttack + GetAnimationSuffix(), 0.5f) || player.AnimationPlayed(AnimationString.FlyRunAttack + GetAnimationSuffix(), 0.5f)))
		{
			if (player.IsLocal())
			{
				MakeOneHit(2);
			}
			CreateSwordTrail();
			attackPhase = -1;
		}
	}

	public override void Attack(float deltaTime)
	{
		GameObject original = Resources.Load("Effect/Sword/sword_sfx") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = gunfire;
		attacked = false;
		lastAttackTime = Time.time;
	}

	public override void StopFire()
	{
		if (shootAudio != null)
		{
			shootAudio.Stop();
		}
	}

	public override void GunOn()
	{
		base.GunOn();
		player.SetLowerBodyAnimation(GetAnimationSuffix(), 0);
	}

	public override void GunOff()
	{
		base.GunOff();
		player.SetLowerBodyAnimation(GetAnimationSuffix(), -1);
	}
}
