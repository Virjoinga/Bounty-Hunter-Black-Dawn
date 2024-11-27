using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : GameUnit
{
	public struct MeleeAttackData
	{
		public string Animation;

		public Transform Trans;

		public float StartPercent;

		public float EndPercent;

		public float Range;

		public float Angle;

		public int Damage;

		public float KnockedSpeed;
	}

	public struct AnimationKey
	{
		public float Time;

		private float LastTime;

		public bool IsTrigger(float CurrentTime)
		{
			if (LastTime < Time && CurrentTime >= Time)
			{
				LastTime = CurrentTime;
				return true;
			}
			if (LastTime < Time && CurrentTime < LastTime)
			{
				LastTime = CurrentTime;
				return true;
			}
			LastTime = CurrentTime;
			return false;
		}

		public void Reset()
		{
			LastTime = Time;
		}
	}

	protected const float FIRE_LINE_HALF_LENGTH = 2.5f;

	protected const float BULLET_SPEED = 100f;

	public static EnemyState PATROL_IDLE_STATE = new PatrolIdleState();

	public static EnemyState PATROL_STATE = new PatrolState();

	public static EnemyState IDLE_STATE = new EnemyIdleState();

	public static EnemyState CATCHING_STATE = new CatchingState();

	public static EnemyState ATTACK_STATE = new EnemyAttackState();

	public static EnemyState GOTHIT_STATE = new GotHitState();

	public static EnemyState DEAD_STATE = new DeadState();

	public static EnemyState GOBACK_STATE = new GoBackState();

	public static EnemyState AWAKE_STATE = new AwakeState();

	public static EnemyState RAGE_STATE = new RageState();

	public static EnemyState FADEOUT_STATE = new FadeoutState();

	protected static float[] FIRE_SPEED_COEFFICIENTS = new float[5] { 1f, 0.8f, 1.1f, 0.9f, 1.2f };

	private static string COLOR_PROPERTY_NAME = "_TintColor";

	private static string MAIN_TEX_NAME = "_MainTex";

	protected int mUniqueID;

	protected byte mLevel;

	protected short mGroupID;

	protected int mExperience;

	protected int mGold;

	protected float mIdleTime;

	protected float mPatrolIdleTime;

	protected float mPatrolSpeed;

	protected float mWalkSpeed;

	protected float mWalkSpeedInit;

	protected float mRunSpeed;

	protected float mRunSpeedInit;

	protected int mDetectRadius;

	protected int mDetectSectorRadius;

	protected int mDetectSectorAngle;

	protected int mActivityRadius;

	protected float mMeleeAttackRadius;

	protected int mMeleeAttackProbability;

	protected int mMeleeAttackDamage1;

	protected int mMeleeAttackDamage1Init;

	protected int mMeleeAttackDamage2;

	protected int mMeleeAttackDamage2Init;

	protected int mRushAttackRadius;

	protected int mRushAttackProbability;

	protected int mRushAttackDamage1;

	protected int mRushAttackDamage1Init;

	protected float mRushAttackSpeed1;

	protected int mRushAttackDamage2;

	protected int mRushAttackDamage2Init;

	protected float mRushAttackSpeed2;

	protected int mRangedStandAttackRadius;

	protected int mRangedMoveAttackRadius;

	protected int mRangedAttackToCatchRadius;

	protected int mRangedAttackProbability;

	protected int mRangedAttackDamage1;

	protected int mRangedAttackDamage1Init;

	protected int mRangedExtraDamage1;

	protected int mRangedExtraDamage1Init;

	protected float mRangedOneShotTime1;

	protected float mRangedInterval1;

	protected int mRangedBulletCount1;

	protected float mRangedBulletSpeed1;

	protected float mRangedExplosionRadius1;

	protected int mRangedAttackDamage2;

	protected int mRangedAttackDamage2Init;

	protected int mRangedExtraDamage2;

	protected int mRangedExtraDamage2Init;

	protected float mRangedOneShotTime2;

	protected float mRangedInterval2;

	protected int mRangedBulletCount2;

	protected float mRangedBulletSpeed2;

	protected float mRangedExplosionRadius2;

	protected int mCoverSearchRadius;

	protected float mCoverInterval;

	protected float mCoverHideIdleTime;

	protected int mCoverExposeCount;

	protected int mCoverAttackCount;

	protected int mCoverBulletCount;

	protected bool mCanAwake;

	protected bool mCanRage;

	protected float mRagePercent;

	protected float mLastIdleTime;

	protected float mLastPatrolTime;

	protected float mLastUpdateNavMeshTime;

	protected float mLastUpdatePosTime;

	protected Timer mShoutAudioTimer = new Timer();

	protected Timer mLastBloodEffectTimer = new Timer();

	protected Timer mDeadTimer = new Timer();

	protected Timer mDeadRemoveBodyTimer = new Timer();

	protected Timer mDetectTimer = new Timer();

	protected Timer mCheckGoBackTimer = new Timer();

	protected Timer mChooseTargetPlayerTimer = new Timer();

	protected Timer mOnHitTimer = new Timer();

	protected Timer mLookAtTimer = new Timer();

	protected Timer mRunAudioTimer = new Timer();

	protected Timer mWalkAudioTimer = new Timer();

	protected EnemyState mState;

	protected EnemyType mEnemyType;

	protected bool mCanTurn;

	protected bool mIsActive;

	protected MonsterConfig mMonsterConfig;

	protected EnemyConfig mEnemyConfig;

	protected UnityEngine.AI.NavMeshAgent mNavMeshAgent;

	protected BaseEnemySpawnScript mSpawnPointScript;

	protected Ray mRay;

	protected RaycastHit mRaycastHit;

	protected GameObject[] mHitObjectArray;

	protected Transform mBodyTransform;

	protected Transform mHeadTransform;

	protected Dictionary<Player, int> mDamageFromPlayers = new Dictionary<Player, int>();

	protected List<Player> mPlayerList = new List<Player>();

	protected Player mLocalPlayer;

	protected GameUnit mTarget;

	protected Vector3 mTargetPosition;

	protected Vector3 mTargetToLookAt;

	protected Vector3 mPatrolTarget;

	protected Vector3 mRushDirection;

	protected GameObject mNextPatrolLinePoint;

	protected float mMaxTurnRadian;

	protected float mTurnSpeed;

	protected float mHitCheckHeight;

	protected string mShadowPath;

	protected GameObject mShadowObject;

	protected float mNavAngularSpeed;

	protected int mNavWalkableMask;

	protected float mNavRadius = 0.5f;

	protected float mNavHeight = 2f;

	protected float mNavBaseOffset;

	protected float mStoppingDistance = 0.5f;

	protected bool mIsCriticalAttacked;

	protected float mFloorHeight = Global.FLOORHEIGHT;

	protected BloodColor mBloodColor;

	protected bool mCanGotHit;

	protected bool mOnGround;

	protected bool mHasRaged;

	protected bool mIsShoot;

	protected List<PreSpawnBuffer> mPreSpawnItems = new List<PreSpawnBuffer>();

	protected int mRowIndexInDataTable;

	protected float mUpdatePosInterval;

	protected Vector3 mDeltaPosition;

	protected int mCurrentDeltaCount;

	protected int mMaxDeltaCount = 30;

	protected float mLastGoBackTime;

	protected float mMaxGoBackTime = 8f;

	protected bool mIsOnOffMeshLink;

	protected float mPreviousSpeed;

	protected Dictionary<string, Renderer> entityRenderers = new Dictionary<string, Renderer>();

	protected int mKillerID;

	protected CharacterSkillManager skillMgr = new CharacterSkillManager();

	public ESpawnType SpawnType;

	protected byte pointID;

	public DebugComponent mDebugComponent;

	public byte EnemyID { get; set; }

	public bool IsElite { get; set; }

	public bool IsMasterPlayer { get; set; }

	public bool IsHit { get; set; }

	public Vector3 SpawnPosition { get; set; }

	public byte PointID
	{
		get
		{
			if (mSpawnPointScript != null)
			{
				return mSpawnPointScript.PointID;
			}
			return pointID;
		}
		set
		{
			pointID = value;
		}
	}

	public int UniqueID
	{
		get
		{
			return mUniqueID;
		}
		set
		{
			mUniqueID = value;
		}
	}

	public byte Level
	{
		get
		{
			return mLevel;
		}
		set
		{
			mLevel = value;
		}
	}

	public GameObject NextPatrolLinePoint
	{
		get
		{
			return mNextPatrolLinePoint;
		}
		set
		{
			mNextPatrolLinePoint = value;
		}
	}

	public EnemyType EnemyType
	{
		get
		{
			return mEnemyType;
		}
		set
		{
			mEnemyType = value;
		}
	}

	public float SpeedRate
	{
		get
		{
			return detailedProperties.SpeedBonus;
		}
		set
		{
			detailedProperties.SpeedBonus = value;
		}
	}

	public float SqrDistanceFromTargetPlayer
	{
		get
		{
			return (mTargetPosition - entityTransform.position).sqrMagnitude;
		}
	}

	public virtual bool CanPatrol()
	{
		return false;
	}

	protected float GetFireSpeedCoefficent()
	{
		return FIRE_SPEED_COEFFICIENTS[EnemyID % FIRE_SPEED_COEFFICIENTS.Length];
	}

	public CharacterSkillManager GetCharacterSkillManager()
	{
		return skillMgr;
	}

	public override bool InPlayingState()
	{
		return IsActive() && mState != DEAD_STATE && mState != FADEOUT_STATE;
	}

	public virtual bool AimAssist()
	{
		return true;
	}

	public Timer GetLastBloodEffectTimer()
	{
		return mLastBloodEffectTimer;
	}

	public float GetIdleTimeDuration()
	{
		return Time.time - mLastIdleTime;
	}

	public void SetIdleTimeNow()
	{
		mLastIdleTime = Time.time + UnityEngine.Random.Range(-0.5f, 0.5f);
	}

	public float GetPatrolTimeDuration()
	{
		return Time.time - mLastPatrolTime;
	}

	public void SetPatrolTimeNow()
	{
		mLastPatrolTime = Time.time;
	}

	public float GetUpdatePosTimeDuration()
	{
		return Time.time - mLastUpdatePosTime;
	}

	public void SetUpdatePosTimeNow()
	{
		mLastUpdatePosTime = Time.time;
	}

	public float GetGoBackTimeDuration()
	{
		return Time.time - mLastGoBackTime;
	}

	public void SetGoBackTimeNow()
	{
		mLastGoBackTime = Time.time;
	}

	public void SetRushDirection(Vector3 dir)
	{
		mRushDirection = dir;
	}

	public Vector3 GetRushDirection()
	{
		return mRushDirection;
	}

	public virtual bool IsBoss()
	{
		if (EnemyType.ELITE_CYPHER < mEnemyType)
		{
			return true;
		}
		return false;
	}

	public bool IsEnemyElite()
	{
		if (EnemyType.ELITE_HATI <= mEnemyType && mEnemyType <= EnemyType.ELITE_CYPHER)
		{
			return true;
		}
		return false;
	}

	public virtual bool IsBossRush()
	{
		return false;
	}

	public bool IsActive()
	{
		return mIsActive;
	}

	public virtual bool CanAutoAim()
	{
		return InPlayingState();
	}

	public virtual void UpdatePosition(Vector3 position, EnemyStateConst nextState)
	{
		if (!IsMasterPlayer && mIsActive && entityTransform != null)
		{
			mDeltaPosition = position - entityTransform.position;
			mCurrentDeltaCount = 0;
		}
	}

	public void SetPlayerList(List<int> playerIDList)
	{
		mPlayerList = new List<Player>();
		mDamageFromPlayers = new Dictionary<Player, int>();
		IsMasterPlayer = false;
		if (playerIDList.Count > 0 && playerIDList[0] == mLocalPlayer.GetUserID())
		{
			IsMasterPlayer = true;
		}
		foreach (int playerID in playerIDList)
		{
			if (playerID == mLocalPlayer.GetUserID())
			{
				if (!mPlayerList.Contains(mLocalPlayer))
				{
					mPlayerList.Add(mLocalPlayer);
				}
				if (!mDamageFromPlayers.ContainsKey(mLocalPlayer))
				{
					mDamageFromPlayers.Add(mLocalPlayer, 0);
				}
				continue;
			}
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				if (!mPlayerList.Contains(remotePlayerByUserID))
				{
					mPlayerList.Add(remotePlayerByUserID);
				}
				if (!mDamageFromPlayers.ContainsKey(remotePlayerByUserID))
				{
					mDamageFromPlayers.Add(remotePlayerByUserID, 0);
				}
			}
		}
	}

	protected virtual void StartIdleWhenAddPlayer()
	{
		if (mState == CATCHING_STATE)
		{
			EndCatching();
			StartEnemyIdleWithoutResetTime();
		}
		else if (mState == PATROL_STATE)
		{
			EndPatrol();
			StartPatrolIdleWithoutResetTime();
		}
	}

	public void AddPlayer(int playerID)
	{
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			if (!mPlayerList.Contains(remotePlayerByUserID))
			{
				mPlayerList.Add(remotePlayerByUserID);
			}
			if (!mDamageFromPlayers.ContainsKey(remotePlayerByUserID))
			{
				mDamageFromPlayers.Add(remotePlayerByUserID, 0);
			}
		}
		if (IsMasterPlayer)
		{
			if (mTarget == null)
			{
				ChooseTargetPlayer(true);
			}
			else
			{
				EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(PointID, EnemyID, mTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		StartIdleWhenAddPlayer();
	}

	public void RemovePlayer(int playerID)
	{
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			if (mPlayerList.Contains(remotePlayerByUserID))
			{
				mPlayerList.Remove(remotePlayerByUserID);
			}
			if (mDamageFromPlayers.ContainsKey(remotePlayerByUserID))
			{
				mDamageFromPlayers.Remove(remotePlayerByUserID);
			}
		}
		IsMasterPlayer = mPlayerList[0] == mLocalPlayer;
		if (remotePlayerByUserID == mTarget)
		{
			mTarget = null;
		}
	}

	protected virtual void LoadConfig()
	{
		mEnemyConfig = GameConfig.GetInstance().enemyConfigId[UniqueID];
		mDisplayName = mEnemyConfig.DisplayName;
		BasicMaxHp = mEnemyConfig.MaxHp;
		base.MaxHp = (int)((float)(BasicMaxHp * mLevel) * (1f + (float)(int)mLevel / 30f));
		Hp = base.MaxHp;
		BasicMaxShield = mEnemyConfig.MaxShield;
		base.MaxShield = (int)((float)(BasicMaxShield * mLevel) * (1f + (float)(int)mLevel / 30f));
		Shield = base.MaxShield;
		mExperience = mEnemyConfig.Experience;
		mGold = mEnemyConfig.Gold;
		mGroupID = mEnemyConfig.GroupID;
		mIdleTime = mEnemyConfig.IdleTime;
		BasicShieldRecovery = mEnemyConfig.ShieldRecoverySpeed;
		base.ShieldRecovery = (int)((float)(BasicShieldRecovery * mLevel) * (1f + (float)(int)mLevel / 30f));
		mPatrolIdleTime = mEnemyConfig.PatrolIdleTime;
		mPatrolSpeed = mEnemyConfig.PatrolSpeed;
		mWalkSpeed = mEnemyConfig.WalkSpeed;
		mWalkSpeedInit = mWalkSpeed;
		mRunSpeed = mEnemyConfig.RunSpeed;
		mRunSpeedInit = mRunSpeed;
		mDetectRadius = mEnemyConfig.DetectRadius;
		mDetectSectorRadius = mEnemyConfig.DetectSectorRadius;
		mDetectSectorAngle = mEnemyConfig.DetectSectorAngle;
		mActivityRadius = mEnemyConfig.ActivityRadius;
		mMeleeAttackRadius = mEnemyConfig.MeleeAttackRadius;
		mMeleeAttackProbability = mEnemyConfig.MeleeAttackProbability;
		mMeleeAttackDamage1Init = mEnemyConfig.MeleeAttackDamage1;
		mMeleeAttackDamage1 = mMeleeAttackDamage1Init;
		mMeleeAttackDamage2Init = mEnemyConfig.MeleeAttackDamage2;
		mMeleeAttackDamage2 = mMeleeAttackDamage2Init;
		mRushAttackRadius = mEnemyConfig.RushAttackRadius;
		mRushAttackProbability = mEnemyConfig.RushAttackProbability;
		mRushAttackDamage1Init = mEnemyConfig.RushAttackDamage1;
		mRushAttackDamage1 = mRushAttackDamage1Init;
		mRushAttackSpeed1 = mEnemyConfig.RushAttackSpeed1;
		mRushAttackDamage2Init = mEnemyConfig.RushAttackDamage2;
		mRushAttackDamage2 = mRushAttackDamage2Init;
		mRushAttackSpeed2 = mEnemyConfig.RushAttackSpeed2;
		mRangedStandAttackRadius = mEnemyConfig.RangedStandAttackRadius;
		mRangedMoveAttackRadius = mEnemyConfig.RangedMoveAttackRadius;
		mRangedAttackToCatchRadius = mEnemyConfig.RangedAttackToCatchRadius;
		mRangedAttackProbability = mEnemyConfig.RangedAttackProbability;
		mRangedAttackDamage1Init = mEnemyConfig.RangedAttackDamage1;
		mRangedAttackDamage1 = mRangedAttackDamage1Init;
		mRangedExtraDamage1Init = mEnemyConfig.RangedExtraDamage1;
		mRangedExtraDamage1 = mRangedExtraDamage1Init;
		mRangedOneShotTime1 = mEnemyConfig.RangedOneShotTime1;
		mRangedInterval1 = mEnemyConfig.RangedInterval1;
		mRangedBulletCount1 = mEnemyConfig.RangedBulletCount1;
		mRangedBulletSpeed1 = mEnemyConfig.RangedBulletSpeed1;
		mRangedExplosionRadius1 = mEnemyConfig.RangedExplosionRadius1;
		mRangedAttackDamage2Init = mEnemyConfig.RangedAttackDamage2;
		mRangedAttackDamage2 = mRangedAttackDamage2Init;
		mRangedExtraDamage2Init = mEnemyConfig.RangedExtraDamage2;
		mRangedExtraDamage2 = mRangedExtraDamage2Init;
		mRangedOneShotTime2 = mEnemyConfig.RangedOneShotTime2;
		mRangedInterval2 = mEnemyConfig.RangedInterval2;
		mRangedBulletCount2 = mEnemyConfig.RangedBulletCount2;
		mRangedBulletSpeed2 = mEnemyConfig.RangedBulletSpeed2;
		mRangedExplosionRadius2 = mEnemyConfig.RangedExplosionRadius2;
		mCoverSearchRadius = mEnemyConfig.CoverSearchRadius;
		mCoverInterval = mEnemyConfig.CoverInterval;
		mCoverHideIdleTime = mEnemyConfig.CoverHideIdleTime;
		mCoverExposeCount = mEnemyConfig.CoverExposeCount;
		mCoverAttackCount = mEnemyConfig.CoverAttackCount;
		mCoverBulletCount = mEnemyConfig.CoverBulletCount;
		CalculateAbility();
	}

	public virtual void CalculateAbility()
	{
		base.MaxHp = (int)((float)(BasicMaxHp * mLevel) * (1f + (float)(int)mLevel / 30f));
		base.MaxShield = (int)((float)(BasicMaxShield * mLevel) * (1f + (float)(int)mLevel / 30f));
		base.ShieldRecovery = (int)((float)(BasicShieldRecovery * mLevel) * (1f + (float)(int)mLevel / 30f));
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		if (mLevel - charLevel >= 2)
		{
			mExperience = 10 * (charLevel + 2);
		}
		else
		{
			mExperience = 10 * mLevel;
		}
		mGold = 5 * mLevel;
		mMeleeAttackDamage1 = mMeleeAttackDamage1Init * mLevel;
		mMeleeAttackDamage2 = mMeleeAttackDamage2Init * mLevel;
		mRushAttackDamage1 = mRushAttackDamage1Init * mLevel;
		mRushAttackDamage2 = mRushAttackDamage2Init * mLevel;
		mRangedAttackDamage1 = mRangedAttackDamage1Init * mLevel;
		mRangedAttackDamage2 = mRangedAttackDamage2Init * mLevel;
		mRangedExtraDamage1 = mRangedExtraDamage1Init * mLevel;
		mRangedExtraDamage2 = mRangedExtraDamage2Init * mLevel;
		if (IsBoss())
		{
			mExperience *= 10;
			mGold *= 5;
		}
		else if (IsEnemyElite())
		{
			mExperience *= 3;
			mGold *= 3;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			int playerCount = GameApp.GetInstance().GetGameWorld().GetPlayerCount();
			float hpParaWithPlayerCount = GetHpParaWithPlayerCount(playerCount);
			if (base.MaxHp != 0)
			{
				int num = (int)((float)base.MaxHp * hpParaWithPlayerCount);
				Hp = (int)((float)Hp * ((float)num / (float)base.MaxHp));
				base.MaxHp = num;
			}
			if (base.MaxShield != 0)
			{
				int num2 = (int)((float)base.MaxShield * hpParaWithPlayerCount);
				Shield = (int)((float)Shield * ((float)num2 / (float)base.MaxShield));
				base.MaxShield = num2;
			}
		}
		if (Hp > base.MaxHp)
		{
			Hp = base.MaxHp;
		}
		if (Shield > base.MaxShield)
		{
			Shield = base.MaxShield;
		}
	}

	public virtual void Init()
	{
		base.GameUnitType = EGameUnitType.ENEMY;
		gameScene = GameApp.GetInstance().GetGameScene();
		gameWorld = GameApp.GetInstance().GetGameWorld();
		mLocalPlayer = gameWorld.GetLocalPlayer();
		mRagePercent = 0.4f;
		mTurnSpeed = 0.1f;
		mNavAngularSpeed = 720f;
		mNavWalkableMask = (1 << NavMeshLayer.NORMAL_GROUND) | (1 << NavMeshLayer.JUMP);
		mNavBaseOffset = 0f;
		mHitCheckHeight = 0.5f;
		mUpdatePosInterval = 1f;
		LoadConfig();
		mShadowPath = "Effect/Shadow";
		mIsCriticalAttacked = false;
		mShieldRecoverySecondTimer.SetTimer(1f, true);
		mShieldRecoveryStartTimer.SetTimer(mEnemyConfig.ShieldRecoveryInterval, true);
		mIsActive = false;
		mCanTurn = false;
		mCanAwake = false;
		mCanRage = false;
		mHasRaged = false;
		mTarget = null;
		mShoutAudioTimer.SetTimer(5f, true);
		mDetectTimer.SetTimer(1f, false);
		mCheckGoBackTimer.SetTimer(1f, false);
		mChooseTargetPlayerTimer.SetTimer(3f, true);
		mOnHitTimer.SetTimer(mEnemyConfig.OnHitInterval, true);
		mLookAtTimer.SetTimer(0.3f, true);
		mLastUpdateNavMeshTime = Time.time;
		mLastBloodEffectTimer.SetTimer(0.3f, false);
		IsHit = false;
		mCanGotHit = true;
		ApplyBuffToUnitStatus();
		mDotTimer.SetTimer(1f, false);
	}

	public virtual void Deactivate()
	{
		mIsActive = false;
		IsHit = false;
		mOnGround = false;
		entityRenderers.Clear();
		mTarget = null;
		DestroyNavMeshAgent();
		if (entityObject != null)
		{
			UnityEngine.Object.Destroy(entityObject);
			entityObject = null;
			animationObject = null;
			mShadowObject = null;
		}
		entityTransform = null;
		UserStateHUD.GetInstance().RemoveEnemy(base.Name);
	}

	public virtual bool CanActivate()
	{
		bool flag = GameApp.GetInstance().GetSceneStreaingManager().isLoad(AssetBundleName.ENEMY[(int)EnemyType]);
		if (flag)
		{
			string[] array = AssetBundleName.ENEMY_TEXTURE[(int)EnemyType];
			foreach (string dataName in array)
			{
				flag = GameApp.GetInstance().GetSceneStreaingManager().isLoad(dataName);
				if (!flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	protected virtual void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.EnemyBody);
		mHeadTransform = entityTransform.Find(BoneName.EnemyHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	protected virtual string GetBodyMeshName()
	{
		return string.Empty;
	}

	protected virtual void InitRenders()
	{
		GameObject gameObject = entityObject.transform.Find(GetBodyMeshName()).gameObject;
		if (null != gameObject)
		{
			if ((IsEnemyElite() || NeedEliteImg()) && gameObject.GetComponent<Renderer>().materials.Length > 0 && gameObject.GetComponent<Renderer>().materials[0] != null)
			{
				gameObject.GetComponent<Renderer>().materials[0].mainTexture = GameApp.GetInstance().GetSceneStreaingManager().CloneTexture(AssetBundleName.ENEMY_TEXTURE[(int)EnemyType][0]);
			}
			if (!entityRenderers.ContainsKey("body"))
			{
				entityRenderers.Add("body", gameObject.GetComponent<Renderer>());
			}
		}
	}

	public virtual void Activate()
	{
		gameScene = GameApp.GetInstance().GetGameScene();
		gameWorld = GameApp.GetInstance().GetGameWorld();
		SetObject(GameApp.GetInstance().GetSceneStreaingManager().CloneGameObject(AssetBundleName.ENEMY[(int)EnemyType]));
		entityObject.transform.position = SpawnPosition;
		entityObject.name = base.Name;
		animation = entityObject.GetComponent<Animation>();
		rigidbody = entityObject.GetComponent<Rigidbody>();
		collider = entityObject.transform.GetComponent<Collider>();
		InitRenders();
		if (mShadowPath != string.Empty)
		{
			GameObject original = Resources.Load(mShadowPath) as GameObject;
			mShadowObject = UnityEngine.Object.Instantiate(original) as GameObject;
			mShadowObject.transform.parent = entityTransform;
			mShadowObject.transform.localPosition = Vector3.zero;
			mShadowObject.transform.localScale = Vector3.one;
		}
		if (mTarget == null)
		{
			mTarget = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			mTargetPosition = mTarget.GetTransform().position;
		}
		if (SpawnType != 0)
		{
			StartEnemyIdleWithoutResetTime();
		}
		else
		{
			StartPatrolIdleWithoutResetTime();
		}
		CreateNavMeshAgent();
		CreateDebugComponent();
		InitBips();
		mIsActive = true;
		mOnGround = false;
		mIsShoot = false;
		mIsOnOffMeshLink = false;
		mPreviousSpeed = 0f;
	}

	public virtual void LinkEnemy(List<Enemy> enemyList)
	{
	}

	public virtual bool NeedEliteImg()
	{
		return false;
	}

	public virtual bool CanBeHit()
	{
		return true;
	}

	public virtual bool CanBeSpawn()
	{
		if (Hp > 0)
		{
			return true;
		}
		return false;
	}

	public void UploadResponse()
	{
		IsMasterPlayer = true;
		mPlayerList = new List<Player>();
		mPlayerList.Add(mLocalPlayer);
		mDamageFromPlayers = new Dictionary<Player, int>();
		mDamageFromPlayers.Add(mLocalPlayer, 0);
	}

	public virtual void SetState(EnemyState newState)
	{
		if (mDebugComponent != null && mDebugComponent.ShowStateLogWhenChanged)
		{
			Debug.Log(string.Concat(mState, "  ->>>>>>>> ", newState));
		}
		mState = newState;
	}

	public EnemyState GetState()
	{
		return mState;
	}

	public Vector3 GetShieldPosition()
	{
		if (null != mBodyTransform)
		{
			return mBodyTransform.position;
		}
		if (null != entityTransform)
		{
			return entityTransform.position;
		}
		return Vector3.zero;
	}

	public Quaternion GetShieldRotation()
	{
		if (null != entityTransform)
		{
			return entityTransform.rotation;
		}
		return Quaternion.identity;
	}

	public virtual Vector3 GetShieldScale()
	{
		return new Vector3(1f, 1f, 1f);
	}

	public Vector3 GetBodyPosition()
	{
		if (null != mBodyTransform)
		{
			return mBodyTransform.position;
		}
		if (null != entityTransform)
		{
			return entityTransform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetHeadPosition()
	{
		if (null != mHeadTransform)
		{
			return mHeadTransform.position;
		}
		if (null != entityTransform)
		{
			return entityTransform.position;
		}
		return Vector3.zero;
	}

	protected override void SendShieldRecoveryRequest()
	{
		if (IsMasterPlayer)
		{
			int deltaShield = Shield - mPrevShield;
			EnemyShieldRecoveryRequest request = new EnemyShieldRecoveryRequest(PointID, EnemyID, deltaShield);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		mPrevShield = Shield;
	}

	public new virtual void OnHit(DamageProperty dp)
	{
		if (!InPlayingState())
		{
			return;
		}
		int num = 0;
		if (dp.isPenetration)
		{
			num += Mathf.CeilToInt((float)dp.damage * 0.3f);
		}
		if (InPlayingState())
		{
			UserStateHUD.GetInstance().PushDamage(dp.damage + num, dp.criticalAttack, dp.elementType, GetHeadPosition());
		}
		ResetShieldRecoveryStartTimer();
		if (Shield > 0)
		{
			Shield -= dp.damage;
			ShowShieldBreak();
			if (Shield < 0)
			{
				dp.damage = -Shield;
				Shield = 0;
			}
			else if ((mState == PATROL_IDLE_STATE || mState == PATROL_STATE) && !mSpawnPointScript.IsAwaking)
			{
				OnDetectTargetPlayer(mLocalPlayer);
			}
			if (Shield > 0)
			{
				dp.damage = 0;
			}
			ShowShield();
		}
		dp.damage += num;
		if (dp.damage <= 0)
		{
			return;
		}
		Hp -= dp.damage;
		gameWorld.GetLocalPlayer().DamageToHealth(dp.damage);
		if (IsElite)
		{
			mIsCriticalAttacked = false;
		}
		else
		{
			mIsCriticalAttacked = dp.criticalAttack;
		}
		if (Hp < 0)
		{
			Hp = 0;
			if (mState == PATROL_IDLE_STATE || mState == PATROL_STATE || mState == AWAKE_STATE)
			{
				mSpawnPointScript.InformTarget(mLocalPlayer);
			}
			StartDead();
			AchievementWithDeath(dp.wType, dp.attackerType, dp.elementType);
		}
		else if (mState == PATROL_IDLE_STATE || mState == PATROL_STATE)
		{
			if (!mSpawnPointScript.IsAwaking)
			{
				OnDetectTargetPlayer(mLocalPlayer);
			}
		}
		else if (mState != AWAKE_STATE && mState != GOBACK_STATE)
		{
			if (mCanGotHit && mOnHitTimer.Ready())
			{
				mOnHitTimer.Do();
				EndCurrentState();
				StartGotHit();
			}
			else
			{
				TryFindCover();
			}
		}
	}

	public virtual void ShowShield()
	{
	}

	public virtual void ShowShieldBreak()
	{
		EffectPlayer.GetInstance().PlayOthersShieldBreak(GetShieldPosition(), GetShieldRotation() * UnityEngine.Random.rotation, GetShieldScale());
	}

	public virtual void OnDamage(int killerID, int damage)
	{
		if (!InPlayingState())
		{
			return;
		}
		IsHit = true;
		Player player = null;
		foreach (Player key3 in mDamageFromPlayers.Keys)
		{
			if (key3 != null && key3.GetUserID() == killerID)
			{
				player = key3;
				break;
			}
		}
		if (player != null)
		{
			Dictionary<Player, int> dictionary;
			Dictionary<Player, int> dictionary2 = (dictionary = mDamageFromPlayers);
			Player key;
			Player key2 = (key = player);
			int num = dictionary[key];
			dictionary2[key2] = num + damage;
		}
		ResetShieldRecoveryStartTimer();
	}

	public virtual void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		if (!InPlayingState())
		{
			return;
		}
		if (killerID == GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserID() && InPlayingState())
		{
			UserStateHUD.GetInstance().PushDamage(damage, criticalAttack, (ElementType)elementType, GetHeadPosition());
			gameWorld.GetLocalPlayer().DamageToHealth(damage);
			if (weaponType != WeaponType.Melee)
			{
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.I_need_no_weapons, AchievementTrigger.Type.Stop);
				AchievementManager.GetInstance().Trigger(trigger);
			}
		}
		mKillerID = killerID;
		OnDamage(killerID, damage);
		if (currentShield < Shield)
		{
			ShowShield();
			ShowShieldBreak();
		}
		Shield = currentShield;
		if ((mState == PATROL_IDLE_STATE || mState == PATROL_STATE) && !mSpawnPointScript.IsAwaking && mLocalPlayer.GetUserID() == killerID)
		{
			OnDetectTargetPlayer(mLocalPlayer);
		}
		if (currentHp >= Hp)
		{
			return;
		}
		Hp = currentHp;
		if (Hp > 0 && mIsActive)
		{
			PlayOnHitBloodEffect(GetPosition(), (ElementType)elementType);
			if (mState != PATROL_IDLE_STATE && mState != PATROL_STATE && mState != AWAKE_STATE && mState != GOBACK_STATE)
			{
				if (mCanGotHit && mOnHitTimer.Ready())
				{
					mOnHitTimer.Do();
					EndCurrentState();
					StartGotHit();
				}
				else if (IsMasterPlayer)
				{
					TryFindCover();
				}
			}
		}
		if (Hp != 0)
		{
			return;
		}
		if (mState == PATROL_IDLE_STATE || mState == PATROL_STATE || mState == AWAKE_STATE)
		{
			if (mLocalPlayer.GetUserID() == killerID)
			{
				mSpawnPointScript.InformTarget(mLocalPlayer);
			}
			else
			{
				RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(killerID);
				if (remotePlayerByUserID != null)
				{
					mSpawnPointScript.InformTarget(remotePlayerByUserID);
				}
			}
		}
		SetCriticalAttack(criticalAttack);
		StartDead();
		if (killerID == GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserID() && InPlayingState())
		{
			AchievementWithDeath(weaponType, attackerType, (ElementType)elementType);
		}
	}

	public virtual void SetCriticalAttack(bool criticalAttack)
	{
		if (IsElite)
		{
			mIsCriticalAttacked = false;
		}
		else
		{
			mIsCriticalAttacked = criticalAttack;
		}
	}

	public GameUnit GetCurrentTarget()
	{
		return mTarget;
	}

	public virtual void LookAtPointHorizontal(Vector3 targetPoint)
	{
		Vector3 targetPoint2 = targetPoint;
		targetPoint2.y = entityTransform.position.y;
		LookAtPoint(targetPoint2);
	}

	public virtual void LookAtPoint(Vector3 targetPoint)
	{
		if (mLookAtTimer.Ready())
		{
			mLookAtTimer.Do();
			mTargetToLookAt = targetPoint;
			Vector3 from = mTargetToLookAt - entityTransform.position;
			float f = Vector3.Angle(from, entityTransform.forward);
			float num = Mathf.Abs(f) * ((float)Math.PI / 180f);
			mMaxTurnRadian = num * mTurnSpeed;
			SetCanTurn(true);
		}
	}

	public void ChangeTarget(GameUnit target)
	{
		mTarget = target;
		if (mTarget != null && mTarget.InPlayingState() && mTarget.GetTransform() != null)
		{
			mTargetPosition = mTarget.GetTransform().position;
		}
	}

	public virtual void LookAtTargetHorizontal()
	{
		LookAtPointHorizontal(mTargetPosition);
	}

	public virtual void LookAtTarget()
	{
		LookAtPoint(mTargetPosition);
	}

	protected List<Player> GetPotentialPlayerList()
	{
		List<Player> list = new List<Player>();
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			if (mLocalPlayer.InPlayingState())
			{
				list.Add(mLocalPlayer);
			}
		}
		else
		{
			foreach (Player mPlayer in mPlayerList)
			{
				if (mPlayer.InPlayingState())
				{
					list.Add(mPlayer);
				}
			}
		}
		return list;
	}

	private List<GameUnit> GetPotentialTargetList()
	{
		List<GameUnit> list = new List<GameUnit>();
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			if (mLocalPlayer.InPlayingState())
			{
				list.Add(mLocalPlayer);
			}
			Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
			foreach (SummonedItem value in summonedList.Values)
			{
				if (value.InPlayingState())
				{
					list.Add(value);
				}
			}
		}
		else
		{
			foreach (Player mPlayer in mPlayerList)
			{
				if (mPlayer.InPlayingState())
				{
					list.Add(mPlayer);
				}
				Dictionary<string, SummonedItem> summonedList2 = mPlayer.GetSummonedList();
				foreach (SummonedItem value2 in summonedList2.Values)
				{
					if (value2.InPlayingState())
					{
						list.Add(value2);
					}
				}
			}
		}
		return list;
	}

	public GameUnit GetRandomTarget()
	{
		List<GameUnit> potentialTargetList = GetPotentialTargetList();
		if (potentialTargetList.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, potentialTargetList.Count);
			return potentialTargetList[index];
		}
		return null;
	}

	public GameUnit GetNearestTarget()
	{
		GameUnit result = null;
		float num = 99999f;
		List<GameUnit> potentialTargetList = GetPotentialTargetList();
		foreach (GameUnit item in potentialTargetList)
		{
			float num2 = Vector3.Distance(item.GetTransform().position, entityTransform.position);
			if (num2 < num)
			{
				result = item;
				num = num2;
			}
		}
		return result;
	}

	public GameUnit GetFarthestTarget()
	{
		GameUnit result = null;
		float num = 0f;
		List<GameUnit> potentialTargetList = GetPotentialTargetList();
		foreach (GameUnit item in potentialTargetList)
		{
			float num2 = Vector3.Distance(item.GetTransform().position, entityTransform.position);
			if (num2 > num)
			{
				result = item;
				num = num2;
			}
		}
		return result;
	}

	public virtual Vector3 GetAveragePosition()
	{
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return mLocalPlayer.GetTransform().position;
		}
		int num = 0;
		Vector3 vector = Vector3.zero;
		foreach (Player mPlayer in mPlayerList)
		{
			if (mPlayer.InPlayingState())
			{
				num++;
				vector += mPlayer.GetTransform().position;
			}
		}
		if (num > 0)
		{
			mRay = new Ray(vector + 3f * Vector3.up, Vector3.down);
			int num2 = 1 << PhysicsLayer.FLOOR;
			vector = ((!Physics.Raycast(mRay, out mRaycastHit, 10f)) ? mTargetPosition : (mRaycastHit.point + new Vector3(0f, 0.1f, 0f)));
		}
		return (num != 0) ? vector : mLocalPlayer.GetTransform().position;
	}

	public virtual float GetAveragehorizontalDistance()
	{
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return GetDistanceFromLocalPlayer();
		}
		int num = 0;
		float num2 = 0f;
		foreach (Player mPlayer in mPlayerList)
		{
			if (mPlayer.InPlayingState())
			{
				num2 += Vector3.Distance(new Vector3(mPlayer.GetTransform().position.x, 0f, mPlayer.GetTransform().position.z), new Vector3(entityTransform.position.x, 0f, entityTransform.position.z));
				num++;
			}
		}
		return (num != 0) ? (num2 / (float)num) : 0f;
	}

	public virtual float GetNearestDistanceToTargetPlayer()
	{
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return 99999f;
		}
		float num = 99999f;
		foreach (Player mPlayer in mPlayerList)
		{
			if (mPlayer.InPlayingState() && mPlayer != mTarget)
			{
				float num2 = Vector3.Distance(mPlayer.GetTransform().position, mTargetPosition);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	protected virtual bool RaycastTargetPlayer(out RaycastHit rayhit)
	{
		mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), mTargetPosition + new Vector3(0f, 0.5f, 0f) - (entityTransform.position + new Vector3(0f, 0.5f, 0f)));
		return Physics.Raycast(mRay, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER));
	}

	public int GetLoot(LootType lootType)
	{
		int num = 0;
		switch (lootType)
		{
		case LootType.Money:
			num = ((!IsElite) ? UnityEngine.Random.Range(50, 150) : UnityEngine.Random.Range(150, 200));
			break;
		case LootType.Enegy:
			num = ((!IsElite) ? UnityEngine.Random.Range(50, 80) : UnityEngine.Random.Range(100, 150));
			break;
		}
		float num2 = 1f;
		for (int i = 0; i < gameScene.DifficultyLevel; i++)
		{
			num2 *= 1.075f;
		}
		return (int)((float)num * num2);
	}

	public void AddExpToPlayer(bool isLocal)
	{
		if (isLocal)
		{
			mLocalPlayer.Kills++;
		}
		int num = mExperience;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && isLocal)
		{
			num = (int)((float)num * 1.1f);
		}
		if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
		{
			num = (int)((float)num * 0.1f);
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			switch (GameApp.GetInstance().GetGameWorld().GetPlayerCount())
			{
			case 2:
				num = (int)((float)num * 1.05f);
				break;
			case 3:
				num = (int)((float)num * 1.15f);
				break;
			case 4:
				num = (int)((float)num * 1.4f);
				break;
			}
		}
		num = mLocalPlayer.AddExp(num);
		if (IsActive())
		{
			NumberManager.GetInstance().ShowExp(GetHeadPosition(), num);
		}
	}

	public int GetGold()
	{
		return mGold;
	}

	public float GetDistanceFromGameUnit(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 99999f;
		}
		return (entityTransform.position - gameUnit.GetTransform().position).magnitude;
	}

	public float GetDistanceFromLocalPlayer()
	{
		return GetDistanceFromGameUnit(mLocalPlayer);
	}

	public float GetDistanceFromTarget()
	{
		return (entityTransform.position - mTargetPosition).magnitude;
	}

	public float GetHorizontalDistanceFromTarget()
	{
		Vector3 vector = mTargetPosition - entityTransform.position;
		vector.y = 0f;
		return vector.magnitude;
	}

	public float GetHorizontalDistanceFromLocalPlayer()
	{
		return GetHorizontalDistanceFromGameUnit(mLocalPlayer);
	}

	public float GetHorizontalDistanceFromGameUnit(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 99999f;
		}
		Vector3 vector = gameUnit.GetTransform().position - entityTransform.position;
		vector.y = 0f;
		return vector.magnitude;
	}

	public float GetSqrDistanceFromGameUnit(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 99999f;
		}
		return (entityTransform.position - gameUnit.GetTransform().position).sqrMagnitude;
	}

	public float GetSqrDistanceFromLocalPlayer()
	{
		return GetSqrDistanceFromGameUnit(mLocalPlayer);
	}

	public float GetSqrDistanceFromTarget()
	{
		return (entityTransform.position - mTargetPosition).sqrMagnitude;
	}

	public float GetHorizontalSqrDistanceFromGameUnit(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 99999f;
		}
		Vector3 vector = gameUnit.GetTransform().position - entityTransform.position;
		vector.y = 0f;
		return vector.sqrMagnitude;
	}

	public float GetHorizontalSqrDistanceFromLocalPlayer()
	{
		return GetHorizontalSqrDistanceFromGameUnit(mLocalPlayer);
	}

	public float GetHorizontalSqrDistanceFromTarget()
	{
		Vector3 vector = mTargetPosition - entityTransform.position;
		vector.y = 0f;
		return vector.sqrMagnitude;
	}

	public float GetGameUnitHorizontalAngle(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 0f;
		}
		Vector3 vector = new Vector3(entityTransform.forward.x, 0f, entityTransform.forward.z);
		Vector3 vector2 = gameUnit.GetTransform().position - entityTransform.position;
		vector2.y = 0f;
		float num = Vector3.Angle(vector2, vector);
		if (Vector3.Cross(vector, vector2).y < 0f)
		{
			num = 0f - num;
		}
		return num;
	}

	public float GetLocalPlayerHorizontalAngle()
	{
		return GetGameUnitHorizontalAngle(mLocalPlayer);
	}

	public float GetTargetHorizontalAngle()
	{
		Vector3 vector = new Vector3(entityTransform.forward.x, 0f, entityTransform.forward.z);
		Vector3 vector2 = mTargetPosition - entityTransform.position;
		vector2.y = 0f;
		float num = Vector3.Angle(vector2, vector);
		if (Vector3.Cross(vector, vector2).y < 0f)
		{
			num = 0f - num;
		}
		return num;
	}

	public float GetGameUnitHorizontalAbsAngle(GameUnit gameUnit)
	{
		if (gameUnit == null || gameUnit.GetTransform() == null || entityTransform == null)
		{
			return 0f;
		}
		Vector3 to = new Vector3(entityTransform.forward.x, 0f, entityTransform.forward.z);
		Vector3 from = gameUnit.GetTransform().position - entityTransform.position;
		from.y = 0f;
		return Vector3.Angle(from, to);
	}

	public float GetLocalPlayerHorizontalAbsAngle()
	{
		return GetGameUnitHorizontalAbsAngle(mLocalPlayer);
	}

	public float GetTargetHorizontalAbsAngle()
	{
		Vector3 to = new Vector3(entityTransform.forward.x, 0f, entityTransform.forward.z);
		Vector3 from = mTargetPosition - entityTransform.position;
		from.y = 0f;
		return Vector3.Angle(from, to);
	}

	public float GetTargetVerticalAbsAngle(Vector3 pos)
	{
		Vector3 to = new Vector3(0f, entityTransform.forward.y, entityTransform.forward.z);
		return Vector3.Angle(pos, to);
	}

	public virtual void DoShoutAudio()
	{
	}

	public virtual void DoLogic(float deltaTime)
	{
		if (!mIsActive)
		{
			return;
		}
		ApplyBuffToUnitStatus();
		if (mTarget != null)
		{
			if (mTarget.InPlayingState() && mTarget.GetTransform() != null)
			{
				mTargetPosition = mTarget.GetTransform().position;
			}
			else
			{
				mTarget = null;
			}
		}
		if (InPlayingState())
		{
			DoShieldRecovery(deltaTime);
		}
		if (!IsMasterPlayer && mCurrentDeltaCount < mMaxDeltaCount)
		{
			entityTransform.position += mDeltaPosition / mMaxDeltaCount;
			mCurrentDeltaCount++;
		}
		if (CanTurn())
		{
			Vector3 target = mTargetToLookAt - entityTransform.position;
			Vector3 vector = Vector3.RotateTowards(entityTransform.forward, target, mMaxTurnRadian, 100f);
			entityTransform.LookAt(entityTransform.position + vector);
		}
		if (mDebugComponent != null && mDebugComponent.ShowStateLog)
		{
			Debug.Log(string.Concat(base.Name, " :", mState, "  ->>>>>>>> "));
		}
		mState.NextState(this, deltaTime);
		if (null != mNavMeshAgent)
		{
			if (!mIsOnOffMeshLink && mNavMeshAgent.isOnOffMeshLink)
			{
				mPreviousSpeed = mNavMeshAgent.speed;
				mNavMeshAgent.speed = 0.9f;
				mIsOnOffMeshLink = true;
			}
			else if (mIsOnOffMeshLink && !mNavMeshAgent.isOnOffMeshLink)
			{
				mNavMeshAgent.speed = mPreviousSpeed;
				mIsOnOffMeshLink = false;
			}
		}
		if (InPlayingState())
		{
			DoDot();
		}
	}

	public new bool AnimationPlayed(string name, float percent)
	{
		if (animation[name].time >= animation[name].clip.length * percent)
		{
			return true;
		}
		return false;
	}

	public bool isAllPlayerDead()
	{
		bool flag = true;
		if (mLocalPlayer.InPlayingState())
		{
			flag = false;
		}
		if (flag)
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item.InPlayingState())
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	public virtual Vector3 GetGround()
	{
		mFloorHeight = Global.FLOORHEIGHT;
		Vector3 vector = Vector3.up;
		mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, 1 << PhysicsLayer.FLOOR))
		{
			mFloorHeight = mRaycastHit.point.y;
			vector = mRaycastHit.normal;
		}
		if (vector.y < 0f)
		{
			vector = -vector;
		}
		return vector;
	}

	public virtual bool NeedShowDefent()
	{
		return false;
	}

	public virtual bool IsInvincible()
	{
		return false;
	}

	public float GetFloorHeight()
	{
		return mFloorHeight;
	}

	public static GameObject GetEnemyByCollider(Collider c)
	{
		GameObject gameObject = c.gameObject;
		while (gameObject.transform.parent != null && !(gameObject.transform.parent.gameObject.tag == TagName.OBJECT_POOL))
		{
			gameObject = gameObject.transform.parent.gameObject;
		}
		return gameObject;
	}

	public void UpdateElementDamage(DamageProperty dp)
	{
		if (dp.elementType != ElementType.Fire && dp.elementType != ElementType.Shock && dp.elementType != ElementType.Corrosive)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		float num3 = dp.damage;
		if (Shield > 0)
		{
			num2 = (float)dp.damage * ElementWeaponConfig.ShieldDamageBias[(int)(dp.elementType - 2)];
			if (num2 > (float)Shield)
			{
				float num4 = num2 - (float)Shield;
				if (mShieldType == ShieldType.FLESH)
				{
					num4 *= ElementWeaponConfig.ShieldToFleshBias[(int)(dp.elementType - 2)];
				}
				else if (mShieldType == ShieldType.MECHANICAL)
				{
					num4 *= ElementWeaponConfig.ShieldToMechanicalBias[(int)(dp.elementType - 2)];
				}
				num = (float)Shield + num4;
			}
			else
			{
				num = num2;
			}
		}
		else if (mShieldType == ShieldType.FLESH)
		{
			num = (float)dp.damage * ElementWeaponConfig.FleshDamageBias[(int)(dp.elementType - 2)];
		}
		else if (mShieldType == ShieldType.MECHANICAL)
		{
			num = (float)dp.damage * ElementWeaponConfig.MechanicalDamageBias[(int)(dp.elementType - 2)];
		}
		dp.damage = (int)num;
		if (mDebugComponent.ShowElementLog)
		{
			Debug.Log("ElementType: " + dp.elementType);
			Debug.Log("Original Damage: " + num3 + " ,Element Damage: " + (int)num);
		}
		if (dp.isTriggerDlementDot)
		{
			ElementDotData elementDotData = default(ElementDotData);
			elementDotData.type = dp.elementType;
			elementDotData.time = dp.elementDotTime;
			elementDotData.damage = (int)(dp.elementDotDamage * num3);
			elementDotData.isPenetration = dp.isPenetration;
			elementDotData.unitLevel = dp.unitLevel;
			elementDotData.weaponLevel = dp.weaponLevel;
			elementDotData.weaponType = dp.wType;
			mElementDotList.Clear();
			mElementDotList.Add(elementDotData);
			mDotTimer.Do();
			if (mDebugComponent.ShowElementLog)
			{
				Debug.Log("Dot Damage: " + elementDotData.damage + " ,Dot Time: " + elementDotData.time);
			}
		}
	}

	public virtual void HitEnemy(DamageProperty dp)
	{
		if (IsInvincible())
		{
			return;
		}
		if (dp.attackerType != DamageProperty.AttackerType._HealingStation)
		{
			UpdateElementDamage(dp);
		}
		int num = dp.damage;
		if (NeedShowDefent())
		{
			num /= 10;
		}
		if (!GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode() && dp.attackerType != DamageProperty.AttackerType._HealingStation)
		{
			num = DamageLevelSuppress(num, dp.unitLevel, dp.weaponLevel, Level);
		}
		dp.damage = num;
		if (dp.isLocal && dp.wType != WeaponType.Melee)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.I_need_no_weapons, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger);
		}
		if (dp.isLocal)
		{
			if (dp.criticalAttack)
			{
				AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Head_on_the_target, AchievementTrigger.Type.Start);
				AchievementManager.GetInstance().Trigger(trigger2);
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Head_on_the_target, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
			}
			else
			{
				AchievementTrigger trigger3 = AchievementTrigger.Create(AchievementID.Head_on_the_target, AchievementTrigger.Type.Stop);
				AchievementManager.GetInstance().Trigger(trigger3);
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			if (dp.isLocal)
			{
				EnemyOnHitRequest request = new EnemyOnHitRequest(PointID, EnemyID, num, dp.criticalAttack, (byte)dp.elementType, dp.isPenetration, dp.wType, dp.attackerType);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			OnHit(dp);
		}
		PlayOnHitBloodEffect(dp.hitpoint, dp.elementType);
		Player localPlayer = gameWorld.GetLocalPlayer();
		localPlayer.GetCharacterSkillManager().OnHitEnemyTrigger(localPlayer, dp.wType, this);
	}

	public virtual void UpdateNavMeshInCatching(bool force)
	{
		if (!force && !(Time.time - mLastUpdateNavMeshTime > 6f / mRunSpeed))
		{
			return;
		}
		mLastUpdateNavMeshTime = Time.time;
		if (mTarget != null)
		{
			Vector3 destination = mTargetPosition;
			if (null != mNavMeshAgent)
			{
				mNavMeshAgent.Resume();
				mNavMeshAgent.SetDestination(destination);
				mNavMeshAgent.speed = mRunSpeed;
				SetCanTurn(false);
			}
		}
	}

	public virtual bool TryPathForNavMesh(Vector3 destination)
	{
		if (null != mNavMeshAgent)
		{
			UnityEngine.AI.NavMeshHit hit = default(UnityEngine.AI.NavMeshHit);
			if (!mNavMeshAgent.Raycast(destination, out hit))
			{
				mNavMeshAgent.Resume();
				return true;
			}
		}
		return false;
	}

	public virtual void SetNavMeshForPatrol()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			mNavMeshAgent.speed = mPatrolSpeed;
			SetCanTurn(false);
		}
	}

	public virtual void SetNavMeshForGoBack()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			Debug.Log("SetNavMeshForGoBack: " + mPatrolTarget);
			mNavMeshAgent.speed = 2f * mRunSpeed;
			SetCanTurn(false);
		}
	}

	public virtual void StopNavMesh()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Stop(false);
		}
	}

	public virtual void CreateNavMeshAgent()
	{
		if (mNavMeshAgent == null)
		{
			entityObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent = entityObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			mNavMeshAgent.radius = cc.radius;
			mNavMeshAgent.height = 1f;
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

	public void CreateDebugComponent()
	{
		entityObject.AddComponent<DebugComponent>();
		mDebugComponent = entityObject.GetComponent<DebugComponent>();
	}

	public void DestroyNavMeshAgent()
	{
		if (mNavMeshAgent != null)
		{
			UnityEngine.Object.Destroy(mNavMeshAgent);
			mNavMeshAgent = null;
		}
	}

	public virtual bool CanTurn()
	{
		return mCanTurn && mState != DEAD_STATE && mState != FADEOUT_STATE;
	}

	public void SetSpawnPointScript(BaseEnemySpawnScript script)
	{
		mSpawnPointScript = script;
	}

	public void SetPatrolTarget(Vector3 point)
	{
		mPatrolTarget = point;
	}

	public virtual void OnInformTarget(GameUnit target)
	{
		if (mState == PATROL_IDLE_STATE || mState == PATROL_STATE)
		{
			EndCurrentState();
			ChangeTarget(target);
			StartEnemyIdleWithoutResetTime();
		}
	}

	public virtual void OnDetectTargetPlayer(Player targetPlayer)
	{
		if (targetPlayer != null)
		{
			EndCurrentState();
			StartAwake();
			ChangeTarget(targetPlayer);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				Debug.Log("targetPlayer.GetUserID() = " + targetPlayer.GetUserID());
				EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(PointID, EnemyID, targetPlayer);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				EnemyStateRequest request2 = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.AWAKE, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	protected bool DetectPlayer()
	{
		bool result = false;
		if (!mSpawnPointScript.IsAwaking && mDetectTimer.Ready())
		{
			mDetectTimer.Do();
			if (mLocalPlayer != null && canDetectPlayer(mLocalPlayer))
			{
				OnDetectTargetPlayer(mLocalPlayer);
				result = true;
			}
		}
		return result;
	}

	protected bool canDetectPlayer(Player player)
	{
		bool flag = false;
		bool result = false;
		if (GetSqrDistanceFromGameUnit(player) < (float)(mDetectRadius * mDetectRadius))
		{
			flag = true;
		}
		if (!flag && GetSqrDistanceFromGameUnit(player) < (float)(mDetectSectorRadius * mDetectSectorRadius) && GetGameUnitHorizontalAbsAngle(player) < (float)mDetectSectorAngle)
		{
			flag = true;
		}
		if (flag)
		{
			mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), player.GetTransform().position - entityTransform.position);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
			if (Physics.Raycast(mRay, out mRaycastHit, GetDistanceFromGameUnit(player), layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
			{
				result = true;
			}
		}
		return result;
	}

	public void DetectBullet(Vector3 hitPosition)
	{
		if (SpawnType != ESpawnType.ARENA && SpawnType != ESpawnType.BOSS_RUSH && mIsActive && !mSpawnPointScript.IsAwaking && (mState == PATROL_IDLE_STATE || mState == PATROL_STATE) && (entityTransform.position - hitPosition).sqrMagnitude < (float)(mDetectRadius * mDetectRadius))
		{
			mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), hitPosition - entityTransform.position);
			float distance = (hitPosition - entityTransform.position).magnitude - 0.1f;
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL);
			if (!Physics.Raycast(mRay, out mRaycastHit, distance, layerMask))
			{
				OnDetectTargetPlayer(mLocalPlayer);
			}
		}
	}

	public void DetectWeaponSound()
	{
		if (SpawnType != ESpawnType.ARENA && SpawnType != ESpawnType.BOSS_RUSH && mIsActive && !mSpawnPointScript.IsAwaking && (mState == PATROL_IDLE_STATE || mState == PATROL_STATE) && GetSqrDistanceFromLocalPlayer() < 900f)
		{
			OnDetectTargetPlayer(mLocalPlayer);
		}
	}

	public void SetCanTurn(bool canTurn)
	{
		mCanTurn = canTurn;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.updateRotation = !canTurn;
		}
	}

	public List<PreSpawnBuffer> GetPreSpawnBuffer()
	{
		return mPreSpawnItems;
	}

	public string GetCallName()
	{
		return mEnemyConfig.CallName;
	}

	public int GetRowIndex()
	{
		return mRowIndexInDataTable;
	}

	protected bool canHitTargetPlayer()
	{
		if (mTarget != null && GetGameUnitHorizontalAbsAngle(mTarget) > 20f)
		{
			return false;
		}
		Vector3 direction = mTargetPosition - entityTransform.position;
		mRay = new Ray(entityTransform.position + new Vector3(0f, mHitCheckHeight, 0f), direction);
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
		if (Physics.Raycast(mRay, out mRaycastHit, GetDistanceFromTarget(), layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
		{
			return true;
		}
		return false;
	}

	protected virtual bool TryFindCover()
	{
		return false;
	}

	protected void CheckKnocked(GameUnit gameUnit, float speed)
	{
		Vector3 from = new Vector3(gameUnit.GetTransform().forward.x, 0f, gameUnit.GetTransform().forward.z);
		Vector3 to = entityTransform.position - gameUnit.GetTransform().position;
		to.y = 0f;
		float f = Vector3.Angle(from, to);
		if (Mathf.Abs(f) > 90f)
		{
			speed = 0f - speed;
		}
		gameUnit.OnKnocked(speed);
	}

	public virtual void EndCurrentState()
	{
		if (PATROL_IDLE_STATE == mState)
		{
			EndPatrolIdle();
		}
		else if (PATROL_STATE == mState)
		{
			EndPatrol();
		}
		else if (IDLE_STATE == mState)
		{
			EndEnemyIdle();
		}
		else if (CATCHING_STATE == mState)
		{
			EndCatching();
		}
		else if (ATTACK_STATE == mState)
		{
			EndEnemyAttack();
		}
		else if (GOTHIT_STATE == mState)
		{
			EndGotHit();
		}
		else if (DEAD_STATE == mState)
		{
			EndDead();
		}
		else if (GOBACK_STATE == mState)
		{
			EndGoBack();
		}
		else if (AWAKE_STATE == mState)
		{
			EndAwake();
		}
		else if (RAGE_STATE == mState)
		{
			EndRage();
		}
	}

	public virtual bool IsCrit()
	{
		return false;
	}

	public void DoDot()
	{
		if (!mDotTimer.Ready())
		{
			return;
		}
		mDotTimer.Do();
		int[] array = new int[3];
		bool[] array2 = new bool[3];
		int[] array3 = new int[3];
		int[] array4 = new int[3];
		if (mElementDotList.Count > 0)
		{
			ElementDotData elementDotData = (ElementDotData)mElementDotList[0];
			array[(int)(elementDotData.type - 2)] += elementDotData.damage;
			array2[(int)(elementDotData.type - 2)] = elementDotData.isPenetration;
			array3[(int)(elementDotData.type - 2)] = elementDotData.unitLevel;
			array4[(int)(elementDotData.type - 2)] = elementDotData.weaponLevel;
			elementDotData.time--;
			if (elementDotData.time <= 0)
			{
				mElementDotList.RemoveAt(0);
			}
			else
			{
				mElementDotList[0] = elementDotData;
			}
		}
		if (array[1] > 0 && array[1] > Shield && !array2[1])
		{
			float num = array[1] - Shield;
			if (mShieldType == ShieldType.FLESH)
			{
				num *= ElementWeaponConfig.ShieldToFleshBias[1];
			}
			else if (mShieldType == ShieldType.MECHANICAL)
			{
				num *= ElementWeaponConfig.ShieldToMechanicalBias[1];
			}
			array[1] = Shield + (int)num;
		}
		for (int i = 0; i < 3; i++)
		{
			if (array[i] <= 0)
			{
				continue;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				if (GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					int num2 = array[i];
					if (NeedShowDefent())
					{
						num2 /= 10;
					}
					if (!GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
					{
						num2 = DamageLevelSuppress(num2, array3[i], array4[i], Level);
					}
					EnemyOnHitRequest request = new EnemyOnHitRequest(PointID, EnemyID, num2, false, (byte)(i + 2), array2[i], WeaponType.NoGun, DamageProperty.AttackerType._Dot);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				continue;
			}
			DamageProperty damageProperty = new DamageProperty();
			switch (i)
			{
			case 0:
				damageProperty.elementType = ElementType.Fire;
				break;
			case 1:
				damageProperty.elementType = ElementType.Shock;
				break;
			case 2:
				damageProperty.elementType = ElementType.Corrosive;
				break;
			}
			if (!GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode())
			{
				array[i] = DamageLevelSuppress(array[i], array3[i], array4[i], Level);
			}
			damageProperty.damage = array[i];
			damageProperty.criticalAttack = false;
			damageProperty.isPenetration = array2[i];
			damageProperty.unitLevel = array3[i];
			damageProperty.weaponLevel = array4[i];
			damageProperty.wType = WeaponType.NoGun;
			damageProperty.attackerType = DamageProperty.AttackerType._Dot;
			OnHit(damageProperty);
		}
	}

	protected virtual void PlayAwakeSound()
	{
	}

	public virtual void StartAwake()
	{
		SetState(AWAKE_STATE);
		mSpawnPointScript.IsAwaking = true;
		LookAtTargetHorizontal();
		PlayAwakeSound();
	}

	public virtual void DoAwake()
	{
		if (mCanAwake && !mSpawnPointScript.HasAwaked)
		{
			PlayAnimation(AnimationString.ENEMY_AWAKE, WrapMode.ClampForever, 1f);
			LookAtTargetHorizontal();
			if (AnimationPlayed(AnimationString.ENEMY_AWAKE, 1f))
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

	protected virtual void EndAwake()
	{
	}

	public virtual void StartCatching()
	{
		SetState(CATCHING_STATE);
		SetUpdatePosTimeNow();
		UpdateNavMeshInCatching(true);
	}

	protected virtual void PlayRunningSound()
	{
	}

	protected virtual string GetRunAnimationName()
	{
		return AnimationString.ENEMY_RUN;
	}

	protected virtual string GetWalkAnimationName()
	{
		return AnimationString.ENEMY_WALK;
	}

	public virtual void DoCatching()
	{
		PlayAnimation(GetRunAnimationName(), WrapMode.Loop);
		UpdateNavMeshInCatching(false);
		PlayRunningSound();
		if (!IsMasterPlayer || NeedGoBack())
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			if (GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.CATCHING, GetTransform().position, (short)(SpeedRate * 100f));
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else if (CheckTargetInCatching())
			{
				MakeDecisionInCatching();
			}
		}
		else if (CheckTargetInCatching())
		{
			MakeDecisionInCatching();
		}
	}

	protected virtual bool CheckTargetInCatching()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null)
		{
			EndCatching();
			if (SpawnType == ESpawnType.ARENA || SpawnType == ESpawnType.BOSS_RUSH)
			{
				StartEnemyIdle();
			}
			else
			{
				StartGoBack(GetGoBackPosition());
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				if (SpawnType == ESpawnType.ARENA || SpawnType == ESpawnType.BOSS_RUSH)
				{
					EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.IDLE, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				else
				{
					EnemyStateRequest request2 = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.GO_BACK, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
			return false;
		}
		return true;
	}

	protected virtual void MakeDecisionInCatching()
	{
	}

	protected virtual void EndCatching()
	{
		StopNavMesh();
	}

	protected virtual void PlayDeadSound()
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (num < 50)
		{
			AudioManager.GetInstance().PlaySound("RPG_Audio/Enemy/enemy_dead1");
		}
		else
		{
			AudioManager.GetInstance().PlaySound("RPG_Audio/Enemy/enemy_dead2");
		}
	}

	protected virtual void PlayOnHitBloodEffect(Vector3 position, ElementType elementType)
	{
		if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying() && GetLastBloodEffectTimer().Ready())
		{
			gameScene.GetEffectPool(EffectPoolType.HIT_BLOOD).CreateObject(position, Vector3.zero, Quaternion.identity);
			GetLastBloodEffectTimer().Do();
		}
		gameScene.PlayOnHitElementEffect(position, elementType);
	}

	protected virtual void PlayDeadBloodEffect()
	{
		if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying())
		{
			GameObject original = Resources.Load("Effect/update_effect/RPG_blood_big") as GameObject;
			UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up * 1.2f, Quaternion.identity);
		}
	}

	protected virtual void PlayGroundDeadBloodEffect(Vector3 normal)
	{
		if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying())
		{
			GameObject gameObject = null;
			int num = UnityEngine.Random.Range(0, 100);
			int num2 = num % 3 + 1;
			gameObject = Resources.Load("Effect/Blood_Ground" + num2) as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, new Vector3(entityTransform.position.x, mFloorHeight + 0.1f, entityTransform.position.z), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
			gameObject2.transform.localScale *= 0.3f;
			ScaleScript scaleScript = gameObject2.AddComponent<ScaleScript>();
			scaleScript.enabled = true;
			scaleScript.scaleSpeed = 0.1f;
			scaleScript.enableMaxScale = true;
			scaleScript.maxScale = 0.3f;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, normal);
			gameObject2.transform.rotation = quaternion * gameObject2.transform.rotation;
		}
	}

	public virtual void StartDead()
	{
		if (!InPlayingState())
		{
			return;
		}
		if (Arena.GetInstance().IsCurrentSceneArena())
		{
			EnemySpawnScript.GetInstance().NotifyOneEnemyDead();
		}
		EndCurrentState();
		SetState(DEAD_STATE);
		PlayDeadSound();
		GetGround();
		GameObject[] array = mHitObjectArray;
		foreach (GameObject gameObject in array)
		{
			gameObject.layer = PhysicsLayer.DEFAULT;
		}
		entityObject.layer = PhysicsLayer.DEADBODY;
		if (mShadowObject != null)
		{
			UnityEngine.Object.Destroy(mShadowObject);
		}
		PlayDeadBloodEffect();
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			AddExpToPlayer(true);
			Player localPlayer = gameWorld.GetLocalPlayer();
			localPlayer.GetCharacterSkillManager().OnEnemyKillTrigger(localPlayer);
			if (mIsCriticalAttacked)
			{
				localPlayer.GetCharacterSkillManager().OnEnemyCriticalKillTrigger(localPlayer);
			}
			localPlayer.GetUserState().GetQuestProgress().OnQuestProgressEnemyKill(mGroupID);
			if (localPlayer.DYING_STATE.InDyingState)
			{
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Not_yet_not);
				achievementTrigger.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
			}
		}
		else
		{
			AddExpToPlayer(false);
			Player localPlayer2 = gameWorld.GetLocalPlayer();
			if (mKillerID == localPlayer2.GetUserID())
			{
				localPlayer2.GetCharacterSkillManager().OnEnemyKillTrigger(localPlayer2);
				if (mIsCriticalAttacked)
				{
					localPlayer2.GetCharacterSkillManager().OnEnemyCriticalKillTrigger(localPlayer2);
				}
				if (localPlayer2.DYING_STATE.InDyingState)
				{
					AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.Not_yet_not);
					achievementTrigger2.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger2);
				}
			}
		}
		DestroyNavMeshAgent();
	}

	public virtual void DoDead()
	{
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		if (mOnGround)
		{
			if (mDeadTimer.Ready() && AnimationPlayed(AnimationString.ENEMY_DEAD, 1f))
			{
				EndDead();
				StartFadeout();
			}
		}
		else if (entityTransform.position.y <= mFloorHeight)
		{
			entityTransform.position = new Vector3(entityTransform.position.x, mFloorHeight, entityTransform.position.z);
			mOnGround = true;
			mDeadTimer.SetTimer(3f, false);
			Vector3 ground = GetGround();
			entityTransform.rotation = Quaternion.FromToRotation(entityTransform.up, ground) * entityTransform.rotation;
			GameApp.GetInstance().GetLootManager().OnLoot(this);
			PlayGroundDeadBloodEffect(ground);
		}
		else
		{
			entityTransform.Translate(10f * Vector3.down * Time.deltaTime, Space.World);
		}
	}

	public virtual void EndDead()
	{
	}

	public virtual void StartEnemyAttack()
	{
		SetState(ATTACK_STATE);
		LookAtTargetHorizontal();
	}

	public void CheckEnemyAttack(MeleeAttackData AttackData)
	{
		if (mLocalPlayer.CheckHitTimerReady(this) && AnimationPlayed(AttackData.Animation, AttackData.StartPercent) && !AnimationPlayed(AttackData.Animation, AttackData.EndPercent))
		{
			Vector3 vector = AttackData.Trans.InverseTransformPoint(mLocalPlayer.GetTransform().position);
			if (Vector3.Distance(AttackData.Trans.position, mLocalPlayer.GetTransform().position) < AttackData.Range && vector.z > 0f && Mathf.Abs(vector.x / vector.z) < Mathf.Tan((float)Math.PI / 180f * AttackData.Angle))
			{
				mLocalPlayer.OnHit(AttackData.Damage, this);
				mLocalPlayer.ResetCheckHitTimer(this);
				CheckKnocked(mLocalPlayer, AttackData.KnockedSpeed);
			}
		}
		Dictionary<string, SummonedItem> summonedList = mLocalPlayer.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.CheckHitTimerReady(this) && AnimationPlayed(AttackData.Animation, AttackData.StartPercent) && !AnimationPlayed(AttackData.Animation, AttackData.EndPercent))
			{
				Vector3 vector2 = AttackData.Trans.InverseTransformPoint(value.GetTransform().position);
				if (Vector3.Distance(AttackData.Trans.position, value.GetTransform().position) < AttackData.Range && vector2.z > 0f && Mathf.Abs(vector2.x / vector2.z) < Mathf.Tan((float)Math.PI / 180f * AttackData.Angle))
				{
					value.OnHit(AttackData.Damage);
					value.ResetCheckHitTimer(this);
					CheckKnocked(value, AttackData.KnockedSpeed);
				}
			}
		}
	}

	protected virtual void CheckEnemyAttack()
	{
	}

	public virtual void DoEnemyAttack()
	{
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		CheckEnemyAttack();
		if (AnimationPlayed(AnimationString.ENEMY_ATTACK, 1f))
		{
			EndEnemyAttack();
			StartEnemyIdle();
		}
	}

	protected virtual void EndEnemyAttack()
	{
	}

	public virtual void StartEnemyIdle()
	{
		SetState(IDLE_STATE);
		SetIdleTimeNow();
		if (IsMasterPlayer)
		{
			ChooseTargetPlayer(false);
		}
	}

	protected virtual void StartEnemyIdleWithoutResetTime()
	{
		SetState(IDLE_STATE);
		mLastIdleTime = Time.time - mIdleTime + UnityEngine.Random.Range(0f, 0.5f);
		if (IsMasterPlayer)
		{
			ChooseTargetPlayer(false);
		}
	}

	public virtual void DoEnemyIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (IsMasterPlayer && !NeedGoBack() && CheckTargetInEnemyIdle())
		{
			MakeDecisionInEnemyIdle();
		}
	}

	protected virtual bool CheckTargetInEnemyIdle()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null)
		{
			if (SpawnType != ESpawnType.ARENA && SpawnType != ESpawnType.BOSS_RUSH)
			{
				EndEnemyIdle();
				StartGoBack(GetGoBackPosition());
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.GO_BACK, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			return false;
		}
		return true;
	}

	protected virtual void MakeDecisionInEnemyIdle()
	{
	}

	protected virtual void ChooseTargetPlayer(bool force)
	{
		if (!force && !mChooseTargetPlayerTimer.Ready())
		{
			return;
		}
		mChooseTargetPlayerTimer.Do();
		Player player = null;
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			player = mLocalPlayer;
		}
		else
		{
			Player player2 = null;
			float num = 9999f;
			float num2 = 0f;
			foreach (KeyValuePair<Player, int> mDamageFromPlayer in mDamageFromPlayers)
			{
				if (mDamageFromPlayer.Key.InPlayingState())
				{
					float distanceFromGameUnit = GetDistanceFromGameUnit(mDamageFromPlayer.Key);
					if (distanceFromGameUnit < num)
					{
						player2 = mDamageFromPlayer.Key;
						num = distanceFromGameUnit;
					}
					float num3 = (float)mDamageFromPlayer.Value / distanceFromGameUnit;
					if (num3 > num2)
					{
						player = mDamageFromPlayer.Key;
						num2 = num3;
					}
				}
			}
			if (player == null)
			{
				player = player2;
			}
		}
		if (player == null)
		{
			mTarget = null;
			return;
		}
		GameUnit gameUnit = player;
		float num4 = GetDistanceFromGameUnit(player);
		Dictionary<string, SummonedItem> summonedList = player.GetSummonedList();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.InPlayingState())
			{
				float distanceFromGameUnit2 = GetDistanceFromGameUnit(value);
				if (distanceFromGameUnit2 < num4)
				{
					gameUnit = value;
					num4 = distanceFromGameUnit2;
				}
			}
		}
		if (gameUnit == null)
		{
			mTarget = null;
		}
		else if (gameUnit != mTarget)
		{
			ChangeTarget(gameUnit);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(PointID, EnemyID, gameUnit);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected virtual void EndEnemyIdle()
	{
	}

	protected virtual void StartFadeout()
	{
		SetState(FADEOUT_STATE);
		mDeadRemoveBodyTimer.SetTimer(3f, false);
		ChangeShader();
	}

	public virtual void DoFadeout()
	{
		DoFadeOut(Time.deltaTime);
		if (mDeadRemoveBodyTimer.Ready())
		{
			EndFadeout();
		}
	}

	protected virtual void EndFadeout()
	{
		Deactivate();
	}

	protected virtual bool NeedGoBack()
	{
		if (SpawnType == ESpawnType.ARENA || SpawnType == ESpawnType.BOSS_RUSH)
		{
			return false;
		}
		if (mCheckGoBackTimer.Ready())
		{
			mCheckGoBackTimer.Do();
			Vector3 vector = mSpawnPointScript.gameObject.transform.position - entityTransform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > (float)(mActivityRadius * mActivityRadius))
			{
				EndCurrentState();
				StartGoBack(GetGoBackPosition());
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.GO_BACK, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				return true;
			}
		}
		return false;
	}

	protected virtual Vector3 GetGoBackPosition()
	{
		return mSpawnPointScript.gameObject.transform.position;
	}

	protected virtual bool EndGoBackCondition()
	{
		return (mPatrolTarget - entityTransform.position).sqrMagnitude < 49f || GetGoBackTimeDuration() > mMaxGoBackTime;
	}

	public virtual void StartGoBack(Vector3 target)
	{
		SetState(GOBACK_STATE);
		SetPatrolTarget(target);
		SetNavMeshForGoBack();
		SetUpdatePosTimeNow();
		SetGoBackTimeNow();
	}

	public virtual void DoGoBack()
	{
		PlayAnimation(GetRunAnimationName(), WrapMode.Loop);
		if (EndGoBackCondition())
		{
			EndGoBack();
			StartPatrolIdle();
		}
		else if (IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.GO_BACK, GetTransform().position, (short)(SpeedRate * 100f));
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected virtual void EndGoBack()
	{
		StopNavMesh();
		Hp = base.MaxHp;
		Shield = base.MaxShield;
		if (IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			EnemyFullHpShieldRequest request = new EnemyFullHpShieldRequest(PointID, EnemyID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected virtual void PlayGotHitSound()
	{
	}

	public virtual void StartGotHit()
	{
		SetState(GOTHIT_STATE);
		StopNavMesh();
		PlayGotHitSound();
	}

	public virtual void DoGotHit()
	{
		PlayAnimation(AnimationString.ENEMY_GOTHIT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.ENEMY_GOTHIT, 1f))
		{
			EndGotHit();
			if (mCanRage && !mHasRaged && (float)Hp < (float)base.MaxHp * mRagePercent)
			{
				StartRage();
			}
			else
			{
				StartEnemyIdleWithoutResetTime();
			}
		}
	}

	protected virtual void EndGotHit()
	{
	}

	public virtual void StartPatrolIdleWithoutResetTime()
	{
		SetState(PATROL_IDLE_STATE);
		mLastIdleTime = Time.time - mIdleTime + UnityEngine.Random.Range(0f, 0.2f);
	}

	public virtual void StartPatrolIdle()
	{
		SetState(PATROL_IDLE_STATE);
		SetIdleTimeNow();
	}

	public virtual void DoPatrolIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (DetectPlayer() || !IsMasterPlayer || !(GetIdleTimeDuration() > mPatrolIdleTime))
		{
			return;
		}
		Vector3 vector = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
		vector.Normalize();
		float num = UnityEngine.Random.Range(10f, 20f);
		Vector3 vector2 = mSpawnPointScript.gameObject.transform.position + vector * num;
		if (TryPathForNavMesh(vector2))
		{
			StartPatrol(vector2);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.PATROL, entityTransform.position, vector2);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected virtual void MakeDecisionInPatrolIdle()
	{
	}

	protected virtual void EndPatrolIdle()
	{
	}

	public virtual void StartPatrol(Vector3 patrolTarget)
	{
		SetState(PATROL_STATE);
		SetPatrolTimeNow();
		SetPatrolTarget(patrolTarget);
		SetNavMeshForPatrol();
		SetUpdatePosTimeNow();
	}

	protected virtual void PlayWalkSound()
	{
	}

	public virtual void DoPatrol()
	{
		Debug.DrawLine(entityTransform.position, mPatrolTarget, Color.blue);
		PlayAnimation(GetWalkAnimationName(), WrapMode.Loop);
		PlayWalkSound();
		if (!DetectPlayer())
		{
			if (EndPatrolCondition())
			{
				EndPatrol();
				StartPatrolIdle();
			}
			else if (IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request = new EnemyStateRequest(PointID, EnemyID, EnemyStateConst.PATROL, GetTransform().position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected virtual bool EndPatrolCondition()
	{
		return GetPatrolTimeDuration() > 15f || (mPatrolTarget - entityTransform.position).sqrMagnitude < 4f;
	}

	protected virtual void EndPatrol()
	{
		StopNavMesh();
	}

	protected virtual void PlayRageSound()
	{
	}

	public virtual void StartRage()
	{
		SetState(RAGE_STATE);
		mHasRaged = true;
		PlayRageSound();
	}

	public virtual void DoRage()
	{
		PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
		{
			EndRage();
			StartEnemyIdleWithoutResetTime();
		}
	}

	protected virtual void EndRage()
	{
	}

	protected virtual void ChangeShader()
	{
		foreach (KeyValuePair<string, Renderer> entityRenderer in entityRenderers)
		{
			Material[] materials = entityRenderer.Value.materials;
			foreach (Material material in materials)
			{
				Texture mainTexture = material.mainTexture;
				material.shader = gameScene.TransparentShader;
				material.SetColor(COLOR_PROPERTY_NAME, Color.gray);
				material.SetTexture(MAIN_TEX_NAME, mainTexture);
			}
		}
	}

	public void DoFadeOut(float deltaTime)
	{
		string propertyName = "_TintColor";
		foreach (KeyValuePair<string, Renderer> entityRenderer in entityRenderers)
		{
			Material[] materials = entityRenderer.Value.materials;
			foreach (Material material in materials)
			{
				Color color = material.GetColor(propertyName);
				color.a -= 0.5f * deltaTime;
				if (color.a > 0f)
				{
					material.SetColor(propertyName, color);
				}
			}
		}
	}

	public void ApplyBuffToUnitStatus()
	{
		detailedProperties.Reset();
		skillMgr.ApplyAllStateSkills(this);
		mWalkSpeed = mWalkSpeedInit * (1f + detailedProperties.SpeedBonus);
		mRunSpeed = mRunSpeedInit * (1f + detailedProperties.SpeedBonus);
		IncreaseElementResistance(ElementType.Fire, detailedProperties.ElementsResistanceBonus[0]);
		IncreaseElementResistance(ElementType.Shock, detailedProperties.ElementsResistanceBonus[1]);
		IncreaseElementResistance(ElementType.Corrosive, detailedProperties.ElementsResistanceBonus[2]);
		if (SpeedRate < 0f)
		{
			if (!isSpeedDownEffectAdded)
			{
				isSpeedDownEffectAdded = true;
				GameObject original = Resources.Load("RPG_effect/RPG_SpeedDown") as GameObject;
				GameObject gameObject = UnityEngine.Object.Instantiate(original, entityTransform.position, Quaternion.identity) as GameObject;
				gameObject.transform.parent = mBodyTransform;
			}
		}
		else if (isSpeedDownEffectAdded)
		{
			isSpeedDownEffectAdded = false;
			Transform transform = mBodyTransform.Find("RPG_SpeedDown(Clone)");
			if (transform != null)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			if (IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				EnemySpeedDownOverRequest request = new EnemySpeedDownOverRequest(PointID, EnemyID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected virtual bool IsJumping()
	{
		return false;
	}

	private void AchievementWithDeath(WeaponType weaponType, DamageProperty.AttackerType attackerType, ElementType elementType)
	{
		switch (attackerType)
		{
		case DamageProperty.AttackerType._PlayerOrEnemy:
		{
			if (weaponType == WeaponType.Sniper)
			{
				AchievementTrigger achievementTrigger2 = AchievementTrigger.Create(AchievementID.No_survival, AchievementTrigger.Type.Data);
				achievementTrigger2.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger2);
			}
			if (IsJumping() && weaponType != WeaponType.Melee && weaponType != WeaponType.Grenade)
			{
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Duck_Hunt, AchievementTrigger.Type.Data);
				AchievementManager.GetInstance().Trigger(trigger);
			}
			AchievementTrigger achievementTrigger3 = AchievementTrigger.Create(AchievementID.Hulk, AchievementTrigger.Type.Data);
			achievementTrigger3.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger3);
			AchievementTrigger achievementTrigger4 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Data);
			achievementTrigger4.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger4);
			if (weaponType == WeaponType.Melee)
			{
				AchievementTrigger achievementTrigger5 = AchievementTrigger.Create(AchievementID.I_need_no_weapons, AchievementTrigger.Type.Data);
				achievementTrigger5.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger5);
				AchievementTrigger achievementTrigger6 = AchievementTrigger.Create(AchievementID.Fighter, AchievementTrigger.Type.Data);
				achievementTrigger6.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger6);
			}
			if (weaponType != WeaponType.Melee)
			{
				AchievementTrigger achievementTrigger7 = AchievementTrigger.Create(AchievementID.Hitman, AchievementTrigger.Type.Data);
				achievementTrigger7.PutData(1);
				AchievementManager.GetInstance().Trigger(achievementTrigger7);
			}
			break;
		}
		case DamageProperty.AttackerType._EngineerGun:
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Take_a_rest, AchievementTrigger.Type.Data);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			break;
		}
		}
		if (attackerType == DamageProperty.AttackerType._Dot && elementType == ElementType.Corrosive)
		{
			AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Poisoner, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger2);
			AchievementTrigger achievementTrigger8 = AchievementTrigger.Create(AchievementID.Poisoner, AchievementTrigger.Type.Data);
			achievementTrigger8.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger8);
		}
		else
		{
			AchievementTrigger trigger3 = AchievementTrigger.Create(AchievementID.Poisoner, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger3);
		}
		AchievementTrigger trigger4 = AchievementTrigger.Create(AchievementID.Reaper, AchievementTrigger.Type.Start);
		AchievementManager.GetInstance().Trigger(trigger4);
		AchievementTrigger achievementTrigger9 = AchievementTrigger.Create(AchievementID.Reaper, AchievementTrigger.Type.Data);
		achievementTrigger9.PutData(1);
		AchievementManager.GetInstance().Trigger(achievementTrigger9);
	}

	public float GetHpParaWithPlayerCount(int playerCount)
	{
		float num = 1f;
		switch (playerCount)
		{
		case 1:
			return 1f;
		case 2:
			return 1.75f;
		case 3:
			return 2.5f;
		case 4:
			return 3.25f;
		default:
			return 1f;
		}
	}

	public override int DamageLevelSuppress(int originalDamage, int attackerLevel, int attackerWeaponLevel, int selfLevel)
	{
		int num = originalDamage;
		int num2 = attackerLevel - selfLevel;
		int num3 = attackerWeaponLevel - selfLevel;
		float num4 = (float)(num2 * num2) * 0.03f;
		float num5 = (float)(num3 * num3) * 0.07f;
		if (num2 < 0)
		{
			num4 *= -1f;
		}
		if (num3 < 0)
		{
			num5 *= -1f;
		}
		float value = num4 + num5;
		value = Mathf.Clamp(value, -0.9f, 2f);
		num = (int)((float)num * (1f + value));
		if (num < 1)
		{
			num = 1;
		}
		return num;
	}
}
