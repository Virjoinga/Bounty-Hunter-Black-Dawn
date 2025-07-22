using System.Collections.Generic;
using UnityEngine;

public class Shell : EnemyBoss
{
	public static EnemyState SHELL_DOUBLE_ATTACK = new ShellDoubleAttackState();

	public static EnemyState SHELL_INIT_MOVE = new ShellInitMoveState();

	public static EnemyState SHELL_JUMP_OUT = new ShellJumpOutState();

	public static EnemyState SHELL_MISSILE_END = new ShellMissileEndState();

	public static EnemyState SHELL_MISSILE_START = new ShellMissileStartState();

	public static EnemyState SHELL_MISSILE = new ShellMissileState();

	public static EnemyState SHELL_SCREW_IN = new ShellScrewInState();

	public static EnemyState SHELL_SCREW_IDLE = new ShellScrewIdleState();

	public static EnemyState SHELL_SCREW_OUT = new ShellScrewOutState();

	public static EnemyState SHELL_SINGLE_ATTACK = new ShellSingleAttackState();

	public static EnemyState SHELL_SPIN_END = new ShellSpinEndState();

	public static EnemyState SHELL_SPIN_START = new ShellSpinStartState();

	public static EnemyState SHELL_SPIN = new ShellSpinState();

	public static EnemyState SHELL_TAUNT = new ShellTauntState();

	protected int[] mProbability1 = new int[2];

	protected int[] mProbability2 = new int[2];

	protected int[] mProbability3 = new int[2];

	protected int[] mProbability4 = new int[2];

	protected int[] mProbability5 = new int[2];

	protected new float mGroupAttackDistance;

	protected Vector3 mSpinTarget;

	protected GameObject mDoubleAttackTrail;

	protected GameObject mSingleAttackTrail;

	protected ObjectPool mSpinSparkPool;

	protected Vector3 mSpinSparkOffset;

	protected Vector3 mMissileOffset;

	protected int mCurrentMissileCount;

	protected Collider mLeftArmCollider;

	protected Collider mRightArmCollider;

	protected Timer mAttackTimer = new Timer();

	protected float mScrewIdleStartTime;

	protected float mMaxScrewIdleTime;

	protected bool mSpinEffect;

	protected bool mScrewInEffect;

	protected bool mScrewIdleEffect;

	protected bool mScrewOutEffect;

	protected bool mJumpOutEffect;

	protected bool mLandEffect;

	protected bool mFinishJumpOut;

	protected float mPushPlayerRange = 16f;

	protected AnimationKey mSoundMeleeAttack01a = new AnimationKey
	{
		Time = 0.7f
	};

	protected AnimationKey mSoundMeleeAttack01b = new AnimationKey
	{
		Time = 1.666f
	};

	protected AnimationKey mSoundSingleAttack = new AnimationKey
	{
		Time = 0.7f
	};

	protected AnimationKey mSoundMove01 = new AnimationKey
	{
		Time = 0.1875f
	};

	protected AnimationKey mSoundMove02 = new AnimationKey
	{
		Time = 0.375f
	};

	protected AnimationKey mSoundMove03 = new AnimationKey
	{
		Time = 0.5625f
	};

	protected AnimationKey mSoundMove04 = new AnimationKey
	{
		Time = 0.75f
	};

	protected AnimationKey mSoundShell = new AnimationKey
	{
		Time = 1.7f
	};

	public int Probability1
	{
		get
		{
			return mProbability1[(int)mMindState];
		}
	}

	public int Probability2
	{
		get
		{
			return mProbability2[(int)mMindState];
		}
	}

	public int Probability3
	{
		get
		{
			return mProbability3[(int)mMindState];
		}
	}

	public int Probability4
	{
		get
		{
			return mProbability4[(int)mMindState];
		}
	}

	public int Probability5
	{
		get
		{
			return mProbability5[(int)mMindState];
		}
	}

