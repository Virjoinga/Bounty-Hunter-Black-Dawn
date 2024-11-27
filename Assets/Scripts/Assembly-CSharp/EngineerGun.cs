using System.Collections.Generic;
using UnityEngine;

public class EngineerGun : SummonedItem
{
	private const short EngineerGunHPBonusSkillID = 1035;

	private const short EngineerGunShieldBonusSkillID = 1036;

	private const short ShieldRecoverBonusSkillID = 1037;

	private const short MoveSpeedBonusSkillID = 1038;

	private const short BulletRecoverSkillID = 1043;

	private const short AttackRateBonusSkillID = 1044;

	private const short DamageBonusSkillID = 1047;

	private const short EngineerGunAttackRateBonusSkillID = 1041;

	private const short EngineerGunDamageBonusSkillID = 1042;

	private const short UpgradSkillID = 1046;

	public static ControllableUnitState ENGINEER_GUN_RUN_STATE = new EngineerGunRunState();

	public static ControllableUnitState ENGINEER_GUN_FIRE_STATE = new EngineerGunFireState();

	public static ControllableUnitState ENGINEER_GUN_CANNON_STATE = new EngineerGunCannonState();

	public static ControllableUnitState ENGINEER_GUN_RUN_FIRE_STATE = new EngineerGunRunFireState();

	public static ControllableUnitState ENGINEER_GUN_RUN_CANNON_STATE = new EngineerGunRunCannonState();

	protected Transform mBodyTransform;

	protected GameObject mLeftSpark;

	protected GameObject mRightSpark;

	protected Timer mDistanceCheckTimer = new Timer();

	protected float mLastUpdateNavMeshTime;

	protected GameUnit mTargetGameUnit;

	protected Vector3 mTargetPoint;

	private Ray mRay;

	private RaycastHit mRaycastHit;

	protected float mLastIdleTime;

	protected bool mIsShoot;

	protected int mCurrentBulletCount;

	protected int mCurrentCannonCount;

	protected float mMaxIdleTime;

	protected float mStartRunDistance;

	protected float mStopRunDistance;

	protected float mAttackDistance;

	protected int mMaxBulletCount;

	protected int mMaxCannonCount;

	protected int mFireDamage;

	protected int mCannonDamage;

	protected int mFireProbability;

	protected float mFireAnimationSpeed;

	protected float mCannonAnimationSpeed;

	private Weapon tempWeapon;

	protected short EngineerGunHPBonusPercentage;

	protected short EngineerGunShieldBonusPercentage;

	protected short ShieldRecoverBonusPercentage;

	protected short ShieldRecoverBonusRange = 10;

	protected string ShieldRecoverIcon = string.Empty;

	protected short MoveSpeedBonusPercentage;

	protected short MoveSpeedBonusRange = 10;

	protected string MoveSpeedIcon = string.Empty;

	protected short BulletRecoverCount;

	protected short BulletRecoverTime;

	protected short BulletRecoverRange = 10;

	private bool BulletRecvoerSkillAdded;

	protected short AttackRateBonusPercentage;

	protected short AttackRateBonusRange = 10;

	protected string AttackRateIcon = string.Empty;

	protected short DamageBonusPercentage;

	protected short DamageBonusRange = 10;

	protected string DamageBonusIcon = string.Empty;

	protected short EngineerGunAttackRateBonusPercentage;

	protected short EngineerGunDamageBonusPercentage;

	protected bool IsUpgrade;

	protected short UpgradeDamageBonus;

	private Timer mShieldRecoverTimer = new Timer();

	private Timer mMoveSpeedTimer = new Timer();

	private Timer mAttackRateTimer = new Timer();

	private Timer mDamageBonusTimer = new Timer();

	public EngineerGun()
	{
		base.SummonedType = ESummonedType.ENGINEER_GUN;
	}

	public override short GetPara1()
	{
		return EngineerGunHPBonusPercentage;
	}

	public override short GetPara2()
	{
		return EngineerGunShieldBonusPercentage;
	}

	public override short GetPara4()
	{
		return ShieldRecoverBonusPercentage;
	}

	public override short GetPara5()
	{
		return ShieldRecoverBonusRange;
	}

	public override short GetPara6()
	{
		return MoveSpeedBonusPercentage;
	}

	public override short GetPara7()
	{
		return MoveSpeedBonusRange;
	}

	public override short GetPara8()
	{
		return BulletRecoverCount;
	}

	public override short GetPara9()
	{
		return BulletRecoverTime;
	}

	public override short GetPara10()
	{
		return BulletRecoverRange;
	}

	public override short GetPara11()
	{
		return AttackRateBonusPercentage;
	}

	public override short GetPara12()
	{
		return AttackRateBonusRange;
	}

	public override short GetPara13()
	{
		return DamageBonusPercentage;
	}

	public override short GetPara14()
	{
		return DamageBonusRange;
	}

