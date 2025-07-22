using UnityEngine;

public class FloatIrradiateLaserScript : MonoBehaviour
{
	public int mDamage;

	public Enemy mEnemy;

	public GameObject mSpark;

	public Vector3 mTargetPos;

	private Ray mRay;

	private RaycastHit mRaycastHit;

	private float mLength;

	private LocalPlayer mLocalPlayer;

	private bool destroyed;

	public void Start()
	{
		mRaycastHit = default(RaycastHit);
		mLocalPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
	}

	private void OnEnable()
	{
		destroyed = true;
	}

	public void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (mEnemy == null)
		{
			return;
		}
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (mLocalPlayer != null && mLocalPlayer.GetTransform() != null)
			{
				mLocalPlayer.OnHit(mDamage, mEnemy);
				Debug.Log("FloatIrradiateLaserScript: OnTriggerEnter: " + mDamage);
			}
		}
		else
		{
			if (other.gameObject.layer != PhysicsLayer.SUMMONED)
			{
				return;
			}
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (controllableByCollider != null)
			{
				SummonedItem summonedByName = mLocalPlayer.GetSummonedByName(controllableByCollider.name);
				if (summonedByName != null)
				{
					summonedByName.OnHit(mDamage);
				}
			}
		}
	}
}
