using System.Collections.Generic;
using UnityEngine;

public class Cypher : Enemy
{
	public static EnemyState CYPHER_MOVE_STATE = new CypherMoveState();

	public static EnemyState CYPHER_FIRE_STATE = new CypherFireState();

	public static EnemyState CYPHER_CANNON_STATE = new CypherCannonState();

	private float mLastMoveTime;

	private int mCurrentBulletCount;

	private int mCurrentCannonCount;

	private Transform mLeftFireSparkTransform;

	private Transform mRightFireSparkTransform;

	private Transform mCannonTransform;

	private GameObject mLeftFireSparkObject;

	private GameObject mRightFireSparkObject;

	private GameObject mPowerEffectObject;

	private GameObject mDeadSmokeObject;

	private Vector3 mCircuitousTarget;

	private bool mNeedCircuitousMove;

	private float mFireAnimationLength;

	private float mCannonAnimationLength;

	private Timer mCanHitTargetTimer = new Timer();

	private Timer mFireCheckTimer = new Timer();

	private bool mNeverSeePlayer;

	private Vector3 mDeadDirection;

	private float mDeadDropSpeed;

	public override bool CanPatrol()
	{
		return true;
	}

	public float GetMoveTimeDuration()
	{
		return Time.time - mLastMoveTime;
	}

