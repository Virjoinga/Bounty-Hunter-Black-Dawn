using UnityEngine;

public class Ghost : Enemy
{
	public static EnemyState GHOST_DODGE_LEFT_STATE = new GhostDodgeLeftState();

	public static EnemyState GHOST_DODGE_RIGHT_STATE = new GhostDodgeRightState();

	public static EnemyState GHOST_DOUBLE_ATTACK_STATE = new GhostDoubleAttackState();

	public static EnemyState GHOST_JUMP_ATTACK_STATE = new GhostJumpAttackState();

	public static EnemyState GHOST_STICK_ATTCK_STATE = new GhostStickAttackState();

	protected Timer mRunAttackTimer = new Timer();

	protected Timer mDodgeTimer = new Timer();

	protected int mDoubleAttackCount;

	protected float mDodgeLeftDistance;

	protected float mDodgeRightDistance;

	protected bool mNeedDodge;

	protected bool mIsJump;

	protected int mProbabilityRightAttack;

	protected int mProbabilityLeftAttack;

	protected int mProbabilityJumpAttack;

	protected int mProbabilityDodgeLeft;

	protected int mProbabilityDodgeRight;

	protected MeleeAttackData mAttackData = default(MeleeAttackData);

	protected MeleeAttackData mStickAttackData = default(MeleeAttackData);

	protected MeleeAttackData mDoubleLeftAttackData = default(MeleeAttackData);

	protected MeleeAttackData mDoubleRightAttackData = default(MeleeAttackData);

	protected MeleeAttackData mJumpAttackData = default(MeleeAttackData);

	public override void Init()
	{
		base.Init();
		mCanAwake = true;
		mCanRage = true;
		mRunAttackTimer.SetTimer(1f, false);
		mDodgeTimer.SetTimer(3.2f, false);
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		mProbabilityRightAttack = 40;
		mProbabilityLeftAttack = 80;
		mProbabilityJumpAttack = 50;
		mProbabilityDodgeLeft = 15;
		mProbabilityDodgeRight = 30;
		mDodgeLeftDistance = 2f;
		mDodgeRightDistance = 5f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.GhostMesh;
	}

