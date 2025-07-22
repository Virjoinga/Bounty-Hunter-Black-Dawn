using System.Collections.Generic;
using UnityEngine;

public class Giant : Enemy
{
	public static EnemyState GIANT_DODGE_LEFT_STATE = new GiantDodgeLeftState();

	public static EnemyState GIANT_DODGE_RIGHT_STATE = new GiantDodgeRightState();

	public static EnemyState GIANT_FIST_ATTACK_STATE = new GiantFistAttackState();

	public static EnemyState GIANT_GROUND_ATTACK_STATE = new GiantGroundAttackState();

	public static EnemyState GIANT_RUN_ATTACK_STATE = new GiantRunAttackState();

	protected Timer mRunAttackTimer = new Timer();

	protected Timer mDodgeTimer = new Timer();

	protected float mDodgeDistance;

	protected int mProbabilityHammerAttack;

	protected int mProbabilityFistAttack;

	protected int mProbabilityRunAttack;

	protected int mProbabilityDodgeLeft;

	protected int mProbabilityDodgeRight;

	protected MeleeAttackData mHammerAttackData = default(MeleeAttackData);

	protected MeleeAttackData mFistAttackData = default(MeleeAttackData);

	protected MeleeAttackData mGroundAttackData = default(MeleeAttackData);

	protected MeleeAttackData mRunAttackData = default(MeleeAttackData);

	protected bool mNeedDodge;

	protected bool mLandEffectPlayed;

	protected bool mGroundAttackEffectPlayed;

	protected bool mRageEffectPlayed;

	protected GameObject mRunSmokeEffect;

	public override void Init()
	{
		base.Init();
		mCanAwake = true;
		mCanRage = true;
		mRunAttackTimer.SetTimer(1f, false);
		mRunAudioTimer.SetTimer(0.4f, true);
		mDodgeTimer.SetTimer(7.2f, false);
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		mDodgeDistance = 10f;
		mProbabilityHammerAttack = 40;
		mProbabilityFistAttack = 80;
		mProbabilityRunAttack = 30;
		mProbabilityDodgeLeft = 15;
		mProbabilityDodgeRight = 30;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.GiantMesh;
	}

	public override void Activate()
	{
		base.Activate();
		Transform parent = entityObject.transform.Find(BoneName.GiantRightHandPoint);
		GameObject original = Resources.Load("Enemy/gungun") as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.name = "gungun";
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mNeedDodge = false;
		mLandEffectPlayed = false;
		mGroundAttackEffectPlayed = false;
		mRageEffectPlayed = false;
		mHammerAttackData.Animation = AnimationString.ENEMY_ATTACK;
		mHammerAttackData.Trans = entityTransform;
		mHammerAttackData.StartPercent = 0.2f;
		mHammerAttackData.EndPercent = 0.35f;
		mHammerAttackData.Range = 7f;
		mHammerAttackData.Angle = 60f;
		mHammerAttackData.Damage = mMeleeAttackDamage1;
		mHammerAttackData.KnockedSpeed = 0f;
		mFistAttackData.Animation = AnimationString.GIANT_FIST_ATTACK;
		mFistAttackData.Trans = entityTransform;
		mFistAttackData.StartPercent = 0.17f;
		mFistAttackData.EndPercent = 0.44f;
		mFistAttackData.Range = 5f;
		mFistAttackData.Angle = 60f;
		mFistAttackData.Damage = mMeleeAttackDamage2;
		mFistAttackData.KnockedSpeed = 0f;
		mGroundAttackData.Animation = AnimationString.GIANT_GROUND_ATTACK;
		mGroundAttackData.Trans = entityTransform.Find(BoneName.GiantRightHandHammer);
		mGroundAttackData.StartPercent = 0.26f;
		mGroundAttackData.EndPercent = 0.73f;
		mGroundAttackData.Range = mRangedExplosionRadius1;
		mGroundAttackData.Angle = 180f;
		mGroundAttackData.Damage = mRangedExtraDamage1;
		mGroundAttackData.KnockedSpeed = 10f;
		mRunAttackData.Animation = AnimationString.GIANT_RUN_ATTACK;
		mRunAttackData.Trans = entityTransform;
		mRunAttackData.StartPercent = 0.45f;
		mRunAttackData.EndPercent = 0.55f;
		mRunAttackData.Range = 7f;
		mRunAttackData.Angle = 60f;
		mRunAttackData.Damage = mRushAttackDamage1;
		mRunAttackData.KnockedSpeed = 10f;
		GameObject original2 = Resources.Load("RPG_effect/RPG_Giant_run_smoke_001") as GameObject;
		mRunSmokeEffect = Object.Instantiate(original2, Vector3.zero, Quaternion.identity) as GameObject;
		mRunSmokeEffect.transform.parent = entityTransform;
		mRunSmokeEffect.transform.localPosition = Vector3.zero;
		mRunSmokeEffect.transform.localRotation = Quaternion.identity;
		mRunSmokeEffect.SetActive(false);
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.GiantHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override Vector3 GetShieldScale()
	{
		return new Vector3(1f, 1f, 1f);
	}

	protected override void ChangeShader()
	{
		base.ChangeShader();
		GameObject gameObject = entityObject.transform.Find(BoneName.GiantRightHandHammer).gameObject;
		gameObject.SetActive(false);
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mState == Enemy.CATCHING_STATE && mDodgeTimer.Ready())
		{
			mDodgeTimer.Do();
			mNeedDodge = true;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mState == Enemy.CATCHING_STATE && mDodgeTimer.Ready())
		{
			mDodgeTimer.Do();
			mNeedDodge = true;
		}
	}

