using System;
using System.Collections.Generic;
using UnityEngine;

public class GameScene
{
	protected Dictionary<string, Enemy> enemyList = new Dictionary<string, Enemy>();

	protected Dictionary<short, Npc> npcList = new Dictionary<short, Npc>();

	protected HashSet<int> lostEnemyList = new HashSet<int>();

	protected int enemyRespawns;

	protected int enemyID;

	protected Dictionary<string, SummonedItem> summonedList = new Dictionary<string, SummonedItem>();

	protected List<SummonedItem> toDeleteSummonedList = new List<SummonedItem>();

	protected Timer switchBossLevelTimer = new Timer();

	protected EnemyType bossType;

	protected short bossID;

	protected float bossLeveStartFadeTime = -1f;

	protected FirstPersonCameraScript fpcs;

	protected ObjectPool[] mEffectPools;

	protected Enemy[] bossArray;

	protected VSClock vsClock;

	protected TaskManager taskManager = new TaskManager();

	protected List<GameObject> portal = new List<GameObject>();

	protected Dictionary<string, TDMCapturePointScript> capturePoint = new Dictionary<string, TDMCapturePointScript>();

	private GameObject mItemRoot;

	private GameObject mMithrilFruitMachineEffect;

	private GameObject mGoldFruitMachineEffect;

	public int BaseDifficultyLevel { get; set; }

	public int DifficultyLevel { get; set; }

	public int TotalEnemyCount { get; set; }

	public int TotalWaves { get; set; }

	public GameState State { get; set; }

	public bool InBossBattle { get; set; }

	public MithrilDropInfo MithrilDrops { get; set; }

	public VSBattleInformation BattleInfo { get; set; }

	public PVPRewardInfo PVPReward { get; set; }

	public bool IsDungeon { get; set; }

	public MapType mapType { get; set; }

	public bool Exit { get; set; }

	public Shader TransparentShader { get; set; }

	public int EnemyID
	{
		get
		{
			return enemyID;
		}
		set
		{
			enemyID = value;
		}
	}

	public GameObject GetItemRoot()
	{
		return mItemRoot;
	}

	public Enemy GetBoss(int index)
	{
		if (index < bossArray.Length)
		{
			return bossArray[index];
		}
		return null;
	}