	public override void Activate()
	{
		base.Activate();
		Transform parent = entityObject.transform.Find(BoneName.GhostLeftPoint);
		GameObject original = Resources.Load("Enemy/knife01") as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.name = "knife01";
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Transform parent2 = entityObject.transform.Find(BoneName.GhostRightPoint);
		GameObject original2 = Resources.Load("Enemy/knife02") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original2) as GameObject;
		gameObject2.name = "knife02";
		gameObject2.transform.parent = parent2;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mNeedDodge = false;
		mAttackData.Animation = AnimationString.ENEMY_ATTACK;
		mAttackData.Trans = entityTransform;
		mAttackData.StartPercent = 0.42f;
		mAttackData.EndPercent = 0.62f;
		mAttackData.Range = 3f;
		mAttackData.Angle = 60f;
		mAttackData.Damage = mMeleeAttackDamage1;
		mAttackData.KnockedSpeed = 0f;
		mStickAttackData.Animation = AnimationString.GHOST_STICK_ATTACK;
		mStickAttackData.Trans = entityTransform;
		mStickAttackData.StartPercent = 0.33f;
		mStickAttackData.EndPercent = 0.54f;
		mStickAttackData.Range = 3f;
		mStickAttackData.Angle = 60f;
		mStickAttackData.Damage = mMeleeAttackDamage2;
		mStickAttackData.KnockedSpeed = 0f;
		mDoubleLeftAttackData.Animation = AnimationString.GHOST_DOUBLE_ATTACK;
		mDoubleLeftAttackData.Trans = entityTransform;
		mDoubleLeftAttackData.StartPercent = 0.06f;
		mDoubleLeftAttackData.EndPercent = 0.31f;
		mDoubleLeftAttackData.Range = 3f;
		mDoubleLeftAttackData.Angle = 60f;
		mDoubleLeftAttackData.Damage = mRangedAttackDamage1;
		mDoubleLeftAttackData.KnockedSpeed = 0f;
		mDoubleRightAttackData.Animation = AnimationString.GHOST_DOUBLE_ATTACK;
		mDoubleRightAttackData.Trans = entityTransform;
		mDoubleRightAttackData.StartPercent = 0.56f;
		mDoubleRightAttackData.EndPercent = 0.81f;
		mDoubleRightAttackData.Range = 3f;
		mDoubleRightAttackData.Angle = 60f;
		mDoubleRightAttackData.Damage = mRangedAttackDamage1;
		mDoubleRightAttackData.KnockedSpeed = 0f;
		mJumpAttackData.Animation = AnimationString.GHOST_JUMP_ATTACK;
		mJumpAttackData.Trans = entityTransform;
		mJumpAttackData.StartPercent = 0.59f;
		mJumpAttackData.EndPercent = 0.67f;
		mJumpAttackData.Range = 3f;
		mJumpAttackData.Angle = 60f;
		mJumpAttackData.Damage = mRushAttackDamage1;
		mJumpAttackData.KnockedSpeed = 0f;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.GhostHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	protected override void ChangeShader()
	{
		base.ChangeShader();
		GameObject gameObject = entityObject.transform.Find(BoneName.GhostLeftKnife).gameObject;
		gameObject.SetActive(false);
		GameObject gameObject2 = entityObject.transform.Find(BoneName.GhostRightKnife).gameObject;
		gameObject2.SetActive(false);
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mState == Enemy.CATCHING_STATE || (mState == Enemy.IDLE_STATE && mDodgeTimer.Ready()))
		{
			mDodgeTimer.Do();
			mNeedDodge = true;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mState == Enemy.CATCHING_STATE || (mState == Enemy.IDLE_STATE && mDodgeTimer.Ready()))
		{
			mDodgeTimer.Do();
			mNeedDodge = true;
		}
	}

	private void SetNavMeshForJumpAttack()
	{
		Vector3 vector = mTargetPosition;
		Vector3 normalized = (vector - entityTransform.position).normalized;
		vector -= 2f * normalized;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(vector);
			mNavMeshAgent.speed = 15f;
			SetCanTurn(false);
		}
	}

	private void SetNavMeshForDodgeLeft(Vector3 dodgeTarget)
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(dodgeTarget);
			mNavMeshAgent.speed = 4f;
			SetCanTurn(true);
		}
	}

	private void SetNavMeshForDodgeRight(Vector3 dodgeTarget)
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(dodgeTarget);
			mNavMeshAgent.speed = 10f;
			SetCanTurn(true);
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == GHOST_DODGE_LEFT_STATE)
		{
			EndGhostDodgeLeft();
		}
		else if (mState == GHOST_DODGE_RIGHT_STATE)
		{
			EndGhostDodgeRight();
		}
		else if (mState == GHOST_DOUBLE_ATTACK_STATE)
		{
			EndGhostDoubleAttack();
		}
		else if (mState == GHOST_JUMP_ATTACK_STATE)
		{
			EndGhostJumpAttack();
		}
		else if (mState == GHOST_STICK_ATTCK_STATE)
		{
			EndGhostStickAttack();
		}
	}

	public override void StartCatching()
	{
		base.StartCatching();
		mRunAttackTimer.Do();
	}

	protected override void MakeDecisionInCatching()
	{
		if (mNeedDodge)
		{
			mNeedDodge = false;
			if (!(GetDistanceFromTarget() > 2f * mMeleeAttackRadius))
			{
				return;
			}
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			Vector3 vector = Vector3.zero;
			int num = Random.Range(0, 100);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.GAME_UNIT) | (1 << PhysicsLayer.PLAYER_COLLIDER);
			if (num < mProbabilityDodgeLeft)
			{
				Vector3 vector2 = -entityTransform.right;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), vector2);
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeLeftDistance, layerMask))
				{
					vector = entityTransform.position + vector2 * mDodgeLeftDistance;
					EndEnemyIdle();
					StartGhostDodgeLeft(vector);
					enemyStateConst = EnemyStateConst.GHOST_DODGE_LEFT;
				}
			}
			else if (num < mProbabilityDodgeRight)
			{
				Vector3 right = entityTransform.right;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), right);
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeRightDistance, layerMask))
				{
					vector = entityTransform.position + right * mDodgeRightDistance;
					EndEnemyIdle();
					StartGhostDodgeRight(vector);
					enemyStateConst = EnemyStateConst.GHOST_DODGE_RIGHT;
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			return;
		}
		EnemyStateConst enemyStateConst2 = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
		{
			EndCatching();
			int num2 = Random.Range(0, 100);
			if (num2 < mProbabilityRightAttack)
			{
				StartEnemyAttack();
				enemyStateConst2 = EnemyStateConst.ATTACK;
			}
			else if (num2 < mProbabilityLeftAttack)
			{
				StartGhostStickAttack();
				enemyStateConst2 = EnemyStateConst.GHOST_STICK_ATTACK;
			}
			else
			{
				StartGhostDoubleAttack();
				enemyStateConst2 = EnemyStateConst.GHOST_DOUBLE_ATTACK;
			}
		}
		else if (mRunAttackTimer.Ready() && sqrDistanceFromTarget < 100f && sqrDistanceFromTarget > 81f)
		{
			mRunAttackTimer.Do();
			int num3 = Random.Range(0, 100);
			if (num3 < mProbabilityJumpAttack && canHitTargetPlayer())
			{
				EndCatching();
				StartGhostJumpAttack();
				enemyStateConst2 = EnemyStateConst.GHOST_JUMP_ATTACK;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst2 != 0)
		{
			EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst2, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_dead");
	}

	public override void StartEnemyAttack()
	{
		base.StartEnemyAttack();
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_attack01");
	}

	protected override void CheckEnemyAttack()
	{
		CheckEnemyAttack(mAttackData);
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (mNeedDodge)
		{
			mNeedDodge = false;
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			Vector3 vector = Vector3.zero;
			int num = Random.Range(0, 100);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.GAME_UNIT) | (1 << PhysicsLayer.PLAYER_COLLIDER);
			if (num < mProbabilityDodgeLeft)
			{
				Vector3 vector2 = -entityTransform.right;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), vector2);
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeLeftDistance, layerMask))
				{
					vector = entityTransform.position + vector2 * mDodgeLeftDistance;
					EndEnemyIdle();
					StartGhostDodgeLeft(vector);
					enemyStateConst = EnemyStateConst.GHOST_DODGE_LEFT;
				}
			}
			else if (num < mProbabilityDodgeRight)
			{
				Vector3 right = entityTransform.right;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), right);
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeRightDistance, layerMask))
				{
					vector = entityTransform.position + right * mDodgeRightDistance;
					EndEnemyIdle();
					StartGhostDodgeRight(vector);
					enemyStateConst = EnemyStateConst.GHOST_DODGE_RIGHT;
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		if (!(GetIdleTimeDuration() > mIdleTime))
		{
			return;
		}
		EnemyStateConst enemyStateConst2 = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
		{
			EndEnemyIdle();
			int num2 = Random.Range(0, 100);
			if (num2 < mProbabilityRightAttack)
			{
				StartEnemyAttack();
				enemyStateConst2 = EnemyStateConst.ATTACK;
			}
			else if (num2 < mProbabilityLeftAttack)
			{
				StartGhostStickAttack();
				enemyStateConst2 = EnemyStateConst.GHOST_STICK_ATTACK;
			}
			else
			{
				StartGhostDoubleAttack();
				enemyStateConst2 = EnemyStateConst.GHOST_DOUBLE_ATTACK;
			}
		}
		else
		{
			EndEnemyIdle();
			StartCatching();
			enemyStateConst2 = EnemyStateConst.CATCHING;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst2 != 0)
		{
			EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst2, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_attacked");
	}

	protected override void PlayRageSound()
	{
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_scream");
	}

	public void StartGhostDodgeLeft(Vector3 dodgeTarget)
	{
		SetState(GHOST_DODGE_LEFT_STATE);
		LookAtTargetHorizontal();
		SetNavMeshForDodgeLeft(dodgeTarget);
		mNeedDodge = false;
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_dodge");
	}

	public void DoGhostDodgeLeft()
	{
		PlayAnimation(AnimationString.GHOST_DODGE_LEFT, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.GHOST_DODGE_LEFT, 1f))
		{
			EndGhostDodgeLeft();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected void EndGhostDodgeLeft()
	{
		StopNavMesh();
	}

	public void StartGhostDodgeRight(Vector3 dodgeTarget)
	{
		SetState(GHOST_DODGE_RIGHT_STATE);
		LookAtTargetHorizontal();
		SetNavMeshForDodgeRight(dodgeTarget);
		mNeedDodge = false;
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_dodge");
	}

	public void DoGhostDodgeRight()
	{
		PlayAnimation(AnimationString.GHOST_DODGE_RIGHT, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.GHOST_DODGE_RIGHT, 1f))
		{
			EndGhostDodgeRight();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected void EndGhostDodgeRight()
	{
		StopNavMesh();
	}

	public void StartGhostDoubleAttack()
	{
		SetState(GHOST_DOUBLE_ATTACK_STATE);
		LookAtTargetHorizontal();
		mDoubleAttackCount = 0;
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_attack01");
	}

	public void DoGhostDoubleAttack()
	{
		PlayAnimation(AnimationString.GHOST_DOUBLE_ATTACK, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (animation[AnimationString.GHOST_DOUBLE_ATTACK].time > animation[AnimationString.GHOST_DOUBLE_ATTACK].clip.length)
		{
			animation[AnimationString.GHOST_DOUBLE_ATTACK].time -= animation[AnimationString.GHOST_DOUBLE_ATTACK].clip.length;
			mDoubleAttackCount++;
			if (mDoubleAttackCount < mRangedBulletCount1)
			{
				PlaySound("RPG_Audio/Enemy/Ghost/Ghost_attack01");
			}
		}
		CheckEnemyAttack(mDoubleLeftAttackData);
		CheckEnemyAttack(mDoubleRightAttackData);
		if (mDoubleAttackCount >= mRangedBulletCount1)
		{
			EndGhostDoubleAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGhostDoubleAttack()
	{
	}

	public void StartGhostJumpAttack()
	{
		SetState(GHOST_JUMP_ATTACK_STATE);
		mIsJump = false;
	}

	public void DoGhostJumpAttack()
	{
		PlayAnimation(AnimationString.GHOST_JUMP_ATTACK, WrapMode.ClampForever);
		float num = animation[AnimationString.GHOST_JUMP_ATTACK].time / animation[AnimationString.GHOST_JUMP_ATTACK].clip.length;
		if (!mIsJump)
		{
			if (num > 0.2f)
			{
				SetNavMeshForJumpAttack();
				mIsJump = true;
				PlaySound("RPG_Audio/Enemy/Ghost/Ghost_jump_out");
			}
		}
		else
		{
			CheckEnemyAttack(mJumpAttackData);
		}
		if (num > 1f)
		{
			EndGhostJumpAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGhostJumpAttack()
	{
		StopNavMesh();
	}

	public void StartGhostStickAttack()
	{
		SetState(GHOST_STICK_ATTCK_STATE);
		LookAtTargetHorizontal();
		PlaySound("RPG_Audio/Enemy/Ghost/Ghost_attack01");
	}

	public void DoGhostStickAttack()
	{
		PlayAnimation(AnimationString.GHOST_STICK_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		CheckEnemyAttack(mStickAttackData);
		if (AnimationPlayed(AnimationString.GHOST_STICK_ATTACK, 1f))
		{
			EndGhostStickAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGhostStickAttack()
	{
	}

	protected override bool IsJumping()
	{
		return mState == GHOST_JUMP_ATTACK_STATE;
	}
}
