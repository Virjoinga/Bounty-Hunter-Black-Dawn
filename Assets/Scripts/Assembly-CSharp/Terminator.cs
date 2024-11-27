using System;
using System.Collections.Generic;
using UnityEngine;

public class Terminator : EnemyBoss
{
	private enum EGrenadeDirection
	{
		FORWARD = 0,
		BACK = 1,
		LEFT = 2,
		RIGHT = 3,
		NUM = 4
	}

	private enum ECenterAction
	{
		LASER = 0,
		GRENADE = 1,
		FINAL = 2,
		NUM = 3
	}

	private enum EGrenadeType
	{
		AOE = 0,
		RECOVER = 1,
		NUM = 2
	}

	private const float GUN_POINT_OFFSET = 3.47f;

	private const float START_JUMP_VERTICAL_SPEED = 20f;

	private const float JUMP_GRAVITY = 20f;

	private const float LAND_GRAVITY = 40f;

	private const float ELEVATOR_DROP_DISTANCE = 12f;

	private const float ELEVATOR_SPEED = 3f;

	private const int LASER_BEAM_NUM = 3;

	private const float RECOVER_GRENADE_FORCE_VALUE = 15f;

	private const float MIN_AOE_GRENADE_FORCE_VALUE = 20f;

	private const float MAX_AOE_GRENADE_FORCE_VALUE = 30f;

	public static EnemyState TERMINATOR_STAMP = new TerminatorStampState();

	public static EnemyState TERMINATOR_MISSILE = new TerminatorMissileState();

	public static EnemyState TERMINATOR_MISSILE_READY = new TerminatorMissileReadyState();

	public static EnemyState TERMINATOR_FIRE = new TerminatorFireState();

	public static EnemyState TERMINATOR_ELEVATE = new TerminatorElevateState();

	public static EnemyState TERMINATOR_GRENADE_END = new TerminatorGrenadeEndState();

	public static EnemyState TERMINATOR_GRENADE_START = new TerminatorGrenadeStartState();

	public static EnemyState TERMINATOR_GRENADE = new TerminatorGrenadeState();

	public static EnemyState TERMINATOR_JUMP_END = new TerminatorJumpEndState();

	public static EnemyState TERMINATOR_JUMP_START = new TerminatorJumpStartState();

	public static EnemyState TERMINATOR_JUMP = new TerminatorJumpState();

	public static EnemyState TERMINATOR_LASER = new TerminatorLaserState();

	private GameEntity mBodyEntity;

	private Animation mBodyAnimation;

	private int[] mProbability1 = new int[2];

	private int[] mProbability2 = new int[2];

	private int[] mProbability3 = new int[2];

	private Vector3 mCenterPosition;

	private Vector3 mJumpSpeed;

	private Transform mBodyRotateTransform;

	private Transform mGunMuzzleTransform;

	private Transform mMissileMuzzleTransform;

	private Transform mMissileRearTransform;

	private Transform mRightHandTransform;

	private Transform[] mGrenadeTransformArray;

	private GameObject[] mElevateObjects;

	private Vector3[] mElevateOriginPositions;

	private GameObject mLaserPower;

	private GameObject mLaserSpark;

	private GameObject[] mLaserBeamArray;

	private GameObject mGunFireEffect;

	private Vector3 mMissileOffset;

	private int mCurrentMissileCount;

	private Timer mJumpCheckTimer = new Timer();

	private Timer mMoveAudioTimer = new Timer();

	private Timer mElevateTimer = new Timer();

	private bool mIsMoving;

	private bool mBodyCanTurn;

	private Vector3 mMoveTarget;

	private bool mHasElevate;

	private bool mCanStartBattle;

	private ETerminatorMissileType mMissileType;

	private float mStartMoveTime;

	private float mMaxMoveTime;

	private float mStartRestTime;

	private float mMaxRestTime;

	private float mStartJumpIdleTime;

	private float mMaxJumpIdleTime;

	private float mMissileReadyTime;

	private float mMaxMissileReadyTime;

	private float mStartLandTime;

	private bool mHasLand;

	private bool mCanMissile;

	private int mCurrentBulletCount;

	private bool mStampEffect;

	private bool mJumpEffect;

	private bool mDeadEffect;

	private bool mLaserActive;

	private bool mLaserDeactive;

	private ECenterAction mCenterAction;

	private EGrenadeType mGrenadeType;

	private float mHpTriggerForLaser;

	private float mHpTriggerForFinalBattle1;

	private float mHpTriggerForFinalBattle2;

	private int mLaserTimes;

	private float mStartGrenadeTime;

	private float mGrenadeInterval;

	private float[] mGrenadeExplodeTimes;

	private int mGrenadeNum;

	private bool mIsFinalBattle;

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

	public override void Init()
	{
		base.Init();
		mShadowPath = string.Empty;
		mNavAngularSpeed = 90f;
		mTurnSpeed = 0.05f;
		mLookAtTimer.SetTimer(2f, true);
		mMaxJumpIdleTime = 0.01f;
		mMaxMissileReadyTime = 2f;
		mMissileOffset = new Vector3(0f, 5f, 0f);
		mShieldType = ShieldType.MECHANICAL;
		mTouchtimer.SetTimer(2f, false);
		mJumpCheckTimer.SetTimer(2f, true);
		mMoveAudioTimer.SetTimer(0.8f, false);
		mElevateTimer.SetTimer(5f, false);
	}

	private void SetMoveTimeNow()
	{
		mStartMoveTime = Time.time;
	}

	private float GetMoveTimeDuration()
	{
		return Time.time - mStartMoveTime;
	}

	private void SetRestTimeNow()
	{
		mStartRestTime = Time.time;
	}

	private float GetRestTimeDuration()
	{
		return Time.time - mStartRestTime;
	}

	private void SetJumpIdleTimeNow()
	{
		mStartJumpIdleTime = Time.time;
	}

	private float GetJumpIdleTimeDuration()
	{
		return Time.time - mStartJumpIdleTime;
	}

	private void SetMissileReadyTimeNow()
	{
		mMissileReadyTime = Time.time;
	}

	private float GetMissileReadyTimeDuration()
	{
		return Time.time - mMissileReadyTime;
	}

	private void SetGrenadeTimeNow()
	{
		mStartGrenadeTime = Time.time;
	}

	private float GetGrenadeTimeDuration()
	{
		return Time.time - mStartGrenadeTime;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.ShellMesh;
	}

