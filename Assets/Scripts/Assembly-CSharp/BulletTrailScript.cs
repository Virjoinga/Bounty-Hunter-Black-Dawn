using UnityEngine;

public class BulletTrailScript : MonoBehaviour
{
	public Vector3 beginPos;

	public Vector3 endPos;

	public float speed;

	public bool isActive;

	protected float startTime;

	protected float deltaTime;

	protected float stopDistance;

	private void Start()
	{
		startTime = Time.time;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			speed *= 2f;
		}
		stopDistance = 0.05f * speed;
	}

	private void Update()
	{
		if (isActive)
		{
			base.transform.Translate(speed * (endPos - beginPos).normalized * Time.deltaTime, Space.World);
			if ((base.transform.position - endPos).magnitude < stopDistance)
			{
				isActive = false;
				base.gameObject.SetActive(false);
			}
			deltaTime = 0f;
		}
	}
}
