using UnityEngine;

public class Monk : Enemy
{
	public const float P_ATTACK01 = 0.1f;

	public const float P_ATTACK02 = 0.1f;

	public const float P_ATTACK03 = 0.1f;

	public const float P_CONTINUE_ATTACK03 = 0.4f;

	public const float P_RUSH_ATTACK = 0.4f;

	public const float P_DEFENSE = 0.4f;

	public const float MAX_DEFENSE2_TIME = 1f;

	public const float DEFENSE2_DELTA = 0.2f;

	public const float RUSH_ATTACK_PLAYER_DISTANCE = 2f;

	public static EnemyState ATTACK01 = new MonkAttack01State();

	public static EnemyState ATTACK02 = new MonkAttack02State();

	public static EnemyState ATTACK03 = new MonkAttack03State();

	public static EnemyState ATTACK02Con = new MonkAttack02ConState();

	public static EnemyState DEFENSE01 = new MonkDefense01State();

	public static EnemyState DEFENSE02 = new MonkDefense02State();

	public static EnemyState DEFENSE03 = new MonkDefense03State();

	public static EnemyState RUSH_ATTACK = new MonkRushAttackState();

	protected MeleeAttackData MonkMeleeAttackData01 = default(MeleeAttackData);

	protected MeleeAttackData MonkMeleeAttackData02 = default(MeleeAttackData);

	protected MeleeAttackData MonkMeleeAttackData03 = default(MeleeAttackData);

	protected MeleeAttackData MonkRunAttackData = default(MeleeAttackData);

	protected Timer mAttackTimer = new Timer();

	protected Timer mTryRushAttackTimer = new Timer();

	protected Timer mRushAttackTimer = new Timer();

	protected Timer mDefenseTimer = new Timer();

	protected AnimationKey mSoundMove1 = default(AnimationKey);

	protected AnimationKey mSoundMove2 = default(AnimationKey);

	protected AnimationKey mSoundRun1 = default(AnimationKey);

	protected AnimationKey mSoundRun2 = default(AnimationKey);

	protected Timer mSoundTimerAttack01 = new Timer();

	protected Timer mSoundTimerAttack02 = new Timer();

	protected Timer mSoundTimerAttack03 = new Timer();

	protected bool mNeedDefense;

	protected float mDefenseStartTime;

	protected GameObject mRunSmokeEffect;

	protected bool mRushAttackAnimStopped;

	public override void Init()
	{
		base.Init();
		mAttackTimer.SetTimer(2f, false);
		mTryRushAttackTimer.SetTimer(2f, false);
		mRushAttackTimer.SetTimer(2f, false);
		mDefenseTimer.SetTimer(5f, false);
		mCanAwake = true;
		mCanRage = true;
		mShieldType = ShieldType.MECHANICAL;
	}

	protected override string GetBodyMeshName()
	{
		return "gaizhuangshou";
	}

