using System.Collections.Generic;
using UnityEngine;

public class AutoLinkManager
{
	public static void LinkAll()
	{
		LinkAllCovers();
		LinkAllPatrolLines();
		LinkAllSpawnGroups();
		LinkAllQuestSpawnGroups();
	}

	public static void LinkAllCovers()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.COVER);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			CoverLinkScript component = gameObject.GetComponent<CoverLinkScript>();
			if (!(null != component))
			{
				continue;
			}
			int childCount = gameObject.transform.GetChildCount();
			for (int j = 0; j < childCount; j++)
			{
				Transform child = gameObject.transform.GetChild(j);
				if (child.gameObject.name.ToLower().Contains("expose"))
				{
					component.Expose = child.gameObject;
				}
				else if (child.gameObject.name.ToLower().Contains("hide"))
				{
					component.Hide = child.gameObject;
				}
			}
		}
	}

	public static void LinkAllPatrolLines()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.PATROL);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			PatrolGroupScript component = gameObject.GetComponent<PatrolGroupScript>();
			if (!(null != component))
			{
				continue;
			}
			int childCount = gameObject.transform.GetChildCount();
			PatrolPointScript patrolPointScript = null;
			PatrolPointScript patrolPointScript2 = null;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = gameObject.transform.GetChild(j);
				if (child.gameObject.name.ToLower().Contains("start"))
				{
					patrolPointScript = child.gameObject.GetComponent<PatrolPointScript>();
				}
				else if (child.gameObject.name.ToLower().Contains("end"))
				{
					patrolPointScript2 = child.gameObject.GetComponent<PatrolPointScript>();
				}
			}
			if (null != patrolPointScript && null != patrolPointScript2)
			{
				patrolPointScript.NextPoint = patrolPointScript2.gameObject;
				patrolPointScript2.NextPoint = patrolPointScript.gameObject;
				component.StartPoint = patrolPointScript.gameObject;
			}
		}
	}

	public static void LinkAllSpawnGroups()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_GROUP);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			int childCount = gameObject.transform.GetChildCount();
			for (int j = 0; j < childCount; j++)
			{
				Transform child = gameObject.transform.GetChild(j);
				if (child.gameObject.tag == TagName.ENEMY_SPAWN_POINT)
				{
					list.Add(child.gameObject);
				}
				else if (child.gameObject.tag == TagName.PATROL)
				{
					list2.Add(child.gameObject);
				}
			}
			if (list.Count == 0)
			{
				Debug.Log("There is no spawn points in " + gameObject.name + "!!!");
				continue;
			}
			EnemySpawnPointScript component = list[0].GetComponent<EnemySpawnPointScript>();
			if (!(null != component))
			{
				continue;
			}
			component.PatrolLines = new List<GameObject>();
			foreach (GameObject item in list2)
			{
				component.PatrolLines.Add(item);
			}
		}
	}

	public static void LinkAllQuestSpawnGroups()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.QUEST_ENEMY_SPAWN_GROUP);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			List<GameObject> list3 = new List<GameObject>();
			GameObject gameObject2 = null;
			GameObject gameObject3 = null;
			int childCount = gameObject.transform.GetChildCount();
			for (int j = 0; j < childCount; j++)
			{
				Transform child = gameObject.transform.GetChild(j);
				if (child.gameObject.tag == TagName.TRADITIONAL_SPAWN_POINT)
				{
					list.Add(child.gameObject);
				}
				else if (child.gameObject.tag == TagName.SKY_SPAWN_POINT)
				{
					list2.Add(child.gameObject);
				}
				else if (child.gameObject.tag == TagName.ENEMY_SPAWN_TRIGGER)
				{
					gameObject3 = child.gameObject;
				}
				else if (child.gameObject.tag == TagName.PATROL)
				{
					list3.Add(child.gameObject);
				}
				else if (child.gameObject.tag == TagName.QUEST_ENEMY_SPAWN)
				{
					gameObject2 = child.gameObject;
				}
			}
			if (list.Count == 0 && list2.Count == 0)
			{
				Debug.Log("There is no spawn points in " + gameObject.name + "!!!");
				continue;
			}
			if (gameObject2 == null)
			{
				Debug.Log("There is no QuestEnemySpawn in " + gameObject.name + "!!!");
				continue;
			}
			if (gameObject3 == null)
			{
				Debug.Log("There is no Spawn Trigger in " + gameObject.name + "!!!");
				continue;
			}
			EnemySpawnTriggerScript component = gameObject3.GetComponent<EnemySpawnTriggerScript>();
			QuestEnemySpawnScript component2 = gameObject2.GetComponent<QuestEnemySpawnScript>();
			if (!(null != component) || !(null != component2))
			{
				continue;
			}
			component.SpawnScript = component2;
			component2.PatrolLines = new List<GameObject>();
			foreach (GameObject item in list3)
			{
				component2.PatrolLines.Add(item);
			}
			component2.TranditionalSpanwPoints = list;
			component2.SkySpawnPoints = list2;
			NameComparer comparer = new NameComparer();
			component2.TranditionalSpanwPoints.Sort(comparer);
			component2.SkySpawnPoints.Sort(comparer);
			for (int k = 1; k < component2.TranditionalSpanwPoints.Count; k++)
			{
				if (component2.TranditionalSpanwPoints[k].name == component2.TranditionalSpanwPoints[k - 1].name)
				{
					Debug.LogError("[SpawnPoint]Same name: " + component2.TranditionalSpanwPoints[k].name);
				}
			}
			for (int l = 1; l < component2.SkySpawnPoints.Count; l++)
			{
				if (component2.SkySpawnPoints[l].name == component2.SkySpawnPoints[l - 1].name)
				{
					Debug.LogError("[SkySpawnPoint]Same name: " + component2.SkySpawnPoints[l].name);
				}
			}
		}
	}
}