	public override void Init()
	{
		base.Init();
		mShadowPath = string.Empty;
		mSpinSparkOffset = new Vector3(-1f, 0.05f, 4f);
		mMissileOffset = new Vector3(0f, 5f, 0f);
		mAttackTimer.SetTimer(2f, false);
		mShieldType = ShieldType.MECHANICAL;
		mTouchtimer.SetTimer(2f, false);
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.ShellMesh;
	}

	public override void Activate()
	{
		base.Activate();
		mFinishJumpOut = false;
		mLeftArmCollider = entityObject.transform.Find(BoneName.ShellLeftArm).gameObject.GetComponent<Collider>();
		mRightArmCollider = entityObject.transform.Find(BoneName.ShellRightArm).gameObject.GetComponent<Collider>();
		GameObject original = Resources.Load("RPG_effect/RPG_shell_attack_002") as GameObject;
		mDoubleAttackTrail = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		mDoubleAttackTrail.transform.parent = entityTransform;
		mDoubleAttackTrail.transform.localPosition = Vector3.zero;
		mDoubleAttackTrail.transform.localRotation = Quaternion.identity;
		mDoubleAttackTrail.SetActive(false);
		GameObject original2 = Resources.Load("RPG_effect/RPG_shell_attack_003") as GameObject;
		mSingleAttackTrail = Object.Instantiate(original2, Vector3.zero, Quaternion.identity) as GameObject;
		mSingleAttackTrail.transform.parent = entityTransform;
		mSingleAttackTrail.transform.localPosition = Vector3.zero;
		mSingleAttackTrail.transform.localRotation = Quaternion.identity;
		mSingleAttackTrail.SetActive(false);
		GameObject prefab = Resources.Load("RPG_effect/RPG_shell_attack_001") as GameObject;
		mSpinSparkPool = new ObjectPool();
		mSpinSparkPool.Init("effect/RPG_shell_attack_001", prefab, 6, 1f);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		if (mSpinSparkPool != null)
		{
			mSpinSparkPool.DestroyObject();
			mSpinSparkPool = null;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.SHELL_SLIME);
		GameObject[] array2 = array;
		foreach (GameObject obj in array2)
		{
			Object.DestroyObject(obj);
		}
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		mGroupAttackDistance = 12f;
		mMaxScrewIdleTime = 0.36f;
		mMaxCatchingTime[0] = 5f;
		mMaxCatchingTime[1] = 4f;
		mProbability1[0] = 70;
		mProbability1[1] = 50;
		mProbability2[0] = 25;
		mProbability3[0] = 70;
		mProbability2[1] = 25;
		mProbability3[1] = 70;
		mProbability4[0] = 30;
		mProbability4[1] = 30;
		mProbability5[0] = 20;
		mProbability5[1] = 10;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.ShellHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	private void CheckTouchPlayer()
	{
		if (!InPlayingState() || mState == SHELL_SCREW_IN || mState == SHELL_SCREW_IDLE || mState == SHELL_SCREW_OUT || !mTouchtimer.Ready())
		{
			return;
		}
		if ((mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < mPushPlayerRange)
		{
			mTouchtimer.Do();
			CheckKnocked(mLocalPlayer, 5f);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if ((value.GetTransform().position - entityTransform.position).sqrMagnitude < mPushPlayerRange)
			{
				mTouchtimer.Do();
				CheckKnocked(value, 5f);
			}
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		CheckTouchPlayer();
		if (mIsActive)
		{
			mSpinSparkPool.AutoDestruct();
		}
	}

	public override Vector3 GetShieldScale()
	{
		return new Vector3(2f, 2f, 2f);
	}

	protected void SetNavMeshForSpin()
	{
		mSpinTarget = mTargetPosition;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mSpinTarget);
			mNavMeshAgent.speed = mRushAttackSpeed1;
			SetCanTurn(false);
		}
	}

	public override void StartSeeBoss()
	{
		base.StartSeeBoss();
		if (null != mDoubleAttackTrail)
		{
			mDoubleAttackTrail.SetActive(false);
		}
		if (null != mSingleAttackTrail)
		{
			mSingleAttackTrail.SetActive(false);
		}
		EndBossInit();
		StartShellJumpOut();
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == SHELL_DOUBLE_ATTACK)
		{
			EndShellDoubleAttack();
		}
		else if (mState == SHELL_INIT_MOVE)
		{
			EndShellInitMove();
		}
		else if (mState == SHELL_JUMP_OUT)
		{
			EndShellJumpOut();
		}
		else if (mState == SHELL_MISSILE_END)
		{
			EndShellMissileEnd();
		}
		else if (mState == SHELL_MISSILE_START)
		{
			EndShellMissileStart();
		}
		else if (mState == SHELL_MISSILE)
		{
			EndShellMissile();
		}
		else if (mState == SHELL_SCREW_IN)
		{
			EndShellScrewIn();
		}
		else if (mState == SHELL_SCREW_IDLE)
		{
			EndShellScrewIdle();
		}
		else if (mState == SHELL_SCREW_OUT)
		{
			EndShellScrewOut();
		}
		else if (mState == SHELL_SINGLE_ATTACK)
		{
			EndShellSingleAttack();
		}
		else if (mState == SHELL_SPIN_END)
		{
			EndShellSpinEnd();
		}
		else if (mState == SHELL_SPIN_START)
		{
			EndShellSpinStart();
		}
		else if (mState == SHELL_SPIN)
		{
			EndShellSpin();
		}
		else if (mState == SHELL_TAUNT)
		{
			EndShellTaunt();
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
			mNavMeshAgent.radius = 5f;
			mNavMeshAgent.height = 12f;
			mNavMeshAgent.baseOffset = mNavBaseOffset;
			mNavMeshAgent.speed = mRunSpeed;
			mNavMeshAgent.angularSpeed = mNavAngularSpeed;
			mNavMeshAgent.acceleration = 100000f;
			mNavMeshAgent.walkableMask = mNavWalkableMask;
			mNavMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
			mNavMeshAgent.stoppingDistance = mStoppingDistance;
			StopNavMesh();
		}
	}

