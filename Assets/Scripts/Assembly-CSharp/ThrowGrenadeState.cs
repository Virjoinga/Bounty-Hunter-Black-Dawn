using UnityEngine;

public class ThrowGrenadeState : PlayerState
{
	private bool throwed;

	public override void NextState(Player player, float deltaTime)
	{
		bool inAimState = player.InAimState;
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		if (player.IsLocal())
		{
			if (inAimState)
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomIn(deltaTime);
			}
			else
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomOut(deltaTime);
			}
			player.Move(inputController.inputInfo.moveDirection);
		}
		if (player.IsPlayingAnimation(AnimationString.ThrowGrenade) && !player.AnimationPlayed(AnimationString.ThrowGrenade, 0.2f))
		{
			throwed = false;
		}
		if (!throwed && player.IsPlayingAnimation(AnimationString.ThrowGrenade) && player.AnimationPlayed(AnimationString.ThrowGrenade, 0.33f))
		{
			if (player.HandGrenade != null)
			{
				player.GetUserState().AddBulletByWeaponType(WeaponType.Grenade, -1);
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.The_Arms_Dealer, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
				Ray ray = default(Ray);
				FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
				Vector3 vector = component.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 50f));
				Vector3 normalized = (vector - component.CameraTransform.position).normalized;
				ray = new Ray(component.CameraTransform.position + normalized * 1.8f, normalized);
				Vector3 vector2 = default(Vector3);
				RaycastHit hitInfo;
				vector2 = ((!Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER))) ? component.CameraTransform.TransformPoint(0f, 0f, 1000f) : hitInfo.point);
				Transform weaponGunFire = WeaponResourceConfig.GetWeaponGunFire(player.GetWeapon().GetObject(), 0);
				Vector3 normalized2 = (vector2 - weaponGunFire.position).normalized;
				normalized2.y += 0.5f;
				GameObject original = Resources.Load("Effect/GrenadeShot") as GameObject;
				GameObject gameObject = Object.Instantiate(original, player.GetWeapon().GetObject().transform.position, Quaternion.LookRotation(normalized2)) as GameObject;
				GrenadePhysicsScript component2 = gameObject.GetComponent<GrenadePhysicsScript>();
				GrenadeShotScript component3 = gameObject.GetComponent<GrenadeShotScript>();
				component2.dir = normalized2;
				component2.life = 3f;
				component3.dir = normalized2;
				component3.flySpeed = 65f;
				component3.explodeRadius = player.HandGrenade.ExplosionRange;
				component3.life = 3f;
				component3.damage = player.HandGrenade.Damage;
				component3.GunType = WeaponType.Grenade;
				component3.targetPos = vector2;
				component3.elementType = player.HandGrenade.elementType;
				component3.elementPara = player.HandGrenade.elementPara;
				component3.elementDotTriggerBase = player.HandGrenade.elementDotTriggerBase;
				component3.elementDotTriggerScale = player.HandGrenade.elementDotTriggerScale;
				component3.level = player.HandGrenade.Level;
				component3.criticalRate = player.HandGrenade.CriticalRate;
				component3.criticalDamageRate = player.HandGrenade.CriticalDamage;
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					PlayerFireRocketRequest request = new PlayerFireRocketRequest(8, weaponGunFire.position, normalized2, component3.elementType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			throwed = true;
		}
		if (player.IsPlayingAnimation(AnimationString.ThrowGrenade) && player.AnimationPlayed(AnimationString.ThrowGrenade, 1f))
		{
			player.SetState(Player.IDLE_STATE);
		}
	}
}
