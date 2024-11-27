using System.Collections.Generic;
using UnityEngine;

public class Cybershoot : Enemy
{
	public static EnemyState CYBERSHOOT_JUMP_STATE = new CybershootJumpState();

	public static EnemyState CYBERSHOOT_DODGE_STATE = new CybershootDodgeState();

	public static EnemyState CYBERSHOOT_FIRE_STATE = new CybershootFireState();

	public static EnemyState CYBERSHOOT_CANNON_STATE = new CybershootCannonState();

	private int mCurrentBulletCount;

	private int mCurrentCannonCount;

	private Transform mFireSparkTransform;

	private Transform mLeftCannonTransform;

	private Transform mRightCannonTransform;

	private GameObject mFireSparkObject;

	private Vector3 mDodgeTarget;

	private float mFireAnimationLength;

	private float mCannonAnimationLength;

	private Timer mCanHitTargetTimer = new Timer();

	private Timer mFireCheckTimer = new Timer();

	protected Timer mTryJumpTimer = new Timer();

	private bool mNeverSeePlayer;

	private bool mLandEffectPlayed;

	private bool mDeadEffectPlayed;

	public override bool CanPatrol()
	{
		return true;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.CybershootMesh;
	}

	public override void Init()
	{
		base.Init();
		mShieldType = ShieldType.MECHANICAL;
		mFireAnimationLength = 5f;
		mCannonAnimationLength = 15f;
		mCanHitTargetTimer.SetTimer(1f, false);
		mFireCheckTimer.SetTimer(10f, false);
		mTryJumpTimer.SetTimer(2f, false);
		mWalkAudioTimer.SetTimer(0.5f, false);
		mRunAudioTimer.SetTimer(0.5f, false);
	}