	public new virtual void StartEnemyIdle()
	{
		if (base.IsMasterPlayer)
		{
			int num = Random.Range(0, 100);
			if (num < Probability5)
			{
				StartShellTaunt();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.SHELL_TAUNT, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				SetState(Enemy.IDLE_STATE);
				SetIdleTimeNow();
			}
		}
		else
		{
			SetState(Enemy.IDLE_STATE);
			SetIdleTimeNow();
		}
	}

	protected override void StartEnemyIdleWithoutResetTime()
	{
		SetState(Enemy.IDLE_STATE);
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null)
		{
			return;
		}
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		GameUnit gameUnit = GetCurrentTarget();
		if (GetIdleTimeDuration() > mIdleTime)
		{
			EndEnemyIdle();
			float averagehorizontalDistance = GetAveragehorizontalDistance();
			if (averagehorizontalDistance < mMeleeAttackRadius)
			{
				gameUnit = GetNearestTarget();
				ChangeTarget(gameUnit);
				float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
				if (nearestDistanceToTargetPlayer < mGroupAttackDistance)
				{
					StartShellDoubleAttack();
					enemyStateConst = EnemyStateConst.SHELL_DOUBLE_ATTACK;
				}
				else
				{
					int num = Random.Range(0, 100);
					if (num < Probability1)
					{
						StartShellSingleAttack();
						enemyStateConst = EnemyStateConst.SHELL_SINGLE_ATTACK;
					}
					else
					{
						StartShellDoubleAttack();
						enemyStateConst = EnemyStateConst.SHELL_DOUBLE_ATTACK;
					}
				}
			}
			else if (averagehorizontalDistance < (float)mRushAttackRadius)
			{
				gameUnit = GetRandomTarget();
				int num2 = Random.Range(0, 100);
				if (num2 < Probability2)
				{
					StartCatching();
					enemyStateConst = EnemyStateConst.CATCHING;
				}
				else if (num2 < Probability3 && canHitTargetPlayer())
				{
					StartShellMissileStart();
					enemyStateConst = EnemyStateConst.SHELL_MISSILE_START;
				}
				else
				{
					StartShellSpinStart();
					enemyStateConst = EnemyStateConst.SHELL_SPIN_START;
				}
			}
			else
			{
				gameUnit = GetFarthestTarget();
				int num3 = Random.Range(0, 100);
				if (num3 < Probability4 && canHitTargetPlayer())
				{
					StartShellMissileStart();
					enemyStateConst = EnemyStateConst.SHELL_MISSILE_START;
				}
				else
				{
					StartShellScrewIn();
					enemyStateConst = EnemyStateConst.SHELL_SCREW_IN;
				}
			}
		}
		if (enemyStateConst == EnemyStateConst.NO_STATE)
		{
			return;
		}
		ChangeTarget(gameUnit);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (gameUnit != null)
			{
				EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(base.PointID, base.EnemyID, gameUnit);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
	}

