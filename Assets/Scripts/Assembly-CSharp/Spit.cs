using UnityEngine;

public class Spit : Enemy
{
	protected const float SLIME_DAMAGE_INTERVAL = 1f;

	public static EnemyState Melee_ATTACK_STATE = new SpitMeleeAttackState();

	public static EnemyState SPIT_STATE01 = new SpitState01();

	public static EnemyState SPIT_STATE02 = new SpitState02();

	public static EnemyState SPIT_STATE03 = new SpitState03();

	protected MeleeAttackData SpitMeleeAttackData = default(MeleeAttackData);

	protected Timer mAttackTimer = new Timer();

	protected Timer mTrySpitTimer = new Timer();

	protected Timer mSpitTimer = new Timer();

	public override void Init()
	{
		base.Init();
		mAttackTimer.SetTimer(2f, false);
		mTrySpitTimer.SetTimer(2f, false);
		mSpitTimer.SetTimer(0.5f, false);
		mShieldType = ShieldType.MECHANICAL;
	}

	protected override string GetBodyMeshName()
	{
		return "spit";
	}

	public override void Activate()
	{
		base.Activate();
		SpitMeleeAttackData.Animation = AnimationString.SPIT_MELEE_ATTACK;
		SpitMeleeAttackData.Trans = entityTransform;
		SpitMeleeAttackData.StartPercent = 0.4f;
		SpitMeleeAttackData.EndPercent = 1f;
		SpitMeleeAttackData.Range = mMeleeAttackRadius;
		SpitMeleeAttackData.Angle = 60f;
		SpitMeleeAttackData.Damage = mMeleeAttackDamage1;
		SpitMeleeAttackData.KnockedSpeed = 0f;
	}

	protected override void InitBips()
	{
		mBodyTransform = entityTransform.Find("Bip02");
		mHeadTransform = entityTransform.Find("Bip02/Bip02 Pelvis/Bip02 Spine/Bip02 Neck/Bip02 Cover_M");
		mHitObjectArray = new GameObject[2];
		mHitObjectArray[0] = mBodyTransform.gameObject;
		mHitObjectArray[1] = mHeadTransform.gameObject;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
	}

	public void Shot()
	{
		GameObject original = Resources.Load("RPG_effect/RPG_Spit_fire_002") as GameObject;
		Vector3 position = entityTransform.Find("Bip02/Bip02 Pelvis/Bip02 Spine/Bip02 Neck/Bip02 Cover_M").position;
		GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		Vector3 vector = mTargetPosition - entityTransform.position;
		EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
		component.enemy = this;
		component.speed = vector.normalized * mRangedBulletSpeed1;
		component.attackDamage = mRangedAttackDamage1;
		component.areaDamage = mRangedExtraDamage1;
		component.damageType = DamageType.Slime;
		component.trType = TrajectoryType.Straight;
		component.enemyType = mEnemyType;
		component.explodeEffect = "Effect/Enemy/Boss/MercenaryBoss/MercenaryBossRpgExplosion";
		component.explodeRadius = mRangedExplosionRadius1;
		component.slimePrefab = "RPG_effect/RPG_Spit_fire_003";
		component.slimeDamageInterval = 1f;
	}

	public new virtual bool IsCrit()
	{
		return SPIT_STATE02 == mState;
	}

	protected override void PlayRunningSound()
	{
		PlayLogarithmicSingleSound("RPG_Audio/Enemy/Spit/Spit_move");
	}

