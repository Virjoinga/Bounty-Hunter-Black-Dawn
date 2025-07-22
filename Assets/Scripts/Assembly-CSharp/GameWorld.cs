using System.Collections.Generic;
using UnityEngine;

public class GameWorld
{
	protected LocalPlayer mLocalPlayer;

	protected List<RemotePlayer> mRemotePlayerList = new List<RemotePlayer>();

	protected Dictionary<byte, CityInfo> mCityDictionary = new Dictionary<byte, CityInfo>();

	public static bool NeedGetSeneceState;

	public Timer mShopItemRefreshTimer = new Timer();

	private float mLastUpdateRealTime;

	public Timer mBlackMarketRefreshTimer = new Timer();

	public bool mBlackMarketInfoPoped;

	public GameObject globalTempItemObject = new GameObject();

	private EBossState mBossState;

	public byte CurrentSceneID { get; set; }

	public int PortalID { get; set; }

	public EBossState BossState
	{
		get
		{
			return mBossState;
		}
		set
		{
			mBossState = value;
		}
	}

	public SceneConfig GetSceneConfig(int sceneID)
	{
		Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
		foreach (SceneConfig value in sceneConfig.Values)
		{
			if (value.SceneID == sceneID)
			{
				return value;
			}
		}
		return null;
	}

