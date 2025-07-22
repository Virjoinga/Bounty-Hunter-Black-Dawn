using UnityEngine;

public class ScreenDirection : MonoBehaviour
{
	protected float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
	}

	private void ResetDirection()
	{
	}

	private void LateUpdate()
	{
		ResetDirection();
	}
}
