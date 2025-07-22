using System.Collections.Generic;
using UnityEngine;

public class FloatControler : EnemyBoss
{
	public static EnemyState FC_PATROL_STATE = new FCPatrolState();

	public static EnemyState FC_LASER_LOCK_RAGE_STATE = new FCLaserLockRageState();

	public static EnemyState FC_LASER_LOCK_STATE = new FCLaserLockState();

	public static EnemyState FC_FLY_BACK_STATE = new FCFlyBackState();

	public static EnemyState FC_INACTIVE_STATE = new FCInactiveState();

	protected byte m_currentMissileCount;

	private Transform m_laserMuzzleTransform;

	private float m_blueMissileStartTime;

	private float m_redMissileStartTime;

	private float m_laserLockStartTime;

	private float m_laserLockRageStartTime;

	private int m_damageInlaserLockRage;

	private List<Transform> m_targetList = new List<Transform>();

	private FloatCore m_float;

	private Timer m_laserlockDuration = new Timer();

	private Timer m_laserlockRageDuration = new Timer();

	private GameObject[] m_laserBeamArray = new GameObject[4];

	private GameObject[] m_laserBeamArray2 = new GameObject[4];

	private float m_hpPctForLaserLockRage;

	public bool m_readyChange;

	public bool m_finishChange;

	private bool mDeadEffect;

	private GameObject m_activeEffect;

	public override void Init()
	{
		base.Init();
		mNavWalkableMask = 1 << NavMeshLayer.FLY_AREA;
		mShieldType = ShieldType.MECHANICAL;
		mHpPercetageForRage = 0.9f;
		m_hpPctForLaserLockRage = 0.03f;
		m_readyChange = false;
		m_finishChange = false;
	}

