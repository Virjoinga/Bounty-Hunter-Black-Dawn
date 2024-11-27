using UnityEngine;

public class FisheyeAnimationScript : MonoBehaviour
{
	private const int TIMES = 0;

	private const byte STATE_NONE = 0;

	private const byte STATE_INTENSIFY = 1;

	private const byte STATE_WEAKEN = 2;

	public const byte STATE_INTENSIFY_SHAKEN = 3;

	public const byte STATE_WEAKEN_SHAKEN = 4;

	public float duration = 1f;

	public float fovRange = 40f;

	private float startTime;

	private byte state;

	private float initFOV;

	private void Awake()
	{
		state = 0;
	}

	private void Update()
	{
		float num = 0f;
		byte b = state;
		if (state != 0)
		{
			num = Time.time - startTime;
			if (num > duration)
			{
				num = duration;
				state = 0;
			}
		}
		switch (b)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
		{
			float num2 = fovRange * num / duration;
			float f = 0f * num / duration;
			Camera.main.GetComponent<Camera>().fov = initFOV + num2 * Mathf.Cos(f);
			break;
		}
		case 4:
		{
			float num2 = fovRange - fovRange * num / duration;
			float f = 0f * num / duration;
			Camera.main.GetComponent<Camera>().fov = initFOV + num2 * Mathf.Cos(f);
			break;
		}
		}
	}

	public void Act(byte state)
	{
		if (this.state == 0)
		{
			this.state = state;
			startTime = Time.time;
			initFOV = Camera.main.GetComponent<Camera>().fov;
		}
	}

	public bool HasNoAction()
	{
		return state == 0;
	}
}
