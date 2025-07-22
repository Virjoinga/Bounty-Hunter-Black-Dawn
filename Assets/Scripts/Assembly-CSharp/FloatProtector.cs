using System.Collections.Generic;
using UnityEngine;

public class FloatProtector : EnemyBoss
{
	public static EnemyState FP_FLY_STATE = new FPFlyState();

	public static EnemyState FP_FLY_BACK_STATE = new FPFlyBackState();

	public static EnemyState FP_INACTIVE_STATE = new FPInactiveState();

	public static EnemyState FP_CHANGE_TO_STATE = new FPChangeToState();

	public static EnemyState FP_READY_CHANGE_STATE = new FPReadyChangeState();

	public static EnemyState FP_FLY_ATTACK_STATE = new FPFlyAttackState();

	private string m_currentChangeAnim = "01";

	private EnemyState m_endChangeState;

	protected byte m_currentMissileCount;

	private Transform m_laserMuzzleTransform;

	private GameObject mFireSparkObject;

	private Transform mFireSparkTransform;

	private float m_blueMissileStartTime;

	private List<Transform> m_targetList = new List<Transform>();

	private FloatCore m_float;

	private int m_pointId;

	private float m_flyTime;

	private float m_flyAttackTime;

	private Timer m_flyAttackDuration = new Timer();

	private bool m_flying;

	private float m_flyRotateTime;

	private Vector3 m_nextTargetPos;

	private int m_currBulletNum;

	public bool m_finishChange;

	public bool m_readyChange;

	private bool mDeadEffect;

	private Timer mAnimWaitTime = new Timer();

	private GameObject m_activeEffect;

	public override void Init()
	{
		base.Init();
		mNavWalkableMask = 1 << NavMeshLayer.FLY_AREA;
		mShieldType = ShieldType.MECHANICAL;
		m_finishChange = false;
		m_readyChange = false;
	}

