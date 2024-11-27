using System.Collections.Generic;
using UnityEngine;

public class ShellMissileScript : MonoBehaviour
{
	public Enemy mEnemy;

	public int mExplosionDamage = 5;

	public float mExplosionRadius = 5f;

	public Vector3 mRisingSpeed;

	public float mAttackSpeedValue;

	public float mRisingTime;

	public GameUnit mTarget;

	public Vector3 mTargetPosition;

	public bool mHasSlime;

	public float mSlimeDamageInterval;

	public int mSlimeDamage;

	public float mSlimeDuration;

	private float mStartTime;

	private bool mIsRising = true;

	private Vector3 mAttackSpeed;

	private bool destroyed;

	private void Start()
	{
		mStartTime = Time.time;
		mIsRising = true;
		base.transform.LookAt(base.transform.position + mRisingSpeed);
	}

	private void Update()
	{
		if (Time.time - mStartTime > mRisingTime && mIsRising)
		{
			mIsRising = false;
			if (mTarget != null && mTarget.InPlayingState() && null != mTarget.GetTransform())
			{
				mTargetPosition = mTarget.GetTransform().position;
			}
			Vector3 normalized = (mTargetPosition - base.transform.position).normalized;
			mAttackSpeed = mAttackSpeedValue * normalized;
			base.transform.LookAt(mTargetPosition);
		}
		if (mIsRising)
		{
			base.transform.Translate(mRisingSpeed * Time.deltaTime, Space.World);
		}
		else
		{
			base.transform.Translate(mAttackSpeed * Time.deltaTime, Space.World);
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
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		if (Mathf.Sqrt((base.transform.position - localPlayer.GetPosition()).sqrMagnitude) < 20f)
		{
			localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
		}
		AudioManager.GetInstance().PlaySoundAt("Audio/rpg/rpg-21_boom", base.transform.position);
		RaycastHit hitInfo;
		if ((localPlayer.GetTransform().position - base.transform.position).sqrMagnitude < mExplosionRadius * mExplosionRadius)
		{
			Ray ray = new Ray(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
			float distance = Vector3.Distance(base.transform.position, localPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
			if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				localPlayer.OnHit(mExplosionDamage, mEnemy);
			}
		}
		Dictionary<string, SummonedItem> summonedList = localPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if ((value.GetTransform().position - base.transform.position).sqrMagnitude < mExplosionRadius * mExplosionRadius)
			{
				Ray ray = new Ray(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - base.transform.position);
				float distance2 = Vector3.Distance(base.transform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				if (Physics.Raycast(ray, out hitInfo, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo.collider.gameObject.layer == PhysicsLayer.SUMMONED)
				{
					value.OnHit(mExplosionDamage);
				}
			}
		}
		if (mHasSlime)
		{
			Vector3 vector = base.transform.position;
			Quaternion rotation = Quaternion.identity;
			Ray ray = new Ray(vector + Vector3.up * 0.5f, 100f * Vector3.down);
			int layerMask = 1 << PhysicsLayer.FLOOR;
			if (Physics.Raycast(ray, out hitInfo, 5f, layerMask))
			{
				vector = hitInfo.point + 0.01f * hitInfo.normal;
				rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			}
			GameObject original2 = Resources.Load("RPG_effect/RPG_shell_fire_002") as GameObject;
			GameObject gameObject = Object.Instantiate(original2, vector, rotation) as GameObject;
			EnemySlimeScript component = gameObject.GetComponent<EnemySlimeScript>();
			component.enemy = mEnemy;
			component.damageInterval = mSlimeDamageInterval;
			component.slimeDamage = mSlimeDamage;
			component.disappearTime = mSlimeDuration;
			component.minScale = 1f;
		}
		Object.DestroyObject(base.gameObject);
		destroyed = true;
	}
}
