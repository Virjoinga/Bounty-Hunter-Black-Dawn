using System.Collections.Generic;
using UnityEngine;

public class FloatPlasmaScript : MonoBehaviour
{
	public Enemy mEnemy;

	public int mExplosionDamage = 5;

	public float mExplosionRadius = 5f;

	public Vector3 mRisingSpeed;

	public float mSpeedValue;

	public float mRisingTime;

	public Vector3 mTargetPosition;

	public Player mTrackingPlayer;

	public Timer mTrackingUpdateTargetPosTimer = new Timer();

	public float mTrackingFlyAcceleration;

	public float mFactorForDistance;

	private float mStartTime;

	private bool mIsRising = true;

	private float mLastUpdatetargetPosTime;

	private Vector3 mDowningSpeed;

	private bool destroyed;

	private float mTargetStartTime;

	private Vector3 mTargetDir;

	public void Start()
	{
		mStartTime = Time.time;
		mLastUpdatetargetPosTime = Time.time;
		mIsRising = true;
	}

	public void Update()
	{
		AudioManager.GetInstance().PlaySoundSingleAt("RPG_Audio/Enemy/Float/float_lightball02", base.transform.position);
		if (Time.time - mStartTime > mRisingTime && mIsRising)
		{
			mIsRising = false;
			mTargetStartTime = Time.time;
			mTargetDir = (mTargetPosition - base.transform.position).normalized;
		}
		if (mIsRising)
		{
			base.transform.Translate(mRisingSpeed * Time.deltaTime, Space.World);
			return;
		}
		if (mTrackingPlayer != null)
		{
			Transform transform = mTrackingPlayer.GetTransform();
			if (transform != null && mTrackingPlayer.InPlayingState() && mTrackingUpdateTargetPosTimer.Ready())
			{
				mTargetPosition = mTrackingPlayer.GetTransform().position + Vector3.up * 1.5f;
				mLastUpdatetargetPosTime = Time.time - 0.49f;
				mTrackingUpdateTargetPosTimer.Do();
				mTargetDir = (mTargetPosition - base.transform.position).normalized;
			}
		}
		Move();
	}

	private void Move()
	{
		Quaternion to = Quaternion.LookRotation(mTargetDir);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, to, (Time.time - mLastUpdatetargetPosTime) / 0.5f);
		mSpeedValue += mTrackingFlyAcceleration * Time.deltaTime;
		base.transform.Translate(base.transform.forward * mSpeedValue * Time.deltaTime, Space.World);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (destroyed)
		{
			return;
		}
		GameObject original = Resources.Load("RPG_effect/RPG_explosion_stander_001") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Enemy/Float/float_lightball_attacked", base.transform.position);
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (mTrackingPlayer != null)
			{
				Debug.Log("Red FloatPlasmaScript: OnTriggerEnter " + (int)((float)mExplosionDamage * (1f / (mEnemy.GetHorizontalDistanceFromLocalPlayer() + 0.1f))));
				localPlayer.OnHit((int)((float)mExplosionDamage * (1f / (mEnemy.GetHorizontalDistanceFromLocalPlayer() + 0.1f))), mEnemy);
			}
			else
			{
				Debug.Log("Blue FloatPlasmaScript: OnTriggerEnter " + mExplosionDamage);
				localPlayer.OnHit(mExplosionDamage, mEnemy);
			}
		}
		Dictionary<string, SummonedItem> summonedList = localPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (other.gameObject.layer == PhysicsLayer.SUMMONED)
			{
				if (mTrackingPlayer != null)
				{
					value.OnHit((int)((float)mExplosionDamage * (1f / (mEnemy.GetHorizontalDistanceFromGameUnit(value) + 0.1f))));
				}
				else
				{
					value.OnHit(mExplosionDamage);
				}
			}
		}
		Object.DestroyObject(base.gameObject);
		destroyed = true;
	}
}