	protected override void PlayRunningSound()
	{
		float currentTime = animation[AnimationString.ENEMY_RUN].time % animation[AnimationString.ENEMY_RUN].length;
		if (mSoundMove01.IsTrigger(currentTime) || mSoundMove02.IsTrigger(currentTime) || mSoundMove03.IsTrigger(currentTime) || mSoundMove04.IsTrigger(currentTime))
		{
			PlaySound("RPG_Audio/Enemy/Shell/Shell_move");
		}
	}

	public override void StartCatching()
	{
		base.StartCatching();
		SetCatchingTimeNow();
		mSoundMove01.Reset();
		mSoundMove02.Reset();
		mSoundMove03.Reset();
		mSoundMove04.Reset();
	}

	protected override void MakeDecisionInCatching()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null)
		{
			return;
		}
		if (GetCatchingTimeDuration() > base.MaxCatchingTime)
		{
			EndCatching();
			StartEnemyIdleWithoutResetTime();
		}
		else
		{
			if (!(GetHorizontalSqrDistanceFromTarget() < mMeleeAttackRadius * mMeleeAttackRadius))
			{
				return;
			}
			EndCatching();
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
			if (nearestDistanceToTargetPlayer < mGroupAttackDistance)
			{
				StartShellDoubleAttack();
				enemyStateConst = EnemyStateConst.SHELL_DOUBLE_ATTACK;
			}
			else
			{
				int num = Random.Range(0, 100);
				if (num < Probability1)
				{
					StartShellSingleAttack();
					enemyStateConst = EnemyStateConst.SHELL_SINGLE_ATTACK;
				}
				else
				{
					StartShellDoubleAttack();
					enemyStateConst = EnemyStateConst.SHELL_DOUBLE_ATTACK;
				}
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected void StartShellInitMove()
	{
		SetState(SHELL_INIT_MOVE);
	}

	public void DoShellInitMove()
	{
	}

	protected void EndShellInitMove()
	{
	}

	protected virtual void StartShellJumpOut()
	{
		SetState(SHELL_JUMP_OUT);
		mLandEffect = false;
		mJumpOutEffect = false;
	}

	public virtual void DoShellJumpOut()
	{
		if (!mFinishJumpOut)
		{
			PlayAnimation(AnimationString.SHELL_JUMP_OUT, WrapMode.ClampForever);
			entityTransform.Translate(entityTransform.forward * 10f * Time.deltaTime, Space.World);
			float num = animation[AnimationString.SHELL_JUMP_OUT].time / animation[AnimationString.SHELL_JUMP_OUT].clip.length;
			if (num > 0.33f && !mJumpOutEffect)
			{
				mJumpOutEffect = true;
				PlaySound("RPG_Audio/Enemy/Shell/Shell_jump_out");
				GameObject original = Resources.Load("RPG_effect/RPG_Shell_RushOut_001") as GameObject;
				Object.Instantiate(original, entityTransform.position, entityTransform.rotation);
			}
			if (num > 0.66f && !mLandEffect)
			{
				mLandEffect = true;
				PlaySound("RPG_Audio/Enemy/Shell/Shell_land");
				GameObject original2 = Resources.Load("RPG_effect/RPG_Shell_RushOut_002") as GameObject;
				Object.Instantiate(original2, entityTransform.position, Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f));
			}
			if (num > 1f)
			{
				MyCreateNavMeshAgent();
				mFinishJumpOut = true;
			}
		}
		else
		{
			PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		}
	}

	protected void EndShellJumpOut()
	{
	}

	public void StartShellDoubleAttack()
	{
		SetState(SHELL_DOUBLE_ATTACK);
		LookAtTargetHorizontal();
		if (null != mDoubleAttackTrail)
		{
			mDoubleAttackTrail.SetActive(true);
		}
		mSoundMeleeAttack01a.Reset();
		mSoundMeleeAttack01b.Reset();
	}

	public void DoShellDoubleAttack()
	{
		PlayAnimation(AnimationString.SHELL_DOUBLE_ATTACK, WrapMode.ClampForever);
		float num = animation[AnimationString.SHELL_DOUBLE_ATTACK].time / animation[AnimationString.SHELL_DOUBLE_ATTACK].clip.length;
		if (mLocalPlayer.CheckHitTimerReady(this) && num > 0.3f && num < 0.4f)
		{
			if (mRightArmCollider.bounds.Intersects(mLocalPlayer.GetPlayerCollider().bounds))
			{
				mLocalPlayer.OnHit(mMeleeAttackDamage1, this);
				CheckKnocked(mLocalPlayer, 20f);
				mLocalPlayer.ResetCheckHitTimer(this);
			}
		}
		else if (mLocalPlayer.CheckHitTimerReady(this) && num > 0.7f && num < 0.8f && mLeftArmCollider.bounds.Intersects(mLocalPlayer.GetPlayerCollider().bounds))
		{
			mLocalPlayer.OnHit(mMeleeAttackDamage1, this);
			CheckKnocked(mLocalPlayer, 20f);
			mLocalPlayer.ResetCheckHitTimer(this);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && num > 0.3f && num < 0.4f)
			{
				if (mLeftArmCollider.bounds.Intersects(value.GetHitCheckCollider().bounds))
				{
					value.OnHit(mMeleeAttackDamage1);
					CheckKnocked(value, 20f);
					value.ResetCheckHitTimer(this);
				}
			}
			else if (value.CheckHitTimerReady(this) && num > 0.7f && num < 0.8f && mLeftArmCollider.bounds.Intersects(value.GetHitCheckCollider().bounds))
			{
				value.OnHit(mMeleeAttackDamage1);
				CheckKnocked(value, 20f);
				value.ResetCheckHitTimer(this);
			}
		}
		if (AnimationPlayed(AnimationString.SHELL_DOUBLE_ATTACK, 1f))
		{
			EndShellDoubleAttack();
			StartEnemyIdle();
		}
		float time = animation[AnimationString.SHELL_DOUBLE_ATTACK].time;
		if (mSoundMeleeAttack01a.IsTrigger(time) || mSoundMeleeAttack01b.IsTrigger(time))
		{
			PlaySound("RPG_Audio/Enemy/Shell/Shell_attack01");
		}
	}

	protected void EndShellDoubleAttack()
	{
		if (null != mDoubleAttackTrail)
		{
			mDoubleAttackTrail.SetActive(false);
		}
	}

	public void StartShellSpinStart()
	{
		SetState(SHELL_SPIN_START);
	}

	public void DoShellSpinStart()
	{
		PlayAnimation(AnimationString.SHELL_SPIN_START, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.SHELL_SPIN_START, 1f))
		{
			EndShellSpinStart();
			StartShellSpin();
		}
	}

	protected void EndShellSpinStart()
	{
	}

	protected void StartShellSpin()
	{
		SetState(SHELL_SPIN);
		SetNavMeshForSpin();
		mSpinEffect = false;
		SetCatchingTimeNow();
	}

	public void DoShellSpin()
	{
		PlayAnimation(AnimationString.SHELL_SPIN, WrapMode.Loop);
		if (animation[AnimationString.SHELL_SPIN].time > animation[AnimationString.SHELL_SPIN].clip.length)
		{
			animation[AnimationString.SHELL_SPIN].time -= animation[AnimationString.SHELL_SPIN].clip.length;
			mSpinEffect = false;
		}
		else if (!mSpinEffect && animation[AnimationString.SHELL_SPIN].time > animation[AnimationString.SHELL_SPIN].clip.length / 2f)
		{
			Vector3 lookAtRotation = mSpinTarget - entityTransform.position;
			mSpinEffect = true;
			Vector3 position = entityTransform.position + entityTransform.up * mSpinSparkOffset.y;
			mSpinSparkPool.CreateObject(position, lookAtRotation, Quaternion.identity);
		}
		if (mLocalPlayer.CheckHitTimerReady(this) && (mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < 36f)
		{
			mLocalPlayer.OnHit(mRushAttackDamage1, this);
			CheckKnocked(mLocalPlayer, 20f);
			mLocalPlayer.ResetCheckHitTimer(this);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && (value.GetTransform().position - entityTransform.position).sqrMagnitude < 36f)
			{
				value.OnHit(mRushAttackDamage1);
				CheckKnocked(value, 20f);
				value.ResetCheckHitTimer(this);
			}
		}
		if (GetCatchingTimeDuration() > base.MaxCatchingTime || (entityTransform.position - mSpinTarget).sqrMagnitude < 25f)
		{
			EndShellSpin();
			StartShellSpinEnd();
		}
		PlaySoundSingle("RPG_Audio/Enemy/Shell/Shell_attack02");
	}

	protected void EndShellSpin()
	{
		StopNavMesh();
	}

	protected void StartShellSpinEnd()
	{
		SetState(SHELL_SPIN_END);
	}

	public void DoShellSpinEnd()
	{
		PlayAnimation(AnimationString.SHELL_SPIN_END, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_SPIN_END, 1f))
		{
			EndShellSpinEnd();
			StartEnemyIdle();
		}
	}

	protected void EndShellSpinEnd()
	{
	}

	public void StartShellSingleAttack()
	{
		SetState(SHELL_SINGLE_ATTACK);
		LookAtTargetHorizontal();
		if (null != mSingleAttackTrail)
		{
			mSingleAttackTrail.SetActive(true);
		}
		mSoundSingleAttack.Reset();
	}

	public void DoShellSingleAttack()
	{
		PlayAnimation(AnimationString.SHELL_SINGLE_ATTACK, WrapMode.ClampForever);
		float num = animation[AnimationString.SHELL_SINGLE_ATTACK].time / animation[AnimationString.SHELL_SINGLE_ATTACK].clip.length;
		if (mLocalPlayer.CheckHitTimerReady(this) && num > 0.45f && num < 0.6f && (mRightArmCollider.bounds.Intersects(mLocalPlayer.GetPlayerCollider().bounds) || mLeftArmCollider.bounds.Intersects(mLocalPlayer.GetPlayerCollider().bounds)))
		{
			mLocalPlayer.OnHit(mMeleeAttackDamage1, this);
			CheckKnocked(mLocalPlayer, 20f);
			mLocalPlayer.ResetCheckHitTimer(this);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && num > 0.45f && num < 0.6f && (mRightArmCollider.bounds.Intersects(value.GetHitCheckCollider().bounds) || mLeftArmCollider.bounds.Intersects(value.GetHitCheckCollider().bounds)))
			{
				value.OnHit(mMeleeAttackDamage1);
				CheckKnocked(value, 20f);
				value.ResetCheckHitTimer(this);
			}
		}
		if (AnimationPlayed(AnimationString.SHELL_SINGLE_ATTACK, 1f))
		{
			EndShellSingleAttack();
			StartEnemyIdle();
		}
		if (mSoundSingleAttack.IsTrigger(animation[AnimationString.SHELL_SINGLE_ATTACK].time))
		{
			PlaySound("RPG_Audio/Enemy/Shell/Shell_attack01");
		}
	}

	protected void EndShellSingleAttack()
	{
		if (null != mSingleAttackTrail)
		{
			mSingleAttackTrail.SetActive(false);
		}
	}

	public void StartShellMissileStart()
	{
		SetState(SHELL_MISSILE_START);
		mSoundShell.Reset();
	}

	public void DoShellMissileStart()
	{
		PlayAnimation(AnimationString.SHELL_MISSILE_START, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.SHELL_MISSILE_START, 1f))
		{
			EndShellMissileStart();
			StartShellMissile();
		}
		if (mSoundShell.IsTrigger(animation[AnimationString.SHELL_MISSILE_START].time))
		{
			PlaySound("RPG_Audio/Enemy/Shell/Shell_open_shell");
		}
	}

	protected void EndShellMissileStart()
	{
	}

	protected void StartShellMissile()
	{
		SetState(SHELL_MISSILE);
		mCurrentMissileCount = 0;
		mIsShoot = false;
	}

	private void ShotMissile()
	{
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		Vector3 position = entityTransform.position + entityTransform.right * mMissileOffset.x + entityTransform.up * mMissileOffset.y + entityTransform.forward * mMissileOffset.z;
		GameObject original = Resources.Load("RPG_effect/RPG_shell_rocket_001") as GameObject;
		foreach (Player item in potentialPlayerList)
		{
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
			ShellMissileScript component = gameObject.GetComponent<ShellMissileScript>();
			if (null != component)
			{
				Vector3 normalized = (entityTransform.forward + 5f * Vector3.up).normalized;
				component.mEnemy = this;
				component.mRisingSpeed = mRangedBulletSpeed1 * normalized;
				component.mRisingTime = 0.64f;
				component.mTarget = item;
				component.mTargetPosition = mTargetPosition;
				component.mExplosionDamage = mRangedExtraDamage1;
				component.mAttackSpeedValue = mRangedBulletSpeed2;
				component.mExplosionRadius = mRangedExplosionRadius1;
				component.mHasSlime = true;
				component.mSlimeDamageInterval = mRangedInterval2;
				component.mSlimeDamage = mRangedExtraDamage2;
				component.mSlimeDuration = mRangedOneShotTime2;
			}
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_shell_fire_001") as GameObject;
		Object.Instantiate(original2, position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/Shell/Shell_fire");
	}

	public void DoShellMissile()
	{
		PlayAnimation(AnimationString.SHELL_MISSILE, WrapMode.Loop);
		if (!mIsShoot)
		{
			ShotMissile();
			mIsShoot = true;
		}
		if (mIsShoot && animation[AnimationString.SHELL_MISSILE].time > animation[AnimationString.SHELL_MISSILE].clip.length)
		{
			animation[AnimationString.SHELL_MISSILE].time -= animation[AnimationString.SHELL_MISSILE].clip.length;
			mCurrentMissileCount++;
			mIsShoot = false;
		}
		if (mCurrentMissileCount >= mRangedBulletCount1)
		{
			EndShellMissile();
			StartShellMissileEnd();
		}
	}

	protected void EndShellMissile()
	{
	}

	protected void StartShellMissileEnd()
	{
		SetState(SHELL_MISSILE_END);
		PlaySound("RPG_Audio/Enemy/Shell/Shell_open_shell");
	}

	public void DoShellMissileEnd()
	{
		PlayAnimation(AnimationString.SHELL_MISSILE_END, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_MISSILE_END, 1f))
		{
			EndShellMissileEnd();
			StartEnemyIdle();
		}
	}

	protected void EndShellMissileEnd()
	{
	}

	public void StartShellScrewIn()
	{
		SetState(SHELL_SCREW_IN);
		mScrewInEffect = false;
	}

	public void DoShellScrewIn()
	{
		PlayAnimation(AnimationString.SHELL_SCREW_IN, WrapMode.ClampForever);
		if (!mScrewInEffect && AnimationPlayed(AnimationString.SHELL_SCREW_IN, 0.41f))
		{
			mScrewInEffect = true;
			PlaySound("RPG_Audio/Enemy/Shell/Shell_screw");
			GameObject original = Resources.Load("RPG_effect/RPG_SpiderAppear_big") as GameObject;
			Vector3 position = entityTransform.position + 0.02f * Vector3.up;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			Object.Instantiate(original, position, rotation);
		}
		if (AnimationPlayed(AnimationString.SHELL_SCREW_IN, 1f))
		{
			EndShellScrewIn();
			StartShellScrewIdle();
		}
	}

	protected void EndShellScrewIn()
	{
	}

	protected void StartShellScrewIdle()
	{
		SetState(SHELL_SCREW_IDLE);
		mScrewIdleEffect = false;
		mScrewIdleStartTime = Time.time;
	}

	public void DoShellScrewIdle()
	{
		if (!mScrewIdleEffect && Time.time - mScrewIdleStartTime > mMaxScrewIdleTime - 2f)
		{
			mScrewIdleEffect = true;
			entityTransform.position = mTargetPosition;
			GameObject original = Resources.Load("RPG_effect/RPG_RiskTip_001") as GameObject;
			Vector3 position = mTargetPosition + 0.02f * Vector3.up;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			Object.Instantiate(original, position, rotation);
		}
		else if (Time.time - mScrewIdleStartTime > mMaxScrewIdleTime)
		{
			EndShellScrewIdle();
			StartShellScrewOut();
		}
	}

	protected void EndShellScrewIdle()
	{
	}

	protected void StartShellScrewOut()
	{
		SetState(SHELL_SCREW_OUT);
		mScrewOutEffect = false;
		mLandEffect = false;
	}

	public void DoShellScrewOut()
	{
		PlayAnimation(AnimationString.SHELL_SCREW_OUT, WrapMode.ClampForever);
		float num = animation[AnimationString.SHELL_SCREW_OUT].time / animation[AnimationString.SHELL_SCREW_OUT].clip.length;
		if (((mLocalPlayer.CheckHitTimerReady(this) && num > 0.23f && num < 0.3f) || num > 0.7f) && (mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < 25f)
		{
			mLocalPlayer.OnHit(mRushAttackDamage2, this);
			CheckKnocked(mLocalPlayer, 20f);
			mLocalPlayer.ResetCheckHitTimer(this);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (((value.CheckHitTimerReady(this) && num > 0.23f && num < 0.3f) || num > 0.7f) && (value.GetTransform().position - entityTransform.position).sqrMagnitude < 25f)
			{
				value.OnHit(mRushAttackDamage2);
				CheckKnocked(value, 20f);
				value.ResetCheckHitTimer(this);
			}
		}
		if (!mScrewOutEffect && num > 0.23f)
		{
			mScrewOutEffect = true;
			GameObject original = Resources.Load("RPG_effect/RPG_SpiderAppear_big") as GameObject;
			Vector3 position = entityTransform.position + 0.02f * Vector3.up;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			Object.Instantiate(original, position, rotation);
			PlaySound("RPG_Audio/Enemy/Shell/Shell_jump_out");
		}
		else if (!mLandEffect && num > 0.7f)
		{
			mLandEffect = true;
			PlaySound("RPG_Audio/Enemy/Shell/Shell_land");
			GameObject original2 = Resources.Load("RPG_effect/RPG_Shell_RushOut_002") as GameObject;
			Object.Instantiate(original2, entityTransform.position, Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f));
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (Mathf.Sqrt((GetPosition() - localPlayer.GetPosition()).sqrMagnitude) < 15f)
			{
			}
			localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
		}
		else if (num > 1f)
		{
			EndShellScrewOut();
			StartEnemyIdle();
		}
	}

	protected void EndShellScrewOut()
	{
	}

	public void StartShellTaunt()
	{
		SetState(SHELL_TAUNT);
		PlaySound("RPG_Audio/Enemy/Shell/Shell_taunt");
	}

	public void DoShellTaunt()
	{
		PlayAnimation(AnimationString.SHELL_TAUNT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_TAUNT, 1f))
		{
			EndShellTaunt();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected void EndShellTaunt()
	{
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Shell/Shell_attacked");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Shell/Shell_dead");
	}

	protected override bool IsJumping()
	{
		return mState == SHELL_SCREW_OUT;
	}
}
