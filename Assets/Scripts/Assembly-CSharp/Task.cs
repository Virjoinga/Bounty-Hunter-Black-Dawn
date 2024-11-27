using UnityEngine;

public class Task
{
	protected float startTime;

	public float ExecuteSchedule { get; set; }

	public bool IsRepeatedTask { get; set; }

	public Task()
	{
		IsRepeatedTask = false;
	}

	public void StartTask()
	{
		startTime = Time.time;
	}

	public virtual void Do()
	{
	}

	public bool Ready()
	{
		return Time.time - startTime > ExecuteSchedule;
	}
}
