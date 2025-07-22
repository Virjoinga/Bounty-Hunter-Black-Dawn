using UnityEngine;

public class AdvancedSword : Sword
{
	protected float blurFlySpeed = 30f;

	protected int windRnd = 100;

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AdvancedSword;
	}

	public override void MakeExtraHit(int index)
	{
		AudioManager.GetInstance().PlaySoundAt("Audio/light_sword/windblade", entityObject.transform.position);
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
		GameObject original = Resources.Load("Effect/Sword/sword_shadow") as GameObject;
		GameObject gameObject = Object.Instantiate(original, player.GetTransform().position + Vector3.up, Quaternion.LookRotation(normalized2)) as GameObject;
		int num = -45;
		if (index == 2)
		{
			num = 45;
		}
		gameObject.transform.Rotate(new Vector3(0f, 0f, num), Space.Self);
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized2;
		component.flySpeed = blurFlySpeed;
		component.explodeRadius = 3f;
		component.hitForce = hitForce;
		component.life = 8f;
		float num2 = damage;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num2 /= VSMath.SWORD_DAMAGE_BOOTH;
		}
		component.damage = (int)(num2 * 0.4f);
		component.GunType = WeaponType.AdvancedSword;
		component.isPenerating = true;
		component.targetPos = aimTarget;
		component.bagIndex = 0;
		component.weapon = this;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(21, gunfire.position, normalized2, ElementType.NoElement);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void CreateSwordTrail()
	{
		GameObject original = Resources.Load("Effect/Sword/sword_trail") as GameObject;
		GameObject gameObject = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.transform.parent = gunfire;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}
}