	public GameObject GetPlayerSpawnPoint()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			FirstSceneStreamingScript component = gameObject.GetComponent<FirstSceneStreamingScript>();
			if (null == component || component.PortalID == PortalID)
			{
				return gameObject;
			}
		}
		if (array.Length > 0)
		{
			return array[0];
		}
		return null;
	}

	public void ChangeGameScene(byte cityID, byte sceneID)
	{
		GameApp.GetInstance().GetUserState().SetCurrentCityID(cityID);
		CurrentSceneID = sceneID;
	}

	public LocalPlayer GetLocalPlayer()
	{
		return mLocalPlayer;
	}

	public LocalPlayer InitLocalPlayer()
	{
		if (mLocalPlayer == null)
		{
			mLocalPlayer = new LocalPlayer();
			mLocalPlayer.Init();
		}
		return mLocalPlayer;
	}

	public void DestroyLocalPlayer()
	{
		if (mLocalPlayer != null)
		{
			Object.Destroy(mLocalPlayer.GetObject());
			mLocalPlayer = null;
		}
	}

	public RemotePlayer GetRemotePlayerByUserID(int userID)
	{
		RemotePlayer result = null;
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (userID == mRemotePlayer.GetUserID())
			{
				result = mRemotePlayer;
				break;
			}
		}
		return result;
	}

	public List<RemotePlayer> GetRemotePlayers()
	{
		return mRemotePlayerList;
	}

	public int GetPlayerCount()
	{
		return mRemotePlayerList.Count + 1;
	}

	public bool IsLocalPlayer(int playerID)
	{
		if (mLocalPlayer != null && playerID == mLocalPlayer.GetUserID())
		{
			return true;
		}
		return false;
	}

	public int GetInGamePlayerCount()
	{
		int num = 1;
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer != null && mRemotePlayer.State != Player.LOSE_STATE)
			{
				num++;
			}
		}
		return num;
	}

	public int GetPlayingPlayerCount()
	{
		int num = 0;
		if (mLocalPlayer.InPlayingState())
		{
			num++;
		}
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer.InPlayingState())
			{
				num++;
			}
		}
		return num;
	}

	public bool RemotePlayerExists(int channelID)
	{
		RemotePlayer remotePlayerByUserID = GetRemotePlayerByUserID(channelID);
		if (remotePlayerByUserID != null)
		{
			return true;
		}
		return false;
	}

	public void AddRemotePlayer(RemotePlayer player)
	{
		Debug.Log("AddRemotePlayer");
		if (Lobby.GetInstance().MasterPlayerID == player.GetUserID())
		{
			player.IsRoomMaster = true;
		}
		UserStateHUD.GetInstance().AddRemotePlayer(player);
		mRemotePlayerList.Add(player);
	}

	public void RemoveRemotePlayer(int userID)
	{
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer != null && mRemotePlayer.GetUserID() == userID)
			{
				UserStateHUD.GetInstance().RemoveRemotePlayer(mRemotePlayer.GetUserID().ToString());
				DestroyRemotePlayer(mRemotePlayer);
				mRemotePlayerList.Remove(mRemotePlayer);
				break;
			}
		}
	}

	private void DestroyRemotePlayer(RemotePlayer remotePlayer)
	{
		remotePlayer.ClearSummoned();
		remotePlayer.ClearWeaponList();
		Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (KeyValuePair<string, Enemy> item in enemies)
		{
			item.Value.RemovePlayer(remotePlayer.GetUserID());
		}
		Object.Destroy(remotePlayer.GetObject());
	}

	public void RemoveAllRemotePlayers()
	{
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer != null)
			{
				UserStateHUD.GetInstance().RemoveRemotePlayer(mRemotePlayer.GetUserID().ToString());
				DestroyRemotePlayer(mRemotePlayer);
			}
		}
		mRemotePlayerList.Clear();
		RecalculateEnemyAbility();
	}

	public void LeaveCurrentRoom()
	{
		if (Lobby.GetInstance().IsInRoomState())
		{
			LeaveRoomRequest request = new LeaveRoomRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		foreach (RemotePlayer mRemotePlayer in mRemotePlayerList)
		{
			if (mRemotePlayer != null)
			{
				DestroyRemotePlayer(mRemotePlayer);
			}
		}
		mRemotePlayerList.Clear();
	}

	public void ExitMultiplayerMode()
	{
		if (Lobby.GetInstance().IsInRoomState())
		{
			LeaveRoomRequest request = new LeaveRoomRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		GameApp.GetInstance().CloseConnectionGameServer();
		CurrentSceneID = GetSceneConfig(CurrentSceneID).FatherSceneID;
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		inGameUIScript.FrGoToPhase(6, true, false, false);
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		GetLocalPlayer().SetState(Player.IDLE_STATE);
		GetLocalPlayer().Hp = GetLocalPlayer().MaxHp;
		GetLocalPlayer().Shield = GetLocalPlayer().MaxShield;
		GetLocalPlayer().GetCharacterSkillManager().RemoveAllBuff();
		GetLocalPlayer().ClearSpeedDownEffect();
		GetLocalPlayer().RemoveHealingEffect();
		GetLocalPlayer().ClearExtraShield();
		if (HUDManager.instance != null)
		{
			Object.DestroyImmediate(HUDManager.instance.gameObject);
		}
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(CurrentSceneID);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			gameScene.LeaveScene();
		}
		GameApp.GetInstance().GetGameWorld().BossState = EBossState.NORMAL;
		GameApp.GetInstance().GetGameWorld().PortalID = 0;
		Application.LoadLevel(sceneConfig.SceneFileName);
	}

	public void CreateTeamSkills()
	{
	}

	private Enemy CreateEnemy(EnemyType enemyType)
	{
		Enemy result = null;
		switch (enemyType)
		{
		case EnemyType.HATI:
		case EnemyType.ELITE_HATI:
			result = new Hati();
			break;
		case EnemyType.OBSIDIAN:
		case EnemyType.ELITE_OBSIDIAN:
			result = new Obsidian();
			break;
		case EnemyType.MOB_REVOLVER:
		case EnemyType.ELITE_MOB_REVOLVER:
			result = new MobRevolver();
			break;
		case EnemyType.MOB_SMG:
		case EnemyType.ELITE_MOB_SMG:
			result = new MobSMG();
			break;
		case EnemyType.MERCENARY_ASSAULT_RIFLE:
		case EnemyType.ELITE_MERCENARY_ASSAULT_RIFLE:
			result = new MercenaryAssaultRifle();
			break;
		case EnemyType.MERCENARY_SHOTGUN:
		case EnemyType.ELITE_MERCENARY_SHOTGUN:
			result = new MercenaryShotgun();
			break;
		case EnemyType.GIANT:
		case EnemyType.ELITE_GIANT:
		case EnemyType.ELITE_SUPER_GIANT:
			result = new Giant();
			break;
		case EnemyType.GHOST:
		case EnemyType.ELITE_GHOST:
			result = new Ghost();
			break;
		case EnemyType.SPIT:
		case EnemyType.ELITE_SPIT:
			result = new Spit();
			break;
		case EnemyType.MONK:
		case EnemyType.ELITE_MONK:
			result = new Monk();
			break;
		case EnemyType.WORM:
		case EnemyType.ELITE_WORM:
			result = new Worm();
			break;
		case EnemyType.CYBERSHOOT:
		case EnemyType.ELITE_CYBERSHOOT:
			result = new Cybershoot();
			break;
		case EnemyType.CYPHER:
		case EnemyType.ELITE_CYPHER:
			result = new Cypher();
			break;
		case EnemyType.SHELL:
			result = new Shell();
			break;
		case EnemyType.MERCENARY_BOSS:
			result = new MercenaryBoss();
			break;
		case EnemyType.TERMINATOR:
			result = new Terminator();
			break;
		case EnemyType.FLOAT:
			result = new FloatCore();
			break;
		case EnemyType.FLOAT_CONTROLER:
			result = new FloatControler();
			break;
		case EnemyType.FLOAT_PROTECTOR:
			result = new FloatProtector();
			break;
		case EnemyType.SHELL_PROTOTYPE:
			result = new Shell_ProtoType();
			break;
		case EnemyType.BOSS_FATHER:
			result = new BossFather();
			break;
		case EnemyType.BOSS_TYLER:
			result = new BossTyler();
			break;
		case EnemyType.BOSS_MERCENARY:
			result = new BossMercenary();
			break;
		case EnemyType.BOSS_SHELL_PROTOTYPE:
			result = new BossPShell();
			break;
		}
		return result;
	}

	public void AddEnemy(byte pointID, byte enemyID, EnemyType enemyType, bool isElite, byte enemyLevel, int uniqueID, Vector3 spawnPosition)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (!mCityDictionary.ContainsKey(currentCityID))
		{
			mCityDictionary.Add(currentCityID, new CityInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			mCityDictionary[currentCityID].mSceneDictionary.Add(CurrentSceneID, new SceneInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.ContainsKey(pointID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.Add(pointID, new SpawnPointInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary[pointID].mEnemyDictionary.ContainsKey(enemyID))
		{
			Enemy enemy = CreateEnemy(enemyType);
			if (enemy != null)
			{
				enemy.EnemyType = enemyType;
				enemy.IsElite = isElite;
				enemy.UniqueID = uniqueID;
				enemy.Level = enemyLevel;
				enemy.PointID = pointID;
				enemy.EnemyID = enemyID;
				enemy.Name = "E_" + pointID + "_" + enemyID;
				enemy.SpawnPosition = spawnPosition;
				enemy.Init();
				mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary[pointID].mEnemyDictionary.Add(enemyID, enemy);
			}
		}
	}

	public void RemovePoint(byte pointID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.ContainsKey(pointID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.Remove(pointID);
		}
	}

	public bool HasSpawnPoint(byte pointID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.ContainsKey(pointID))
		{
			return true;
		}
		return false;
	}

	public Enemy GetEnemy(byte pointID, byte enemyID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.ContainsKey(pointID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary[pointID].mEnemyDictionary.ContainsKey(enemyID))
		{
			return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary[pointID].mEnemyDictionary[enemyID];
		}
		return null;
	}

	public Dictionary<byte, Enemy> GetEnemies(byte pointID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.ContainsKey(pointID))
		{
			return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary[pointID].mEnemyDictionary;
		}
		return new Dictionary<byte, Enemy>();
	}

	public void ClearCityInfo()
	{
		mCityDictionary.Clear();
	}

	public int GetEnemiesSpawnedInPointCount(byte pointID)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		Dictionary<string, Enemy> enemies = gameScene.GetEnemies();
		int num = 0;
		foreach (Enemy value in enemies.Values)
		{
			if (value.PointID == pointID && value.IsActive())
			{
				num++;
			}
		}
		return num;
	}

	public bool HasExploreBlock(byte pointID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.ContainsKey(pointID))
		{
			return true;
		}
		return false;
	}

	public void AddExploreItem(byte blockID, byte exploreItemId, ExploreItemStates explorable, short questID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (!mCityDictionary.ContainsKey(currentCityID))
		{
			mCityDictionary.Add(currentCityID, new CityInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			mCityDictionary[currentCityID].mSceneDictionary.Add(CurrentSceneID, new SceneInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.ContainsKey(blockID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.Add(blockID, new ExploreItemBlockInfo());
		}
		if (mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary[blockID].mExplorableStateDictionary.ContainsKey(exploreItemId))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary[blockID].mExplorableStateDictionary.Remove(exploreItemId);
		}
		ExploreItemStatesInfo exploreItemStatesInfo = new ExploreItemStatesInfo();
		exploreItemStatesInfo.mQuestID = questID;
		exploreItemStatesInfo.mState = explorable;
		mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary[blockID].mExplorableStateDictionary.Add(exploreItemId, exploreItemStatesInfo);
	}

	public void RemoveExploreInfo(byte blockID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.ContainsKey(blockID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.Remove(blockID);
		}
	}

	public Dictionary<byte, ExploreItemBlockInfo> GetExploreItemBlockDictionary()
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary;
		}
		return null;
	}

	public ExploreItemBlockInfo GetExplorItemBlock(byte blockID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID) && mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.ContainsKey(blockID))
		{
			return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary[blockID];
		}
		if (!mCityDictionary.ContainsKey(currentCityID))
		{
			mCityDictionary.Add(currentCityID, new CityInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			mCityDictionary[currentCityID].mSceneDictionary.Add(CurrentSceneID, new SceneInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.ContainsKey(blockID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary.Add(blockID, new ExploreItemBlockInfo());
		}
		Debug.Log("CurrentCityID " + currentCityID + " does not exist!?");
		Debug.Log("Block " + blockID + " does not exist!?");
		return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mExploreItemBlockDictionary[blockID];
	}

	public void UpdateExplorableInBlock()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.EXPLORE_ITEM);
		if (array == null)
		{
			return;
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject != null)
			{
				ExplorItemBlockScript component = gameObject.GetComponent<ExplorItemBlockScript>();
				if (component != null)
				{
					component.UpdateExplore();
				}
				Debug.Log("RefreshExplorableInBlock: " + gameObject.name);
			}
		}
	}

	public void UpdateExplorableInBlockForNet(short questId)
	{
		UpdateExploreItemBlockRequest request = new UpdateExploreItemBlockRequest(questId);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void DeactiveAllEnemiesInScene()
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (!mCityDictionary.ContainsKey(currentCityID) || !mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			return;
		}
		foreach (SpawnPointInfo value in mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.Values)
		{
			foreach (Enemy value2 in value.mEnemyDictionary.Values)
			{
				if (value2 != null)
				{
					value2.Deactivate();
				}
			}
		}
	}

	public void RecalculateEnemyAbility()
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (!mCityDictionary.ContainsKey(currentCityID) || !mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			return;
		}
		foreach (SpawnPointInfo value in mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mSpawnPointDictionary.Values)
		{
			foreach (Enemy value2 in value.mEnemyDictionary.Values)
			{
				if (value2 != null)
				{
					value2.CalculateAbility();
				}
			}
		}
	}

	public void DownloadEnemyInPoint(List<int> playerIDList, bool exist, byte pointID, List<EnemyStatus> enemyStatusList)
	{
		if (!exist)
		{
			RemovePoint(pointID);
		}
		foreach (EnemyStatus enemyStatus in enemyStatusList)
		{
			if (!exist)
			{
				AddEnemy(pointID, enemyStatus.mEnemyID, enemyStatus.mEnemyType, enemyStatus.mIsElite, enemyStatus.mEnemyLevel, enemyStatus.mUniqueID, enemyStatus.mSpawnPosition);
			}
			Enemy enemy = GetEnemy(pointID, enemyStatus.mEnemyID);
			if (enemy != null)
			{
				if (!enemy.IsHit)
				{
					enemy.Hp = enemyStatus.mHp;
					enemy.Shield = enemyStatus.mShield;
				}
				enemy.SpeedRate = (float)enemyStatus.mSpeedRate / 100f;
				enemy.SetPlayerList(playerIDList);
			}
		}
		InitEnemyInPoint(pointID);
	}

	public void InitEnemyInPoint(byte pointID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			EnemySpawnPointScript component = gameObject.GetComponent<EnemySpawnPointScript>();
			if (null != component && component.PointID == pointID)
			{
				component.InitEnemyFromGameWorld();
				break;
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag(TagName.QUEST_ENEMY_SPAWN);
		GameObject[] array4 = array3;
		foreach (GameObject gameObject2 in array4)
		{
			QuestEnemySpawnScript component2 = gameObject2.GetComponent<QuestEnemySpawnScript>();
			if (null != component2 && component2.PointID == pointID)
			{
				component2.InitEnemyFromGameWorld();
				break;
			}
		}
	}

	public void DownloadEnemy(List<int> playerIDList, byte pointID, byte enemyID, EnemyStatus enemyStatus)
	{
		Enemy enemy = GetEnemy(pointID, enemyID);
		if (enemy != null)
		{
			if (!enemy.IsHit)
			{
				enemy.Hp = enemyStatus.mHp;
				enemy.Shield = enemyStatus.mShield;
			}
			enemy.SpawnPosition = enemyStatus.mSpawnPosition;
			enemy.Level = enemyStatus.mEnemyLevel;
			enemy.IsElite = enemyStatus.mIsElite;
			enemy.SpeedRate = (float)enemyStatus.mSpeedRate / 100f;
			if (playerIDList.Count > 0)
			{
				enemy.SetPlayerList(playerIDList);
			}
		}
		InitEnemy(pointID, enemyID);
	}

	public void InitEnemy(byte pointID, byte enemyID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.QUEST_ENEMY_SPAWN);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			QuestEnemySpawnScript component = gameObject.GetComponent<QuestEnemySpawnScript>();
			if (null != component && component.PointID == pointID)
			{
				component.InitEnemy(enemyID);
				break;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
		{
			EnemySpawnScript.GetInstance().InitEnemy(enemyID);
		}
	}

	public void UploadEnemyInCurrentCity()
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID))
		{
			UploadEnemyInCityRequest request = new UploadEnemyInCityRequest(mCityDictionary[currentCityID].mSceneDictionary);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void DownloadExploreItemBlock(byte blockID, List<byte> itemIDs, List<ExploreItemStatesInfo> explorableStatesInfo)
	{
		ExplorItemBlockScript exploreItemBlockScript = GetExploreItemBlockScript(blockID);
		if (exploreItemBlockScript != null)
		{
			Debug.Log("Init");
			exploreItemBlockScript.Init();
		}
		ExploreItemBlockInfo explorItemBlock = GetExplorItemBlock(blockID);
		for (int i = 0; i < explorableStatesInfo.Count; i++)
		{
			Debug.Log("itemIDs[" + i + "] = " + itemIDs[i] + "  explorableStates[" + i + "] = " + explorableStatesInfo[i].mState);
			if (explorItemBlock.mExplorableStateDictionary.ContainsKey(itemIDs[i]))
			{
				explorItemBlock.mExplorableStateDictionary.Remove(itemIDs[i]);
			}
			ExploreItemStatesInfo exploreItemStatesInfo = new ExploreItemStatesInfo();
			exploreItemStatesInfo.mQuestID = explorableStatesInfo[i].mQuestID;
			ExploreItemStates exploreItemStates = (exploreItemStatesInfo.mState = explorableStatesInfo[i].mState);
			explorItemBlock.mExplorableStateDictionary.Add(itemIDs[i], exploreItemStatesInfo);
			Debug.Log(string.Concat("itemIDs[i]: ", itemIDs[i], ", ", exploreItemStates, ", ", explorableStatesInfo[i].mQuestID));
		}
		RefreshEploreItemBlock(blockID);
	}

	public void UpdateExploreItem(byte blockID, byte itemID, ExploreItemStatesInfo state)
	{
		ExploreItemBlockInfo explorItemBlock = GetExplorItemBlock(blockID);
		if (explorItemBlock.mExplorableStateDictionary.ContainsKey(itemID))
		{
			explorItemBlock.mExplorableStateDictionary.Remove(itemID);
		}
		explorItemBlock.mExplorableStateDictionary.Add(itemID, state);
		RefreshEploreItemBlock(blockID);
	}

	public void RefreshEploreItemBlock(byte blockID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.EXPLORE_ITEM);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			ExplorItemBlockScript component = gameObject.GetComponent<ExplorItemBlockScript>();
			if (null != component && component.BlockID == blockID)
			{
				component.RefreshExplorableStates();
				break;
			}
		}
	}

	public ExplorItemBlockScript GetExploreItemBlockScript(byte blockID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.EXPLORE_ITEM);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			ExplorItemBlockScript component = gameObject.GetComponent<ExplorItemBlockScript>();
			if (null != component && component.BlockID == blockID)
			{
				return component;
			}
		}
		return null;
	}

	public void CheckShopRefresh()
	{
		if (mShopItemRefreshTimer.Ready())
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.RefreshShopItems();
			return;
		}
		if (Time.timeScale == 0f)
		{
			float num = Time.realtimeSinceStartup - mLastUpdateRealTime;
			float interval = mShopItemRefreshTimer.GetTimeNeededToNextReady() - num;
			mShopItemRefreshTimer.SetTimer(interval, false);
		}
		if (GameApp.GetInstance().GetUserState() != null)
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.TimeToShopRefresh = mShopItemRefreshTimer.GetTimeNeededToNextReady();
		}
	}

	public void CheckBlackMarketRefresh()
	{
		if (mBlackMarketRefreshTimer.Ready())
		{
			if (!mBlackMarketInfoPoped && ShopUIScript.mInstance == null)
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_NEW_ITEM_ON_SALE"));
				mBlackMarketInfoPoped = true;
			}
		}
		else
		{
			if (Time.timeScale == 0f && (!(ShopUIScript.mInstance != null) || ShopUIScript.mInstance.CurrentPage != ShopPageType.BlackMarket))
			{
				float num = Time.realtimeSinceStartup - mLastUpdateRealTime;
				float interval = mBlackMarketRefreshTimer.GetTimeNeededToNextReady() - num;
				mBlackMarketRefreshTimer.SetTimer(interval, false);
			}
			BlackMarketIcon.BlackMarketTimeToReady = mBlackMarketRefreshTimer.GetTimeNeededToNextReady();
		}
		mLastUpdateRealTime = Time.realtimeSinceStartup;
	}

	public void CheckRolePlayTimer()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		if (userState != null)
		{
			userState.OperInfo.AddInfo(OperatingInfoType.ROLE_PLAY_TIME, (int)(Time.deltaTime * 1000f));
		}
	}

	public void ChangeSubModeFromBossToStory()
	{
		GameApp.GetInstance().GetGameMode().SubModePlay = SubMode.Story;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ChangePlayerSubGameModeRequest request = new ChangePlayerSubGameModeRequest(0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void OnLoseBossBattle()
	{
		if (BossArea.instance != null && BossArea.instance.m_Trigger != null)
		{
			BossArea.instance.m_Trigger.m_IsDoorOpen = true;
		}
		mBossState = EBossState.LOSE;
	}

	public void OnWinBossBattle()
	{
		mBossState = EBossState.WIN;
		GameApp.GetInstance().GetGameWorld().ChangeSubModeFromBossToStory();
		if (BossArea.instance != null && BossArea.instance.m_Trigger != null)
		{
			BossArea.instance.m_Trigger.m_IsDoorOpen = true;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.BOSS_PORTAL);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			MapEntranceScript component = gameObject.GetComponent<MapEntranceScript>();
			if (null != component)
			{
				component.enabled = true;
			}
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag(TagName.BOSS_PORTAL_EFFECT);
		GameObject[] array4 = array3;
		foreach (GameObject gameObject2 in array4)
		{
			int childCount = gameObject2.transform.GetChildCount();
			for (int k = 0; k < childCount; k++)
			{
				gameObject2.transform.GetChild(k).gameObject.SetActive(true);
			}
		}
	}

	public bool IsBoss(int uniqueId)
	{
		return uniqueId == 1190 || uniqueId == 1191;
	}

	public bool GetChestOpenState(int chestID)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (!mCityDictionary.ContainsKey(currentCityID))
		{
			mCityDictionary.Add(currentCityID, new CityInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			mCityDictionary[currentCityID].mSceneDictionary.Add(CurrentSceneID, new SceneInfo());
		}
		if (!mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.ContainsKey(chestID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.Add(chestID, false);
		}
		return mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState[chestID];
	}

	public void SetChestOpenState(int chestID, bool isOpen)
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			if (mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.ContainsKey(chestID))
			{
				mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.Remove(chestID);
			}
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.Add(chestID, isOpen);
		}
	}

	public void ClearChestOpenState()
	{
		byte currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
		if (mCityDictionary.ContainsKey(currentCityID) && mCityDictionary[currentCityID].mSceneDictionary.ContainsKey(CurrentSceneID))
		{
			mCityDictionary[currentCityID].mSceneDictionary[CurrentSceneID].mChestOpenState.Clear();
		}
	}

	public bool CheckFinalMainQuest(Quest quest)
	{
		if (quest.m_commonId == 66)
		{
			return true;
		}
		return false;
	}

	public bool IsInstanceScene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 33 || currentSceneID == 39 || currentSceneID == 34 || currentSceneID == 38 || currentSceneID == 43 || currentSceneID == 49;
	}

	public bool IsCityScene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 32 || currentSceneID == 37 || currentSceneID == 35 || currentSceneID == 42;
	}

	public bool IsVSScene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 44 || currentSceneID == 45 || currentSceneID == 47 || currentSceneID == 48;
	}

	public bool IsVS2Scene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 45 || currentSceneID == 48;
	}

	public bool IsVS1Scene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 44 || currentSceneID == 47;
	}

	public bool Is1V1VSScene()
	{
		int currentSceneID = CurrentSceneID;
		return currentSceneID == 47 || currentSceneID == 48;
	}
}
