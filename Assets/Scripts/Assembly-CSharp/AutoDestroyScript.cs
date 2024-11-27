using UnityEngine;

public class AutoDestroyScript : IgnoreTimeScale
{
	public float life;

	protected float startTime;

	private void Start()
	{
		startTime = 0f;
	}

	private void Update()
	{
		startTime += UpdateRealTimeDelta();
		if (startTime > life)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
