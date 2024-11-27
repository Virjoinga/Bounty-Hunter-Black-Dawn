using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
	protected const float mMaxDrillIdleTime = 3.6f;

	protected const float mPatrolDistance = 12f;

	protected const float mDrillOutDistance = 3f;

	public const float P_ATTACK01 = 1f;

	public const float P_ATTACK02 = 1f;

	public const float P_ATTACK03 = 1f;

	public static EnemyState DRILL_IN_STATE = new WormDrillInState();

	public static EnemyState DRILL_OUT_STATE01 = new WormDrillOutState01();

	public static EnemyState DRILL_OUT_STATE02 = new WormDrillOutState02();

	public static EnemyState DRILL_OUT_STATE03 = new WormDrillOutState03();

	public static EnemyState DRILL_IDLE_STATE = new WormDrillIdleState();

	public static EnemyState DRILL_OUT_TAUNT = new WormDrillOutStateTaunt();

	public static EnemyState TAUNT = new WormTauntState();

	public static EnemyState ATTACK01 = new WormAttack01();

	public static EnemyState ATTACK02 = new WormAttack02();

	public static EnemyState ATTACK03 = new WormAttack03();

	protected MeleeAttackData AttackData01 = default(MeleeAttackData);

	protected MeleeAttackData AttackData02 = default(MeleeAttackData);

	protected Timer mPatrolTimer = new Timer();

	protected GameObject mDrillOutEffect;

	protected Vector3 mDrillOutPos;

	private Renderer[] mDrillOutEffectRenderList;

	protected float mIdleStartTime;

	protected Transform mMiddleBodyTransform;

	public float MaxDrillIdleTime
	{
		get
		{
			return 3.6f;
		}
	}

	public float PatrolDistance
	{
		get
		{
			return 12f;
		}
	}

	public void SetIdleStartTime()
	{
		mIdleStartTime = Time.time;
	}

	public float GetDrillIdleTimeDuration()
	{
		return Time.time - mIdleStartTime;
	}

	public override void Init()
	{
		base.Init();
		mShadowPath = string.Empty;
		mPatrolTimer.SetTimer(2f, false);
	}

	protected override string GetBodyMeshName()
	{
		return "Worm";
	}

	public override bool CanAutoAim()
	{
		return base.CanAutoAim() && mState != DRILL_IN_STATE;
	}

	public override void Activate()
	{
		base.Activate();
		AttackData01.Animation = AnimationString.WORM_MELEE_ATTACK01;
		AttackData01.Trans = entityTransform;
		AttackData01.StartPercent = 0.4f;
		AttackData01.EndPercent = 1f;
		AttackData01.Range = mMeleeAttackRadius;
		AttackData01.Angle = 30f;
		AttackData01.Damage = mMeleeAttackDamage1;
		AttackData01.KnockedSpeed = 0f;
		AttackData02.Animation = AnimationString.WORM_MELEE_ATTACK02;
		AttackData02.Trans = entityTransform;
		AttackData02.StartPercent = 0.4f;
		AttackData02.EndPercent = 1f;
		AttackData02.Range = mMeleeAttackRadius;
		AttackData02.Angle = 30f;
		AttackData02.Damage = mMeleeAttackDamage2;
		AttackData02.KnockedSpeed = 0f;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find("worm00");
		mHeadTransform = entityTransform.Find("worm00/worm01/worm02/worm03/worm04/worm05/worm06/worm07/worm08/worm09");
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
		mMiddleBodyTransform = entityTransform.Find("worm00/worm01/worm02/worm03/worm04/worm05/worm06");
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
	}

	public void ShowDrillOutEffect()
	{
		if (mDrillOutEffect == null)
		{
			GameObject original = Resources.Load("RPG_effect/RPG_WormAppear_001") as GameObject;
			mDrillOutEffect = Object.Instantiate(original, entityTransform.position, Quaternion.identity) as GameObject;
			mDrillOutPos = entityTransform.position;
		}
	}

	protected override void MakeDecisionInCatching()
	{
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void DoEnemyIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (base.IsMasterPlayer && CheckTargetInEnemyIdle())
		{
			MakeDecisionInEnemyIdle();
		}
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
			float num = Random.Range(0f, 3f);
			if (num <= 1f)
			{
				StartAttack01();
				enemyStateConst = EnemyStateConst.WORM_ATTACK_01;
			}
			else if (num <= 2f)
			{
				StartAttack02();
				enemyStateConst = EnemyStateConst.WORM_ATTACK_02;
			}
			else
			{
				StartAttack03();
				enemyStateConst = EnemyStateConst.WORM_ATTACK_03;
			}
		}
		else
		{
			EndEnemyIdle();
			StartDrillIn();
			enemyStateConst = EnemyStateConst.WORM_DRILL_IN;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void StartGoBack(Vector3 target)
	{
		SetState(Enemy.PATROL_IDLE_STATE);
		Vector3 offset = target - entityTransform.position;
		mNavMeshAgent.Resume();
		mNavMeshAgent.Move(offset);
	}

	public override void StartPatrolIdle()
	{
		SetState(Enemy.PATROL_IDLE_STATE);
	}

	public override void DoPatrolIdle()
	{
		ChooseTargetPlayer(false);
		LookAtTargetHorizontal();
		PlayAnimation(AnimationString.WORM_DRILLIN, WrapMode.ClampForever);
		if (!base.IsMasterPlayer || !mPatrolTimer.Ready() || mTarget == null)
		{
			return;
		}
		Vector3 vector = mTargetPosition - entityTransform.position;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (magnitude < 12f)
		{
			EndPatrolIdle();
			StartDrillOutTaunt();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.WORM_DRILL_OUT_TAUNT, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartAttack01()
	{
		SetState(ATTACK01);
		PlaySound("RPG_Audio/Enemy/Worm/Worm_attack01");
	}

	public void DoAttack01()
	{
		PlayAnimation(AnimationString.WORM_MELEE_ATTACK01, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		CheckEnemyAttack(AttackData01);
		if (AnimationPlayed(AnimationString.WORM_MELEE_ATTACK01, 1f))
		{
			StartEnemyIdle();
		}
	}

	public void EndAttack01()
	{
	}

	public void StartAttack02()
	{
		SetState(ATTACK02);
		GameObject original = Resources.Load("RPG_effect/RPG_WormAttack_001") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Worm/Worm_attack02");
	}

	public void DoAttack02()
	{
		PlayAnimation(AnimationString.WORM_MELEE_ATTACK02, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		CheckEnemyAttack(AttackData02);
		if (AnimationPlayed(AnimationString.WORM_MELEE_ATTACK02, 1f))
		{
			StartEnemyIdle();
		}
	}

	public void EndAttack02()
	{
	}

	public void StartAttack03()
	{
		SetState(ATTACK03);
		GameObject original = Resources.Load("RPG_effect/RPG_WormAttack_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Worm/Worm_attack03");
	}

	public void DoAttack03()
	{
		PlayAnimation(AnimationString.WORM_MELEE_ATTACK03, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (mLocalPlayer.CheckHitTimerReady(this))
		{
			Vector3 vector = mLocalPlayer.GetTransform().position - mMiddleBodyTransform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < (float)(mRushAttackRadius * mRushAttackRadius))
			{
				mLocalPlayer.OnHit(mRushAttackDamage1, this);
				mLocalPlayer.ResetCheckHitTimer(this);
			}
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this))
			{
				Vector3 vector2 = value.GetTransform().position - mMiddleBodyTransform.position;
				vector2.y = 0f;
				if (vector2.sqrMagnitude < (float)(mRushAttackRadius * mRushAttackRadius))
				{
					value.OnHit(mRushAttackDamage1);
					value.ResetCheckHitTimer(this);
				}
			}
		}
		if (AnimationPlayed(AnimationString.WORM_MELEE_ATTACK03, 1f))
		{
			StartEnemyIdle();
		}
	}

	public void EndAttack03()
	{
	}

	public void StartDrillIn()
	{
		SetState(DRILL_IN_STATE);
		GameObject original = Resources.Load("RPG_effect/RPG_WormAppear_002") as GameObject;
		Object.Instantiate(original, mDrillOutPos, Quaternion.identity);
		Object.Destroy(mDrillOutEffect);
		mDrillOutEffect = null;
		PlaySound("RPG_Audio/Enemy/Worm/Worm_screw_in");
	}

	public void DoDrillIn()
	{
		PlayAnimation(AnimationString.WORM_DRILLIN, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_DRILLIN, 1f))
		{
			StartDrillIdle();
		}
	}

	protected void EndDrillIn()
	{
	}

	public void StartDrillOut01(Vector3 destination)
	{
		SetState(DRILL_OUT_STATE01);
		LookAtTargetHorizontal();
		mNavMeshAgent.Resume();
		mNavMeshAgent.Move(destination - GetTransform().position);
		mDeltaPosition = Vector3.zero;
		PlaySound("RPG_Audio/Enemy/Worm/Worm_screw_out");
	}

	public void DoDrillOut01()
	{
		StopNavMesh();
		PlayAnimation(AnimationString.WORM_DRILLOUT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 0.1f))
		{
			ShowDrillOutEffect();
		}
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 1f))
		{
			StartAttack01();
		}
	}

	protected void EndDirllOut01()
	{
	}

	public void StartDrillOut02(Vector3 destination)
	{
		SetState(DRILL_OUT_STATE02);
		LookAtTargetHorizontal();
		mNavMeshAgent.Resume();
		mNavMeshAgent.Move(destination - GetTransform().position);
		mDeltaPosition = Vector3.zero;
	}

	public void DoDrillOut02()
	{
		StopNavMesh();
		PlayAnimation(AnimationString.WORM_DRILLOUT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 0.1f))
		{
			ShowDrillOutEffect();
		}
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 1f))
		{
			StartAttack02();
		}
	}

	protected void EndDrillOut02()
	{
	}

	public void StartDrillOut03(Vector3 destination)
	{
		StopNavMesh();
		SetState(DRILL_OUT_STATE03);
		LookAtTargetHorizontal();
		mNavMeshAgent.Resume();
		mNavMeshAgent.Move(destination - GetTransform().position);
		mDeltaPosition = Vector3.zero;
	}

	public void DoDrillOut03()
	{
		PlayAnimation(AnimationString.WORM_DRILLOUT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 0.1f))
		{
			ShowDrillOutEffect();
		}
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 1f))
		{
			StartAttack03();
		}
	}

	protected void EndDirllOut03()
	{
	}

	public void StartDrillOutTaunt()
	{
		SetState(DRILL_OUT_TAUNT);
		LookAtTargetHorizontal();
		PlayAwakeSound();
		PlaySound("RPG_Audio/Enemy/Worm/Worm_screw_out");
	}

	public void DoDrillOutTaunt()
	{
		PlayAnimation(AnimationString.WORM_DRILLOUT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 0.1f))
		{
			ShowDrillOutEffect();
		}
		if (AnimationPlayed(AnimationString.WORM_DRILLOUT, 1f))
		{
			StartTaunt();
		}
	}

	protected void EndDirllOutTaunt()
	{
	}

	public void StartTaunt()
	{
		SetState(TAUNT);
		LookAtTargetHorizontal();
	}

	public void DoTaunt()
	{
		PlayAnimation(AnimationString.WORM_TAUNT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.WORM_TAUNT, 1f))
		{
			StartDrillIn();
		}
	}

	protected void EndTaunt()
	{
	}

	public void StartDrillIdle()
	{
		SetState(DRILL_IDLE_STATE);
		SetIdleStartTime();
	}

	public void DoDrillIdle()
	{
		if (base.IsMasterPlayer && !NeedGoBack() && GetDrillIdleTimeDuration() > MaxDrillIdleTime)
		{
			Vector3 vector = mTargetPosition - entityTransform.position;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			Vector3 vector2 = GetTransform().position + vector.normalized * (magnitude - 3f);
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			float num = Random.Range(0f, 3f);
			if (num <= 1f)
			{
				StartDrillOut01(vector2);
				enemyStateConst = EnemyStateConst.WORM_DRILL_OUT01;
			}
			else if (num <= 2f)
			{
				StartDrillOut02(vector2);
				enemyStateConst = EnemyStateConst.WORM_DRILL_OUT02;
			}
			else
			{
				StartDrillOut03(vector2);
				enemyStateConst = EnemyStateConst.WORM_DRILL_OUT03;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position, vector2);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected void EndDrillIdle()
	{
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Worm/Worm_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		GameObject original = Resources.Load("RPG_effect/RPG_WormDead_001") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	public override void EndDead()
	{
		if (mDrillOutEffect != null)
		{
			Object.Destroy(mDrillOutEffect);
			mDrillOutEffect = null;
		}
	}

	protected override void PlayAwakeSound()
	{
		PlaySound("RPG_Audio/Enemy/Worm/Worm_scream");
	}

	public override void StartAwake()
	{
		StartDrillIdle();
		mSpawnPointScript.IsAwaking = true;
		LookAtTargetHorizontal();
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Worm/Worm_attacked");
	}
}
