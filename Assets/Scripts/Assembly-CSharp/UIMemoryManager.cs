using UnityEngine;

public class UIMemoryManager
{
	private const int MAX_MEMORY_CLEAR_WEIGHT = 6;

	private static int mMemoryClearCounter;

	public static void IncreaseMemoryClearCounter(int weight)
	{
		mMemoryClearCounter += weight;
	}

	public static bool CheckMemoryClear()
	{
		if (mMemoryClearCounter >= 6)
		{
			Resources.UnloadUnusedAssets();
			Debug.Log("Resources.UnloadUnusedAssets()");
			mMemoryClearCounter = 0;
			return true;
		}
		return false;
	}
}
