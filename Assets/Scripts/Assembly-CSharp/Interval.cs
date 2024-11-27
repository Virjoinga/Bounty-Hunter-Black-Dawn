using UnityEngine;

public class Interval
{
	private int[] weights;

	private int total;

	public Interval(params int[] weights)
	{
		this.weights = weights;
		total = 0;
		foreach (int num in weights)
		{
			total += num;
		}
	}

	public int GetIndex(int value)
	{
		if (weights.Length < 1)
		{
			return -1;
		}
		int num = 0;
		for (int i = 0; i < weights.Length; i++)
		{
			num += weights[i];
			if (value < num)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetIndex()
	{
		int value = Random.Range(0, total);
		return GetIndex(value);
	}
}
