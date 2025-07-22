using System.Collections.Generic;
using UnityEngine;

public abstract class SummonedItem : ControllableUnit
{
	public const float KNOCKED_DISTANCE = 5f;

	protected Player mOwnerPlayer;

	protected bool mIsSameTeam = true;

	protected UnityEngine.AI.NavMeshAgent mNavMeshAgent;

	protected float knockedSpeed;

	protected float knockStartTime;

	public static SummonedItemKnockedState KNOCKED_STATE = new SummonedItemKnockedState();

	protected Dictionary<Enemy, Timer> mCheckHitTimers = new Dictionary<Enemy, Timer>();

	public ESummonedType SummonedType { get; set; }

	public SummonedItem()
	{
		base.ControllableType = EControllableType.SUMMONED;
	}

	public void SetOwnerPlayer(Player player)
	{
		mOwnerPlayer = player;
		if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.IsSameTeam(player))
		{
			mIsSameTeam = true;
		}
		else
		{
			mIsSameTeam = false;
		}
	}

	public Player GetOwnerPlayer()
	{
		return mOwnerPlayer;
	}

	public bool IsSameTeam()
	{
		return mIsSameTeam;
	}

	public override void Init()
	{
		base.Init();
		gameScene = GameApp.GetInstance().GetGameScene();
		base.Name = "S_" + base.ID;
		startTime = Time.time;
		GameObject original = Resources.Load(GetResourcePath()) as GameObject;
		GameObject gameObject = Object.Instantiate(original, base.Position, base.Rotation) as GameObject;
		gameObject.name = base.Name;
		SetObject(gameObject);
		animation = entityObject.GetComponent<Animation>();
		mHitCheckCollider = entityObject.transform.Find("collision").gameObject.GetComponent<Collider>();
		gameScene.AddSummoned(base.Name, this);
		mShieldRecoveryStartTimer.SetTimer(5f, true);
		StartInit();
	}

	public void AddEnemyCheckHitTimer(Enemy enemy)
	{
		if (!mCheckHitTimers.ContainsKey(enemy))
		{
			Timer timer = new Timer();
			timer.SetTimer(1f, false);
			mCheckHitTimers.Add(enemy, timer);
		}
		else
		{
			Debug.Log(string.Concat("XXX: Add Duplicated enemy: ", enemy, "is added to SummonedItem."));
		}
	}

	public void AddEnemyCheckHitTimers(Dictionary<string, Enemy> enemyList)
	{
		foreach (KeyValuePair<string, Enemy> enemy in enemyList)
		{
			AddEnemyCheckHitTimer(enemy.Value);
		}
	}

	public void RemoveEnemyCheckHitTimer(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			mCheckHitTimers.Remove(enemy);
		}
		else
		{
			Debug.Log(string.Concat("XXX: Remove enemy: ", enemy, "is not registed in SummonedItem."));
		}
	}

	public void RemoveAllEnemyCheckHitTimer()
	{
		mCheckHitTimers.Clear();
	}

	public bool CheckHitTimerReady(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			return mCheckHitTimers[enemy].Ready();
		}
		Debug.Log(string.Concat("XXX: Check enemy: ", enemy, "is not registed in SummonedItem."));
		return false;
	}

	public void ResetCheckHitTimer(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			mCheckHitTimers[enemy].Do();
		}
		else
		{
			Debug.Log(string.Concat("XXX: Reset enemy: ", enemy, "is not registed in SummonedItem."));
		}
	}

	public virtual void LateLoop(float deltaTime)
	{
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == KNOCKED_STATE)
		{
			EndKnocked();
		}
	}

	public void CreateNavMeshAgent()
	{
		if (mNavMeshAgent == null)
		{
			entityObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent = entityObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent.baseOffset = 0f;
			mNavMeshAgent.height = 1.5f;
			mNavMeshAgent.speed = 10f;
			mNavMeshAgent.angularSpeed = 360f;
			mNavMeshAgent.acceleration = 100000f;
			mNavMeshAgent.walkableMask = (1 << NavMeshLayer.NORMAL_GROUND) | (1 << NavMeshLayer.JUMP);
			mNavMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
			StopNavMesh();
		}
	}

	public void StopNavMesh()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Stop(false);
		}
	}

	public void DestroyNavMeshAgent()
	{
		if (mNavMeshAgent != null)
		{
			Object.Destroy(mNavMeshAgent);
			mNavMeshAgent = null;
		}
	}

	protected void SetNavMeshForRun()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mOwnerPlayer.GetTransform().position + Vector3.up * 0.1f);
		}
	}

	public void StartKnocked()
	{
		SetState(KNOCKED_STATE);
		knockStartTime = Time.time;
		if (mNavMeshAgent != null)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(entityTransform.position - entityTransform.forward * 5f);
		}
	}

	public override void OnKnocked(float speed)
	{
		if (speed > 1f)
		{
			StartKnocked();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				ControllableItemStateRequest request = new ControllableItemStateRequest(base.ControllableType, base.ID, ControllableStateConst.KNOCKED, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public virtual void DoKnocked()
	{
		if (Time.time - knockStartTime > 0.5f)
		{
			EndKnocked();
			StartIdle();
			SendTransform();
		}
	}

	protected void EndKnocked()
	{
		StopNavMesh();
	}
}
