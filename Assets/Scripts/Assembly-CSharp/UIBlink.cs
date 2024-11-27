using UnityEngine;

public class UIBlink : MonoBehaviour
{
	public GameObject target;

	public float interval = 1f;

	private float startTime;

	private void OnEnable()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		if (target != null && Time.time - startTime > interval)
		{
			startTime = Time.time;
			target.SetActive(!target.activeSelf);
		}
	}
}
