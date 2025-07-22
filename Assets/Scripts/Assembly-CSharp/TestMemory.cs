using System.Collections.Generic;
using UnityEngine;

public class TestMemory : MonoBehaviour
{
	public List<GameObject> test = new List<GameObject>();

	private void OnGUI()
	{
		if (!GUI.Button(new Rect(0f, 0f, 100f, 100f), "test."))
		{
			return;
		}
		if (test.Count != 0)
		{
			for (int i = 0; i < test.Count; i++)
			{
				MemoryManager.Free(test[i]);
				test[i] = null;
			}
			test.Clear();
			Debug.Log("Free");
		}
		else
		{
			GameObject original = Resources.Load("Character/Soldier/SoldierM_T") as GameObject;
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.SetActiveRecursively(false);
			test.Add(gameObject);
		}
	}
}
