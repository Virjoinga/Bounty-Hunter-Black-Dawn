using UnityEngine;

public class FireLineScript : MonoBehaviour
{
	public Vector3 beginPos;

	public Vector3 endPos;

	public float speed;

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
		deltaTime += Time.deltaTime;
		if (!(deltaTime < 0.03f))
		{
			base.transform.Translate(speed * (endPos - beginPos).normalized * deltaTime, Space.World);
			if ((base.transform.position - endPos).magnitude < stopDistance)
			{
				base.gameObject.active = false;
			}
			deltaTime = 0f;
		}
	}
}
