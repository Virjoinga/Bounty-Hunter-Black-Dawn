using System.Collections.Generic;
using UnityEngine;

public class BaseEnemySpawnScript : MonoBehaviour
{
	public List<GameObject> PatrolLines = new List<GameObject>();

	protected ESpawnerStatus mStatus;

	protected List<Enemy> mSpawnedEnemyList = new List<Enemy>();

	protected int mRefCount;

	protected int mPatrolLineIndex;

	public byte PointID { get; set; }

	public bool HasAwaked { get; set; }

	public bool IsAwaking { get; set; }

	public virtual void Activate()
	{
	}

	public virtual void Deactivate()
	{
	}

	public void InformTarget(GameUnit target)
	{
		IsAwaking = false;
		HasAwaked = true;
		foreach (Enemy mSpawnedEnemy in mSpawnedEnemyList)
		{
			if (mSpawnedEnemy != null && mSpawnedEnemy.IsActive())
			{
				mSpawnedEnemy.OnInformTarget(target);
			}
		}
	}

	public void IncreaseRefCount()
	{
		if (mRefCount == 0)
		{
			Activate();
		}
		mRefCount++;
	}

	public void DecreaseRefCount()
	{
		mRefCount--;
		if (mRefCount <= 0)
		{
			mRefCount = 0;
			Deactivate();
		}
	}
}