	private bool BodyCanTurn()
	{
		return mBodyCanTurn;
	}

	private void SetBodyCanTurn(bool canTurn)
	{
		mBodyCanTurn = canTurn;
	}

	public override void Activate()
	{
		base.Activate();
		mIsMoving = false;
		mIsFinalBattle = false;
		mCanMissile = true;
		mCenterPosition = base.SpawnPosition;
		mLaserTimes = 0;
		SetCanTurn(false);
		SetBodyCanTurn(false);
		mBodyEntity = new GameEntity();
		mBodyEntity.SetObject(entityObject.transform.Find(BoneName.TerminatorBody).gameObject);
		mBodyAnimation = mBodyEntity.GetObject().GetComponent<Animation>();
		mBodyAnimation[AnimationString.TMX_BODY_FIRE].speed = 0.2f / mRangedOneShotTime1;
		mBodyAnimation[AnimationString.TMX_BODY_MISSILE].speed = 1f / mRangedOneShotTime2;
		animation[AnimationString.TMX_LEG_MISSILE].speed = 1f / mRangedOneShotTime2;
		mBodyRotateTransform = entityObject.transform.Find(BoneName.TerminatorRotateNode);
		mRightHandTransform = entityObject.transform.Find(BoneName.TerminatorRightHand);
		GameObject original = Resources.Load("RPG_effect/RPG_TMXL_GunFire_001") as GameObject;
		mGunFireEffect = UnityEngine.Object.Instantiate(original) as GameObject;
		mGunFireEffect.name = "GunFire";
		mGunMuzzleTransform = mGunFireEffect.transform;
		mGunMuzzleTransform.parent = mBodyEntity.GetTransform();
		mGunMuzzleTransform.localPosition = new Vector3(-1.59f, -3.47f, 1.44f);
		mGunMuzzleTransform.localRotation = Quaternion.identity;
		mGunMuzzleTransform.localScale = Vector3.one;
		mGunFireEffect.SetActive(false);
		GameObject gameObject = new GameObject();
		gameObject.name = "MissileMuzzle";
		mMissileMuzzleTransform = gameObject.transform;
		mMissileMuzzleTransform.parent = entityObject.transform.Find(BoneName.TerminatorRocket);
		mMissileMuzzleTransform.localPosition = new Vector3(0f, -2.69f, 0.27f);
		mMissileMuzzleTransform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		mMissileMuzzleTransform.localScale = Vector3.one;
		GameObject gameObject2 = new GameObject();
		gameObject2.name = "MissileRear";
		mMissileRearTransform = gameObject2.transform;
		mMissileRearTransform.parent = entityObject.transform.Find(BoneName.TerminatorRocket);
		mMissileRearTransform.localPosition = new Vector3(0f, 0.43f, 0.29f);
		mMissileRearTransform.localRotation = Quaternion.identity;
		mMissileRearTransform.localScale = Vector3.one;
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_LaserLight_001") as GameObject;
		mLaserPower = UnityEngine.Object.Instantiate(original2) as GameObject;
		mLaserPower.transform.parent = mBodyEntity.GetTransform();
		mLaserPower.transform.localPosition = new Vector3(0.02f, -1.69f, 0.29f);
		mLaserPower.transform.localRotation = Quaternion.identity;
		mLaserPower.transform.localScale = Vector3.one;
		mLaserPower.SetActive(false);
		GameObject original3 = Resources.Load("RPG_effect/RPG_TMXL_LaserLight_002") as GameObject;
		mLaserSpark = UnityEngine.Object.Instantiate(original3) as GameObject;
		mLaserSpark.SetActive(false);
		GameObject original4 = Resources.Load("RPG_effect/RPG_TMXL_LaserLight_003") as GameObject;
		mLaserBeamArray = new GameObject[3];
		float[] array = new float[3] { 90f, 94f, 97f };
		for (int i = 0; i < 3; i++)
		{
			mLaserBeamArray[i] = UnityEngine.Object.Instantiate(original4) as GameObject;
			mLaserBeamArray[i].transform.parent = mBodyEntity.GetTransform();
			mLaserBeamArray[i].transform.localPosition = new Vector3(0.02f, -1.61f, 0.29f);
			mLaserBeamArray[i].transform.localRotation = Quaternion.Euler(array[i], 0f, 0f);
			mLaserBeamArray[i].transform.localScale = Vector3.one;
			mLaserBeamArray[i].SetActive(false);
			if (i > 0)
			{
				mLaserBeamArray[i].GetComponent<Collider>().isTrigger = false;
				mLaserBeamArray[i].GetComponent<Collider>().enabled = false;
			}
		}
		mGrenadeTransformArray = new Transform[4];
		GameObject gameObject3 = new GameObject();
		gameObject3.name = "ForwardGrenade";
		mGrenadeTransformArray[0] = gameObject3.transform;
		mGrenadeTransformArray[0].parent = mBodyTransform;
		mGrenadeTransformArray[0].localPosition = new Vector3(0f, 0f, 2.45f);
		mGrenadeTransformArray[0].localRotation = Quaternion.Euler(0f, 0f, -90f);
		mGrenadeTransformArray[0].localScale = Vector3.one;
		GameObject gameObject4 = new GameObject();
		gameObject4.name = "BackGrenade";
		mGrenadeTransformArray[1] = gameObject4.transform;
		mGrenadeTransformArray[1].parent = mBodyTransform;
		mGrenadeTransformArray[1].localPosition = new Vector3(0f, 0f, -0.7f);
		mGrenadeTransformArray[1].localRotation = Quaternion.Euler(0f, 180f, 90f);
		mGrenadeTransformArray[1].localScale = Vector3.one;
		GameObject gameObject5 = new GameObject();
		gameObject5.name = "LeftGrenade";
		mGrenadeTransformArray[2] = gameObject5.transform;
		mGrenadeTransformArray[2].parent = mBodyTransform;
		mGrenadeTransformArray[2].localPosition = new Vector3(0f, 1.55f, 0.65f);
		mGrenadeTransformArray[2].localRotation = Quaternion.Euler(-90f, -90f, 0f);
		mGrenadeTransformArray[2].localScale = Vector3.one;
		GameObject gameObject6 = new GameObject();
		gameObject6.name = "RightGrenade";
		mGrenadeTransformArray[3] = gameObject6.transform;
		mGrenadeTransformArray[3].parent = mBodyTransform;
		mGrenadeTransformArray[3].localPosition = new Vector3(0f, -1.55f, 0.65f);
		mGrenadeTransformArray[3].localRotation = Quaternion.Euler(90f, 90f, 0f);
		mGrenadeTransformArray[3].localScale = Vector3.one;
		animation[AnimationString.TMX_LEG_MOVE_FORWARD].speed = 0.5f;
		animation[AnimationString.TMX_LEG_MOVE_BACK].speed = 0.5f;
		animation[AnimationString.TMX_LEG_MOVE_LEFT].speed = 0.5f;
		animation[AnimationString.TMX_LEG_MOVE_RIGHT].speed = 0.5f;
		mBodyAnimation[AnimationString.TMX_BODY_LASER].speed = 0.5f;
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
		mGrenadeExplodeTimes = new float[2];
		mMaxMoveTime = 10f;
		mMaxRestTime = 5f;
		mHpTriggerForLaser = 0.2f;
		mHpTriggerForFinalBattle1 = 0.2f;
		mHpTriggerForFinalBattle2 = 0.1f;
		mGrenadeInterval = 30f;
		mGrenadeNum = 20;
		mGrenadeExplodeTimes[0] = 5f;
		mGrenadeExplodeTimes[1] = 3f;
		mProbability1[0] = 70;
		mProbability1[1] = 70;
		mProbability2[0] = 60;
		mProbability2[1] = 50;
		mProbability3[0] = 50;
		mProbability3[1] = 30;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = entityTransform.Find(BoneName.TerminatorHead);
		gameObject.transform.localPosition = new Vector3(0f, -2.3f, 3.43f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		mHeadTransform = gameObject.transform;
		mHitObjectArray = new GameObject[3];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = entityTransform.Find(BoneName.TerminatorBody).gameObject;
		mHitObjectArray[2] = mHeadTransform.gameObject;
	}

	protected override void InitRenders()
	{
	}

	private void CheckTouchPlayer()
	{
		if (!InPlayingState() || !mTouchtimer.Ready())
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

	private string GetMoveAnimation(Vector3 moveDirection)
	{
		string result = AnimationString.TMX_LEG_MOVE_FORWARD;
		float num = Vector3.Angle(moveDirection, entityTransform.forward);
		if (num > 135f)
		{
			result = AnimationString.TMX_LEG_MOVE_BACK;
		}
		else if (num > 45f)
		{
			result = ((!(Vector3.Cross(moveDirection, entityTransform.forward).y > 0f)) ? AnimationString.TMX_LEG_MOVE_RIGHT : AnimationString.TMX_LEG_MOVE_LEFT);
		}
		return result;
	}

	private string GetMoveAnimation()
	{
		string result = AnimationString.TMX_LEG_MOVE_FORWARD;
		if (null != mNavMeshAgent)
		{
			result = GetMoveAnimation(mNavMeshAgent.velocity);
		}
		return result;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (!mIsActive)
		{
			return;
		}
		if (!mIsFinalBattle && (mState == Enemy.IDLE_STATE || mState == TERMINATOR_FIRE))
		{
			if (mIsMoving)
			{
				PlayAnimation(GetMoveAnimation(), WrapMode.Loop);
				LegAdjustDirection();
				if (mMoveAudioTimer.Ready())
				{
					mMoveAudioTimer.Do();
					PlaySound("RPG_Audio/Enemy/TMX/TMX_walk");
				}
			}
			else
			{
				PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
			}
			if (base.IsMasterPlayer)
			{
				if (mIsMoving)
				{
					bool flag = false;
					if (GetMoveTimeDuration() > mMaxMoveTime)
					{
						flag = true;
					}
					else if ((entityTransform.position - mMoveTarget).sqrMagnitude < 25f)
					{
						flag = true;
					}
					if (flag)
					{
						StartTerminatorRest();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.TERMINATOR_REST, entityTransform.position);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
					}
				}
				else if (GetRestTimeDuration() > mMaxRestTime)
				{
					Vector3 averagePosition = GetAveragePosition();
					StartTerminatorMove(averagePosition);
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.TERMINATOR_MOVE, entityTransform.position, averagePosition);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
		}
		if (BodyCanTurn())
		{
			Vector3 target = mTargetToLookAt - mBodyRotateTransform.position;
			Vector3 vector = Vector3.RotateTowards(mBodyRotateTransform.forward, target, mMaxTurnRadian, 100f);
			mBodyRotateTransform.LookAt(mBodyRotateTransform.position + vector);
		}
		CheckTouchPlayer();
	}

	public override Vector3 GetShieldScale()
	{
		return new Vector3(2f, 2f, 2f);
	}

	private void BodyLookAtTargetHorizontal()
	{
		Vector3 vector = mTargetPosition;
		vector.y = mBodyRotateTransform.position.y;
		if (mLookAtTimer.Ready())
		{
			mLookAtTimer.Do();
			mTargetToLookAt = vector;
			Vector3 from = mTargetToLookAt - mBodyRotateTransform.position;
			float f = Vector3.Angle(from, mBodyRotateTransform.forward);
			float num = Mathf.Abs(f) * ((float)Math.PI / 180f);
			mMaxTurnRadian = num * mTurnSpeed;
			SetBodyCanTurn(true);
		}
	}

	private void BodyLookForward()
	{
		if (mLookAtTimer.Ready())
		{
			mLookAtTimer.Do();
			mTargetToLookAt = mBodyRotateTransform.position + entityTransform.forward;
			float f = Vector3.Angle(entityTransform.forward, mBodyRotateTransform.forward);
			float num = Mathf.Abs(f) * ((float)Math.PI / 180f);
			mMaxTurnRadian = num * mTurnSpeed;
			SetBodyCanTurn(true);
		}
	}

	private void LegAdjustDirection()
	{
		if (null != mNavMeshAgent)
		{
			Vector3 velocity = mNavMeshAgent.velocity;
			Vector3 target = velocity;
			float num = Vector3.Angle(velocity, entityTransform.forward);
			if (num > 135f)
			{
				target = -velocity;
			}
			else if (num > 45f)
			{
				target = ((!(Vector3.Cross(velocity, entityTransform.forward).y > 0f)) ? Vector3.Cross(velocity, Vector3.up) : Vector3.Cross(Vector3.up, velocity));
			}
			Vector3 vector = Vector3.RotateTowards(entityTransform.forward, target, 0.05f, 100f);
			entityTransform.LookAt(entityTransform.position + vector);
		}
	}

	public override void StartSeeBoss()
	{
		mIsActive = true;
		SetGrenadeTimeNow();
		EndBossInit();
		StartTerminatorElevate();
	}

	public override void StartBossBattle()
	{
		mCanStartBattle = true;
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		if (mMindState == MindState.NORMAL && (float)Hp < (float)base.MaxHp * (1f - mHpPercetageForRage))
		{
			mMindState = MindState.RAGE;
		}
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		if (mMindState == MindState.NORMAL && (float)Hp < (float)base.MaxHp * (1f - mHpPercetageForRage))
		{
			mMindState = MindState.RAGE;
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == TERMINATOR_MISSILE)
		{
			EndTerminatorMissile();
		}
		else if (mState == TERMINATOR_FIRE)
		{
			EndTerminatorFire();
		}
		else if (mState == TERMINATOR_ELEVATE)
		{
			EndTerminatorElevate();
		}
		else if (mState == TERMINATOR_GRENADE_END)
		{
			EndTerminatorGrenadeEnd();
		}
		else if (mState == TERMINATOR_GRENADE_START)
		{
			EndTerminatorGrenadeStart();
		}
		else if (mState == TERMINATOR_GRENADE)
		{
			EndTerminatorGrenade();
		}
		else if (mState == TERMINATOR_STAMP)
		{
			EndTerminatorStamp();
		}
		else if (mState == TERMINATOR_JUMP_END)
		{
			EndTerminatorJumpEnd();
		}
		else if (mState == TERMINATOR_JUMP_START)
		{
			EndTerminatorJumpStart();
		}
		else if (mState == TERMINATOR_JUMP)
		{
			EndTerminatorJump();
		}
		else if (mState == TERMINATOR_LASER)
		{
			EndTerminatorLaser();
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
			mNavMeshAgent.speed = mWalkSpeed;
			mNavMeshAgent.angularSpeed = mNavAngularSpeed;
			mNavMeshAgent.acceleration = 10f;
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

	protected override void PlayOnHitBloodEffect(Vector3 position, ElementType elementType)
	{
		if (!gameScene.PlayOnHitElementEffect(position, elementType))
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_WALL_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
		}
	}

	public void StartTerminatorMove(Vector3 targetPosition)
	{
		SetMoveTimeNow();
		mMoveTarget = targetPosition;
		mIsMoving = true;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mMoveTarget);
			mNavMeshAgent.speed = mWalkSpeed;
			mNavMeshAgent.updateRotation = false;
			SetCanTurn(false);
		}
	}

	public void StartTerminatorRest()
	{
		SetRestTimeNow();
		StopNavMesh();
		mIsMoving = false;
	}

	public void StartFinalBattle()
	{
		mIsFinalBattle = true;
		mMissileType = ETerminatorMissileType.NORMAL;
		StartEnemyIdle();
		StartTerminatorRest();
		PlaySound("RPG_Audio/Environment/start_fighting");
		Debug.Log("Start Final Battle!!!");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/TMX/TMX_dead");
	}

	public override void StartDead()
	{
		StartTerminatorRest();
		base.StartDead();
		mDeadEffect = false;
		GameObject original = Resources.Load("RPG_effect/RPG_Cybershoot_Die_001") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = entityTransform.Find(BoneName.TerminatorRightUpperArm);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject2.transform.parent = mBodyTransform;
		gameObject2.transform.localPosition = new Vector3(0f, -1.4f, 0.3f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		GameObject gameObject3 = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject3.transform.parent = mBodyTransform;
		gameObject3.transform.localPosition = new Vector3(-1.6f, 0.8f, 1.2f);
		gameObject3.transform.localRotation = Quaternion.identity;
		gameObject3.transform.localScale = Vector3.one;
		Debug.Log("Now_Im_the_boss");
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Now_Im_the_boss);
		AchievementManager.GetInstance().Trigger(trigger);
	}

	public override void DoDead()
	{
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		float num = animation[AnimationString.ENEMY_DEAD].time / animation[AnimationString.ENEMY_DEAD].clip.length;
		if (mOnGround)
		{
			if (!mDeadEffect && num > 0.9f)
			{
				mDeadEffect = true;
				GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Die_001") as GameObject;
				UnityEngine.Object.Instantiate(original, mBodyTransform.position, Quaternion.identity);
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

	protected override void StartEnemyIdleWithoutResetTime()
	{
		SetState(Enemy.IDLE_STATE);
	}

	public override void DoEnemyIdle()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (base.IsMasterPlayer && GetIdleTimeDuration() > mIdleTime)
		{
			MakeDecisionInEnemyIdle();
		}
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
		if (GetIdleTimeDuration() > mIdleTime)
		{
			EndEnemyIdle();
			if (mIsFinalBattle)
			{
				StartTerminatorLaser(2);
				enemyStateConst = EnemyStateConst.TERMINATOR_LASER;
			}
			else if ((float)Hp < (float)base.MaxHp * (1f - (float)(mLaserTimes + 1) * mHpTriggerForLaser))
			{
				ECenterAction eCenterAction = ECenterAction.LASER;
				float num = 400f;
				if ((float)Hp < (float)base.MaxHp * mHpTriggerForFinalBattle1)
				{
					eCenterAction = ECenterAction.FINAL;
					num = 4f;
				}
				if ((entityTransform.position - mCenterPosition).sqrMagnitude < num)
				{
					StartTerminatorLaser((byte)eCenterAction);
					enemyStateConst = EnemyStateConst.TERMINATOR_LASER;
				}
				else
				{
					StartTerminatorJumpStart((byte)eCenterAction);
					enemyStateConst = EnemyStateConst.TERMINATOR_JUMP_START;
				}
			}
			else if (GetGrenadeTimeDuration() > mGrenadeInterval)
			{
				if ((entityTransform.position - mCenterPosition).sqrMagnitude < 400f)
				{
					StartTerminatorGrenadeStart(0);
					enemyStateConst = EnemyStateConst.TERMINATOR_GRENADE_START;
				}
				else
				{
					StartTerminatorJumpStart(1);
					enemyStateConst = EnemyStateConst.TERMINATOR_JUMP_START;
				}
			}
			else
			{
				GameUnit nearestTarget = GetNearestTarget();
				if (nearestTarget != null && nearestTarget.GetTransform() != null && (nearestTarget.GetTransform().position - mRightHandTransform.position).sqrMagnitude < mMeleeAttackRadius * mMeleeAttackRadius)
				{
					int num2 = UnityEngine.Random.Range(0, 100);
					if (num2 < Probability1)
					{
						StartTerminatorStamp();
						enemyStateConst = EnemyStateConst.TERMINATOR_STAMP;
					}
				}
				if (enemyStateConst == EnemyStateConst.NO_STATE)
				{
					int num3 = UnityEngine.Random.Range(0, 100);
					if (num3 < Probability2 || !mCanMissile)
					{
						StartTerminatorFire();
						enemyStateConst = EnemyStateConst.TERMINATOR_FIRE;
						nearestTarget = GetRandomTarget();
						ChangeTarget(nearestTarget);
						if (nearestTarget != null && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(base.PointID, base.EnemyID, nearestTarget);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						}
					}
					else
					{
						int num4 = UnityEngine.Random.Range(0, 100);
						if (num3 < Probability3)
						{
							StartTerminatorMissileReady(0);
						}
						else
						{
							StartTerminatorMissileReady(1);
						}
						enemyStateConst = EnemyStateConst.TERMINATOR_MISSILE_READY;
					}
				}
			}
		}
		if (enemyStateConst != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest enemyStateRequest = null;
			switch (enemyStateConst)
			{
			case EnemyStateConst.TERMINATOR_MISSILE_READY:
				enemyStateRequest = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mMissileType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
				break;
			case EnemyStateConst.TERMINATOR_JUMP_START:
				enemyStateRequest = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mCenterAction);
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
				break;
			case EnemyStateConst.TERMINATOR_LASER:
				enemyStateRequest = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mCenterAction);
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
				break;
			case EnemyStateConst.TERMINATOR_GRENADE_START:
				enemyStateRequest = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)mGrenadeType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
				break;
			default:
				enemyStateRequest = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(enemyStateRequest);
				break;
			}
		}
	}

