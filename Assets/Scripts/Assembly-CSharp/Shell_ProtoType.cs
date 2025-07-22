using System.Collections.Generic;
using UnityEngine;

public class Shell_ProtoType : Shell
{
	protected new AnimationKey mSoundMeleeAttack01a = new AnimationKey
	{
		Time = 0.7f
	};

	protected new AnimationKey mSoundMeleeAttack01b = new AnimationKey
	{
		Time = 1.666f
	};

	protected new AnimationKey mSoundSingleAttack = new AnimationKey
	{
		Time = 0.7f
	};

	protected new AnimationKey mSoundMove01 = new AnimationKey
	{
		Time = 0.1875f
	};

	protected new AnimationKey mSoundMove02 = new AnimationKey
	{
		Time = 0.375f
	};

	protected new AnimationKey mSoundMove03 = new AnimationKey
	{
		Time = 0.5625f
	};

	protected new AnimationKey mSoundMove04 = new AnimationKey
	{
		Time = 0.75f
	};

	protected new AnimationKey mSoundShell = new AnimationKey
	{
		Time = 1.7f
	};

	public new int Probability1
	{
		get
		{
			return mProbability1[(int)mMindState];
		}
	}

	public new int Probability2
	{
		get
		{
			return mProbability2[(int)mMindState];
		}
	}

	public new int Probability3
	{
		get
		{
			return mProbability3[(int)mMindState];
		}
	}

	public new int Probability4
	{
		get
		{
			return mProbability4[(int)mMindState];
		}
	}

	public new int Probability5
	{
		get
		{
			return mProbability5[(int)mMindState];
		}
	}

	public override void Init()
	{
		base.Init();
		mPushPlayerRange = 8f;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.ShellMesh;
	}

