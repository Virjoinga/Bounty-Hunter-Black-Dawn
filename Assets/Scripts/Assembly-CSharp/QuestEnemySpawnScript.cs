using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEnemySpawnScript : BaseEnemySpawnScript
{
	public List<GameObject> TranditionalSpanwPoints = new List<GameObject>();

	public List<GameObject> SkySpawnPoints = new List<GameObject>();

	public float spawnInterval = 1f;

	public int maxSpawns = 7;

	public short QuestId;

	private Player player;

	private GameWorld gameWorld;

	private SpawnConfig spawnConfigInfo;

	private int playerIndex;

	private int spIndex;

	private int skySpIndex;

	private bool mNeedWaitForDownload;

	private byte mCurrentEnemyID;

	protected int enemySpawning;

	protected int spCount;

	protected int skySpCount;

	private bool mIsSpawning;

	private List<EnemySpawnInfo> EnemyInfos { get; set; }

	public int EnemySpawning
	{
		get
		{
			return enemySpawning;
		}
		set
		{
			enemySpawning = value;
		}
	}

	private void Start()
	{
	}

	public void StartSpawn()
	{
		if (!mIsSpawning)
		{
			StartCoroutine(SpawnThread());
			mIsSpawning = true;
		}
	}

	public void StopSpawn()
	{
		if (mIsSpawning)
		{
			mIsSpawning = false;
			StopCoroutine("SpawnThread");
		}
	}

	public void InitEnemyInPoint()
	{
	}

	public override void Deactivate()
	{
		Debug.Log("QuestEnemySpawnScript.Deactivate!!!!!!!! " + base.gameObject.name);
		StopSpawn();
		foreach (Enemy mSpawnedEnemy in mSpawnedEnemyList)
		{
			mSpawnedEnemy.Deactivate();
			GameApp.GetInstance().GetGameScene().RemoveEnemy(mSpawnedEnemy.Name);
		}
		mSpawnedEnemyList.Clear();
		GameApp.GetInstance().GetGameWorld().RemovePoint(base.PointID);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerLeaveSpawnPointRequest request = new PlayerLeaveSpawnPointRequest(base.PointID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			RemoveSpawnPointRequest request2 = new RemoveSpawnPointRequest(base.PointID, false);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
		base.enabled = false;
	}

	private IEnumerator SpawnThread()
	{
		mPatrolLineIndex = 0;
		mSpawnedEnemyList = new List<Enemy>();
		enemySpawning = 0;
		spIndex = 0;
		skySpIndex = 0;
		spCount = TranditionalSpanwPoints.Count;
		skySpCount = SkySpawnPoints.Count;
		gameWorld = GameApp.GetInstance().GetGameWorld();
		QuestEnemySpawnConfig spawnConfig = null;
		if (GameConfig.GetInstance().questEnemySpawnConfig.ContainsKey(base.gameObject.name))
		{
			spawnConfig = GameConfig.GetInstance().questEnemySpawnConfig[base.gameObject.name];
		}
		if (spawnConfig == null)
		{
			Debug.LogError("No config for quest point: " + base.gameObject.name);
			yield return 0;
		}
		foreach (EnemySpawnInfo info in spawnConfig.EnemyInfos)
		{
			string[] enemyTextureNames = AssetBundleName.ENEMY_TEXTURE[(int)info.EType];
			string[] array = enemyTextureNames;
			foreach (string t in array)
			{
				GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(t, StreamingDataType.TEXTURE);
				while (!GameApp.GetInstance().GetSceneStreaingManager().isLoad(t))
				{
					yield return 0;
				}
			}
			string enemyDataName = AssetBundleName.ENEMY[(int)info.EType];
			GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(enemyDataName, StreamingDataType.ENEMY);
			while (!GameApp.GetInstance().GetSceneStreaingManager().isLoad(enemyDataName))
			{
				yield return 0;
			}
		}
		AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/Environment/start_fighting");
		mCurrentEnemyID = 0;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			DownloadEnemyInPointRequest downloadRequest = new DownloadEnemyInPointRequest(base.PointID, false);
			GameApp.GetInstance().GetNetworkManager().SendRequest(downloadRequest);
			mNeedWaitForDownload = true;
		}
		while (mNeedWaitForDownload)
		{
			yield return new WaitForSeconds(1f);
		}
		int enemyCount3 = 0;
		int spawnId = 0;
		foreach (EnemySpawnInfo info2 in spawnConfig.EnemyInfos)
		{
			Debug.Log(string.Concat("start spawn ", info2.EType, ":", info2.Count));
			for (int i = 0; i < info2.Count; i++)
			{
				if (spawnId < mCurrentEnemyID)
				{
					spawnId++;
					continue;
				}
				spawnId++;
				yield return new WaitForSeconds(spawnInterval);
				Debug.Log("spawn " + info2.EType);
				enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(base.PointID);
				while (enemyCount3 + enemySpawning >= maxSpawns)
				{
					yield return new WaitForSeconds(1f);
					enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(base.PointID);
				}
				if (!mIsSpawning)
				{
					break;
				}
				byte enemyID = mCurrentEnemyID;
				mCurrentEnemyID++;
				int level = Random.Range(info2.MinLevel, info2.MaxLevel + 1);
				Vector3 spawnPosition2 = Vector3.zero;
				if (info2.EType == EnemyType.ELITE_OBSIDIAN || info2.EType == EnemyType.OBSIDIAN)
				{
					spawnPosition2 = SkySpawnPoints[skySpIndex].transform.position;
					skySpIndex++;
					if (skySpIndex >= skySpCount)
					{
						skySpIndex = 0;
					}
				}
				else
				{
					spawnPosition2 = TranditionalSpanwPoints[spIndex].transform.position;
					spIndex++;
					if (spIndex >= spCount)
					{
						spIndex = 0;
					}
				}
				GameApp.GetInstance().GetGameWorld().AddEnemy(base.PointID, enemyID, info2.EType, false, (byte)level, info2.UniqueID, spawnPosition2);
				enemySpawning++;
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					RequireEnemyInPointRequest requireRequest = new RequireEnemyInPointRequest(base.PointID, enemyID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(requireRequest);
				}
				else
				{
					InitEnemy(enemyID);
				}
			}
			if (!mIsSpawning)
			{
				break;
			}
		}
		enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(base.PointID);
		while (enemyCount3 + enemySpawning > 0 && mIsSpawning)
		{
			yield return new WaitForSeconds(1f);
			enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(base.PointID);
		}
		if (mIsSpawning)
		{
			GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressEnemyKillall(QuestId);
		}
	}

	public void InitEnemy(byte enemyID)
	{
		Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(base.PointID, enemyID);
		if (enemy != null)
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				enemy.IsMasterPlayer = true;
			}
			enemy.SetSpawnPointScript(this);
			if (enemy.CanPatrol())
			{
				GameObject gameObject = PatrolLines[mPatrolLineIndex % PatrolLines.Count];
				PatrolGroupScript component = gameObject.GetComponent<PatrolGroupScript>();
				if (null != component)
				{
					enemy.NextPatrolLinePoint = component.StartPoint;
				}
				mPatrolLineIndex++;
			}
			enemy.SpawnType = ESpawnType.QUEST;
			enemy.Activate();
			GameApp.GetInstance().GetGameScene().AddEnemy(enemy.Name, enemy);
			if (!mSpawnedEnemyList.Contains(enemy))
			{
				mSpawnedEnemyList.Add(enemy);
			}
		}
		enemySpawning--;
	}

	public void InitEnemyFromGameWorld()
	{
		mPatrolLineIndex = 0;
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(base.PointID);
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			mCurrentEnemyID++;
			if (!item.Value.CanBeSpawn())
			{
				continue;
			}
			item.Value.SetSpawnPointScript(this);
			if (item.Value.CanPatrol())
			{
				GameObject gameObject = PatrolLines[mPatrolLineIndex % PatrolLines.Count];
				PatrolGroupScript component = gameObject.GetComponent<PatrolGroupScript>();
				if (null != component)
				{
					item.Value.NextPatrolLinePoint = component.StartPoint;
				}
				mPatrolLineIndex++;
			}
			item.Value.SpawnType = ESpawnType.QUEST;
			item.Value.Activate();
			mSpawnedEnemyList.Add(item.Value);
			GameApp.GetInstance().GetGameScene().AddEnemy(item.Value.Name, item.Value);
		}
		mNeedWaitForDownload = false;
	}
}