	protected override void StartBossInit()
	{
		SetState(EnemyBoss.INIT_STATE);
		mIsActive = false;
		mCanGotHit = false;
		entityTransform.position -= new Vector3(0f, 10000f, 0f);
	}

	public override void DoBossInit()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
	}

	private void CheckShoot()
	{
		bool hitLocalPlayer = false;
		float horizontalDistanceFromTarget = GetHorizontalDistanceFromTarget();
		Vector3 vector = entityTransform.position + mBodyRotateTransform.forward * horizontalDistanceFromTarget + new Vector3(0f, mTargetPosition.y - entityTransform.position.y + 1.5f, 0f);
		horizontalDistanceFromTarget = (vector - mGunMuzzleTransform.position).magnitude;
		float num = UnityEngine.Random.Range(0f, horizontalDistanceFromTarget);
		if (num < 20f)
		{
			vector = vector + UnityEngine.Random.Range(-0.1f, 0.1f) * mBodyRotateTransform.right + UnityEngine.Random.Range(-0.1f, 0.1f) * mBodyRotateTransform.up;
		}
		else
		{
			float num2 = UnityEngine.Random.Range(-0.01f, 0.01f);
			vector = vector + (num2 * horizontalDistanceFromTarget + 0.5f * Mathf.Sign(num2)) * mBodyRotateTransform.right + UnityEngine.Random.Range(-0.7f, 0.7f) * mBodyRotateTransform.up;
		}
		bool hitCollider = false;
		Vector3 direction = vector - mGunMuzzleTransform.position;
		direction.Normalize();
		mRay = new Ray(mGunMuzzleTransform.position, direction);
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
		CreateBulletTrail(hitCollider, hitLocalPlayer, direction);
		PlaySound("RPG_Audio/Weapon/assault_rifle/assault_fire");
	}

	private void CreateBulletTrail(bool hitCollider, bool hitLocalPlayer, Vector3 direction)
	{
		if (!hitLocalPlayer && (mRaycastHit.point - mGunMuzzleTransform.position).sqrMagnitude < 25f)
		{
			return;
		}
		GameObject gameObject = gameScene.GetEffectPool(EffectPoolType.BULLET_TRAIL).CreateObject(mGunMuzzleTransform.position + 2.5f * direction, direction, Quaternion.identity);
		if (gameObject == null)
		{
			Debug.Log("fire line obj null");
			return;
		}
		BulletTrailScript component = gameObject.GetComponent<BulletTrailScript>();
		component.beginPos = mGunMuzzleTransform.position + 2.5f * direction;
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
			component.endPos = mGunMuzzleTransform.position + direction * 100f;
		}
		component.speed = 100f;
		component.isActive = true;
	}

	private void Shoot(string shootAnimation)
	{
		if (!mIsShoot)
		{
			CheckShoot();
			mIsShoot = true;
			mGunFireEffect.SetActive(true);
		}
		if (mIsShoot && mBodyAnimation[shootAnimation].time > mBodyAnimation[shootAnimation].clip.length)
		{
			mBodyAnimation[shootAnimation].time -= mBodyAnimation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
			mGunFireEffect.SetActive(false);
		}
	}

	public void StartTerminatorFire()
	{
		SetState(TERMINATOR_FIRE);
		BodyLookAtTargetHorizontal();
		mCurrentBulletCount = 0;
		mIsShoot = false;
		mCanMissile = true;
	}

	public void DoTerminatorFire()
	{
		mBodyEntity.PlayAnimation(AnimationString.TMX_BODY_FIRE, WrapMode.Loop);
		BodyLookAtTargetHorizontal();
		Shoot(AnimationString.TMX_BODY_FIRE);
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndTerminatorFire();
			StartEnemyIdle();
		}
	}

	private void EndTerminatorFire()
	{
		SetBodyCanTurn(false);
		mGunFireEffect.SetActive(false);
	}

	private void StartTerminatorElevate()
	{
		SetState(TERMINATOR_ELEVATE);
		MyCreateNavMeshAgent();
		mElevateTimer.Do();
		mHasElevate = false;
		mCanStartBattle = false;
		mElevateObjects = GameObject.FindGameObjectsWithTag(TagName.TMX_ELEVATOR);
		if (mElevateObjects.Length > 0)
		{
			mElevateOriginPositions = new Vector3[mElevateObjects.Length];
		}
		for (int i = 0; i < mElevateObjects.Length; i++)
		{
			mElevateOriginPositions[i] = mElevateObjects[i].transform.position;
			mElevateObjects[i].transform.position = mElevateObjects[i].transform.position - new Vector3(0f, 12f, 0f);
		}
		entityTransform.position = mCenterPosition - new Vector3(0f, 12f, 0f);
		PlaySound("RPG_Audio/Enemy/TMX/TMX_elevator");
	}

	public void DoTerminatorElevate()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (mHasElevate)
		{
			if (mCanStartBattle && mElevateTimer.Ready())
			{
				EndTerminatorElevate();
				StartEnemyIdle();
			}
		}
		else if (entityTransform.position.y + 3f * Time.deltaTime > mCenterPosition.y)
		{
			entityTransform.position = mCenterPosition;
			for (int i = 0; i < mElevateObjects.Length; i++)
			{
				mElevateObjects[i].transform.position = mElevateOriginPositions[i];
			}
			mHasElevate = true;
		}
		else
		{
			entityTransform.position += 3f * Vector3.up * Time.deltaTime;
			for (int j = 0; j < mElevateObjects.Length; j++)
			{
				mElevateObjects[j].transform.position = mElevateObjects[j].transform.position + 3f * Vector3.up * Time.deltaTime;
			}
		}
	}

	private void EndTerminatorElevate()
	{
		entityTransform.position = mCenterPosition;
		for (int i = 0; i < mElevateObjects.Length; i++)
		{
			mElevateObjects[i].transform.position = mElevateOriginPositions[i];
		}
	}

	public void StartTerminatorMissileReady(byte missileType)
	{
		SetState(TERMINATOR_MISSILE_READY);
		mMissileType = (ETerminatorMissileType)missileType;
		mCanMissile = false;
		StartTerminatorRest();
		SetMissileReadyTimeNow();
		BodyLookForward();
	}

	public void DoTerminatorMissileReady()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		BodyLookForward();
		if (Mathf.Abs(Vector3.Angle(entityTransform.forward, mBodyRotateTransform.forward)) < 10f || GetMissileReadyTimeDuration() > mMaxMissileReadyTime)
		{
			EndTerminatorMissileReady();
			StartTerminatorMissile();
		}
	}

	private void EndTerminatorMissileReady()
	{
		SetBodyCanTurn(false);
	}

	public void StartTerminatorMissile()
	{
		SetState(TERMINATOR_MISSILE);
		mCurrentMissileCount = 0;
		mIsShoot = false;
	}

	private void ShotMissile()
	{
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		GameObject original = Resources.Load("Effect/Enemy/Boss/Terminator/Terminator_Rocket") as GameObject;
		foreach (Player item in potentialPlayerList)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original, mMissileMuzzleTransform.position, Quaternion.LookRotation(mMissileMuzzleTransform.forward)) as GameObject;
			TerminatorMissileScript component = gameObject.GetComponent<TerminatorMissileScript>();
			if (null != component)
			{
				component.enemy = this;
				component.explosionDamage = mRangedExtraDamage2;
				component.explosionRadius = mRangedExplosionRadius2;
				component.speed = mRangedBulletSpeed2;
				component.angularSpeed = 0.03f;
				component.targetPosition = item.GetTransform().position;
				component.type = mMissileType;
				component.targetTransform = item.GetTransform();
			}
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		UnityEngine.Object.Instantiate(original2, mMissileRearTransform.position, Quaternion.identity);
		PlaySound("RPG_Audio/Enemy/TMX/TMX_fire04");
	}

	public void DoTerminatorMissile()
	{
		PlayAnimation(AnimationString.TMX_LEG_MISSILE, WrapMode.Loop);
		mBodyEntity.PlayAnimation(AnimationString.TMX_BODY_MISSILE, WrapMode.Loop);
		if (!mIsShoot)
		{
			ShotMissile();
			mIsShoot = true;
		}
		if (mIsShoot && animation[AnimationString.TMX_LEG_MISSILE].time > animation[AnimationString.TMX_LEG_MISSILE].clip.length)
		{
			animation[AnimationString.TMX_LEG_MISSILE].time -= animation[AnimationString.TMX_LEG_MISSILE].clip.length;
			mCurrentMissileCount++;
			mIsShoot = false;
		}
		if (mCurrentMissileCount >= mRangedBulletCount2)
		{
			EndTerminatorMissile();
			StartEnemyIdle();
		}
	}

	private void EndTerminatorMissile()
	{
	}

	public void StartTerminatorJumpStart(byte action)
	{
		SetState(TERMINATOR_JUMP_START);
		StartTerminatorRest();
		mCenterAction = (ECenterAction)action;
		mJumpEffect = false;
		float num = 1f;
		Vector3 vector = mCenterPosition - entityTransform.position;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num2 = magnitude / num;
		Vector3 vector2 = vector.normalized * num2;
		mJumpSpeed = 20f * Vector3.up + vector2;
	}

	public void DoTerminatorJumpStart()
	{
		PlayAnimation(AnimationString.TMX_LEG_JUMP_START, WrapMode.ClampForever);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (AnimationPlayed(AnimationString.TMX_LEG_JUMP_START, 0.44f))
		{
			if (!mJumpEffect)
			{
				mJumpEffect = true;
				GameObject original = Resources.Load("RPG_effect/RPG_TMXLeg_jump001") as GameObject;
				UnityEngine.Object.Instantiate(original, entityTransform.position, Quaternion.identity);
				PlaySound("RPG_Audio/Enemy/TMX/TMX_jump01");
			}
			cc.Move(mJumpSpeed * Time.deltaTime);
			mJumpSpeed += 20f * Vector3.down * Time.deltaTime;
			if (mJumpSpeed.y < 0f)
			{
				cc.Move(new Vector3(mCenterPosition.x, entityTransform.position.y, mCenterPosition.z) - entityTransform.position);
				EndTerminatorJumpStart();
				StartTerminatorJump();
			}
		}
	}

	private void EndTerminatorJumpStart()
	{
	}

	private void StartTerminatorJump()
	{
		SetState(TERMINATOR_JUMP);
		SetJumpIdleTimeNow();
	}

	public void DoTerminatorJump()
	{
		PlayAnimation(AnimationString.TMX_LEG_JUMP, WrapMode.Loop);
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (GetJumpIdleTimeDuration() > mMaxJumpIdleTime)
		{
			EndTerminatorJump();
			StartTerminatorJumpEnd();
		}
	}

	private void EndTerminatorJump()
	{
	}

	private void StartTerminatorJumpEnd()
	{
		SetState(TERMINATOR_JUMP_END);
		mHasLand = false;
		mJumpSpeed = Vector3.zero;
		float num = entityTransform.position.y - mCenterPosition.y;
		float num2 = Mathf.Sqrt(2f * num / 40f);
		mStartLandTime = Time.time + num2 - 0.26f;
	}

	public void DoTerminatorJumpEnd()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (!mHasLand)
		{
			mJumpSpeed += 40f * Vector3.down * Time.deltaTime;
			if (entityTransform.position.y + mJumpSpeed.y * Time.deltaTime < mCenterPosition.y)
			{
				cc.Move(mCenterPosition - entityTransform.position);
				mHasLand = true;
				GameObject original = Resources.Load("RPG_effect/RPG_TMXLeg_jump002") as GameObject;
				UnityEngine.Object.Instantiate(original, entityTransform.position, Quaternion.identity);
				PlaySound("RPG_Audio/Enemy/TMX/TMX_jump03");
			}
			else
			{
				cc.Move(mJumpSpeed * Time.deltaTime);
			}
		}
		if (Time.time < mStartLandTime)
		{
			PlayAnimation(AnimationString.TMX_LEG_JUMP, WrapMode.Loop);
			return;
		}
		PlayAnimation(AnimationString.TMX_LEG_JUMP_END, WrapMode.ClampForever);
		if (!AnimationPlayed(AnimationString.TMX_LEG_JUMP_END, 0.5f))
		{
			if (mLocalPlayer.CheckHitTimerReady(this) && (mLocalPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < 100f)
			{
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), mLocalPlayer.GetTransform().position - entityTransform.position);
				float distance = Vector3.Distance(entityTransform.position, mLocalPlayer.GetTransform().position);
				if (Physics.Raycast(mRay, out mRaycastHit, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER)
				{
					mLocalPlayer.OnHit(mRushAttackDamage1, this);
					CheckKnocked(mLocalPlayer, 20f);
					mLocalPlayer.ResetCheckHitTimer(this);
				}
			}
			Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
			{
				foreach (SummonedItem value in summonedList.Values)
				{
					if (value.CheckHitTimerReady(this) && (value.GetTransform().position - entityTransform.position).sqrMagnitude < 100f)
					{
						mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), value.GetTransform().position - entityTransform.position);
						float distance2 = Vector3.Distance(entityTransform.position, value.GetTransform().position);
						if (Physics.Raycast(mRay, out mRaycastHit, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.SUMMONED)
						{
							value.OnHit(mRushAttackDamage1);
							CheckKnocked(value, 20f);
							value.ResetCheckHitTimer(this);
						}
					}
				}
				return;
			}
		}
		if (!AnimationPlayed(AnimationString.TMX_LEG_JUMP_END, 1f))
		{
			return;
		}
		EndTerminatorJumpEnd();
		if (base.IsMasterPlayer)
		{
			if (mCenterAction == ECenterAction.LASER || mCenterAction == ECenterAction.FINAL)
			{
				StartTerminatorLaser((byte)mCenterAction);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.TERMINATOR_LASER, entityTransform.position, (byte)mCenterAction);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else if (mCenterAction == ECenterAction.GRENADE)
			{
				StartTerminatorGrenadeStart(0);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.TERMINATOR_GRENADE_START, entityTransform.position, (byte)mGrenadeType);
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
			StartEnemyIdle();
		}
	}

	private void EndTerminatorJumpEnd()
	{
	}

	public void StartTerminatorStamp()
	{
		SetState(TERMINATOR_STAMP);
		StartTerminatorRest();
		mStampEffect = false;
		mCanMissile = true;
	}

	public void DoTerminatorStamp()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		PlayAnimation(AnimationString.TMX_LEG_STAMP, WrapMode.ClampForever);
		float num = animation[AnimationString.TMX_LEG_STAMP].time / animation[AnimationString.TMX_LEG_STAMP].clip.length;
		if (!mStampEffect && num > 0.4f)
		{
			mStampEffect = true;
			GameObject original = Resources.Load("RPG_effect/RPG_TMXL_attack") as GameObject;
			UnityEngine.Object.Instantiate(original, new Vector3(mRightHandTransform.position.x, entityTransform.position.y, mRightHandTransform.position.z), Quaternion.identity);
			PlaySound("RPG_Audio/Enemy/TMX/TMX_melee_attack");
		}
		if (mLocalPlayer.CheckHitTimerReady(this) && num > 0.4f && num < 0.5f && (mLocalPlayer.GetTransform().position - mRightHandTransform.position).sqrMagnitude < 64f)
		{
			mRay = new Ray(mRightHandTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f) - mRightHandTransform.position);
			float distance = Vector3.Distance(mRightHandTransform.position, mLocalPlayer.GetTransform().position + new Vector3(0f, 0.5f, 0f));
			if (Physics.Raycast(mRay, out mRaycastHit, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER)) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				mLocalPlayer.OnHit(mMeleeAttackDamage1, this);
				CheckKnocked(mLocalPlayer, 20f);
				mLocalPlayer.ResetCheckHitTimer(this);
			}
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && num > 0.4f && num < 0.5f && (value.GetTransform().position - mRightHandTransform.position).sqrMagnitude < 64f)
			{
				mRay = new Ray(mRightHandTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f) - mRightHandTransform.position);
				float distance2 = Vector3.Distance(mRightHandTransform.position, value.GetTransform().position + new Vector3(0f, 0.5f, 0f));
				if (Physics.Raycast(mRay, out mRaycastHit, distance2, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.SUMMONED)) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.SUMMONED)
				{
					value.OnHit(mMeleeAttackDamage1);
					CheckKnocked(value, 20f);
					value.ResetCheckHitTimer(this);
				}
			}
		}
		if (num > 1f)
		{
			EndTerminatorStamp();
			StartEnemyIdle();
		}
	}

	private void EndTerminatorStamp()
	{
	}

	public void StartTerminatorGrenadeStart(byte type)
	{
		StartTerminatorRest();
		SetState(TERMINATOR_GRENADE_START);
		mGrenadeType = (EGrenadeType)type;
		mCanMissile = true;
		SetGrenadeTimeNow();
	}

	public void DoTerminatorGrenadeStart()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		PlayAnimation(AnimationString.TMX_LEG_GRENADE_START, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.TMX_LEG_GRENADE_START, 1f))
		{
			EndTerminatorGrenadeStart();
			StartTerminatorGrenade();
		}
	}

	private void EndTerminatorGrenadeStart()
	{
	}

	private void StartTerminatorGrenade()
	{
		SetState(TERMINATOR_GRENADE);
		mIsShoot = false;
	}

	private void ShotRecoverGrenade()
	{
		GameObject original = Resources.Load("Effect/Enemy/Boss/Terminator/Terminator_Grenade") as GameObject;
		for (int i = 0; i < 4; i++)
		{
			Transform transform = mGrenadeTransformArray[i];
			Vector3 vector = transform.position + transform.forward * 10f + transform.right * UnityEngine.Random.Range(-2f, 2f) + transform.up * UnityEngine.Random.Range(1f, 3f);
			Vector3 normalized = (vector - transform.position).normalized;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, transform.position, Quaternion.LookRotation(normalized)) as GameObject;
			TerminatorGrenadeScript component = gameObject.GetComponent<TerminatorGrenadeScript>();
			component.enemy = this;
			component.dir = normalized;
			component.explodeTime = mGrenadeExplodeTimes[1];
			component.explodeRadius = mRangedExplosionRadius1;
			component.explodeDamage = mRangedExtraDamage1;
			component.forceValue = 15f;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original, mBodyRotateTransform.position, Quaternion.LookRotation(Vector3.down)) as GameObject;
		TerminatorGrenadeScript component2 = gameObject2.GetComponent<TerminatorGrenadeScript>();
		component2.enemy = this;
		component2.dir = Vector3.down;
		component2.explodeTime = mGrenadeExplodeTimes[1];
		component2.explodeRadius = mRangedExplosionRadius1;
		component2.explodeDamage = mRangedExtraDamage1;
		component2.forceValue = 15f;
	}

	private void ShotAoeGrenade()
	{
		GameObject original = Resources.Load("Effect/Enemy/Boss/Terminator/Terminator_Grenade") as GameObject;
		for (int i = 0; i < mGrenadeNum; i++)
		{
			Vector3 vector = mBodyRotateTransform.position + mBodyRotateTransform.forward * UnityEngine.Random.Range(-2f, 2f) + mBodyRotateTransform.up + mBodyRotateTransform.right * UnityEngine.Random.Range(-2f, 2f);
			Vector3 normalized = (vector - mBodyRotateTransform.position).normalized;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, mBodyRotateTransform.position, Quaternion.LookRotation(normalized)) as GameObject;
			TerminatorGrenadeScript component = gameObject.GetComponent<TerminatorGrenadeScript>();
			component.enemy = this;
			component.dir = normalized;
			component.explodeTime = mGrenadeExplodeTimes[0] + (float)i;
			component.explodeRadius = mRangedExplosionRadius1;
			component.explodeDamage = mRangedExtraDamage1;
			component.forceValue = UnityEngine.Random.Range(20f, 30f);
			component.enableCollisionTime = 1f;
		}
	}

	public void DoTerminatorGrenade()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		PlayAnimation(AnimationString.TMX_LEG_GRENADE, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.TMX_LEG_GRENADE, 1f))
		{
			EndTerminatorGrenade();
			StartTerminatorGrenadeEnd();
		}
		else if (!mIsShoot && AnimationPlayed(AnimationString.TMX_LEG_GRENADE, 0.5f))
		{
			if (mGrenadeType == EGrenadeType.AOE)
			{
				ShotAoeGrenade();
			}
			else if (mGrenadeType == EGrenadeType.RECOVER)
			{
				ShotRecoverGrenade();
			}
			mIsShoot = true;
		}
	}

	private void EndTerminatorGrenade()
	{
	}

	private void StartTerminatorGrenadeEnd()
	{
		SetState(TERMINATOR_GRENADE_END);
	}

	public void DoTerminatorGrenadeEnd()
	{
		mBodyEntity.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		PlayAnimation(AnimationString.TMX_LEG_GRENADE_END, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.TMX_LEG_GRENADE_END, 1f))
		{
			EndTerminatorGrenadeEnd();
			StartEnemyIdle();
		}
	}

	private void EndTerminatorGrenadeEnd()
	{
	}

	public void StartTerminatorLaser(byte action)
	{
		SetState(TERMINATOR_LASER);
		mCenterAction = (ECenterAction)action;
		StartTerminatorRest();
		mLaserPower.SetActive(true);
		mLaserActive = false;
		mLaserDeactive = false;
		mCanMissile = true;
		mLaserTimes = Mathf.CeilToInt((1f - (float)Hp / (float)base.MaxHp) / mHpTriggerForLaser - 1f);
		Debug.Log("(float)Hp / MaxHp = " + (float)Hp / (float)base.MaxHp);
		Debug.Log("mLaserTimes = " + mLaserTimes);
		PlaySound("RPG_Audio/Enemy/TMX/TMX_fire02");
	}

	public void DoTerminatorLaser()
	{
		mBodyEntity.PlayAnimation(AnimationString.TMX_BODY_LASER, WrapMode.ClampForever);
		if (mBodyEntity.AnimationPlayed(AnimationString.TMX_BODY_LASER, 1f))
		{
			EndTerminatorLaser();
			if (!mIsFinalBattle && mCenterAction == ECenterAction.FINAL)
			{
				StartFinalBattle();
			}
			else
			{
				StartEnemyIdle();
			}
		}
		else if (mBodyEntity.AnimationPlayed(AnimationString.TMX_BODY_LASER, 0.78f))
		{
			if (!mLaserDeactive)
			{
				mLaserDeactive = true;
				mLaserPower.SetActive(false);
				mLaserSpark.SetActive(false);
				for (int i = 0; i < 3; i++)
				{
					mLaserBeamArray[i].SetActive(false);
				}
				if ((float)Hp < (float)base.MaxHp * mHpTriggerForFinalBattle2)
				{
					ShotMissile();
				}
			}
		}
		else
		{
			if (!mBodyEntity.AnimationPlayed(AnimationString.TMX_BODY_LASER, 0.45f) || mLaserActive)
			{
				return;
			}
			mLaserActive = true;
			for (int j = 0; j < 3; j++)
			{
				mLaserBeamArray[j].SetActive(true);
				TerminatorLaserScript component = mLaserBeamArray[j].GetComponent<TerminatorLaserScript>();
				if (null != component)
				{
					component.mEnemy = this;
					component.mSpark = mLaserSpark;
					component.mDamage = mMeleeAttackDamage2;
				}
			}
		}
	}

	private void EndTerminatorLaser()
	{
		mLaserPower.SetActive(false);
		mLaserSpark.SetActive(false);
		for (int i = 0; i < 3; i++)
		{
			mLaserBeamArray[i].SetActive(false);
		}
	}

	protected override bool IsJumping()
	{
		return mState == TERMINATOR_JUMP;
	}
}
