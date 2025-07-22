using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotGameScene
{
	public byte mCityID;

	public byte mSceneID;

	public int mPortalID;

	public MapType mMapType;

	private RobotUser mOwner;

	private float mChangeSceneInterval;

	private float mLastChangeSceneTime;

	private float mLastSendInvitationTime;

	private float mChangePositionInterval;

	private float mLastChangePositionTime;

	private Dictionary<ESceneID, Dictionary<string, byte>> mSpawnNameDictionary;

	private Dictionary<byte, EnemySpawnPointScript> mSpawnScriptDictionary;

	private Dictionary<ESceneID, Dictionary<int, RobotPosition>> mRobotPositionDictionary;

	private int mCurrentPositionID;

	private List<byte> mCurrentPointIDList;

	private GameObject mObject;

	private bool mIn1V1Queue;

	private float mLastSend1V1Time;

	private bool mCanSend1V1;

	private byte mRound1V1;

	private float mInterval1V1;

	public RobotGameScene(RobotUser robotUser)
	{
		mOwner = robotUser;
		mObject = mOwner.gameObject;
		mCityID = (byte)UnityEngine.Random.Range(0, 3);
		mIn1V1Queue = false;
		mLastSend1V1Time = Time.time;
		mRound1V1 = 0;
		mCanSend1V1 = false;
		if (mCityID == 0)
		{
			mSceneID = 32;
		}
		else if (mCityID == 1)
		{
			mSceneID = 37;
		}
		else if (mCityID == 2)
		{
			mSceneID = 35;
		}
		mPortalID = 0;
		mRobotPositionDictionary = new Dictionary<ESceneID, Dictionary<int, RobotPosition>>();
		mRobotPositionDictionary.Add(ESceneID.CITY_0, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.CITY_0].Add(0, new RobotPosition(new Vector3(-15.26201f, 0.1799994f, 2.508512f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.CITY_1, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.CITY_1].Add(0, new RobotPosition(new Vector3(-4.728905f, 0.1799982f, 5.65425f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.CITY_2, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.CITY_2].Add(0, new RobotPosition(new Vector3(10.68762f, -4.096485f, 45.22125f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.ARENA_1, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.ARENA_1].Add(0, new RobotPosition(new Vector3(154.4764f, -43.56271f, 88.32655f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.ARENA_2, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.ARENA_2].Add(0, new RobotPosition(new Vector3(138.0526f, 0.17998914f, 88.75349f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.INSTANCE_0, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.INSTANCE_0].Add(0, new RobotPosition(new Vector3(1.194704f, 0.3076136f, 127.4587f), new byte[2] { 0, 3 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_0].Add(1, new RobotPosition(new Vector3(-14.75207f, -0.433012f, 5.333708f), new byte[3] { 0, 1, 3 }));
		mRobotPositionDictionary.Add(ESceneID.INSTANCE_1, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(0, new RobotPosition(new Vector3(177.6248f, -26.16714f, -271.7027f), new byte[1] { 7 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(1, new RobotPosition(new Vector3(-38.56031f, -35.51873f, -119.0798f), new byte[3] { 2, 9, 20 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(2, new RobotPosition(new Vector3(162.5795f, -43.93277f, 81.63622f), new byte[1] { 5 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(3, new RobotPosition(new Vector3(128.6089f, -26.16714f, -205.8706f), new byte[3] { 7, 11, 14 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(4, new RobotPosition(new Vector3(47.72713f, -44.0487f, 8.079023f), new byte[2] { 6, 10 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_1].Add(5, new RobotPosition(new Vector3(46.63325f, -70.15779f, 254.7972f), new byte[3] { 15, 18, 19 }));
		mRobotPositionDictionary.Add(ESceneID.INSTANCE_2, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(0, new RobotPosition(new Vector3(151.6267f, 0.18000627f, 222.977f), new byte[2] { 12, 17 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(1, new RobotPosition(new Vector3(-39.44843f, 7.817538f, 152.4435f), new byte[1] { 7 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(2, new RobotPosition(new Vector3(-33.1334f, -15.17548f, 22.78571f), new byte[2] { 3, 15 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(3, new RobotPosition(new Vector3(141.0191f, 0.17999884f, 47.2697f), new byte[3] { 2, 10, 18 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(4, new RobotPosition(new Vector3(-7.904904f, 7.118984f, 236.8743f), new byte[2] { 7, 13 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_2].Add(5, new RobotPosition(new Vector3(61.0289f, -15.87401f, -73.71793f), new byte[2] { 4, 9 }));
		mRobotPositionDictionary.Add(ESceneID.INSTANCE_3, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(0, new RobotPosition(new Vector3(1.656805f, 0.1800004f, -4.248147f), new byte[1] { 13 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(1, new RobotPosition(new Vector3(78.80365f, -3.333349f, 395.1805f), new byte[0]));
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(2, new RobotPosition(new Vector3(-86.97524f, 25.88647f, 564.6921f), new byte[1] { 17 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(3, new RobotPosition(new Vector3(67.65151f, 0.17999455f, 114.711f), new byte[1] { 10 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(4, new RobotPosition(new Vector3(103.7566f, -3.8363f, 366.4967f), new byte[2] { 2, 14 }));
		mRobotPositionDictionary[ESceneID.INSTANCE_3].Add(5, new RobotPosition(new Vector3(-78.46854f, -4.278491f, 409.0953f), new byte[2] { 3, 16 }));
		mRobotPositionDictionary.Add(ESceneID.VS_1, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.VS_1].Add(0, new RobotPosition(new Vector3(131.28f, 2.77f, 212.6f), new byte[0]));
		mRobotPositionDictionary[ESceneID.VS_1].Add(1, new RobotPosition(new Vector3(-156.22f, 2.72f, 218f), new byte[0]));
		mRobotPositionDictionary.Add(ESceneID.VS_2, new Dictionary<int, RobotPosition>());
		mRobotPositionDictionary[ESceneID.VS_2].Add(0, new RobotPosition(new Vector3(131.28f, 2.77f, 212.6f), new byte[0]));
		mRobotPositionDictionary[ESceneID.VS_2].Add(1, new RobotPosition(new Vector3(-156.22f, 2.72f, 218f), new byte[0]));
		mSpawnNameDictionary = new Dictionary<ESceneID, Dictionary<string, byte>>();
		mSpawnNameDictionary.Add(ESceneID.INSTANCE_0, new Dictionary<string, byte>());
		mSpawnNameDictionary[ESceneID.INSTANCE_0].Add("SP0-3", 0);
		mSpawnNameDictionary[ESceneID.INSTANCE_0].Add("SP0-2", 1);
		mSpawnNameDictionary[ESceneID.INSTANCE_0].Add("SP0-1", 2);
		mSpawnNameDictionary[ESceneID.INSTANCE_0].Add("SP0-4", 3);
		mSpawnNameDictionary.Add(ESceneID.INSTANCE_1, new Dictionary<string, byte>());
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-18", 0);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-20", 1);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-7", 2);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-16", 3);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-BOSS", 4);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-17", 5);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-9", 6);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-2", 7);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-19", 8);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-6", 9);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-8", 10);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-3", 11);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-11", 12);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-12", 13);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-4", 14);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-21", 15);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-24", 16);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-13", 17);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-23", 18);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-22", 19);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-5", 20);
		mSpawnNameDictionary[ESceneID.INSTANCE_1].Add("SP1-10", 21);
		mSpawnNameDictionary.Add(ESceneID.INSTANCE_2, new Dictionary<string, byte>());
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-17", 0);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-19", 1);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-2", 2);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-4", 3);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-18", 4);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-9", 5);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-11", 6);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-13", 7);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-15", 8);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-10", 9);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-7", 10);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-16", 11);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-1", 12);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-12", 13);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-5", 14);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-3", 15);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-20", 16);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-0", 17);
		mSpawnNameDictionary[ESceneID.INSTANCE_2].Add("SP2-6", 18);
		mSpawnNameDictionary.Add(ESceneID.INSTANCE_3, new Dictionary<string, byte>());
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-10", 0);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-12", 1);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-11", 2);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-14", 3);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-BOSS", 4);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-7", 5);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-8", 6);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-6", 7);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-15", 8);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-9", 9);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-5", 10);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-3", 11);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-18", 12);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-1", 13);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-13", 14);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-16", 15);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-2", 16);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-17", 17);
		mSpawnNameDictionary[ESceneID.INSTANCE_3].Add("SP3-4", 18);
		mCurrentPointIDList = new List<byte>();
		ResetAll();
	}

	public void ResetAll()
	{
		ResetLastChangeSceneTime();
		ResetLastSendInvitationTime();
		ResetLastChangePositionTime();
	}

	public void SetScene(ESceneID sceneID)
	{
		foreach (byte mCurrentPointID in mCurrentPointIDList)
		{
			PlayerLeaveSpawnPointRequest request = new PlayerLeaveSpawnPointRequest(mCurrentPointID);
			mOwner.GetNetworkManager().SendRequestAsRobot(request, mOwner);
		}
		switch (sceneID)
		{
		case ESceneID.CITY_0:
		case ESceneID.CITY_2:
		case ESceneID.CITY_1:
			switch (sceneID)
			{
			case ESceneID.CITY_0:
				mCityID = 0;
				break;
			case ESceneID.CITY_1:
				mCityID = 1;
				break;
			case ESceneID.CITY_2:
				mCityID = 2;
				break;
			}
			mMapType = MapType.City;
			break;
		case ESceneID.INSTANCE_0:
		case ESceneID.INSTANCE_2:
		case ESceneID.INSTANCE_3:
		case ESceneID.INSTANCE_1:
			switch (sceneID)
			{
			case ESceneID.INSTANCE_0:
				mCityID = 0;
				break;
			case ESceneID.INSTANCE_1:
				mCityID = 1;
				break;
			case ESceneID.INSTANCE_2:
				mCityID = 2;
				break;
			case ESceneID.INSTANCE_3:
				mCityID = 2;
				break;
			}
			mMapType = MapType.Instance;
			break;
		case ESceneID.ARENA_1:
		case ESceneID.ARENA_2:
			switch (sceneID)
			{
			case ESceneID.ARENA_1:
				mCityID = 1;
				break;
			case ESceneID.ARENA_2:
				mCityID = 2;
				break;
			}
			mMapType = MapType.Arena;
			break;
		case ESceneID.VS_1:
		case ESceneID.VS_2:
			mCityID = 0;
			mMapType = MapType.VS;
			mIn1V1Queue = false;
			mCanSend1V1 = false;
			break;
		}
		mSceneID = (byte)sceneID;
		Debug.Log(string.Concat("sceneID ", sceneID, " mPortalID ", mPortalID));
		if (!mRobotPositionDictionary[sceneID].ContainsKey(mPortalID))
		{
			mPortalID = 0;
		}
		mObject.transform.position = mRobotPositionDictionary[sceneID][mPortalID].mPosition;
		if (mMapType == MapType.Instance)
		{
			InitSpawnPoint();
			SpawnEnemy(mRobotPositionDictionary[sceneID][mPortalID].mPointIDList);
		}
		ResetAll();
	}

	public void SpawnEnemy(List<byte> pointList)
	{
		List<byte> list = new List<byte>();
		List<byte> list2 = new List<byte>();
		foreach (byte mCurrentPointID in mCurrentPointIDList)
		{
			if (!pointList.Contains(mCurrentPointID))
			{
				list.Add(mCurrentPointID);
			}
		}
		foreach (byte point in pointList)
		{
			if (!mCurrentPointIDList.Contains(point))
			{
				list2.Add(point);
			}
		}
		foreach (byte item in list)
		{
			PlayerLeaveSpawnPointRequest request = new PlayerLeaveSpawnPointRequest(item);
			mOwner.GetNetworkManager().SendRequestAsRobot(request, mOwner);
			if (mCurrentPointIDList.Contains(item))
			{
				mCurrentPointIDList.Remove(item);
			}
		}
		foreach (byte item2 in list2)
		{
			RequireEnemyInPointRequest request2 = new RequireEnemyInPointRequest(item2);
			mOwner.GetNetworkManager().SendRequestAsRobot(request2, mOwner);
			if (!mCurrentPointIDList.Contains(item2))
			{
				mCurrentPointIDList.Add(item2);
			}
		}
	}

	public void ResetLastChangeSceneTime()
	{
		mChangeSceneInterval = UnityEngine.Random.Range(10f, 20f);
		mLastChangeSceneTime = Time.time;
	}

	public void ResetLastSendInvitationTime()
	{
		mLastSendInvitationTime = Time.time;
	}

	public void ResetLastChangePositionTime()
	{
		mChangePositionInterval = UnityEngine.Random.Range(60f, 90f);
		mLastChangePositionTime = Time.time;
	}

	public void SetPortalID(int portalID)
	{
		mPortalID = portalID;
	}

	public void ChangeScene(byte targetSceneID)
	{
		SetScene((ESceneID)targetSceneID);
		ChangeGameSceneRequest request = new ChangeGameSceneRequest(mCityID, mSceneID);
		mOwner.GetNetworkManager().SendRequestAsRobot(request, mOwner);
	}

	private void InitSpawnPoint()
	{
		mSpawnScriptDictionary = new Dictionary<byte, EnemySpawnPointScript>();
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[39];
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<EnemyType> list = new List<EnemyType>();
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			string data = unitDataTable.GetData(i, 0, string.Empty, false);
			dictionary.Add(data, i);
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (!dictionary.ContainsKey(gameObject.name) || !mSpawnNameDictionary[(ESceneID)mSceneID].ContainsKey(gameObject.name))
			{
				continue;
			}
			int iRow = dictionary[gameObject.name];
			EnemySpawnPointScript component = gameObject.GetComponent<EnemySpawnPointScript>();
			component.PointID = mSpawnNameDictionary[(ESceneID)mSceneID][gameObject.name];
			component.ResetSpawnGroupListForRobot();
			int data2 = unitDataTable.GetData(iRow, 1, 0, false);
			int data3 = unitDataTable.GetData(iRow, 2, 0, false);
			List<SpawnGroup> list2 = new List<SpawnGroup>();
			for (int k = 0; k < 5; k++)
			{
				string data4 = unitDataTable.GetData(iRow, k * 4 + 3, string.Empty, false);
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
						spawnGroup.mCount = unitDataTable.GetData(iRow, k * 4 + 4, 0, false);
						spawnGroup.mLowerLevel = unitDataTable.GetData(iRow, k * 4 + 5, 0, false);
						spawnGroup.mUpperLevel = unitDataTable.GetData(iRow, k * 4 + 6, 0, false);
						if (k < data2)
						{
							component.AddEnemyToSpawnGroupList(spawnGroup);
						}
						else
						{
							list2.Add(spawnGroup);
						}
						if (!list.Contains(spawnGroup.mType))
						{
							list.Add(spawnGroup.mType);
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
			data3 = Mathf.Min(data3, list2.Count);
			for (int l = 0; l < data3; l++)
			{
				int index = UnityEngine.Random.Range(0, list2.Count);
				component.AddEnemyToSpawnGroupList(list2[index]);
				list2.RemoveAt(index);
			}
			mSpawnScriptDictionary.Add(component.PointID, component);
		}
	}

	public void UploadEnemy(byte pointID)
	{
		if (mSpawnScriptDictionary.ContainsKey(pointID))
		{
			EnemySpawnPointScript enemySpawnPointScript = mSpawnScriptDictionary[pointID];
			enemySpawnPointScript.SpawnEnemyForRobot(mOwner);
		}
	}

	public void OnStopMatch()
	{
		mIn1V1Queue = false;
		mCanSend1V1 = false;
		mRound1V1 = 0;
	}

	public void OnVsFail(byte interval)
	{
		mIn1V1Queue = true;
		mLastSend1V1Time = Time.time;
		mInterval1V1 = (int)interval;
		mRound1V1++;
		mCanSend1V1 = true;
	}

	public void Update()
	{
		if (mMapType == MapType.City && mIn1V1Queue)
		{
			if (mCanSend1V1 && Time.time - mLastSend1V1Time > mInterval1V1)
			{
				mCanSend1V1 = false;
				mLastSend1V1Time = Time.time;
				VSQuickMatchRequest request = new VSQuickMatchRequest(UIVS.Mode.CaptureHold_1v1, mRound1V1);
				mOwner.GetNetworkManager().SendRequestAsRobot(request, mOwner);
			}
		}
		else if (Time.time - mLastChangeSceneTime > mChangeSceneInterval && Time.time - mLastSendInvitationTime > 30f)
		{
			ResetLastSendInvitationTime();
			if (mMapType == MapType.City)
			{
				if (UnityEngine.Random.Range(0, 100) < 10)
				{
					ESceneID eSceneID = ESceneID.INSTANCE_0;
					int num = UnityEngine.Random.Range(0, 4);
					mPortalID = UnityEngine.Random.Range(0, 3);
					switch (num)
					{
					case 0:
						eSceneID = ESceneID.INSTANCE_0;
						mPortalID = 0;
						break;
					case 1:
						eSceneID = ESceneID.INSTANCE_1;
						break;
					case 2:
						eSceneID = ESceneID.INSTANCE_2;
						break;
					case 3:
						eSceneID = ESceneID.INSTANCE_3;
						break;
					}
					InvitationRequest request2 = new InvitationRequest(mOwner.GetLobby().GetChannelID(), InvitationRequest.Type.Story, SubMode.Story, (byte)eSceneID, (short)mPortalID);
					mOwner.GetNetworkManager().SendRequestAsRobot(request2, mOwner);
				}
				else
				{
					InvitationRequest request3 = new InvitationRequest(mOwner.GetLobby().GetChannelID(), InvitationRequest.Type.VS, SubMode.Story, 0, 0);
					mOwner.GetNetworkManager().SendRequestAsRobot(request3, mOwner);
				}
			}
			else if (mMapType == MapType.Instance)
			{
				ESceneID eSceneID2 = ESceneID.CITY_0;
				mPortalID = 0;
				if (mCityID == 0)
				{
					eSceneID2 = ESceneID.CITY_0;
				}
				else if (mCityID == 1)
				{
					eSceneID2 = ESceneID.CITY_1;
				}
				else if (mCityID == 2)
				{
					eSceneID2 = ESceneID.CITY_2;
				}
				InvitationRequest request4 = new InvitationRequest(mOwner.GetLobby().GetChannelID(), InvitationRequest.Type.Story, SubMode.Story, (byte)eSceneID2, (short)mPortalID);
				mOwner.GetNetworkManager().SendRequestAsRobot(request4, mOwner);
			}
			else if (mMapType != MapType.Arena && mMapType != MapType.VS)
			{
			}
		}
		if (mMapType == MapType.Instance && Time.time - mLastChangePositionTime > mChangePositionInterval)
		{
			ResetLastChangePositionTime();
			int key = UnityEngine.Random.Range(0, mRobotPositionDictionary[(ESceneID)mSceneID].Count);
			mObject.transform.position = mRobotPositionDictionary[(ESceneID)mSceneID][key].mPosition;
			SpawnEnemy(mRobotPositionDictionary[(ESceneID)mSceneID][key].mPointIDList);
		}
	}
}
