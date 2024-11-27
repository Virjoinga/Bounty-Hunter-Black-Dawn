using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
	public List<GameObject> test = new List<GameObject>();

	public int index;

	public int offsety;

	private void Start()
	{
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, offsety, 100f, 100f), "test." + index) && index < test.Count && test[index] != null)
		{
			test[index].SetActive(true);
			index++;
		}
	}
}
