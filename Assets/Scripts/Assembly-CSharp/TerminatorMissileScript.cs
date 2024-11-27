using System.Collections.Generic;
using UnityEngine;

public class TerminatorMissileScript : MonoBehaviour
{
	private const float LOCK_DISTANCE = 20f;

	private const float HEAD_HEIGHT = 20f;

	private const float HEAD_LOCK_DISTANCE = 20f;

	public Enemy enemy;

	public float explosionRadius;

	public int explosionDamage;

	public float speed;

	public Transform targetTransform;

	public float angularSpeed;

	public Vector3 targetPosition;

	public ETerminatorMissileType type;

	private bool mLock;

	private bool mOnHead;

	private Timer mCheckDistanceTimer = new Timer();

	private bool destroyed;

	public void Start()
	{
		mLock = false;
		mOnHead = false;
		mCheckDistanceTimer.SetTimer(0.5f, false);
	}

	public void Update()
	{
		if (!mLock && targetTransform != null)
		{
			targetPosition = targetTransform.position;
		}
		if (type == ETerminatorMissileType.NORMAL)
		{
			Vector3 target = targetPosition - base.transform.position;
			Vector3 vector = Vector3.RotateTowards(base.transform.forward, target, angularSpeed, 1f);
			base.transform.LookAt(base.transform.position + vector);
			base.transform.Translate(base.transform.forward * speed * Time.deltaTime, Space.World);
			if (!mLock && mCheckDistanceTimer.Ready())
			{
				mCheckDistanceTimer.Do();
				if ((targetPosition - base.transform.position).sqrMagnitude < 400f)
				{
					mLock = true;
				}
			}
		}
		else
		{
			if (type != ETerminatorMissileType.HEAD)
			{
				return;
			}
			if (mOnHead)
			{
				Vector3 target2 = targetPosition + new Vector3(0f, 1.5f, 0f) - base.transform.position;
				Vector3 vector2 = Vector3.RotateTowards(base.transform.forward, target2, 0.2f * angularSpeed, 1f);
				base.transform.LookAt(base.transform.position + vector2);
				base.transform.Translate(base.transform.forward * speed * Time.deltaTime, Space.World);
				return;
			}
			Vector3 target3 = targetPosition + new Vector3(0f, 20f, 0f) - base.transform.position;
			Vector3 vector3 = Vector3.RotateTowards(base.transform.forward, target3, angularSpeed, 1f);
			base.transform.LookAt(base.transform.position + vector3);
			base.transform.Translate(base.transform.forward * speed * Time.deltaTime, Space.World);
			if (!mCheckDistanceTimer.Ready())
			{
				return;
			}
			mCheckDistanceTimer.Do();
			Vector3 vector4 = targetPosition - base.transform.position;
			vector4.y = 0f;
			if (vector4.sqrMagnitude < 400f)
			{
				Ray ray = new Ray(base.transform.position, targetPosition + new Vector3(0f, 0.5f, 0f) - base.transform.position);
				RaycastHit hitInfo = default(RaycastHit);
				float magnitude = (targetPosition + new Vector3(0f, 0.5f, 0f) - base.transform.position).magnitude;
				int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED);
				if (Physics.Raycast(ray, out hitInfo, magnitude, layerMask) && (hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER || hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER || hitInfo.collider.gameObject.layer == PhysicsLayer.SUMMONED))
				{
					mOnHead = true;
					mLock = true;
					base.transform.LookAt(targetPosition);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		GameObject original = Resources.Load("RPG_effect/RPG_explosion_stander_001") as GameObject;
		Vector3 vector = base.transform.position - base.transform.forward;
		Object.Instantiate(original, vector, Quaternion.identity);
		if (Mathf.Sqrt((base.transform.position - localPlayer.GetPosition()).sqrMagnitude) < 20f)
		{
			localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
		}
		AudioManager.GetInstance().PlaySoundAt("Audio/rpg/rpg-21_boom", vector);
		if ((localPlayer.GetTransform().position - vector).sqrMagnitude < explosionRadius * explosionRadius)
		{
			Ray ray = new Ray(vector, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - vector);
			float distance = Vector3.Distance(vector, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				localPlayer.OnHit(explosionDamage, enemy);
			}
		}
		Dictionary<string, SummonedItem> summonedList = localPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if ((value.GetTransform().position - vector).sqrMagnitude < explosionRadius * explosionRadius)
			{
				Ray ray2 = new Ray(vector, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - vector);
				float distance2 = Vector3.Distance(vector, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				RaycastHit hitInfo2;
				if (Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
				{
					value.OnHit(explosionDamage);
				}
			}
		}
		Object.DestroyObject(base.gameObject);
		destroyed = true;
	}
}
