using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil
{
	private static void Activate(Transform t)
	{
		SetActiveSelf(t.gameObject, true);
		int i = 0;
		for (int childCount = t.GetChildCount(); i < childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				return;
			}
		}
		int j = 0;
		for (int childCount2 = t.GetChildCount(); j < childCount2; j++)
		{
			Transform child2 = t.GetChild(j);
			Activate(child2);
		}
	}

	private static void Deactivate(Transform t)
	{
		SetActiveSelf(t.gameObject, false);
	}

	public static void SetActive(GameObject go, bool state)
	{
		if (state)
		{
			Activate(go.transform);
		}
		else
		{
			Deactivate(go.transform);
		}
	}

	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	public static void SetActiveSelfWithParents(GameObject go)
	{
		if (!go.activeSelf)
		{
			SetActiveSelf(go, true);
		}
		if (go.transform.parent != null)
		{
			SetActiveSelfWithParents(go.transform.parent.gameObject);
		}
	}

	public static List<Material> GetMaterials(GameObject obj)
	{
		List<Material> list = new List<Material>();
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(null != componentsInChildren[i]))
			{
				continue;
			}
			for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
			{
				if (componentsInChildren[i].materials[j] != null)
				{
					list.Add(componentsInChildren[i].materials[j]);
				}
			}
		}
		return list;
	}
}
