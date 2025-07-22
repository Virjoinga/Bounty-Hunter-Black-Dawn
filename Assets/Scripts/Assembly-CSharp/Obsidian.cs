using System.Collections.Generic;
using UnityEngine;

public class Obsidian : Enemy
{
	public static EnemyState OBSIDIAN_DIVE_END_STATE = new ObsidianDiveEndState();

	public static EnemyState OBSIDIAN_DIVE_START_STATE = new ObsidianDiveStartState();

	public static EnemyState OBSIDIAN_DIVE_STATE = new ObsidianDiveState();

	public static EnemyState OBSIDIAN_FLY_AROUND_STATE = new ObsidianFlyAroundState();

	public static EnemyState OBSIDIAN_SPAWN_STATE = new ObsidianSpawnState();

	protected float mPatrolRadius;

	protected bool mIsClockWise;

	protected float mFlyAroundRadius;

	protected Timer mHitCheckTimer = new Timer();

	protected Timer mDiveHitCheckTimer = new Timer();

	protected float mSpawnTime;

	protected Vector3 mDiveStartPosition;

	protected Vector3 mSpawnPosition;

	protected Timer mAttackSoundTimer = new Timer();

	protected Timer mWingsSoundTimer = new Timer();

	public override void Init()
	{
		base.Init();
		mShadowPath = string.Empty;
		mNavAngularSpeed = 180f;
		mTurnSpeed = 0.015f;
		mNavWalkableMask = 1 << NavMeshLayer.FLY_AREA;
		mSpawnTime = Random.Range(0f, 1f);
		mCanGotHit = false;
		mPatrolRadius = Random.Range(10f, 20f);
		mFlyAroundRadius = (float)mRushAttackRadius + Random.Range(-2f, 2f);
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			mIsClockWise = true;
		}
		else
		{
			mIsClockWise = false;
		}
		mHitCheckTimer.SetTimer(1f, false);
		mDiveHitCheckTimer.SetTimer(0.3f, false);
		mAttackSoundTimer.SetTimer(5f, false);
		mWingsSoundTimer.SetTimer(0.5f, true);
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.ObsidianMesh;
	}

	public override void Activate()
	{
		base.Activate();
		if (SpawnType != 0)
		{
			StartObsidianFlyAround(entityTransform.position);
		}
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.ObsidianBody);
		mHeadTransform = entityTransform.Find(BoneName.ObsidianHead);
		mHitObjectArray = new GameObject[6];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
		mHitObjectArray[2] = entityTransform.Find(BoneName.ObsidianLeftFly3).gameObject;
		mHitObjectArray[3] = entityTransform.Find(BoneName.ObsidianLeftFly4).gameObject;
		mHitObjectArray[4] = entityTransform.Find(BoneName.ObsidianRightFly3).gameObject;
		mHitObjectArray[5] = entityTransform.Find(BoneName.ObsidianRightFly4).gameObject;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (mIsActive)
		{
			PlayWingsSound();
		}
	}

	public override void UpdatePosition(Vector3 position, EnemyStateConst nextState)
	{
		if (!base.IsMasterPlayer)
		{
			if (mNavMeshAgent != null && !mNavMeshAgent.enabled && (nextState == EnemyStateConst.OBSIDIAN_PATROL || nextState == EnemyStateConst.OBSIDIAN_FLY_AROUND || nextState == EnemyStateConst.CATCHING))
			{
				entityTransform.position = position;
				return;
			}
			mDeltaPosition = position - entityTransform.position;
			mCurrentDeltaCount = 0;
		}
	}

	protected override void StartIdleWhenAddPlayer()
	{
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == OBSIDIAN_DIVE_END_STATE)
		{
			EndObsidianDiveEnd();
		}
		else if (mState == OBSIDIAN_DIVE_START_STATE)
		{
			EndObsidianDiveStart();
		}
		else if (mState == OBSIDIAN_DIVE_STATE)
		{
			EndObsidianDive();
		}
		else if (mState == OBSIDIAN_FLY_AROUND_STATE)
		{
			EndObsidianFlyAround();
		}
		else if (mState == OBSIDIAN_SPAWN_STATE)
		{
			EndObsidianSpawn();
		}
	}

	public override void OnInformTarget(GameUnit target)
	{
		if (mState == Enemy.PATROL_STATE)
		{
			EndCurrentState();
			ChangeTarget(target);
			StartCatching();
		}
	}

	public override void UpdateNavMeshInCatching(bool force)
	{
		if (!force && !(Time.time - mLastUpdateNavMeshTime > 9f / mRunSpeed))
		{
			return;
		}
		mLastUpdateNavMeshTime = Time.time;
		Vector3 origin = mTargetPosition;
		mRay = new Ray(origin, Vector3.up);
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			origin = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
			if (null != mNavMeshAgent)
			{
				mNavMeshAgent.enabled = true;
				mNavMeshAgent.Resume();
				mNavMeshAgent.SetDestination(origin);
				mNavMeshAgent.speed = mRunSpeed;
				SetCanTurn(false);
			}
		}
	}

	public override void CreateNavMeshAgent()
	{
		if (mNavMeshAgent == null)
		{
			entityObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent = entityObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent.radius = cc.radius;
			mNavMeshAgent.height = 1f;
			mNavMeshAgent.baseOffset = mNavBaseOffset;
			mNavMeshAgent.speed = mRunSpeed;
			mNavMeshAgent.angularSpeed = mNavAngularSpeed;
			mNavMeshAgent.acceleration = 100000f;
			mNavMeshAgent.walkableMask = mNavWalkableMask;
			mNavMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
			mNavMeshAgent.stoppingDistance = mStoppingDistance;
			mNavMeshAgent.enabled = false;
		}
	}

	public override void StopNavMesh()
	{
		if (null != mNavMeshAgent && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.Stop(true);
			mNavMeshAgent.enabled = false;
		}
	}

	public override void SetNavMeshForPatrol()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			mNavMeshAgent.speed = mPatrolSpeed;
			SetCanTurn(false);
		}
	}

	public override bool TryPathForNavMesh(Vector3 destination)
	{
		if (null != mNavMeshAgent)
		{
			bool enabled = mNavMeshAgent.enabled;
			mNavMeshAgent.enabled = true;
			UnityEngine.AI.NavMeshHit hit = default(UnityEngine.AI.NavMeshHit);
			if (!mNavMeshAgent.Raycast(destination, out hit))
			{
				mNavMeshAgent.enabled = enabled;
				return true;
			}
			mNavMeshAgent.enabled = enabled;
		}
		return false;
	}

	private void PlayWingsSound()
	{
		if (IsPlayingAnimation(AnimationString.ENEMY_RUN) && mWingsSoundTimer.Ready())
		{
			float num = animation[AnimationString.ENEMY_RUN].time / animation[AnimationString.ENEMY_RUN].clip.length;
			num -= Mathf.Floor(num);
			if (num < 0.1f)
			{
				mWingsSoundTimer.Do();
				PlaySound("RPG_Audio/Enemy/Obsidian/obsidian_wings");
			}
		}
	}

	public override void DoAwake()
	{
		EndAwake();
		StartCatching();
		mSpawnPointScript.InformTarget(mTarget);
	}

	protected override void MakeDecisionInCatching()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null && SpawnType != ESpawnType.ARENA && SpawnType != ESpawnType.BOSS_RUSH)
		{
			EndCatching();
			mPatrolTarget = CalculateNextDestination(mSpawnPointScript.gameObject.transform.position, mPatrolRadius);
			StartObsidianPatrol(mPatrolTarget);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_PATROL, entityTransform.position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			if (NeedGoBack())
			{
				return;
			}
			float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
			if (horizontalSqrDistanceFromTarget < (float)(mRushAttackRadius * mRushAttackRadius))
			{
				EndCatching();
				mPatrolTarget = CalculateNextDestination(mTargetPosition, mFlyAroundRadius);
				StartObsidianFlyAround(mPatrolTarget);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_FLY_AROUND, GetTransform().position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
		}
	}

	protected override void PlayDeadSound()
	{
		StopSound("RPG_Audio/Enemy/Obsidian/obsidian_attack");
		PlaySound("RPG_Audio/Enemy/Obsidian/obsidian_die");
	}

	protected override bool NeedGoBack()
	{
		if (SpawnType == ESpawnType.ARENA || SpawnType == ESpawnType.BOSS_RUSH)
		{
			return false;
		}
		if (mCheckGoBackTimer.Ready())
		{
			mCheckGoBackTimer.Do();
			Vector3 vector = mSpawnPointScript.gameObject.transform.position - entityTransform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > (float)(mActivityRadius * mActivityRadius))
			{
				mPatrolTarget = CalculateNextDestination(mSpawnPointScript.gameObject.transform.position, mPatrolRadius);
				EndCurrentState();
				StartObsidianPatrol(mPatrolTarget);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				return true;
			}
		}
		return false;
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Obsidian/obsidian_attacked");
	}

	public override void StartPatrolIdleWithoutResetTime()
	{
		base.StartPatrolIdleWithoutResetTime();
		StopAnimation(AnimationString.ENEMY_RUN);
	}

	public override void DoPatrolIdle()
	{
		if (!base.IsMasterPlayer || !(GetIdleTimeDuration() > mSpawnTime))
		{
			return;
		}
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop);
		Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		vector.Normalize();
		float num = Random.Range(10f, 20f);
		Vector3 origin = mSpawnPointScript.gameObject.transform.position + vector * num;
		mRay = new Ray(origin, Vector3.up);
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		if (!Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			return;
		}
		mSpawnPosition = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
		Vector3 direction = mSpawnPosition - entityTransform.position;
		float distance = direction.magnitude + 1f;
		direction.Normalize();
		mRay = new Ray(entityTransform.position, direction);
		layerMask = (1 << PhysicsLayer.FLY_AREA) | (1 << PhysicsLayer.WALL);
		if (Physics.Raycast(mRay, out mRaycastHit, distance, layerMask) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.FLY_AREA)
		{
			EndPatrolIdle();
			StartObsidianSpawn(mSpawnPosition);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_SPAWN, GetTransform().position, mSpawnPosition);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartObsidianPatrol(Vector3 patrolTarget)
	{
		SetState(Enemy.PATROL_STATE);
		SetPatrolTimeNow();
		SetPatrolTarget(patrolTarget);
		SetNavMeshForPatrol();
	}

	public override void DoPatrol()
	{
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop, 1f);
		if (!DetectPlayer() && base.IsMasterPlayer && (entityTransform.position - mPatrolTarget).sqrMagnitude < 1f)
		{
			mPatrolTarget = CalculateNextDestination(mSpawnPointScript.gameObject.transform.position, mPatrolRadius);
			StartObsidianPatrol(mPatrolTarget);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_PATROL, GetTransform().position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartObsidianSpawn(Vector3 target)
	{
		SetState(OBSIDIAN_SPAWN_STATE);
		mSpawnPosition = target;
		SetUpdatePosTimeNow();
		LookAtPoint(mSpawnPosition);
	}

	public void DoObsidianSpawn()
	{
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop);
		float num = entityTransform.position.y - mSpawnPosition.y;
		if (num < -7f)
		{
			LookAtPoint(mSpawnPosition);
		}
		else
		{
			LookAtPointHorizontal(mSpawnPosition);
		}
		if (entityTransform.position.y > mSpawnPosition.y)
		{
			if (base.IsMasterPlayer)
			{
				EndObsidianSpawn();
				Vector3 vector = new Vector3(entityTransform.position.x, mSpawnPointScript.gameObject.transform.position.y, entityTransform.position.z);
				Vector3 position = mSpawnPointScript.gameObject.transform.position;
				mPatrolTarget = CalculateNextDestination(position, mPatrolRadius);
				StartObsidianPatrol(mPatrolTarget);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_PATROL, GetTransform().position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else
		{
			cc.Move(mRunSpeed * entityTransform.forward * Time.deltaTime);
			if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_SPAWN, GetTransform().position, mSpawnPosition);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	protected Vector3 CalculateNextDestination(Vector3 center, float radius)
	{
		Vector3 vector = new Vector3(entityTransform.position.x, mSpawnPointScript.gameObject.transform.position.y, entityTransform.position.z);
		Vector3 vector2 = new Vector3(center.x, mSpawnPointScript.gameObject.transform.position.y, center.z);
		Vector3 rhs = vector - vector2;
		Vector3 vector3 = Vector3.Cross(Vector3.up, rhs);
		vector3.Normalize();
		if (!mIsClockWise)
		{
			vector3 = -vector3;
		}
		Vector3 vector4 = vector + vector3 * radius * 0.36f;
		Vector3 vector5 = vector4 - vector2;
		vector5.Normalize();
		vector4 = vector2 + vector5 * radius;
		mRay = new Ray(vector4, Vector3.up);
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			Vector3 vector6 = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
			if (TryPathForNavMesh(vector6))
			{
				return vector6;
			}
		}
		if (mState == OBSIDIAN_DIVE_END_STATE)
		{
			return mSpawnPosition;
		}
		mIsClockWise = !mIsClockWise;
		return entityTransform.position;
	}

	protected void EndObsidianSpawn()
	{
	}

	public void StartObsidianDiveStart()
	{
		SetState(OBSIDIAN_DIVE_START_STATE);
		LookAtPoint(mTargetPosition + Vector3.up);
		mDiveStartPosition = entityTransform.position;
		PlaySound("RPG_Audio/Enemy/Obsidian/obsidian_wings");
	}

	public void DoObsidianDiveStart()
	{
		PlayAnimation(AnimationString.OBSIDIAN_DIVE_START, WrapMode.ClampForever, 1.5f);
		LookAtPoint(mTargetPosition + Vector3.up);
		entityTransform.position += mRunSpeed * entityTransform.forward * Time.deltaTime;
		if (AnimationPlayed(AnimationString.OBSIDIAN_DIVE_START, 1f))
		{
			EndObsidianDiveStart();
			StartObsidianDive();
		}
	}

	protected void EndObsidianDiveStart()
	{
	}

	protected void StartObsidianDive()
	{
		SetState(OBSIDIAN_DIVE_STATE);
		mTurnSpeed = 0.1f;
		LookAtPoint(mTargetPosition + Vector3.up);
	}

	public void DoObsidianDive()
	{
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.Loop);
		if (mAttackSoundTimer.Ready() && entityTransform.position.y < mTargetPosition.y + 15f)
		{
			mAttackSoundTimer.Do();
			PlaySound("RPG_Audio/Enemy/Obsidian/obsidian_attack");
		}
		LookAtPoint(mTargetPosition + Vector3.up);
		entityTransform.position += mRushAttackSpeed1 * entityTransform.forward * Time.deltaTime;
		bool flag = false;
		if (base.IsMasterPlayer && mDiveHitCheckTimer.Ready())
		{
			mDiveHitCheckTimer.Do();
			flag = !canHitTargetPlayer();
		}
		if (!flag && GetHorizontalSqrDistanceFromLocalPlayer() < 2f && entityTransform.position.y < mTargetPosition.y + 1.7f)
		{
			mLocalPlayer.OnHit(mRushAttackDamage1, this);
			flag = true;
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (!flag && GetHorizontalSqrDistanceFromGameUnit(value) < 2f && entityTransform.position.y < mTargetPosition.y + 1.7f)
			{
				value.OnHit(mRushAttackDamage1);
				flag = true;
			}
		}
		if (!flag && entityTransform.position.y < mTargetPosition.y + 1.1f)
		{
			flag = true;
		}
		if (flag)
		{
			EndObsidianDive();
			mPatrolTarget = mDiveStartPosition;
			Vector3 vector = new Vector3(entityTransform.forward.x, 0f, entityTransform.forward.z);
			entityTransform.LookAt(entityTransform.position + vector);
			StartObsidianDiveEnd(mPatrolTarget);
		}
	}

	protected void EndObsidianDive()
	{
	}

	public void StartObsidianDiveEnd(Vector3 target)
	{
		SetState(OBSIDIAN_DIVE_END_STATE);
		mPatrolTarget = target;
		mTurnSpeed = 0.015f;
		SetUpdatePosTimeNow();
		LookAtPoint(mPatrolTarget);
	}

	public void DoObsidianDiveEnd()
	{
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop, 1.3f);
		float num = entityTransform.position.y - mPatrolTarget.y;
		if (num < -7f)
		{
			LookAtPoint(mPatrolTarget);
		}
		else
		{
			LookAtPointHorizontal(mPatrolTarget);
			entityTransform.position += Vector3.up * Time.deltaTime;
		}
		if (num > 0f)
		{
			if (base.IsMasterPlayer)
			{
				EndObsidianDiveEnd();
				mPatrolTarget = CalculateNextDestination(mTargetPosition, mFlyAroundRadius);
				StartObsidianFlyAround(mPatrolTarget);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_FLY_AROUND, GetTransform().position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else
		{
			entityTransform.position += 0.6f * mRunSpeed * entityTransform.forward * Time.deltaTime;
			if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_DIVE_END, GetTransform().position, mPatrolTarget, (short)(base.SpeedRate * 100f));
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	protected void EndObsidianDiveEnd()
	{
		SetIdleTimeNow();
	}

	protected void SetNavMeshForFlyAround()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			mNavMeshAgent.speed = mWalkSpeed;
			SetCanTurn(false);
		}
	}

	public void StartObsidianFlyAround(Vector3 target)
	{
		SetState(OBSIDIAN_FLY_AROUND_STATE);
		SetPatrolTarget(target);
		SetNavMeshForFlyAround();
	}

	public void DoObsidianFlyAround()
	{
		Debug.DrawLine(entityTransform.position, mPatrolTarget, Color.blue);
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop, 1.2f);
		if (!base.IsMasterPlayer)
		{
			return;
		}
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null && SpawnType != ESpawnType.ARENA && SpawnType != ESpawnType.BOSS_RUSH)
		{
			EndObsidianFlyAround();
			mPatrolTarget = CalculateNextDestination(mSpawnPointScript.gameObject.transform.position, mPatrolRadius);
			StartObsidianPatrol(mPatrolTarget);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			if (NeedGoBack())
			{
				return;
			}
			ChooseTargetPlayer(false);
			if (GetIdleTimeDuration() > mIdleTime && mHitCheckTimer.Ready())
			{
				mHitCheckTimer.Do();
				if (canHitTargetPlayer())
				{
					EndObsidianFlyAround();
					StartObsidianDiveStart();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_DIVE_START, GetTransform().position, 100);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
					return;
				}
			}
			if ((entityTransform.position - mPatrolTarget).sqrMagnitude < 1f)
			{
				mPatrolTarget = CalculateNextDestination(mTargetPosition, mFlyAroundRadius);
				StartObsidianFlyAround(mPatrolTarget);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.OBSIDIAN_FLY_AROUND, GetTransform().position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
			}
		}
	}

	protected void EndObsidianFlyAround()
	{
		StopNavMesh();
	}
}
