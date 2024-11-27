using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
	protected Transform cameraTransform;

	protected float lastUpdateTime;

	private void Start()
	{
		if (Camera.main != null)
		{
			cameraTransform = Camera.main.transform;
		}
	}

	private void Update()
	{
		if (Time.time - lastUpdateTime < 0.02f)
		{
			return;
		}
		lastUpdateTime = Time.time;
		if (cameraTransform == null)
		{
			if (Camera.main != null)
			{
				cameraTransform = Camera.main.transform;
			}
		}
		else
		{
			base.transform.LookAt(cameraTransform);
		}
	}
}
