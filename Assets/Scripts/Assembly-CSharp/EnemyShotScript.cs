using System.Collections.Generic;
using UnityEngine;

public class EnemyShotScript : MonoBehaviour
{
	public Enemy enemy;

	public Vector3 speed;

	public float explodeRadius = 8f;

	public int attackDamage = 5;

	public int areaDamage = 5;

	public TrajectoryType trType;

	public DamageType damageType;

	public EnemyType enemyType;

	public string explodeEffect;

	public string slimePrefab;

	public float slimeDamageInterval;

	private bool destroyed;

	private void Start()
	{
	}

	private void Update()
	{
		if (trType == TrajectoryType.Parabola)
		{
			speed += Physics.gravity.y * Vector3.up * Time.deltaTime;
			base.transform.Translate(speed * Time.deltaTime, Space.World);
			base.transform.LookAt(base.transform.position + speed * 10f);
		}
		else if (trType == TrajectoryType.Straight)
		{
			base.transform.Translate(speed * Time.deltaTime, Space.World);
			base.transform.LookAt(base.transform.position + speed * 10f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		Object.DestroyObject(base.gameObject);
		destroyed = true;
		Vector3 position = base.gameObject.transform.position;
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (Mathf.Sqrt((base.transform.position - localPlayer.GetPosition()).sqrMagnitude) < 20f)
		{
			localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
		}
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (damageType == DamageType.Normal || damageType == DamageType.Sputtering || damageType == DamageType.Slime)
			{
				localPlayer.OnHit(attackDamage, enemy);
			}
		}
		else if (other.gameObject.layer == PhysicsLayer.SUMMONED && (damageType == DamageType.Normal || damageType == DamageType.Sputtering || damageType == DamageType.Slime))
		{
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (controllableByCollider != null)
			{
				SummonedItem summonedByName = localPlayer.GetSummonedByName(controllableByCollider.name);
				if (summonedByName != null)
				{
					summonedByName.OnHit(attackDamage);
				}
			}
		}
		if (damageType == DamageType.Sputtering || damageType == DamageType.Explosion)
		{
			if (damageType == DamageType.Explosion)
			{
				if (explodeEffect != null && string.Empty != explodeEffect)
				{
					GameObject original = Resources.Load(explodeEffect) as GameObject;
					Object.Instantiate(original, base.transform.position, Quaternion.identity);
				}
				AudioManager.GetInstance().PlaySoundAt("Audio/rpg/rpg-21_boom", base.transform.position);
			}
			if ((localPlayer.GetTransform().position - base.transform.position).sqrMagnitude < explodeRadius * explodeRadius)
			{
				Ray ray = new Ray(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
				float distance = Vector3.Distance(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
				{
					localPlayer.OnHit(areaDamage, enemy);
				}
			}
			Dictionary<string, SummonedItem> summonedList = localPlayer.GetSummonedList();
			{
				foreach (SummonedItem value in summonedList.Values)
				{
					if ((value.GetTransform().position - base.transform.position).sqrMagnitude < explodeRadius * explodeRadius)
					{
						Ray ray2 = new Ray(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
						float distance2 = Vector3.Distance(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
						RaycastHit hitInfo2;
						if (Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
						{
							value.OnHit(areaDamage);
						}
					}
				}
				return;
			}
		}
		if (damageType == DamageType.Slime && slimePrefab != null)
		{
			Vector3 vector = base.transform.position;
			Quaternion rotation = Quaternion.identity;
			Ray ray3 = new Ray(vector + Vector3.up * 0.5f, 100f * Vector3.down);
			int layerMask = (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.WALL);
			RaycastHit hitInfo3;
			if (Physics.Raycast(ray3, out hitInfo3, 5f, layerMask))
			{
				vector = hitInfo3.point + 0.01f * hitInfo3.normal;
				rotation = Quaternion.FromToRotation(Vector3.up, hitInfo3.normal);
			}
			GameObject original2 = Resources.Load(slimePrefab) as GameObject;
			GameObject gameObject = Object.Instantiate(original2, vector, rotation) as GameObject;
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Enemy/Spit/Spit_attack02", base.transform.position);
			EnemySlimeScript component = gameObject.GetComponent<EnemySlimeScript>();
			component.enemy = enemy;
			component.damageInterval = slimeDamageInterval;
			component.slimeDamage = areaDamage;
		}
	}
}
