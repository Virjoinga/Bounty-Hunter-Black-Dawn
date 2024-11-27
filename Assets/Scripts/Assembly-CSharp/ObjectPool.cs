using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	protected List<GameObject> objects;

	protected List<Transform> transforms;

	protected List<float> createdTime;

	protected float life;

	protected bool hasAnimation;

	protected bool hasParticleEmitter;

	protected GameObject folderObject;

	public void Init(string poolName, GameObject prefab, int initNum, float life)
	{
		objects = new List<GameObject>();
		transforms = new List<Transform>();
		createdTime = new List<float>();
		this.life = life;
		folderObject = new GameObject(poolName);
		folderObject.tag = TagName.OBJECT_POOL;
		for (int i = 0; i < initNum; i++)
		{
			GameObject gameObject = Object.Instantiate(prefab) as GameObject;
			objects.Add(gameObject);
			transforms.Add(gameObject.transform);
			createdTime.Add(0f);
			gameObject.active = false;
			gameObject.transform.parent = folderObject.transform;
			if (gameObject.GetComponent<Animation>() != null)
			{
				hasAnimation = true;
			}
			if (gameObject.GetComponent<ParticleEmitter>() != null)
			{
				hasParticleEmitter = true;
			}
			gameObject.SetActive(false);
		}
	}

	public GameObject CreateObject(Vector3 position, Vector3 lookAtRotation, Quaternion rotation)
	{
		for (int i = 0; i < objects.Count; i++)
		{
			if (!objects[i].active)
			{
				transforms[i].position = position;
				if (lookAtRotation != Vector3.zero)
				{
					transforms[i].rotation = Quaternion.LookRotation(lookAtRotation);
				}
				if (rotation != Quaternion.identity)
				{
					transforms[i].rotation = rotation;
				}
				objects[i].SetActive(true);
				createdTime[i] = Time.time;
				return objects[i];
			}
		}
		GameObject gameObject = Object.Instantiate(objects[0]) as GameObject;
		objects.Add(gameObject);
		transforms.Add(gameObject.transform);
		createdTime.Add(0f);
		gameObject.name = objects[0].name;
		gameObject.transform.parent = folderObject.transform;
		if (gameObject.GetComponent<Animation>() != null)
		{
			hasAnimation = true;
		}
		if (gameObject.GetComponent<ParticleEmitter>() != null)
		{
			hasParticleEmitter = true;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	public void DestroyObject()
	{
		if (null != folderObject)
		{
			Object.Destroy(folderObject);
		}
	}

	public void AutoDestruct()
	{
		if (objects == null)
		{
			return;
		}
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i].active && Time.time - createdTime[i] > life)
			{
				objects[i].transform.parent = folderObject.transform;
				objects[i].SetActive(false);
			}
		}
	}

	public void DestructAll()
	{
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i] != null && objects[i].active)
			{
				objects[i].transform.parent = folderObject.transform;
				objects[i].SetActive(false);
			}
		}
	}

	public GameObject DeleteObject(GameObject obj)
	{
		obj.SetActive(false);
		return obj;
	}

	public void DontDestroyOnLoad()
	{
		if (null != folderObject)
		{
			Object.DontDestroyOnLoad(folderObject);
		}
		for (int i = 0; i < objects.Count; i++)
		{
			if (objects[i] != null)
			{
				Object.DontDestroyOnLoad(objects[i]);
			}
		}
	}
}