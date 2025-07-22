using UnityEngine;

public class TerminatorLaserScript : MonoBehaviour
{
	private const float SCLAE_XY = 10f;

	public int mDamage;

	public Enemy mEnemy;

	public GameObject mSpark;

	private Ray mRay;

	private RaycastHit mRaycastHit;

	private float mLength;

	private LocalPlayer mLocalPlayer;

	private Timer mPlayerHitTimer = new Timer();

	private Timer mSummonedHitTimer = new Timer();

	private Vector3[] mPlayerOffests;

	private Vector3[] mSummonedOffsets;

	public void Start()
	{
		mRaycastHit = default(RaycastHit);
		base.transform.localScale = new Vector3(10f, 10f, 0f);
		mLocalPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		mPlayerHitTimer.SetTimer(0.3f, true);
		mSummonedHitTimer.SetTimer(0.3f, true);
		mPlayerOffests = new Vector3[5]
		{
			new Vector3(0f, 1.95f, 0f),
			new Vector3(0.4f, 1.5f, 0f),
			new Vector3(-0.4f, 1.5f, 0f),
			new Vector3(0.4f, 0.5f, 0f),
			new Vector3(-0.4f, 0.5f, 0f)
		};
		mSummonedOffsets = new Vector3[2]
		{
			new Vector3(0.45f, 1.2f, 0f),
			new Vector3(-0.45f, 1.2f, 0f)
		};
	}

	public void Update()
	{
		mRay = new Ray(base.transform.position, base.transform.forward);
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR);
		if (Physics.Raycast(mRay, out mRaycastHit, 200f, layerMask))
		{
			float magnitude = (mRaycastHit.point - base.transform.position).magnitude;
			base.transform.localScale = new Vector3(10f, 10f, (magnitude + 0.1f) / 10f);
			mSpark.transform.position = mRaycastHit.point;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (mEnemy == null)
		{
			return;
		}
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (mLocalPlayer == null || !(mLocalPlayer.GetTransform() != null) || !mPlayerHitTimer.Ready())
			{
				return;
			}
			mPlayerHitTimer.Do();
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.PLAYER);
			Transform transform = mLocalPlayer.GetTransform();
			bool flag = false;
			Vector3[] array = mPlayerOffests;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = array[i];
				Vector3 vector2 = transform.position + transform.right * vector.x + transform.up * vector.y + transform.forward * vector.z;
				mRay = new Ray(base.transform.position, vector2 - base.transform.position);
				float magnitude = (vector2 - base.transform.position).magnitude;
				if (Physics.Raycast(mRay, out mRaycastHit, magnitude, layerMask) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mLocalPlayer.OnHit(mDamage, mEnemy);
			}
		}
		else
		{
			if (other.gameObject.layer != PhysicsLayer.SUMMONED)
			{
				return;
			}
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (!(controllableByCollider != null))
			{
				return;
			}
			SummonedItem summonedByName = mLocalPlayer.GetSummonedByName(controllableByCollider.name);
			if (summonedByName == null || !(null != summonedByName.GetTransform()) || !mSummonedHitTimer.Ready())
			{
				return;
			}
			mSummonedHitTimer.Do();
			int layerMask2 = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.SUMMONED);
			Transform transform2 = summonedByName.GetTransform();
			bool flag2 = false;
			Vector3[] array2 = mSummonedOffsets;
			for (int j = 0; j < array2.Length; j++)
			{
				Vector3 vector3 = array2[j];
				Vector3 vector4 = transform2.position + transform2.right * vector3.x + transform2.up * vector3.y + transform2.forward * vector3.z;
				mRay = new Ray(base.transform.position, vector4 - base.transform.position);
				float magnitude2 = (vector4 - base.transform.position).magnitude;
				if (Physics.Raycast(mRay, out mRaycastHit, magnitude2, layerMask2) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.SUMMONED)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				summonedByName.OnHit(mDamage);
			}
		}
	}
}