	public override void Activate()
	{
		base.Activate();
		animation[AnimationString.CYPHER_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		animation[AnimationString.CYPHER_CANNON].speed = GetFireSpeedCoefficent() * mCannonAnimationLength / 30f / mRangedOneShotTime2;
		GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Fire_001") as GameObject;
		mFireSparkObject = Object.Instantiate(original) as GameObject;
		mFireSparkObject.name = "GunSpark";
		mFireSparkObject.SetActive(false);
		mFireSparkTransform = mFireSparkObject.transform;
		mFireSparkTransform.parent = entityTransform.Find(BoneName.CybershootGun);
		mFireSparkTransform.localPosition = Vector3.zero;
		mFireSparkTransform.localRotation = Quaternion.identity;
		mFireSparkTransform.localScale = Vector3.one;
		mLeftCannonTransform = entityTransform.Find(BoneName.CybershootLeftCannon);
		mRightCannonTransform = entityTransform.Find(BoneName.CybershootRightCannon);
		if (SpawnType == ESpawnType.STREAMING)
		{
			mNeverSeePlayer = false;
		}
		else
		{
			mNeverSeePlayer = true;
		}
		mLandEffectPlayed = false;
		mDeadEffectPlayed = false;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.CybershootHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
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

	private void Shoot(string shootAnimation)
	{
		if (!mIsShoot)
		{
			CheckShoot();
			mIsShoot = true;
			mFireSparkObject.SetActive(true);
		}
		if (mIsShoot && animation[shootAnimation].time > animation[shootAnimation].clip.length)
		{
			animation[shootAnimation].time -= animation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
			mFireSparkObject.SetActive(false);
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

	public void CannonShoot()
	{
		Transform transform = ((Random.Range(0, 100) >= 50) ? mRightCannonTransform : mLeftCannonTransform);
		Vector3 forward = mTargetPosition - transform.position;
		GameObject original = Resources.Load("RPG_effect/RPG_shell_rocket_001") as GameObject;
		GameObject gameObject = Object.Instantiate(original, transform.position, Quaternion.identity) as GameObject;
		ShellMissileScript component = gameObject.GetComponent<ShellMissileScript>();
		if (null != component)
		{
			Vector3 normalized = (entityTransform.forward + 0.4f * Vector3.up).normalized;
			component.mEnemy = this;
			component.mRisingSpeed = 10f * normalized;
			component.mRisingTime = 0.2f;
			component.mTarget = mTarget;
			component.mTargetPosition = mTargetPosition;
			component.mExplosionDamage = mRangedExtraDamage2;
			component.mAttackSpeedValue = mRangedBulletSpeed2;
			component.mExplosionRadius = mRangedExplosionRadius2;
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_Cypher_Fire_002") as GameObject;
		Object.Instantiate(original2, transform.position, Quaternion.LookRotation(forward));
		PlaySound("RPG_Audio/Enemy/Shell/Shell_fire");
	}

	public void SetNavMeshForDodge()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mDodgeTarget);
			mNavMeshAgent.speed = 10f;
			SetCanTurn(true);
		}
	}

	private void SetDodgeTarget(Vector3 target)
	{
		mDodgeTarget = target;
	}

	protected override string GetRunAnimationName()
	{
		return AnimationString.ENEMY_WALK;
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (CYBERSHOOT_JUMP_STATE == mState)
		{
			EndCybershootJump();
		}
		else if (CYBERSHOOT_DODGE_STATE == mState)
		{
			EndCybershootDodge();
		}
		else if (CYBERSHOOT_FIRE_STATE == mState)
		{
			EndCybershootFire();
		}
		else if (CYBERSHOOT_CANNON_STATE == mState)
		{
			EndCybershootCannon();
		}
	}

	protected override void PlayAwakeSound()
	{
	}

	protected override void PlayRunningSound()
	{
		if (mRunAudioTimer.Ready())
		{
			mRunAudioTimer.Do();
			PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_walk");
		}
	}

	protected override void MakeDecisionInCatching()
	{
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		if (horizontalSqrDistanceFromTarget < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
		{
			if (mCanHitTargetTimer.Ready())
			{
				mCanHitTargetTimer.Do();
				if (canHitTargetPlayer())
				{
					EndCatching();
					mNeverSeePlayer = false;
					int num = Random.Range(0, 100);
					if (num < mRangedAttackProbability)
					{
						StartCybershootFire();
						enemyStateConst = EnemyStateConst.CYBERSHOOT_FIRE;
					}
					else
					{
						StartCybershootCannon();
						enemyStateConst = EnemyStateConst.CYBERSHOOT_CANNON;
					}
				}
			}
		}
		else if (horizontalSqrDistanceFromTarget < 676f && horizontalSqrDistanceFromTarget > 625f && mTryJumpTimer.Ready())
		{
			mTryJumpTimer.Do();
			int num2 = Random.Range(0, 100);
			if (num2 < 50 && canHitTargetPlayer())
			{
				EndCatching();
				StartCybershootJump();
				enemyStateConst = EnemyStateConst.CYBERSHOOT_JUMP;
				mNeverSeePlayer = false;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		mDeadEffectPlayed = false;
		GameObject original = Resources.Load("RPG_effect/RPG_TMX_Die_001") as GameObject;
		GameObject gameObject = Object.Instantiate(original, mBodyTransform.position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = mBodyTransform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
	}

	public override void DoDead()
	{
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		float num = animation[AnimationString.ENEMY_DEAD].time / animation[AnimationString.ENEMY_DEAD].clip.length;
		if (mOnGround)
		{
			if (!mDeadEffectPlayed && num > 0.9f)
			{
				mDeadEffectPlayed = true;
				GameObject original = Resources.Load("RPG_effect/RPG_TMX_Die_002") as GameObject;
				Object.Instantiate(original, mBodyTransform.position, Quaternion.identity);
				PlaySound("Audio/rpg/rpg-21_boom");
				if ((mLocalPlayer.GetTransform().position - mBodyTransform.position).sqrMagnitude < mRangedExplosionRadius1 * mRangedExplosionRadius1)
				{
					Ray ray = new Ray(mBodyTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - mBodyTransform.position);
					float distance = Vector3.Distance(mBodyTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER)
					{
						mLocalPlayer.OnHit(mRangedExtraDamage1, this);
					}
				}
				Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
				foreach (SummonedItem value in summonedList.Values)
				{
					if ((value.GetTransform().position - mBodyTransform.position).sqrMagnitude < mRangedExplosionRadius1 * mRangedExplosionRadius1)
					{
						Ray ray2 = new Ray(mBodyTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - mBodyTransform.position);
						float distance2 = Vector3.Distance(mBodyTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
						RaycastHit hitInfo2;
						if (Physics.Raycast(ray2, out hitInfo2, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && hitInfo2.collider.gameObject.layer == PhysicsLayer.SUMMONED)
						{
							value.OnHit(mRangedExtraDamage1);
						}
					}
				}
			}
			if (num > 1f)
			{
				EndDead();
			}
		}
		else if (entityTransform.position.y <= mFloorHeight)
		{
			entityTransform.position = new Vector3(entityTransform.position.x, mFloorHeight + 0.05f, entityTransform.position.z);
			mOnGround = true;
			Vector3 ground = GetGround();
			entityTransform.rotation = Quaternion.FromToRotation(entityTransform.up, ground) * entityTransform.rotation;
			GameApp.GetInstance().GetLootManager().OnLoot(this);
		}
		else
		{
			entityTransform.Translate(10f * Vector3.down * Time.deltaTime, Space.World);
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
		else if (horizontalSqrDistanceFromTarget < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && (float)Random.Range(0, 100) < 50f && !mNeverSeePlayer)
		{
			Vector3 vector2 = -entityTransform.forward;
			float num = 10f;
			vector = entityTransform.position + vector2 * num + new Vector3(0f, 0.2f, 0f);
			if (TryPathForNavMesh(vector))
			{
				EndEnemyIdle();
				StartCybershootDodge(vector);
				enemyStateConst = EnemyStateConst.CYBERSHOOT_DODGE;
			}
		}
		else if (mFireCheckTimer.Ready())
		{
			mFireCheckTimer.Do();
			if (canHitTargetPlayer())
			{
				EndEnemyIdle();
				int num2 = Random.Range(0, 100);
				if (num2 < mRangedAttackProbability)
				{
					StartCybershootFire();
					enemyStateConst = EnemyStateConst.CYBERSHOOT_FIRE;
				}
				else
				{
					StartCybershootCannon();
					enemyStateConst = EnemyStateConst.CYBERSHOOT_CANNON;
				}
			}
			else
			{
				EndEnemyIdle();
				StartCatching();
				enemyStateConst = EnemyStateConst.CATCHING;
			}
		}
		else if (mNeverSeePlayer)
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
				StartCybershootFire();
				enemyStateConst = EnemyStateConst.CYBERSHOOT_FIRE;
			}
			else
			{
				StartCybershootCannon();
				enemyStateConst = EnemyStateConst.CYBERSHOOT_CANNON;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest enemyStateRequest = null;
			enemyStateRequest = ((enemyStateConst != EnemyStateConst.CYBERSHOOT_DODGE) ? new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position) : new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector));
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
		PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_attacked");
	}

	protected override void PlayWalkSound()
	{
		if (mWalkAudioTimer.Ready())
		{
			mWalkAudioTimer.Do();
			PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_walk");
		}
	}

	public override void DoPatrolIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (DetectPlayer() || !base.IsMasterPlayer || !(GetIdleTimeDuration() > mPatrolIdleTime))
		{
			return;
		}
		PatrolPointScript component = mNextPatrolLinePoint.GetComponent<PatrolPointScript>();
		if (null != component)
		{
			mNextPatrolLinePoint = component.NextPoint;
			StartPatrol(mNextPatrolLinePoint.transform.position);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartCybershootJump()
	{
		SetState(CYBERSHOOT_JUMP_STATE);
		Vector3 vector = mTargetPosition;
		Vector3 normalized = (vector - entityTransform.position).normalized;
		vector -= 5f * normalized;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(vector);
			mNavMeshAgent.speed = 16f;
			SetCanTurn(false);
		}
		mLandEffectPlayed = false;
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_jump");
	}

	public void DoCybershootJump()
	{
		PlayAnimation(AnimationString.ENEMY_JUMP, WrapMode.ClampForever);
		float num = animation[AnimationString.ENEMY_JUMP].time / animation[AnimationString.ENEMY_JUMP].clip.length;
		if (num >= 0.85f && null != mNavMeshAgent && !mLandEffectPlayed)
		{
			mNavMeshAgent.speed = 1f;
			mLandEffectPlayed = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		}
		if (num > 1f)
		{
			EndCybershootJump();
			StartEnemyIdle();
		}
	}

	private void EndCybershootJump()
	{
		StopNavMesh();
	}

	public void StartCybershootDodge(Vector3 target)
	{
		SetState(CYBERSHOOT_DODGE_STATE);
		mLandEffectPlayed = false;
		SetDodgeTarget(target);
		SetNavMeshForDodge();
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Cybershoot/Cybershoot_jump");
	}

	public void DoCybershootDodge()
	{
		PlayAnimation(AnimationString.CYBERSHOOT_DODGE, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		float num = animation[AnimationString.CYBERSHOOT_DODGE].time / animation[AnimationString.CYBERSHOOT_DODGE].clip.length;
		if (num >= 0.85f && null != mNavMeshAgent && !mLandEffectPlayed)
		{
			mNavMeshAgent.speed = 1f;
			mLandEffectPlayed = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		}
		if (num >= 1f)
		{
			EndCybershootDodge();
			StartEnemyIdleWithoutResetTime();
		}
	}

	private void EndCybershootDodge()
	{
		StopNavMesh();
	}

	public void StartCybershootFire()
	{
		SetState(CYBERSHOOT_FIRE_STATE);
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public void DoCybershootFire()
	{
		PlayAnimation(AnimationString.CYBERSHOOT_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(AnimationString.CYBERSHOOT_FIRE);
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndCybershootFire();
			StartEnemyIdle();
		}
	}

	private void EndCybershootFire()
	{
		mFireSparkObject.SetActive(false);
	}

	public void StartCybershootCannon()
	{
		SetState(CYBERSHOOT_CANNON_STATE);
		mCurrentCannonCount = 0;
		mIsShoot = false;
	}

	public void DoCybershootCannon()
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
			EndCybershootCannon();
			StartEnemyIdle();
		}
	}

	private void EndCybershootCannon()
	{
	}

	protected override bool IsJumping()
	{
		return mState == CYBERSHOOT_JUMP_STATE;
	}
}
