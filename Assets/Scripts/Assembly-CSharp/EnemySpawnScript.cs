using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour, EffectsCameraListener, UIArenaResultListener
{
	protected List<Enemy> mSpawnedEnemyList;

	protected Player player;

	protected GameWorld gameWorld;

	protected float lastUpdateTime = -1000f;

	private SpawnConfig spawnConfigInfo;

	private int playerIndex;

	public float spawnInterval = 1f;

	public int maxSpawns = 7;

	protected List<EnemySpawnItem> spawnItems;

	protected static EnemySpawnScript instance;

	private int spIndex;

	private int skySpIndex;

	public bool enableSpawn;

	protected int stableID;

	public static int InitSpawnTableID;

	protected bool isProcessingGameEnd;

	protected int enemyCountRetainedCurrentRound;

	protected int waveIndex = 1;

	protected int maxEnemyLevel = 1;

	protected int enemySpawning;

	protected int startWaveLevel;

	protected GameObject[] spawnPoints;

	protected GameObject[] skySpawnPoints;

	protected int spCount;

	protected int skySpCount;

	protected int TotalCash;

	protected int TotalExp;

	protected bool mIsDisconnectedInMultiplay;

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

	public bool IsDisconnectedInMultiplay
	{
		get
		{
			return mIsDisconnectedInMultiplay && isProcessingGameEnd;
		}
	}

	public static EnemySpawnScript GetInstance()
	{
		return instance;
	}

	public void StartSpawn()
	{
		enableSpawn = true;
	}

	public void StopSpawn()
	{
		enableSpawn = false;
	}

	public void SetSpawnTableID(int stableID)
	{
		this.stableID = stableID;
	}

	private IEnumerator Start()
	{
		mIsDisconnectedInMultiplay = false;
		spIndex = 0;
		skySpIndex = 0;
		instance = GameObject.Find("TraditionalSpawns").GetComponent<EnemySpawnScript>();
		mSpawnedEnemyList = new List<Enemy>();
		while (!InGameUIScript.bInited)
		{
			yield return 0;
		}
		startWaveLevel = 0;
		enemySpawning = 0;
		NameComparer nameComparer = new NameComparer();
		spawnPoints = GameObject.FindGameObjectsWithTag(TagName.TRADITIONAL_SPAWN_POINT);
		skySpawnPoints = GameObject.FindGameObjectsWithTag(TagName.SKY_SPAWN_POINT);
		List<GameObject> spawnPointList = new List<GameObject>(spawnPoints);
		spawnPointList.Sort(nameComparer);
		List<GameObject> skySpawnPointList = new List<GameObject>(skySpawnPoints);
		skySpawnPointList.Sort(nameComparer);
		for (int i = 1; i < spawnPointList.Count; i++)
		{
			if (spawnPointList[i].name == spawnPointList[i - 1].name)
			{
				Debug.LogError("[SpawnPoint]Same name: " + spawnPointList[i].name);
			}
		}
		for (int j = 1; j < skySpawnPointList.Count; j++)
		{
			if (skySpawnPointList[j].name == skySpawnPointList[j - 1].name)
			{
				Debug.LogError("[SkySpawnPoint]Same name: " + skySpawnPointList[j].name);
			}
		}
		spCount = spawnPointList.Count;
		skySpCount = skySpawnPointList.Count;
		for (int n = 0; n < 10; n++)
		{
			yield return 0;
		}
		gameWorld = GameApp.GetInstance().GetGameWorld();
		SetSpawnTableID(InitSpawnTableID);
		spawnConfigInfo = new SpawnConfig();
		spawnConfigInfo.Rounds = new List<Round>();
		UnitDataTable dataTable = Res2DManager.GetInstance().vDataTable[53 + stableID];
		int waveNum = 1;
		Round round3 = new Round();
		Round newWave = round3;
		newWave.EnemyInfos = new List<EnemySpawnInfo>();
		spawnConfigInfo.Rounds.Add(newWave);
		for (int m = 0; m < dataTable.sRows; m++)
		{
			int nexWaveNum;
			for (nexWaveNum = dataTable.GetData(m, 0, 0, false); nexWaveNum > waveNum; waveNum++)
			{
				newWave = new Round
				{
					EnemyInfos = new List<EnemySpawnInfo>()
				};
				spawnConfigInfo.Rounds.Add(newWave);
			}
			EnemySpawnInfo info = new EnemySpawnInfo();
			string enemyCallName = dataTable.GetData(m, 1, string.Empty, false);
			Debug.Log("enemyCallName : " + enemyCallName);
			EnemyConfig eConfig = GameConfig.GetInstance().enemyConfig[enemyCallName];
			info.EType = (EnemyType)(int)Enum.Parse(typeof(EnemyType), eConfig.EnemyType, true);
			info.UniqueID = eConfig.UniqueID;
			info.From = SpawnFromType.Door;
			info.Count = dataTable.GetData(m, 2, 0, false);
			info.MinLevel = dataTable.GetData(m, 4, 0, false);
			info.MaxLevel = dataTable.GetData(m, 3, 0, false);
			newWave.EnemyInfos.Add(info);
			Debug.Log(string.Concat("prepare spawn ", info.EType, ":", info.Count, "   wave:", waveNum));
			if (m == 0)
			{
				startWaveLevel = nexWaveNum;
			}
		}
		foreach (Round round2 in spawnConfigInfo.Rounds)
		{
			foreach (EnemySpawnInfo info4 in round2.EnemyInfos)
			{
				string[] enemyTextureNames = AssetBundleName.ENEMY_TEXTURE[(int)info4.EType];
				string[] array = enemyTextureNames;
				foreach (string t in array)
				{
					GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(t, StreamingDataType.TEXTURE);
					while (!GameApp.GetInstance().GetSceneStreaingManager().isLoad(t))
					{
						yield return 0;
					}
				}
				string enemyDataName = AssetBundleName.ENEMY[(int)info4.EType];
				GameApp.GetInstance().GetSceneStreaingManager().AddToLoadData(enemyDataName, StreamingDataType.ENEMY);
				while (!GameApp.GetInstance().GetSceneStreaingManager().isLoad(enemyDataName))
				{
					yield return 0;
				}
			}
		}
		byte currentEnemyID = 0;
		waveIndex = 1;
		maxEnemyLevel = 1;
		int totalWave = spawnConfigInfo.Rounds.Count;
		int survivalEnemyLevelAdd = 0;
		for (int w = 0; w < totalWave; w++)
		{
			Round round = spawnConfigInfo.Rounds[w];
			int enemyCount3 = 0;
			if (round.EnemyInfos.Count == 0)
			{
				continue;
			}
			NextWaveWarning(waveIndex);
			enemyCountRetainedCurrentRound = 0;
			foreach (EnemySpawnInfo info3 in round.EnemyInfos)
			{
				enemyCountRetainedCurrentRound += info3.Count;
			}
			for (int l = 0; l < 10; l++)
			{
				UserStateHUD.GetInstance().InfoBox.SetNumberInfo(LocalizationManager.GetInstance().GetString("MSG_ARENA_COUNTDOWN"), 10 - l);
				yield return new WaitForSeconds(1f);
			}
			UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_ARENA_START").Replace("%d", string.Empty + (w + 1 + survivalEnemyLevelAdd)));
			NotifyEnemyCountRetained();
			StartNextWave(w);
			foreach (EnemySpawnInfo info2 in round.EnemyInfos)
			{
				Debug.Log(string.Concat("start spawn ", info2.EType, ":", info2.Count));
				for (int k = 0; k < info2.Count; k++)
				{
					yield return new WaitForSeconds(spawnInterval);
					enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(0);
					while (enemyCount3 + enemySpawning >= maxSpawns)
					{
						yield return new WaitForSeconds(2f);
						enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(0);
					}
					while (!enableSpawn)
					{
						yield return new WaitForSeconds(1f);
					}
					byte enemyID = currentEnemyID;
					currentEnemyID++;
					int level2 = UnityEngine.Random.Range(info2.MinLevel, info2.MaxLevel + 1);
					level2 += survivalEnemyLevelAdd;
					if (maxEnemyLevel < level2)
					{
						maxEnemyLevel = level2;
					}
					Vector3 spawnPosition2 = Vector3.zero;
					if (info2.EType == EnemyType.ELITE_OBSIDIAN || info2.EType == EnemyType.OBSIDIAN)
					{
						spawnPosition2 = skySpawnPointList[skySpIndex].transform.position;
						skySpIndex++;
						if (skySpIndex >= skySpCount)
						{
							skySpIndex = 0;
						}
					}
					else
					{
						spawnPosition2 = spawnPointList[spIndex].transform.position;
						spIndex++;
						if (spIndex >= spCount)
						{
							spIndex = 0;
						}
					}
					GameApp.GetInstance().GetGameWorld().AddEnemy(0, enemyID, info2.EType, false, (byte)level2, info2.UniqueID, spawnPosition2);
					enemySpawning++;
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						RequireEnemyInPointRequest requireRequest = new RequireEnemyInPointRequest(0, enemyID);
						GameApp.GetInstance().GetNetworkManager().SendRequest(requireRequest);
					}
					else
					{
						InitEnemy(enemyID);
					}
				}
			}
			enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(0);
			while (enemyCount3 + enemySpawning > 0)
			{
				yield return new WaitForSeconds(1f);
				enemyCount3 = gameWorld.GetEnemiesSpawnedInPointCount(0);
			}
			currentEnemyID = 0;
			foreach (Enemy enemy in mSpawnedEnemyList)
			{
				enemy.Deactivate();
				GameApp.GetInstance().GetGameScene().RemoveEnemy(enemy.Name);
			}
			mSpawnedEnemyList.Clear();
			gameWorld.RemovePoint(0);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				RemoveSpawnPointRequest rspRequest = new RemoveSpawnPointRequest(0, true);
				GameApp.GetInstance().GetNetworkManager().SendRequest(rspRequest);
			}
			if (GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Arena_Survival && w == totalWave - 1)
			{
				w--;
				survivalEnemyLevelAdd++;
			}
			waveIndex++;
		}
		OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), true);
	}

	public void NotifyOneEnemyDead()
	{
		enemyCountRetainedCurrentRound--;
		NotifyEnemyCountRetained();
	}

	public void NotifyEnemyCountRetained()
	{
		UserStateHUD.GetInstance().InfoBox.SetNumberInfo(LocalizationManager.GetInstance().GetString("MSG_ARENA_ENEMY_COUNT"), enemyCountRetainedCurrentRound);
	}

	public void InitEnemy(byte enemyID)
	{
		Enemy enemy = GameApp.GetInstance().GetGameWorld().GetEnemy(0, enemyID);
		if (enemy != null)
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				enemy.IsMasterPlayer = true;
			}
			enemy.SpawnType = ESpawnType.ARENA;
			enemy.Activate();
			GameApp.GetInstance().GetGameScene().AddEnemy(enemy.Name, enemy);
			if (!mSpawnedEnemyList.Contains(enemy))
			{
				mSpawnedEnemyList.Add(enemy);
			}
		}
		enemySpawning--;
	}

	private void NextWaveWarning(int waveIndex)
	{
		Debug.Log("New Wave Coming..........................################@@@@@@@@@@@@@@@@@@@@@" + waveIndex);
	}

	private void StartNextWave(int wave)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ITEM);
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		if (wave != startWaveLevel - 1)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			for (int j = 1; j < 9; j++)
			{
				userState.AddBulletByWeaponType((WeaponType)j, (short)Mathf.CeilToInt((float)userState.GetMaxBulletByWeaponType((WeaponType)j) * 0.2f));
			}
			UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_ARENA_BULLET_BONUS").Replace("%d", 20f + "%"));
			GiveArenaBonus(false);
		}
	}

	public void DisconnectMulitplay()
	{
		mIsDisconnectedInMultiplay = true;
		OnArenaGameEnd(GameApp.GetInstance().GetGameWorld().GetLocalPlayer(), false);
	}

	public void OnArenaGameEnd(Player player, bool bWin)
	{
		if (!isProcessingGameEnd)
		{
			Debug.Log("Start wave-------" + startWaveLevel + "-------Current wave------" + waveIndex);
			UserStateHUD.GetInstance().InfoBox.CloseNumberInfo();
			isProcessingGameEnd = true;
			enableSpawn = false;
			this.player = player;
			player.StopSpecialAction();
			if (bWin)
			{
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.SetState(Player.LOSE_STATE);
			}
			GiveArenaBonus(true);
		}
	}

	public void GiveArenaBonus(bool isEndArena)
	{
		GameApp.GetInstance().GetUserState().ArenaRewards = new ArenaRewardsInfo();
		GameApp.GetInstance().GetUserState().ArenaRewards.item = null;
		int num = 0;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			num = GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
				.Count;
		}
		int[] array = new int[4] { 6400, 7200, 8000, 8800 };
		int[] array2 = new int[4] { 600, 700, 800, 900 };
		int[] array3 = new int[4] { 2, 3, 3, 3 };
		int num2 = startWaveLevel + waveIndex - 1;
		float num3 = 0f;
		if (waveIndex != 1)
		{
			num3 = (float)(int)((float)((startWaveLevel + num2 - 1) * (num2 - startWaveLevel) * 100) / 72f) / 100f;
		}
		Debug.Log("WAVE FACTOR-----" + num3);
		int num4 = (int)(num3 * (float)array[num]);
		num4 -= TotalCash;
		TotalCash += num4;
		int num5 = (int)(num3 * (float)array2[num]);
		num5 -= TotalExp;
		TotalExp += num5;
		int num6 = (int)(num3 * (float)array3[num]);
		GameApp.GetInstance().GetUserState().AddCash(num4);
		GameApp.GetInstance().GetUserState().AddExp(num5);
		Debug.Log("Gold---This wave: " + num4 + "  All wave: " + TotalCash + "  TotalMoney: " + GameApp.GetInstance().GetUserState().GetCash());
		Debug.Log("Exp---This wave: " + num5 + "  All wave: " + TotalExp + "  TotalExp: " + GameApp.GetInstance().GetUserState().GetExp());
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && isEndArena)
		{
			GameApp.GetInstance().GetGlobalState().AddMithril(num6);
		}
		else
		{
			num6 = 0;
		}
		GameObject item = null;
		if (isEndArena && num3 != 0f)
		{
			ItemQuality itemQuality = ItemQuality.Common;
			float num7 = UnityEngine.Random.Range(0f, 100f);
			if (num7 < 0.5f * num3)
			{
				itemQuality = ItemQuality.Legendary;
			}
			else if (num7 < 0.5f * num3)
			{
				itemQuality = ItemQuality.Epic;
			}
			else if (num7 < 1f * num3)
			{
				itemQuality = ItemQuality.Rare;
			}
			else if (num7 < 3f * num3)
			{
				itemQuality = ItemQuality.Uncommon;
			}
			if (itemQuality != ItemQuality.Common)
			{
				byte b = (byte)GameApp.GetInstance().GetUserState().GetCharLevel();
				int num8 = UnityEngine.Random.Range(0, 100);
				byte b2 = 3;
				if (num8 < 10)
				{
					b2 = 0;
				}
				else if (num8 < 25)
				{
					b2 = 1;
				}
				else if (num8 < 55)
				{
					b2 = 2;
				}
				if (b2 >= b)
				{
					b2 = (byte)(b - 1);
				}
				b -= b2;
				ItemClasses[] array4 = new ItemClasses[8]
				{
					ItemClasses.SubmachineGun,
					ItemClasses.AssultRifle,
					ItemClasses.Pistol,
					ItemClasses.Revolver,
					ItemClasses.Shotgun,
					ItemClasses.Sniper,
					ItemClasses.RPG,
					ItemClasses.U_Shield
				};
				int num9 = UnityEngine.Random.Range(0, 8);
				ItemClasses itemClass = array4[num9];
				int number = 1;
				short num10 = 0;
				string itemName = string.Empty;
				byte itemType = 0;
				GameApp.GetInstance().GetLootManager().GetRandomNameNumberTypeByItemClass(itemClass, ref itemName, ref number, ref itemType);
				GameObject obj = new GameObject();
				ItemBase itemBase = GameApp.GetInstance().GetLootManager().CreateItemBaseFromGameObject(obj, b, itemClass, (byte)number, itemQuality, num10);
				GameApp.GetInstance().GetUserState().ArenaRewards.item = itemBase.mNGUIBaseItem;
				item = GameApp.GetInstance().GetLootManager().CreateIcon(itemBase);
				UnityEngine.Object.Destroy(itemBase);
				if (GameApp.GetInstance().GetUserState().ArenaRewards.item != null && !GameApp.GetInstance().GetUserState().ItemInfoData.BackPackIsFull())
				{
					NGUIGameItem value = new NGUIGameItem(num10, GameApp.GetInstance().GetUserState().ArenaRewards.item);
					ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
					for (int i = 0; i < itemInfoData.BackpackSlotCount; i++)
					{
						if (itemInfoData.BackPackItems[i] == null)
						{
							itemInfoData.BackPackItems[i] = value;
							GameApp.GetInstance().GetUserState().ArenaRewards.item = null;
							break;
						}
					}
				}
			}
		}
		GameApp.GetInstance().Save();
		if (isEndArena)
		{
			UIArenaResult.Show(TotalExp, TotalCash, num6, item, this);
		}
	}

	public void OnArenaResultConfirm()
	{
		Debug.Log("Game Ends...  Teleport To City Bar!");
		if (IsDisconnectedInMultiplay)
		{
			Time.timeScale = 1f;
			GameApp.GetInstance().CloseConnectionGameServer();
			if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() != 6)
			{
				InGameMenuManager.GetInstance().Close();
			}
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(6, true, false, false);
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			if (gameWorld != null)
			{
				gameWorld.ExitMultiplayerMode();
			}
		}
		else
		{
			EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
		}
		mIsDisconnectedInMultiplay = false;
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		if (player != null)
		{
			if (player.Hp <= 0)
			{
				player.ReSpawnFromArena();
			}
			else
			{
				player.SetState(Player.IDLE_STATE);
			}
		}
		GameApp.GetInstance().GetGameMode().SubModePlay = SubMode.Story;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ChangePlayerSubGameModeRequest request = new ChangePlayerSubGameModeRequest(0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			PlayerLeaveSpawnPointRequest request2 = new PlayerLeaveSpawnPointRequest(0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
		isProcessingGameEnd = false;
		GameApp.GetInstance().GetGameScene().LeaveScene();
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(Arena.GetInstance().LastSceneID);
		Application.LoadLevel(sceneConfig.SceneFileName);
	}
}
