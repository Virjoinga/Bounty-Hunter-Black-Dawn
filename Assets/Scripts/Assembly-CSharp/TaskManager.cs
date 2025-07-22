using System.Collections.Generic;

public class TaskManager
{
	public List<Task> taskList = new List<Task>();

	public void StartTask(Task task)
	{
		task.StartTask();
		taskList.Add(task);
	}

	public void StopTask(Task task)
	{
		if (taskList.Contains(task))
		{
			taskList.Remove(task);
		}
	}

	public void Update()
	{
		for (int num = taskList.Count - 1; num >= 0; num--)
		{
			Task task = taskList[num];
			if (task.Ready())
			{
				task.Do();
				if (task.IsRepeatedTask)
				{
					task.StartTask();
				}
				else
				{
					taskList.RemoveAt(num);
				}
			}
		}
	}
}
