using System.Collections.Generic;
using UnityEngine;

public class SceneStreamingTriggerScript : MonoBehaviour
{
	public string[] BlockNames;

	public GameObject[] SpawnPoints;

	public float UnloadDelayTime = 5f;

	private float mExitTime;

	private bool mToUnload;

	public List<string> MaterialNames = new List<string>();

	public List<string> TextureNames = new List<string>();

	private void Start()
	{
	}

	private void Update()
	{
		if (mToUnload && Time.time - mExitTime > UnloadDelayTime)
		{
			Unload();
			GameApp.GetInstance().GetSceneStreaingManager().RemoveTriggerToUnload(this);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			Debug.Log(string.Concat("OnTriggerEnter: ", base.gameObject.name, ", pos = ", base.gameObject.transform.position, ", other: ", other.name, ", pos + ", other.transform.position));
			if (mToUnload)
			{
				mToUnload = false;
				GameApp.GetInstance().GetSceneStreaingManager().RemoveTriggerToUnload(this);
			}
			else
			{
				Load();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			Debug.Log(string.Concat("OnTriggerExit: ", base.gameObject.name, ", pos = ", base.gameObject.transform.position, ", other: ", other.name, ", pos + ", other.transform.position));
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.CheckIfOnGround())
			{
				mToUnload = true;
				mExitTime = Time.time;
				GameApp.GetInstance().GetSceneStreaingManager().AddTriggerToUnload(this);
			}
		}
	}

	public void Load()
	{
		if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.HasStreamingVolume(base.gameObject))
		{
			return;
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.AddStreamingVolume(base.gameObject);
		foreach (string textureName in TextureNames)
		{
			string dataName = "scenes/" + Application.loadedLevelName.ToLower() + "/textures/android_prefab_" + textureName;
			GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(dataName, StreamingDataType.TEXTURE);
		}
		foreach (string materialName in MaterialNames)
		{
			string dataName2 = "scenes/" + Application.loadedLevelName.ToLower() + "/materials/android_prefab_" + materialName;
			GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(dataName2, StreamingDataType.MATERIAL);
		}
		string[] blockNames = BlockNames;
		foreach (string text in blockNames)
		{
			string dataName3 = "scenes/" + Application.loadedLevelName.ToLower() + "/android_prefab_" + text.ToLower();
			GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(dataName3, StreamingDataType.SCENE);
		}
		GameObject[] spawnPoints = SpawnPoints;
		foreach (GameObject gameObject in spawnPoints)
		{
			if (!(null != gameObject))
			{
				continue;
			}
			BaseEnemySpawnScript component = gameObject.GetComponent<BaseEnemySpawnScript>();
			if (!(null != component))
			{
				continue;
			}
			component.IncreaseRefCount();
			EnemySpawnPointScript enemySpawnPointScript = component as EnemySpawnPointScript;
			if (!(null != enemySpawnPointScript))
			{
				continue;
			}
			List<EnemyType> enemyTypeList = enemySpawnPointScript.GetEnemyTypeList();
			foreach (EnemyType item in enemyTypeList)
			{
				string[] array = AssetBundleName.ENEMY_TEXTURE[(int)item];
				string[] array2 = array;
				foreach (string dataName4 in array2)
				{
					GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(dataName4, StreamingDataType.TEXTURE);
				}
				string dataName5 = AssetBundleName.ENEMY[(int)item];
				GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(dataName5, StreamingDataType.ENEMY);
			}
		}
	}

	public void Unload()
	{
		mToUnload = false;
		if (!GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.HasStreamingVolume(base.gameObject))
		{
			return;
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.RemoveStreamingVolume(base.gameObject);
		string[] blockNames = BlockNames;
		foreach (string text in blockNames)
		{
			string dataName = "scenes/" + Application.loadedLevelName.ToLower() + "/android_prefab_" + text.ToLower();
			GameApp.GetInstance().GetSceneStreaingManager().AddToUnloadData(dataName);
		}
		foreach (string materialName in MaterialNames)
		{
			string dataName2 = "scenes/" + Application.loadedLevelName.ToLower() + "/materials/android_prefab_" + materialName;
			GameApp.GetInstance().GetSceneStreaingManager().AddToUnloadData(dataName2);
		}
		foreach (string textureName in TextureNames)
		{
			string dataName3 = "scenes/" + Application.loadedLevelName.ToLower() + "/textures/android_prefab_" + textureName;
			GameApp.GetInstance().GetSceneStreaingManager().AddToUnloadData(dataName3);
		}
		GameObject[] spawnPoints = SpawnPoints;
		foreach (GameObject gameObject in spawnPoints)
		{
			if (!(null != gameObject))
			{
				continue;
			}
			BaseEnemySpawnScript component = gameObject.GetComponent<BaseEnemySpawnScript>();
			if (!(null != component))
			{
				continue;
			}
			component.DecreaseRefCount();
			EnemySpawnPointScript enemySpawnPointScript = component as EnemySpawnPointScript;
			if (!(null != enemySpawnPointScript))
			{
				continue;
			}
			List<EnemyType> enemyTypeList = enemySpawnPointScript.GetEnemyTypeList();
			foreach (EnemyType item in enemyTypeList)
			{
				string dataName4 = AssetBundleName.ENEMY[(int)item];
				GameApp.GetInstance().GetSceneStreaingManager().AddToUnloadData(dataName4);
				string[] array = AssetBundleName.ENEMY_TEXTURE[(int)item];
				string[] array2 = array;
				foreach (string dataName5 in array2)
				{
					GameApp.GetInstance().GetSceneStreaingManager().AddToUnloadData(dataName5);
				}
			}
		}
	}
}