	protected override void MakeDecisionInCatching()
	{
		EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
		float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
		if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
		{
			EndCatching();
			StartMeleeAttack();
			enemyStateConst = EnemyStateConst.SPIT_MELEE_ATTACK;
		}
		else if (sqrDistanceFromTarget < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && mTrySpitTimer.Ready() && canHitTargetPlayer())
		{
			mTrySpitTimer.Do();
			EndCatching();
			StartSpit01();
			enemyStateConst = EnemyStateConst.SPIT_SPIT01;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && enemyStateConst != 0)
		{
			EnemyStateRequest request = new EnemyStateRequest(base.PointID, base.EnemyID, enemyStateConst, GetTransform().position);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void MakeDecisionInEnemyIdle()
	{
		float num = Random.Range(-0.1f, 0.1f);
		if (GetIdleTimeDuration() > mIdleTime + num)
		{
			EnemyStateConst enemyStateConst = EnemyStateConst.NO_STATE;
			float sqrDistanceFromTarget = GetSqrDistanceFromTarget();
			if (sqrDistanceFromTarget < mMeleeAttackRadius * mMeleeAttackRadius)
			{
				EndEnemyIdle();
				StartMeleeAttack();
				enemyStateConst = EnemyStateConst.SPIT_MELEE_ATTACK;
			}
			else if (sqrDistanceFromTarget < (float)(mRangedStandAttackRadius * mRangedStandAttackRadius) && mTrySpitTimer.Ready() && canHitTargetPlayer())
			{
				mTrySpitTimer.Do();
				EndEnemyIdle();
				StartSpit01();
				enemyStateConst = EnemyStateConst.SPIT_SPIT01;
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
	}

	public override void DoPatrol()
	{
		base.DoPatrol();
		PlaySoundSingle("RPG_Audio/Enemy/Spit/Spit_move");
	}

	public void StartMeleeAttack()
	{
		SetState(Melee_ATTACK_STATE);
		PlaySound("RPG_Audio/Enemy/Spit/Spit_attack01");
	}

	public void DoMeleeAttack()
	{
		PlayAnimation(AnimationString.SPIT_MELEE_ATTACK, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		CheckEnemyAttack(SpitMeleeAttackData);
		if (AnimationPlayed(AnimationString.SPIT_MELEE_ATTACK, 1f))
		{
			StartEnemyIdle();
		}
	}

	protected void EndMeleeAttack()
	{
	}

	public void StartSpit01()
	{
		SetState(SPIT_STATE01);
		GameObject original = Resources.Load("RPG_effect/RPG_Spit_fire_001") as GameObject;
		Object.Instantiate(original, entityTransform.position, Quaternion.identity);
	}

	public void DoSpit01()
	{
		PlayAnimation(AnimationString.SPIT_SPIT_ATTACK01, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.SPIT_SPIT_ATTACK01, 1f))
		{
			StartSpit02();
		}
	}

	protected void EndSpit01()
	{
	}

	public void StartSpit02()
	{
		SetState(SPIT_STATE02);
	}

	public void DoSpit02()
	{
		PlayAnimation(AnimationString.SPIT_SPIT_ATTACK02, WrapMode.ClampForever);
		if (mSpitTimer.Ready() && AnimationPlayed(AnimationString.SPIT_SPIT_ATTACK02, 0.9f))
		{
			mSpitTimer.Do();
			Shot();
		}
		if (AnimationPlayed(AnimationString.SPIT_SPIT_ATTACK02, 1f))
		{
			SetState(SPIT_STATE03);
		}
	}

	protected void EndSpit02()
	{
	}

	public void DoSpit03()
	{
		PlayAnimation(AnimationString.SPIT_SPIT_ATTACK03, WrapMode.ClampForever);
		LookAtTargetHorizontal();
		if (AnimationPlayed(AnimationString.SPIT_SPIT_ATTACK03, 1f))
		{
			StartEnemyIdle();
		}
	}

	protected void EndSpit03()
	{
	}

	protected override void PlayGotHitSound()
	{
		PlaySound("RPG_Audio/Enemy/Spit/Spit_attacked");
	}

	protected override void PlayDeadSound()
	{
		PlaySound("RPG_Audio/Enemy/Spit/Spit_dead");
	}

	protected override void PlayDeadBloodEffect()
	{
	}

	protected override void PlayGroundDeadBloodEffect(Vector3 normal)
	{
		if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying())
		{
			GameObject original = Resources.Load("RPG_effect/RPG_Spit_dead_001") as GameObject;
			Object.Instantiate(original, entityTransform.position, Quaternion.FromToRotation(Vector3.up, normal));
		}
	}
}
