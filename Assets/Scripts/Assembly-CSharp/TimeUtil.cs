using UnityEngine;

public class TimeUtil
{
	private float totalTime;

	private float maxValue;

	private float curTime;

	public TimeUtil(float totalTime, float maxValue)
	{
		this.totalTime = totalTime;
		this.maxValue = maxValue;
		Reset();
	}

	public float GetValue()
	{
		float deltaTime = Time.deltaTime;
		curTime = ((!(curTime + deltaTime > totalTime)) ? (curTime + deltaTime) : totalTime);
		return maxValue * curTime / totalTime;
	}

	public void Reset()
	{
		curTime = 0f;
	}
}