	public override void Activate()
	{
		base.Activate();
		animationObject = entityTransform.Find(BoneName.FloatCtrlAnimationObject).gameObject;
		animation = animationObject.GetComponent<Animation>();
		m_readyChange = false;
		m_finishChange = false;
		mDeadEffect = false;
		m_currentMissileCount = 0;
		m_damageInlaserLockRage = 0;
		mMindState = MindState.NORMAL;
		SetBlueMissileLaunchNow();
		SetRedMissileLaunchNow();
		SetLaserLockNow();
		SetLaserLockRageNow();
		GameObject gameObject = new GameObject();
		gameObject.name = "CannonPoint";
		m_laserMuzzleTransform = gameObject.transform;
		m_laserMuzzleTransform.parent = mBodyTransform;
		m_laserMuzzleTransform.localPosition = new Vector3(0f, 0.02f, 0f);
		m_laserMuzzleTransform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		m_laserMuzzleTransform.localScale = Vector3.one;
		GameObject original = Resources.Load("RPG_effect/st_effect/boss_fly01_lightnine_sphere01") as GameObject;
		m_activeEffect = Object.Instantiate(original) as GameObject;
		m_activeEffect.transform.parent = mBodyTransform;
		m_activeEffect.transform.localPosition = new Vector3(0f, -2.015763f, 0f);
		m_activeEffect.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		m_activeEffect.transform.localScale = Vector3.one;
		m_activeEffect.SetActive(true);
		GameObject original2 = Resources.Load("RPG_effect/float/boss_float_laser_blue") as GameObject;
		for (int i = 0; i < m_laserBeamArray.Length; i++)
		{
			m_laserBeamArray[i] = Object.Instantiate(original2) as GameObject;
			m_laserBeamArray[i].transform.parent = mHeadTransform;
			m_laserBeamArray[i].transform.localPosition = new Vector3(0f, -1.2f, 0f);
			m_laserBeamArray[i].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			m_laserBeamArray[i].transform.localScale = Vector3.one;
			m_laserBeamArray[i].SetActive(false);
		}
		GameObject original3 = Resources.Load("RPG_effect/float/boss_float_laser_red") as GameObject;
		for (int j = 0; j < m_laserBeamArray2.Length; j++)
		{
			m_laserBeamArray2[j] = Object.Instantiate(original3) as GameObject;
			m_laserBeamArray2[j].transform.parent = mHeadTransform;
			m_laserBeamArray2[j].transform.localPosition = new Vector3(0f, -1.2f, 0f);
			m_laserBeamArray2[j].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			m_laserBeamArray2[j].transform.localScale = Vector3.one;
			m_laserBeamArray2[j].SetActive(false);
		}
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		m_laserlockDuration.SetTimer(mRangedOneShotTime2, false);
		m_laserlockRageDuration.SetTimer(mRangedBulletSpeed2, false);
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.FloatCtrlBody);
		mHeadTransform = entityTransform.Find(BoneName.FloatCtrlHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	protected override void InitRenders()
	{
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.FloatMesh;
	}

	public override void LinkEnemy(List<Enemy> enemyList)
	{
		foreach (Enemy enemy in enemyList)
		{
			if (enemy.EnemyType == EnemyType.FLOAT)
			{
				m_float = enemy as FloatCore;
				break;
			}
		}
	}

	public override bool CanBeHit()
	{
		if (m_float != null && m_float.GetPhase() == FloatCore.FloatPhase.Phase_2)
		{
			return true;
		}
		return false;
	}

	public override bool AimAssist()
	{
		if (m_float != null && m_float.GetPhase() == FloatCore.FloatPhase.Phase_2)
		{
			return true;
		}
		return false;
	}

	public override bool CanBeSpawn()
	{
		if (m_float == null)
		{
			return true;
		}
		if (m_float != null && m_float.Hp > 0)
		{
			return true;
		}
		return false;
	}

	private new bool InPlayingState()
	{
		if (mState == FC_INACTIVE_STATE)
		{
			return false;
		}
		return true;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (!mIsActive || !InPlayingState() || !base.IsMasterPlayer)
		{
			return;
		}
		if (m_float.GetPhase() == FloatCore.FloatPhase.Phase_1)
		{
			if (GetBlueMissileLaunchDuration() >= mRangedInterval1)
			{
				SetBlueMissileLaunchNow();
				StartShotMissile();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_SHOT_MISSILE, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else
		{
			if (m_float.GetPhase() != FloatCore.FloatPhase.Phase_2)
			{
				return;
			}
			if (GetBlueMissileLaunchDuration() >= (float)mRangedBulletCount1)
			{
				SetBlueMissileLaunchNow();
				StartShotMissile();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_SHOT_MISSILE, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
			if (GetRedMissileLaunchDuration() > (float)mRushAttackRadius && mMindState == MindState.RAGE)
			{
				SetRedMissileLaunchNow();
				StartShotRedMissile();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_SHOT_RED_MISSILE, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
			}
		}
	}

	public override bool CanPatrol()
	{
		return true;
	}

	private void SetRedMissileLaunchNow()
	{
		m_redMissileStartTime = Time.time;
	}

	private float GetRedMissileLaunchDuration()
	{
		return Time.time - m_redMissileStartTime;
	}

	private void ResetRedMissileLaunch()
	{
		m_redMissileStartTime = Time.time - mCoverInterval;
	}

	private void SetBlueMissileLaunchNow()
	{
		m_blueMissileStartTime = Time.time;
	}

	private float GetBlueMissileLaunchDuration()
	{
		return Time.time - m_blueMissileStartTime;
	}

	private void ResetLaserLockRage()
	{
		m_laserLockRageStartTime = Time.time - mRangedBulletSpeed2;
	}

	private void SetLaserLockRageNow()
	{
		m_laserLockRageStartTime = Time.time;
	}

	private float GetLaserLockRageDuration()
	{
		return Time.time - m_laserLockRageStartTime;
	}

	private void SetLaserLockNow()
	{
		m_laserLockStartTime = Time.time;
	}

	private float GetLaserLockDuration()
	{
		return Time.time - m_laserLockStartTime;
	}

	public override bool IsInvincible()
	{
		if (m_float.GetPhase() != FloatCore.FloatPhase.Phase_2)
		{
			return true;
		}
		return false;
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mMindState == MindState.NORMAL && (float)Hp <= (float)base.MaxHp * mHpPercetageForRage)
		{
			mMindState = MindState.RAGE;
			ResetRedMissileLaunch();
			ResetLaserLockRage();
			if (m_float != null)
			{
				m_float.ResetIrradiateLaser();
				m_float.ResetBlueLaser();
			}
		}
		if (GetState() == FC_LASER_LOCK_RAGE_STATE)
		{
			m_damageInlaserLockRage += dp.damage;
			if ((float)m_damageInlaserLockRage >= (float)base.MaxHp * m_hpPctForLaserLockRage)
			{
				EndFCLaserLockRage();
				StartEnemyIdle();
			}
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mMindState == MindState.NORMAL && (float)Hp <= (float)base.MaxHp * mHpPercetageForRage)
		{
			mMindState = MindState.RAGE;
			ResetRedMissileLaunch();
			ResetLaserLockRage();
			if (m_float != null)
			{
				m_float.ResetIrradiateLaser();
				m_float.ResetBlueLaser();
			}
		}
		if (GetState() == FC_LASER_LOCK_RAGE_STATE)
		{
			m_damageInlaserLockRage += damage;
			if ((float)m_damageInlaserLockRage >= (float)base.MaxHp * m_hpPctForLaserLockRage)
			{
				EndFCLaserLockRage();
				StartEnemyIdle();
			}
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == FC_LASER_LOCK_RAGE_STATE)
		{
			EndFCLaserLockRage();
		}
		else if (mState == FC_PATROL_STATE)
		{
			EndFCPatrol();
		}
		else if (mState == FC_LASER_LOCK_STATE)
		{
			EndFCLaserLock();
		}
		else if (mState == FC_FLY_BACK_STATE)
		{
			EndFCFlyBack();
		}
		else if (mState == FC_INACTIVE_STATE)
		{
			EndFCInactive();
		}
	}

	public override void CreateNavMeshAgent()
	{
	}

	protected void MyCreateNavMeshAgent()
	{
		if (mNavMeshAgent == null)
		{
			entityObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent = entityObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent.radius = cc.radius;
			mNavMeshAgent.height = 12f;
			mNavMeshAgent.baseOffset = mNavBaseOffset;
			mNavMeshAgent.speed = mWalkSpeed;
			mNavMeshAgent.angularSpeed = mNavAngularSpeed;
			mNavMeshAgent.acceleration = 10f;
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

	private void FollowFloat()
	{
		if (m_float != null)
		{
			Transform floatCtrlPoint = m_float.GetFloatCtrlPoint();
			if (floatCtrlPoint != null)
			{
				entityTransform.position = floatCtrlPoint.position;
				entityTransform.rotation = Quaternion.LookRotation(floatCtrlPoint.forward);
			}
		}
	}

	protected override void PlayOnHitBloodEffect(Vector3 position, ElementType elementType)
	{
		if (!gameScene.PlayOnHitElementEffect(position, elementType))
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_WALL_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
		}
	}

	public void StartFloatMove(Vector3 targetPosition)
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.speed = mWalkSpeed;
			mNavMeshAgent.updateRotation = false;
			SetCanTurn(false);
		}
	}

	public void StartFloatRest()
	{
	}

	public void StartFinalBattle()
	{
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/TMX/TMX_dead");
	}

	public override void StartDead()
	{
		m_activeEffect.SetActive(false);
		if (GetState() != FC_FLY_BACK_STATE && GetState() != FC_INACTIVE_STATE && base.IsMasterPlayer)
		{
			EndCurrentState();
			StartFCFlyBack();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_FLY_BACK, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartRealDead()
	{
		base.StartDead();
		mDeadEffect = false;
	}

	public override void DoDead()
	{
		PlayAnimation(AnimationString.FLOAT_CONTROLER_DEAD, WrapMode.ClampForever);
		float num = animation[AnimationString.FLOAT_CONTROLER_DEAD].time / animation[AnimationString.FLOAT_CONTROLER_DEAD].clip.length;
		if (!mDeadEffect && num > 0.9f)
		{
			mDeadEffect = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Die_001") as GameObject;
			Object.Instantiate(original, mBodyTransform.position, Quaternion.identity);
		}
		else if (num >= 1f)
		{
			EndDead();
		}
	}

	public override void EndDead()
	{
		Deactivate();
	}

	protected override void StartEnemyIdleWithoutResetTime()
	{
		SetState(Enemy.IDLE_STATE);
	}

	public override void DoEnemyIdle()
	{
		if (mMindState == MindState.NORMAL)
		{
			FollowFloat();
		}
		if (base.IsMasterPlayer && GetIdleTimeDuration() > mIdleTime)
		{
			MakeDecisionInEnemyIdle();
		}
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (m_float.GetPhase() != FloatCore.FloatPhase.Phase_2)
		{
			return;
		}
		if (mMindState == MindState.RAGE)
		{
			PatrolPointScript component = mNextPatrolLinePoint.GetComponent<PatrolPointScript>();
			if (null != component)
			{
				mNextPatrolLinePoint = component.NextPoint;
				StartFCPatrol(GetSkyPoint(mNextPatrolLinePoint.transform.position));
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_PATROL, entityTransform.position, mPatrolTarget);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else if (GetLaserLockDuration() > mRangedInterval2)
		{
			SetLaserLockNow();
			StartFCLaserLock();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_LASER_LOCK, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void StartFCPatrol(Vector3 patrolTarget)
	{
		SetState(FC_PATROL_STATE);
		SetPatrolTimeNow();
		SetPatrolTarget(patrolTarget);
		SetNavMeshForPatrol();
		SetUpdatePosTimeNow();
	}

	protected override void PlayWalkSound()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Float/float_fin_fly");
	}

	private bool EndFCPatrolCondition()
	{
		return GetPatrolTimeDuration() >= 2f || (mPatrolTarget - entityTransform.position).sqrMagnitude < 14f;
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

	public void DoFCPatrol()
	{
		Debug.DrawLine(entityTransform.position, mPatrolTarget, Color.blue);
		PlayWalkSound();
		if (base.IsMasterPlayer && GetLaserLockRageDuration() > (float)mRangedBulletCount2)
		{
			EndFCPatrol();
			StartFCLaserLockRage();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_LASER_LOCK_RAGE, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else if (EndFCPatrolCondition())
		{
			EndFCPatrol();
			StartEnemyIdle();
		}
		else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FC_PATROL, GetTransform().position, mPatrolTarget);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	private Vector3 GetSkyPoint(Vector3 groundPoint)
	{
		Vector3 result = groundPoint;
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		for (int i = 0; i < 5; i++)
		{
			Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			vector.Normalize();
			Vector3 origin = groundPoint + vector * i;
			mRay = new Ray(origin, Vector3.up);
			if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
			{
				result = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
				break;
			}
		}
		return result;
	}

	public void EndFCPatrol()
	{
		StopNavMesh();
		StopSound("RPG_Audio/Enemy/Float/float_fin_fly");
	}

	public override bool IsBoss()
	{
		return false;
	}

	public override void StartSeeBoss()
	{
		mIsActive = true;
		EndBossInit();
		StartEnemyIdle();
		MyCreateNavMeshAgent();
	}

	public override void StartBossBattle()
	{
	}

	protected override void StartBossInit()
	{
		SetState(EnemyBoss.INIT_STATE);
		mIsActive = false;
		mCanGotHit = false;
	}

	public override void DoBossInit()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
	}

	public void StartShotMissile()
	{
		GameObject original = Resources.Load("RPG_effect/float/boss_float_sphere_blue") as GameObject;
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				GameObject gameObject = Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.LookRotation(m_laserMuzzleTransform.forward)) as GameObject;
				FloatPlasmaScript component = gameObject.GetComponent<FloatPlasmaScript>();
				if (null != component)
				{
					Vector3 normalized = (m_laserMuzzleTransform.right + -3f * Vector3.up).normalized;
					component.mEnemy = this;
					component.mRisingSpeed = 3f * normalized;
					component.mRisingTime = 0.6f;
					component.mTargetPosition = item.GetTransform().position + Vector3.up * 1.2f;
					component.mExplosionDamage = mRangedAttackDamage1;
					component.mSpeedValue = mRangedBulletSpeed1;
					component.mExplosionRadius = mRangedExplosionRadius1;
					component.mTrackingFlyAcceleration = 10f;
				}
			}
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original2, m_laserMuzzleTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Float/float_lightball01");
	}

	public void StartShotRedMissile()
	{
		GameObject original = Resources.Load("RPG_effect/float/boss_float_sphere_red") as GameObject;
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				GameObject gameObject = Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.LookRotation(m_laserMuzzleTransform.forward)) as GameObject;
				FloatPlasmaScript component = gameObject.GetComponent<FloatPlasmaScript>();
				if (null != component)
				{
					Vector3 normalized = (m_laserMuzzleTransform.right + -3f * Vector3.up).normalized;
					component.mEnemy = this;
					component.mRisingSpeed = 3f * normalized;
					component.mRisingTime = 0.6f;
					component.mTargetPosition = item.GetTransform().position + Vector3.up * 1.2f;
					component.mExplosionDamage = mRushAttackDamage1;
					component.mSpeedValue = mRushAttackSpeed1;
					component.mExplosionRadius = mRangedExplosionRadius1;
					component.mTrackingPlayer = item;
					component.mTrackingFlyAcceleration = 6f;
					component.mTrackingUpdateTargetPosTimer.SetTimer(0.1f, false);
				}
			}
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original2, m_laserMuzzleTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Float/float_lightball01");
	}

	public void StartFCLaserLock()
	{
		SetState(FC_LASER_LOCK_STATE);
		m_laserlockDuration.Do();
	}

	private void LaserLock()
	{
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		int num = 0;
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				m_laserBeamArray[num].SetActive(true);
				Vector3 vector = item.GetTransform().position + Vector3.up * 1.5f;
				Vector3 normalized = (vector - m_laserBeamArray[num].transform.position).normalized;
				m_laserBeamArray[num].transform.rotation = Quaternion.LookRotation(normalized);
				FloatLaserLockScript component = m_laserBeamArray[num].GetComponent<FloatLaserLockScript>();
				if (null != component)
				{
					component.mEnemy = this;
					component.mTargetPos = vector;
					component.mDamage = 1;
				}
				num++;
			}
		}
		GameObject original = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.identity);
	}

