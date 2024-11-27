using System.Collections.Generic;
using UnityEngine;

public class FloatCore : EnemyBoss
{
	public enum FloatPhase
	{
		Phase_None = 0,
		Phase_1 = 1,
		Phase_1_TO_2 = 2,
		Phase_2 = 3,
		Phase_2_TO_3 = 4,
		Phase_3 = 5
	}

	public enum FloatMindState
	{
		State_0 = 0,
		State_1 = 1,
		State_2 = 2,
		State_3 = 3,
		State_4 = 4,
		State_5 = 5,
		State_6 = 6,
		State_7 = 7,
		State_8 = 8,
		State_9 = 9
	}

	private const string METEOR_PATH = "/Float_Point/Path";

	private const string MASS_LASER_POINT = "/Float_Point/MessLaser";

	public static EnemyState FLOAT_PATROL_STATE = new FloatPatrolState();

	public static EnemyState FLOAT_GO_BACK_STATE = new FloatGoBackState();

	public static EnemyState FLOAT_LASER_STATE = new FloatLaserState();

	public static EnemyState FLOAT_CHANGE_TO_1_STATE = new FloatChangeTo1State();

	public static EnemyState FLOAT_CHANGE_TO_2_STATE = new FloatChangeTo2State();

	public static EnemyState FLOAT_CHANGE_TO_3_STATE = new FloatChangeTo3State();

	public static EnemyState FLOAT_FLY_BACK_STATE = new FloatFlyBackState();

	public static EnemyState FLOAT_CAUTION_STATE = new FloatCautionState();

	public static EnemyState FLOAT_CAUTION_END_STATE = new FloatCautionEndState();

	public static EnemyState FLOAT_METEOR_STATE = new FloatMeteorState();

	public static EnemyState FLOAT_MASS_LASER_STATE = new FloatMassLaserState();

	public static EnemyState FLOAT_CATCHING_STATE = new FloatCatchingState();

	private FloatPhase m_floatPhase;

	protected byte m_currentMissileCount;

	private Transform m_laserMuzzleTransform;

	private float m_blueLaserStartTime;

	private float m_irradiateLaserStartTime;

	private float m_irradiateLaserLaunchStartTime;

	private Timer m_irradiateLaserHitTimer = new Timer();

	private GameObject m_irradiateLaser;

	private List<Vector3> m_targetList = new List<Vector3>();

	private FloatControler m_floatControler;

	private List<FloatProtector> m_floatProtectors = new List<FloatProtector>();

	private Transform m_floatCtrlPoint;

	private List<Transform> m_floatPrtcPoint = new List<Transform>();

	private Timer m_changeTo1Time = new Timer();

	private Timer m_changeTo2Time = new Timer();

	private Timer m_changeTo3Time = new Timer();

	private Timer m_changeTo4Time = new Timer();

	private Timer m_changeWaitTime = new Timer();

	private bool m_changeToFinish;

	private bool m_startSeeBoss;

	private float[] m_hpPctForRage = new float[9] { 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f };

	private FloatMindState m_currMindPhase;

	private bool m_changeMindPhase;

	private Vector3 mMoveTarget;

	private float m_moveTime;

	private List<List<Transform>> m_meteorPath = new List<List<Transform>>();

	private List<Transform> m_massLaserPoint = new List<Transform>();

	private int m_meteorPointIndex;

	private int m_meteorGroupIndex;

	private Timer m_meteorStart = new Timer();

	private Timer m_meteorGotHit = new Timer();

	private GameObject m_meteor;

	private GameObject m_safeLaser;

	private GameObject m_meteorEffect;

	private bool mDeadEffect;

	private bool m_cautionAudio;

	private float m_cautionTime;

	public override void Init()
	{
		base.Init();
		InitMeteorPath();
		InitMassLaserPoint();
		mNavWalkableMask = 1 << NavMeshLayer.FLY_AREA;
		mShieldType = ShieldType.MECHANICAL;
		mNavAngularSpeed = 180f;
		base.SpawnPosition = GetSkyPoint(base.SpawnPosition);
		m_cautionAudio = false;
	}

