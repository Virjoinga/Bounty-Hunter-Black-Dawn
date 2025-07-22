using System.Collections.Generic;
using UnityEngine;

public class Hati : Enemy
{
	public static EnemyState HATI_JUMP_STATE = new HatiJumpState();

	protected Timer mTryJumpTimer = new Timer();

	protected MeleeAttackData mMeleeAttackData = default(MeleeAttackData);

	public override void Init()
	{
		base.Init();
		mTryJumpTimer.SetTimer(2f, false);
		mRunAudioTimer.SetTimer(0.4f, true);
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.HatiMesh;
	}

	public override void Activate()
	{
		base.Activate();
		mMeleeAttackData.Animation = AnimationString.ENEMY_ATTACK;
		mMeleeAttackData.Trans = entityTransform;
		mMeleeAttackData.StartPercent = 0.4f;
		mMeleeAttackData.EndPercent = 1f;
		mMeleeAttackData.Range = mMeleeAttackRadius;
		mMeleeAttackData.Angle = 60f;
		mMeleeAttackData.Damage = mMeleeAttackDamage1;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.HatiHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (mIsActive)
		{
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (HATI_JUMP_STATE == mState)
		{
			EndHatiJump();
		}
	}

	protected override void PlayRunningSound()
	{
		if (mRunAudioTimer.Ready())
		{
			float num = animation[AnimationString.ENEMY_RUN].time / animation[AnimationString.ENEMY_RUN].clip.length;
			num -= Mathf.Floor(num);
			if (num < 0.1f)
			{
				mRunAudioTimer.Do();
				PlaySound("RPG_Audio/Enemy/Hati/Hati_run");
			}
		}
	}

	protected override void MakeDecisionInCatching()
	{
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
		{
			EndCatching();
			StartEnemyAttack();
			enemyStateConst = EnemyStateConst.ATTACK;
		}
		else if (sqrDistanceFromTarget < 196f && sqrDistanceFromTarget > 169f && mTryJumpTimer.Ready())
		{
			mTryJumpTimer.Do();
			int num = Random.Range(0, 100);
			if (num < 50 && canHitTargetPlayer())
			{
				EndCatching();
				StartHatiJump();
				enemyStateConst = EnemyStateConst.HATI_JUMP;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void StartEnemyAttack()
	{
		base.StartEnemyAttack();
		PlaySound("RPG_Audio/Enemy/Hati/Hati_attack");
	}

	protected override void CheckEnemyAttack()
	{
		CheckEnemyAttack(mMeleeAttackData);
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
			StartEnemyAttack();
			enemyStateConst = EnemyStateConst.ATTACK;
		}
		else if (sqrDistanceFromTarget < 196f && sqrDistanceFromTarget > 169f)
		{
			if (mTryJumpTimer.Ready())
			{
				mTryJumpTimer.Do();
				int num = Random.Range(0, 100);
				if (num < 50 && canHitTargetPlayer())
				{
					EndEnemyIdle();
					StartHatiJump();
					enemyStateConst = EnemyStateConst.HATI_JUMP;
				}
			}
			else
			{
				EndEnemyIdle();
				StartCatching();
				enemyStateConst = EnemyStateConst.CATCHING;
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
		PlaySound("RPG_Audio/Enemy/Hati/Hati_attacked");
	}

	public void StartHatiJump()
	{
		SetState(HATI_JUMP_STATE);
		PlaySound("RPG_Audio/Enemy/Hati/Hati_jump");
		Vector3 vector = mTargetPosition;
		Vector3 normalized = (vector - entityTransform.position).normalized;
		vector -= 2f * normalized;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(vector);
			mNavMeshAgent.speed = 12f;
			SetCanTurn(false);
		}
	}

	public void DoHatiJump()
	{
		PlayAnimation(AnimationString.ENEMY_JUMP, WrapMode.ClampForever);
		if (mLocalPlayer.CheckHitTimerReady(this))
		{
			Vector3 vector = entityTransform.InverseTransformPoint(mLocalPlayer.GetTransform().position);
			if (Vector3.Distance(entityTransform.position, mLocalPlayer.GetTransform().position) < 3f && vector.z > 0f)
			{
				mLocalPlayer.OnHit(mMeleeAttackDamage1, this);
				mLocalPlayer.ResetCheckHitTimer(this);
			}
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && value.InPlayingState())
			{
				Vector3 vector2 = entityTransform.InverseTransformPoint(value.GetTransform().position);
				if (Vector3.Distance(entityTransform.position, value.GetTransform().position) < 3f && vector2.z > 0f)
				{
					value.OnHit(mMeleeAttackDamage1);
					value.ResetCheckHitTimer(this);
				}
			}
		}
		if (AnimationPlayed(AnimationString.ENEMY_JUMP, 1f))
		{
			EndHatiJump();
			StartEnemyIdle();
		}
	}

	protected void EndHatiJump()
	{
		StopNavMesh();
	}

	protected override bool IsJumping()
	{
		return mState == HATI_JUMP_STATE;
	}
}