	public void Init()
	{
		IsDungeon = false;
		if (Application.loadedLevelName.StartsWith("City"))
		{
			mapType = MapType.City;
		}
		else if (Application.loadedLevelName.StartsWith("Arena"))
		{
			mapType = MapType.Arena;
			TransparentShader = Shader.Find("iPhone/AlphaBlend_Color");
		}
		else if (Application.loadedLevelName.StartsWith("Instance"))
		{
			mapType = MapType.Instance;
			TransparentShader = Shader.Find("iPhone/AlphaBlend_Color");
		}
		else if (Application.loadedLevelName.StartsWith("VS"))
		{
			mapType = MapType.VS;
		}
		else if (Application.loadedLevelName.StartsWith("BossRush"))
		{
			mapType = MapType.BossRush;
			TransparentShader = Shader.Find("iPhone/AlphaBlend_Color");
		}
		else
		{
			mapType = MapType.City;
			Debug.LogError("Invalid Scene Name: " + Application.loadedLevelName);
		}
		if (IsDungeon)
		{
			DungeonResource.GetInstance().LoadAllResource();
			DungeonManager.GetInstance().Create();
			DungeonManager.GetInstance().LoadFirstMap();
		}
		Exit = false;
		AutoLinkManager.LinkAll();
		InitSpawnPoints();
		InitCovers();
		fpcs = Camera.main.GetComponent<FirstPersonCameraScript>();
		fpcs.Init();
		InitNpc();
		State = GameState.Playing;
		vsClock = Lobby.GetInstance().GetVSClock();
		vsClock.Restart();
		mEffectPools = new ObjectPool[20];
		for (int i = 0; i < mEffectPools.Length; i++)
		{
			mEffectPools[i] = new ObjectPool();
		}
		GameObject prefab = Resources.Load("Effect/update_effect/RPG_blood_small") as GameObject;
		GameObject prefab2 = Resources.Load("RPG_effect/RPG_BulletScars_001") as GameObject;
		GameObject prefab3 = Resources.Load("RPG_effect/RPG_BulletScars_002") as GameObject;
		GameObject prefab4 = Resources.Load("RPG_effect/RPG_BulletScars_005") as GameObject;
		GameObject prefab5 = Resources.Load("RPG_effect/RPG_BulletScars_004") as GameObject;
		GameObject prefab6 = Resources.Load("Effect/Fireline") as GameObject;
		GameObject prefab7 = Resources.Load("Effect/Fireline_FPS") as GameObject;
		GameObject prefab8 = Resources.Load("RPG_effect/RPG_Rifle_001") as GameObject;
		GameObject prefab9 = Resources.Load("RPG_effect/RPG_Shotgun_001") as GameObject;
		GameObject gameObject = Resources.Load("RPG_effect/RPG_Shotgun_002") as GameObject;
		GameObject prefab10 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_radiation_ballistic") as GameObject;
		GameObject prefab11 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_Lightning_ballistic01") as GameObject;
		GameObject prefab12 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_poison_ballistic") as GameObject;
		GameObject prefab13 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_radiation_hit") as GameObject;
		GameObject prefab14 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_Lightning_hit") as GameObject;
		GameObject prefab15 = Resources.Load("RPG_effect/st_effect/RPG_Cypher_poison_hit") as GameObject;
		GameObject prefab16 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_Fire_002") as GameObject;
		GameObject prefab17 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_radiation_002") as GameObject;
		GameObject prefab18 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_lightning_002") as GameObject;
		GameObject prefab19 = Resources.Load("RPG_Effect/st_effect/RPG_Cypher_poison_002") as GameObject;
		mEffectPools[0].Init("effect/RPG_blood_small", prefab, 12, 0.4f);
		mEffectPools[1].Init("effect/RPG_BulletScars_001", prefab2, 12, 0.2f);
		mEffectPools[2].Init("effect/RPG_BulletScars_002", prefab3, 12, 0.2f);
		mEffectPools[3].Init("effect/RPG_BulletScars_005", prefab4, 6, 1f);
		mEffectPools[4].Init("effect/RPG_BulletScars_004", prefab5, 6, 1f);
		mEffectPools[5].Init("effect/Fireline", prefab6, 12, 0.4f);
		mEffectPools[6].Init("effect/Fireline_FPS", prefab7, 8, 0.4f);
		mEffectPools[7].Init("effect/RPG_Rifle_001", prefab8, 12, 0.8f);
		mEffectPools[8].Init("effect/RPG_Shotgun_001", prefab9, 6, 0.7f);
		mEffectPools[9].Init("effect/RPG_Shotgun_002", prefab9, 6, 0.7f);
		mEffectPools[10].Init("effect/Fire_Fireline", prefab10, 8, 0.4f);
		mEffectPools[11].Init("effect/Shock_Fireline", prefab11, 8, 0.4f);
		mEffectPools[12].Init("effect/Corrosive_Fireline", prefab12, 8, 0.4f);
		mEffectPools[13].Init("effect/Fire_BulletScars_001", prefab13, 12, 0.2f);
		mEffectPools[14].Init("effect/Shock_BulletScars_001", prefab14, 12, 0.2f);
		mEffectPools[15].Init("effect/Corrosive_BulletScars_001", prefab15, 12, 0.2f);
		mEffectPools[16].Init("effect/RPG_Unloop_Spark", prefab16, 12, 0.8f);
		mEffectPools[17].Init("effect/RPG_Unloop_Spark_Fire", prefab17, 12, 0.8f);
		mEffectPools[18].Init("effect/RPG_Unloop_Spark_Shock", prefab18, 12, 0.8f);
		mEffectPools[19].Init("effect/RPG_Unloop_Spark_Corrosive", prefab19, 12, 0.8f);
		InBossBattle = false;
		BattleInfo = new VSBattleInformation();
		PVPReward = new PVPRewardInfo();
		initPortal();
		initCapurePoint();
		mItemRoot = GameObject.Find("BlockRoot/ItemRoot");
		NumberManager.GetInstance().Init();
		if (!GameApp.GetInstance().GetUserState().m_questStateContainer.CheckSubQuestCompleted(101))
		{
			mGoldFruitMachineEffect = GameObject.Find("RPG_LuckDraw_001");
			if (mGoldFruitMachineEffect != null)
			{
				mGoldFruitMachineEffect.SetActive(false);
			}
			mMithrilFruitMachineEffect = GameObject.Find("RPG_LuckDraw_002");
			if (mMithrilFruitMachineEffect != null)
			{
				mMithrilFruitMachineEffect.SetActive(false);
			}
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.RefreshInstanceRespawnTimes();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.RefreshWeaponListFromItemInfo();
		Debug.Log("mRouletteEffect : " + mMithrilFruitMachineEffect);
	}

	private void initPortal()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.PORTAL);
		if (array != null)
		{
			GameObject[] array2 = array;
			foreach (GameObject item in array2)
			{
				portal.Add(item);
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag(TagName.BOSS_PORTAL);
		if (array3 != null)
		{
			GameObject[] array4 = array3;
			foreach (GameObject item2 in array4)
			{
				portal.Add(item2);
			}
		}
	}

	private void initCapurePoint()
	{
		TDMCapturePointScript[] array = UnityEngine.Object.FindObjectsOfType(typeof(TDMCapturePointScript)) as TDMCapturePointScript[];
		if (array != null)
		{
			TDMCapturePointScript[] array2 = array;
			foreach (TDMCapturePointScript tDMCapturePointScript in array2)
			{
				capturePoint.Add(string.Empty + tDMCapturePointScript.PointID, tDMCapturePointScript);
			}
		}
	}

	public List<GameObject> GetPortal()
	{
		return portal;
	}

	public Dictionary<string, TDMCapturePointScript> GetCapturePoint()
	{
		return capturePoint;
	}

	public TaskManager GetTaskManager()
	{
		return taskManager;
	}

	public ObjectPool GetEffectPool(EffectPoolType type)
	{
		return mEffectPools[(int)type];
	}

	public void AddLostEnemy(int enemyID)
	{
		lostEnemyList.Add(enemyID);
	}

	public int GetLostEnemyCount()
	{
		return lostEnemyList.Count;
	}

	public void AddRespawnedEnemy()
	{
		enemyRespawns++;
	}

	public int GetRespawnedEnemy()
	{
		return enemyRespawns;
	}

	public FirstPersonCameraScript GetCamera()
	{
		return fpcs;
	}

	public void AddEnemy(string enemyName, Enemy enemy)
	{
		if (!enemyList.ContainsKey(enemyName))
		{
			enemyList.Add(enemyName, enemy);
			UserStateHUD.GetInstance().AddEnemy(enemy);
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer.AddEnemyCheckHitTimer(enemy);
		}
		else
		{
			Debug.LogError("Enemy already exist: " + enemyName);
			enemy.Deactivate();
		}
	}

	public void RemoveEnemy(string enemyName)
	{
		if (enemyList.ContainsKey(enemyName))
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer.RemoveEnemyCheckHitTimer(enemyList[enemyName]);
			enemyList.Remove(enemyName);
		}
		UserStateHUD.GetInstance().RemoveEnemy(enemyName);
	}