	public override void Activate()
	{
		base.Activate();
		animationObject = entityTransform.Find(BoneName.FloatPrtcAnimationObject).gameObject;
		animation = animationObject.GetComponent<Animation>();
		m_endChangeState = null;
		m_currentMissileCount = 0;
		m_flying = false;
		m_currBulletNum = 0;
		m_finishChange = false;
		m_readyChange = false;
		mDeadEffect = false;
		mMindState = MindState.NORMAL;
		SetBlueMissileLaunchNow();
		SetFlyTimeNow();
		SetFlyAttackTimeNow();
		GameObject original = Resources.Load("RPG_effect/st_effect/boss_fly01_lightnine") as GameObject;
		m_activeEffect = Object.Instantiate(original) as GameObject;
		m_activeEffect.transform.parent = mBodyTransform;
		m_activeEffect.transform.localPosition = new Vector3(0f, -0.6582337f, 0f);
		m_activeEffect.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		m_activeEffect.transform.localScale = Vector3.one;
		m_activeEffect.SetActive(true);
		GameObject original2 = Resources.Load("RPG_effect/RPG_Cypher_Fire_001") as GameObject;
		mFireSparkObject = Object.Instantiate(original2) as GameObject;
		mFireSparkObject.name = "GunSpark";
		mFireSparkObject.SetActive(false);
		mFireSparkTransform = mFireSparkObject.transform;
		mFireSparkTransform.parent = mBodyTransform;
		mFireSparkTransform.localPosition = new Vector3(0f, -2.1f, 0f);
		mFireSparkTransform.localRotation = Quaternion.identity;
		mFireSparkTransform.localScale = Vector3.one;
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		m_pointId = mCoverSearchRadius;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.FloatPrtcBody);
		mHeadTransform = entityTransform.Find(BoneName.FloatPrtcHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	protected override void InitRenders()
	{
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

	public override bool CanBeHit()
	{
		if (m_float != null && m_float.GetPhase() == FloatCore.FloatPhase.Phase_1)
		{
			return true;
		}
		return false;
	}

	public override bool AimAssist()
	{
		if (m_float != null && m_float.GetPhase() == FloatCore.FloatPhase.Phase_1)
		{
			return true;
		}
		return false;
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

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (mIsActive)
		{
		}
	}

	public FloatCore GetFloat()
	{
		return m_float;
	}

	public override bool CanPatrol()
	{
		return false;
	}

	public float GetFlyTimeDuration()
	{
		return Time.time - m_flyTime;
	}

	public void SetFlyTimeNow()
	{
		m_flyTime = Time.time;
	}

	public float GetFlyAttackTimeDuration()
	{
		return Time.time - m_flyAttackTime;
	}

	public void SetFlyAttackTimeNow()
	{
		m_flyAttackTime = Time.time;
	}

	private void SetBlueMissileLaunchNow()
	{
		m_blueMissileStartTime = Time.time;
	}

	private float GetBlueMissileLaunchDuration()
	{
		return Time.time - m_blueMissileStartTime;
	}

	public override bool IsInvincible()
	{
		if (m_float.GetPhase() != FloatCore.FloatPhase.Phase_1)
		{
			return true;
		}
		return false;
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mMindState == MindState.NORMAL && (float)Hp < (float)base.MaxHp * (1f - mHpPercetageForRage))
		{
			mMindState = MindState.RAGE;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mMindState == MindState.NORMAL && (float)Hp < (float)base.MaxHp * (1f - mHpPercetageForRage))
		{
			mMindState = MindState.RAGE;
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == FP_FLY_STATE)
		{
			EndFPFly();
		}
		else if (mState == FP_FLY_ATTACK_STATE)
		{
			EndFPFlyAttack();
		}
		else if (mState == FP_FLY_BACK_STATE)
		{
			EndFPFlyBack();
		}
		else if (mState == FP_INACTIVE_STATE)
		{
			EndFPInactive();
		}
		else if (mState == FP_CHANGE_TO_STATE)
		{
			EndFPChangeTo();
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
			Transform floatPrtcPoint = m_float.GetFloatPrtcPoint(m_pointId);
			if (floatPrtcPoint != null)
			{
				StopAnimation();
				mHeadTransform.localPosition = Vector3.zero;
				mHeadTransform.localRotation = Quaternion.Euler(270f, 0f, 0f);
				entityTransform.position = floatPrtcPoint.position;
				entityTransform.rotation = Quaternion.LookRotation(floatPrtcPoint.forward);
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
		PlaySound("RPG_Audio/Environment/start_fighting");
		Debug.Log("Start Final Battle!!!");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/TMX/TMX_dead");
	}

	public override void StartDead()
	{
		m_activeEffect.SetActive(false);
		if (base.IsMasterPlayer && GetState() != FP_READY_CHANGE_STATE && GetState() != FP_CHANGE_TO_STATE && GetState() != FP_INACTIVE_STATE)
		{
			StartReadyChange();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_READY_CHANGE, entityTransform.position);
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
		PlayAnimation(AnimationString.FLOAT_PROTECTOR_DEAD, WrapMode.ClampForever);
		float num = animation[AnimationString.FLOAT_PROTECTOR_DEAD].time / animation[AnimationString.FLOAT_PROTECTOR_DEAD].clip.length;
		if (num > 0.9f && !mDeadEffect)
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
		Debug.Log("EndDead -------------------");
	}

	public override void StartPatrol(Vector3 patrolTarget)
	{
		base.StartPatrol(patrolTarget);
		StopSound("RPG_Audio/Enemy/Cypher/cypher_idle");
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

	protected override void PlayWalkSound()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Cypher/cypher_move");
	}

	protected override void EndPatrol()
	{
		base.EndPatrol();
		StopSound("RPG_Audio/Enemy/Cypher/cypher_move");
	}

	protected override void StartEnemyIdleWithoutResetTime()
	{
		SetState(Enemy.IDLE_STATE);
	}

	public override void DoEnemyIdle()
	{
		FollowFloat();
		if (base.IsMasterPlayer && GetIdleTimeDuration() > mIdleTime)
		{
			MakeDecisionInEnemyIdle();
		}
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget != null)
		{
			EndEnemyIdle();
			StartFPFly();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_FLY, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected override void EndEnemyIdle()
	{
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

	public override bool IsBoss()
	{
		return false;
	}

	public override void StartSeeBoss()
	{
		mIsActive = true;
		EndBossInit();
		Debug.Log("StartSeeBoss: " + base.Name);
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
		FollowFloat();
	}

	public void StartFPFly()
	{
		Debug.Log("--------------------------------");
		SetState(FP_FLY_STATE);
		SetFlyTimeNow();
		SetFlyAttackTimeNow();
		m_flying = false;
	}

	private void Shoot()
	{
		if (mIsShoot)
		{
			mFireSparkObject.SetActive(false);
		}
		else if (!mIsShoot)
		{
			CheckShoot();
			m_currBulletNum++;
			mIsShoot = true;
			mFireSparkObject.SetActive(true);
		}
	}

	private void CheckShoot()
	{
		CheckShoot(mFireSparkTransform);
		PlaySound("RPG_Audio/Weapon/assault_rifle/assault_fire");
	}

	private void CheckShoot(Transform fireTransform)
	{
		bool hitLocalPlayer = false;
		Vector3 vector = mTargetPosition + new Vector3(0f, 1.5f, 0f);
		if (mTarget == mLocalPlayer)
		{
			float distanceFromTarget = GetDistanceFromTarget();
			float num = Random.Range(0f, distanceFromTarget);
			if (num < 4f)
			{
				vector = vector + Random.Range(-0.1f, 0.1f) * entityTransform.right + Random.Range(-0.1f, 0.1f) * entityTransform.up;
			}
			else
			{
				float num2 = Random.Range(-0.01f, 0.01f);
				vector = vector + (num2 * distanceFromTarget + 0.5f * Mathf.Sign(num2)) * entityTransform.right + Random.Range(-0.7f, 0.7f) * entityTransform.up;
			}
		}
		else
		{
			vector = vector + Random.Range(-1f, 1f) * entityTransform.right + Random.Range(-0.5f, 0.5f) * entityTransform.up;
		}
		bool hitCollider = false;
		Vector3 vector2 = new Vector3(entityTransform.position.x, fireTransform.position.y, entityTransform.position.z);
		Vector3 direction = vector - vector2;
		direction.Normalize();
		mRay = new Ray(vector2, direction);
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED);
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			hitCollider = true;
			if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				mLocalPlayer.OnHit(mRangedAttackDamage1, this);
				hitLocalPlayer = true;
			}
			else if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.SUMMONED)
			{
				GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(mRaycastHit.collider);
				if (controllableByCollider != null)
				{
					SummonedItem summonedByName = mLocalPlayer.GetSummonedByName(controllableByCollider.name);
					if (summonedByName != null)
					{
						summonedByName.OnHit(mRangedAttackDamage1);
					}
				}
			}
		}
		CreateBulletTrail(fireTransform, hitCollider, hitLocalPlayer, direction);
	}

	private void CreateBulletTrail(Transform fireTransform, bool hitCollider, bool hitLocalPlayer, Vector3 direction)
	{
		if (!hitLocalPlayer && (mRaycastHit.point - fireTransform.position).sqrMagnitude < 25f)
		{
			return;
		}
		GameObject gameObject = gameScene.GetEffectPool(EffectPoolType.BULLET_TRAIL).CreateObject(fireTransform.position + 2.5f * direction, direction, Quaternion.identity);
		if (gameObject == null)
		{
			Debug.Log("fire line obj null");
			return;
		}
		BulletTrailScript component = gameObject.GetComponent<BulletTrailScript>();
		component.beginPos = fireTransform.position + 2.5f * direction;
		if (hitCollider)
		{
			if (hitLocalPlayer)
			{
				component.endPos = mRaycastHit.point + 2.5f * direction;
			}
			else
			{
				component.endPos = mRaycastHit.point - 2.5f * direction;
			}
		}
		else
		{
			component.endPos = fireTransform.position + direction * 100f;
		}
		component.speed = 100f;
		component.isActive = true;
	}

	public void StartFlyToTarget(Vector3 target)
	{
		m_flying = true;
		m_nextTargetPos = target;
		m_flyRotateTime = Time.time;
	}

	public void DoFPFlyAround()
	{
		if (base.IsMasterPlayer && NeedFlyBack())
		{
			Debug.Log("GoBack............................");
			StartFPFlyBack();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_FLY_BACK, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			return;
		}
		if (!m_flying)
		{
			if (base.IsMasterPlayer)
			{
				m_flying = true;
				if (mTarget == null)
				{
					ChooseTargetPlayer(true);
				}
				if (mTarget != null)
				{
					m_nextTargetPos = GetDestination(mTarget.GetPosition());
					m_flyRotateTime = Time.time;
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_FLY_TO, entityTransform.position, m_nextTargetPos);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
		}
		else if ((m_nextTargetPos - GetPosition()).magnitude <= mRunSpeed)
		{
			m_flying = false;
		}
		else if (CheckDistanceWithGround() && Mathf.Abs(Time.time - m_flyRotateTime) < 3f)
		{
			m_flyRotateTime = Time.time - 3f;
		}
		else
		{
			Vector3 normalized = (m_nextTargetPos - GetPosition()).normalized;
			Quaternion to = Quaternion.LookRotation(normalized);
			GetTransform().rotation = Quaternion.Lerp(GetTransform().rotation, to, (Time.time - m_flyRotateTime) / 3f);
			entityTransform.position += mRunSpeed * entityTransform.forward * Time.deltaTime;
		}
		if (!base.IsMasterPlayer || !(GetFlyAttackTimeDuration() > mRangedInterval1))
		{
			return;
		}
		SetFlyAttackTimeNow();
		if (canHitTargetPlayer() && GetHorizontalDistanceFromTarget() > 15f)
		{
			EndFPFly();
			StartFPFlyAttack();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_FLY_ATTACK, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
			}
		}
	}

	private bool CheckDistanceWithGround()
	{
		mRay = new Ray(GetTransform().position, GetTransform().forward);
		int layerMask = 1 << PhysicsLayer.FLOOR;
		if (Physics.Raycast(mRay, out mRaycastHit, 12f, layerMask))
		{
			return true;
		}
		return false;
	}

	private Vector3 GetDestination(Vector3 pos)
	{
		Vector3 vector2;
		int layerMask;
		do
		{
			Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			vector.Normalize();
			float num = Random.Range(50f, 80f);
			float num2 = Random.Range(1f, 5f);
			vector2 = pos + Vector3.up * 8f + new Vector3(vector.x * num, vector.y * num2, vector.z * num);
			mRay = new Ray(vector2, Vector3.up);
			layerMask = 1 << PhysicsLayer.FLY_AREA;
		}
		while (!Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask) || !((vector2 - GetPosition()).magnitude > mRunSpeed));
		return vector2;
	}

