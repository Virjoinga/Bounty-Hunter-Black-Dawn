using UnityEngine;

public class AssaultRifle : Weapon
{
	public override WeaponType GetWeaponType()
	{
		return WeaponType.AssaultRifle;
	}

	public override string GetAnimationSuffix()
	{
		return "SMG01_";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "SMG01_";
	}

	public override float GetReloadAnimationSpeed()
	{
		return 2.37f * base.ReloadTimeScale / reloadTime;
	}

	public override void Init(Player player)
	{
		base.Init(player);
		base.AttackFrequencyT = 0.25f;
		Object.DontDestroyOnLoad(gunfire);
		playSoundTimer.SetTimer(0.1f, true);
		Object original = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_Fire_001");
		gunfireObj = (GameObject)Object.Instantiate(original, gunfire.position, Quaternion.identity);
		gunfireObj.transform.parent = gunfire;
		gunfireObj.transform.localPosition = Vector3.zero;
		gunfireObj.transform.localRotation = Quaternion.identity;
		gunfireObj.transform.localScale = Vector3.one;
		Object original2 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_radiation");
		gunfireObj_Fire = (GameObject)Object.Instantiate(original2, gunfire.position, Quaternion.identity);
		gunfireObj_Fire.transform.parent = gunfire;
		gunfireObj_Fire.transform.localPosition = Vector3.zero;
		gunfireObj_Fire.transform.localRotation = Quaternion.identity;
		Object original3 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_lightning");
		gunfireObj_Shock = (GameObject)Object.Instantiate(original3, gunfire.position, Quaternion.identity);
		gunfireObj_Shock.transform.parent = gunfire;
		gunfireObj_Shock.transform.localPosition = Vector3.zero;
		gunfireObj_Shock.transform.localRotation = Quaternion.identity;
		Object original4 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_poison");
		gunfireObj_Corrosive = (GameObject)Object.Instantiate(original4, gunfire.position, Quaternion.identity);
		gunfireObj_Corrosive.transform.parent = gunfire;
		gunfireObj_Corrosive.transform.localPosition = Vector3.zero;
		gunfireObj_Corrosive.transform.localRotation = Quaternion.identity;
		StopFire();
		elementDotTriggerBase = ElementWeaponConfig.ElementDotTriggerBase[6];
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[6];
		elementDotTriggerScale = ElementWeaponConfig.ElementDotTriggerScale[6];
	}

	public override void Loop(float deltaTime)
	{
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

	public override void PlaySound()
	{
		if (playSoundTimer.Ready())
		{
			if (!PlayElementSound())
			{
				AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/assault_rifle/assault_fire", player.GetTransform().position);
			}
			playSoundTimer.Do();
		}
	}

	public override void CreateTrajectory()
	{
		base.CreateTrajectory();
		Vector3 normalized = player.GetTransform().forward.normalized;
		gunfire.transform.rotation = player.GetTransform().rotation;
		gunfire.transform.rotation = Quaternion.AngleAxis(player.AngleV, -gunfire.transform.right) * gunfire.transform.rotation;
		normalized = gunfire.transform.forward.normalized;
		if (!player.IsLocal())
		{
			GetElementTypeAndPara();
		}
		PlayElementGunfire();
		PlayElementBulletTrail(gunfire.position, normalized);
	}

	public override void Attack(float deltaTime)
	{
		base.Attack(deltaTime);
	}

	public override void Reload()
	{
		base.Reload();
		PlayAnimation("reload", WrapMode.Once, GetReloadAnimationSpeed());
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/assault_rifle/assault_reload", player.GetTransform().position);
		elementDotTriggerTimes = ElementWeaponConfig.ElementDotTriggerTime[6];
	}
}
