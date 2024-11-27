using UnityEngine;

public class StartScreenDirection : MonoBehaviour
{
	protected float startTime;

	protected int state;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		ResetDirection();
	}

	private void ResetDirection()
	{
	}

	private void LateUpdate()
	{
	}
}
