using UnityEngine;

public abstract class EnemyHuman : Enemy
{
	public static EnemyState HUMAN_CIRCUITOUS_MOVE = new HumanCircuitousMoveState();

	public static EnemyState HUMAN_CROUCH_IDLE = new HumanCrouchIdleState();

	public static EnemyState HUMAN_CROUCH_RELOAD = new HumanCrouchReloadState();

	public static EnemyState HUMAN_CROUCH = new HumanCrouchState();

	public static EnemyState HUMAN_FIRE_COVER_EXPOSE_END = new HumanFireCoverExposeEndState();

	public static EnemyState HUMAN_FIRE_COVER_EXPOSE_START = new HumanFireCoverExposeStartState();

	public static EnemyState HUMAN_FIRE_COVER_EXPOSE = new HumanFireCoverExposeState();

	public static EnemyState HUMAN_FULL_COVER_FIRE = new HumanFullCoverFireState();

	public static EnemyState HUMAN_FULL_COVER_IDLE = new HumanFullCoverIdleState();

	public static EnemyState HUMAN_FULL_COVER_STANDUP = new HumanFullCoverStandupState();

	public static EnemyState HUMAN_IDLE_COVER_HIDE = new HumanIdleCoverHideState();

	public static EnemyState HUMAN_LEAN_CROUCH_BACK = new HumanLeanCrouchBackState();

	public static EnemyState HUMAN_LEAN_CROUCH_FIRE = new HumanLeanCrouchFireState();

	public static EnemyState HUMAN_LEAN_CROUCH = new HumanLeanCrouchState();

	public static EnemyState HUMAN_MOVE_COVER_EXPOSE = new HumanMoveCoverExposeState();

	public static EnemyState HUMAN_MOVE_COVER_HIDE = new HumanMoveCoverHideState();

	public static EnemyState HUMAN_MOVE = new HumanMoveState();

	public static EnemyState HUMAN_RELOAD = new HumanReloadState();

	public static EnemyState HUMAN_RUN_TO_COVER = new HumanRunToCoverState();

	public static EnemyState HUMAN_STAND_FIRE = new HumanStandFireState();

	public static EnemyState HUMAN_STANDUP = new HumanStandupState();

	protected float mLastMoveTime;

	protected float mLastCoverTime;

	protected float mLastIdleCoverMoveTime;

	protected int mCurrentBulletCount;

	protected int mCurrentCoverAttackCount;

	protected Timer mCoverCheckTimer = new Timer();

	protected Timer mStandFireCheckTimer = new Timer();

	protected CoverLinkScript mCurrentCoverScript;

	protected Transform mWeaponFireTransform;

	protected Vector3 mIdleCoverExposeMoveDirection;

	protected Vector3 mCircuitousTarget;

	protected bool mNeedCircuitousMove;

	protected float mCoverAngle;

	protected float mCrouchHideHeight;

	protected float mCrouchFireHeight;

	protected float mStandFireHeight;

	protected Vector3 mLeanRightPosition;

	protected Vector3 mLeanLeftPosition;

	protected float mFireAnimationLength;

	protected string mWeaponPrefabPath;

	protected bool mNeverSeePlayer;

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

	public float GetCoverTimeDuration()
	{
		return Time.time - mLastCoverTime;
	}

	public void SetCoverTimeNow()
	{
		mLastCoverTime = Time.time;
	}

	public float GetIdleCoverMoveTimeDuration()
	{
		return Time.time - mLastIdleCoverMoveTime;
	}

	public void SetIdleCoverMoveTimeNow()
	{
		mLastIdleCoverMoveTime = Time.time;
	}

	public override void Init()
	{
		base.Init();
		mCoverAngle = 60f;
		mCrouchHideHeight = 1f;
		mCrouchFireHeight = 1.25f;
		mStandFireHeight = 1.75f;
		mLastMoveTime = Time.time;
		mLastCoverTime = Time.time;
		mLastIdleCoverMoveTime = Time.time;
		mCoverCheckTimer.SetTimer(0.2f, false);
		mStandFireCheckTimer.SetTimer(10f, false);
	}