	public void SetMoveTimeNow()
	{
		mLastMoveTime = Time.time;
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

	public override void Init()
	{
		base.Init();
		mShadowPath = string.Empty;
		mShieldType = ShieldType.MECHANICAL;
		mNavAngularSpeed = 180f;
		mTurnSpeed = 0.015f;
		mNavWalkableMask = 1 << NavMeshLayer.FLY_AREA;
		mLastMoveTime = Time.time;
		mFireAnimationLength = 5f;
		mCannonAnimationLength = 10f;
		mCanHitTargetTimer.SetTimer(1f, false);
		mFireCheckTimer.SetTimer(10f, false);
		mDeadTimer.SetTimer(0.5f, false);
		base.SpawnPosition = GetSkyPoint(base.SpawnPosition);
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
			mNavMeshAgent.height = cc.height;
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

	public override void Activate()
	{
		base.Activate();
		animationObject = entityTransform.Find(BoneName.CypherAnimationObject).gameObject;
		animation = animationObject.GetComponent<Animation>();
		animation[AnimationString.CYPHER_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		animation[AnimationString.CYPHER_CANNON].speed = GetFireSpeedCoefficent() * mCannonAnimationLength / 30f / mRangedOneShotTime2;
		GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Fire_001") as GameObject;
		mLeftFireSparkObject = Object.Instantiate(original) as GameObject;
		mLeftFireSparkObject.name = "LeftSpark";
		mLeftFireSparkTransform = mLeftFireSparkObject.transform;
		mLeftFireSparkTransform.parent = mBodyTransform;
		mLeftFireSparkTransform.localPosition = new Vector3(0.58f, -0.43f, 0.15f);
		mLeftFireSparkTransform.localRotation = Quaternion.identity;
		mLeftFireSparkTransform.localScale = Vector3.one;
		mRightFireSparkObject = Object.Instantiate(original) as GameObject;
		mRightFireSparkObject.name = "RightSpark";
		mRightFireSparkTransform = mRightFireSparkObject.transform;
		mRightFireSparkTransform.parent = mBodyTransform;
		mRightFireSparkTransform.localPosition = new Vector3(0.58f, 0.43f, 0.15f);
		mRightFireSparkTransform.localRotation = Quaternion.identity;
		mRightFireSparkTransform.localScale = Vector3.one;
		GameObject gameObject = new GameObject();
		gameObject.name = "CannonPoint";
		mCannonTransform = gameObject.transform;
		mCannonTransform.parent = mBodyTransform;
		mCannonTransform.localPosition = new Vector3(0.57f, 0.02f, 0.2f);
		mCannonTransform.localRotation = Quaternion.Euler(0f, 90f, 0f);
		mCannonTransform.localScale = Vector3.one;
		GameObject original2 = Resources.Load("RPG_effect/RPG_Cypher_001") as GameObject;
		mPowerEffectObject = Object.Instantiate(original2) as GameObject;
		mPowerEffectObject.transform.parent = mHeadTransform;
		mPowerEffectObject.transform.localPosition = new Vector3(-0.13f, 0f, -0.38f);
		mPowerEffectObject.transform.localRotation = Quaternion.Euler(0f, 16.77f, 0f);
		mPowerEffectObject.transform.localScale = Vector3.one;
		GameObject original3 = Resources.Load("RPG_effect/RPG_Cypher_Die_002") as GameObject;
		mDeadSmokeObject = Object.Instantiate(original3) as GameObject;
		mDeadSmokeObject.transform.parent = mBodyTransform;
		mDeadSmokeObject.transform.localPosition = Vector3.one;
		mDeadSmokeObject.transform.localRotation = Quaternion.identity;
		mDeadSmokeObject.transform.localScale = Vector3.one;
		mLeftFireSparkObject.SetActive(false);
		mRightFireSparkObject.SetActive(false);
		mPowerEffectObject.SetActive(true);
		mDeadSmokeObject.SetActive(false);
		mNeedCircuitousMove = false;
		if (SpawnType == ESpawnType.STREAMING)
		{
			mNeverSeePlayer = false;
		}
		else
		{
			mNeverSeePlayer = true;
		}
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.CypherBody);
		mHeadTransform = entityTransform.Find(BoneName.CypherHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	protected override void InitRenders()
	{
	}

	protected override void PlayOnHitBloodEffect(Vector3 position, ElementType elementType)
	{
		if (!gameScene.PlayOnHitElementEffect(position, elementType))
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_WALL_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
		}
	}

	protected override void PlayDeadBloodEffect()
	{
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (InPlayingState() && mState != Enemy.PATROL_STATE && mState != Enemy.CATCHING_STATE && mState != CYPHER_MOVE_STATE)
		{
			PlaySoundSingle("RPG_Audio/Enemy/Cypher/cypher_idle");
		}
	}

	private void Shoot(string shootAnimation)
	{
		if (!mIsShoot)
		{
			CheckShoot();
			mIsShoot = true;
			mLeftFireSparkObject.SetActive(true);
			mRightFireSparkObject.SetActive(true);
		}
		if (mIsShoot && animation[shootAnimation].time > animation[shootAnimation].clip.length)
		{
			animation[shootAnimation].time -= animation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
			mLeftFireSparkObject.SetActive(false);
			mRightFireSparkObject.SetActive(false);
		}
	}

	private void CheckShoot()
	{
		CheckShoot(mLeftFireSparkTransform);
		CheckShoot(mRightFireSparkTransform);
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

	public void CannonShoot()
	{
		Vector3 forward = mTargetPosition - mCannonTransform.position;
		GameObject original = Resources.Load("RPG_effect/RPG_Cypher_rocket_001") as GameObject;
		GameObject gameObject = Object.Instantiate(original, mCannonTransform.position, Quaternion.LookRotation(forward)) as GameObject;
		EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
		component.enemy = this;
		component.speed = forward.normalized * mRangedBulletSpeed2;
		component.attackDamage = 0;
		component.areaDamage = mRangedExtraDamage2;
		component.damageType = DamageType.Explosion;
		component.trType = TrajectoryType.Straight;
		component.enemyType = mEnemyType;
		component.explodeEffect = "RPG_effect/RPG_shell_fire_001";
		component.explodeRadius = mRangedExplosionRadius2;
		GameObject original2 = Resources.Load("RPG_effect/RPG_Cypher_Fire_002") as GameObject;
		Object.Instantiate(original2, mCannonTransform.position, Quaternion.LookRotation(forward));
		PlaySound("RPG_Audio/Enemy/Shell/Shell_fire");
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

	public override void SetNavMeshForGoBack()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			mNavMeshAgent.speed = 2f * mRunSpeed;
			SetCanTurn(false);
		}
	}

	public void SetNavMeshForMove()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mCircuitousTarget);
			mNavMeshAgent.speed = mWalkSpeed;
			SetCanTurn(true);
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

	private void SetCircuitousTarget(Vector3 target)
	{
		mCircuitousTarget = target;
	}

	private string GetMoveAnimation(Vector3 moveDirection)
	{
		string result = AnimationString.CYPHER_MOVE_FORWARD;
		float num = Vector3.Angle(moveDirection, entityTransform.forward);
		if (num > 135f)
		{
			result = AnimationString.CYPHER_MOVE_FORWARD;
		}
		else if (num > 45f)
		{
			result = ((!(Vector3.Cross(moveDirection, entityTransform.forward).y > 0f)) ? AnimationString.CYPHER_MOVE_RIGHT : AnimationString.CYPHER_MOVE_LEFT);
		}
		return result;
	}

	private string GetMoveAnimation()
	{
		string result = AnimationString.HUMAN_MOVE_FORWARD;
		if (null != mNavMeshAgent)
		{
			result = GetMoveAnimation(mNavMeshAgent.velocity);
		}
		return result;
	}

	protected override string GetRunAnimationName()
	{
		return AnimationString.CYPHER_MOVE_FORWARD;
	}

	protected override string GetWalkAnimationName()
	{
		return AnimationString.CYPHER_MOVE_FORWARD;
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mState == Enemy.IDLE_STATE || mState == CYPHER_FIRE_STATE || mState == CYPHER_CANNON_STATE || mState == Enemy.GOTHIT_STATE)
		{
			mNeedCircuitousMove = true;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mState == Enemy.IDLE_STATE || mState == CYPHER_FIRE_STATE || mState == CYPHER_CANNON_STATE || mState == Enemy.GOTHIT_STATE)
		{
			mNeedCircuitousMove = true;
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (CYPHER_MOVE_STATE == mState)
		{
			EndCypherMove();
		}
		else if (CYPHER_FIRE_STATE == mState)
		{
			EndCypherFire();
		}
		else if (CYPHER_CANNON_STATE == mState)
		{
			EndCypherCannon();
		}
	}

	protected override void PlayAwakeSound()
	{
	}

	public override void StartCatching()
	{
		base.StartCatching();
		StopSound("RPG_Audio/Enemy/Cypher/cypher_idle");
	}

	protected override void PlayRunningSound()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Cypher/cypher_move");
	}

	protected override void MakeDecisionInCatching()
	{
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		if (!(horizontalSqrDistanceFromTarget < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius)) || !mCanHitTargetTimer.Ready())
		{
			return;
		}
		mCanHitTargetTimer.Do();
		if (canHitTargetPlayer())
		{
			EndCatching();
			mNeverSeePlayer = false;
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			int num = Random.Range(0, 100);
			if (num < mRangedAttackProbability)
			{
				StartCypherFire();
				enemyStateConst = EnemyStateConst.CYPHER_FIRE;
			}
			else
			{
				StartCypherCannon();
				enemyStateConst = EnemyStateConst.CYPHER_CANNON;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected override void EndCatching()
	{
		base.EndCatching();
		StopSound("RPG_Audio/Enemy/Cypher/cypher_move");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Cypher/cypher_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		mPowerEffectObject.SetActive(false);
		mDeadSmokeObject.SetActive(true);
		mDeadDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		mDeadDirection.Normalize();
		mDeadDropSpeed = Random.Range(15f, 20f);
		mDeadTimer.SetTimer(Random.Range(0.1f, 0.3f), false);
		animation[AnimationString.ENEMY_DEAD].speed = Random.Range(0.5f, 1f);
	}

	public override void DoDead()
	{
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		if (mOnGround)
		{
			GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Die_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
			PlaySound("Audio/rpg/rpg-21_boom");
			if ((mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < mRangedExplosionRadius1 * mRangedExplosionRadius1)
			{
				Ray ray = new Ray(entityTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - entityTransform.position);
				float distance = Vector3.Distance(entityTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
				{
					mLocalPlayer.OnHit(mRangedExtraDamage1, this);
				}
			}
			Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
			foreach (SummonedItem value in summonedList.Values)
			{
				if ((value.GetTransform().position - entityTransform.position).sqrMagnitude < mRangedExplosionRadius1 * mRangedExplosionRadius1)
				{
					Ray ray2 = new Ray(entityTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - entityTransform.position);
					float distance2 = Vector3.Distance(entityTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
					RaycastHit hitInfo2;
					if (Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
					{
						value.OnHit(mRangedExtraDamage1);
					}
				}
			}
			EndDead();
		}
		else if (entityTransform.position.y <= mFloorHeight)
		{
			entityTransform.position = new Vector3(entityTransform.position.x, mFloorHeight + 0.05f, entityTransform.position.z);
			mOnGround = true;
			GameApp.GetInstance().GetLootManager().OnLoot(this);
		}
		else if (mDeadTimer.Ready())
		{
			entityTransform.Translate(mDeadDropSpeed * Vector3.down * Time.deltaTime, Space.World);
			Vector3 vector = Vector3.RotateTowards(entityTransform.forward, mDeadDirection, 0.5f, 100f);
			entityTransform.LookAt(entityTransform.position + vector);
		}
	}

	public override void EndDead()
	{
		Deactivate();
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (!(GetIdleTimeDuration() > mIdleTime))
		{
			return;
		}
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		Vector3 vector = Vector3.zero;
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		if (horizontalSqrDistanceFromTarget > (float)(mRangedAttackToCatchRadius * mRangedAttackToCatchRadius))
		{
			EndEnemyIdle();
			StartCatching();
			enemyStateConst = EnemyStateConst.CATCHING;
		}
		else if (mFireCheckTimer.Ready())
		{
			mFireCheckTimer.Do();
			if (canHitTargetPlayer())
			{
				if (!mNeedCircuitousMove || GetIdleTimeDuration() > 2f * mIdleTime)
				{
					EndEnemyIdle();
					int num = Random.Range(0, 100);
					if (num < mRangedAttackProbability)
					{
						StartCypherFire();
						enemyStateConst = EnemyStateConst.CYPHER_FIRE;
					}
					else
					{
						StartCypherCannon();
						enemyStateConst = EnemyStateConst.CYPHER_CANNON;
					}
				}
				else
				{
					Vector3 vector2 = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
					vector2.Normalize();
					float num2 = Random.Range((float)mRangedStandAttackRadius * 0.25f, (float)mRangedStandAttackRadius * 0.75f);
					vector = entityTransform.position + vector2 * num2 + new Vector3(0f, 0.2f, 0f);
					float sqrMagnitude = (vector - mTargetPosition).sqrMagnitude;
					if (sqrMagnitude > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) / 4f && sqrMagnitude < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && TryPathForNavMesh(vector))
					{
						EndEnemyIdle();
						StartCypherMove(vector);
						enemyStateConst = EnemyStateConst.CYPHER_MOVE;
					}
				}
			}
			else
			{
				EndEnemyIdle();
				StartCatching();
				enemyStateConst = EnemyStateConst.CATCHING;
			}
		}
		else if (!mNeedCircuitousMove || GetIdleTimeDuration() > 2f * mIdleTime)
		{
			if (mNeverSeePlayer)
			{
				EndEnemyIdle();
				StartCatching();
				enemyStateConst = EnemyStateConst.CATCHING;
			}
			else
			{
				EndEnemyIdle();
				int num3 = Random.Range(0, 100);
				if (num3 < mRangedAttackProbability)
				{
					StartCypherFire();
					enemyStateConst = EnemyStateConst.CYPHER_FIRE;
				}
				else
				{
					StartCypherCannon();
					enemyStateConst = EnemyStateConst.CYPHER_CANNON;
				}
			}
		}
		else
		{
			Vector3 vector3 = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			vector3.Normalize();
			float num4 = Random.Range((float)mRangedStandAttackRadius * 0.25f, (float)mRangedStandAttackRadius * 0.75f);
			vector = entityTransform.position + vector3 * num4 + new Vector3(0f, 0.2f, 0f);
			float sqrMagnitude2 = (vector - mTargetPosition).sqrMagnitude;
			if (sqrMagnitude2 > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) * 0.25f && sqrMagnitude2 < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && TryPathForNavMesh(vector))
			{
				EndEnemyIdle();
				StartCypherMove(vector);
				enemyStateConst = EnemyStateConst.CYPHER_MOVE;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest enemyStateRequest = null;
			enemyStateRequest = ((enemyStateConst != EnemyStateConst.CYPHER_MOVE) ? new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position) : new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector));
			GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
		}
	}

	protected override Vector3 GetGoBackPosition()
	{
		return mNextPatrolLinePoint.transform.position;
	}

	protected override bool EndGoBackCondition()
	{
		return (mPatrolTarget - entityTransform.position).sqrMagnitude < 1f || GetGoBackTimeDuration() > mMaxGoBackTime;
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Cypher/cypher_attacked");
	}

	public override void StartPatrol(Vector3 patrolTarget)
	{
		base.StartPatrol(patrolTarget);
		StopSound("RPG_Audio/Enemy/Cypher/cypher_idle");
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

	public override void DoPatrolIdle()
	{
		PlayAnimation(AnimationString.CYPHER_MOVE_FORWARD, WrapMode.Loop);
		if (DetectPlayer() || !base.IsMasterPlayer || !(GetIdleTimeDuration() > mPatrolIdleTime))
		{
			return;
		}
		PatrolPointScript component = mNextPatrolLinePoint.GetComponent<PatrolPointScript>();
		if (null != component)
		{
			mNextPatrolLinePoint = component.NextPoint;
			StartPatrol(GetSkyPoint(mNextPatrolLinePoint.transform.position));
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartCypherMove(Vector3 target)
	{
		SetState(CYPHER_MOVE_STATE);
		mNeedCircuitousMove = false;
		SetCircuitousTarget(target);
		SetNavMeshForMove();
		SetMoveTimeNow();
		SetUpdatePosTimeNow();
		StopSound("RPG_Audio/Enemy/Cypher/cypher_idle");
	}

	public void DoCypherMove()
	{
		LookAtTargetHorizontal();
		PlayWalkSound();
		string moveAnimation = GetMoveAnimation();
		PlayAnimation(moveAnimation, WrapMode.Loop);
		float num = animation[moveAnimation].time / animation[moveAnimation].clip.length;
		num -= Mathf.Floor(num);
		if (mTarget == null)
		{
			EndCypherMove();
			StartEnemyIdle();
			return;
		}
		if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.CYPHER_MOVE, GetTransform().position, mCircuitousTarget, (short)(base.SpeedRate * 100f));
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		if ((mCircuitousTarget - entityTransform.position).sqrMagnitude < 1f)
		{
			EndCypherMove();
			if (base.IsMasterPlayer)
			{
				EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
				int num2 = Random.Range(0, 100);
				if (num2 < mRangedAttackProbability)
				{
					StartCypherFire();
					enemyStateConst = EnemyStateConst.CYPHER_FIRE;
				}
				else
				{
					StartCypherCannon();
					enemyStateConst = EnemyStateConst.CYPHER_CANNON;
				}
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
			else
			{
				StartEnemyIdle();
			}
		}
		else
		{
			if (!(GetMoveTimeDuration() > mRangedInterval1))
			{
				return;
			}
			EndCypherMove();
			if (base.IsMasterPlayer)
			{
				EnemyStateConst enemyStateConst2 = EnemyStateConst.NO_STATE;
				int num3 = Random.Range(0, 100);
				if (num3 < mRangedAttackProbability)
				{
					StartCypherFire();
					enemyStateConst2 = EnemyStateConst.CYPHER_FIRE;
				}
				else
				{
					StartCypherCannon();
					enemyStateConst2 = EnemyStateConst.CYPHER_CANNON;
				}
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst2, GetTransform().position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
			}
			else
			{
				StartEnemyIdle();
			}
		}
	}

	private void EndCypherMove()
	{
		StopNavMesh();
		StopSound("RPG_Audio/Enemy/Cypher/cypher_move");
	}

	public void StartCypherFire()
	{
		SetState(CYPHER_FIRE_STATE);
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public void DoCypherFire()
	{
		PlayAnimation(AnimationString.CYPHER_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(AnimationString.CYPHER_FIRE);
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndCypherFire();
			StartEnemyIdle();
		}
	}

	private void EndCypherFire()
	{
		mLeftFireSparkObject.SetActive(false);
		mRightFireSparkObject.SetActive(false);
	}

	public void StartCypherCannon()
	{
		SetState(CYPHER_CANNON_STATE);
		mCurrentCannonCount = 0;
		mIsShoot = false;
	}

	public void DoCypherCannon()
	{
		PlayAnimation(AnimationString.CYPHER_CANNON, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (!mIsShoot)
		{
			CannonShoot();
			mIsShoot = true;
		}
		if (mIsShoot && animation[AnimationString.CYPHER_CANNON].time > animation[AnimationString.CYPHER_CANNON].clip.length)
		{
			animation[AnimationString.CYPHER_CANNON].time -= animation[AnimationString.CYPHER_CANNON].clip.length;
			mCurrentCannonCount++;
			mIsShoot = false;
		}
		if (mCurrentCannonCount >= mRangedBulletCount2)
		{
			EndCypherCannon();
			StartEnemyIdle();
		}
	}

	private void EndCypherCannon()
	{
	}
}
