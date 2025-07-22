using UnityEngine;

public class GrenadeLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 20f;

	protected static int sbulletCount;

	public override int BulletCountInGun
	{
		get
		{
			return 0;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public GrenadeLauncher()
	{
		maxTotalBullet = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.GrenadeLauncher;
	}

	public override string GetAnimationSuffix()
	{
		return "_grenade_launcher";
	}

	public override string GetReloadAnimationSuffix()
	{
		return "_shotgun";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void GunOn()
	{
		base.GunOn();
		SetWeaponShootSpeed(1f);
	}

	public override void Init(Player player)
	{
		base.Init(player);
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_fire", player.GetTransform().position);
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
	}
}