	private bool NeedFlyBack()
	{
		if (GetFlyTimeDuration() >= 40f || m_currBulletNum >= mRangedBulletCount1)
		{
			return true;
		}
		return false;
	}

	private void EndFPFly()
	{
	}

	public void StartFPFlyAttack()
	{
		SetState(FP_FLY_ATTACK_STATE);
		m_flyAttackDuration.SetTimer(0.5f, false);
		mIsShoot = false;
	}

	public void DoFPFlyAttack()
	{
		Shoot();
		if (m_flyAttackDuration.Ready())
		{
			EndFPFlyAttack();
			StartFPFly();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FP_FLY, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void EndFPFlyAttack()
	{
	}

	public void StartFPFlyBack()
	{
		SetState(FP_FLY_BACK_STATE);
	}

	public void DoFPFlyBack()
	{
		Transform floatPrtcPoint = m_float.GetFloatPrtcPoint(m_pointId);
		if (floatPrtcPoint != null)
		{
			Vector3 normalized = (floatPrtcPoint.position - GetPosition()).normalized;
			float magnitude = (floatPrtcPoint.position - GetPosition()).magnitude;
			Vector3 vector = ((!CheckDistanceWithGround()) ? Vector3.RotateTowards(entityTransform.forward, normalized, 0.03f, 1f) : Vector3.RotateTowards(entityTransform.forward, normalized, 0.5f, 1f));
			entityTransform.LookAt(entityTransform.position + vector);
			float num = Mathf.Max(mRunSpeed * 2f - mRunSpeedInit * 2f / (magnitude + 1f), 2f);
			entityTransform.position += num * entityTransform.forward * Time.deltaTime;
			if ((floatPrtcPoint.position - GetPosition()).magnitude <= num + 1f)
			{
				EndFPFlyBack();
				StartEnemyIdle();
			}
		}
	}

	private void EndFPFlyBack()
	{
		m_currBulletNum = 0;
		FollowFloat();
	}

	public void StartReadyChange()
	{
		base.ShieldRecovery = 0;
		m_readyChange = false;
		SetState(FP_READY_CHANGE_STATE);
	}

	public void DoFPReadyChange()
	{
		Transform floatPrtcPoint = m_float.GetFloatPrtcPoint(m_pointId);
		if (floatPrtcPoint != null)
		{
			Vector3 normalized = (floatPrtcPoint.position - GetPosition()).normalized;
			float magnitude = (floatPrtcPoint.position - GetPosition()).magnitude;
			Vector3 vector = ((!CheckDistanceWithGround()) ? Vector3.RotateTowards(entityTransform.forward, normalized, 0.03f, 1f) : Vector3.RotateTowards(entityTransform.forward, normalized, 0.5f, 1f));
			entityTransform.LookAt(entityTransform.position + vector);
			float num = Mathf.Max(mRunSpeed * 2f - mRunSpeedInit * 2f / (magnitude + 1f), 2f);
			entityTransform.position += num * entityTransform.forward * Time.deltaTime;
			if ((floatPrtcPoint.position - GetPosition()).magnitude <= num + 1f)
			{
				EndFPReadyChange();
				StartFPInactive();
			}
		}
	}

	private void EndFPReadyChange()
	{
		m_readyChange = true;
		FollowFloat();
	}

	public void StartFPInactive()
	{
		Debug.Log("StartFPInactive----------------");
		SetState(FP_INACTIVE_STATE);
	}

	public void DoFPInactive()
	{
		FollowFloat();
	}

	public void EndFPInactive()
	{
	}

	public void StartFPChangeTo1()
	{
		SetState(FP_CHANGE_TO_STATE);
		SetStringPhase("00");
		m_endChangeState = Enemy.IDLE_STATE;
		m_finishChange = false;
		SetPosition(m_float.GetPosition());
		GetTransform().rotation = Quaternion.LookRotation(m_float.GetTransform().forward);
	}

	public void StartFPChangeTo2()
	{
		SetState(FP_CHANGE_TO_STATE);
		SetStringPhase("01");
		m_endChangeState = FP_INACTIVE_STATE;
		m_finishChange = false;
		SetPosition(m_float.GetPosition());
		GetTransform().rotation = Quaternion.LookRotation(m_float.GetTransform().forward);
	}

	public void StartFPChange2To3()
	{
		SetState(FP_CHANGE_TO_STATE);
		SetStringPhase("02");
		m_finishChange = false;
		m_endChangeState = FP_INACTIVE_STATE;
		SetPosition(m_float.GetPosition());
		GetTransform().rotation = Quaternion.LookRotation(m_float.GetTransform().forward);
	}

	public void StartFPChange3To4()
	{
		SetState(FP_CHANGE_TO_STATE);
		SetStringPhase("03");
		m_finishChange = false;
		m_endChangeState = FP_INACTIVE_STATE;
		SetPosition(m_float.GetPosition());
		GetTransform().rotation = Quaternion.LookRotation(m_float.GetTransform().forward);
	}

	public void StartFPChange4To3()
	{
		SetState(FP_CHANGE_TO_STATE);
		SetStringPhase("03");
		m_currentChangeAnim += "_back";
		m_finishChange = false;
		m_endChangeState = FP_INACTIVE_STATE;
		SetPosition(m_float.GetPosition());
		GetTransform().rotation = Quaternion.LookRotation(m_float.GetTransform().forward);
	}

	public void DoFPChangeTo()
	{
		if (!m_finishChange)
		{
			string name = AnimationString.FLOAT_PROTECTOR_CHANGE + (m_pointId + 1) + m_currentChangeAnim;
			PlayAnimation(name, WrapMode.ClampForever);
			if (AnimationPlayed(name, 1f))
			{
				m_finishChange = true;
			}
		}
	}

	public void EndFPChangeTo()
	{
		FollowFloat();
	}

	private void SetStringPhase(string strAnim)
	{
		m_currentChangeAnim = strAnim;
	}
}
