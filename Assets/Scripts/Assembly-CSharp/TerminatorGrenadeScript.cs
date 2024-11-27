using System.Collections.Generic;
using UnityEngine;

public class TerminatorGrenadeScript : MonoBehaviour
{
	public Enemy enemy;

	public float explodeTime;

	public Vector3 dir;

	public float explodeRadius;

	public int explodeDamage;

	public float forceValue;

	public float enableCollisionTime;

	private Transform proTransform;

	private float createdTime;

	private bool collisionEnabled;

	private bool flashEnabled;

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
		collisionEnabled = false;
		flashEnabled = false;
		base.GetComponent<Rigidbody>().AddForce(dir * forceValue, ForceMode.Impulse);
	}

	public void Update()
	{
		float num = Time.time - createdTime;
		if (!collisionEnabled && num > enableCollisionTime)
		{
			collisionEnabled = true;
			base.GetComponent<Collider>().enabled = true;
		}
		if (num > explodeTime)
		{
			Explode();
			Object.DestroyObject(base.gameObject);
		}
		else if (num > explodeTime - 1f)
		{
			AudioManager.GetInstance().PlaySoundSingleAt("RPG_Audio/Enemy/TMX/TMX_grenade03", base.gameObject, base.transform.position, AudioRolloffMode.Linear, explodeRadius + 2f);
		}
		else if (num > explodeTime - 2f)
		{
			AudioManager.GetInstance().PlaySoundSingleAt("RPG_Audio/Enemy/TMX/TMX_grenade02", base.gameObject, base.transform.position, AudioRolloffMode.Linear, explodeRadius + 2f);
		}
		else if (num > explodeTime - 3f)
		{
			if (!flashEnabled)
			{
				flashEnabled = true;
				base.transform.GetChild(0).gameObject.SetActive(true);
			}
			AudioManager.GetInstance().PlaySoundSingleAt("RPG_Audio/Enemy/TMX/TMX_grenade01", base.gameObject, base.transform.position, AudioRolloffMode.Linear, explodeRadius + 2f);
		}
	}

	private void Explode()
	{
		GameObject original = Resources.Load("RPG_effect/RPG_explosion_stander_001") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Weapon/grenade/grenade_boom", base.transform.position);
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if ((localPlayer.GetTransform().position - base.transform.position).sqrMagnitude < explodeRadius * explodeRadius)
		{
			Ray ray = new Ray(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
			float distance = Vector3.Distance(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				localPlayer.OnHit(explodeDamage, enemy);
			}
		}
		Dictionary<string, SummonedItem> summonedList = localPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if ((value.GetTransform().position - base.transform.position).sqrMagnitude < explodeRadius * explodeRadius)
			{
				Ray ray2 = new Ray(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
				float distance2 = Vector3.Distance(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				RaycastHit hitInfo2;
				if (Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
				{
					value.OnHit(explodeDamage);
				}
			}
		}
	}
}