	public override void Activate()
	{
		base.Activate();
		m_cautionAudio = false;
		m_cautionTime = 0f;
		m_currMindPhase = FloatMindState.State_0;
		m_floatPhase = FloatPhase.Phase_None;
		m_currentMissileCount = 0;
		m_targetList.Clear();
		m_changeMindPhase = false;
		m_changeToFinish = false;
		m_startSeeBoss = false;
		mDeadEffect = false;
		m_meteor = null;
		SetBlueLaserLaunchNow();
		SetIrradiateLaserNow();
		GameObject gameObject = new GameObject();
		gameObject.name = "CannonPoint";
		m_laserMuzzleTransform = gameObject.transform;
		m_laserMuzzleTransform.parent = mBodyTransform;
		m_laserMuzzleTransform.localPosition = new Vector3(0.57f, 0.02f, 0.2f);
		m_laserMuzzleTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		m_laserMuzzleTransform.localScale = Vector3.one;
		GameObject original = Resources.Load("RPG_effect/float/boss_float_Target_10_attack") as GameObject;
		m_irradiateLaser = Object.Instantiate(original) as GameObject;
		m_irradiateLaser.transform.parent = mBodyTransform;
		m_irradiateLaser.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_irradiateLaser.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		m_irradiateLaser.transform.localScale = Vector3.one;
		m_irradiateLaser.SetActive(false);
		m_irradiateLaserHitTimer.SetTimer(0.5f, false);
		m_floatCtrlPoint = entityTransform.Find(BoneName.FloatCtrlPoint);
		m_floatPrtcPoint.Clear();
		for (int i = 0; i < 4; i++)
		{
			Transform item = entityTransform.Find(BoneName.FloatPrtcPoint + (i + 1) + "_Point");
			m_floatPrtcPoint.Add(item);
		}
		m_meteorGotHit.SetTimer(0.3f, false);
		m_meteorStart.SetTimer(3f, false);
		GameObject original2 = Resources.Load("RPG_effect/float/boss_float_Target_green10") as GameObject;
		m_safeLaser = Object.Instantiate(original2) as GameObject;
		m_safeLaser.transform.parent = mBodyTransform;
		m_safeLaser.transform.localPosition = new Vector3(0f, 0f, 0f);
		m_safeLaser.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		m_safeLaser.transform.localScale = Vector3.one;
		m_safeLaser.SetActive(false);
		GameObject original3 = Resources.Load("RPG_effect/st_effect/boss_float_liuxingyu01") as GameObject;
		m_meteorEffect = Object.Instantiate(original3) as GameObject;
		m_meteorEffect.transform.parent = mBodyTransform;
		m_meteorEffect.transform.localPosition = new Vector3(0f, 0f, -2f);
		m_meteorEffect.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		m_meteorEffect.transform.localScale = Vector3.one;
		m_meteorEffect.SetActive(false);
		GameObject original4 = Resources.Load("RPG_effect/st_effect/boss_float_energy") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original4) as GameObject;
		gameObject2.transform.parent = mBodyTransform;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		gameObject2.transform.localScale = Vector3.one;
		Debug.Log("mRushAttackProbability: " + mRushAttackProbability);
	}

	protected override void LoadConfig()
	{
		base.LoadConfig();
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find(BoneName.FloatBody);
		mHeadTransform = entityTransform.Find(BoneName.FloatHead);
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	protected override void InitRenders()
	{
	}

	public override bool CanBeHit()
	{
		if (GetPhase() == FloatPhase.Phase_3)
		{
			return true;
		}
		return false;
	}

	public override bool AimAssist()
	{
		if (GetPhase() == FloatPhase.Phase_3)
		{
			return true;
		}
		return false;
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.FloatMesh;
	}

	public override void LinkEnemy(List<Enemy> enemyList)
	{
		foreach (Enemy enemy in enemyList)
		{
			if (enemy.EnemyType == EnemyType.FLOAT_CONTROLER)
			{
				m_floatControler = enemy as FloatControler;
			}
			else if (enemy.EnemyType == EnemyType.FLOAT_PROTECTOR)
			{
				FloatProtector item = (FloatProtector)enemy;
				if (!m_floatProtectors.Contains(item))
				{
					m_floatProtectors.Add(item);
				}
			}
		}
	}

	private void InitMeteorPath()
	{
		m_meteorPath.Clear();
		GameObject gameObject = GameObject.Find("/Float_Point/Path");
		if (!(gameObject != null))
		{
			return;
		}
		foreach (Transform item2 in gameObject.transform)
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform item3 in item2)
			{
				list.Add(item3);
			}
			m_meteorPath.Add(list);
		}
	}

	private void InitMassLaserPoint()
	{
		m_massLaserPoint.Clear();
		GameObject gameObject = GameObject.Find("/Float_Point/MessLaser");
		if (!(gameObject != null))
		{
			return;
		}
		foreach (Transform item in gameObject.transform)
		{
			m_massLaserPoint.Add(item);
		}
	}

	private List<Transform> CloneMassLaserPoint()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in m_massLaserPoint)
		{
			list.Add(item);
		}
		return list;
	}

	private string GetIdleAnimString()
	{
		string result = AnimationString.FLOAT_IDLE_P1;
		switch (m_floatPhase)
		{
		case FloatPhase.Phase_1:
			result = AnimationString.FLOAT_IDLE_P1;
			break;
		case FloatPhase.Phase_2:
			result = AnimationString.FLOAT_IDLE_P2;
			break;
		case FloatPhase.Phase_3:
			result = AnimationString.FLOAT_IDLE_P3;
			break;
		}
		return result;
	}

	public Transform GetFloatCtrlPoint()
	{
		return m_floatCtrlPoint;
	}

	public Transform GetFloatPrtcPoint(int i)
	{
		if (i < m_floatPrtcPoint.Count)
		{
			return m_floatPrtcPoint[i];
		}
		return null;
	}

	public FloatPhase GetPhase()
	{
		return m_floatPhase;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if (!mIsActive)
		{
			return;
		}
		if (base.IsMasterPlayer)
		{
			if (m_floatPhase == FloatPhase.Phase_None || m_floatPhase == FloatPhase.Phase_1_TO_2 || m_floatPhase == FloatPhase.Phase_2_TO_3)
			{
				return;
			}
			if (m_floatPhase == FloatPhase.Phase_1)
			{
				bool flag = true;
				foreach (FloatProtector floatProtector in m_floatProtectors)
				{
					if (!floatProtector.m_readyChange)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					EndCurrentState();
					StartFloatChangeTo2();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_CHANGE_TO_2, entityTransform.position);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
					return;
				}
				if (GetBlueLaserLaunchDuration() >= mRangedInterval1)
				{
					SetBlueLaserLaunchNow();
					ShotLaser();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						foreach (Vector3 target in m_targetList)
						{
							EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_SHOT_LASER, entityTransform.position, target);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						}
					}
				}
			}
			else if (m_floatPhase == FloatPhase.Phase_2)
			{
				if (m_floatControler != null && m_floatControler.m_readyChange)
				{
					EndCurrentState();
					StartFloatChangeTo3();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request3 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_CHANGE_TO_3, entityTransform.position);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
					}
					return;
				}
				if (GetBlueLaserLaunchDuration() >= mRangedInterval1)
				{
					SetBlueLaserLaunchNow();
					ShotRandomLaser(2);
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						foreach (Vector3 target2 in m_targetList)
						{
							EnemyStateRequest request4 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_SHOT_LASER, entityTransform.position, target2);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request4);
						}
					}
				}
			}
			else if (m_floatPhase == FloatPhase.Phase_3)
			{
				if (GetBlueLaserLaunchDuration() >= mRangedInterval1)
				{
					SetBlueLaserLaunchNow();
					ShotRandomLaser(3);
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						foreach (Vector3 target3 in m_targetList)
						{
							EnemyStateRequest request5 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_SHOT_LASER, entityTransform.position, target3);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request5);
						}
					}
				}
				if (m_changeMindPhase)
				{
					m_changeMindPhase = false;
					if (m_currMindPhase <= FloatMindState.State_5 && !IsInMeteorState())
					{
						EndCurrentState();
						StartFloatFlyBack();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							EnemyStateRequest request6 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_FLY_BACK, entityTransform.position);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request6);
						}
					}
					else if (m_currMindPhase > FloatMindState.State_5 && !IsInMassLaserState() && !IsInMeteorState())
					{
						EndCurrentState();
						StartFloatCaution();
						if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
						{
							EnemyStateRequest request7 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_CAUTION, entityTransform.position);
							GameApp.GetInstance().GetNetworkManager().SendRequest(request7);
						}
					}
				}
			}
		}
		if (m_cautionAudio)
		{
			if (Time.time - m_cautionTime <= 1f)
			{
				PlaySoundSingle("RPG_Audio/Enemy/Float/float_alert01-1");
			}
			else if (Time.time - m_cautionTime <= 2f)
			{
				StopSound("RPG_Audio/Enemy/Float/float_alert01-1");
				PlaySoundSingle("RPG_Audio/Enemy/Float/float_alert01-2");
			}
			else if (Time.time - m_cautionTime <= 3f)
			{
				StopSound("RPG_Audio/Enemy/Float/float_alert01-2");
				PlaySoundSingle("RPG_Audio/Enemy/Float/float_alert01-3");
			}
			else
			{
				m_cautionAudio = false;
			}
		}
	}

	private bool IsInMeteorState()
	{
		if (GetState() == FLOAT_METEOR_STATE || GetState() == FLOAT_FLY_BACK_STATE)
		{
			return true;
		}
		return false;
	}

	private bool IsInMassLaserState()
	{
		if (GetState() == FLOAT_CAUTION_STATE || GetState() == FLOAT_CAUTION_END_STATE || GetState() == FLOAT_MASS_LASER_STATE)
		{
			return true;
		}
		return false;
	}

	public override bool CanPatrol()
	{
		return true;
	}

	private void SetMoveTimeNow()
	{
		m_moveTime = Time.time;
	}

	private float GetMoveTimeDuration()
	{
		return Time.time - m_moveTime;
	}

	private void SetIrradiateLaserLaunchNow()
	{
		m_irradiateLaserLaunchStartTime = Time.time;
	}

	private float GetIrradiateLaserLaunchDuration()
	{
		return Time.time - m_irradiateLaserLaunchStartTime;
	}

	public void ResetIrradiateLaser()
	{
		m_irradiateLaserStartTime = Time.time - (float)mRushAttackProbability;
	}

	private void SetIrradiateLaserNow()
	{
		m_irradiateLaserStartTime = Time.time;
	}

	private float GetIrradiateLaserDuration()
	{
		return Time.time - m_irradiateLaserStartTime;
	}

	public void ResetBlueLaser()
	{
		m_blueLaserStartTime = Time.time - mRangedInterval1;
	}

	private void SetBlueLaserLaunchNow()
	{
		m_blueLaserStartTime = Time.time;
	}

	private float GetBlueLaserLaunchDuration()
	{
		return Time.time - m_blueLaserStartTime;
	}

	public override bool IsInvincible()
	{
		if (m_floatPhase != FloatPhase.Phase_3)
		{
			return true;
		}
		return false;
	}

	public override void OnHit(DamageProperty dp)
	{
		base.OnHit(dp);
		ChangeMindPhase();
	}

	public override void OnHitResponse(int killerID, int damage, int currentShield, int currentHp, bool criticalAttack, byte elementType, WeaponType weaponType, DamageProperty.AttackerType attackerType)
	{
		base.OnHitResponse(killerID, damage, currentShield, currentHp, criticalAttack, elementType, weaponType, attackerType);
		ChangeMindPhase();
	}

	private void ChangeMindPhase()
	{
		if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[8])
		{
			if (m_currMindPhase != FloatMindState.State_9)
			{
				m_currMindPhase = FloatMindState.State_9;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[7])
		{
			if (m_currMindPhase != FloatMindState.State_8)
			{
				m_currMindPhase = FloatMindState.State_8;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[6])
		{
			if (m_currMindPhase != FloatMindState.State_7)
			{
				m_currMindPhase = FloatMindState.State_7;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[5])
		{
			if (m_currMindPhase != FloatMindState.State_6)
			{
				m_currMindPhase = FloatMindState.State_6;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[4])
		{
			if (m_currMindPhase != FloatMindState.State_5)
			{
				m_currMindPhase = FloatMindState.State_5;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[3])
		{
			if (m_currMindPhase != FloatMindState.State_4)
			{
				m_currMindPhase = FloatMindState.State_4;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[2])
		{
			if (m_currMindPhase != FloatMindState.State_3)
			{
				m_currMindPhase = FloatMindState.State_3;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[1])
		{
			if (m_currMindPhase != FloatMindState.State_2)
			{
				m_currMindPhase = FloatMindState.State_2;
				m_changeMindPhase = true;
			}
		}
		else if ((float)Hp <= (float)base.MaxHp * m_hpPctForRage[0] && m_currMindPhase != FloatMindState.State_1)
		{
			m_currMindPhase = FloatMindState.State_1;
			m_changeMindPhase = true;
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == FLOAT_LASER_STATE)
		{
			EndFloatLaser();
		}
		else if (mState == FLOAT_CATCHING_STATE)
		{
			EndFloatCatching();
		}
		else if (mState == FLOAT_PATROL_STATE)
		{
			EndFloatPatrol();
		}
		else if (mState == FLOAT_GO_BACK_STATE)
		{
			EndFloatGoBack();
		}
		else if (mState == FLOAT_CHANGE_TO_1_STATE)
		{
			EndFloatChangeTo1();
		}
		else if (mState == FLOAT_CHANGE_TO_2_STATE)
		{
			EndFloatChangeTo2();
		}
		else if (mState == FLOAT_CHANGE_TO_3_STATE)
		{
			EndFloatChangeTo3();
		}
		else if (mState == FLOAT_FLY_BACK_STATE)
		{
			EndFloatFlyBack();
		}
		else if (mState == FLOAT_CAUTION_STATE)
		{
			EndFloatCaution();
		}
		else if (mState == FLOAT_CAUTION_END_STATE)
		{
			EndFloatCautionEnd();
		}
		else if (mState == FLOAT_METEOR_STATE)
		{
			EndFloatMeteor();
		}
		else if (mState == FLOAT_MASS_LASER_STATE)
		{
			EndFloatMassLaser();
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
			mNavMeshAgent.radius = cc.radius;
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

	public override void UpdateNavMeshInCatching(bool force)
	{
		if (!force && !(Time.time - mLastUpdateNavMeshTime > 9f / mRunSpeed))
		{
			return;
		}
		mLastUpdateNavMeshTime = Time.time;
		Vector3 origin = mTargetPosition;
		mRay = new Ray(origin, Vector3.up);
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			origin = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
			if (null != mNavMeshAgent)
			{
				mNavMeshAgent.enabled = true;
				mNavMeshAgent.Resume();
				mNavMeshAgent.SetDestination(origin);
				mNavMeshAgent.speed = mRunSpeed * 3f;
				SetCanTurn(false);
			}
		}
	}

	protected override void PlayOnHitBloodEffect(Vector3 position, ElementType elementType)
	{
		if (!gameScene.PlayOnHitElementEffect(position, elementType))
		{
			gameScene.GetEffectPool(EffectPoolType.BULLET_WALL_SPARK).CreateObject(position, Vector3.zero, Quaternion.identity);
		}
	}

	public void StartFloatMove(Vector3 targetPosition, float speed)
	{
		SetMoveTimeNow();
		mMoveTarget = targetPosition;
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mMoveTarget);
			mNavMeshAgent.speed = speed;
			mNavMeshAgent.updateRotation = false;
			SetCanTurn(false);
		}
	}

	public void StartFinalBattle()
	{
		PlaySound("RPG_Audio/Environment/start_fighting");
		Debug.Log("Start Final Battle!!!");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/TMX/TMX_dead");
	}

	public override void StartDead()
	{
		base.StartDead();
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.StartRealDead();
		}
		m_floatControler.StartRealDead();
		mDeadEffect = false;
		m_irradiateLaser.SetActive(false);
		m_safeLaser.SetActive(false);
		m_meteorEffect.SetActive(false);
		if (m_meteor != null)
		{
			Object.Destroy(m_meteor);
		}
		GameObject original = Resources.Load("RPG_effect/RPG_Cybershoot_Die_001") as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = entityTransform.Find(BoneName.TerminatorRightUpperArm);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		GameObject gameObject2 = Object.Instantiate(original) as GameObject;
		gameObject2.transform.parent = mBodyTransform;
		gameObject2.transform.localPosition = new Vector3(0f, -1.4f, 0.3f);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		GameObject gameObject3 = Object.Instantiate(original) as GameObject;
		gameObject3.transform.parent = mBodyTransform;
		gameObject3.transform.localPosition = new Vector3(-1.6f, 0.8f, 1.2f);
		gameObject3.transform.localRotation = Quaternion.identity;
		gameObject3.transform.localScale = Vector3.one;
		Debug.Log("Now_Im_the_boss");
		mDeadEffect = false;
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Now_Im_the_boss);
		AchievementManager.GetInstance().Trigger(trigger);
	}

	public override void DoDead()
	{
		PlayAnimation(AnimationString.FLOAT_DEAD, WrapMode.ClampForever);
		float num = animation[AnimationString.FLOAT_DEAD].time / animation[AnimationString.FLOAT_DEAD].clip.length;
		if (!mDeadEffect && num > 0.9f)
		{
			mDeadEffect = true;
			GameObject original = Resources.Load("RPG_effect/RPG_Cypher_Die_001") as GameObject;
			Object.Instantiate(original, mBodyTransform.position, Quaternion.identity);
			PlaySound("Audio/rpg/rpg-21_boom");
		}
		else if (num >= 1f)
		{
			GameApp.GetInstance().GetLootManager().OnLoot(this);
			EndDead();
		}
	}

	public override void EndDead()
	{
		Deactivate();
	}

	public void StartFloatPatrol(Vector3 patrolTarget)
	{
		SetState(FLOAT_PATROL_STATE);
		SetPatrolTimeNow();
		SetPatrolTarget(patrolTarget);
		SetNavMeshForPatrol();
		SetUpdatePosTimeNow();
	}

	public void DoFloatPatrol()
	{
		Debug.DrawLine(entityTransform.position, mPatrolTarget, Color.blue);
		PlayAnimation(GetIdleAnimString(), WrapMode.Loop);
		PlayWalkSound();
		if (EndFloatPatrolCondition())
		{
			EndFloatPatrol();
			StartEnemyIdle();
		}
		else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_PATROL, GetTransform().position, mPatrolTarget);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected bool EndFloatPatrolCondition()
	{
		return GetPatrolTimeDuration() >= 10f || (mPatrolTarget - entityTransform.position).sqrMagnitude < 14f;
	}

	public override void SetNavMeshForPatrol()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.enabled = true;
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mPatrolTarget);
			mNavMeshAgent.speed = mPatrolSpeed;
			SetCanTurn(false);
		}
	}

	protected override void PlayWalkSound()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Float/float_fly");
	}

	public void EndFloatPatrol()
	{
		StopNavMesh();
		StopSound("RPG_Audio/Enemy/Float/float_fly");
	}

	public override void StartEnemyIdle()
	{
		base.StartEnemyIdle();
	}

	protected override void StartEnemyIdleWithoutResetTime()
	{
		SetState(Enemy.IDLE_STATE);
	}

	public override void DoEnemyIdle()
	{
		PlayAnimation(GetIdleAnimString(), WrapMode.Loop);
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
		if ((GetPhase() == FloatPhase.Phase_2 || GetPhase() == FloatPhase.Phase_3) && GetIrradiateLaserDuration() >= (float)mRushAttackProbability)
		{
			SetIrradiateLaserNow();
			StartFloatCatching();
			return;
		}
		PatrolPointScript component = mNextPatrolLinePoint.GetComponent<PatrolPointScript>();
		if (null != component)
		{
			mNextPatrolLinePoint = component.NextPoint;
			StartFloatPatrol(GetSkyPoint(mNextPatrolLinePoint.transform.position));
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_PATROL, entityTransform.position, mPatrolTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	private Vector3 GetSkyPoint(Vector3 groundPoint)
	{
		Vector3 result = groundPoint;
		int layerMask = 1 << PhysicsLayer.FLY_AREA;
		for (int i = 0; i < 5; i++)
		{
			Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			vector.Normalize();
			Vector3 origin = groundPoint + vector * i;
			mRay = new Ray(origin, Vector3.up);
			if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
			{
				result = mRaycastHit.point + new Vector3(0f, 0.2f, 0f);
				break;
			}
		}
		return result;
	}

	protected override void EndEnemyIdle()
	{
	}

	public override void StartSeeBoss()
	{
		mIsActive = true;
		EndBossInit();
		StartFloatChangeTo1();
		MyCreateNavMeshAgent();
	}

	public override void StartBossBattle()
	{
	}

	protected override void StartBossInit()
	{
		SetState(EnemyBoss.INIT_STATE);
		mIsActive = false;
		mCanGotHit = false;
	}

	public override void DoBossInit()
	{
		PlayAnimation(AnimationString.FLOAT_IDLE_P1, WrapMode.Loop);
	}

	public void StartFloatLaser()
	{
		SetState(FLOAT_LASER_STATE);
		GameObject prefab = Resources.Load("RPG_effect/float/boss_float_Target_10") as GameObject;
		Vector3 vector = new Vector3(m_irradiateLaser.transform.position.x, mTargetPosition.y, m_irradiateLaser.transform.position.z);
		CreateCaution(prefab, vector + Vector3.up * -0.02f);
		SetIrradiateLaserLaunchNow();
		mIsShoot = false;
	}

	public void DoFloatLaser()
	{
		if (!mIsShoot && GetIrradiateLaserLaunchDuration() >= 3f)
		{
			m_irradiateLaser.SetActive(true);
			FloatIrradiateLaserScript component = m_irradiateLaser.GetComponent<FloatIrradiateLaserScript>();
			if (null != component)
			{
				component.mEnemy = this;
				component.mDamage = mRushAttackDamage1;
			}
			mIsShoot = true;
			m_irradiateLaserHitTimer.Do();
		}
		if (mIsShoot && m_irradiateLaserHitTimer.Ready())
		{
			EndFloatLaser();
			StartFloatGoBack(GetFloatGoBackPosition());
		}
	}

	private void EndFloatLaser()
	{
		m_irradiateLaser.SetActive(false);
	}

	private void CreateMassLaserCautionForPlayers()
	{
		m_cautionAudio = true;
		m_cautionTime = Time.time;
		m_targetList.Clear();
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		GameObject prefab = Resources.Load("RPG_effect/float/boss_float_Target_5") as GameObject;
		int i = 0;
		List<Transform> list = CloneMassLaserPoint();
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				CreateCaution(prefab, item.GetPosition() + Vector3.up * -0.02f);
				m_targetList.Add(item.GetPosition());
			}
		}
		for (; i < 10; i++)
		{
			int index = Random.Range(0, list.Count);
			CreateCaution(prefab, list[index].position + Vector3.up * -0.02f);
			m_targetList.Add(list[index].position);
			list.RemoveAt(index);
		}
	}

	private void CreateLaserCautionForPlayers()
	{
		m_cautionAudio = true;
		m_cautionTime = Time.time;
		m_targetList.Clear();
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		GameObject prefab = Resources.Load("RPG_effect/float/boss_float_Target_5") as GameObject;
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				CreateCaution(prefab, item.GetTransform().position + Vector3.up * -0.02f);
				m_targetList.Add(item.GetTransform().position);
			}
		}
	}

	private void CreateRandomLaserCautionForPlayers(int num)
	{
		m_cautionAudio = true;
		m_cautionTime = Time.time;
		m_targetList.Clear();
		List<Player> potentialPlayerList = GetPotentialPlayerList();
		GameObject prefab = Resources.Load("RPG_effect/float/boss_float_Target_5") as GameObject;
		foreach (Player item in potentialPlayerList)
		{
			if (item != null)
			{
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
					vector.Normalize();
					float num2 = Random.Range(10f, 15f);
					Vector3 vector2 = item.GetTransform().position + vector * num2;
					CreateCaution(prefab, vector2 + Vector3.up * -0.02f);
					m_targetList.Add(vector2);
				}
			}
		}
	}

	private void CreateCaution(GameObject prefab, Vector3 pos)
	{
		Object.Instantiate(prefab, pos, Quaternion.identity);
	}

	public void StartFloatChangeTo2()
	{
		Debug.Log("StartFloatChangeTo2......");
		StopNavMesh(true);
		m_floatPhase = FloatPhase.Phase_1_TO_2;
		SetState(FLOAT_CHANGE_TO_2_STATE);
		m_changeTo2Time.SetTimer(5f, false);
		m_changeWaitTime.SetTimer(1f, false);
		m_changeToFinish = false;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.StartFPChangeTo2();
		}
		PlayChangeToSound();
	}

	public void DoFloatChangeTo2()
	{
		if (!m_changeToFinish)
		{
			PlayAnimation(AnimationString.FLOAT_CHANGE_P1, WrapMode.Loop);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.FLOAT_IDLE_P2, WrapMode.Loop);
		}
		bool flag = true;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			if (!floatProtector.m_finishChange)
			{
				flag = false;
				break;
			}
		}
		if (m_changeTo2Time.Ready() && flag && !m_changeToFinish)
		{
			m_changeToFinish = true;
			m_changeWaitTime.Do();
		}
		if (!m_changeToFinish || !m_changeWaitTime.Ready() || !base.IsMasterPlayer)
		{
			return;
		}
		EndFloatChangeTo2();
		StartEnemyIdle();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.IDLE, entityTransform.position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		foreach (FloatProtector floatProtector2 in m_floatProtectors)
		{
			floatProtector2.StartFPInactive();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(floatProtector2.PointID, floatProtector2.EnemyID, EnemyStateConst.FP_INACTIVE, floatProtector2.GetPosition());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void EndFloatChangeTo2()
	{
		m_floatPhase = FloatPhase.Phase_2;
		Debug.Log("EndFloatChangeTo2 : " + m_floatPhase);
		PlayAnimationWithoutBlend(GetIdleAnimString(), WrapMode.Loop);
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.EndFPChangeTo();
		}
		StopSound("RPG_Audio/Enemy/Float/float_change_form");
	}

	public void StartFloatChangeTo3()
	{
		Debug.Log("StartFloatChangeTo3......");
		StopNavMesh(true);
		m_floatPhase = FloatPhase.Phase_2_TO_3;
		SetState(FLOAT_CHANGE_TO_3_STATE);
		m_changeTo3Time.SetTimer(5f, false);
		m_changeWaitTime.SetTimer(1f, false);
		m_changeToFinish = false;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.StartFPChange2To3();
		}
		PlayChangeToSound();
	}

	public void DoFloatChangeTo3()
	{
		if (!m_changeToFinish)
		{
			PlayAnimation(AnimationString.FLOAT_CHANGE_P2, WrapMode.Loop);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.FLOAT_IDLE_P3, WrapMode.Loop);
		}
		bool flag = true;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			if (!floatProtector.m_finishChange)
			{
				flag = false;
				break;
			}
		}
		if (m_changeTo3Time.Ready() && flag && !m_changeToFinish)
		{
			m_changeToFinish = true;
			m_changeWaitTime.Do();
		}
		if (!m_changeToFinish || !m_changeWaitTime.Ready() || !base.IsMasterPlayer)
		{
			return;
		}
		EndFloatChangeTo3();
		StartEnemyIdle();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.IDLE, entityTransform.position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		foreach (FloatProtector floatProtector2 in m_floatProtectors)
		{
			floatProtector2.StartFPInactive();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(floatProtector2.PointID, floatProtector2.EnemyID, EnemyStateConst.FP_INACTIVE, floatProtector2.GetPosition());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void EndFloatChangeTo3()
	{
		m_floatPhase = FloatPhase.Phase_3;
		Debug.Log("EndFloatChangeTo3 : " + m_floatPhase);
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.EndFPChangeTo();
		}
		StopSound("RPG_Audio/Enemy/Float/float_change_form");
	}

	public void StartShotLaser(Vector3 pos)
	{
		GameObject prefab = Resources.Load("RPG_effect/float/boss_float_Target_5") as GameObject;
		CreateCaution(prefab, pos + Vector3.up * -0.02f);
		GameObject original = Resources.Load("RPG_effect/float/boss_float_Target_5_attack") as GameObject;
		GameObject gameObject = Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.LookRotation(m_laserMuzzleTransform.forward)) as GameObject;
		FloatLaserScript component = gameObject.GetComponent<FloatLaserScript>();
		if (null != component)
		{
			Vector3 normalized = (m_laserMuzzleTransform.forward + 5f * Vector3.up).normalized;
			component.mEnemy = this;
			component.mRisingSpeed = 100f * normalized;
			component.mRisingTime = 3f;
			component.mTargetPosition = pos;
			component.mExplosionDamage = mRangedAttackDamage1;
			component.mDownSpeedValue = 300f;
			component.mExplosionRadius = mRangedExplosionRadius1;
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original2, m_laserMuzzleTransform.position, Quaternion.identity);
	}

	private void ShotRandomLaser(int num)
	{
		CreateRandomLaserCautionForPlayers(num);
		Laser();
	}

	public void ShotLaser()
	{
		CreateLaserCautionForPlayers();
		Laser();
	}

	private void Laser()
	{
		GameObject original = Resources.Load("RPG_effect/float/boss_float_Target_5_attack") as GameObject;
		foreach (Vector3 target in m_targetList)
		{
			GameObject gameObject = Object.Instantiate(original, m_laserMuzzleTransform.position, Quaternion.LookRotation(m_laserMuzzleTransform.forward)) as GameObject;
			FloatLaserScript component = gameObject.GetComponent<FloatLaserScript>();
			if (null != component)
			{
				Vector3 normalized = (m_laserMuzzleTransform.forward + 5f * Vector3.up).normalized;
				component.mEnemy = this;
				component.mRisingSpeed = 100f * normalized;
				component.mRisingTime = 3f;
				component.mTargetPosition = target;
				component.mExplosionDamage = mRangedAttackDamage1;
				component.mDownSpeedValue = 300f;
				component.mExplosionRadius = mRangedExplosionRadius1;
			}
		}
		GameObject original2 = Resources.Load("RPG_effect/RPG_TMXL_fire03_002") as GameObject;
		Object.Instantiate(original2, m_laserMuzzleTransform.position, Quaternion.identity);
	}

	public void StartFloatCatching()
	{
		SetState(FLOAT_CATCHING_STATE);
		SetUpdatePosTimeNow();
		UpdateNavMeshInCatching(true);
	}

	public void DoFloatCatching()
	{
		PlayAnimation(GetIdleAnimString(), WrapMode.Loop);
		UpdateNavMeshInCatching(false);
		PlayWalkSound();
		if (!base.IsMasterPlayer)
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			if (GetUpdatePosTimeDuration() > mUpdatePosInterval)
			{
				SetUpdatePosTimeNow();
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_CATCHING, GetTransform().position, (short)(base.SpeedRate * 100f));
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else if (CheckFloatTargetInCatching())
			{
				FloatMakeDecisionInCatching();
			}
		}
		else if (CheckFloatTargetInCatching())
		{
			FloatMakeDecisionInCatching();
		}
	}

	protected bool CheckFloatTargetInCatching()
	{
		if (mTarget == null)
		{
			ChooseTargetPlayer(true);
		}
		if (mTarget == null)
		{
			EndFloatCatching();
			return false;
		}
		return true;
	}

	protected void FloatMakeDecisionInCatching()
	{
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		if (horizontalSqrDistanceFromTarget <= 25f && canHitTargetPlayer())
		{
			EndFloatCatching();
			StartFloatLaser();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_LASER, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	protected void EndFloatCatching()
	{
		StopNavMesh();
		StopSound("RPG_Audio/Enemy/Float/float_fly");
	}

	public void StartFloatGoBack(Vector3 target)
	{
		SetState(FLOAT_GO_BACK_STATE);
		SetPatrolTarget(target);
		SetNavMeshForGoBack();
		SetUpdatePosTimeNow();
		SetGoBackTimeNow();
	}

	public void DoFloatGoBack()
	{
		Debug.DrawLine(entityTransform.position, mPatrolTarget, Color.blue);
		PlayAnimation(GetIdleAnimString(), WrapMode.Loop);
		PlayWalkSound();
		if (EndFloatGoBackCondition())
		{
			EndFloatGoBack();
			StartEnemyIdle();
		}
		else if (base.IsMasterPlayer && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode() && GetUpdatePosTimeDuration() > mUpdatePosInterval)
		{
			SetUpdatePosTimeNow();
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_GO_BACK, GetTransform().position, GetFloatGoBackPosition());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void EndFloatGoBack()
	{
		StopNavMesh(true);
		StopSound("RPG_Audio/Enemy/Float/float_fly");
	}

	public void StopNavMesh(bool immediately)
	{
		if (null != mNavMeshAgent && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.Stop(immediately);
		}
	}

	private Vector3 GetFloatGoBackPosition()
	{
		return GetSkyPoint(mSpawnPointScript.gameObject.transform.position);
	}

	private bool EndFloatGoBackCondition()
	{
		return (mPatrolTarget - entityTransform.position).sqrMagnitude < 49f || GetGoBackTimeDuration() > mMaxGoBackTime;
	}

	public void StartFloatChangeTo1()
	{
		Debug.Log("StartFloatChangeTo1......");
		SetState(FLOAT_CHANGE_TO_1_STATE);
		m_changeTo1Time.SetTimer(6f, false);
		m_changeWaitTime.SetTimer(2f, false);
		m_startSeeBoss = false;
		m_changeToFinish = false;
		PlayChangeToSound();
	}

	public void DoFloatChangeTo1()
	{
		if (!m_startSeeBoss)
		{
			foreach (FloatProtector floatProtector in m_floatProtectors)
			{
				if (floatProtector == null || floatProtector.GetTransform() == null)
				{
					return;
				}
			}
			if (m_floatControler == null || m_floatControler.GetTransform() == null)
			{
				return;
			}
			m_startSeeBoss = true;
			foreach (FloatProtector floatProtector2 in m_floatProtectors)
			{
				floatProtector2.StartSeeBoss();
				floatProtector2.StartFPChangeTo1();
			}
			m_floatControler.StartSeeBoss();
		}
		if (!m_changeToFinish)
		{
			PlayAnimation(AnimationString.FLOAT_CHANGE_P1, WrapMode.Loop);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.FLOAT_IDLE_P1, WrapMode.Loop);
		}
		bool flag = true;
		foreach (FloatProtector floatProtector3 in m_floatProtectors)
		{
			if (!floatProtector3.m_finishChange)
			{
				flag = false;
				break;
			}
		}
		if (m_changeTo1Time.Ready() && flag && !m_changeToFinish)
		{
			m_changeToFinish = true;
			m_changeWaitTime.Do();
		}
		if (!m_changeToFinish || !m_changeWaitTime.Ready() || !base.IsMasterPlayer)
		{
			return;
		}
		EndFloatChangeTo1();
		StartEnemyIdle();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.IDLE, entityTransform.position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		foreach (FloatProtector floatProtector4 in m_floatProtectors)
		{
			floatProtector4.StartEnemyIdle();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(floatProtector4.PointID, floatProtector4.EnemyID, EnemyStateConst.IDLE, floatProtector4.GetPosition());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	public void EndFloatChangeTo1()
	{
		m_floatPhase = FloatPhase.Phase_1;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.EndFPChangeTo();
		}
		StopSound("RPG_Audio/Enemy/Float/float_change_form");
	}

	private void PlayChangeToSound()
	{
		PlaySoundSingle("RPG_Audio/Enemy/Float/float_change_form");
	}

	public void StartFloatFlyBack()
	{
		SetState(FLOAT_FLY_BACK_STATE);
		StartFloatMove(GetSkyPoint(mSpawnPointScript.gameObject.transform.position), mRunSpeed);
	}

	public void DoFloatFlyBack()
	{
		PlayWalkSound();
		if (((mMoveTarget - entityTransform.position).sqrMagnitude < 49f || GetMoveTimeDuration() > 15f) && base.IsMasterPlayer)
		{
			EndFloatFlyBack();
			StartFloatMeteor(Random.Range(0, m_meteorPath.Count));
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_METEOR, entityTransform.position, (byte)m_meteorGroupIndex);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	private void EndFloatFlyBack()
	{
		StopNavMesh();
		StopSound("RPG_Audio/Enemy/Float/float_fly");
	}

	public void StartFloatCaution()
	{
		StopNavMesh(true);
		SetState(FLOAT_CAUTION_STATE);
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.StartFPChange3To4();
		}
	}

	public void DoFloatCaution()
	{
		PlayAnimation(AnimationString.FLOAT_CAUTION, WrapMode.ClampForever);
		bool flag = true;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			if (!floatProtector.m_finishChange)
			{
				flag = false;
				break;
			}
		}
		if (!flag || !AnimationPlayed(AnimationString.FLOAT_CAUTION, 1f))
		{
			return;
		}
		EndFloatCaution();
		StartFloatMassLaser();
		foreach (FloatProtector floatProtector2 in m_floatProtectors)
		{
			floatProtector2.StartFPInactive();
		}
		if (base.IsMasterPlayer)
		{
			EndFloatCaution();
			StartFloatMassLaser();
		}
	}

	private void EndFloatCaution()
	{
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.EndFPChangeTo();
		}
	}

	public void StartFloatMassLaser()
	{
		SetState(FLOAT_MASS_LASER_STATE);
		m_changeTo4Time.SetTimer(2f, false);
		mIsShoot = false;
	}

	public void DoFloatMassLaser()
	{
		if (base.IsMasterPlayer && !mIsShoot)
		{
			mIsShoot = true;
			CreateMassLaserCautionForPlayers();
			Laser();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				foreach (Vector3 target in m_targetList)
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.FLOAT_SHOT_LASER, entityTransform.position, target);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		PlayAnimation(AnimationString.FLOAT_IDLE_P4, WrapMode.Loop);
		if (m_changeTo4Time.Ready())
		{
			EndFloatMassLaser();
			StartFloatCautionEnd();
		}
	}

	private void EndFloatMassLaser()
	{
	}

	public void StartFloatCautionEnd()
	{
		m_changeWaitTime.SetTimer(1f, false);
		m_changeToFinish = false;
		SetState(FLOAT_CAUTION_END_STATE);
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.StartFPChange4To3();
		}
	}

	public void DoFloatCautionEnd()
	{
		if (!m_changeToFinish)
		{
			PlayAnimation(AnimationString.FLOAT_CAUTION_END, WrapMode.ClampForever);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.FLOAT_IDLE_P3, WrapMode.Loop);
		}
		bool flag = true;
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			if (!floatProtector.m_finishChange)
			{
				flag = false;
				break;
			}
		}
		if (flag && AnimationPlayed(AnimationString.FLOAT_CAUTION_END, 1f))
		{
			m_changeToFinish = true;
			m_changeWaitTime.Do();
		}
		if (!m_changeToFinish || !m_changeWaitTime.Ready())
		{
			return;
		}
		EndFloatCautionEnd();
		StartEnemyIdle();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.IDLE, entityTransform.position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		foreach (FloatProtector floatProtector2 in m_floatProtectors)
		{
			floatProtector2.StartFPInactive();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(floatProtector2.PointID, floatProtector2.EnemyID, EnemyStateConst.FP_INACTIVE, floatProtector2.GetPosition());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	private void EndFloatCautionEnd()
	{
		foreach (FloatProtector floatProtector in m_floatProtectors)
		{
			floatProtector.EndFPChangeTo();
		}
	}

	public void StartFloatMeteor(int groupIndex)
	{
		SetState(FLOAT_METEOR_STATE);
		m_meteorGroupIndex = groupIndex;
		m_meteorPointIndex = 0;
		mMoveTarget = m_meteorPath[m_meteorGroupIndex][m_meteorPointIndex].position;
		SetMoveTimeNow();
		StartFloatMove(GetSkyPoint(mMoveTarget), mWalkSpeed * 0.3f);
		m_meteorStart.Do();
		m_meteorGotHit.Do();
		m_safeLaser.SetActive(true);
		m_meteorEffect.SetActive(true);
		mIsShoot = false;
		FloatSafeLaserScript component = m_safeLaser.GetComponent<FloatSafeLaserScript>();
		if (null != component)
		{
			component.mEnemy = this;
		}
	}

	public void DoFloatMeteor()
	{
		if (m_meteorStart.Ready())
		{
			PlaySoundSingle("RPG_Audio/Enemy/Float/float_thunder");
		}
		else
		{
			PlaySoundSingle("RPG_Audio/Enemy/Float/float_alert02");
		}
		if (m_meteorStart.Ready() && !mIsShoot)
		{
			mIsShoot = true;
			if (m_meteor == null)
			{
				GameObject original = Resources.Load("RPG_effect/st_effect/boss_float_liuxingyu02") as GameObject;
				m_meteor = Object.Instantiate(original) as GameObject;
				m_meteor.transform.position = GetSkyPoint(mSpawnPointScript.gameObject.transform.position);
				m_meteor.transform.localScale = Vector3.one;
				FloatMeteorScript component = m_meteor.GetComponent<FloatMeteorScript>();
				if (null != component)
				{
					component.mEnemy = this;
					component.mDamage = mRangedAttackDamage2;
				}
			}
			m_meteor.SetActive(true);
		}
		if ((mMoveTarget - entityTransform.position).sqrMagnitude < 36f || GetMoveTimeDuration() > 15f)
		{
			m_meteorPointIndex++;
			if (m_meteorPointIndex >= m_meteorPath[m_meteorGroupIndex].Count)
			{
				EndFloatMeteor();
				StartEnemyIdle();
			}
			else
			{
				SetMoveTimeNow();
				mMoveTarget = m_meteorPath[m_meteorGroupIndex][m_meteorPointIndex].position;
				StartFloatMove(GetSkyPoint(mMoveTarget), mWalkSpeed * 0.3f);
			}
		}
	}

	private void EndFloatMeteor()
	{
		Debug.Log("EndFloatMeteor: ");
		if (m_safeLaser != null)
		{
			m_safeLaser.SetActive(false);
		}
		if (m_meteor != null)
		{
			m_meteor.SetActive(false);
		}
		if (m_meteorEffect != null)
		{
			m_meteorEffect.SetActive(false);
		}
		StopSound("RPG_Audio/Enemy/Float/float_thunder");
	}
}
