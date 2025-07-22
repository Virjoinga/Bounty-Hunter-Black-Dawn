using UnityEngine;

public class IconFlash : MonoBehaviour
{
	public UIWidget icon;

	public bool StartWhenEnable = true;

	public float inc = 0.01f;

	private float cInc;

	private int times;

	private void OnEnable()
	{
		icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
		if (StartWhenEnable)
		{
			times = -1;
		}
		else
		{
			times = 0;
		}
		cInc = 0f - inc;
	}

	private void Update()
	{
		if (times == 0)
		{
			return;
		}
		float num = icon.color.a + cInc;
		if (num > 1f)
		{
			num = 1f;
			cInc = 0f - inc;
			if (times > 0)
			{
				times--;
			}
		}
		else if (num < 0f)
		{
			num = 0f;
			cInc = inc;
		}
		icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, num);
	}

	public int Pause()
	{
		int result = times;
		times = 0;
		return result;
	}

	public void Refresh()
	{
		icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
	}

	public void Resume()
	{
		times = -1;
	}

	public void Resume(int times)
	{
		if (times > 0)
		{
			this.times = times;
		}
	}
}
