using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBoss : Enemy
{
	public enum MindState
	{
		NORMAL = 0,
		RAGE = 1
	}

	public static EnemyState INIT_STATE = new BossInitState();

	protected bool mEnableGravity = true;

	protected GameObject mLeftArmTrail;

	protected GameObject mRightArmTrail;

	protected Collider mBodyCollider;

	protected Collider mFlyBodyCollider;

	protected GameObject[] mTrails;

	protected GameObject mBipObject;

	protected bool mCanShot = true;

	protected Vector3 mAreaCenter;

	protected float mAreaRadius;

	protected float mMaxOverRushDistance;

	protected new Timer mWalkAudioTimer = new Timer();

	protected Timer mTouchtimer = new Timer();

	protected Timer mFiretimer = new Timer();

	protected Timer mCriticalTimer = new Timer();

	protected string mWalkAudioName = string.Empty;

	protected MindState mMindState;

	protected float[] mMaxCatchingTime = new float[2];

	protected float mLastCatchingTime;

	protected float mHpPercetageForRage = 0.4f;

	protected int mCurrentHitTime = 1;

	protected float mAttackDetectionAngle;

	protected float mUpSpeed;

	protected float mDownSpeed;

	protected float mGroupAttackDistance;

	protected float mTouchKnockSpeed;

	protected int mAttackCount;

	public bool CanShot
	{
		get
		{
			return mCanShot;
		}
		set
		{
			mCanShot = value;
		}
	}

	public float MaxCatchingTime
	{
		get
		{
			return mMaxCatchingTime[(int)mMindState];
		}
	}

	public float GroupAttackDistance
	{
		get
		{
			return mGroupAttackDistance;
		}
	}

	public void SetCatchingTimeNow()
	{
		mLastCatchingTime = Time.time;
	}

	public float GetCatchingTimeDuration()
	{
		return Time.time - mLastCatchingTime;
	}

	public void IncreaseAttackCount()
	{
		mAttackCount++;
	}

	public int GetAttackCount()
	{
		return mAttackCount;
	}

	public void ResetAttackCount()
	{
		mAttackCount = 0;
	}

	protected bool GetHitPoint(out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Ray ray = new Ray(transform.position, vector - transform.position);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point;
			return true;
		}
		ray = new Ray(mLocalPlayer.GetTransform().position + Vector3.up * 0.5f, mBipObject.transform.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f) - mLocalPlayer.GetTransform().position - Vector3.up * 0.5f);
		hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point;
			return true;
		}
		return false;
	}

	protected bool GetHitResponsePoint(out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		if (!mLocalPlayer.InPlayingState())
		{
			return false;
		}
		bool flag = true;
		if (GameApp.GetInstance().GetGameWorld().GetPlayingPlayerCount() > 1)
		{
			if (mLocalPlayer.State != Player.ATTACK_STATE)
			{
				flag = false;
			}
			Weapon weapon = mLocalPlayer.GetWeapon();
			if (weapon != null && weapon.GetWeaponType() == WeaponType.LaserGun)
			{
				LaserCannon laserCannon = weapon as LaserCannon;
				if (laserCannon != null && laserCannon.IsOverHeat)
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			return GetHitPoint(out hitPoint);
		}
		RemotePlayer remotePlayer = null;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null && item.InPlayingState() && item.State == Player.ATTACK_STATE)
			{
				remotePlayer = item;
				break;
			}
		}
		if (remotePlayer != null)
		{
			Ray ray = new Ray(remotePlayer.GetTransform().position + Vector3.up * 0.5f, mBipObject.transform.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f) - remotePlayer.GetTransform().position - Vector3.up * 0.5f);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
			{
				hitPoint = hitInfo.point;
				return true;
			}
		}
		return false;
	}

	public override void Init()
	{
		base.Init();
		mMindState = MindState.NORMAL;
	}

	public virtual void InitBossLevelTime()
	{
		mLastCatchingTime = Time.time;
	}

	public bool NeedRage()
	{
		if (mMindState == MindState.RAGE)
		{
			return false;
		}
		if ((float)(base.MaxHp - Hp) > (float)base.MaxHp * mHpPercetageForRage)
		{
			mMindState = MindState.RAGE;
			return true;
		}
		return false;
	}

	public void EnableLeftArmTrail(bool bEnable)
	{
		if (bEnable)
		{
			mLeftArmTrail.GetComponent<TrailRenderer>().enabled = true;
		}
		else
		{
			mLeftArmTrail.GetComponent<TrailRenderer>().enabled = false;
		}
	}

	public void EnableRightArmTrail(bool bEnable)
	{
		if (bEnable)
		{
			mRightArmTrail.GetComponent<TrailRenderer>().enabled = true;
		}
		else
		{
			mRightArmTrail.GetComponent<TrailRenderer>().enabled = false;
		}
	}

	protected virtual void loadParameters()
	{
	}

	public override void Activate()
	{
		base.Activate();
		InitBossLevelTime();
		EnableGravity(true);
		mDeltaPosition = Vector3.zero;
		if (SpawnType != 0)
		{
			StartEnemyIdle();
		}
		else
		{
			StartBossInit();
		}
	}

	protected override void ChooseTargetPlayer(bool force)
	{
		GameUnit randomTarget = GetRandomTarget();
		if (randomTarget != null)
		{
			ChangeTarget(randomTarget);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyChangeTargetRequest request = new EnemyChangeTargetRequest(base.PointID, base.EnemyID, randomTarget);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public virtual bool NearGround()
	{
		return false;
	}

	public virtual bool isFlying()
	{
		return false;
	}

	public override void PlaySound(string name)
	{
		AudioManager.GetInstance().PlaySoundAt(name, entityObject, entityTransform.position, AudioRolloffMode.Linear, 200f);
	}

	public override void PlaySoundSingle(string name)
	{
		AudioManager.GetInstance().PlaySoundSingleAt(name, entityObject, entityTransform.position, AudioRolloffMode.Linear, 200f);
	}

	public virtual bool NeedMoveDown()
	{
		return true;
	}

	public override void DoShoutAudio()
	{
	}

	public virtual void EnableTrailEffect(bool bEnable)
	{
		if (bEnable)
		{
			GameObject[] array = mTrails;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = true;
			}
		}
		else
		{
			GameObject[] array2 = mTrails;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = false;
			}
		}
	}

	protected virtual void StopSoundOnHit()
	{
	}

	public virtual void EnableGravity(bool bEnable)
	{
		mEnableGravity = bEnable;
	}

	public virtual void StartSeeBoss()
	{
		entityObject.SetActive(true);
		mIsActive = true;
	}

	public virtual void StartBossBattle()
	{
		StartEnemyIdle();
	}

	public override void StartDead()
	{
		if (mIsActive)
		{
			base.StartDead();
			Debug.Log("Dodge_This --- Stop");
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger);
		}
	}

	public override void StartEnemyIdle()
	{
		SetState(Enemy.IDLE_STATE);
		SetIdleTimeNow();
		if (base.IsMasterPlayer)
		{
			ChooseTargetPlayer(true);
		}
	}

	public override void DoEnemyIdle()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (base.IsMasterPlayer && GetIdleTimeDuration() > mIdleTime)
		{
			MakeDecisionInEnemyIdle();
		}
	}

	protected override bool NeedGoBack()
	{
		return false;
	}

	public override void StartGotHit()
	{
		NeedRage();
		SetState(Enemy.GOTHIT_STATE);
		StopNavMesh();
		PlayGotHitSound();
	}

	protected virtual void StartBossInit()
	{
		SetState(INIT_STATE);
		entityObject.SetActive(false);
		mIsActive = false;
	}

	public virtual void DoBossInit()
	{
		PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
	}

	protected virtual void EndBossInit()
	{
	}
}
