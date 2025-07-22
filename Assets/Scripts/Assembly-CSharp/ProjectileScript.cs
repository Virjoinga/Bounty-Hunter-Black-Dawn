using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TPS/ProjectileScript")]
public class ProjectileScript : MonoBehaviour
{
	protected GameObject resObject;

	protected GameObject explodeObject;

	protected GameObject smallExplodeObject;

	protected GameObject laserHitObject;

	public Transform targetTransform;

	public Vector3 targetPos;

	protected Transform proTransform;

	protected WeaponType gunType;

	public Vector3 dir;

	public float hitForce;

	public float explodeRadius;

	public float flySpeed;

	public Vector3 speed;

	public float life = 2f;

	public int damage;

	public bool isLocal = true;

	public byte bagIndex;

	public bool isPenerating;

	public Weapon weapon;

	public ElementType elementType;

	protected float createdTime;

	protected float lastTriggerTime;

	protected float gravity = 16f;

	protected float downSpeed;

	protected float deltaTime;

	public DamageProperty.AttackerType attackerType;

	public int level;

	protected float initAngel = 50f;

	protected HashSet<string> penetratingTargets = new HashSet<string>();

	public bool destroyed;

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
		if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || !GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			return;
		}
		if (gunType == WeaponType.RPG || gunType == WeaponType.RocketLauncher || gunType == WeaponType.AutoRocketLauncher)
		{
			flySpeed *= VSMath.RPG_FLY_BOOTH;
		}
		if (gunType == WeaponType.LightFist)
		{
			ScaleScript component = base.transform.GetComponent<ScaleScript>();
			if (component != null)
			{
				component.scaleSpeed *= VSMath.GLOVE_GROW_SPPED_BOOTH;
			}
		}
	}

	public void Update()
	{
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
			if (Time.time - createdTime > life)
			{
				UnityEngine.Object.DestroyObject(base.gameObject);
			}
			deltaTime = 0f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		if (gunType == WeaponType.LightFist)
		{
			explodeRadius = base.transform.localScale.x;
			explodeRadius = Mathf.Clamp(explodeRadius, 5f, 10f);
		}
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int num = 0;
		if (explodeRadius < 7f)
		{
			num = 0;
		}
		else if (7f <= explodeRadius && explodeRadius < 9f)
		{
			num = 1;
		}
		else
		{
			num = 2;
		}
		string empty = string.Empty;
		string text = "RPG_Audio/Weapon/rpg/rpg_boom";
		if (isLocal && weapon != null)
		{
			text = weapon.ExplodeSoundName;
		}
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
		if (gunType == WeaponType.AdvancedSword)
		{
			GameObject original = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, proTransform.position, Quaternion.identity) as GameObject;
		}
		else
		{
			GameObject original2 = Resources.Load(empty) as GameObject;
			UnityEngine.Object.Instantiate(original2, proTransform.position, Quaternion.identity);
			LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (Mathf.Sqrt((base.transform.position - localPlayer2.GetPosition()).sqrMagnitude) < 5f)
			{
				localPlayer2.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
			}
		}
		if (isPenerating)
		{
			if (other.gameObject.layer == PhysicsLayer.WALL || other.gameObject.layer == PhysicsLayer.TRANSPARENT_WALL || other.gameObject.layer == PhysicsLayer.FLOOR)
			{
				UnityEngine.Object.DestroyObject(base.gameObject);
				destroyed = true;
			}
		}
		else
		{
			UnityEngine.Object.DestroyObject(base.gameObject);
			destroyed = true;
		}
		if (isLocal)
		{
			bool criticalAttack = false;
			float num2 = UnityEngine.Random.Range(0f, 1f);
			if (num2 < localPlayer.GetWeapon().CriticalRate)
			{
				criticalAttack = true;
			}
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.damage = damage;
			damageProperty.criticalAttack = criticalAttack;
			damageProperty.isLocal = isLocal;
			damageProperty.wType = gunType;
			damageProperty.isPenetration = false;
			damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
			damageProperty.weaponLevel = level;
			damageProperty.attackerType = attackerType;
			if (isPenerating)
			{
				GameObject enemyByCollider = Enemy.GetEnemyByCollider(other);
				if (enemyByCollider.name.StartsWith("E_"))
				{
					Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
					if (enemyByID != null && !penetratingTargets.Contains(enemyByID.Name))
					{
						damageProperty.hitpoint = enemyByID.GetTransform().position + Vector3.up * 2f;
						weapon.CalculateElement(damageProperty, enemyByID, false);
						enemyByID.HitEnemy(damageProperty);
						penetratingTargets.Add(enemyByID.Name);
					}
				}
				DoVSDamage();
				return;
			}
			if (weapon != null)
			{
				AudioManager.GetInstance().PlaySoundAt(text, base.transform.position);
			}
			int num3 = 0;
			Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, 1 << PhysicsLayer.ENEMY);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Ray ray = new Ray(base.transform.position, collider.transform.position - base.transform.position);
				float num4 = Mathf.Sqrt((base.transform.position - collider.transform.position).sqrMagnitude);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, num4, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
				{
					continue;
				}
				GameObject enemyByCollider2 = Enemy.GetEnemyByCollider(collider);
				if (enemyByCollider2.name.StartsWith("E_"))
				{
					Enemy enemyByID2 = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider2.name);
					if (enemyByID2 != null && enemyByID2.InPlayingState())
					{
						num3++;
						damageProperty.hitpoint = enemyByID2.GetTransform().position + Vector3.up * 2f;
						damageProperty.damage = Mathf.CeilToInt((float)damage * (1f - 2f * num4 / (3f * explodeRadius)));
						weapon.CalculateElement(damageProperty, enemyByID2, false);
						enemyByID2.HitEnemy(damageProperty);
					}
				}
			}
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Yakitoui, AchievementTrigger.Type.Data);
			achievementTrigger.PutData(num3);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			if (num3 == 0)
			{
				if (GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Boss)
				{
					AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Start);
					AchievementManager.GetInstance().Trigger(trigger);
					AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Data);
					achievementTrigger2.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger2);
				}
			}
			else
			{
				AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Panic_Attack, AchievementTrigger.Type.Stop);
				AchievementManager.GetInstance().Trigger(trigger2);
			}
			DoVSDamage();
		}
		else if (gunType == WeaponType.RPG)
		{
			AudioManager.GetInstance().PlaySoundAt(text, base.transform.position);
		}
	}

	protected void DoVSDamage()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || !GameApp.GetInstance().GetGameMode().IsVSMode() || !isLocal)
		{
			return;
		}
		bool criticalAttack = false;
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num < localPlayer.GetWeapon().CriticalRate)
		{
			criticalAttack = true;
		}
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.damage = damage;
		damageProperty.criticalAttack = criticalAttack;
		damageProperty.isLocal = isLocal;
		damageProperty.wType = gunType;
		damageProperty.isPenetration = false;
		damageProperty.unitLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		damageProperty.weaponLevel = level;
		damageProperty.attackerType = attackerType;
		Collider[] array = Physics.OverlapSphere(proTransform.position, explodeRadius, 1 << PhysicsLayer.REMOTE_PLAYER);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Ray ray = new Ray(base.transform.position, collider.transform.position - base.transform.position);
			float num2 = Mathf.Sqrt((base.transform.position - collider.transform.position).sqrMagnitude);
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, num2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
			{
				GameObject playerByCollider = Player.GetPlayerByCollider(collider);
				int userID = Convert.ToInt32(playerByCollider.name);
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(userID);
				if (remotePlayerByUserID != null && !remotePlayerByUserID.IsSameTeam(localPlayer) && remotePlayerByUserID.InPlayingState())
				{
					weapon.CalculateElement(damageProperty, remotePlayerByUserID, false);
					penetratingTargets.Add(remotePlayerByUserID.GetUserID().ToString());
					damageProperty.damage = Mathf.CeilToInt((float)damageProperty.damage * (1f - 2f * Mathf.Sqrt(num2) / (3f * explodeRadius)));
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(localPlayer.GetUserID(), (short)damage, remotePlayerByUserID.GetUserID(), damageProperty.isPenetration, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, DamageProperty.AttackerType._PlayerOrEnemy);
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
				Vector3 vector = summoned.Value.GetTransform().position + new Vector3(0f, 1f, 0f);
				float sqrMagnitude = (vector - proTransform.position).sqrMagnitude;
				float num3 = explodeRadius * explodeRadius;
				if (sqrMagnitude < num3)
				{
					Ray ray2 = new Ray(proTransform.position, vector - proTransform.position);
					RaycastHit hitInfo2;
					if (Physics.Raycast(ray2, out hitInfo2, explodeRadius, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
					{
						damageProperty.damage = Mathf.CeilToInt((float)damageProperty.damage * (1f - 2f * Mathf.Sqrt(sqrMagnitude) / (3f * explodeRadius)));
						ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summoned.Value.ControllableType, summoned.Value.ID, damageProperty.damage);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
		}
	}

	protected void PenetrateLogic(Enemy enemy)
	{
	}
}