	private void SetNavMeshForRunAttack()
	{
		Vector3 vector = mTargetPosition;
		Vector3 normalized = (vector - entityTransform.position).normalized;
		vector -= 2f * normalized;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(vector);
			mNavMeshAgent.speed = mRunSpeed;
			SetCanTurn(false);
		}
	}

	private void SetNavMeshForDodge(Vector3 dodgeTarget)
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
		if (mState == GIANT_DODGE_LEFT_STATE)
		{
			EndGiantDodgeLeft();
		}
		else if (mState == GIANT_DODGE_RIGHT_STATE)
		{
			EndGiantDodgeRight();
		}
		else if (mState == GIANT_FIST_ATTACK_STATE)
		{
			EndGiantFistAttack();
		}
		else if (mState == GIANT_GROUND_ATTACK_STATE)
		{
			EndGiantGroundAttack();
		}
		else if (mState == GIANT_RUN_ATTACK_STATE)
		{
			EndGiantRunAttack();
		}
	}

	protected override void PlayAwakeSound()
	{
		PlaySound("RPG_Audio/Enemy/Giant/Giant_scream");
	}

	public override void DoAwake()
	{
		if (mCanAwake && !mSpawnPointScript.HasAwaked)
		{
			PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever, 1f);
			LookAtTargetHorizontal();
			if (AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
			{
				EndAwake();
				StartEnemyIdleWithoutResetTime();
				mSpawnPointScript.InformTarget(mTarget);
			}
		}
		else
		{
			EndAwake();
			StartEnemyIdleWithoutResetTime();
			mSpawnPointScript.InformTarget(mTarget);
		}
	}

	public override void StartCatching()
	{
		base.StartCatching();
		mRunAttackTimer.Do();
		mRunSmokeEffect.SetActive(true);
	}

	protected override void PlayRunningSound()
	{
		if (mRunAudioTimer.Ready())
		{
			mRunAudioTimer.Do();
			PlaySound("RPG_Audio/Enemy/Giant/Giant_walk");
		}
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
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeDistance, layerMask))
				{
					vector = entityTransform.position + vector2 * mDodgeDistance;
					EndEnemyIdle();
					StartGiantDodgeLeft(vector);
					enemyStateConst = EnemyStateConst.GIANT_DODGE_LEFT;
				}
			}
			else if (num < mProbabilityDodgeRight)
			{
				Vector3 right = entityTransform.right;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), right);
				if (!Physics.Raycast(mRay, out mRaycastHit, mDodgeDistance, layerMask))
				{
					vector = entityTransform.position + right * mDodgeDistance;
					EndEnemyIdle();
					StartGiantDodgeRight(vector);
					enemyStateConst = EnemyStateConst.GIANT_DODGE_RIGHT;
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
			if (num2 < mProbabilityHammerAttack)
			{
				StartEnemyAttack();
				enemyStateConst2 = EnemyStateConst.ATTACK;
			}
			else if (num2 < mProbabilityFistAttack)
			{
				StartGiantFistAttack();
				enemyStateConst2 = EnemyStateConst.GIANT_FIST_ATTACK;
			}
			else
			{
				StartGiantGroundAttack();
				enemyStateConst2 = EnemyStateConst.GIANT_GROUND_ATTACK;
			}
		}
		else if (mRunAttackTimer.Ready() && sqrDistanceFromTarget < 36f && sqrDistanceFromTarget > 25f)
		{
			mRunAttackTimer.Do();
			int num3 = Random.Range(0, 100);
			if (num3 < mProbabilityRunAttack && canHitTargetPlayer())
			{
				EndCatching();
				StartGiantRunAttack();
				enemyStateConst2 = EnemyStateConst.GIANT_RUN_ATTACK;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst2 != 0)
		{
			EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst2, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	protected override void EndCatching()
	{
		base.EndCatching();
		mRunSmokeEffect.SetActive(false);
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Giant/Giant_dead");
	}

	public override void StartEnemyAttack()
	{
		base.StartEnemyAttack();
		PlaySound("RPG_Audio/Enemy/Giant/Giant_attack01");
	}

	protected override void CheckEnemyAttack()
	{
		CheckEnemyAttack(mHammerAttackData);
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (!(GetIdleTimeDuration() > mIdleTime))
		{
			return;
		}
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
		{
			EndEnemyIdle();
			int num = Random.Range(0, 100);
			if (num < mProbabilityHammerAttack)
			{
				StartEnemyAttack();
				enemyStateConst = EnemyStateConst.ATTACK;
			}
			else if (num < mProbabilityFistAttack)
			{
				StartGiantFistAttack();
				enemyStateConst = EnemyStateConst.GIANT_FIST_ATTACK;
			}
			else
			{
				StartGiantGroundAttack();
				enemyStateConst = EnemyStateConst.GIANT_GROUND_ATTACK;
			}
		}
		else
		{
			EndEnemyIdle();
			StartCatching();
			enemyStateConst = EnemyStateConst.CATCHING;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Giant/Giant_attacked");
	}

	protected override void PlayRageSound()
	{
		PlaySound("RPG_Audio/Enemy/Giant/Giant_scream");
	}

	public void StartGiantDodgeLeft(Vector3 targetPosition)
	{
		SetState(GIANT_DODGE_LEFT_STATE);
		mNeedDodge = false;
		mLandEffectPlayed = false;
		SetNavMeshForDodge(targetPosition);
		LookAtTargetHorizontal();
		PlaySound("RPG_Audio/Enemy/Giant/Giant_dodge");
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	public void DoGiantDodgeLeft()
	{
		PlayAnimation(AnimationString.GIANT_DODGE_LEFT, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		float num = animation[AnimationString.GIANT_DODGE_LEFT].time / animation[AnimationString.GIANT_DODGE_LEFT].clip.length;
		if (num >= 0.5f && null != mNavMeshAgent && !mLandEffectPlayed)
		{
			mNavMeshAgent.speed = 1f;
			mLandEffectPlayed = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		}
		if (num >= 1f)
		{
			EndGiantDodgeLeft();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected void EndGiantDodgeLeft()
	{
		StopNavMesh();
	}

	public void StartGiantDodgeRight(Vector3 targetPosition)
	{
		SetState(GIANT_DODGE_RIGHT_STATE);
		mNeedDodge = false;
		mLandEffectPlayed = false;
		SetNavMeshForDodge(targetPosition);
		LookAtTargetHorizontal();
		PlaySound("RPG_Audio/Enemy/Giant/Giant_dodge");
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	public void DoGiantDodgeRight()
	{
		PlayAnimation(AnimationString.GIANT_DODGE_RIGHT, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		float num = animation[AnimationString.GIANT_DODGE_RIGHT].time / animation[AnimationString.GIANT_DODGE_RIGHT].clip.length;
		if (num >= 0.5f && null != mNavMeshAgent && !mLandEffectPlayed)
		{
			mNavMeshAgent.speed = 1f;
			mLandEffectPlayed = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Giant_jump_smoke_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		}
		if (num >= 1f)
		{
			EndGiantDodgeRight();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected void EndGiantDodgeRight()
	{
		StopNavMesh();
	}

	public void StartGiantFistAttack()
	{
		SetState(GIANT_FIST_ATTACK_STATE);
		LookAtTargetHorizontal();
		PlaySound("RPG_Audio/Enemy/Giant/Giant_attack01");
	}

	public void DoGiantFistAttack()
	{
		PlayAnimation(AnimationString.GIANT_FIST_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		CheckEnemyAttack(mFistAttackData);
		if (AnimationPlayed(AnimationString.GIANT_FIST_ATTACK, 1f))
		{
			EndGiantFistAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGiantFistAttack()
	{
	}

	public void StartGiantGroundAttack()
	{
		SetState(GIANT_GROUND_ATTACK_STATE);
		LookAtTargetHorizontal();
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_melee_attack_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + entityTransform.forward * 4f, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Giant/Giant_attack02");
	}

	public void DoGiantGroundAttack()
	{
		PlayAnimation(AnimationString.GIANT_GROUND_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (mLocalPlayer.CheckHitTimerReady(this) && AnimationPlayed(mGroundAttackData.Animation, mGroundAttackData.StartPercent) && !AnimationPlayed(mGroundAttackData.Animation, mGroundAttackData.EndPercent) && (mLocalPlayer.GetTransform().position - mHammerAttackData.Trans.position).sqrMagnitude < mHammerAttackData.Range * mHammerAttackData.Range)
		{
			mLocalPlayer.OnHit(mGroundAttackData.Damage, this);
			mLocalPlayer.ResetCheckHitTimer(this);
			CheckKnocked(mLocalPlayer, mGroundAttackData.KnockedSpeed);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && AnimationPlayed(mGroundAttackData.Animation, mGroundAttackData.StartPercent) && !AnimationPlayed(mGroundAttackData.Animation, mGroundAttackData.EndPercent) && (value.GetTransform().position - mGroundAttackData.Trans.position).sqrMagnitude < mGroundAttackData.Range * mGroundAttackData.Range)
			{
				value.OnHit(mGroundAttackData.Damage);
				value.ResetCheckHitTimer(this);
				CheckKnocked(value, mGroundAttackData.KnockedSpeed);
			}
		}
		if (AnimationPlayed(AnimationString.GIANT_GROUND_ATTACK, 1f))
		{
			EndGiantGroundAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGiantGroundAttack()
	{
	}

	public void StartGiantRunAttack()
	{
		SetState(GIANT_RUN_ATTACK_STATE);
		SetNavMeshForRunAttack();
		PlaySound("RPG_Audio/Enemy/Giant/Giant_attack02");
	}

	public void DoGiantRunAttack()
	{
		PlayAnimation(AnimationString.GIANT_RUN_ATTACK, WrapMode.ClampForever);
		CheckEnemyAttack(mRunAttackData);
		if (AnimationPlayed(AnimationString.GIANT_RUN_ATTACK, 1f))
		{
			EndGiantRunAttack();
			StartEnemyIdle();
		}
	}

	protected void EndGiantRunAttack()
	{
		StopNavMesh();
	}
}
