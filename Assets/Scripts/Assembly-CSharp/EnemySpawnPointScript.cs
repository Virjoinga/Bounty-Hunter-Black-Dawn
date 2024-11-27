using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointScript : BaseEnemySpawnScript
{
	private List<SpawnGroup> mSpawnGroupList;

	private List<Enemy> mToSpawnEnemyList;

	protected int mCounter;

	protected int mPositionCounter;

	private void Start()
	{
		mStatus = ESpawnerStatus.INACTIVE;
		mSpawnGroupList = new List<SpawnGroup>();
		mToSpawnEnemyList = new List<Enemy>();
		mSpawnedEnemyList = new List<Enemy>();
		mRefCount = 0;
		mPatrolLineIndex = 0;
	}

	private void Update()
	{
		foreach (Enemy mToSpawnEnemy in mToSpawnEnemyList)
		{
			mToSpawnEnemy.LinkEnemy(mToSpawnEnemyList);
		}
		foreach (Enemy mToSpawnEnemy2 in mToSpawnEnemyList)
		{
			if (mToSpawnEnemy2.CanActivate())
			{
				mToSpawnEnemy2.Activate();
				if (mToSpawnEnemy2.IsBoss())
				{
					mToSpawnEnemy2.GetTransform().rotation = base.gameObject.transform.rotation;
				}
				GameApp.GetInstance().GetGameScene().AddEnemy(mToSpawnEnemy2.Name, mToSpawnEnemy2);
				if (!mSpawnedEnemyList.Contains(mToSpawnEnemy2))
				{
					mSpawnedEnemyList.Add(mToSpawnEnemy2);
				}
				mToSpawnEnemyList.Remove(mToSpawnEnemy2);
				break;
			}
		}
	}

	public override void Activate()
	{
		if (mStatus != 0)
		{
			return;
		}
		mStatus = ESpawnerStatus.ACTIVE;
		mPositionCounter = 0;
		base.HasAwaked = false;
		base.IsAwaking = false;
		if (!GameApp.GetInstance().GetGameWorld().HasSpawnPoint(base.PointID))
		{
			byte b = 0;
			foreach (SpawnGroup mSpawnGroup in mSpawnGroupList)
			{
				for (int i = 0; i < mSpawnGroup.mCount; i++)
				{
					byte enemyID = b;
					b++;
					float x = base.gameObject.transform.position.x;
					float z = base.gameObject.transform.position.z;
					if (mSpawnGroup.mType >= EnemyType.SHELL)
					{
						int num = mPositionCounter % 9;
						mPositionCounter++;
						switch (num)
						{
						case 0:
							x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
							break;
						case 1:
							x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
							break;
						case 2:
							x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
							break;
						case 3:
							x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
							break;
						case 4:
							x = base.gameObject.transform.position.x;
							z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
							break;
						case 5:
							x = base.gameObject.transform.position.x;
							z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
							break;
						case 6:
							x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z;
							break;
						case 7:
							x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
							z = base.gameObject.transform.position.z;
							break;
						}
					}
					Vector3 spawnPosition = new Vector3(x, base.gameObject.transform.position.y + 0.1f, z);
					byte enemyLevel = (byte)Random.Range(mSpawnGroup.mLowerLevel, mSpawnGroup.mUpperLevel + 1);
					GameApp.GetInstance().GetGameWorld().AddEnemy(base.PointID, enemyID, mSpawnGroup.mType, false, enemyLevel, mSpawnGroup.mUniqueID, spawnPosition);
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				RequireEnemyInPointRequest request = new RequireEnemyInPointRequest(base.PointID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			DownloadEnemyInPointRequest request2 = new DownloadEnemyInPointRequest(base.PointID, true);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
		if (!GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(base.PointID);
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			item.Value.IsMasterPlayer = true;
		}
		InitEnemyFromGameWorld();
	}

	public void InitEnemyFromGameWorld()
	{
		mPatrolLineIndex = 0;
		Dictionary<byte, Enemy> enemies = GameApp.GetInstance().GetGameWorld().GetEnemies(base.PointID);
		foreach (KeyValuePair<byte, Enemy> item in enemies)
		{
			if (!item.Value.CanBeSpawn())
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				item.Value.Hp = item.Value.MaxHp;
				item.Value.Shield = item.Value.MaxShield;
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
			mToSpawnEnemyList.Add(item.Value);
		}
	}

	public override void Deactivate()
	{
		if (mStatus != ESpawnerStatus.ACTIVE)
		{
			return;
		}
		mStatus = ESpawnerStatus.INACTIVE;
		foreach (Enemy mSpawnedEnemy in mSpawnedEnemyList)
		{
			mSpawnedEnemy.Deactivate();
			GameApp.GetInstance().GetGameScene().RemoveEnemy(mSpawnedEnemy.Name);
		}
		mSpawnedEnemyList.Clear();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerLeaveSpawnPointRequest request = new PlayerLeaveSpawnPointRequest(base.PointID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void AddEnemyToSpawnGroupList(SpawnGroup group)
	{
		mSpawnGroupList.Add(group);
	}

	public List<EnemyType> GetEnemyTypeList()
	{
		List<EnemyType> list = new List<EnemyType>();
		foreach (SpawnGroup mSpawnGroup in mSpawnGroupList)
		{
			if (!list.Contains(mSpawnGroup.mType))
			{
				list.Add(mSpawnGroup.mType);
			}
		}
		return list;
	}

	public void ResetSpawnGroupListForRobot()
	{
		mSpawnGroupList = new List<SpawnGroup>();
	}

	public void SpawnEnemyForRobot(RobotUser robot)
	{
		byte b = 0;
		List<EnemyStatus> list = new List<EnemyStatus>();
		foreach (SpawnGroup mSpawnGroup in mSpawnGroupList)
		{
			for (int i = 0; i < mSpawnGroup.mCount; i++)
			{
				byte mEnemyID = b;
				b++;
				float x = base.gameObject.transform.position.x;
				float z = base.gameObject.transform.position.z;
				if (mSpawnGroup.mType >= EnemyType.SHELL)
				{
					int num = mPositionCounter % 9;
					mPositionCounter++;
					switch (num)
					{
					case 0:
						x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
						break;
					case 1:
						x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
						break;
					case 2:
						x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
						break;
					case 3:
						x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
						break;
					case 4:
						x = base.gameObject.transform.position.x;
						z = base.gameObject.transform.position.z - base.gameObject.transform.localScale.z / 2f;
						break;
					case 5:
						x = base.gameObject.transform.position.x;
						z = base.gameObject.transform.position.z + base.gameObject.transform.localScale.z / 2f;
						break;
					case 6:
						x = base.gameObject.transform.position.x - base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z;
						break;
					case 7:
						x = base.gameObject.transform.position.x + base.gameObject.transform.localScale.x / 2f;
						z = base.gameObject.transform.position.z;
						break;
					}
				}
				Vector3 mSpawnPosition = new Vector3(x, base.gameObject.transform.position.y + 0.1f, z);
				byte mEnemyLevel = (byte)Random.Range(mSpawnGroup.mLowerLevel, mSpawnGroup.mUpperLevel + 1);
				EnemyStatus enemyStatus = new EnemyStatus();
				enemyStatus.mEnemyID = mEnemyID;
				enemyStatus.mUniqueID = mSpawnGroup.mUniqueID;
				enemyStatus.mEnemyType = mSpawnGroup.mType;
				enemyStatus.mEnemyLevel = mEnemyLevel;
				enemyStatus.mIsElite = false;
				enemyStatus.mSpawnPosition = mSpawnPosition;
				list.Add(enemyStatus);
			}
		}
		UploadEnemyInPointRequest request = new UploadEnemyInPointRequest(base.PointID, list);
		robot.GetNetworkManager().SendRequestAsRobot(request, robot);
	}
}