	public override void Activate()
	{
		base.Activate();
		mCanGotHit = true;
		mNeedDefense = false;
		MonkMeleeAttackData01.Animation = AnimationString.MONK_MELEE_ATTACK01;
		MonkMeleeAttackData01.Trans = entityTransform;
		MonkMeleeAttackData01.StartPercent = 0.4f;
		MonkMeleeAttackData01.EndPercent = 1f;
		MonkMeleeAttackData01.Range = mMeleeAttackRadius;
		MonkMeleeAttackData01.Angle = 60f;
		MonkMeleeAttackData01.Damage = mMeleeAttackDamage1;
		MonkMeleeAttackData02.Animation = AnimationString.MONK_MELEE_ATTACK02;
		MonkMeleeAttackData02.Trans = entityTransform;
		MonkMeleeAttackData02.StartPercent = 0.4f;
		MonkMeleeAttackData02.EndPercent = 1f;
		MonkMeleeAttackData02.Range = mMeleeAttackRadius;
		MonkMeleeAttackData02.Angle = 60f;
		MonkMeleeAttackData02.Damage = mMeleeAttackDamage2;
		MonkMeleeAttackData03.Animation = AnimationString.MONK_MELEE_ATTACK03;
		MonkMeleeAttackData03.Trans = entityTransform;
		MonkMeleeAttackData03.StartPercent = 0.4f;
		MonkMeleeAttackData03.EndPercent = 1f;
		MonkMeleeAttackData03.Range = mMeleeAttackRadius;
		MonkMeleeAttackData03.Angle = 60f;
		MonkMeleeAttackData03.Damage = mRushAttackDamage1;
		MonkRunAttackData.Animation = AnimationString.MONK_RUN_ATTACK;
		MonkRunAttackData.Trans = entityTransform;
		MonkRunAttackData.StartPercent = 0.6f;
		MonkRunAttackData.EndPercent = 1f;
		MonkRunAttackData.Range = mRushAttackRadius;
		MonkRunAttackData.Angle = 60f;
		MonkRunAttackData.Damage = mRushAttackDamage2;
		MonkRunAttackData.KnockedSpeed = 15f;
		GameObject original = Resources.Load("RPG_effect/RPG_Giant_run_smoke_001") as GameObject;
		mRunSmokeEffect = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		mRunSmokeEffect.transform.parent = entityTransform;
		mRunSmokeEffect.transform.localPosition = Vector3.zero;
		mRunSmokeEffect.transform.localRotation = Quaternion.identity;
		mRunSmokeEffect.SetActive(false);
		mSoundMove1.Time = 0.6f;
		mSoundMove2.Time = 1.3f;
		mSoundRun1.Time = 0.611f;
		mSoundRun2.Time = 0.722f;
		mSoundTimerAttack01.SetTimer(2f, false);
		mSoundTimerAttack02.SetTimer(2f, false);
		mSoundTimerAttack03.SetTimer(2f, false);
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
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

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == RUSH_ATTACK)
		{
			EndRushAttack();
		}
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mState == Enemy.CATCHING_STATE || mState == Enemy.IDLE_STATE)
		{
			mNeedDefense = true;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mState == Enemy.CATCHING_STATE || mState == Enemy.IDLE_STATE)
		{
			mNeedDefense = true;
		}
	}

	public virtual void StartEnemyAttack01()
	{
		SetState(ATTACK01);
		LookAtTargetHorizontal();
		GameObject original = Resources.Load("RPG_effect/RPG_Monk_attack_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	public virtual void DoEnemyAttack01()
	{
		PlayAnimation(AnimationString.MONK_MELEE_ATTACK01, WrapMode.ClampForever);
		CheckEnemyAttack(MonkMeleeAttackData01);
		if (mSoundTimerAttack01.Ready())
		{
			float num = animation[AnimationString.MONK_MELEE_ATTACK01].time / animation[AnimationString.MONK_MELEE_ATTACK01].length;
			if (num > 0.3f)
			{
				PlaySound("RPG_Audio/Enemy/Monk/Monk_attack01");
				mSoundTimerAttack01.Do();
			}
		}
		if (AnimationPlayed(AnimationString.MONK_MELEE_ATTACK01, 1f))
		{
			EndEnemyAttack();
			StartEnemyIdle();
		}
	}

	public virtual void StartEnemyAttack02()
	{
		SetState(ATTACK02);
		LookAtTargetHorizontal();
		GameObject original = Resources.Load("RPG_effect/RPG_Monk_attack_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Monk/Monk_attack01");
	}

	public virtual void DoEnemyAttack02()
	{
		PlayAnimation(AnimationString.MONK_MELEE_ATTACK02, WrapMode.ClampForever);
		CheckEnemyAttack(MonkMeleeAttackData02);
		if (mSoundTimerAttack02.Ready())
		{
			float num = animation[AnimationString.MONK_MELEE_ATTACK02].time / animation[AnimationString.MONK_MELEE_ATTACK02].length;
			if (num > 0.3f)
			{
				PlaySound("RPG_Audio/Enemy/Monk/Monk_attack01");
				mSoundTimerAttack02.Do();
			}
		}
		if (AnimationPlayed(AnimationString.MONK_MELEE_ATTACK02, 1f))
		{
			EndEnemyAttack();
			StartEnemyIdle();
		}
	}

	public virtual void StartEnemyAttack03()
	{
		SetState(ATTACK03);
		LookAtTargetHorizontal();
	}

	public virtual void DoEnemyAttack03()
	{
		PlayAnimation(AnimationString.MONK_MELEE_ATTACK03, WrapMode.ClampForever);
		CheckEnemyAttack(MonkMeleeAttackData03);
		if (mSoundTimerAttack03.Ready())
		{
			float num = animation[AnimationString.MONK_MELEE_ATTACK03].time / animation[AnimationString.MONK_MELEE_ATTACK03].length;
			if (num > 0.3f)
			{
				PlaySound("RPG_Audio/Enemy/Monk/Monk_attack01");
				mSoundTimerAttack03.Do();
			}
		}
		if (AnimationPlayed(AnimationString.MONK_MELEE_ATTACK03, 1f))
		{
			EndEnemyAttack();
			StartEnemyIdle();
		}
	}

	public virtual void StartEnemyAttack02Con()
	{
		SetState(ATTACK02Con);
		LookAtTargetHorizontal();
		GameObject original = Resources.Load("RPG_effect/RPG_Monk_attack_002") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Monk/Monk_attack01");
	}

	public virtual void DoEnemyAttack02Con()
	{
		PlayAnimation(AnimationString.MONK_MELEE_ATTACK02, WrapMode.ClampForever);
		CheckEnemyAttack(MonkMeleeAttackData02);
		if (AnimationPlayed(AnimationString.MONK_MELEE_ATTACK02, 1f))
		{
			StartEnemyAttack03();
		}
	}

	protected override void PlayRunningSound()
	{
		float currentTime = animation["run"].time % animation["run"].length;
		if (mSoundRun2.IsTrigger(currentTime) || mSoundRun1.IsTrigger(currentTime))
		{
			PlaySound("RPG_Audio/Enemy/Monk/Monk_run");
		}
	}

	public override void StartCatching()
	{
		base.StartCatching();
		mRunSmokeEffect.SetActive(true);
		mSoundRun1.Reset();
		mSoundRun2.Reset();
	}

	protected override void MakeDecisionInCatching()
	{
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		if (mNeedDefense && mDefenseTimer.Ready())
		{
			mNeedDefense = false;
			mDefenseTimer.Do();
			float num = Random.Range(0f, 1f);
			if (num < 0.4f)
			{
				EndCatching();
				StartDefense01();
				enemyStateConst = EnemyStateConst.MONK_DENFENSE01;
			}
		}
		else
		{
			float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
			if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
			{
				EndCatching();
				float num2 = Random.Range(0f, 0.3f);
				if (num2 <= 0.1f)
				{
					StartEnemyAttack01();
					enemyStateConst = EnemyStateConst.MONK_ATTACK_01;
				}
				else if (num2 <= 0.2f)
				{
					num2 = Random.Range(0f, 1f);
					if (num2 <= 0.4f)
					{
						enemyStateConst = EnemyStateConst.MONK_ATTACK_02CON;
						StartEnemyAttack02Con();
					}
					else
					{
						enemyStateConst = EnemyStateConst.MONK_ATTACK_02;
						StartEnemyAttack02();
					}
				}
				else
				{
					StartEnemyAttack03();
					enemyStateConst = EnemyStateConst.MONK_ATTACK_03;
				}
			}
			else if (mTryRushAttackTimer.Ready() && sqrDistanceFromTarget < (float)(mRushAttackRadius * mRushAttackRadius) && canHitTargetPlayer())
			{
				mTryRushAttackTimer.Do();
				float num3 = Random.Range(0f, 1f);
				if (num3 < 0.4f)
				{
					EndCatching();
					StartRushAttack();
					enemyStateConst = EnemyStateConst.MONK_RUSH_ATTACK;
				}
			}
		}
		mNeedDefense = false;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void EndCatching()
	{
		base.EndCatching();
		mRunSmokeEffect.SetActive(false);
	}

	public void StartRushAttack()
	{
		SetState(RUSH_ATTACK);
		Vector3 vector = mTargetPosition;
		Vector3 normalized = (vector - entityTransform.position).normalized;
		vector -= 2f * normalized;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(vector);
			mNavMeshAgent.speed = mRunSpeed;
		}
		mRushAttackAnimStopped = false;
	}

	public void DoRushAttack()
	{
		PlayAnimation(AnimationString.MONK_RUN_ATTACK, WrapMode.ClampForever);
		CheckEnemyAttack(MonkRunAttackData);
		if (AnimationPlayed(AnimationString.MONK_RUN_ATTACK, 0.5f) && !mRushAttackAnimStopped)
		{
			mRushAttackAnimStopped = true;
			StopNavMesh();
			GameObject original = Resources.Load("RPG_effect/RPG_Monk_RunAttack_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.identity);
			PlaySound("RPG_Audio/Enemy/Monk/Monk_attack02");
		}
		if (AnimationPlayed(AnimationString.MONK_RUN_ATTACK, 1f))
		{
			EndRushAttack();
			StartEnemyIdle();
		}
	}

	public void EndRushAttack()
	{
		StopNavMesh();
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (!(GetIdleTimeDuration() > mIdleTime))
		{
			return;
		}
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		if (mNeedDefense && mDefenseTimer.Ready())
		{
			mNeedDefense = false;
			mDefenseTimer.Do();
			float num = Random.Range(0f, 1f);
			if (num < 0.4f)
			{
				EndCatching();
				StartDefense01();
				enemyStateConst = EnemyStateConst.MONK_DENFENSE01;
			}
		}
		else
		{
			float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
			if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
			{
				EndEnemyIdle();
				float num2 = Random.Range(0f, 0.3f);
				if (num2 <= 0.1f)
				{
					StartEnemyAttack01();
					enemyStateConst = EnemyStateConst.MONK_ATTACK_01;
				}
				else if (num2 <= 0.2f)
				{
					num2 = Random.Range(0f, 1f);
					if (num2 <= 0.4f)
					{
						enemyStateConst = EnemyStateConst.MONK_ATTACK_02CON;
						StartEnemyAttack02Con();
					}
					else
					{
						enemyStateConst = EnemyStateConst.MONK_ATTACK_02;
						StartEnemyAttack02();
					}
				}
				else
				{
					StartEnemyAttack03();
					enemyStateConst = EnemyStateConst.MONK_ATTACK_03;
				}
			}
			else
			{
				EndEnemyIdle();
				StartCatching();
				enemyStateConst = EnemyStateConst.CATCHING;
			}
		}
		mNeedDefense = false;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void DoPatrolIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (DetectPlayer() || !base.IsMasterPlayer || !(GetIdleTimeDuration() > mPatrolIdleTime))
		{
			return;
		}
		Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		vector.Normalize();
		float num = Random.Range(10f, 20f);
		Vector3 vector2 = mSpawnPointScript.gameObject.transform.position + vector * num;
		if (TryPathForNavMesh(vector2))
		{
			StartPatrol(vector2);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.PATROL, entityTransform.position, vector2);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void StartDefense01()
	{
		SetState(DEFENSE01);
	}

	public void DoDefense01()
	{
		PlayAnimation(AnimationString.MONK_DEFENSE01, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.MONK_DEFENSE01, 1f))
		{
			StartDefense02();
		}
	}

	public void StartDefense02()
	{
		SetState(DEFENSE02);
		base.DamageReduction = 60;
		mCanGotHit = false;
		mDefenseStartTime = Time.time;
	}

	public void DoDefense02()
	{
		PlayAnimation(AnimationString.MONK_DEFENSE02, WrapMode.Loop);
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		if (base.IsMasterPlayer)
		{
			float num = Random.Range(-0.2f, 0.2f);
			if (Time.time - mDefenseStartTime > 1f + num)
			{
				EndDefense02();
				StartDefense03();
				enemyStateConst = EnemyStateConst.MONK_DENFENSE03;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void EndDefense02()
	{
		base.DamageReduction = 100;
		mCanGotHit = true;
	}

	public void StartDefense03()
	{
		SetState(DEFENSE03);
	}

	public void DoDefense03()
	{
		PlayAnimation(AnimationString.MONK_DEFENSE03, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.MONK_DEFENSE03, 1f))
		{
			StartEnemyIdle();
		}
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Monk/Monk_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		GameObject original = Resources.Load("RPG_effect/RPG_Monk_Dead_001") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	protected override void PlayAwakeSound()
	{
		PlaySound("RPG_Audio/Enemy/Monk/Monk_scream");
	}

	public override void StartPatrol(Vector3 patrolTarget)
	{
		base.StartPatrol(patrolTarget);
		mSoundMove1.Reset();
		mSoundMove2.Reset();
	}

	public override void DoPatrol()
	{
		base.DoPatrol();
		float currentTime = animation["walk"].time % animation["walk"].length;
		if (mSoundMove1.IsTrigger(currentTime) || mSoundMove2.IsTrigger(currentTime))
		{
			PlaySound("RPG_Audio/Enemy/Monk/Monk_move");
		}
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Monk/Monk_attacked");
	}

	protected override bool IsJumping()
	{
		return mState == RUSH_ATTACK;
	}
}