	public override void InitValuesAndRanges(short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
		EngineerGunHPBonusPercentage = para1;
		EngineerGunShieldBonusPercentage = para2;
		ShieldRecoverBonusPercentage = para4;
		ShieldRecoverBonusRange = para5;
		MoveSpeedBonusPercentage = para6;
		MoveSpeedBonusRange = para7;
		BulletRecoverCount = para8;
		BulletRecoverTime = para9;
		BulletRecoverRange = para10;
		AttackRateBonusPercentage = para11;
		AttackRateBonusRange = para12;
		DamageBonusPercentage = para13;
		DamageBonusRange = para14;
		base.MaxHp = Mathf.CeilToInt((float)base.MaxHp * (1f + (float)EngineerGunHPBonusPercentage * 0.01f));
		base.MaxShield = Mathf.CeilToInt((float)base.MaxShield * (1f + (float)EngineerGunShieldBonusPercentage * 0.01f));
	}

	protected override string GetResourcePath()
	{
		return "Controllable/Summoned/engineer_gun";
	}

	public override void Init()
	{
		base.Init();
		mOwnerPlayer.AddSummoned(base.Name, this);
		mLastUpdateNavMeshTime = Time.time;
		mBodyTransform = entityObject.transform.Find(BoneName.EngineerGunBody);
		CreateNavMeshAgent();
		mLeftSpark = entityObject.transform.Find(BoneName.EngineerGunLeftSpark).gameObject;
		mRightSpark = entityObject.transform.Find(BoneName.EngineerGunRightSpark).gameObject;
		mLeftSpark.SetActive(false);
		mRightSpark.SetActive(false);
		mTargetGameUnit = null;
		mTargetPoint = new Vector3(mOwnerPlayer.GetTransform().position.x, mBodyTransform.position.y, mOwnerPlayer.GetTransform().position.z);
		mDistanceCheckTimer.SetTimer(7f, true);
		mMaxIdleTime = 2f;
		mMaxBulletCount = 24;
		mMaxCannonCount = 2;
		mStartRunDistance = 20f;
		mStopRunDistance = 5f;
		mAttackDistance = 20f;
		mFireProbability = 100;
		if (IsUpgrade)
		{
			mFireProbability = 50;
		}
		mFireAnimationSpeed = animation[AnimationString.ENGINEER_GUN_FIRE].clip.length / 0.25f * (1f + (float)EngineerGunAttackRateBonusPercentage * 0.01f);
		mCannonAnimationSpeed = animation[AnimationString.ENGINEER_GUN_CANNON].clip.length / 3f * (1f + (float)EngineerGunAttackRateBonusPercentage * 0.01f);
		tempWeapon = new RocketLauncher();
		tempWeapon.ExplodeSoundName = "RPG_Audio/Weapon/rpg/rpg_boom";
		ShieldRecoverIcon = GameConfig.GetInstance().skillConfig[10371].IconName;
		MoveSpeedIcon = GameConfig.GetInstance().skillConfig[10381].IconName;
		AttackRateIcon = GameConfig.GetInstance().skillConfig[10441].IconName;
		DamageBonusIcon = GameConfig.GetInstance().skillConfig[10471].IconName;
	}

	public override void LateLoop(float deltaTime)
	{
		if (mState == ENGINEER_GUN_FIRE_STATE || mState == ENGINEER_GUN_RUN_FIRE_STATE || mState == ENGINEER_GUN_CANNON_STATE || mState == ENGINEER_GUN_RUN_CANNON_STATE)
		{
			mBodyTransform.LookAt(mTargetPoint);
			mBodyTransform.RotateAround(mBodyTransform.position, mBodyTransform.forward, -90f);
		}
	}

	public override void EndCurrentState()
	{
		base.EndCurrentState();
		if (mState == ENGINEER_GUN_RUN_STATE)
		{
			EndEngineerGunRun();
		}
		else if (mState == ENGINEER_GUN_FIRE_STATE)
		{
			EndEngineerGunFire();
		}
		else if (mState == ENGINEER_GUN_CANNON_STATE)
		{
			EndEngineerGunCannon();
		}
		else if (mState == ENGINEER_GUN_RUN_FIRE_STATE)
		{
			EndEngineerGunRunFire();
		}
		else if (mState == ENGINEER_GUN_RUN_CANNON_STATE)
		{
			EndEngineerGunRunCannon();
		}
	}

	protected void UpdateTargetPosition()
	{
		if (mTargetGameUnit == null)
		{
			return;
		}
		if (mTargetGameUnit.InPlayingState())
		{
			if (null != mTargetGameUnit.GetTransform())
			{
				mTargetPoint = new Vector3(mTargetGameUnit.GetTransform().position.x, mBodyTransform.position.y, mTargetGameUnit.GetTransform().position.z);
			}
			else
			{
				mTargetGameUnit = null;
			}
		}
		else
		{
			mTargetGameUnit = null;
		}
	}