	public void ClearAllEnemies()
	{
		enemyList.Clear();
	}

	public Dictionary<string, Enemy> GetEnemies()
	{
		return enemyList;
	}

	public Enemy GetEnemyByID(string enemyID)
	{
		if (enemyList.ContainsKey(enemyID))
		{
			return enemyList[enemyID];
		}
		return null;
	}

	public void LeaveScene()
	{
		npcList.Clear();
		portal.Clear();
		capturePoint.Clear();
		for (int i = 0; i < mEffectPools.Length; i++)
		{
			if (mEffectPools[i] != null)
			{
				mEffectPools[i].DestroyObject();
				mEffectPools[i] = null;
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			EnemySpawnPointScript component = gameObject.GetComponent<EnemySpawnPointScript>();
			if (null != component)
			{
				component.Deactivate();
			}
		}
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer != null)
		{
			localPlayer.ClearStreamingVolume();
		}
		GameApp.GetInstance().GetSceneStreaingManager().UnloadAllImmediately();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.ClearSummonedList();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCollider()
			.enabled = false;
		GameApp.GetInstance().GetGameWorld().ClearCityInfo();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null)
				{
					item.ClearSummonedList();
				}
			}
		}
		localPlayer.ClearExtraShieldWithoutEffect();
		if (localPlayer.HealingEffect != null && !localPlayer.IsInstantHealing)
		{
			localPlayer.RemoveHealingEffect();
		}
		GameApp.GetInstance().GetGameWorld().BossState = EBossState.NORMAL;
		NumberManager.GetInstance().Clear();
	}

	public void AddToDeletSummoned(SummonedItem summoned)
	{
		if (!toDeleteSummonedList.Contains(summoned))
		{
			toDeleteSummonedList.Add(summoned);
		}
	}

	public void AddSummoned(string summonedName, SummonedItem summoned)
	{
		if (!summonedList.ContainsKey(summonedName))
		{
			summonedList.Add(summonedName, summoned);
			summoned.AddEnemyCheckHitTimers(enemyList);
		}
	}

	public void RemoveSummoned(string summonedName)
	{
		if (summonedList.ContainsKey(summonedName))
		{
			summonedList[summonedName].RemoveAllEnemyCheckHitTimer();
			summonedList.Remove(summonedName);
		}
	}

	public SummonedItem GetSummonedByID(string summonedID)
	{
		if (summonedList.ContainsKey(summonedID))
		{
			return summonedList[summonedID];
		}
		return null;
	}

	public void Loop(float deltaTime)
	{
		if (vsClock != null)
		{
			vsClock.Update();
		}
		if (State != GameState.SwitchBossLevel)
		{
			GameApp.GetInstance().GetUserState().QuestInfo.UpdateQuestTime(GameApp.GetInstance().GetUserState().GetCurrentQuest());
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.Loop(deltaTime);
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
				{
					remotePlayer.Loop(deltaTime);
				}
			}
			foreach (Enemy value in enemyList.Values)
			{
				value.DoLogic(deltaTime);
			}
			foreach (Npc value2 in npcList.Values)
			{
				value2.Loop(Time.deltaTime);
			}
			foreach (SummonedItem value3 in summonedList.Values)
			{
				value3.Loop(deltaTime);
			}
			taskManager.Update();
			if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckSubQuestCompleted(101))
			{
				if (mGoldFruitMachineEffect != null && !mGoldFruitMachineEffect.activeSelf)
				{
					mGoldFruitMachineEffect.SetActive(true);
				}
				if (mMithrilFruitMachineEffect != null && !mMithrilFruitMachineEffect.activeSelf)
				{
					mMithrilFruitMachineEffect.SetActive(true);
				}
			}
		}
		ObjectPool[] array = mEffectPools;
		foreach (ObjectPool objectPool in array)
		{
			if (objectPool != null)
			{
				objectPool.AutoDestruct();
			}
		}
	}

	public void LateLoop(float deltaTime)
	{
		if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer() == null || GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.IsSamplingAnimation)
		{
		}
		foreach (SummonedItem value in summonedList.Values)
		{
			value.LateLoop(deltaTime);
		}
		foreach (SummonedItem toDeleteSummoned in toDeleteSummonedList)
		{
			RemoveSummoned(toDeleteSummoned.Name);
			toDeleteSummoned.GetOwnerPlayer().RemoveSummoned(toDeleteSummoned.Name);
			toDeleteSummoned.Destroy();
		}
		toDeleteSummonedList.Clear();
		foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
		{
			if (remotePlayer != null && remotePlayer.CurrentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && remotePlayer.CurrentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID)
			{
				remotePlayer.AdjustAnimationInLateUpdate(deltaTime);
			}
		}
	}

	public void SpawnItem(LootType lootType, Vector3 pos, int amount, short sequenceID)
	{
	}

	public void DestroyLoot(short sequenceID)
	{
		GameObject gameObject = GameObject.Find("Loot_" + sequenceID);
		if (gameObject != null)
		{
			UnityEngine.Object.DestroyObject(gameObject);
		}
	}

	public void VSTimeStopResume()
	{
		if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || vsClock == null || !GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
		{
			if (GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
				.Count == 0)
			{
				vsClock.StopTime();
			}
			else
			{
				vsClock.ResumeTime();
			}
		}
		else
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_TDM)
			{
				return;
			}
			bool flag = false;
			foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
			{
				if (remotePlayer.Team != GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.Team)
				{
					flag = true;
				}
			}
			if (flag)
			{
				vsClock.ResumeTime();
			}
			else
			{
				vsClock.StopTime();
			}
		}
	}

	public void UIPause(bool bPause)
	{
		if (bPause)
		{
			if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				Time.timeScale = 0f;
			}
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null)
			{
				localPlayer.InputController.Block = true;
			}
		}
		else
		{
			Time.timeScale = 1f;
			LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer2 != null)
			{
				localPlayer2.InputController.Block = false;
			}
		}
	}

	private void InitNpc()
	{
		npcList.Clear();
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.NPC);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			short id = GameConfig.GetInstance().npcConfig[gameObject.name].m_id;
			Npc npc = new Npc();
			npc.Init();
			npc.SetObject(gameObject);
			npcList.Add(id, npc);
		}
	}

	public QuestScript GetQuestScript(short id)
	{
		if (npcList.ContainsKey(id))
		{
			return npcList[id].GetObject().GetComponent<QuestScript>();
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.QUEST_BOARD);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			QuestScript component = gameObject.GetComponent<QuestScript>();
			if (component.GetNpcId() == id)
			{
				return gameObject.GetComponent<QuestScript>();
			}
		}
		return null;
	}

	public Npc GetNpc(short id)
	{
		if (npcList.ContainsKey(id))
		{
			return npcList[id];
		}
		Debug.Log("not find npc...");
		return null;
	}

	public Dictionary<short, Npc> GetNpc()
	{
		return npcList;
	}

	public Npc GetNpc(NpcType type)
	{
		foreach (KeyValuePair<short, Npc> npc in npcList)
		{
			if (npc.Value.m_type == type)
			{
				return npc.Value;
			}
		}
		return null;
	}

	public GameObject GetStore()
	{
		Npc npc = GetNpc(NpcType.Businessman);
		if (npc == null)
		{
			return null;
		}
		return GetNpc(NpcType.Businessman).GetObject();
	}

	public GameObject[] GetMultiplay()
	{
		return GameObject.FindGameObjectsWithTag("Multiplay");
	}

	public void UpdateNpcFlag()
	{
		foreach (KeyValuePair<short, Npc> npc in npcList)
		{
			npc.Value.UpdateFlag();
		}
	}

	public byte GetNextSummonedID()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			byte seatID = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetSeatID();
			for (byte b = 0; b <= summonedList.Count; b++)
			{
				byte result = (byte)((seatID << 4) | b);
				string key = "S_" + result;
				if (!summonedList.ContainsKey(key))
				{
					return result;
				}
			}
		}
		else
		{
			for (byte b2 = 0; b2 <= summonedList.Count; b2++)
			{
				string key2 = "S_" + b2;
				if (!summonedList.ContainsKey(key2))
				{
					return b2;
				}
			}
		}
		return 0;
	}

	private void InitSpawnPoints()
	{
		if (mapType != MapType.Instance)
		{
			return;
		}
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[39];
		NameComparer comparer = new NameComparer();
		GameObject[] collection = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
		List<GameObject> list = new List<GameObject>(collection);
		list.Sort(comparer);
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<EnemyType> list2 = new List<EnemyType>();
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 0, string.Empty, false);
			dictionary.Add(data, i);
		}
		byte b = 0;
		foreach (GameObject item in list)
		{
			if (!dictionary.ContainsKey(item.name))
			{
				continue;
			}
			int iRow = dictionary[item.name];
			EnemySpawnPointScript component = item.GetComponent<EnemySpawnPointScript>();
			component.PointID = b;
			b++;
			int data2 = unitDataTable.GetData(iRow, 1, 0, false);
			int data3 = unitDataTable.GetData(iRow, 2, 0, false);
			List<SpawnGroup> list3 = new List<SpawnGroup>();
			for (int j = 0; j < 6; j++)
			{
				string data4 = unitDataTable.GetData(iRow, j * 4 + 3, string.Empty, false);
				if (!(data4 != string.Empty))
				{
					continue;
				}
				if (GameConfig.GetInstance().enemyConfig.ContainsKey(data4))
				{
					EnemyConfig enemyConfig = GameConfig.GetInstance().enemyConfig[data4];
					int uniqueID = enemyConfig.UniqueID;
					string enemyType = enemyConfig.EnemyType;
					EnemyType enemyType2 = EnemyType.HATI;
					if (Enum.IsDefined(typeof(EnemyType), enemyType))
					{
						enemyType2 = (EnemyType)(int)Enum.Parse(typeof(EnemyType), enemyType, true);
						SpawnGroup spawnGroup = new SpawnGroup();
						spawnGroup.mUniqueID = uniqueID;
						spawnGroup.mType = enemyType2;
						spawnGroup.mCount = unitDataTable.GetData(iRow, j * 4 + 4, 0, false);
						spawnGroup.mLowerLevel = unitDataTable.GetData(iRow, j * 4 + 5, 0, false);
						spawnGroup.mUpperLevel = unitDataTable.GetData(iRow, j * 4 + 6, 0, false);
						if (j < data2)
						{
							component.AddEnemyToSpawnGroupList(spawnGroup);
						}
						else
						{
							list3.Add(spawnGroup);
						}
						if (!list2.Contains(spawnGroup.mType))
						{
							list2.Add(spawnGroup.mType);
						}
					}
					else
					{
						Debug.LogError("Invalid Enemy Type: " + enemyType);
					}
				}
				else
				{
					Debug.LogError("Can't find " + data4 + " in enemy data table!!!");
				}
			}
			data3 = Mathf.Min(data3, list3.Count);
			for (int k = 0; k < data3; k++)
			{
				int index = UnityEngine.Random.Range(0, list3.Count);
				component.AddEnemyToSpawnGroupList(list3[index]);
				list3.RemoveAt(index);
			}
		}
		GameObject[] collection2 = GameObject.FindGameObjectsWithTag(TagName.QUEST_ENEMY_SPAWN);
		List<GameObject> list4 = new List<GameObject>(collection2);
		list4.Sort(comparer);
		Dictionary<string, QuestEnemySpawnConfig> questEnemySpawnConfig = GameConfig.GetInstance().questEnemySpawnConfig;
		foreach (GameObject item2 in list4)
		{
			if (questEnemySpawnConfig.ContainsKey(item2.name))
			{
				QuestEnemySpawnScript component2 = item2.GetComponent<QuestEnemySpawnScript>();
				QuestEnemySpawnConfig questEnemySpawnConfig2 = questEnemySpawnConfig[item2.name];
				if (null != component2)
				{
					component2.QuestId = questEnemySpawnConfig2.QuestId;
					component2.PointID = b;
					b++;
				}
				foreach (EnemySpawnInfo enemyInfo in questEnemySpawnConfig2.EnemyInfos)
				{
					if (!list2.Contains(enemyInfo.EType))
					{
						list2.Add(enemyInfo.EType);
					}
				}
			}
			else
			{
				Debug.LogError("No config for quest point: " + item2.name);
			}
		}
		foreach (EnemyType item3 in list2)
		{
		}
	}

	private void InitCovers()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.COVER);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			CoverLinkScript component = gameObject.GetComponent<CoverLinkScript>();
			if (null != component && component.Type == CoverType.STAND && null != component.Expose && null != component.Hide)
			{
				Vector3 lhs = component.Expose.transform.position - component.Hide.transform.position;
				if (Vector3.Cross(lhs, component.Expose.transform.forward).y > 0f)
				{
					component.Direction = CoverDirection.LEFT;
				}
				else
				{
					component.Direction = CoverDirection.RIGHT;
				}
			}
		}
	}

	public SummonedItem GetMasterSummonedItem()
	{
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value != null && value.IsMaster)
			{
				return value;
			}
		}
		return null;
	}

	public bool PlayOnHitElementEffect(Vector3 position, ElementType elementType)
	{
		bool result = false;
		switch (elementType)
		{
		case ElementType.Fire:
			result = true;
			GetEffectPool(EffectPoolType.FIRE_BULLET_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
			break;
		case ElementType.Shock:
			result = true;
			GetEffectPool(EffectPoolType.SHOCK_BULLET_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
			break;
		case ElementType.Corrosive:
			result = true;
			GetEffectPool(EffectPoolType.CORROSIVE_BULLET_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
			break;
		}
		return result;
	}
}
