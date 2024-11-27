using UnityEngine;

public class SpringSimulate
{
	private bool bRelease;

	private float mPercent;

	public float Update()
	{
		if (bRelease)
		{
			mPercent = Mathf.Max(mPercent - mPercent / 5f, 0f);
		}
		return mPercent;
	}

	public void Stretch(float percent)
	{
		bRelease = false;
		mPercent = Mathf.Clamp(percent, 0f, 1f);
	}

	public bool Release()
	{
		bRelease = true;
		return mPercent == 1f;
	}

	public bool IsMax()
	{
		return mPercent == 1f;
	}
}
