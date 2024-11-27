using UnityEngine;

public class Timer
{
	protected TimerInfo info = new TimerInfo();

	protected bool start;

	public float passTime;

	protected bool enable;

	public string Name { get; set; }

	public void SetTimer(float interval, bool doAtStart)
	{
		if (doAtStart)
		{
			info.lastDoTime = -9999f;
		}
		else
		{
			info.lastDoTime = Time.time;
		}
		info.interval = interval;
		passTime = 0f;
		start = true;
		enable = true;
	}

	public void Do()
	{
		info.lastDoTime = Time.time;
		enable = true;
		passTime = 0f;
	}

	public bool Ready()
	{
		if (start && Time.time - info.lastDoTime > info.interval)
		{
			return true;
		}
		return false;
	}

	public void Pause()
	{
		passTime = Time.time - info.lastDoTime;
		start = false;
	}

	public void Resume()
	{
		info.lastDoTime = Time.time - passTime;
		start = true;
	}

	public float GetInterval()
	{
		return info.interval;
	}

	public float GetTimeSpan()
	{
		if (start)
		{
			return Mathf.Min(Time.time - info.lastDoTime, info.interval);
		}
		return Mathf.Min(passTime, info.interval);
	}

	public bool Enable()
	{
		return enable;
	}

	public void Disable()
	{
		enable = false;
	}

	public float GetTimeNeededToNextReady()
	{
		return info.interval - (Time.time - info.lastDoTime);
	}
}
