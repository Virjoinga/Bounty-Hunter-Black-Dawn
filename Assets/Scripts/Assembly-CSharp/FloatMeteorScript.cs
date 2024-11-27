using UnityEngine;

public class FloatMeteorScript : MonoBehaviour
{
	public int mDamage;

	public Enemy mEnemy;

	public Vector3 mTargetPos;

	private Ray mRay;

	private RaycastHit mRaycastHit;

	private float mLength;

	private LocalPlayer mLocalPlayer;

	private Timer mPlayerHitTimer = new Timer();

	private Timer mSummonedHitTimer = new Timer();

	public void Start()
	{
		mRaycastHit = default(RaycastHit);
		mLocalPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		mPlayerHitTimer.SetTimer(1f, true);
		mSummonedHitTimer.SetTimer(1f, true);
	}

	public void Update()
	{
		if (mEnemy == null || (mEnemy != null && mEnemy.GetTransform() == null))
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (mEnemy == null)
		{
			return;
		}
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			bool flag = true;
			if (mLocalPlayer != null && mLocalPlayer.GetTransform() != null && mPlayerHitTimer.Ready())
			{
				if (mEnemy.GetHorizontalSqrDistanceFromLocalPlayer() <= 72f)
				{
					flag = false;
					Debug.Log("meteor no attack.........");
				}
				mPlayerHitTimer.Do();
				if (flag)
				{
					Debug.Log("meteor attack......... " + mDamage);
					mLocalPlayer.OnHit(mDamage, mEnemy);
				}
			}
		}
		else
		{
			if (other.gameObject.layer != PhysicsLayer.SUMMONED)
			{
				return;
			}
			bool flag2 = true;
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (!(controllableByCollider != null))
			{
				return;
			}
			SummonedItem summonedByName = mLocalPlayer.GetSummonedByName(controllableByCollider.name);
			if (summonedByName != null && null != summonedByName.GetTransform() && mSummonedHitTimer.Ready())
			{
				mSummonedHitTimer.Do();
				if (mEnemy.GetHorizontalSqrDistanceFromGameUnit(summonedByName) < 72f)
				{
					flag2 = false;
				}
				if (flag2)
				{
					summonedByName.OnHit(mDamage);
				}
			}
		}
	}
}
