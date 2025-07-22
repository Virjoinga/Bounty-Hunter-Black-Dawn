using UnityEngine;

public class GrenadePhysicsScript : MonoBehaviour
{
	protected Transform proTransform;

	public float life;

	public Vector3 dir;

	protected float createdTime;

	public void Start()
	{
		proTransform = base.transform;
		createdTime = Time.time;
		float num = 15f;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			num *= VSMath.GL_FLY_BOOTH;
		}
		base.GetComponent<Rigidbody>().AddForce(dir * num, ForceMode.Impulse);
	}

	public void Update()
	{
		if (base.GetComponent<Rigidbody>() != null)
		{
			RaycastHit hitInfo = default(RaycastHit);
			Ray ray = new Ray(base.transform.position + Vector3.up * 5f, Vector3.down);
			if (Physics.Raycast(ray, out hitInfo, 5f, 1 << PhysicsLayer.FLOOR))
			{
				ray = new Ray(base.transform.position, Vector3.down);
				if (!Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
				{
					base.GetComponent<Rigidbody>().Sleep();
				}
			}
		}
		if (Time.time - createdTime > life * 2f)
		{
			Object.DestroyObject(base.gameObject);
		}
	}
}
