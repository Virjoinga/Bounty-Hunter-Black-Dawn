using UnityEngine;

public class ThrowGrenadeSkillState : PlayerState
{
	private bool throwed;

	private CharacterMakeDamageSkill skill;

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
		if (!throwed && player.IsPlayingAnimation(AnimationString.ThrowGrenade) && player.AnimationPlayed(AnimationString.ThrowGrenade, 0.33f))
		{
			float num = CharacterSkillManager.CalculateExplosionSkillDamage();
			num *= skill.EffectValueX;
			num += skill.EffectValueY;
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
			GameObject gameObject = Object.Instantiate(original, weaponGunFire.position, Quaternion.LookRotation(normalized2)) as GameObject;
			GrenadePhysicsScript component2 = gameObject.GetComponent<GrenadePhysicsScript>();
			GrenadeShotScript component3 = gameObject.GetComponent<GrenadeShotScript>();
			component2.dir = normalized2;
			component2.life = 3f;
			component3.dir = normalized2;
			component3.flySpeed = 50f;
			component3.explodeRadius = skill.Range;
			component3.life = 3f;
			component3.damage = (int)num;
			component3.GunType = WeaponType.Grenade;
			component3.targetPos = vector2;
			component3.level = GameApp.GetInstance().GetUserState().GetCharLevel();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerFireRocketRequest request = new PlayerFireRocketRequest(8, weaponGunFire.position, normalized2, ElementType.NoElement);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			throwed = true;
		}
		if (player.IsPlayingAnimation(AnimationString.ThrowGrenade) && player.AnimationPlayed(AnimationString.ThrowGrenade, 1f))
		{
			player.SetState(Player.IDLE_STATE);
		}
	}

	public void InitThrow(CharacterMakeDamageSkill _skill)
	{
		throwed = false;
		skill = _skill;
	}
}