	protected void Fire(string shootAnimation)
	{
		if (!mIsShoot)
		{
			CheckFire();
			mIsShoot = true;
			mLeftSpark.SetActive(true);
			mRightSpark.SetActive(true);
			mLeftSpark.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
			mRightSpark.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
		}
		if (mIsShoot && animation[shootAnimation].time > animation[shootAnimation].clip.length)
		{
			animation[shootAnimation].time -= animation[shootAnimation].clip.length;
			mCurrentBulletCount++;
			mIsShoot = false;
			mLeftSpark.SetActive(false);
			mRightSpark.SetActive(false);
		}
	}

	protected void CheckFire()
	{
		PlaySound("RPG_Audio/Weapon/assault_rifle/assault_fire");
		if (!base.IsMaster)
		{
			return;
		}
		Vector3 direction = mTargetPoint - entityTransform.position;
		mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), direction);
		mRaycastHit = default(RaycastHit);
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED);
		if (!Physics.Raycast(mRay, out mRaycastHit, mAttackDistance, layerMask))
		{
			return;
		}
		DamageProperty damageProperty = new DamageProperty();
		damageProperty.hitForce = mRay.direction * 2f;
		damageProperty.damage = mFireDamage;
		damageProperty.criticalAttack = false;
		damageProperty.hitpoint = mRaycastHit.point;
		damageProperty.isLocal = true;
		damageProperty.wType = WeaponType.NoGun;
		damageProperty.isPenetration = false;
		damageProperty.unitLevel = base.Level;
		damageProperty.weaponLevel = base.Level;
		damageProperty.attackerType = DamageProperty.AttackerType._EngineerGun;
		if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.ENEMY || mRaycastHit.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD)
		{
			GameObject enemyByCollider = Enemy.GetEnemyByCollider(mRaycastHit.collider);
			if (enemyByCollider.name.StartsWith("E_"))
			{
				Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID(enemyByCollider.name);
				if (enemyByID.InPlayingState())
				{
					enemyByID.HitEnemy(damageProperty);
				}
			}
		}
		else if (mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				int num = int.Parse(mRaycastHit.collider.transform.parent.name);
				RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(num);
				if (remotePlayerByUserID != null && !remotePlayerByUserID.IsSameTeam(mOwnerPlayer))
				{
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(mOwnerPlayer.GetUserID(), (short)damageProperty.damage, num, false, (byte)damageProperty.elementType, damageProperty.criticalAttack, damageProperty.isTriggerDlementDot, damageProperty.elementDotDamage, damageProperty.elementDotTime, damageProperty.wType, damageProperty.attackerType);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		else
		{
			if (mRaycastHit.collider.gameObject.layer != PhysicsLayer.SUMMONED || !GameApp.GetInstance().GetGameMode().IsMultiPlayer() || !GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				return;
			}
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(mRaycastHit.collider);
			if (!(controllableByCollider != null))
			{
				return;
			}
			foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
			{
				if (remotePlayer != null)
				{
					SummonedItem summonedByName = remotePlayer.GetSummonedByName(controllableByCollider.name);
					if (summonedByName != null && !summonedByName.IsSameTeam())
					{
						ControllableItemOnHitRequest request2 = new ControllableItemOnHitRequest(summonedByName.ControllableType, summonedByName.ID, damageProperty.damage);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						break;
					}
				}
			}
		}
	}

	protected void CannonShoot()
	{
		PlaySound("RPG_Audio/Summoned/turret/turret_fire02");
		Vector3 vector = GetPosition() + new Vector3(0f, 0.8f, 0f);
		Vector3 vector2 = mTargetPoint + new Vector3(0f, 0.2f, 0f);
		Vector3 normalized = (vector2 - vector).normalized;
		string path = "RPG_effect/RPG_RPG01_Projectile";
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original, vector + normalized, Quaternion.LookRotation(normalized)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized;
		component.flySpeed = 16f;
		component.explodeRadius = 10f;
		component.hitForce = 0f;
		component.life = 8f;
		component.damage = mCannonDamage;
		component.GunType = WeaponType.RPG;
		component.targetPos = vector2;
		component.bagIndex = 0;
		component.weapon = tempWeapon;
		component.isLocal = base.IsMaster;
		component.level = base.Level;
		component.attackerType = DamageProperty.AttackerType._EngineerGun;
		mCurrentCannonCount++;
	}

	protected override void StartInit()
	{
		SetState(ControllableUnit.INIT_STATE);
		GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
		PlaySound("RPG_Audio/Summoned/turret/turret_on");
		if (base.IsMaster)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Take_a_rest, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
			foreach (CharacterInstantSkill triggerSkill in mOwnerPlayer.GetCharacterSkillManager().GetTriggerSkillList())
			{
				switch (triggerSkill.skillID)
				{
				case 1038:
				{
					SkillConfig skillConfig = GameConfig.GetInstance().skillConfig[triggerSkill.skillID * 10 + triggerSkill.SkillLevel];
					CharacterStateSkill characterStateSkill = new CharacterStateSkill((short)skillConfig.X1);
					MoveSpeedBonusPercentage = (byte)Mathf.CeilToInt(characterStateSkill.BuffValueY1);
					MoveSpeedBonusRange = triggerSkill.Range;
					break;
				}
				case 1043:
					BulletRecoverCount = (short)triggerSkill.EffectValueX;
					BulletRecoverTime = (short)triggerSkill.EffectValueY;
					BulletRecoverRange = triggerSkill.Range;
					break;
				}
			}
			foreach (CharacterStateSkill stateSkill in mOwnerPlayer.GetCharacterSkillManager().GetStateSkillList())
			{
				switch (stateSkill.skillID)
				{
				case 1035:
					EngineerGunHPBonusPercentage = (short)stateSkill.BuffValueY1;
					break;
				case 1036:
					EngineerGunShieldBonusPercentage = (short)stateSkill.BuffValueY1;
					break;
				case 1037:
					ShieldRecoverBonusPercentage = (short)stateSkill.BuffValueY1;
					ShieldRecoverBonusRange = stateSkill.Range;
					break;
				case 1044:
					AttackRateBonusPercentage = (short)stateSkill.BuffValueY1;
					AttackRateBonusRange = stateSkill.Range;
					break;
				case 1047:
					DamageBonusPercentage = (short)stateSkill.BuffValueY1;
					DamageBonusRange = stateSkill.Range;
					break;
				case 1041:
					EngineerGunAttackRateBonusPercentage = (short)stateSkill.BuffValueY1;
					break;
				case 1042:
					EngineerGunDamageBonusPercentage = (short)stateSkill.BuffValueY1;
					break;
				case 1046:
					Debug.Log("IsUpgrade = " + IsUpgrade);
					IsUpgrade = true;
					UpgradeDamageBonus = (short)stateSkill.BuffValueY1;
					break;
				}
			}
		}
		base.MaxHp = Mathf.CeilToInt((float)base.MaxHp * (1f + (float)EngineerGunHPBonusPercentage * 0.01f));
		Hp = base.MaxHp;
		base.MaxShield = Mathf.CeilToInt((float)base.MaxShield * (1f + (float)EngineerGunShieldBonusPercentage * 0.01f));
		Shield = base.MaxShield;
		mFireDamage = (10 + GameApp.GetInstance().GetUserState().GetCharLevel() * 5) * 2;
		mCannonDamage = mFireDamage * 12;
		mFireDamage = Mathf.CeilToInt((float)mFireDamage * (1f + (float)EngineerGunDamageBonusPercentage * 0.01f));
		mCannonDamage = Mathf.CeilToInt((float)mCannonDamage * (1f + (float)EngineerGunDamageBonusPercentage * 0.01f));
		BulletRecvoerSkillAdded = false;
	}

	public override void DoInit()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_INIT, WrapMode.ClampForever);
		if (AnimationPlayed(AnimationString.ENGINEER_GUN_INIT, 1f))
		{
			EndInit();
			StartIdle();
		}
	}

	protected override void EndInit()
	{
	}

	protected override void StartIdle()
	{
		SetState(ControllableUnit.IDLE_STATE);
		StopAnimation();
		mLastIdleTime = Time.time;
		mShieldRecoverTimer.SetTimer(1f, true);
		mMoveSpeedTimer.SetTimer(1f, true);
		mAttackRateTimer.SetTimer(1f, true);
		mDamageBonusTimer.SetTimer(1f, true);
	}

	public void ChangeTargetUnit(GameUnit target)
	{
		if (target != null)
		{
			mTargetGameUnit = target;
			if (mTargetGameUnit.GetTransform() != null)
			{
				mTargetPoint = mTargetGameUnit.GetTransform().position;
			}
		}
	}

	public void ChangeTargetUnit(byte pointId, byte enemyId)
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameScene().GetEnemyByID("E_" + pointId + "_" + enemyId);
		ChangeTargetUnit(enemyByID);
	}

	private void GetTargetGameUnit()
	{
		bool flag = false;
		int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.ENEMY_HEAD) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.SUMMONED);
		if (mTargetGameUnit == null || !mTargetGameUnit.InPlayingState())
		{
			flag = true;
		}
		else
		{
			float magnitude = (entityTransform.position - mTargetGameUnit.GetTransform().position).magnitude;
			if (magnitude > mAttackDistance)
			{
				flag = true;
			}
			else
			{
				Vector3 direction = mTargetGameUnit.GetTransform().position - entityTransform.position;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), direction);
				mRaycastHit = default(RaycastHit);
				flag = !Physics.Raycast(mRay, out mRaycastHit, magnitude, layerMask) || ((mRaycastHit.collider.gameObject.layer != PhysicsLayer.ENEMY && mRaycastHit.collider.gameObject.layer != PhysicsLayer.ENEMY_HEAD && mRaycastHit.collider.gameObject.layer != PhysicsLayer.REMOTE_PLAYER && mRaycastHit.collider.gameObject.layer != PhysicsLayer.SUMMONED) ? true : false);
			}
		}
		if (!flag)
		{
			return;
		}
		mTargetGameUnit = null;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
			{
				if (remotePlayer == null || remotePlayer.IsSameTeam(mOwnerPlayer) || !remotePlayer.InPlayingState())
				{
					continue;
				}
				float magnitude2 = (entityTransform.position - remotePlayer.GetTransform().position).magnitude;
				if (magnitude2 < mAttackDistance)
				{
					Vector3 direction2 = remotePlayer.GetTransform().position - entityTransform.position;
					mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), direction2);
					mRaycastHit = default(RaycastHit);
					if (Physics.Raycast(mRay, out mRaycastHit, magnitude2, layerMask) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)
					{
						mTargetGameUnit = remotePlayer;
						break;
					}
				}
			}
			if (mTargetGameUnit != null)
			{
				return;
			}
			{
				foreach (RemotePlayer remotePlayer2 in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
				{
					if (remotePlayer2 == null || remotePlayer2.IsSameTeam(mOwnerPlayer))
					{
						continue;
					}
					foreach (KeyValuePair<string, SummonedItem> summoned in remotePlayer2.GetSummonedList())
					{
						if (summoned.Value == null || !summoned.Value.InPlayingState())
						{
							continue;
						}
						float magnitude3 = (entityTransform.position - summoned.Value.GetTransform().position).magnitude;
						if (magnitude3 < mAttackDistance)
						{
							Vector3 direction3 = summoned.Value.GetTransform().position - entityTransform.position;
							mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), direction3);
							mRaycastHit = default(RaycastHit);
							if (Physics.Raycast(mRay, out mRaycastHit, magnitude3, layerMask) && mRaycastHit.collider.gameObject.layer == PhysicsLayer.SUMMONED)
							{
								mTargetGameUnit = summoned.Value;
								break;
							}
						}
					}
				}
				return;
			}
		}
		Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
		foreach (KeyValuePair<string, Enemy> item in enemies)
		{
			if (!item.Value.InPlayingState())
			{
				continue;
			}
			float magnitude4 = (entityTransform.position - item.Value.GetTransform().position).magnitude;
			if (magnitude4 < mAttackDistance)
			{
				Vector3 direction4 = item.Value.GetTransform().position - entityTransform.position;
				mRay = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), direction4);
				mRaycastHit = default(RaycastHit);
				if (Physics.Raycast(mRay, out mRaycastHit, magnitude4, layerMask) && (mRaycastHit.collider.gameObject.layer == PhysicsLayer.ENEMY || mRaycastHit.collider.gameObject.layer == PhysicsLayer.ENEMY_HEAD))
				{
					mTargetGameUnit = item.Value;
					break;
				}
			}
		}
	}

	public override void DoIdle()
	{
		DoCheckLogic();
		if (!base.IsMaster)
		{
			return;
		}
		ControllableStateConst controllableStateConst = ControllableStateConst.NO_STATE;
		bool flag = false;
		if (IsUpgrade && mDistanceCheckTimer.Ready())
		{
			mDistanceCheckTimer.Do();
			if ((mOwnerPlayer.GetTransform().position - entityTransform.position).sqrMagnitude > mStartRunDistance * mStartRunDistance)
			{
				flag = true;
			}
		}
		if (flag)
		{
			EndIdle();
			StartEngineerGunRun();
			controllableStateConst = ControllableStateConst.ENGINEER_GUN_RUN;
		}
		else if (Time.time - mLastIdleTime > mMaxIdleTime)
		{
			GetTargetGameUnit();
			if (mTargetGameUnit != null)
			{
				EndIdle();
				int num = Random.Range(0, 100);
				if (!IsUpgrade)
				{
					num = 0;
				}
				if (num < mFireProbability)
				{
					StartEngineerGunFire();
					controllableStateConst = ControllableStateConst.ENGINEER_GUN_FIRE;
				}
				else
				{
					StartEngineerGunCannon();
					controllableStateConst = ControllableStateConst.ENGINEER_GUN_CANNON;
				}
			}
			else
			{
				mLastIdleTime = Time.time;
			}
		}
		if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || controllableStateConst == ControllableStateConst.NO_STATE)
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			Enemy enemy = mTargetGameUnit as Enemy;
			if (enemy != null)
			{
				ControllableItemChangeTargetRequest request = new ControllableItemChangeTargetRequest(base.ControllableType, base.ID, enemy.PointID, enemy.EnemyID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			bool isTargetPlayer = false;
			int id = 0;
			if (mTargetGameUnit is RemotePlayer)
			{
				RemotePlayer remotePlayer = mTargetGameUnit as RemotePlayer;
				isTargetPlayer = true;
				id = remotePlayer.GetUserID();
			}
			else if (mTargetGameUnit is SummonedItem)
			{
				SummonedItem summonedItem = mTargetGameUnit as SummonedItem;
				isTargetPlayer = false;
				id = summonedItem.ID;
			}
			ControllableItemChangePVPTargetRequest request2 = new ControllableItemChangePVPTargetRequest(isTargetPlayer, id, base.ControllableType, base.ID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
		ControllableItemStateRequest request3 = new ControllableItemStateRequest(base.ControllableType, base.ID, controllableStateConst, entityTransform.position);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
	}

	protected override void EndIdle()
	{
	}

	public override void StartDead()
	{
		SetState(ControllableUnit.DEAD_STATE);
		StopNavMesh();
		GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
		tempWeapon = null;
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		characterSkillManager.RemoveSkillByID(10430);
		BulletRecvoerSkillAdded = false;
		if (base.IsMaster)
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Take_a_rest, AchievementTrigger.Type.Stop);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
	}

	public override void DoDead()
	{
		gameScene.AddToDeletSummoned(this);
	}

	protected override void EndDead()
	{
	}

	public override void StartDisappear()
	{
		SetState(ControllableUnit.DISAPPEAR_STATE);
		StopNavMesh();
		GameObject original = Resources.Load("RPG_effect/RPG_engineer_gun_transform_001") as GameObject;
		Object.Instantiate(original, entityTransform.position + Vector3.up * 0.5f, Quaternion.identity);
		tempWeapon = null;
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		characterSkillManager.RemoveSkillByID(10430);
		BulletRecvoerSkillAdded = false;
		if (base.IsMaster)
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Take_a_rest, AchievementTrigger.Type.Stop);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
	}

	public override void DoDisappear()
	{
		gameScene.AddToDeletSummoned(this);
	}

	protected override void EndDisappear()
	{
	}

	public void StartEngineerGunRun()
	{
		SetState(ENGINEER_GUN_RUN_STATE);
		mLastIdleTime = Time.time;
		SetNavMeshForRun();
		PlaySound("RPG_Audio/Summoned/turret/turret_move");
	}

	public void DoEngineerGunRun()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_RUN, WrapMode.Loop);
		DoCheckLogic();
		if (Time.time - mLastUpdateNavMeshTime > 0.5f)
		{
			mLastUpdateNavMeshTime = Time.time;
			if (null != mNavMeshAgent)
			{
				mNavMeshAgent.SetDestination(mOwnerPlayer.GetTransform().position + Vector3.up * 0.1f);
			}
		}
		if ((mOwnerPlayer.GetTransform().position - entityTransform.position).sqrMagnitude < mStopRunDistance * mStopRunDistance)
		{
			EndEngineerGunRun();
			StartIdle();
			SendTransform();
		}
		else
		{
			if (!base.IsMaster || !(Time.time - mLastIdleTime > mMaxIdleTime))
			{
				return;
			}
			GetTargetGameUnit();
			if (mTargetGameUnit != null)
			{
				ControllableStateConst controllableStateConst = ControllableStateConst.NO_STATE;
				EndEngineerGunRun();
				int num = Random.Range(0, 100);
				if (!IsUpgrade)
				{
					num = 0;
				}
				if (num < mFireProbability)
				{
					StartEngineerGunRunFire();
					controllableStateConst = ControllableStateConst.ENGINEER_GUN_RUN_FIRE;
				}
				else
				{
					StartEngineerGunRunCannon();
					controllableStateConst = ControllableStateConst.ENGINEER_GUN_RUN_CANNON;
				}
				if (!GameApp.GetInstance().GetGameMode().IsMultiPlayer() || controllableStateConst == ControllableStateConst.NO_STATE)
				{
					return;
				}
				if (GameApp.GetInstance().GetGameMode().IsCoopMode())
				{
					Enemy enemy = mTargetGameUnit as Enemy;
					if (enemy != null)
					{
						ControllableItemChangeTargetRequest request = new ControllableItemChangeTargetRequest(base.ControllableType, base.ID, enemy.PointID, enemy.EnemyID);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
				else if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					bool isTargetPlayer = false;
					int id = 0;
					if (mTargetGameUnit is RemotePlayer)
					{
						RemotePlayer remotePlayer = mTargetGameUnit as RemotePlayer;
						isTargetPlayer = true;
						id = remotePlayer.GetUserID();
					}
					else if (mTargetGameUnit is SummonedItem)
					{
						SummonedItem summonedItem = mTargetGameUnit as SummonedItem;
						isTargetPlayer = false;
						id = summonedItem.ID;
					}
					ControllableItemChangePVPTargetRequest request2 = new ControllableItemChangePVPTargetRequest(isTargetPlayer, id, base.ControllableType, base.ID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
				ControllableItemStateRequest request3 = new ControllableItemStateRequest(base.ControllableType, base.ID, controllableStateConst, entityTransform.position);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
			}
			else
			{
				mLastIdleTime = Time.time;
			}
		}
	}

	protected void EndEngineerGunRun()
	{
		StopNavMesh();
	}

	public void StartEngineerGunFire()
	{
		SetState(ENGINEER_GUN_FIRE_STATE);
		mCurrentBulletCount = 0;
		mIsShoot = false;
	}

	public void DoEngineerGunFire()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_FIRE, WrapMode.Loop, mFireAnimationSpeed);
		UpdateTargetPosition();
		Fire(AnimationString.ENGINEER_GUN_FIRE);
		DoCheckLogic();
		if (mCurrentBulletCount >= mMaxBulletCount)
		{
			EndEngineerGunFire();
			StartIdle();
		}
	}

	protected void EndEngineerGunFire()
	{
		mLeftSpark.SetActive(false);
		mRightSpark.SetActive(false);
	}

	public void StartEngineerGunCannon()
	{
		SetState(ENGINEER_GUN_CANNON_STATE);
		mCurrentCannonCount = 0;
		UpdateTargetPosition();
		CannonShoot();
	}

	public void DoEngineerGunCannon()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_CANNON, WrapMode.ClampForever, mCannonAnimationSpeed);
		DoCheckLogic();
		if (mCurrentCannonCount >= mMaxCannonCount && AnimationPlayed(AnimationString.ENGINEER_GUN_CANNON, 1f))
		{
			EndEngineerGunCannon();
			StartIdle();
		}
		else if (animation[AnimationString.ENGINEER_GUN_CANNON].time > animation[AnimationString.ENGINEER_GUN_CANNON].clip.length)
		{
			animation[AnimationString.ENGINEER_GUN_CANNON].time -= animation[AnimationString.ENGINEER_GUN_CANNON].clip.length;
			CannonShoot();
		}
	}

	protected void EndEngineerGunCannon()
	{
	}

	public void StartEngineerGunRunFire()
	{
		SetState(ENGINEER_GUN_RUN_FIRE_STATE);
		SetNavMeshForRun();
		mCurrentBulletCount = 0;
		mIsShoot = false;
		PlaySound("RPG_Audio/Summoned/turret/turret_move");
	}

	public void DoEngineerGunRunFire()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_RUN_FIRE, WrapMode.Loop);
		UpdateTargetPosition();
		DoCheckLogic();
		Fire(AnimationString.ENGINEER_GUN_RUN_FIRE);
		if (mCurrentBulletCount >= mMaxBulletCount)
		{
			EndEngineerGunRunFire();
			StartEngineerGunRun();
		}
	}

	protected void EndEngineerGunRunFire()
	{
		StopNavMesh();
		mLeftSpark.SetActive(false);
		mRightSpark.SetActive(false);
	}

	public void StartEngineerGunRunCannon()
	{
		SetState(ENGINEER_GUN_RUN_CANNON_STATE);
		SetNavMeshForRun();
		UpdateTargetPosition();
		mCurrentCannonCount = 0;
		CannonShoot();
		PlaySound("RPG_Audio/Summoned/turret/turret_move");
	}

	public void DoEngineerGunRunCannon()
	{
		PlayAnimation(AnimationString.ENGINEER_GUN_RUN_CANNON, WrapMode.ClampForever, mCannonAnimationSpeed);
		DoCheckLogic();
		if (mCurrentCannonCount >= mMaxCannonCount && AnimationPlayed(AnimationString.ENGINEER_GUN_CANNON, 1f))
		{
			EndEngineerGunCannon();
			StartIdle();
		}
		else if (animation[AnimationString.ENGINEER_GUN_RUN_CANNON].time > animation[AnimationString.ENGINEER_GUN_RUN_CANNON].clip.length)
		{
			animation[AnimationString.ENGINEER_GUN_RUN_CANNON].time -= animation[AnimationString.ENGINEER_GUN_RUN_CANNON].clip.length;
			CannonShoot();
		}
	}

	protected void EndEngineerGunRunCannon()
	{
		StopNavMesh();
	}

	protected void DoCheckLogic()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		float num = Vector3.Distance(localPlayer.GetTransform().position, GetTransform().position);
		if (IsSameTeam() && ShieldRecoverBonusPercentage > 0 && num < (float)ShieldRecoverBonusRange)
		{
			if (mShieldRecoverTimer.Ready())
			{
				mShieldRecoverTimer.Do();
				CharacterStateSkill characterStateSkill = new CharacterStateSkill();
				characterStateSkill.skillID = (short)(ShieldRecoverBonusRange * 10);
				characterStateSkill.IsPermanent = false;
				characterStateSkill.Duration = 1f;
				characterStateSkill.ModifierOfBuff1 = BuffModifier.VALUE;
				characterStateSkill.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeShieldRecoverySpeed;
				characterStateSkill.BuffValueY1 = ShieldRecoverBonusPercentage;
				characterStateSkill.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill.TargetTypes = new List<SkillTargetType>();
				characterStateSkill.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterStateSkill.IconName = ShieldRecoverIcon;
				characterSkillManager.AddSkill(characterStateSkill);
				characterStateSkill.StartBuff();
			}
		}
		else
		{
			mShieldRecoverTimer.SetTimer(1f, false);
		}
		if (IsSameTeam() && MoveSpeedBonusPercentage > 0 && num < (float)MoveSpeedBonusRange)
		{
			if (mMoveSpeedTimer.Ready())
			{
				mMoveSpeedTimer.Do();
				CharacterStateSkill characterStateSkill2 = new CharacterStateSkill();
				characterStateSkill2.skillID = 10380;
				characterStateSkill2.IsPermanent = false;
				characterStateSkill2.Duration = 10f;
				characterStateSkill2.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill2.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
				characterStateSkill2.BuffValueY1 = MoveSpeedBonusPercentage;
				characterStateSkill2.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill2.TargetTypes = new List<SkillTargetType>();
				characterStateSkill2.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterStateSkill2.IconName = MoveSpeedIcon;
				characterSkillManager.AddSkill(characterStateSkill2);
				characterStateSkill2.StartBuff();
			}
		}
		else
		{
			mMoveSpeedTimer.SetTimer(1f, false);
		}
		if (IsSameTeam() && BulletRecoverCount > 0 && num < (float)BulletRecoverRange)
		{
			if (!BulletRecvoerSkillAdded)
			{
				CharacterBulletRecoverByTimeSkill characterBulletRecoverByTimeSkill = new CharacterBulletRecoverByTimeSkill();
				characterBulletRecoverByTimeSkill.skillID = 10430;
				characterBulletRecoverByTimeSkill.STriggerType = SkillTriggerType.OnNearSummonedItem;
				characterBulletRecoverByTimeSkill.STriggerTypeSubValue = 0;
				characterBulletRecoverByTimeSkill.SEffectType = SkillEffectType.RecoverBullet;
				characterBulletRecoverByTimeSkill.CoolDownTime = 10f;
				characterBulletRecoverByTimeSkill.CoolDownTimeInit = characterBulletRecoverByTimeSkill.CoolDownTime;
				characterBulletRecoverByTimeSkill.EffectValueX = BulletRecoverCount;
				characterBulletRecoverByTimeSkill.EffectValueY = BulletRecoverTime;
				characterBulletRecoverByTimeSkill.TargetTypes = new List<SkillTargetType>();
				characterBulletRecoverByTimeSkill.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterSkillManager.AddSkill(characterBulletRecoverByTimeSkill);
				characterBulletRecoverByTimeSkill.ApplySkill(localPlayer);
				BulletRecvoerSkillAdded = true;
			}
		}
		else if (BulletRecvoerSkillAdded)
		{
			characterSkillManager.RemoveSkillByID(10430);
			BulletRecvoerSkillAdded = false;
		}
		if (IsSameTeam() && AttackRateBonusPercentage > 0 && num < 1044f)
		{
			if (mAttackRateTimer.Ready())
			{
				mAttackRateTimer.Do();
				CharacterStateSkill characterStateSkill3 = new CharacterStateSkill();
				characterStateSkill3.skillID = 10440;
				characterStateSkill3.IsPermanent = false;
				characterStateSkill3.Duration = 1f;
				characterStateSkill3.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill3.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeFireRate;
				characterStateSkill3.BuffValueY1 = AttackRateBonusPercentage;
				characterStateSkill3.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill3.TargetTypes = new List<SkillTargetType>();
				characterStateSkill3.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterStateSkill3.IconName = AttackRateIcon;
				characterSkillManager.AddSkill(characterStateSkill3);
				characterStateSkill3.StartBuff();
			}
		}
		else
		{
			mAttackRateTimer.SetTimer(1f, false);
		}
		if (IsSameTeam() && DamageBonusPercentage > 0 && num < (float)DamageBonusRange)
		{
			if (mDamageBonusTimer.Ready())
			{
				mDamageBonusTimer.Do();
				CharacterStateSkill characterStateSkill4 = new CharacterStateSkill();
				characterStateSkill4.skillID = 10470;
				characterStateSkill4.IsPermanent = false;
				characterStateSkill4.Duration = 1f;
				characterStateSkill4.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
				characterStateSkill4.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeDamage;
				characterStateSkill4.BuffValueY1 = DamageBonusPercentage;
				characterStateSkill4.FunctionType1 = BuffFunctionType.PropertyChange;
				characterStateSkill4.TargetTypes = new List<SkillTargetType>();
				characterStateSkill4.TargetTypes.Add(SkillTargetType.AllWeapon);
				characterStateSkill4.IconName = DamageBonusIcon;
				characterSkillManager.AddSkill(characterStateSkill4);
				characterStateSkill4.StartBuff();
			}
		}
		else
		{
			mDamageBonusTimer.SetTimer(1f, false);
		}
	}
}
