using System;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryBoss : EnemyBoss
{
	public static EnemyState MERCENARY_BOSS_AIR_ATTACK = new MercenaryBossAirAttackState();

	public static EnemyState MERCENARY_BOSS_AIR_DOWN = new MercenaryBossAirDownState();

	public static EnemyState MERCENARY_BOSS_AIR_IDLE = new MercenaryBossAirIdleState();

	public static EnemyState MERCENARY_BOSS_AIR_UP = new MercenaryBossAirUpState();

	public static EnemyState MERCENARY_BOSS_ATTACK = new MercenaryBossAttackState();

	public static EnemyState MERCENARY_BOSS_DASH_READY = new MercenaryBossDashReadyState();

	public static EnemyState MERCENARY_BOSS_DASH = new MercenaryBossDashState();

	public static EnemyState MERCENARY_BOSS_END_ATTACK = new MercenaryBossEndAttackState();

	public static EnemyState MERCENARY_BOSS_IDLE_FIRE = new MercenaryBossIdleFireState();

	public static EnemyState MERCENARY_BOSS_RELOAD = new MercenaryBossReloadState();

	public static EnemyState MERCENARY_BOSS_ROCKET_FORWARD = new MercenaryBossRocketForwardState();

	public static EnemyState MERCENARY_BOSS_ROCKET_READY = new MercenaryBossRocketReadyState();

	public static EnemyState MERCENARY_BOSS_START_ATTACK = new MercenaryBossStartAttackState();

	protected int mMaxIdleFireCount;

	protected int mMaxAirAttackCount;

	protected int mMaxRocketAttackCount;

	protected int mCurrentIdleFireCount;

	protected int mCurrentAirAttackCount;

	protected int mCurrentRocketAttackCount;

	protected int mCurrentBulletCount;

	protected Vector3 mDashTarget;

	protected Timer mRushTimer = new Timer();

	protected Timer mDashTimer = new Timer();

	protected Transform mWeaponRifleTransform;

	protected Transform mWeaponRpgTransform;

	protected Transform mRightHandPointTransform;

	protected Transform leftJet;

	protected Transform rightJet;

	protected GameObject mJetEffectLeft;

	protected GameObject mJetEffectRight;

	public override void Init()
	{
		base.Init();
		mMaxAirAttackCount = 3;
		mRushTimer.SetTimer(30f, true);
		mDashTimer.SetTimer(3f, false);
	}

	protected override string GetBodyMeshName()
	{
		return BoneName.MercenaryBossMesh;
	}

	public override void Activate()
	{
		base.Activate();
		animation[AnimationString.MERCENARY_BOSS_IDLE_FIRE].speed = 2f / 15f / mRangedOneShotTime1;
		animation[AnimationString.MERCENARY_BOSS_AIR_ATTACK].speed = 2f / 3f / mRangedOneShotTime2;
		animation[AnimationString.MERCENARY_BOSS_ATTACK].speed = 2f / 3f / mRangedOneShotTime2;
		Transform parent = entityObject.transform.Find(BoneName.MercenaryBossLeftHandPoint);
		GameObject original = Resources.Load("WeaponL/assault05_l") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.name = "Gun";
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mRightHandPointTransform = entityObject.transform.Find(BoneName.MercenaryBossRightHandPoint);
		GameObject original2 = Resources.Load("WeaponL/rpg01_l") as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original2) as GameObject;
		gameObject2.name = "Gun";
		gameObject2.transform.parent = mRightHandPointTransform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mWeaponRifleTransform = entityObject.transform.Find(BoneName.MercenaryBossWeaponRifleFire);
		mWeaponRpgTransform = entityObject.transform.Find(BoneName.MercenaryBossWeaponRpgFire);
		GameObject original3 = Resources.Load("RPG_effect/RPG_BossGYB_001") as GameObject;
		leftJet = entityTransform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/bone_l_fasheqi/bone_l_fasheqi_Point");
		rightJet = entityTransform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/bone_r_fasheqi/bone_r_fasheqi_Point");
		mJetEffectLeft = UnityEngine.Object.Instantiate(original3, Vector3.zero, Quaternion.identity) as GameObject;
		mJetEffectLeft.transform.parent = leftJet;
		mJetEffectLeft.transform.localPosition = Vector3.zero;
		mJetEffectLeft.transform.localRotation = Quaternion.identity;
		mJetEffectLeft.SetActive(false);
		mJetEffectRight = UnityEngine.Object.Instantiate(original3, Vector3.zero, Quaternion.identity) as GameObject;
		mJetEffectRight.transform.parent = rightJet;
		mJetEffectRight.SetActive(false);
		mJetEffectRight.transform.localPosition = Vector3.zero;
		mJetEffectRight.transform.localRotation = Quaternion.identity;
	}

	private string GetDashAnimation()
	{
		string result = AnimationString.MERCENARY_BOSS_DASH_FORWARD;
		if (null != mNavMeshAgent)
		{
			float num = Vector3.Angle(mNavMeshAgent.velocity, entityTransform.forward);
			if (num > 135f)
			{
				result = AnimationString.MERCENARY_BOSS_DASH_BACK;
			}
			else if (num > 45f)
			{
				result = ((!(Vector3.Cross(mNavMeshAgent.velocity, entityTransform.forward).y > 0f)) ? AnimationString.MERCENARY_BOSS_DASH_RIGHT : AnimationString.MERCENARY_BOSS_DASH_LEFT);
			}
		}
		return result;
	}

	private void SetNavMeshForDashToTarget()
	{
		if (null != mNavMeshAgent)
		{
			mNavMeshAgent.Resume();
			mNavMeshAgent.SetDestination(mDashTarget);
			mNavMeshAgent.speed = mRunSpeed;
			SetCanTurn(true);
		}
	}

	private void SetNavMeshForRocketForwardToTarget()
	{
		if (mTarget != null)
		{
			Vector3 vector = mTargetPosition;
			Vector3 normalized = (vector - entityTransform.position).normalized;
			vector -= 2f * normalized;
			if (null != mNavMeshAgent)
			{
				mNavMeshAgent.Resume();
				mNavMeshAgent.SetDestination(vector);
				mNavMeshAgent.speed = mRushAttackSpeed1;
				SetCanTurn(false);
			}
		}
	}

	private void Shoot()
	{
		if (!mIsShoot)
		{
			CheckShoot();
			mIsShoot = true;
		}
		if (mIsShoot && animation[AnimationString.MERCENARY_BOSS_IDLE_FIRE].time > animation[AnimationString.MERCENARY_BOSS_IDLE_FIRE].clip.length)
		{
			animation[AnimationString.MERCENARY_BOSS_IDLE_FIRE].time -= animation[AnimationString.MERCENARY_BOSS_IDLE_FIRE].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
		}
	}

	private void CheckShoot()
	{
		bool flag = false;
		Vector3 vector = mTargetPosition + new Vector3(0f, 1.5f, 0f);
		if (mTarget == mLocalPlayer)
		{
			float distanceFromTarget = GetDistanceFromTarget();
			float num = UnityEngine.Random.Range(0f, distanceFromTarget);
			if (num < 4f)
			{
				vector = vector + UnityEngine.Random.Range(-0.1f, 0.1f) * entityTransform.right + UnityEngine.Random.Range(-0.1f, 0.1f) * entityTransform.up;
			}
			else
			{
				float num2 = UnityEngine.Random.Range(-0.01f, 0.01f);
				vector = vector + (num2 * distanceFromTarget + 0.5f * Mathf.Sign(num2)) * entityTransform.right + UnityEngine.Random.Range(-0.7f, 0.7f) * entityTransform.up;
			}
		}
		else
		{
			vector = vector + UnityEngine.Random.Range(-1f, 1f) * entityTransform.right + UnityEngine.Random.Range(-0.5f, 0.5f) * entityTransform.up;
		}
		bool flag2 = false;
		Vector3 vector2 = vector - mWeaponRifleTransform.position;
		vector2.Normalize();
		mRay = new Ray(mWeaponRifleTransform.position, vector2);
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED);
		if (Physics.Raycast(mRay, out mRaycastHit, 100f, layerMask))
		{
			flag2 = true;
			if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				mLocalPlayer.OnHit(mRangedAttackDamage1, this);
				flag = true;
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
		gameScene.GetEffectPool(EffectPoolType.RIFLE_SPARK).CreateObject(mWeaponRifleTransform.position, vector2, Quaternion.identity);
		GameObject gameObject = gameScene.GetEffectPool(EffectPoolType.BULLET_TRAIL).CreateObject(mWeaponRifleTransform.position + 2.5f * vector2, vector2, Quaternion.identity);
		if (gameObject == null)
		{
			Debug.Log("fire line obj null");
		}
		else
		{
			BulletTrailScript component = gameObject.GetComponent<BulletTrailScript>();
			component.beginPos = mWeaponRifleTransform.position + 2.5f * vector2;
			if (flag2)
			{
				if (flag)
				{
					component.endPos = mRaycastHit.point + 2.5f * vector2;
				}
				else
				{
					component.endPos = mRaycastHit.point - 2.5f * vector2;
				}
			}
			else
			{
				component.endPos = mWeaponRifleTransform.position + vector2 * 100f;
			}
			component.speed = 100f;
			component.isActive = true;
		}
		PlaySound("RPG_Audio/Weapon/assault_rifle/assault_fire");
	}

	public override Vector3 GetShieldScale()
	{
		return new Vector3(1f, 1f, 1f);
	}

	protected override void ChangeShader()
	{
		base.ChangeShader();
		GameObject gameObject = entityObject.transform.Find(BoneName.MercenaryBossWeaponRifle).gameObject;
		gameObject.SetActive(false);
		GameObject gameObject2 = entityObject.transform.Find(BoneName.MercenaryBossWeaponRpg).gameObject;
		gameObject2.SetActive(false);
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (MERCENARY_BOSS_AIR_ATTACK == mState)
		{
			EndMercenaryBossAirAttack();
		}
		else if (MERCENARY_BOSS_AIR_DOWN == mState)
		{
			EndMercenaryBossAirDown();
		}
		else if (MERCENARY_BOSS_AIR_IDLE == mState)
		{
			EndMercenaryBossAirIdle();
		}
		else if (MERCENARY_BOSS_AIR_UP == mState)
		{
			EndMercenaryBossAirUp();
		}
		else if (MERCENARY_BOSS_ATTACK == mState)
		{
			EndMercenaryBossAttack();
		}
		else if (MERCENARY_BOSS_DASH_READY == mState)
		{
			EndMercenaryBossDashReady();
		}
		else if (MERCENARY_BOSS_DASH == mState)
		{
			EndMercenaryBossDash();
		}
		else if (MERCENARY_BOSS_END_ATTACK == mState)
		{
			EndMercenaryBossEndAttack();
		}
		else if (MERCENARY_BOSS_IDLE_FIRE == mState)
		{
			EndMercenaryBossIdleFire();
		}
		else if (MERCENARY_BOSS_RELOAD == mState)
		{
			EndMercenaryBossReload();
		}
		else if (MERCENARY_BOSS_ROCKET_FORWARD == mState)
		{
			EndMercenaryBossRocketForward();
		}
		else if (MERCENARY_BOSS_ROCKET_READY == mState)
		{
			EndMercenaryBossRocketReady();
		}
		else if (MERCENARY_BOSS_START_ATTACK == mState)
		{
			EndMercenaryBossStartAttack();
		}
	}

	protected override void PlayDeadSound()
	{
		StopSound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		PlaySound("RPG_Audio/Enemy/Mob/Mob_dead");
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
		float horizontalSqrDistanceFromTarget = GetHorizontalSqrDistanceFromTarget();
		if (mRushTimer.Ready() && horizontalSqrDistanceFromTarget < (float)(mRushAttackRadius * mRushAttackRadius))
		{
			mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), mTargetPosition - entityTransform.position);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER);
			if (Physics.Raycast(mRay, out mRaycastHit, GetDistanceFromTarget(), layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.PLAYER || mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
			{
				mRushTimer.Do();
				EndEnemyIdle();
				StartMercenaryBossRocketReady();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.MERCENARY_BOSS_ROCKET_READY, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		if (enemyStateConst != 0)
		{
			return;
		}
		float num = 0f;
		int num2 = UnityEngine.Random.Range(0, 100);
		num = ((num2 >= 50) ? UnityEngine.Random.Range(-20f, -100f) : UnityEngine.Random.Range(20f, 100f));
		Vector3 vector = mTargetPosition - entityTransform.position;
		vector.Normalize();
		Vector3 vector2 = vector + entityTransform.right * Mathf.Tan(num * ((float)Math.PI / 180f));
		vector2.Normalize();
		float num3 = UnityEngine.Random.Range(10f, 18f);
		Vector3 vector3 = entityTransform.position + vector2 * num3 + new Vector3(0f, 0.2f, 0f);
		if (TryPathForNavMesh(vector3))
		{
			EndEnemyIdle();
			StartMercenaryBossDashReady(vector3);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.MERCENARY_BOSS_DASH_READY, entityTransform.position, vector3);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	protected override void PlayGotHitSound()
	{
		StopSound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		PlaySound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_attacked");
	}

	public void StartMercenaryBossAirAttack()
	{
		SetState(MERCENARY_BOSS_AIR_ATTACK);
		LookAtTargetHorizontal();
		mIsShoot = false;
		mCurrentAirAttackCount++;
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossAirAttack()
	{
		PlaySoundSingle("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		PlayAnimation(AnimationString.MERCENARY_BOSS_AIR_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (mTarget == null)
		{
			mCurrentAirAttackCount = mMaxAirAttackCount;
		}
		if (!mIsShoot)
		{
			mIsShoot = true;
			Vector3 forward = mTargetPosition - mWeaponRpgTransform.position;
			GameObject original = Resources.Load("RPG_effect/RPG_Cypher_rocket_001") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, mWeaponRpgTransform.position, Quaternion.LookRotation(forward)) as GameObject;
			EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
			component.enemy = this;
			component.speed = forward.normalized * mRangedBulletSpeed2;
			component.attackDamage = 0;
			component.areaDamage = mRangedExtraDamage2;
			component.damageType = DamageType.Explosion;
			component.trType = TrajectoryType.Straight;
			component.enemyType = mEnemyType;
			component.explodeEffect = "Effect/Enemy/Boss/MercenaryBoss/MercenaryBossRpgExplosion";
			component.explodeRadius = mRangedExplosionRadius2;
			PlaySound("RPG_Audio/Weapon/rpg/rpg_fire");
		}
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_AIR_ATTACK, 1f))
		{
			EndMercenaryBossAirAttack();
			StartMercenaryBossAirIdle();
		}
	}

	private void EndMercenaryBossAirAttack()
	{
		mIsShoot = false;
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	public void StartMercenaryBossAirDown()
	{
		SetState(MERCENARY_BOSS_AIR_DOWN);
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
		StopSound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		PlaySound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_land");
	}

	public void DoMercenaryBossAirDown()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_AIR_DOWN, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_AIR_DOWN, 1f))
		{
			EndMercenaryBossAirDown();
			StartEnemyIdle();
		}
	}

	private void EndMercenaryBossAirDown()
	{
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	private void StartMercenaryBossAirIdle()
	{
		SetState(MERCENARY_BOSS_AIR_IDLE);
		SetIdleTimeNow();
		LookAtTargetHorizontal();
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossAirIdle()
	{
		PlaySoundSingle("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		PlayAnimation(AnimationString.MERCENARY_BOSS_AIR_IDLE, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (!base.IsMasterPlayer || !(GetIdleTimeDuration() > mRangedInterval2))
		{
			return;
		}
		EndMercenaryBossAirIdle();
		if (mCurrentAirAttackCount < mMaxAirAttackCount)
		{
			StartMercenaryBossAirAttack();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.MERCENARY_BOSS_AIR_ATTACK, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			StartMercenaryBossAirDown();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.MERCENARY_BOSS_AIR_DOWN, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}

	private void EndMercenaryBossAirIdle()
	{
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	public void StartMercenaryBossAirUp()
	{
		SetState(MERCENARY_BOSS_AIR_UP);
		mCurrentAirAttackCount = 0;
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
		PlaySound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_rise");
	}

	public void DoMercenaryBossAirUp()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_AIR_UP, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_AIR_UP, 1f))
		{
			EndMercenaryBossAirUp();
			StartMercenaryBossAirAttack();
		}
	}

	private void EndMercenaryBossAirUp()
	{
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	public void StartMercenaryBossAttack()
	{
		SetState(MERCENARY_BOSS_ATTACK);
		LookAtTargetHorizontal();
		mCurrentRocketAttackCount = 0;
		mIsShoot = false;
	}

	public void DoMercenaryBossAttack()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_ATTACK, WrapMode.Loop);
		LookAtTargetHorizontal();
		if (mTarget == null)
		{
			mCurrentRocketAttackCount = mMaxRocketAttackCount - 1;
		}
		if (!mIsShoot)
		{
			mIsShoot = true;
			Vector3 forward = mTargetPosition - mWeaponRpgTransform.position;
			GameObject original = Resources.Load("Effect/Enemy/Boss/MercenaryBoss/MercenaryBossRpgBulletNew") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, mWeaponRpgTransform.position, Quaternion.LookRotation(forward)) as GameObject;
			EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
			component.enemy = this;
			component.speed = forward.normalized * mRangedBulletSpeed2;
			component.attackDamage = 0;
			component.areaDamage = mRangedExtraDamage2;
			component.damageType = DamageType.Explosion;
			component.trType = TrajectoryType.Straight;
			component.enemyType = mEnemyType;
			component.explodeEffect = "Effect/Enemy/Boss/MercenaryBoss/MercenaryBossRpgExplosion";
			component.explodeRadius = mRangedExplosionRadius2;
			PlaySound("RPG_Audio/Weapon/rpg/rpg_fire");
		}
		if (mIsShoot && animation[AnimationString.MERCENARY_BOSS_ATTACK].time > animation[AnimationString.MERCENARY_BOSS_ATTACK].clip.length)
		{
			animation[AnimationString.MERCENARY_BOSS_ATTACK].time -= animation[AnimationString.MERCENARY_BOSS_ATTACK].clip.length;
			mCurrentRocketAttackCount++;
			mIsShoot = false;
		}
		if (mCurrentRocketAttackCount >= mMaxRocketAttackCount)
		{
			EndMercenaryBossAttack();
			StartMercenaryBossEndAttack();
		}
	}

	private void EndMercenaryBossAttack()
	{
		mIsShoot = false;
	}

	public void StartMercenaryBossDashReady(Vector3 targetPoint)
	{
		SetState(MERCENARY_BOSS_DASH_READY);
		LookAtTargetHorizontal();
		mDashTarget = targetPoint;
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossDashReady()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_DASH_READY, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_DASH_READY, 1f))
		{
			EndMercenaryBossDashReady();
			StartMercenaryBossDash(mDashTarget);
		}
	}

	private void EndMercenaryBossDashReady()
	{
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	private void StartMercenaryBossDash(Vector3 targetPoint)
	{
		SetState(MERCENARY_BOSS_DASH);
		LookAtTargetHorizontal();
		mDashTarget = targetPoint;
		SetNavMeshForDashToTarget();
		mDashTimer.Do();
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossDash()
	{
		PlaySoundSingle("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		LookAtTargetHorizontal();
		string dashAnimation = GetDashAnimation();
		PlayAnimation(dashAnimation, WrapMode.Loop);
		Vector3 vector = entityTransform.position - mDashTarget;
		vector.y = 0f;
		if (!(vector.sqrMagnitude < 1f) && !mDashTimer.Ready())
		{
			return;
		}
		EndMercenaryBossDash();
		if (base.IsMasterPlayer)
		{
			if (mTarget == null)
			{
				ChooseTargetPlayer(true);
			}
			if (mTarget == null)
			{
				StartEnemyIdle();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, EnemyStateConst.IDLE, entityTransform.position);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				return;
			}
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			int num = 0;
			int num2 = UnityEngine.Random.Range(0, 100);
			if (num2 < mRangedAttackProbability)
			{
				num = 1;
				if (mMindState == MindState.NORMAL)
				{
					num = UnityEngine.Random.Range(2, 4);
				}
				StartMercenaryBossFirstIdleFire(num);
				enemyStateConst = EnemyStateConst.MERCENARY_BOSS_IDLE_FIRE;
			}
			else if (mMindState == MindState.NORMAL)
			{
				num = 1;
				if (mMindState == MindState.NORMAL)
				{
					num = UnityEngine.Random.Range(1, 3);
				}
				StartMercenaryBossStartAttack(num);
				enemyStateConst = EnemyStateConst.MERCENARY_BOSS_START_ATTCK;
			}
			else
			{
				StartMercenaryBossAirUp();
				enemyStateConst = EnemyStateConst.MERCENARY_BOSS_AIR_UP;
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request2 = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, entityTransform.position, (byte)num);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
		else
		{
			StartEnemyIdle();
		}
	}

	private void EndMercenaryBossDash()
	{
		StopNavMesh();
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
		StopSound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
	}

	private void StartMercenaryBossEndAttack()
	{
		SetState(MERCENARY_BOSS_END_ATTACK);
	}

	public void DoMercenaryBossEndAttack()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_END_ATTACK, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_END_ATTACK, 1f))
		{
			EndMercenaryBossEndAttack();
			StartEnemyIdle();
		}
	}

	private void EndMercenaryBossEndAttack()
	{
	}

	public void StartMercenaryBossFirstIdleFire(int maxCount)
	{
		SetState(MERCENARY_BOSS_IDLE_FIRE);
		LookAtTargetHorizontal();
		mMaxIdleFireCount = maxCount;
		mCurrentIdleFireCount = 1;
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	private void StartMercenaryBossIdleFire()
	{
		SetState(MERCENARY_BOSS_IDLE_FIRE);
		LookAtTargetHorizontal();
		mCurrentIdleFireCount++;
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public void DoMercenaryBossIdleFire()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_IDLE_FIRE, WrapMode.Loop);
		LookAtTargetHorizontal();
		Shoot();
		if (mTarget == null)
		{
			mCurrentIdleFireCount = mMaxIdleFireCount;
		}
		if (mCurrentBulletCount >= mRangedBulletCount1)
		{
			EndMercenaryBossIdleFire();
			if (mCurrentIdleFireCount < mMaxIdleFireCount)
			{
				StartMercenaryBossReload();
			}
			else
			{
				StartEnemyIdle();
			}
		}
	}

	private void EndMercenaryBossIdleFire()
	{
		mIsShoot = false;
	}

	private void StartMercenaryBossReload()
	{
		SetState(MERCENARY_BOSS_RELOAD);
		LookAtTargetHorizontal();
	}

	public void DoMercenaryBossReload()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_RELOAD, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_RELOAD, 1f))
		{
			EndMercenaryBossReload();
			StartMercenaryBossIdleFire();
		}
	}

	private void EndMercenaryBossReload()
	{
	}

	private void StartMercenaryBossRocketForward()
	{
		SetState(MERCENARY_BOSS_ROCKET_FORWARD);
		SetNavMeshForRocketForwardToTarget();
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossRocketForward()
	{
		PlaySoundSingle("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
		float num = animation[AnimationString.MERCENARY_BOSS_ROCKET_FORWARD].time / animation[AnimationString.MERCENARY_BOSS_ROCKET_FORWARD].clip.length;
		PlayAnimation(AnimationString.MERCENARY_BOSS_ROCKET_FORWARD, WrapMode.ClampForever);
		if (num > 0.37f)
		{
			if (mLocalPlayer.CheckHitTimerReady(this))
			{
				Vector3 vector = mLocalPlayer.GetTransform().position - mRightHandPointTransform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude < 2f)
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
					Vector3 vector2 = value.GetTransform().position - mRightHandPointTransform.position;
					vector2.y = 0f;
					if (vector2.sqrMagnitude < 2f)
					{
						value.OnHit(mRushAttackDamage1);
						value.ResetCheckHitTimer(this);
					}
				}
			}
		}
		if (num > 1f)
		{
			EndMercenaryBossRocketForward();
			StartEnemyIdle();
		}
	}

	private void EndMercenaryBossRocketForward()
	{
		StopNavMesh();
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
		StopSound("RPG_Audio/Enemy/MercenaryBoss/MercenaryBoss_fly");
	}

	public void StartMercenaryBossRocketReady()
	{
		SetState(MERCENARY_BOSS_ROCKET_READY);
		LookAtTargetHorizontal();
		mJetEffectLeft.SetActive(true);
		mJetEffectRight.SetActive(true);
	}

	public void DoMercenaryBossRocketReady()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_ROCKET_READY, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_ROCKET_READY, 1f))
		{
			EndMercenaryBossRocketReady();
			StartMercenaryBossRocketForward();
		}
	}

	private void EndMercenaryBossRocketReady()
	{
		mJetEffectLeft.SetActive(false);
		mJetEffectRight.SetActive(false);
	}

	public void StartMercenaryBossStartAttack(int maxCount)
	{
		SetState(MERCENARY_BOSS_START_ATTACK);
		mMaxRocketAttackCount = maxCount;
		LookAtTargetHorizontal();
	}

	public void DoMercenaryBossStartAttack()
	{
		PlayAnimation(AnimationString.MERCENARY_BOSS_START_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.MERCENARY_BOSS_START_ATTACK, 1f))
		{
			EndMercenaryBossStartAttack();
			StartMercenaryBossAttack();
		}
	}

	private void EndMercenaryBossStartAttack()
	{
	}
}