	public void DoFCLaserLock()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Float/float_laser_fire02");
		FollowFloat();
		LaserLock();
		if (m_laserlockDuration.Ready())
		{
			EndFCLaserLock();
			StartEnemyIdle();
		}
	}

	private void EndFCLaserLock()
	{
		GameObject[] laserBeamArray = m_laserBeamArray;
		foreach (GameObject gameObject in laserBeamArray)
		{
			gameObject.SetActive(false);
		}
		StopSound("RPG_Audio/Enemy/Float/float_laser_fire02");
	}

	public void StartFCLaserLockRage()
	{
		SetState(FC_LASER_LOCK_RAGE_STATE);
		m_laserlockRageDuration.Do();
		m_damageInlaserLockRage = 0;
	}

	private void LaserLockRage()
	{
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		int num = 0;
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				m_laserBeamArray2[num].SetActive(true);
				Vector3 vector = item.GetTransform().position + Vector3.up * 1.5f;
				Vector3 normalized = (vector - m_laserBeamArray2[num].transform.position).normalized;
				m_laserBeamArray2[num].transform.rotation = Quaternion.LookRotation(normalized);
				FloatLaserLockScript component = m_laserBeamArray2[num].GetComponent<FloatLaserLockScript>();
				if (null != component)
				{
					component.mEnemy = this;
					component.mTargetPos = vector;
					component.mDamage = 1;
				}
				num++;
			}
		}
		GameObject original = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.identity);
	}

	public void DoFCLaserLockRage()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Float/float_laser_fire02");
		LaserLockRage();
		if (m_laserlockRageDuration.Ready())
		{
			EndFCLaserLockRage();
		}
	}

	private void EndFCLaserLockRage()
	{
		SetLaserLockRageNow();
		GameObject[] laserBeamArray = m_laserBeamArray2;
		foreach (GameObject gameObject in laserBeamArray)
		{
			gameObject.SetActive(false);
		}
		StopSound("RPG_Audio/Enemy/Float/float_laser_fire02");
	}

	public void StartFCFlyBack()
	{
		GameObject[] laserBeamArray = m_laserBeamArray;
		foreach (GameObject gameObject in laserBeamArray)
		{
			gameObject.SetActive(false);
		}
		GameObject[] laserBeamArray2 = m_laserBeamArray2;
		foreach (GameObject gameObject2 in laserBeamArray2)
		{
			gameObject2.SetActive(false);
		}
		SetState(FC_FLY_BACK_STATE);
	}

	public void DoFCFlyBack()
	{
		PlayWalkSound();
		Transform floatCtrlPoint = m_float.GetFloatCtrlPoint();
		if (floatCtrlPoint != null)
		{
			Vector3 normalized = (floatCtrlPoint.position - GetPosition()).normalized;
			float magnitude = (floatCtrlPoint.position - GetPosition()).magnitude;
			Vector3 vector = Vector3.RotateTowards(entityTransform.forward, normalized, 0.03f, 1f);
			entityTransform.LookAt(entityTransform.position + vector);
			float num = Mathf.Max(mRunSpeed - 10f / (magnitude + 1f), 2f);
			entityTransform.position += num * entityTransform.forward * Time.deltaTime;
			if ((floatCtrlPoint.position - GetPosition()).magnitude <= num)
			{
				EndFCFlyBack();
				StartFCInactive();
			}
		}
	}

	private void EndFCFlyBack()
	{
		FollowFloat();
		m_readyChange = true;
		StopSound("RPG_Audio/Enemy/Float/float_fin_fly");
	}

	public void StartFCInactive()
	{
		SetState(FC_INACTIVE_STATE);
	}

	public void DoFCInactive()
	{
		FollowFloat();
	}

	public void EndFCInactive()
	{
	}
}