	public override void Activate()
	{
		base.Activate();
		animation[AnimationString.HUMAN_IDLE_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		animation[AnimationString.HUMAN_LEAN_CROUCH_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		animation[AnimationString.HUMAN_LEAN_LEFT_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		animation[AnimationString.HUMAN_LEAN_RIGHT_FIRE].speed = GetFireSpeedCoefficent() * mFireAnimationLength / 30f / mRangedOneShotTime1;
		Transform parent = entityObject.transform.Find(BoneName.EnemyHumanHandPoint);
		GameObject original = Resources.Load(mWeaponPrefabPath) as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.name = "Gun";
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mWeaponFireTransform = entityObject.transform.Find(BoneName.EnemyHumanWeaponFire);
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

	protected virtual void PlayShootingSound()
	{
	}

	protected virtual void Shoot(string shootAnimation)
	{
		if (!mIsShoot)
		{
			CheckShoot();
			mIsShoot = true;
		}
		if (mIsShoot && animation[shootAnimation].time > animation[shootAnimation].clip.length)
		{
			animation[shootAnimation].time -= animation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
		}
	}

	protected void CheckShoot()
	{
		PlayShootingSound();
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
		Vector3 vector2 = new Vector3(entityTransform.position.x, mWeaponFireTransform.position.y, entityTransform.position.z);
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
		CreateGunFire(direction);
		CreateBulletTrail(hitCollider, hitLocalPlayer, direction);
	}

	protected virtual void CreateWallSpark()
	{
	}

	protected virtual void CreateFloorSpark()
	{
	}

	protected virtual void CreateGunFire(Vector3 direction)
	{
	}

	protected virtual void CreateBulletTrail(bool hitCollider, bool hitLocalPlayer, Vector3 direction)
	{
		if (!hitLocalPlayer && (mRaycastHit.point - mWeaponFireTransform.position).sqrMagnitude < 25f)
		{
			return;
		}
		GameObject gameObject = gameScene.GetEffectPool(EffectPoolType.BULLET_TRAIL).CreateObject(mWeaponFireTransform.position + 2.5f * direction, direction, Quaternion.identity);
		if (gameObject == null)
		{
			Debug.Log("fire line obj null");
			return;
		}
		BulletTrailScript component = gameObject.GetComponent<BulletTrailScript>();
		component.beginPos = mWeaponFireTransform.position + 2.5f * direction;
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
			component.endPos = mWeaponFireTransform.position + direction * 100f;
		}
		component.speed = 100f;
		component.isActive = true;
	}

	protected void SetCircuitousTarget(Vector3 target)
	{
		mCircuitousTarget = target;
	}

	protected override bool TryFindCover()
	{
		if (!base.IsMasterPlayer)
		{
			return false;
		}
		if (mCurrentCoverScript != null)
		{
			return false;
		}
		bool flag = false;
		if (GetCoverTimeDuration() > mCoverInterval)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.COVER);
			int num = -1;
			CoverLinkScript coverLinkScript = null;
			float num2 = mCoverSearchRadius * mCoverSearchRadius;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(Mathf.Abs(entityTransform.position.y - array[i].transform.position.y) < 1f))
				{
					continue;
				}
				float sqrMagnitude = (array[i].transform.position - entityTransform.position).sqrMagnitude;
				if (!(sqrMagnitude < num2))
				{
					continue;
				}
				CoverLinkScript component = array[i].GetComponent<CoverLinkScript>();
				if (!(null != component) || component.Occupied || !(null != component.Expose) || !(null != component.Hide))
				{
					continue;
				}
				if (component.Type == CoverType.STAND)
				{
					Vector3 direction = mTargetPosition - component.Expose.transform.position;
					if (!(direction.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius)))
					{
						continue;
					}
					float num3 = Vector3.Angle(to: new Vector3(direction.x, 0f, direction.z), from: component.Expose.transform.forward);
					if (num3 < 90f)
					{
						bool flag2 = false;
						mRay = new Ray(component.Expose.transform.position + mStandFireHeight * Vector3.up, direction);
						int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
						if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
						{
							flag2 = true;
						}
						bool flag3 = true;
						direction = mTargetPosition - component.Hide.transform.position;
						mRay = new Ray(component.Hide.transform.position + mStandFireHeight * Vector3.up, direction);
						layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
						if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
						{
							flag3 = false;
						}
						if (flag2 && flag3)
						{
							num2 = sqrMagnitude;
							num = i;
							coverLinkScript = component;
							flag = true;
						}
					}
					continue;
				}
				float num4 = Random.Range(0f, 1f);
				component.CrouchPosition = component.Hide.transform.position * num4 + component.Expose.transform.position * (1f - num4);
				Vector3 direction2 = mTargetPosition - component.CrouchPosition;
				if (!(direction2.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius)))
				{
					continue;
				}
				float num5 = Vector3.Angle(to: new Vector3(direction2.x, 0f, direction2.z), from: component.Expose.transform.forward);
				if (num5 < 90f)
				{
					bool flag4 = false;
					mRay = new Ray(component.CrouchPosition + mStandFireHeight * Vector3.up, direction2);
					int layerMask2 = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
					if (Physics.Raycast(mRay, out mRaycastHit, direction2.magnitude, layerMask2) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
					{
						flag4 = true;
					}
					bool flag5 = true;
					mRay = new Ray(component.CrouchPosition + mCrouchHideHeight * Vector3.up, direction2);
					layerMask2 = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
					if (Physics.Raycast(mRay, out mRaycastHit, direction2.magnitude, layerMask2) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
					{
						flag5 = false;
					}
					if (flag4 && flag5)
					{
						num2 = sqrMagnitude;
						num = i;
						coverLinkScript = component;
						flag = true;
					}
				}
			}
			if (flag)
			{
				EndCurrentState();
				StartHumanRunToCover(num, coverLinkScript.CrouchPosition);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_RUN_TO_COVER, entityTransform.position, coverLinkScript.CrouchPosition, (byte)num);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		return flag;
	}

	protected string GetMoveAnimation(Vector3 moveDirection)
	{
		string result = AnimationString.HUMAN_MOVE_FORWARD;
		float num = Vector3.Angle(moveDirection, entityTransform.forward);
		if (num > 135f)
		{
			result = AnimationString.HUMAN_MOVE_BACK;
		}
		else if (num > 45f)
		{
			result = ((!(Vector3.Cross(moveDirection, entityTransform.forward).y > 0f)) ? AnimationString.HUMAN_MOVE_RIGHT : AnimationString.HUMAN_MOVE_LEFT);
		}
		return result;
	}

	protected string GetMoveAnimation()
	{
		string result = AnimationString.HUMAN_MOVE_FORWARD;
		if (null != mNavMeshAgent)
		{
			result = GetMoveAnimation(mNavMeshAgent.velocity);
		}
		return result;
	}

