using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/GrenadeShotScript")]
public class GrenadeShotScript : MonoBehaviour
{
	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	protected WeaponType gunType;

	public Vector3 dir;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	protected float createdTime;

	public ElementType elementType;

	public byte elementPara;

	public float elementDotTriggerBase;

	public float elementDotTriggerScale;

	public int level;

	public float criticalRate;

	public float criticalDamageRate = 1.5f;

	public WeaponType GunType
	{
		set
		{
			gunType = value;
		}
	}

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
	}

	public void Update()
	{
		if (Time.time - createdTime >= life)
		{
			Explode();
		}
	}

	private void Explode()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		string empty = string.Empty;
		string text = "RPG_Audio/Weapon/rpg/rpg_boom";
		switch (elementType)
		{
		case ElementType.Fire:
			empty = "RPG_effect/RPG_explosion_fire_001";
			break;
		case ElementType.Shock:
			empty = "RPG_effect/RPG_explosion_elc_001";
			text = "RPG_Audio/Weapon/element/nano_explode";
			break;
		case ElementType.Corrosive:
			empty = "RPG_effect/RPG_explosion_poision_001";
			text = "RPG_Audio/Weapon/element/poison_explode";
			break;
		default:
			empty = "RPG_effect/RPG_explosion_stander_001";
			break;
		}
		GameObject original = Resources.Load(empty) as GameObject;
		Object.Instantiate(original, proTransform.position, Quaternion.identity);
		if (Mathf.Sqrt((base.transform.position - localPlayer.GetPosition()).sqrMagnitude) < 5f)
		{
			localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
		}
		Object.DestroyObject(base.gameObject);
		AudioManager.GetInstance().PlaySoundAt(text, base.transform.position);
		if (isLocal)
		{
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.damage = damage;
			damageProperty.isLocal = isLocal;
			damageProperty.isPenetration = false;
			damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
			damageProperty.weaponLevel = level;
			bool flag = false;
			float num = Random.Range(0f, 1f);
			if (num < criticalRate)
			{
				flag = true;
			}
			if (flag)
			{
				damageProperty.damage = (int)((float)damageProperty.damage * criticalDamageRate);
			}
			damageProperty.criticalAttack = flag;
			damageProperty.wType = gunType;
			Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, 1 << PhysicsLayer.ENEMY);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Ray ray = new Ray(base.transform.position + Vector3.up * 0.5f, collider.transform.position - (base.transform.position + Vector3.up * 0.5f));
				float num2 = Mathf.Sqrt((base.transform.position - collider.transform.position).sqrMagnitude);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, num2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY)) && hitInfo.collider.gameObject.layer == PhysicsLayer.ENEMY)
				{
					GameObject enemyByCollider = Enemy.GetEnemyByCollider(collider);
					if (enemyByCollider.name.StartsWith("E_"))
					{
						Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
						damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up;
						damageProperty.damage = Mathf.CeilToInt((float)damage * (1f - 2f * num2 / (3f * explodeRadius)));
						damageProperty.elementType = elementType;
						Weapon.SCalculateElement(elementPara, elementDotTriggerBase, elementDotTriggerScale, elementType, damageProperty, enemyByID);
						enemyByID.HitEnemy(damageProperty);
					}
				}
			}
			Player localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer2.GetCharacterSkillManager().OnHitFriendsTrigger(localPlayer2, damageProperty.wType, proTransform.position);
		}
		if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || !GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
		DamageProperty damageProperty2 = new DamageProperty();
		damageProperty2.damage = damage;
		damageProperty2.isLocal = isLocal;
		damageProperty2.isPenetration = false;
		damageProperty2.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		damageProperty2.weaponLevel = level;
		bool flag2 = false;
		float num3 = Random.Range(0f, 1f);
		if (num3 < criticalRate)
		{
			flag2 = true;
		}
		if (flag2)
		{
			damageProperty2.damage = (int)((float)damageProperty2.damage * criticalDamageRate);
		}
		damageProperty2.criticalAttack = flag2;
		damageProperty2.wType = gunType;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				Player remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(item.GetUserID());
				if (remotePlayerByUserID == null || remotePlayerByUserID.IsSameTeam(localPlayer))
				{
					continue;
				}
			}
			Vector3 vector = item.GetTransform().position + new Vector3(0f, 1f, 0f);
			float sqrMagnitude = (vector - proTransform.position).sqrMagnitude;
			float num4 = explodeRadius * explodeRadius;
			if (sqrMagnitude < num4)
			{
				Ray ray2 = new Ray(proTransform.position, vector - proTransform.position);
				RaycastHit hitInfo2;
				if (Physics.Raycast(ray2, out hitInfo2, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
				{
					damageProperty2.damage = Mathf.CeilToInt((float)damageProperty2.damage * (1f - 2f * Mathf.Sqrt(sqrMagnitude) / (3f * explodeRadius)));
					damageProperty2.elementType = elementType;
					Weapon.SCalculateElement(elementPara, elementDotTriggerBase, elementDotTriggerScale, elementType, damageProperty2, item);
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(localPlayer.GetUserID(), (short)damageProperty2.damage, item.GetUserID(), damageProperty2.isPenetration, (byte)damageProperty2.elementType, damageProperty2.criticalAttack, damageProperty2.isTriggerDlementDot, damageProperty2.elementDotDamage, damageProperty2.elementDotTime, damageProperty2.wType, DamageProperty.AttackerType._PlayerOrEnemy);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
		{
			if (remotePlayer == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, SummonedItem> summoned in remotePlayer.GetSummonedList())
			{
				if (summoned.Value == null || !summoned.Value.InPlayingState() || summoned.Value.IsSameTeam())
				{
					continue;
				}
				Vector3 vector2 = summoned.Value.GetTransform().position + new Vector3(0f, 1f, 0f);
				float sqrMagnitude2 = (vector2 - proTransform.position).sqrMagnitude;
				float num5 = explodeRadius * explodeRadius;
				if (sqrMagnitude2 < num5)
				{
					Ray ray3 = new Ray(proTransform.position, vector2 - proTransform.position);
					RaycastHit hitInfo3;
					if (Physics.Raycast(ray3, out hitInfo3, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.SUMMONED)) && hitInfo3.collider.gameObject.layer == PhysicsLayer.SUMMONED)
					{
						damageProperty2.damage = Mathf.CeilToInt((float)damageProperty2.damage * (1f - 2f * Mathf.Sqrt(sqrMagnitude2) / (3f * explodeRadius)));
						ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summoned.Value.ControllableType, summoned.Value.ID, damageProperty2.damage);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
		}
	}
}