	public override void Activate()
	{
		base.Activate();
		if (mDoubleAttackTrail != null)
		{
			mDoubleAttackTrail.transform.localScale *= 0.5f;
		}
		if (mSingleAttackTrail != null)
		{
			mSingleAttackTrail.transform.localScale *= 0.5f;
		}
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		mMeleeAttackRadius *= 0.5f;
		mRushAttackRadius = Mathf.CeilToInt((float)mRushAttackRadius * 0.5f);
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
		if (!InPlayingState() || mState == Shell.SHELL_SCREW_IN || mState == Shell.SHELL_SCREW_IDLE || mState == Shell.SHELL_SCREW_OUT || !mTouchtimer.Ready())
		{
			return;
		}
		if ((mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < 16f)
		{
			mTouchtimer.Do();
			CheckKnocked(mLocalPlayer, 5f);
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if ((value.GetTransform().position - entityTransform.position).sqrMagnitude < 16f)
			{
				mTouchtimer.Do();
				CheckKnocked(value, 5f);
			}
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override Vector3 GetShieldScale()
	{
		return new Vector3(2f, 2f, 2f);
	}

	protected new void SetNavMeshForSpin()
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
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
	}

	public override void CreateNavMeshAgent()
	{
	}

	protected new void MyCreateNavMeshAgent()
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
			}
			else
			{
				gameUnit = GetFarthestTarget();
				StartShellScrewIn();
				enemyStateConst = EnemyStateConst.SHELL_SCREW_IN;
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

	protected new void StartShellInitMove()
	{
		SetState(Shell.SHELL_INIT_MOVE);
	}

	public new void DoShellInitMove()
	{
	}

	protected new void EndShellInitMove()
	{
	}

	protected override void StartShellJumpOut()
	{
		SetState(Shell.SHELL_JUMP_OUT);
		mLandEffect = false;
		mJumpOutEffect = false;
		mScrewOutEffect = false;
	}

	public override void DoShellJumpOut()
	{
		if (!mFinishJumpOut)
		{
			PlayAnimation(AnimationString.SHELL_SCREW_OUT, WrapMode.ClampForever);
			float num = animation[AnimationString.SHELL_SCREW_OUT].time / animation[AnimationString.SHELL_SCREW_OUT].clip.length;
			if (!mScrewOutEffect && num > 0.23f)
			{
				mScrewOutEffect = true;
				GameObject original = Resources.Load("RPG_effect/RPG_SpiderAppear_big") as GameObject;
				Vector3 position = entityTransform.position + 0.02f * Vector3.up;
				Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
				GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
				gameObject.transform.localScale *= 0.5f;
				PlaySound("RPG_Audio/Enemy/Shell/Shell_jump_out");
			}
			else if (!mLandEffect && num > 0.7f)
			{
				mLandEffect = true;
				PlaySound("RPG_Audio/Enemy/Shell/Shell_land");
				GameObject original2 = Resources.Load("RPG_effect/RPG_Shell_RushOut_002") as GameObject;
				GameObject gameObject2 = Object.Instantiate(original2, entityTransform.position, Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f)) as GameObject;
				gameObject2.transform.localScale *= 0.5f;
				LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				if (Mathf.Sqrt((GetPosition() - localPlayer.GetPosition()).sqrMagnitude) < 15f)
				{
				}
				localPlayer.CameraVibrateController.VibrateUntilEnd(CameraVibrateController.Direction.EarthQuake);
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

	protected new void EndShellJumpOut()
	{
	}

	public new void StartShellDoubleAttack()
	{
		SetState(Shell.SHELL_DOUBLE_ATTACK);
		LookAtTargetHorizontal();
		if (null != mDoubleAttackTrail)
		{
			mDoubleAttackTrail.SetActive(true);
		}
		mSoundMeleeAttack01a.Reset();
		mSoundMeleeAttack01b.Reset();
	}

	public new void DoShellDoubleAttack()
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

	protected new void EndShellDoubleAttack()
	{
		if (null != mDoubleAttackTrail)
		{
			mDoubleAttackTrail.SetActive(false);
		}
	}

	public new void StartShellSpinStart()
	{
		SetState(Shell.SHELL_SPIN_START);
	}

	public new void DoShellSpinStart()
	{
		PlayAnimation(AnimationString.SHELL_SPIN_START, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.SHELL_SPIN_START, 1f))
		{
			EndShellSpinStart();
			StartShellSpin();
		}
	}

	protected new void EndShellSpinStart()
	{
	}

	protected new void StartShellSpin()
	{
		SetState(Shell.SHELL_SPIN);
		SetNavMeshForSpin();
		mSpinEffect = false;
		SetCatchingTimeNow();
	}

	public new void DoShellSpin()
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

	protected new void EndShellSpin()
	{
		StopNavMesh();
	}

	protected new void StartShellSpinEnd()
	{
		SetState(Shell.SHELL_SPIN_END);
	}

	public new void DoShellSpinEnd()
	{
		PlayAnimation(AnimationString.SHELL_SPIN_END, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_SPIN_END, 1f))
		{
			EndShellSpinEnd();
			StartEnemyIdle();
		}
	}

	protected new void EndShellSpinEnd()
	{
	}

	public new void StartShellSingleAttack()
	{
		SetState(Shell.SHELL_SINGLE_ATTACK);
		LookAtTargetHorizontal();
		if (null != mSingleAttackTrail)
		{
			mSingleAttackTrail.SetActive(true);
		}
		mSoundSingleAttack.Reset();
	}

	public new void DoShellSingleAttack()
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

	protected new void EndShellSingleAttack()
	{
		if (null != mSingleAttackTrail)
		{
			mSingleAttackTrail.SetActive(false);
		}
	}

	public new void StartShellMissileStart()
	{
		SetState(Shell.SHELL_MISSILE_START);
		mSoundShell.Reset();
	}

	public new void DoShellMissileStart()
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

	protected new void EndShellMissileStart()
	{
	}

	protected new void StartShellMissile()
	{
		SetState(Shell.SHELL_MISSILE);
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

	public new void DoShellMissile()
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

	protected new void EndShellMissile()
	{
	}

	protected new void StartShellMissileEnd()
	{
		SetState(Shell.SHELL_MISSILE_END);
		PlaySound("RPG_Audio/Enemy/Shell/Shell_open_shell");
	}

	public new void DoShellMissileEnd()
	{
		PlayAnimation(AnimationString.SHELL_MISSILE_END, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_MISSILE_END, 1f))
		{
			EndShellMissileEnd();
			StartEnemyIdle();
		}
	}

	protected new void EndShellMissileEnd()
	{
	}

	public new void StartShellScrewIn()
	{
		SetState(Shell.SHELL_SCREW_IN);
		mScrewInEffect = false;
	}

	public new void DoShellScrewIn()
	{
		PlayAnimation(AnimationString.SHELL_SCREW_IN, WrapMode.ClampForever);
		if (!mScrewInEffect && AnimationPlayed(AnimationString.SHELL_SCREW_IN, 0.41f))
		{
			mScrewInEffect = true;
			PlaySound("RPG_Audio/Enemy/Shell/Shell_screw");
			GameObject original = Resources.Load("RPG_effect/RPG_SpiderAppear_big") as GameObject;
			Vector3 position = entityTransform.position + 0.02f * Vector3.up;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
			gameObject.transform.localScale *= 0.5f;
		}
		if (AnimationPlayed(AnimationString.SHELL_SCREW_IN, 1f))
		{
			EndShellScrewIn();
			StartShellScrewIdle();
		}
	}

	protected new void EndShellScrewIn()
	{
	}

	protected new void StartShellScrewIdle()
	{
		SetState(Shell.SHELL_SCREW_IDLE);
		mScrewIdleEffect = false;
		mScrewIdleStartTime = Time.time;
	}

	public new void DoShellScrewIdle()
	{
		if (!mScrewIdleEffect && Time.time - mScrewIdleStartTime > mMaxScrewIdleTime - 2f)
		{
			mScrewIdleEffect = true;
			entityTransform.position = mTargetPosition;
			GameObject original = Resources.Load("RPG_effect/RPG_RiskTip_001") as GameObject;
			Vector3 position = mTargetPosition + 0.02f * Vector3.up;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
			gameObject.transform.localScale *= 0.5f;
		}
		else if (Time.time - mScrewIdleStartTime > mMaxScrewIdleTime)
		{
			EndShellScrewIdle();
			StartShellScrewOut();
		}
	}

	protected new void EndShellScrewIdle()
	{
	}

	protected new void StartShellScrewOut()
	{
		SetState(Shell.SHELL_SCREW_OUT);
		mScrewOutEffect = false;
		mLandEffect = false;
	}

	public new void DoShellScrewOut()
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
			GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
			gameObject.transform.localScale *= 0.5f;
			PlaySound("RPG_Audio/Enemy/Shell/Shell_jump_out");
		}
		else if (!mLandEffect && num > 0.7f)
		{
			mLandEffect = true;
			PlaySound("RPG_Audio/Enemy/Shell/Shell_land");
			GameObject original2 = Resources.Load("RPG_effect/RPG_Shell_RushOut_002") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original2, entityTransform.position, Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f)) as GameObject;
			gameObject2.transform.localScale *= 0.5f;
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

	protected new void EndShellScrewOut()
	{
	}

	public new void StartShellTaunt()
	{
		SetState(Shell.SHELL_TAUNT);
		PlaySound("RPG_Audio/Enemy/Shell/Shell_taunt");
	}

	public new void DoShellTaunt()
	{
		PlayAnimation(AnimationString.SHELL_TAUNT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.SHELL_TAUNT, 1f))
		{
			EndShellTaunt();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected new void EndShellTaunt()
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
		return mState == Shell.SHELL_SCREW_OUT;
	}
}