	public virtual void SetNavMeshForMoveToTarget()
	{
		Vector3 destination = mTargetPosition;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(destination);
			mNavMeshAgent.speed = mWalkSpeed;
			SetCanTurn(true);
		}
	}

	public virtual void SetNavMeshForCircuitousMove()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mCircuitousTarget);
			mNavMeshAgent.speed = mWalkSpeed;
			SetCanTurn(true);
		}
	}

	public virtual void SetNavMeshForMoveToCoverExpose()
	{
		if (null != mNavMeshAgent && null != mCurrentCoverScript)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mCurrentCoverScript.Expose.transform.position);
			mNavMeshAgent.speed = 1f;
			SetCanTurn(true);
		}
	}

	public virtual void SetNavMeshForMoveToCoverHide()
	{
		if (null != mNavMeshAgent && null != mCurrentCoverScript)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mCurrentCoverScript.Hide.transform.position);
			mNavMeshAgent.speed = 1f;
			SetCanTurn(true);
		}
	}

	public virtual void SetNavMeshForRunToCover()
	{
		if (null != mNavMeshAgent && null != mCurrentCoverScript)
		{
			mNavMeshAgent.Resume();
			if (mCurrentCoverScript.Type == CoverType.STAND)
			{
				mNavMeshAgent.SetDestination(mCurrentCoverScript.Hide.transform.position);
			}
			else
			{
				mNavMeshAgent.SetDestination(mCurrentCoverScript.CrouchPosition);
			}
			mNavMeshAgent.speed = mRunSpeed;
			SetCanTurn(false);
		}
	}

	protected void SetCurrentCoverScript(int coverId)
	{
		LeaveCover();
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.COVER);
		CoverLinkScript coverLinkScript = (mCurrentCoverScript = array[coverId].GetComponent<CoverLinkScript>());
		coverLinkScript.Occupied = true;
		coverLinkScript.Id = coverId;
	}

	protected void SetCurrentCoverScript(int coverId, Vector3 position)
	{
		LeaveCover();
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.COVER);
		CoverLinkScript coverLinkScript = (mCurrentCoverScript = array[coverId].GetComponent<CoverLinkScript>());
		coverLinkScript.Occupied = true;
		coverLinkScript.Id = coverId;
		if (coverLinkScript.Type == CoverType.CROUCH)
		{
			coverLinkScript.CrouchPosition = position;
		}
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mState == Enemy.IDLE_STATE || mState == HUMAN_STAND_FIRE || mState == Enemy.GOTHIT_STATE)
		{
			mNeedCircuitousMove = true;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mState == Enemy.IDLE_STATE || mState == HUMAN_STAND_FIRE || mState == Enemy.GOTHIT_STATE)
		{
			mNeedCircuitousMove = true;
		}
	}

	protected void LeaveCover()
	{
		if (null != mCurrentCoverScript)
		{
			mCurrentCoverScript.Occupied = false;
			mCurrentCoverScript = null;
			SetCoverTimeNow();
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (HUMAN_CIRCUITOUS_MOVE == mState)
		{
			EndHumanCircuitousMove();
		}
		else if (HUMAN_CROUCH_IDLE == mState)
		{
			EndHumanCrouchIdle();
		}
		else if (HUMAN_CROUCH_RELOAD == mState)
		{
			EndHumanCrouchReload();
		}
		else if (HUMAN_CROUCH == mState)
		{
			EndHumanCrouch();
		}
		else if (HUMAN_FIRE_COVER_EXPOSE_END == mState)
		{
			EndHumanFireCoverExposeEnd();
		}
		else if (HUMAN_FIRE_COVER_EXPOSE_START == mState)
		{
			EndHumanFireCoverExposeStart();
		}
		else if (HUMAN_FIRE_COVER_EXPOSE == mState)
		{
			EndHumanFireCoverExpose();
		}
		else if (HUMAN_FULL_COVER_FIRE == mState)
		{
			EndHumanFullCoverFire();
		}
		else if (HUMAN_FULL_COVER_IDLE == mState)
		{
			EndHumanFullCoverIdle();
		}
		else if (HUMAN_FULL_COVER_STANDUP == mState)
		{
			EndHumanFullCoverStandup();
		}
		else if (HUMAN_IDLE_COVER_HIDE == mState)
		{
			EndHumanIdleCoverHide();
		}
		else if (HUMAN_LEAN_CROUCH_BACK == mState)
		{
			EndHumanLeanCrouchBack();
		}
		else if (HUMAN_LEAN_CROUCH_FIRE == mState)
		{
			EndHumanLeanCrouchFire();
		}
		else if (HUMAN_LEAN_CROUCH == mState)
		{
			EndHumanLeanCrouch();
		}
		else if (HUMAN_MOVE_COVER_EXPOSE == mState)
		{
			EndHumanMoveCoverExpose();
		}
		else if (HUMAN_MOVE_COVER_HIDE == mState)
		{
			EndHumanMoveCoverHide();
		}
		else if (HUMAN_MOVE == mState)
		{
			EndHumanMove();
		}
		else if (HUMAN_RELOAD == mState)
		{
			EndHumanReload();
		}
		else if (HUMAN_RUN_TO_COVER == mState)
		{
			EndHumanRunToCover();
		}
		else if (HUMAN_STAND_FIRE == mState)
		{
			EndHumanStandFire();
		}
		else if (HUMAN_STANDUP == mState)
		{
			EndHumanStandup();
		}
	}

	protected override void PlayAwakeSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/Mob_scream");
	}

	protected override void MakeDecisionInCatching()
	{
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		if (horizontalSqrDistanceFromTarget < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
		{
			EndCatching();
			StartHumanMove();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_MOVE, GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Mob/Mob_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		if (null != mCurrentCoverScript)
		{
			mCurrentCoverScript.Occupied = false;
		}
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
		else if (horizontalSqrDistanceFromTarget > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius))
		{
			EndEnemyIdle();
			StartHumanMove();
			enemyStateConst = EnemyStateConst.HUMAN_MOVE;
		}
		else if (mStandFireCheckTimer.Ready())
		{
			mStandFireCheckTimer.Do();
			if (canHitTargetPlayer())
			{
				if (!mNeedCircuitousMove || GetIdleTimeDuration() > 2f * mIdleTime)
				{
					EndEnemyIdle();
					StartHumanStandFire();
					enemyStateConst = EnemyStateConst.HUMAN_STAND_FIRE;
				}
				else
				{
					Vector3 vector2 = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
					vector2.Normalize();
					float num = Random.Range((float)mRangedStandAttackRadius * 0.25f, (float)mRangedStandAttackRadius * 0.75f);
					vector = entityTransform.position + vector2 * num + new Vector3(0f, 0.2f, 0f);
					float sqrMagnitude = (vector - mTargetPosition).sqrMagnitude;
					if (sqrMagnitude > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) / 4f && sqrMagnitude < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && TryPathForNavMesh(vector))
					{
						EndEnemyIdle();
						StartHumanCircuitousMove(vector);
						enemyStateConst = EnemyStateConst.HUMAN_CIRCUITOUS_MOVE;
					}
				}
			}
			else
			{
				EndEnemyIdle();
				StartHumanMove();
				enemyStateConst = EnemyStateConst.HUMAN_MOVE;
			}
		}
		else if (!mNeedCircuitousMove || GetIdleTimeDuration() > 2f * mIdleTime)
		{
			if (mNeverSeePlayer)
			{
				EndEnemyIdle();
				StartHumanMove();
				enemyStateConst = EnemyStateConst.HUMAN_MOVE;
			}
			else
			{
				EndEnemyIdle();
				StartHumanStandFire();
				enemyStateConst = EnemyStateConst.HUMAN_STAND_FIRE;
			}
		}
		else
		{
			Vector3 vector3 = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			vector3.Normalize();
			float num2 = Random.Range((float)mRangedStandAttackRadius * 0.25f, (float)mRangedStandAttackRadius * 0.75f);
			vector = entityTransform.position + vector3 * num2 + new Vector3(0f, 0.2f, 0f);
			float sqrMagnitude2 = (vector - mTargetPosition).sqrMagnitude;
			if (sqrMagnitude2 > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) * 0.25f && sqrMagnitude2 < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && TryPathForNavMesh(vector))
			{
				EndEnemyIdle();
				StartHumanCircuitousMove(vector);
				enemyStateConst = EnemyStateConst.HUMAN_CIRCUITOUS_MOVE;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest enemyStateRequest = null;
			enemyStateRequest = ((enemyStateConst != EnemyStateConst.HUMAN_CIRCUITOUS_MOVE) ? new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position) : new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector));
			GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
		}
	}

	public override void StartEnemyIdle()
	{
		base.StartEnemyIdle();
		LeaveCover();
	}

	protected override void StartEnemyIdleWithoutResetTime()
	{
		base.StartEnemyIdleWithoutResetTime();
		LeaveCover();
	}

	protected override Vector3 GetGoBackPosition()
	{
		return mNextPatrolLinePoint.transform.position;
	}

	protected override bool EndGoBackCondition()
	{
		return (mPatrolTarget - entityTransform.position).sqrMagnitude < 1f || GetGoBackTimeDuration() > mMaxGoBackTime;
	}

	public override void DoGotHit()
	{
		PlayAnimation(AnimationString.ENEMY_GOTHIT, WrapMode.ClampForever);
		if (!AnimationPlayed(AnimationString.ENEMY_GOTHIT, 1f))
		{
			return;
		}
		if (base.IsMasterPlayer)
		{
			if (mCurrentCoverScript != null)
			{
				EndGotHit();
				Vector3 to = mTargetPosition - entityTransform.position;
				to.y = 0f;
				float num = Vector3.Angle(mCurrentCoverScript.Expose.transform.forward, to);
				if (num < 90f)
				{
					EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
					if (mCurrentCoverScript.Type == CoverType.STAND)
					{
						StartHumanMoveCoverExpose(mCurrentCoverScript.Id);
						enemyStateConst = EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE;
					}
					else
					{
						Vector3 direction = mTargetPosition - entityTransform.position;
						mRay = new Ray(entityTransform.position + mCrouchFireHeight * Vector3.up, direction);
						int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
						if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask))
						{
							if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
							{
								StartHumanLeanCrouchFire(mCurrentCoverScript.Id);
								enemyStateConst = EnemyStateConst.HUMAN_LEAN_CROUCH_FIRE;
							}
							else
							{
								StartHumanFullCoverFire(mCurrentCoverScript.Id);
								enemyStateConst = EnemyStateConst.HUMAN_FULL_COVER_FIRE;
							}
						}
						else
						{
							StartHumanFullCoverFire(mCurrentCoverScript.Id);
							enemyStateConst = EnemyStateConst.HUMAN_FULL_COVER_FIRE;
						}
					}
					if (enemyStateConst != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mCurrentCoverScript.Id);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
				else
				{
					StartEnemyIdleWithoutResetTime();
				}
			}
			else if (!TryFindCover())
			{
				EndGotHit();
				StartEnemyIdleWithoutResetTime();
			}
		}
		else
		{
			EndGotHit();
			StartEnemyIdleWithoutResetTime();
		}
	}

	public override void DoPatrolIdle()
	{
		PlayAnimation(AnimationString.ENEMY_PATROL_IDLE, WrapMode.Loop);
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
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, mNextPatrolLinePoint.transform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartHumanCircuitousMove(Vector3 target)
	{
		SetState(HUMAN_CIRCUITOUS_MOVE);
		mNeedCircuitousMove = false;
		SetCircuitousTarget(target);
		SetNavMeshForCircuitousMove();
		SetMoveTimeNow();
		SetUpdatePosTimeNow();
	}

	public void DoHumanCircuitousMove()
	{
		LookAtTargetHorizontal();
		string moveAnimation = GetMoveAnimation();
		PlayAnimation(moveAnimation, WrapMode.Loop);
		float num = animation[moveAnimation].time / animation[moveAnimation].clip.length;
		num -= Mathf.Floor(num);
		if (num > 0.9f)
		{
			if (mTarget == null)
			{
				EndHumanCircuitousMove();
				StartEnemyIdle();
			}
			else if ((mCircuitousTarget - entityTransform.position).sqrMagnitude < 1f)
			{
				EndHumanCircuitousMove();
				if (base.IsMasterPlayer)
				{
					StartHumanStandFire();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_STAND_FIRE, GetTransform().position);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
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
				EndHumanCircuitousMove();
				if (base.IsMasterPlayer)
				{
					StartHumanStandFire();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_STAND_FIRE, GetTransform().position);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
				else
				{
					StartEnemyIdle();
				}
			}
		}
		else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_CIRCUITOUS_MOVE, GetTransform().position, mCircuitousTarget, (short)(base.SpeedRate * 100f));
			GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
		}
	}

	protected void EndHumanCircuitousMove()
	{
		StopNavMesh();
	}

	protected virtual void StartHumanCrouchIdle()
	{
		SetState(HUMAN_CROUCH_IDLE);
		SetIdleTimeNow();
	}

	public virtual void DoHumanCrouchIdle()
	{
		PlayAnimation(AnimationString.HUMAN_CROUCH_IDLE, WrapMode.Loop);
		if (mTarget == null)
		{
			EndHumanCrouchIdle();
			StartHumanStandup();
		}
		else
		{
			if (!base.IsMasterPlayer || !(GetIdleTimeDuration() > mIdleTime))
			{
				return;
			}
			EndHumanCrouchIdle();
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			bool flag = true;
			Vector3 direction = mTargetPosition - entityTransform.position;
			if (direction.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
			{
				float num = Vector3.Angle(to: new Vector3(direction.x, 0f, direction.z), from: mCurrentCoverScript.Expose.transform.forward);
				if (num < 90f)
				{
					mRay = new Ray(entityTransform.position + mCrouchHideHeight * Vector3.up, direction);
					int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
					if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				mRay = new Ray(entityTransform.position + mCrouchFireHeight * Vector3.up, direction);
				int layerMask2 = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
				if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask2))
				{
					if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
					{
						StartHumanLeanCrouch(mCurrentCoverScript.Id);
						enemyStateConst = EnemyStateConst.HUMAN_LEAN_CROUCH;
					}
					else
					{
						StartHumanFullCoverStandup(mCurrentCoverScript.Id);
						enemyStateConst = EnemyStateConst.HUMAN_FULL_COVER_STANDUP;
					}
				}
				else
				{
					StartHumanFullCoverStandup(mCurrentCoverScript.Id);
					enemyStateConst = EnemyStateConst.HUMAN_FULL_COVER_STANDUP;
				}
			}
			else
			{
				StartHumanStandup();
				enemyStateConst = EnemyStateConst.HUMAN_STANDUP;
			}
			if (enemyStateConst != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest enemyStateRequest = null;
				enemyStateRequest = ((enemyStateConst != EnemyStateConst.HUMAN_STANDUP) ? new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mCurrentCoverScript.Id) : new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position));
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
			}
		}
	}

	protected virtual void EndHumanCrouchIdle()
	{
	}

	protected virtual void StartHumanCrouchReload()
	{
		SetState(HUMAN_CROUCH_RELOAD);
	}

	public virtual void DoHumanCrouchReload()
	{
		PlayAnimation(AnimationString.HUMAN_CROUCH_RELOAD, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.HUMAN_CROUCH_RELOAD, 1f))
		{
			EndHumanCrouchReload();
			StartHumanCrouchIdle();
		}
	}

	protected virtual void EndHumanCrouchReload()
	{
	}

	public virtual void StartHumanCrouch(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_CROUCH);
		LookAtTargetHorizontal();
	}

	public virtual void DoHumanCrouch()
	{
		PlayAnimation(AnimationString.HUMAN_CROUCH, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.HUMAN_CROUCH, 1f))
		{
			EndHumanCrouch();
			StartHumanCrouchReload();
		}
	}

	protected virtual void EndHumanCrouch()
	{
	}

	protected virtual void StartHumanFireCoverExposeEnd()
	{
		SetState(HUMAN_FIRE_COVER_EXPOSE_END);
	}

	public virtual void DoHumanFireCoverExposeEnd()
	{
		string name = AnimationString.HUMAN_LEAN_LEFT_BACK;
		if (mCurrentCoverScript.Direction == CoverDirection.RIGHT)
		{
			name = AnimationString.HUMAN_LEAN_RIGHT_BACK;
		}
		PlayAnimation(name, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (!AnimationPlayed(name, 1f))
		{
			return;
		}
		EndHumanFireCoverExposeEnd();
		if (mCurrentCoverAttackCount >= mCoverAttackCount)
		{
			LeaveCover();
			StartHumanReload();
		}
		else if (base.IsMasterPlayer)
		{
			StartHumanMoveCoverHide(mCurrentCoverScript.Id);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_MOVE_COVER_HIDE, GetTransform().position, (byte)mCurrentCoverScript.Id);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			StartHumanIdleCoverHide();
		}
	}

	protected virtual void EndHumanFireCoverExposeEnd()
	{
	}

	public virtual void StartHumanFireCoverExposeStart(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_FIRE_COVER_EXPOSE_START);
		LookAtTargetHorizontal();
	}

	public virtual void DoHumanFireCoverExposeStart()
	{
		string name = AnimationString.HUMAN_LEAN_LEFT;
		if (mCurrentCoverScript.Direction == CoverDirection.RIGHT)
		{
			name = AnimationString.HUMAN_LEAN_RIGHT;
		}
		PlayAnimation(name, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(name, 1f))
		{
			EndHumanFireCoverExposeStart();
			StartHumanFireCoverExpose();
		}
	}

	protected virtual void EndHumanFireCoverExposeStart()
	{
	}

	protected virtual void StartHumanFireCoverExpose()
	{
		SetState(HUMAN_FIRE_COVER_EXPOSE);
		LookAtTargetHorizontal();
		mCurrentCoverAttackCount++;
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public virtual void DoHumanFireCoverExpose()
	{
		string text = AnimationString.HUMAN_LEAN_LEFT_FIRE;
		if (mCurrentCoverScript.Direction == CoverDirection.RIGHT)
		{
			text = AnimationString.HUMAN_LEAN_RIGHT_FIRE;
		}
		PlayAnimation(text, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(text);
		if (mTarget == null)
		{
			mCurrentCoverAttackCount = mCoverAttackCount;
		}
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndHumanFireCoverExpose();
			StartHumanFireCoverExposeEnd();
		}
		else if (mCoverCheckTimer.Ready())
		{
			mCoverCheckTimer.Do();
			Vector3 vector = mTargetPosition - entityTransform.position;
			float num = Vector3.Angle(to: new Vector3(vector.x, 0f, vector.z), from: mCurrentCoverScript.Expose.transform.forward);
			if (num > 90f)
			{
				StartEnemyIdleWithoutResetTime();
			}
		}
	}

	protected virtual void EndHumanFireCoverExpose()
	{
		SetCoverTimeNow();
	}

	public virtual void StartHumanFullCoverFire(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_FULL_COVER_FIRE);
		LookAtTargetHorizontal();
		mCurrentCoverAttackCount++;
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public virtual void DoHumanFullCoverFire()
	{
		PlayAnimation(AnimationString.HUMAN_IDLE_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(AnimationString.HUMAN_IDLE_FIRE);
		if (mTarget == null)
		{
			mCurrentCoverAttackCount = mCoverAttackCount;
		}
		if (mCurrentBulletCount < mRangedBulletCount1)
		{
			return;
		}
		EndHumanFullCoverFire();
		if (mCurrentCoverAttackCount >= mCoverAttackCount)
		{
			LeaveCover();
			StartHumanReload();
		}
		else if (base.IsMasterPlayer)
		{
			int num = Random.Range(0, 100);
			if (num < 50)
			{
				StartHumanReload();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_RELOAD, GetTransform().position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				StartHumanCrouch(mCurrentCoverScript.Id);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_CROUCH, GetTransform().position, (byte)mCurrentCoverScript.Id);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
		}
		else
		{
			StartHumanFullCoverIdle();
		}
	}

	protected virtual void EndHumanFullCoverFire()
	{
	}

	protected virtual void StartHumanFullCoverIdle()
	{
		SetState(HUMAN_FULL_COVER_IDLE);
		LookAtTargetHorizontal();
		SetIdleTimeNow();
	}

	public virtual void DoHumanFullCoverIdle()
	{
		PlayAnimation(AnimationString.Idle, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (base.IsMasterPlayer && GetIdleTimeDuration() > mIdleTime)
		{
			EndHumanFullCoverIdle();
			StartHumanFullCoverFire(mCurrentCoverScript.Id);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_FULL_COVER_FIRE, GetTransform().position, (byte)mCurrentCoverScript.Id);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected virtual void EndHumanFullCoverIdle()
	{
	}

	public virtual void StartHumanFullCoverStandup(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_FULL_COVER_STANDUP);
		LookAtTargetHorizontal();
	}

	public virtual void DoHumanFullCoverStandup()
	{
		PlayAnimation(AnimationString.HUMAN_STANDUP, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.HUMAN_STANDUP, 1f))
		{
			EndHumanFullCoverStandup();
			StartHumanFullCoverFire(mCurrentCoverScript.Id);
		}
	}

	protected virtual void EndHumanFullCoverStandup()
	{
	}

	protected virtual void StartHumanIdleCoverHide()
	{
		SetState(HUMAN_IDLE_COVER_HIDE);
		SetIdleTimeNow();
	}

	public virtual void DoHumanIdleCoverHide()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (mTarget == null)
		{
			EndHumanIdleCoverHide();
			StartEnemyIdle();
		}
		else
		{
			if (!base.IsMasterPlayer || !(GetIdleTimeDuration() > mIdleTime))
			{
				return;
			}
			EndHumanIdleCoverHide();
			if (base.IsMasterPlayer)
			{
				Vector3 vector = mTargetPosition - entityTransform.position;
				if (vector.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
				{
					float num = Vector3.Angle(to: new Vector3(vector.x, 0f, vector.z), from: mCurrentCoverScript.Expose.transform.forward);
					if (num < 90f)
					{
						StartHumanMoveCoverExpose(mCurrentCoverScript.Id);
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE, GetTransform().position, (byte)mCurrentCoverScript.Id);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
					}
					else
					{
						StartEnemyIdle();
					}
				}
				else
				{
					StartEnemyIdle();
				}
			}
			else
			{
				StartEnemyIdle();
			}
		}
	}

	protected virtual void EndHumanIdleCoverHide()
	{
	}

	protected virtual void StartHumanLeanCrouchBack()
	{
		SetState(HUMAN_LEAN_CROUCH_BACK);
	}

	public virtual void DoHumanLeanCrouchBack()
	{
		PlayAnimation(AnimationString.HUMAN_LEAN_CROUCH_BACK, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.HUMAN_LEAN_CROUCH_BACK, 1f))
		{
			EndHumanLeanCrouchBack();
			StartHumanCrouchReload();
		}
	}

	protected virtual void EndHumanLeanCrouchBack()
	{
	}

	public virtual void StartHumanLeanCrouchFire(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_LEAN_CROUCH_FIRE);
		LookAtTargetHorizontal();
		mCurrentCoverAttackCount++;
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public virtual void DoHumanLeanCrouchFire()
	{
		PlayAnimation(AnimationString.HUMAN_LEAN_CROUCH_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(AnimationString.HUMAN_LEAN_CROUCH_FIRE);
		if (mTarget == null)
		{
			mCurrentCoverAttackCount = mCoverAttackCount;
		}
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndHumanLeanCrouchFire();
			if (mCurrentCoverAttackCount >= mCoverAttackCount)
			{
				LeaveCover();
				StartHumanReload();
			}
			else
			{
				StartHumanLeanCrouchBack();
			}
		}
	}

	protected virtual void EndHumanLeanCrouchFire()
	{
	}

	public virtual void StartHumanLeanCrouch(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_LEAN_CROUCH);
		LookAtTargetHorizontal();
	}

	public virtual void DoHumanLeanCrouch()
	{
		PlayAnimation(AnimationString.HUMAN_LEAN_CROUCH, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.HUMAN_LEAN_CROUCH, 1f))
		{
			EndHumanLeanCrouch();
			StartHumanLeanCrouchFire(mCurrentCoverScript.Id);
		}
	}

	protected virtual void EndHumanLeanCrouch()
	{
	}

	public virtual void StartHumanMoveCoverExpose(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_MOVE_COVER_EXPOSE);
		SetNavMeshForMoveToCoverExpose();
	}

	public virtual void DoHumanMoveCoverExpose()
	{
		LookAtTargetHorizontal();
		string moveAnimation = GetMoveAnimation();
		PlayAnimation(moveAnimation, WrapMode.Loop);
		bool flag = false;
		if (mTarget == null)
		{
			EndHumanMoveCoverExpose();
			StartEnemyIdle();
			return;
		}
		if (mCoverCheckTimer.Ready())
		{
			mCoverCheckTimer.Do();
			Vector3 vector = mLeanLeftPosition;
			if (mCurrentCoverScript.Direction == CoverDirection.RIGHT)
			{
				vector = mLeanRightPosition;
			}
			Vector3 vector2 = entityTransform.position + vector.x * entityTransform.right + vector.y * entityTransform.up + vector.z * entityTransform.forward;
			Vector3 vector3 = mTargetPosition + vector.y * Vector3.up - vector2;
			mRay = new Ray(vector2, vector3);
			Debug.DrawRay(vector2, vector3);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
			if (Physics.Raycast(mRay, out mRaycastHit, vector3.magnitude, layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
			{
				flag = true;
			}
		}
		if (!flag && (entityTransform.position - mCurrentCoverScript.Expose.transform.position).sqrMagnitude < 1f)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		EndHumanMoveCoverExpose();
		if (base.IsMasterPlayer)
		{
			StartHumanFireCoverExposeStart(mCurrentCoverScript.Id);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_FIRE_COVER_EXPOSE_START, GetTransform().position, (byte)mCurrentCoverScript.Id);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			StartHumanIdleCoverHide();
		}
	}

	protected virtual void EndHumanMoveCoverExpose()
	{
		mIdleCoverExposeMoveDirection = mNavMeshAgent.velocity.normalized;
		StopNavMesh();
	}

	public virtual void StartHumanMoveCoverHide(int coverId)
	{
		SetCurrentCoverScript(coverId);
		SetState(HUMAN_MOVE_COVER_HIDE);
		SetNavMeshForMoveToCoverHide();
	}

	public virtual void DoHumanMoveCoverHide()
	{
		LookAtTargetHorizontal();
		string moveAnimation = GetMoveAnimation();
		PlayAnimation(moveAnimation, WrapMode.Loop);
		bool flag = false;
		float num = animation[moveAnimation].time / animation[moveAnimation].clip.length;
		num -= Mathf.Floor(num);
		if (num > 0.9f)
		{
			Vector3 vector = mLeanLeftPosition;
			if (mCurrentCoverScript.Direction == CoverDirection.RIGHT)
			{
				vector = mLeanRightPosition;
			}
			Vector3 vector2 = entityTransform.position + vector.x * entityTransform.right + vector.y * entityTransform.up + vector.z * entityTransform.forward;
			Vector3 vector3 = mTargetPosition + vector.y * Vector3.up - vector2;
			mRay = new Ray(vector2, vector3);
			Debug.DrawRay(vector2, vector3);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
			if (Physics.Raycast(mRay, out mRaycastHit, vector3.magnitude, layerMask) && mRaycastHit.collider.gameObject.layer != PhysicsLayer.PLAYER && mRaycastHit.collider.gameObject.layer != PhysicsLayer.REMOTE_PLAYER)
			{
				flag = true;
			}
		}
		if (!flag && (entityTransform.position - mCurrentCoverScript.Hide.transform.position).sqrMagnitude < 1f)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		EndHumanMoveCoverHide();
		if (base.IsMasterPlayer)
		{
			StartHumanReload();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_RELOAD, GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			StartHumanIdleCoverHide();
		}
	}

	protected virtual void EndHumanMoveCoverHide()
	{
		StopNavMesh();
	}

	public virtual void StartHumanMove()
	{
		SetState(HUMAN_MOVE);
		SetMoveTimeNow();
		if (base.IsMasterPlayer)
		{
			ChooseTargetPlayer(false);
		}
		SetNavMeshForMoveToTarget();
		SetUpdatePosTimeNow();
	}

	public virtual void DoHumanMove()
	{
		LookAtTargetHorizontal();
		string moveAnimation = GetMoveAnimation();
		PlayAnimation(moveAnimation, WrapMode.Loop);
		float num = animation[moveAnimation].time / animation[moveAnimation].clip.length;
		num -= Mathf.Floor(num);
		if (mTarget == null && num > 0.9f && !mIsOnOffMeshLink)
		{
			EndHumanMove();
			StartEnemyIdle();
		}
		else
		{
			if (!base.IsMasterPlayer)
			{
				return;
			}
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			if (num > 0.9f)
			{
				float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
				if (horizontalSqrDistanceFromTarget > (float)(mRangedAttackToCatchRadius * mRangedAttackToCatchRadius))
				{
					EndHumanMove();
					StartCatching();
					enemyStateConst = EnemyStateConst.CATCHING;
				}
				else if (horizontalSqrDistanceFromTarget > (float)(mRangedStandAttackRadius * mRangedStandAttackRadius))
				{
					if (GetMoveTimeDuration() > mRangedInterval1)
					{
						if (mNeverSeePlayer)
						{
							if (canHitTargetPlayer() && !mIsOnOffMeshLink)
							{
								mNeverSeePlayer = false;
							}
							else
							{
								SetMoveTimeNow();
								SetNavMeshForMoveToTarget();
							}
						}
						if (!mNeverSeePlayer)
						{
							EndHumanMove();
							StartHumanStandFire();
							enemyStateConst = EnemyStateConst.HUMAN_STAND_FIRE;
						}
					}
				}
				else if (canHitTargetPlayer() && !mIsOnOffMeshLink)
				{
					EndHumanMove();
					StartHumanStandFire();
					mStandFireCheckTimer.Do();
					enemyStateConst = EnemyStateConst.HUMAN_STAND_FIRE;
				}
				else if (GetMoveTimeDuration() > mRangedInterval1)
				{
					if (mNeverSeePlayer)
					{
						if (canHitTargetPlayer() && !mIsOnOffMeshLink)
						{
							mNeverSeePlayer = false;
						}
						else
						{
							SetMoveTimeNow();
							SetNavMeshForMoveToTarget();
						}
					}
					if (!mNeverSeePlayer)
					{
						EndHumanMove();
						StartHumanStandFire();
						enemyStateConst = EnemyStateConst.HUMAN_STAND_FIRE;
					}
				}
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_MOVE, GetTransform().position, (short)(base.SpeedRate * 100f));
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	protected virtual void EndHumanMove()
	{
		StopNavMesh();
	}

	public virtual void StartHumanReload()
	{
		SetState(HUMAN_RELOAD);
	}

	public virtual void DoHumanReload()
	{
		PlayAnimation(AnimationString.HUMAN_RELOAD, WrapMode.ClampForever);
		if (mCurrentCoverScript == null)
		{
			LookAtTargetHorizontal();
		}
		if (AnimationPlayed(AnimationString.HUMAN_RELOAD, 1f))
		{
			EndHumanReload();
			if (mCurrentCoverScript == null)
			{
				StartEnemyIdle();
			}
			else if (mCurrentCoverScript.Type == CoverType.STAND)
			{
				StartHumanIdleCoverHide();
			}
			else
			{
				StartHumanFullCoverIdle();
			}
		}
	}

	protected virtual void EndHumanReload()
	{
	}

	public virtual void StartHumanRunToCover(int coverId, Vector3 position)
	{
		SetCurrentCoverScript(coverId, position);
		SetNavMeshForRunToCover();
		mCurrentCoverAttackCount = 0;
		SetState(HUMAN_RUN_TO_COVER);
		SetUpdatePosTimeNow();
	}

	public virtual void DoHumanRunToCover()
	{
		PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop);
		if (mTarget == null)
		{
			EndHumanRunToCover();
			StartEnemyIdle();
		}
		else if (mCurrentCoverScript.Type == CoverType.STAND)
		{
			if ((entityTransform.position - mCurrentCoverScript.Hide.transform.position).sqrMagnitude < 1f)
			{
				EndHumanRunToCover();
				if (base.IsMasterPlayer)
				{
					Vector3 vector = mTargetPosition - entityTransform.position;
					if (vector.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
					{
						float num = Vector3.Angle(to: new Vector3(vector.x, 0f, vector.z), from: mCurrentCoverScript.Expose.transform.forward);
						if (num < 90f)
						{
							StartHumanMoveCoverExpose(mCurrentCoverScript.Id);
							if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
							{
								EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_MOVE_COVER_EXPOSE, entityTransform.position, (byte)mCurrentCoverScript.Id);
								GameApp.GetInstance().GetNetworkManager().SendRequest(request);
							}
						}
						else
						{
							StartEnemyIdle();
						}
					}
					else
					{
						StartEnemyIdle();
					}
				}
				else
				{
					StartEnemyIdle();
				}
			}
			else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_RUN_TO_COVER, GetTransform().position, mCurrentCoverScript.CrouchPosition, (byte)mCurrentCoverScript.Id, (short)(base.SpeedRate * 100f));
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		else if ((entityTransform.position - mCurrentCoverScript.CrouchPosition).sqrMagnitude < 1f)
		{
			EndHumanRunToCover();
			if (base.IsMasterPlayer)
			{
				Vector3 direction = mTargetPosition - entityTransform.position;
				if (direction.sqrMagnitude < (float)(mRangedMoveAttackRadius * mRangedMoveAttackRadius))
				{
					float num2 = Vector3.Angle(to: new Vector3(direction.x, 0f, direction.z), from: mCurrentCoverScript.Expose.transform.forward);
					if (num2 < 90f)
					{
						mRay = new Ray(entityTransform.position + mCrouchFireHeight * Vector3.up, direction);
						int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
						if (Physics.Raycast(mRay, out mRaycastHit, direction.magnitude, layerMask))
						{
							if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
							{
								StartHumanLeanCrouchFire(mCurrentCoverScript.Id);
								if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
								{
									EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_LEAN_CROUCH_FIRE, entityTransform.position, (byte)mCurrentCoverScript.Id);
									GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
								}
							}
							else
							{
								StartHumanFullCoverFire(mCurrentCoverScript.Id);
								if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
								{
									EnemyStateRequest request4 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_FULL_COVER_FIRE, entityTransform.position, (byte)mCurrentCoverScript.Id);
									GameApp.GetInstance().GetNetworkManager().SendRequest(request4);
								}
							}
						}
						else
						{
							StartHumanFullCoverFire(mCurrentCoverScript.Id);
							if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
							{
								EnemyStateRequest request5 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_FULL_COVER_FIRE, entityTransform.position, (byte)mCurrentCoverScript.Id);
								GameApp.GetInstance().GetNetworkManager().SendRequest(request5);
							}
						}
					}
					else
					{
						StartEnemyIdle();
					}
				}
				else
				{
					StartEnemyIdle();
				}
			}
			else
			{
				StartEnemyIdle();
			}
		}
		else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request6 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.HUMAN_RUN_TO_COVER, GetTransform().position, mCurrentCoverScript.CrouchPosition, (byte)mCurrentCoverScript.Id, (short)(base.SpeedRate * 100f));
			GameApp.GetInstance().GetNetworkManager().SendRequest(request6);
		}
	}

	protected virtual void EndHumanRunToCover()
	{
		StopNavMesh();
		mIdleCoverExposeMoveDirection = mNavMeshAgent.velocity.normalized;
	}

	public virtual void StartHumanStandFire()
	{
		SetState(HUMAN_STAND_FIRE);
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public virtual void DoHumanStandFire()
	{
		PlayAnimation(AnimationString.HUMAN_IDLE_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot(AnimationString.HUMAN_IDLE_FIRE);
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndHumanStandFire();
			StartHumanReload();
		}
	}

	protected virtual void EndHumanStandFire()
	{
	}

	public virtual void StartHumanStandup()
	{
		SetState(HUMAN_STANDUP);
	}

	public virtual void DoHumanStandup()
	{
		PlayAnimation(AnimationString.HUMAN_STANDUP, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.HUMAN_STANDUP, 1f))
		{
			EndHumanStandup();
			StartEnemyIdle();
		}
	}

	protected virtual void EndHumanStandup()
	{
	}

	protected override void ChangeShader()
	{
		base.ChangeShader();
		GameObject gameObject = entityObject.transform.Find(BoneName.EnemyHumanWeapon).gameObject;
		gameObject.SetActive(false);
	}
}
