using UnityEngine;

public class FloatSafeLaserScript : MonoBehaviour
{
	public Enemy mEnemy;

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
		if (mEnemy != null)
		{
		}
	}
}
