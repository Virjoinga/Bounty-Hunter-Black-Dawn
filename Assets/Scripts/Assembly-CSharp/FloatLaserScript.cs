using UnityEngine;

public class FloatLaserScript : MonoBehaviour
{
	public Enemy mEnemy;

	public int mExplosionDamage = 5;

	public float mExplosionRadius = 5f;

	public Vector3 mRisingSpeed;

	public float mDownSpeedValue;

	public float mRisingTime;

	public Vector3 mTargetPosition;

	private float mStartTime;

	private bool mIsRising = true;

	private Vector3 mDowningSpeed;

	private bool destroyed;

	public void Start()
	{
		mStartTime = Time.time;
		mIsRising = true;
		base.transform.LookAt(base.transform.position + mRisingSpeed);
	}

	public void Update()
	{
		if (Time.time - mStartTime > mRisingTime && mIsRising)
		{
			mIsRising = false;
			base.transform.position = new Vector3(mTargetPosition.x, base.transform.position.y, mTargetPosition.z);
			Vector3 normalized = (mTargetPosition - base.transform.position).normalized;
			mDowningSpeed = mDownSpeedValue * normalized;
			base.transform.LookAt(mTargetPosition);
		}
		if (mIsRising)
		{
			base.transform.Translate(mRisingSpeed * Time.deltaTime, Space.World);
		}
		else
		{
			base.transform.Translate(mDowningSpeed * Time.deltaTime, Space.World);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject original = Resources.Load("RPG_effect/RPG_explosion_stander_001") as GameObject;
		Object.Instantiate(original, base.transform.position, Quaternion.identity);
		AudioManager.GetInstance().PlaySoundAt("Audio/rpg/rpg-21_boom", base.transform.position);
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			Debug.Log("FloatLaserScript: OnTriggerEnter " + mExplosionDamage);
			localPlayer.OnHit(mExplosionDamage, mEnemy);
		}
		else if (other.gameObject.layer == PhysicsLayer.SUMMONED)
		{
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (controllableByCollider != null)
			{
				SummonedItem summonedByName = localPlayer.GetSummonedByName(controllableByCollider.name);
				summonedByName.OnHit(mExplosionDamage);
			}
		}
	}
}
