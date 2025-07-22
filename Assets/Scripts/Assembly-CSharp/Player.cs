using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameUnit
{
	public static PlayerState IDLE_STATE = new IdleState();

	public static PlayerState TALK_STATE = new TalkState();

	public static PlayerState ATTACK_STATE = new AttackState();

	public static PlayerState ATTACK_INTERVAL_STATE = new AttackIntervalState();

	public static PlayerState RELOAD_STATE = new ReloadState();

	public static PlayerState THROW_GRENADE_STATE = new ThrowGrenadeState();

	public static PlayerState MELEE_ATTACK_STATE = new MeleeAttackState();

	public static PlayerState THROW_GRENADE_SKILL_STATE = new ThrowGrenadeSkillState();

	public static PlayerState DEAD_STATE = new PlayerDeadState();

	public static PlayerState KNOCKED_STATE = new PlayerKnockedState();

	public static PlayerState WIN_STATE = new WinState();

	public static PlayerState LOSE_STATE = new PlayerLoseState();

	public static PlayerState WAIT_REBIRTH_STATE = new PlayerWaitRebirthState();

	public static PlayerState WAIT_VS_REBIRTH_STATE = new PlayerWaitVSRebirthState();

	public static PlayerState FALL_DOWN_STATE = new FallDownState();

	public static PlayerState FIRST_AID_STATE = new FirstAidState();

	public static PlayerState SWITCH_WEAPON_DOWN_STATE = new SwitchWeaponDownState();

	public static PlayerState SWITCH_WEAPON_ON_STATE = new SwitchWeaponOnState();

	public DyingState DYING_STATE;

	protected static float FALL_DOWN_DURATION = 0.7f;

	protected static float FALL_DOWN_OFFSET = -0.8f;

	protected int userID;

	protected byte seatID;

	protected Weapon weapon;

	protected Weapon weaponToChange;

	protected List<Weapon> weaponList = new List<Weapon>();

	protected UserState userState;

	protected int currentBagIndex;

	protected int monsterCash;

	protected int bonusCash;

	protected int pickupCash;

	protected int bossCash;

	protected int bossMithril;

	protected int exp;

	protected int instanceRespawnTimes;

	protected float teleportStartFadeTime = -1f;

	protected bool bStartFade;

	protected int initEnegy;

	protected int pickupEnegy;

	protected int maxCombo;

	protected float knockedSpeed;

	protected float knockStartTime;

	protected Transform mSpine;

	protected Transform mHead;

	protected Transform mWeapon;

	protected AimArm[] mArms;

	protected IK1JointAnalytic ikSolver = new IK1JointAnalytic();

	protected Collider mPlayerCollider;

	protected bool isSpecialWinIdle = true;

	protected bool winSpecialFinish = true;

	protected int lastAssistItemCount = -1;

	protected float mRebirthHealthPercentage;

	protected float mRebirthHealthPercentageInit;

	protected Timer rebirthTimer = new Timer();

	protected Timer deadAnimationTimer = new Timer();

	protected Timer winAnimationTimer = new Timer();

	protected Timer vsRebirthTimer = new Timer();

	protected Timer firstAidTimer = new Timer();

	protected Timer SavedTimer = new Timer();

	protected Timer fallDownTimer = new Timer();

	protected Timer unhurtTimer = new Timer();

	protected Timer lastBloodEffectTimer = new Timer();

	protected int runningPhase = -1;

	protected float velocityJumpY;

	protected float baseATK;

	protected CharacterSkillManager skillMgr = new CharacterSkillManager();

	public GameObject CurrentRespawnPoint;

	public short CurrentRespawnPointID;

	protected Dictionary<string, SummonedItem> summonedList = new Dictionary<string, SummonedItem>();

	protected Dictionary<string, SummonedItem> canHitList = new Dictionary<string, SummonedItem>();

	public Timer ThrowGrenadeTimer = new Timer();

	public Timer MeleeAttackTimer = new Timer();

	public GrenadeInfo HandGrenade;

	public GameObject Knife;

	public GameObject KnifeEffect;

	protected float ShieldRecoveryDelayInit;

	protected Timer footStepAudioTimer = new Timer();

	protected Timer onHitAudioTimer = new Timer();

	protected int hpRecoverValueByShield;

	protected int hpRecoverValueByShieldInit;

	protected int hpRecoverValueByChip;

	protected int hpRecoverValueByChipInit;

	protected int hpRecoverValueByGrenade;

	protected int hpRecoverValueByGrenadeInit;

	protected int bulletRecoverValueByChip;

	protected int bulletRecoverValueByChipInit;

	protected Dictionary<Enemy, Timer> mCheckHitTimers = new Dictionary<Enemy, Timer>();

	public GameObject ExtraShieldObject;

	public GameObject SpeedDownEffect;

	protected Transform NameSignTrans;

	protected Task openAimTask;

	protected bool hasGravity;

	protected float LastDamageImmunityRate;

	protected int LastDamageReduction;

	protected int LastElementResistanceFire;

	protected int LastElementResistanceShock;

	protected int LastElementResistanceCorrosive;

	protected int LastElementResistanceExplosion;

	public PlayerState State { get; set; }

	public bool InAimState { get; set; }

	public bool IsDeadJustNow { get; set; }

	public bool IsRoomMaster { get; set; }

	public bool IsPVPReady { get; set; }

	public InputController inputController { get; set; }

	public AimAssistController aimAssistController { get; set; }

	public CameraVibrateController cameraVibrateController { get; set; }

	public bool NeverGotHit { get; set; }

	public int Kills { get; set; }

	public bool JustMakeAShoot { get; set; }

	public float AngleV { get; set; }

	public float TargetAngleV { get; set; }

	public bool SendingRebirthRequest { get; set; }

	public TeamName Team { get; set; }

	public int TeammateId { get; set; }

	public byte CurrentCityID { get; set; }

	public byte CurrentSceneID { get; set; }

	public float ShieldRecoveryDelay { get; set; }

	public float DamageImmunityRateBonus { get; set; }

	public float CapacityToHealthPercentage { get; set; }

	public string FootStepAudio { get; set; }

	public float RebirthHealthPercentage
	{
		get
		{
			return mRebirthHealthPercentage;
		}
	}

	public int ExtraShield { get; set; }

	public int HpRecoverValueByShield
	{
		get
		{
			return hpRecoverValueByShield;
		}
	}

	public int HpRecoverValueByChip
	{
		get
		{
			return hpRecoverValueByChip;
		}
	}

	protected int HpRecoverValueByGrenade
	{
		get
		{
			return hpRecoverValueByGrenade;
		}
	}

	public int BulletRecoverValueByChip
	{
		get
		{
			return bulletRecoverValueByChip;
		}
	}

	public InputController InputController
	{
		get
		{
			return inputController;
		}
	}

	public AimAssistController AimAssistController
	{
		get
		{
			return aimAssistController;
		}
	}

	public CameraVibrateController CameraVibrateController
	{
		get
		{
			return cameraVibrateController;
		}
	}

	public UserState GetUserState()
	{
		return userState;
	}

	public Timer GetLastBloodEffectTimer()
	{
		return lastBloodEffectTimer;
	}

	public CharacterSkillManager GetCharacterSkillManager()
	{
		return skillMgr;
	}

	public Collider GetPlayerCollider()
	{
		return mPlayerCollider;
	}

	public int GetInitEnegy()
	{
		return initEnegy;
	}

	public int GetMaxCombo()
	{
		return maxCombo;
	}

	public int GetPickupEnegy()
	{
		return pickupEnegy;
	}

	public int GetMonsterCash()
	{
		return monsterCash;
	}

	public void SetMonsterCash(int cash)
	{
		monsterCash = cash;
	}

	public void AddMonsterCash(int cash)
	{
		monsterCash += cash;
	}

	public void SetBossCash(int cash)
	{
		bossCash = cash;
	}

	public int GetBossCash()
	{
		return bossCash;
	}

	public void SetBossMithril(int mithril)
	{
		bossMithril = mithril;
	}

	public int GetBossMithril()
	{
		return bossMithril;
	}

	public void SetPickupEnegy(int enegy)
	{
		pickupEnegy = enegy;
	}

	public int GetBounsCash()
	{
		return bonusCash;
	}

	public void SetBounsCash(int cash)
	{
		bonusCash = cash;
	}

	public void AddBounsCash(int cash)
	{
		bonusCash += cash;
	}

	public int GetPickupCash()
	{
		return pickupCash;
	}

	public void SetPickupCash(int cash)
	{
		pickupCash = cash;
	}

	public void AddPickupCash(int cash)
	{
		pickupCash += cash;
	}

	public int GetExp()
	{
		return exp;
	}

	public void SetExp(int exp)
	{
		this.exp = exp;
	}

	public int AddExp(int exp)
	{
		this.exp += exp;
		return userState.AddExp(exp);
	}

	public void AddSummoned(string summonedName, SummonedItem summoned)
	{
		if (!summonedList.ContainsKey(summonedName))
		{
			summonedList.Add(summonedName, summoned);
		}
	}

	public void RemoveSummoned(string summonedName)
	{
		if (summonedList.ContainsKey(summonedName))
		{
			summonedList.Remove(summonedName);
		}
	}

	public void ClearSummonedList()
	{
		if (IsLocal() && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			foreach (SummonedItem value in summonedList.Values)
			{
				ControllableItemDisappearRequest request = new ControllableItemDisappearRequest(value.ControllableType, value.ID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		summonedList.Clear();
	}

	public SummonedItem GetSummonedByName(string summonedName)
	{
		if (summonedList.ContainsKey(summonedName))
		{
			return summonedList[summonedName];
		}
		return null;
	}

	public void ClearSummoned()
	{
		foreach (SummonedItem value in summonedList.Values)
		{
			value.EndCurrentState();
			value.StartDisappear();
		}
		summonedList.Clear();
	}

	public Dictionary<string, SummonedItem> GetSummonedList()
	{
		canHitList.Clear();
		foreach (SummonedItem value in summonedList.Values)
		{
			if (value.SummonedType != ESummonedType.TRAPS)
			{
				canHitList.Add(value.Name, value);
			}
		}
		return canHitList;
	}

	public List<Weapon> GetWeaponList()
	{
		return weaponList;
	}

	public Weapon GetWeapon(int id)
	{
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i] != null && weaponList[i].GunID == id)
			{
				return weaponList[i];
			}
		}
		return null;
	}

	public string GetWeaponAnimationSuffix()
	{
		return weapon.GetAnimationSuffix();
	}

	public string GetWeaponReloadAnimationSuffix()
	{
		return weapon.GetReloadAnimationSuffix();
	}

	public virtual bool IsLocal()
	{
		return false;
	}

	public void SetUserID(int userID)
	{
		this.userID = userID;
	}

	public int GetUserID()
	{
		return userID;
	}

	public void SetSeatID(byte seatID)
	{
		this.seatID = seatID;
	}

	public byte GetSeatID()
	{
		return seatID;
	}

	public float GetKnockedTimeDuration()
	{
		return Time.time - knockStartTime;
	}

	public bool KnockedComplete()
	{
		return GetKnockedTimeDuration() > 0.5f;
	}

	public virtual void SetState(PlayerState state)
	{
		State = state;
	}

	public Weapon GetWeapon()
	{
		return weapon;
	}

	public Weapon GetWeaponToChange()
	{
		return weaponToChange;
	}

	protected void SetWeaponToChange(Weapon w)
	{
		weaponToChange = w;
	}

	public override bool InPlayingState()
	{
		if (State == DEAD_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE)
		{
			return false;
		}
		return true;
	}

	public bool CanUseGravity()
	{
		return hasGravity;
	}

	public bool InDyingState()
	{
		return DYING_STATE.InDyingState;
	}

	public bool InFallDownState()
	{
		return State == FALL_DOWN_STATE;
	}

	public bool CanReload()
	{
		if (State == RELOAD_STATE || State == FALL_DOWN_STATE || State == DEAD_STATE || State == FIRST_AID_STATE)
		{
			return false;
		}
		return true;
	}

	public bool CanRecoverShieldState()
	{
		if (State == DEAD_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE || InDyingState() || State == FALL_DOWN_STATE || (State == KNOCKED_STATE && Hp <= 0))
		{
			return false;
		}
		return true;
	}

	public bool CanRecoverHPState()
	{
		if (State == DEAD_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE || InDyingState() || State == FALL_DOWN_STATE || (State == KNOCKED_STATE && Hp <= 0))
		{
			return false;
		}
		return true;
	}

	public bool CanRecoverBulletState()
	{
		if (State == DEAD_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE || State == FALL_DOWN_STATE)
		{
			return false;
		}
		return true;
	}

	public bool CanAim()
	{
		if (State == DEAD_STATE || State == FALL_DOWN_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE || State == RELOAD_STATE || State == THROW_GRENADE_STATE || State == MELEE_ATTACK_STATE || State == THROW_GRENADE_SKILL_STATE)
		{
			return false;
		}
		if (DYING_STATE.InDyingState)
		{
			return false;
		}
		return true;
	}

	public bool CanFirstAidTeammate()
	{
		if (State == DEAD_STATE || State == FALL_DOWN_STATE || State == WAIT_REBIRTH_STATE || State == WAIT_VS_REBIRTH_STATE || State == WIN_STATE || State == LOSE_STATE)
		{
			return false;
		}
		if (DYING_STATE.InDyingState)
		{
			return false;
		}
		return true;
	}

	public bool CanTeleport()
	{
		if (State == DEAD_STATE || State == FALL_DOWN_STATE || DYING_STATE.InDyingState)
		{
			return false;
		}
		return true;
	}

	public bool IsSameTeam(Player player)
	{
		if (GameApp.GetInstance().GetGameMode().IsTeamMode())
		{
			return Team == player.Team;
		}
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			return true;
		}
		return false;
	}

	public bool InSameScene(Player player)
	{
		return userState.GetCurrentCityID() == player.CurrentCityID && gameWorld.CurrentSceneID == player.CurrentSceneID;
	}

	public virtual void Init()
	{
		base.GameUnitType = EGameUnitType.PLAYER;
		SetObject(AvatarBuilder.GetInstance().RebuildAvatar(userState, this));
		UnityEngine.Object.DontDestroyOnLoad(entityObject);
		GameObject gameObject = null;
		gameObject = ((!IsLocal()) ? (Resources.Load("WeaponL/knife01_l") as GameObject) : (Resources.Load("Weapon/knife01") as GameObject));
		Transform transform = GetTransform().Find(BoneName.Knife);
		if (transform != null && gameObject != null)
		{
			Knife = UnityEngine.Object.Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
			Knife.transform.parent = transform;
			Knife.SetActive(false);
		}
		gameWorld = GameApp.GetInstance().GetGameWorld();
		GameObject gameObject2 = GameObject.FindGameObjectWithTag(TagName.RESPAWN);
		if (gameObject2 != null)
		{
			entityObject.transform.position = gameObject2.transform.position;
		}
		entityObject.name = "Player";
		animationObject = entityObject;
		Transform transform2 = entityObject.transform.Find("Entity");
		if (transform2 != null)
		{
			animationObject = transform2.gameObject;
		}
		if (IsLocal())
		{
			RefreshWeaponListFromItemInfo();
			RefreshShieldFromItemInfo();
		}
		hasGravity = true;
		monsterCash = 0;
		bonusCash = 0;
		pickupCash = 0;
		exp = 0;
		bossCash = 0;
		bossMithril = 0;
		State = IDLE_STATE;
		inputController = new InputController();
		aimAssistController = new AimAssistController();
		cameraVibrateController = new CameraVibrateController();
		skillMgr.LoadCharacterSkills(this);
		ResetUnitStatus();
		if (IsLocal())
		{
			Hp = 0;
			Shield = 0;
			userState.LevelUp();
		}
		ApplyBuffToUnitStatus();
		State = IDLE_STATE;
		DYING_STATE = new DyingState(this);
		initEnegy = userState.Enegy;
		NeverGotHit = true;
		JustMakeAShoot = false;
		SendingRebirthRequest = false;
		unhurtTimer.SetTimer(4f, true);
		lastBloodEffectTimer.SetTimer(0.3f, false);
		mWeapon = entityTransform.Find(BoneName.Weapon);
		Transform transform3 = entityTransform.Find("Entity");
		if (!IsLocal())
		{
			mSpine = transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
			mHead = transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head");
			mArms = new AimArm[2]
			{
				new AimArm(transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm"), transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm"), transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand")),
				new AimArm(transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm"), transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm"), transform3.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand"))
			};
		}
		Transform transform4 = entityTransform.Find("collider");
		if (null != transform4)
		{
			mPlayerCollider = transform4.GetComponent<Collider>();
		}
		else
		{
			Debug.Log("XXX playerColliderTransform can not be found");
		}
		mRebirthHealthPercentageInit = 0.25f;
		mRebirthHealthPercentage = mRebirthHealthPercentageInit;
		FootStepAudio = "RPG_Audio/Player/player_footstep01_c0_i0_i1";
		footStepAudioTimer.SetTimer(0.4f, true);
		onHitAudioTimer.SetTimer(5f, true);
		MeleeAttackTimer.SetTimer(1f, true);
		ThrowGrenadeTimer.SetTimer(2f, true);
		ExtraShield = 0;
		hpRecoverValueByShield = 0;
		hpRecoverValueByShieldInit = 0;
		hpRecoverValueByChip = 0;
		hpRecoverValueByChipInit = 0;
		hpRecoverValueByGrenade = 0;
		hpRecoverValueByGrenadeInit = 0;
		bulletRecoverValueByChip = 0;
		bulletRecoverValueByChipInit = 0;
	}

	public void AddEnemyCheckHitTimer(Enemy enemy)
	{
		if (!mCheckHitTimers.ContainsKey(enemy))
		{
			Timer timer = new Timer();
			timer.SetTimer(1f, false);
			mCheckHitTimers.Add(enemy, timer);
			Dictionary<string, SummonedItem> dictionary = GetSummonedList();
			{
				foreach (SummonedItem value in dictionary.Values)
				{
					value.AddEnemyCheckHitTimer(enemy);
				}
				return;
			}
		}
		Debug.Log(string.Concat("XXX: Add Duplicated enemy: ", enemy, "is added to local player."));
	}

	public bool CheckHitTimerReady(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			return mCheckHitTimers[enemy].Ready();
		}
		Debug.Log(string.Concat("XXX: Check enemy: ", enemy, "is not registed in localplayer."));
		return false;
	}

	public void RemoveEnemyCheckHitTimer(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			mCheckHitTimers.Remove(enemy);
			Dictionary<string, SummonedItem> dictionary = GetSummonedList();
			{
				foreach (SummonedItem value in dictionary.Values)
				{
					value.RemoveEnemyCheckHitTimer(enemy);
				}
				return;
			}
		}
		Debug.Log(string.Concat("XXX: Remove enemy: ", enemy, "is not registed in localplayer."));
	}

	public void ResetCheckHitTimer(Enemy enemy)
	{
		if (mCheckHitTimers.ContainsKey(enemy))
		{
			mCheckHitTimers[enemy].Do();
		}
		else
		{
			Debug.Log(string.Concat("XXX: Reset enemy: ", enemy, "is not registed in localplayer."));
		}
	}

	public void LevelUp(int level)
	{
		if (IsLocal())
		{
			BasicMaxHp = 2 * (100 + level * 20);
			base.MaxHp = BasicMaxHp + detailedProperties.HpBonus;
			base.MaxHp = (int)Math.Ceiling((float)base.MaxHp * (1f + detailedProperties.HpPercentageBonus));
			BasicSpeed = 8.25f;
			BasicMeleeATK = 2 * (10 * level + 20);
		}
	}

	public void ResetUnitStatus()
	{
		if (IsLocal())
		{
			base.MaxHp = 0;
			BasicMaxHp = 0;
			base.Speed = 0f;
			BasicSpeed = 0f;
			BasicDyingTimeLength = 10f;
			hpRecoverValueByShield = hpRecoverValueByShieldInit;
			hpRecoverValueByChip = hpRecoverValueByChipInit;
			hpRecoverValueByGrenade = hpRecoverValueByGrenadeInit;
			bulletRecoverValueByChip = bulletRecoverValueByChipInit;
		}
	}

	public void ApplyBuffToUnitStatus()
	{
		if (IsLocal())
		{
			detailedProperties.Reset();
			skillMgr.ApplyAllStateSkills(this);
			Apply_Shield_Chip_Grenade_States();
			float num = 1f;
			if (GameApp.GetInstance().GetUserState().ItemInfoData.IsShieldEquiped)
			{
				num = ItemShield.HpEnhancePara[(int)GameApp.GetInstance().GetUserState().ItemInfoData.Shield.baseItem.Quality];
			}
			base.MaxHp = Mathf.CeilToInt((float)BasicMaxHp * num) + detailedProperties.HpBonus;
			base.MaxHp = (int)Math.Ceiling((float)base.MaxHp * (1f + detailedProperties.HpPercentageBonus));
			if (Hp > base.MaxHp)
			{
				Hp = base.MaxHp;
			}
			weapon.DamageBonus(detailedProperties.DamageBonusAll, detailedProperties.DamageBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.AttackFrequencyBonus(detailedProperties.AttackIntervalPercentageBonusAll, detailedProperties.AttackIntervalPercentageBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.ExplosionRangeBonus(detailedProperties.ExplosionRangeBonus);
			weapon.AccuracyBonus(detailedProperties.AccuracyBonusAll, detailedProperties.AccuracyBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.RecoilBonus(detailedProperties.RecoilBonusAll, detailedProperties.RecoilBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.GunCapacityBonus(detailedProperties.MagsBonusAll, detailedProperties.MagsBonus[(int)(weapon.GetWeaponType() - 1)], detailedProperties.MagsBonusValueAll, detailedProperties.MagsBonusValue[(int)(weapon.GetWeaponType() - 1)]);
			weapon.ReloadTimeBonus(detailedProperties.ReloadTimeBonusAll, detailedProperties.ReloadTimeBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.CriticalRateBonus(detailedProperties.CriticalRateBonusAll, detailedProperties.CriticalRateBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.CriticalDamageBonus(detailedProperties.CriticalDamageBonusAll, detailedProperties.CriticalDamageBonus[(int)(weapon.GetWeaponType() - 1)]);
			weapon.PenetrationBonus(detailedProperties.PenetrationBonusAll, detailedProperties.PenetrationBonus[(int)(weapon.GetWeaponType() - 1)]);
			if (HandGrenade != null)
			{
				HandGrenade.Damage = Mathf.CeilToInt((float)HandGrenade.DamageInit * (1f + detailedProperties.DamageBonusAll + detailedProperties.DamageBonus[7]));
				HandGrenade.ExplosionRange = HandGrenade.ExplosionRangeInit * (1f + detailedProperties.ExplosionRangeBonus);
			}
			base.MeleeATK = Mathf.CeilToInt((float)BasicMeleeATK * weapon.MeleeDamage * (1f + detailedProperties.MeleeDamageBonus));
			if (State == ATTACK_STATE || State == THROW_GRENADE_STATE || State == THROW_GRENADE_SKILL_STATE)
			{
				base.Speed = Mathf.Clamp(BasicSpeed * (1f + detailedProperties.SpeedBonus) * 0.7f, 0.7f, 15f);
			}
			else
			{
				base.Speed = Mathf.Clamp(BasicSpeed * (1f + detailedProperties.SpeedBonus), 1f, 15f);
			}
			base.MaxShield = BasicMaxShield + detailedProperties.MaxShieldBonus;
			base.MaxShield = (int)((float)base.MaxShield * (1f + detailedProperties.MaxShieldPercentageBonus));
			if (Shield > base.MaxShield)
			{
				Shield = base.MaxShield;
			}
			base.ShieldRecovery = BasicShieldRecovery + detailedProperties.ShieldRecoveryBonus;
			base.ShieldRecovery = (int)((float)base.ShieldRecovery * (1f + detailedProperties.ShieldRecoveryPercentageBonus));
			ShieldRecoveryDelay = ShieldRecoveryDelayInit - detailedProperties.ShieldRecoverDelayBonus;
			ShieldRecoveryDelay *= 1f - detailedProperties.ShieldRecoverDelayBonusPercentage;
			IncreaseElementResistance(ElementType.Fire, detailedProperties.ElementsResistanceBonus[0]);
			IncreaseElementResistance(ElementType.Shock, detailedProperties.ElementsResistanceBonus[1]);
			IncreaseElementResistance(ElementType.Corrosive, detailedProperties.ElementsResistanceBonus[2]);
			base.DamageReduction = BasicDamageReduction + detailedProperties.DamageReductionBonus;
			base.ExplosionDamageReduction = BasicExplosionRduction + detailedProperties.ExplosionDamageReductionBonus;
			DamageImmunityRateBonus = detailedProperties.DamageImmunityRateBonus;
			CapacityToHealthPercentage = detailedProperties.CapacitToHealthBonus;
			base.DamageToHealthPercentage = BasicDamageToHealthPercentage + detailedProperties.DamageToHealthBonus;
			base.DyingTimeLength = BasicDyingTimeLength * (1f + detailedProperties.DeathTimeBonus);
			if (detailedProperties.RebirthHealthBonus > mRebirthHealthPercentageInit)
			{
				mRebirthHealthPercentage = detailedProperties.RebirthHealthBonus;
			}
			else
			{
				mRebirthHealthPercentage = mRebirthHealthPercentageInit;
			}
			base.DropRate = BasicDropRate * (1f + detailedProperties.DropRateBonus);
			if (LastDamageImmunityRate != DamageImmunityRateBonus)
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.DamageImmunityRate);
			}
			if (LastDamageReduction != base.DamageReduction)
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.DamageReduction);
			}
			if (LastElementResistanceFire != GetElementResistance(ElementType.Fire))
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.ElementResistanceFire);
			}
			if (LastElementResistanceShock != GetElementResistance(ElementType.Shock))
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.ElementResistanceShock);
			}
			if (LastElementResistanceCorrosive != GetElementResistance(ElementType.Corrosive))
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.ElementResistanceCorrosive);
			}
			if (LastElementResistanceExplosion != base.ExplosionDamageReduction)
			{
				(this as LocalPlayer).UpdateDamagePara(LocalPlayer.DamagePara.ElementResistanceExplosion);
			}
			LastDamageImmunityRate = DamageImmunityRateBonus;
			LastDamageReduction = base.DamageReduction;
			LastElementResistanceFire = GetElementResistance(ElementType.Fire);
			LastElementResistanceShock = GetElementResistance(ElementType.Shock);
			LastElementResistanceCorrosive = GetElementResistance(ElementType.Corrosive);
			LastElementResistanceExplosion = base.ExplosionDamageReduction;
		}
	}

	public virtual void Loop(float deltaTime)
	{
		ResetUnitStatus();
		LevelUp(userState.GetCharLevel());
		ApplyBuffToUnitStatus();
	}

	public void CreateDeadBlood()
	{
		if (GameApp.GetInstance().GetGlobalState().GetBloodSpraying())
		{
			GameObject original = Resources.Load("Effect/update_effect/RPG_blood_big") as GameObject;
			GameObject gameObject = null;
			int num = UnityEngine.Random.Range(0, 100);
			FloorInfo ground = GetGround();
			int num2 = num % 3 + 1;
			gameObject = Resources.Load("Effect/Blood_Ground" + num2) as GameObject;
			UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up * 1.2f, Quaternion.identity);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, new Vector3(entityTransform.position.x, ground.height + 0.1f, entityTransform.position.z), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground.normal);
			gameObject2.transform.rotation = quaternion * gameObject2.transform.rotation;
		}
	}

	public void CreatePlayerSign()
	{
		Transform transform = entityTransform.Find("teamsign");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		GameObject original = Resources.Load("Avatar/TeamSign/P" + (seatID + 1)) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up * 2.6f, Quaternion.identity) as GameObject;
		gameObject.name = "teamsign";
		gameObject.transform.parent = entityTransform;
		Renderer renderer = gameObject.GetComponent<Renderer>();
		Color color = Color.white;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			color = ((!GameApp.GetInstance().GetGameMode().IsTeamMode()) ? UIConstant.COLOR_PLAYER_ICONS[seatID] : UIConstant.COLOR_TEAM_PLAYER[(int)Team]);
		}
		if (renderer != null)
		{
			renderer.material.SetColor("_TintColor", color);
		}
	}

	public void ActivatePlayer(bool bEnable)
	{
		if (!bEnable)
		{
			weapon.EnableGunObject(false);
			Transform transform = entityTransform.Find("Entity");
			int childCount = transform.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				if (transform.GetChild(i).name == "default")
				{
					transform.GetChild(i).GetComponent<Renderer>().enabled = false;
				}
			}
			return;
		}
		Transform transform2 = entityTransform.Find("Entity");
		int childCount2 = transform2.GetChildCount();
		for (int j = 0; j < childCount2; j++)
		{
			if (transform2.GetChild(j).name == "default")
			{
				transform2.GetChild(j).GetComponent<Renderer>().enabled = true;
			}
		}
		weapon.EnableGunObject(true);
	}

	public bool UseItem(byte bagIndex)
	{
		return false;
	}

	public void UseItemByItemID(byte itemID, float hpRate)
	{
	}

	public int GetItemsCount()
	{
		return 0;
	}

	public void AdjustAnimationInLateUpdate(float deltaTime)
	{
		if (AngleV == 0f)
		{
			return;
		}
		float num = Mathf.Clamp(AngleV, -36f, 36f) * ((float)Math.PI / 180f);
		Matrix4x4 identity = Matrix4x4.identity;
		identity[0, 0] = Mathf.Cos(num / 2f);
		identity[0, 1] = 0f - Mathf.Sin(num / 2f);
		identity[1, 0] = Mathf.Sin(num / 2f);
		identity[1, 1] = Mathf.Cos(num / 2f);
		Matrix4x4 matrix4x = Util.CreateMatrixPosition(new Vector3(0f, 0.5f, 0f));
		Vector3 position = (mWeapon.localToWorldMatrix * matrix4x).GetColumn(3);
		Matrix4x4 matrix4x2 = Util.CreateMatrix(mWeapon.right, mWeapon.up, mWeapon.forward, position);
		Matrix4x4 matrix4x3 = matrix4x2.inverse * mWeapon.localToWorldMatrix;
		Matrix4x4 identity2 = Matrix4x4.identity;
		identity2[1, 1] = Mathf.Cos(num);
		identity2[1, 2] = Mathf.Sin(num);
		identity2[2, 1] = 0f - Mathf.Sin(num);
		identity2[2, 2] = Mathf.Cos(num);
		Matrix4x4 matrix4x4 = matrix4x2 * identity2 * matrix4x3;
		if (null != mSpine)
		{
			Matrix4x4 matrix4x5 = Util.RelativeMatrix(mSpine, entityTransform);
			Matrix4x4 matrix4x6 = matrix4x5 * identity;
			Matrix4x4 m = entityTransform.localToWorldMatrix * matrix4x6;
			mSpine.rotation = Util.QuaternionFromMatrix(m);
		}
		if (null != mHead)
		{
			Matrix4x4 matrix4x7 = Util.RelativeMatrix(mHead, entityTransform);
			Matrix4x4 matrix4x8 = matrix4x7 * identity;
			Matrix4x4 m2 = entityTransform.localToWorldMatrix * matrix4x8;
			mHead.rotation = Util.QuaternionFromMatrix(m2);
		}
		for (int i = 0; i < mArms.Length; i++)
		{
			if (null != mArms[i].mShoulder && null != mArms[i].mElbow && null != mArms[i].mShoulder)
			{
				mArms[i].mHandRelativeToWeapon = Util.RelativeMatrix(mArms[i].mHand, mWeapon);
				Vector3 target = (matrix4x4 * mArms[i].mHandRelativeToWeapon).MultiplyPoint3x4(Vector3.zero);
				ikSolver.Solve(new Transform[3]
				{
					mArms[i].mShoulder,
					mArms[i].mElbow,
					mArms[i].mHand
				}, target);
				mArms[i].mHand.rotation = Util.QuaternionFromMatrix(matrix4x4 * mArms[i].mHandRelativeToWeapon);
			}
		}
	}

	public void AddMixingTransformAnimation(string weaponSuffix)
	{
		if (animationObject.GetComponent<Animation>()[AnimationString.RunAttack + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.RunAttack + weaponSuffix].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyAttack + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyAttack + weaponSuffix].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyIdle + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyIdle + weaponSuffix].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.Fly + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.Fly + weaponSuffix].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
	}

	public void SetLowerBodyAnimation(string weaponSuffix, int layer)
	{
		if (animationObject.GetComponent<Animation>()[AnimationString.Run + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.Run + weaponSuffix].layer = layer;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.Idle + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.Idle + weaponSuffix].layer = layer;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.Attack + weaponSuffix] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.Attack + weaponSuffix].layer = layer;
		}
	}

	public void SetAnimationBlending()
	{
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyIdle] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyIdle].layer = -1;
		}
		if (animationObject.GetComponent<Animation>()["fly_stand_shoot_jian_lower"] != null)
		{
			animationObject.GetComponent<Animation>()["fly_stand_shoot_jian_lower"].layer = -1;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyForward] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyForward].layer = -1;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyBack] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyBack].layer = -1;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyLeft] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyLeft].layer = -1;
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyRight] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyRight].layer = -1;
		}
		SetLowerBodyAnimation("_rifle", -1);
		SetLowerBodyAnimation("_shotgun", -1);
		SetLowerBodyAnimation("_bazinga", -1);
		SetLowerBodyAnimation("_grenade_launcher", -1);
		SetLowerBodyAnimation("_BLACKSTARS", -1);
		SetLowerBodyAnimation("_laser", -1);
		SetLowerBodyAnimation("_bow", -1);
		SetLowerBodyAnimation("_fist", -1);
		SetLowerBodyAnimation("_Sniper", -1);
		AddMixingTransformAnimation("_rifle");
		AddMixingTransformAnimation("_shotgun");
		AddMixingTransformAnimation("_bazinga");
		AddMixingTransformAnimation("_grenade_launcher");
		AddMixingTransformAnimation("_BLACKSTARS");
		AddMixingTransformAnimation("_laser");
		AddMixingTransformAnimation("_bow");
		AddMixingTransformAnimation("_fist");
		AddMixingTransformAnimation("_Sniper");
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_machinegun"] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_machinegun"].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
		if (animationObject.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_jian"] != null)
		{
			animationObject.GetComponent<Animation>()[AnimationString.FlyRunAttack + "_jian"].AddMixingTransform(entityTransform.Find(BoneName.UpperBody));
		}
	}

	public void PlayWalkSound()
	{
		if (footStepAudioTimer.Ready())
		{
			footStepAudioTimer.Do();
			AudioManager.GetInstance().PlaySoundAt(FootStepAudio, GetTransform().position);
		}
	}

	public void PlayOnHitSound()
	{
		if (onHitAudioTimer.Ready())
		{
			onHitAudioTimer.Do();
			if (userState.GetSex() == Sex.M)
			{
				AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Player/player_attacked_M", GetTransform().position);
			}
			else
			{
				AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Player/player_attacked_F", GetTransform().position);
			}
		}
	}

	public void ResetRunningPhase()
	{
		runningPhase = -1;
	}

	public void DropAtSpawnPosition()
	{
		Aim(false);
		GameObject playerSpawnPoint = gameWorld.GetPlayerSpawnPoint();
		if (null != playerSpawnPoint)
		{
			List<GameObject> list = new List<GameObject>();
			int childCount = playerSpawnPoint.transform.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				list.Add(playerSpawnPoint.transform.GetChild(i).gameObject);
			}
			NameComparer comparer = new NameComparer();
			list.Sort(comparer);
			for (int j = 1; j < list.Count; j++)
			{
				if (list[j].name == list[j - 1].name)
				{
					Debug.LogError("[PlayerSpawnPoint]Same name: " + list[j].name);
				}
			}
			if (seatID < list.Count)
			{
				GetTransform().position = list[seatID].transform.position;
				GetTransform().rotation = list[seatID].transform.rotation;
			}
			else
			{
				Debug.LogError("[PLAYER SPAWNPOINT] respawnPoint = " + playerSpawnPoint.name);
				GetTransform().position = list[0].transform.position;
				GetTransform().rotation = list[0].transform.rotation;
			}
			GetCollider().enabled = true;
			if (IsLocal())
			{
				if (DYING_STATE.InDyingState)
				{
					GameObject gameObject = entityTransform.Find("Entity").gameObject;
					Debug.Log("into OnRecoverFromDying()");
					DYING_STATE.OnRecoverFromDying();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						PlayerRecoverFromDyingRequest request = new PlayerRecoverFromDyingRequest((short)(RebirthHealthPercentage * 100f));
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
				FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
				if (null != component)
				{
					if (seatID < list.Count)
					{
						component.AngelH = list[seatID].transform.rotation.eulerAngles.y;
					}
					else
					{
						component.AngelH = list[0].transform.rotation.eulerAngles.y;
					}
				}
			}
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up, Quaternion.identity) as GameObject;
	}

	public void StartUnhurt()
	{
		if (IsLocal())
		{
			unhurtTimer.Do();
		}
	}

	public bool IsUnhurtNow()
	{
		return !unhurtTimer.Ready();
	}

	public float RemainUnhurtTime()
	{
		return unhurtTimer.GetTimeSpan() / unhurtTimer.GetInterval();
	}

	public void DropAtSpawnInBoss()
	{
		Aim(false);
		GameObject[] collection = GameObject.FindGameObjectsWithTag(TagName.PLAYER_SPAWN_IN_BOSS);
		List<GameObject> list = new List<GameObject>(collection);
		NameComparer comparer = new NameComparer();
		list.Sort(comparer);
		for (int i = 1; i < list.Count; i++)
		{
			if (list[i].name == list[i - 1].name)
			{
				Debug.LogError("[SpawnPointInBoss]Same name: " + list[i].name);
			}
		}
		if (seatID < list.Count)
		{
			GetTransform().position = list[seatID].transform.position;
			GetTransform().rotation = list[seatID].transform.rotation;
		}
		else
		{
			Debug.LogError("[PLAYER SPAWNPOINT] respawnPoint = SpawnPointInBoss");
			GetTransform().position = list[0].transform.position;
			GetTransform().rotation = list[0].transform.rotation;
		}
		GetCollider().enabled = true;
		if (IsLocal())
		{
			FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
			if (null != component)
			{
				if (seatID < list.Count)
				{
					component.AngelH = list[seatID].transform.rotation.eulerAngles.y;
				}
				else
				{
					component.AngelH = list[0].transform.rotation.eulerAngles.y;
				}
			}
		}
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up, Quaternion.identity);
	}

	public void CreatePlayerNameSign()
	{
		NameSignTrans = entityTransform.Find("Sign");
		if (NameSignTrans != null && NameSignTrans.GetChildCount() == 0)
		{
			GameObject gameObject = null;
			if (Team == TeamName.Red)
			{
				gameObject = Resources.Load("PlayerSignFlag/UI/RedFlag") as GameObject;
			}
			else if (Team == TeamName.Blue)
			{
				gameObject = Resources.Load("PlayerSignFlag/UI/BlueFlag") as GameObject;
			}
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
				gameObject2.transform.parent = NameSignTrans;
				gameObject2.transform.rotation = Quaternion.identity;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.name = "NameSign";
			}
		}
	}

	public void SetPlayerNameSignVisible(bool state)
	{
		if (NameSignTrans != null)
		{
			NameSignTrans.gameObject.SetActive(state);
		}
	}

	public void DropAtSpawnPositionVS()
	{
		Debug.Log("DropAtSpawnPositionVS, isLocalPlayer = " + IsLocal());
		Debug.Log("Team = " + Team);
		Debug.Log("seatID = " + seatID);
		animationObject.GetComponent<Animation>().Stop();
		Aim(false);
		GameObject[] array = null;
		array = ((Team != TeamName.Blue) ? GameObject.FindGameObjectsWithTag(TagName.TEAM_RED_RESPAWN) : GameObject.FindGameObjectsWithTag(TagName.TEAM_BLUE_RESPAWN));
		List<GameObject> list = new List<GameObject>(array);
		NameComparer comparer = new NameComparer();
		list.Sort(comparer);
		for (int i = 1; i < list.Count; i++)
		{
			if (list[i].name == list[i - 1].name)
			{
				Debug.LogError("[PlayerSpawnPoint]Same name: " + list[i].name);
			}
		}
		int num = seatID - 4 * (int)Team;
		Debug.Log("pointID = " + num);
		if (num < list.Count)
		{
			GetTransform().position = list[num].transform.position;
			GetTransform().rotation = list[num].transform.rotation;
		}
		else
		{
			Debug.Log("Team = " + Team);
			Debug.Log("seatID = " + seatID);
			Debug.Log("spawnPointList.Count = " + list.Count);
			Debug.LogError("[PLAYER SPAWNPOINT] respawnPoint = TeamSpawnPoint, pointID = " + num);
			GetTransform().position = list[0].transform.position;
			GetTransform().rotation = list[0].transform.rotation;
		}
		GetCollider().enabled = true;
		if (IsLocal())
		{
			FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
			if (null != component)
			{
				component.AngelV = 0f;
				if (seatID < list.Count)
				{
					component.AngelH = list[seatID].transform.rotation.eulerAngles.y;
				}
				else
				{
					component.AngelH = list[0].transform.rotation.eulerAngles.y;
				}
			}
		}
		GetCharacterController().enabled = true;
		SetState(IDLE_STATE);
		Hp = base.MaxHp;
		Shield = base.MaxShield;
		SendingRebirthRequest = false;
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up, Quaternion.identity) as GameObject;
		if (IsLocal())
		{
			(this as LocalPlayer).RemoveHealingEffect();
			(this as LocalPlayer).ClearDot();
		}
	}

	public void ReSpawnFromArena()
	{
		animationObject.GetComponent<Animation>().Stop();
		if (IsLocal())
		{
			if (GameApp.GetInstance().GetGameMode().IsSingle())
			{
				DYING_STATE.OnRecoverFromDying();
			}
			else
			{
				PlayerRebirthRequest request = new PlayerRebirthRequest(-1);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			GameApp.GetInstance().GetGameScene().State = GameState.Playing;
			SetState(IDLE_STATE);
			ActivatePlayer(true);
			(this as LocalPlayer).RemoveHealingEffect();
		}
	}

	public void ReSpawnAtPoint(int index)
	{
		animationObject.GetComponent<Animation>().Stop();
		if (IsLocal())
		{
			GameApp.GetInstance().GetGameScene().State = GameState.Playing;
		}
		if (GameApp.GetInstance().GetGameScene().mapType == MapType.Instance)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.CHECK_POINT);
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				CheckPointScript component = gameObject.GetComponent<CheckPointScript>();
				if (!(component != null) || component.CheckPointID != index)
				{
					continue;
				}
				RespawnStreamingScript componentInChildren = component.gameObject.GetComponentInChildren<RespawnStreamingScript>();
				if (componentInChildren != null)
				{
					GetTransform().position = componentInChildren.transform.position;
					GetTransform().rotation = componentInChildren.transform.rotation;
					if (IsLocal())
					{
						FirstPersonCameraScript component2 = Camera.main.GetComponent<FirstPersonCameraScript>();
						if (null != component2)
						{
							component2.AngelV = 0f;
							component2.AngelH = componentInChildren.transform.rotation.eulerAngles.y;
						}
					}
				}
				else
				{
					Debug.Log("NO RespawnStreamingScript found.");
				}
				break;
			}
		}
		if (GameApp.GetInstance().GetGameScene().mapType == MapType.Arena || GameApp.GetInstance().GetGameScene().mapType == MapType.BossRush)
		{
			if (IsLocal())
			{
				ActivatePlayer(true);
			}
			else
			{
				ActivatePlayer(false);
			}
		}
		else
		{
			ActivatePlayer(true);
		}
		GetCharacterController().enabled = true;
		SetState(IDLE_STATE);
		Hp = base.MaxHp;
		Shield = base.MaxShield;
		SendingRebirthRequest = false;
		GameObject original = Resources.Load("Loot/item_gold") as GameObject;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up, Quaternion.identity) as GameObject;
		if (IsLocal())
		{
			(this as LocalPlayer).RemoveHealingEffect();
		}
	}

	public bool DoWaitVSRebirth()
	{
		return vsRebirthTimer.Ready();
	}

	public void OnWaitVSRebirth()
	{
		vsRebirthTimer = new Timer();
		vsRebirthTimer.SetTimer(15f, false);
	}

	public int GetVSRebirthRemainingTime()
	{
		return (int)(15f - vsRebirthTimer.GetTimeSpan());
	}

	public virtual bool StartWaitRebirth()
	{
		return true;
	}

	public Timer GetRebirthTimer()
	{
		return rebirthTimer;
	}

	public Timer GetFirstAidTimer()
	{
		return firstAidTimer;
	}

	public Timer GetSaveTimer()
	{
		return SavedTimer;
	}

	public void StopSaveTimer()
	{
		SavedTimer.Disable();
	}

	public void StartSaveTimer()
	{
		SavedTimer.SetTimer(3f, false);
		SavedTimer.Do();
	}

	public void StartFirstAidTimer()
	{
		firstAidTimer.SetTimer(3f, false);
		firstAidTimer.Do();
	}

	public void Attack()
	{
		JustMakeAShoot = true;
		weapon.Attack(Time.deltaTime);
	}

	public void PlayAttackSound()
	{
		weapon.PlaySound();
	}

	public override void Move(Vector3 motion)
	{
		Vector3 motion2 = motion * base.Speed * Time.deltaTime;
		if (CanUseGravity())
		{
			motion2.y = -0.2f;
		}
		else
		{
			motion2.y = -0.001f;
		}
		cc.Move(motion2);
		hasGravity = !cc.isGrounded;
	}

	public void Reload()
	{
		if (CanReload())
		{
			if (InAimState)
			{
				Aim(false);
			}
			if (GetUserState().GetBulletByWeaponType(weapon.GetWeaponType()) > 0 && weapon.BulletCountInGun < weapon.GunCapacity)
			{
				StopSpecialAction();
				PlayAnimation(GetWeaponAnimationSuffix() + AnimationString.Reload, WrapMode.ClampForever, weapon.GetReloadAnimationSpeed());
				SetState(RELOAD_STATE);
				weapon.Reload();
			}
		}
	}

	public void ReloadComplete()
	{
		weapon.ReloadComplete();
	}

	public void ThrowGrenade()
	{
		if (HandGrenade == null || !ThrowGrenadeTimer.Ready() || !InPlayingState() || State == THROW_GRENADE_STATE || State == THROW_GRENADE_SKILL_STATE)
		{
			return;
		}
		if (InAimState)
		{
			Aim(false);
		}
		if (GetUserState().GetBulletByWeaponType(WeaponType.Grenade) > 0)
		{
			if (State == RELOAD_STATE)
			{
				weapon.StopReload();
			}
			else if (State == MELEE_ATTACK_STATE)
			{
				CancelMeleeAttack();
			}
			else if (State == SWITCH_WEAPON_DOWN_STATE)
			{
				SwitchWeapon();
			}
			Debug.Log("Grenade Count: " + GetUserState().GetBulletByWeaponType(WeaponType.Grenade));
			StopAnimation(AnimationString.ThrowGrenade);
			PlayAnimation(AnimationString.ThrowGrenade, WrapMode.ClampForever);
			SetState(THROW_GRENADE_STATE);
			ThrowGrenadeTimer.Do();
		}
	}

	public void ThrowGrenadeSkill(CharacterMakeDamageSkill _skill)
	{
		if (InPlayingState() && State != THROW_GRENADE_SKILL_STATE)
		{
			if (InAimState)
			{
				Aim(false);
			}
			StopSpecialAction();
			StopAnimation(AnimationString.ThrowGrenade);
			PlayAnimation(AnimationString.ThrowGrenade, WrapMode.ClampForever);
			(THROW_GRENADE_SKILL_STATE as ThrowGrenadeSkillState).InitThrow(_skill);
			SetState(THROW_GRENADE_SKILL_STATE);
		}
	}

	public void MeleeAttack()
	{
		if (MeleeAttackTimer.Ready() && InPlayingState() && State != MELEE_ATTACK_STATE)
		{
			if (InAimState)
			{
				Aim(false);
			}
			if (State == RELOAD_STATE)
			{
				weapon.StopReload();
			}
			else if (State == SWITCH_WEAPON_DOWN_STATE)
			{
				SwitchWeapon();
			}
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.I_need_no_weapons, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
			(MELEE_ATTACK_STATE as MeleeAttackState).ClearHitEnemies();
			Knife.SetActive(true);
			PlayAnimation(AnimationString.MeleeAttack, WrapMode.ClampForever);
			SetState(MELEE_ATTACK_STATE);
			AudioManager.GetInstance().PlaySound("RPG_Audio/Weapon/Knife/knf_melee01");
			KnifeEffect = EffectPlayer.GetInstance().PlayMeleeWave();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerMeleeAttackRequest request = new PlayerMeleeAttackRequest();
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			MeleeAttackTimer.Do();
		}
	}

	public void StartJump()
	{
		velocityJumpY = 4f;
	}

	public bool Jump()
	{
		velocityJumpY += Physics.gravity.y * Time.deltaTime;
		inputController.inputInfo.moveDirection += new Vector3(0f, velocityJumpY, 0f);
		if (cc.isGrounded && velocityJumpY < 0f)
		{
			velocityJumpY = 0f;
			return true;
		}
		return false;
	}

	public virtual void ChangeWeaponInBag(int bagIndex)
	{
	}

	protected void ChangeWeapon(Weapon w)
	{
		if (w == null)
		{
			return;
		}
		if (weapon != null)
		{
			if (IsLocal() && InAimState)
			{
				Aim(false);
			}
			weapon.GunOff();
		}
		weapon = w;
		weapon.GunOn();
		userState.SetWeapon(GetWeapon().GetWeaponType(), GetWeapon().WeaponNameNumber);
		if (IsLocal())
		{
			(this as LocalPlayer).UsedBulletWithoutReload = 0;
		}
	}

	public void SwitchWeapon()
	{
		if (weapon != null)
		{
			if (IsLocal() && InAimState)
			{
				Aim(false);
			}
			weapon.GunOff();
		}
		weapon = weaponToChange;
		weapon.GunOn();
		userState.SetWeapon(GetWeapon().GetWeaponType(), GetWeapon().WeaponNameNumber);
		if (IsLocal())
		{
			(this as LocalPlayer).UsedBulletWithoutReload = 0;
		}
	}

	public virtual void RefreshAvatar()
	{
	}

	public void GetKnocked()
	{
		cc.Move(-entityTransform.forward * knockedSpeed * Time.deltaTime);
		if (KnockedComplete())
		{
			SetState(IDLE_STATE);
		}
	}

	public void Aim(bool bAim)
	{
		if (!IsLocal())
		{
			return;
		}
		InAimState = bAim;
		if (bAim)
		{
			if (GetWeapon().HasScope)
			{
				openAimTask = new OpenAimTask();
				openAimTask.ExecuteSchedule = 0.2f;
				TaskManager taskManager = GameApp.GetInstance().GetGameScene().GetTaskManager();
				taskManager.StartTask(openAimTask);
				if (HUDManager.instance != null)
				{
					HUDManager.instance.m_InfoManager.m_SightHeadState.SetActive(false);
				}
				Transform transform = Camera.main.transform.Find("Npc_Collision");
				transform.GetComponent<Collider>().enabled = true;
			}
		}
		else if (GetWeapon().HasScope)
		{
			TaskManager taskManager2 = GameApp.GetInstance().GetGameScene().GetTaskManager();
			taskManager2.StopTask(openAimTask);
			if (HUDManager.instance != null)
			{
				HUDManager.instance.m_InfoManager.m_Aim.SetActive(false);
				HUDManager.instance.m_InfoManager.m_SightHeadState.SetActive(true);
			}
			ActivatePlayer(true);
			Transform transform2 = Camera.main.transform.Find("Npc_Collision");
			transform2.GetComponent<Collider>().enabled = false;
		}
	}

	public void UnderAttackSetHP(int hp)
	{
		int hp2 = Hp;
		if (InPlayingState())
		{
			if (IsLocal())
			{
				PlayOnHitSound();
				Transform transform = Camera.main.transform.Find("Screen_Blood");
				if (transform != null)
				{
					ScreenBloodScript component = transform.GetComponent<ScreenBloodScript>();
					component.NewBlood(hp2 - hp);
				}
			}
			else if (hp2 == hp)
			{
				return;
			}
			if (IsLocal() && GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Boss && hp < hp2)
			{
				Debug.Log("Dodge_This --- Data");
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(-1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
				Debug.Log("Dodge_This --- Stop");
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Stop);
				AchievementManager.GetInstance().Trigger(trigger);
			}
			ResetShieldRecoveryStartTimer();
			Hp = hp;
			hp2 = hp;
			if (IsLocal())
			{
				if (hp2 == 1)
				{
					AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Start);
					AchievementManager.GetInstance().Trigger(trigger2);
				}
				else
				{
					AchievementTrigger trigger3 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Stop);
					AchievementManager.GetInstance().Trigger(trigger3);
				}
				skillMgr.OnHPValueTrigger(this, hp2, (float)hp2 / (float)base.MaxHp);
			}
			if (hp2 <= 0 && InPlayingState() && !InDyingState() && State != FALL_DOWN_STATE)
			{
				StopSpecialAction();
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					if (IsLocal())
					{
						LocalPlayer localPlayer = (LocalPlayer)this;
						localPlayer.GetTransform().rotation = Quaternion.Euler(0f, localPlayer.GetTransform().rotation.eulerAngles.y, 0f);
						localPlayer.RestoreEntityLocalPosition();
					}
					IsDeadJustNow = true;
					OnDead();
					SetState(DEAD_STATE);
				}
				else
				{
					BeginFallDownState();
				}
			}
		}
		NeverGotHit = false;
	}

	public void UnderAttackSetShield(int shield)
	{
		if (InPlayingState())
		{
			int shield2 = Shield;
			int num = shield2 - shield;
			if (IsLocal())
			{
				Transform transform = Camera.main.transform.Find("Screen_Blood");
				if (transform != null)
				{
					ScreenBloodScript component = transform.GetComponent<ScreenBloodScript>();
					component.NewBlood(num);
				}
			}
			else if (shield2 == shield)
			{
				return;
			}
			ResetShieldRecoveryStartTimer();
			if (shield < shield2 && shield <= 0)
			{
				ShowShieldBreak();
			}
			shield2 = shield;
			if (IsLocal())
			{
				if (base.MaxShield != 0)
				{
					skillMgr.OnShieldValueTrigger(this, shield2, shield2 / base.MaxShield);
				}
				(this as LocalPlayer).RecoverHP((float)num * CapacityToHealthPercentage);
			}
			Shield = shield2;
		}
		NeverGotHit = false;
	}

	public void UnderAttackSetExtraShield(int extraShield)
	{
		if (InPlayingState())
		{
			int num = ExtraShield - extraShield;
			if (IsLocal())
			{
				if (extraShield == 0 && ExtraShield != 0)
				{
					(this as LocalPlayer).ClearExtraShield();
				}
			}
			else if (ExtraShield == extraShield)
			{
				return;
			}
			ExtraShield = extraShield;
		}
		NeverGotHit = false;
	}

	public virtual void ShowShieldBreak()
	{
	}

	public void UnderAttack(int damage)
	{
		if (InPlayingState())
		{
			int shield = Shield;
			if (ExtraShield > 0 && ExtraShield >= damage)
			{
				ExtraShield -= damage;
			}
			else if (shield > 0 && shield > damage)
			{
				if (ExtraShield > 0)
				{
					damage -= ExtraShield;
					(this as LocalPlayer).ClearExtraShield();
				}
				shield = (Shield = shield - damage);
				skillMgr.OnShieldValueTrigger(this, shield, shield / base.MaxShield);
				if (IsLocal())
				{
					(this as LocalPlayer).RecoverHP((float)damage * CapacityToHealthPercentage);
				}
				ResetShieldRecoveryStartTimer();
			}
			else
			{
				ResetShieldRecoveryStartTimer();
				if (shield != 0)
				{
					skillMgr.OnShieldValueTrigger(this, 0, 0f);
					if (IsLocal())
					{
						(this as LocalPlayer).RecoverHP((float)shield * CapacityToHealthPercentage);
					}
					ShowShieldBreak();
				}
				int hp = Hp;
				damage -= shield;
				shield = 0;
				hp -= damage;
				hp = (Hp = Mathf.Clamp(hp, 0, base.MaxHp));
				Shield = shield;
				skillMgr.OnHPValueTrigger(this, hp, (float)hp / (float)base.MaxHp);
				if (hp <= 0)
				{
					StopSpecialAction();
					if ((GameApp.GetInstance().GetGameMode().IsSingle() && GameApp.GetInstance().GetUserState().GetCash() < GetInstanceRespawnCost()) || GameApp.GetInstance().GetGameWorld().BossState == EBossState.BATTLE || Arena.GetInstance().IsCurrentSceneArena())
					{
						if (IsLocal())
						{
							LocalPlayer localPlayer = (LocalPlayer)this;
							localPlayer.GetTransform().rotation = Quaternion.Euler(0f, localPlayer.GetTransform().rotation.eulerAngles.y, 0f);
							localPlayer.RestoreEntityLocalPosition();
						}
						IsDeadJustNow = true;
						OnDead();
						SetState(DEAD_STATE);
					}
					else if (InPlayingState() && !InDyingState() && State != FALL_DOWN_STATE)
					{
						BeginFallDownState();
					}
				}
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(-1);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
				AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Stop);
				AchievementManager.GetInstance().Trigger(trigger);
				if (hp == 1)
				{
					AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Start);
					AchievementManager.GetInstance().Trigger(trigger2);
				}
				else
				{
					AchievementTrigger trigger3 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Stop);
					AchievementManager.GetInstance().Trigger(trigger3);
				}
			}
		}
		NeverGotHit = false;
	}

	public void Block()
	{
	}

	public bool DoWin()
	{
		if (!winSpecialFinish)
		{
			PlayAnimation(AnimationString.WinSpecial, WrapMode.ClampForever);
			if (AnimationPlayed(AnimationString.WinSpecial, 1f))
			{
				winSpecialFinish = true;
			}
		}
		else if (GetWeapon().GetWeaponType() == WeaponType.MachineGun)
		{
			PlayAnimation(AnimationString.WinIdleMachineGun, WrapMode.Loop);
		}
		else if (isSpecialWinIdle)
		{
			PlayAnimation(AnimationString.WinIdleSpecial, WrapMode.Loop);
		}
		else
		{
			PlayAnimationWithoutBlend(AnimationString.WinIdle, WrapMode.Loop);
		}
		return winAnimationTimer.Ready();
	}

	public bool DoVSLose()
	{
		return winAnimationTimer.Ready();
	}

	public virtual bool CheckLose()
	{
		return false;
	}

	public virtual bool FallDownCompleted()
	{
		return false;
	}

	public virtual bool DeadAnimationCompleted()
	{
		return false;
	}

	public virtual bool WinAnimationCompleted()
	{
		return false;
	}

	public void OnHit(int attackDamage, Enemy enemy)
	{
		if (IsUnhurtNow() || (Shield > 0 && CheckIfImmuniteDamageDamage()))
		{
			return;
		}
		attackDamage = (int)((float)attackDamage * (1f - (float)base.DamageReduction * 0.01f));
		if (InPlayingState())
		{
			if (!GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode() && enemy != null)
			{
				attackDamage = DamageLevelSuppress(attackDamage, enemy.Level, enemy.Level, GameApp.GetInstance().GetUserState().GetCharLevel());
			}
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerOnHitRequest request = new PlayerOnHitRequest((short)attackDamage, false, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				UnderAttack(attackDamage);
				PlayOnHitSound();
			}
			UserStateHUD.GetInstance().PushUnitWhoAttacksUser(enemy);
		}
	}

	public void StartKnocked(float speed)
	{
		if (InPlayingState() && Mathf.Abs(speed) > 1f)
		{
			if (IsLocal())
			{
				StopSpecialAction();
			}
			knockedSpeed = speed;
			GetWeapon().StopFire();
			GetWeapon().AutoDestructEffect();
			if (State != FALL_DOWN_STATE)
			{
				SetState(KNOCKED_STATE);
			}
			knockStartTime = Time.time;
		}
	}

	public override void OnKnocked(float speed)
	{
		if (ExtraShield <= 0)
		{
			StartKnocked(speed);
			if (InPlayingState() && Mathf.Abs(speed) > 1f && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerOnKnockedRequest request = new PlayerOnKnockedRequest(speed);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public override void OnDead()
	{
		GetWeapon().StopFire();
		if (InAimState)
		{
			Aim(false);
		}
		StopSpecialAction();
		PlayAnimation(AnimationString.Dead, WrapMode.ClampForever);
		if (userState.GetSex() == Sex.M)
		{
			AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_killed_M");
		}
		else
		{
			AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_killed_F");
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			CreateDeadBlood();
		}
	}

	public void OnPickUp(LootType lootType, int amount)
	{
		switch (lootType)
		{
		case LootType.Enegy:
		{
			userState.Enegy += amount;
			pickupEnegy += amount;
			AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_energy");
			GameObject original2 = Resources.Load("Loot/item_enegy") as GameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(original2, entityTransform.position + Vector3.up, Quaternion.identity) as GameObject;
			gameObject2.transform.parent = entityTransform;
			break;
		}
		case LootType.Money:
		{
			AddPickupCash(amount);
			int num = UnityEngine.Random.Range(0, 100);
			if (num < 50)
			{
				AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money01");
			}
			else
			{
				AudioManager.GetInstance().PlaySound("Audio/pickup/pickup_money02");
			}
			GameObject original = Resources.Load("Loot/item_gold") as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate(original, entityTransform.position + Vector3.up, Quaternion.identity) as GameObject;
			gameObject.transform.parent = entityTransform;
			break;
		}
		}
	}

	public void OnPickUp(short sequenceID)
	{
		GameObject gameObject = GameObject.Find("Loot_" + sequenceID);
		if (gameObject != null)
		{
			ItemScript component = gameObject.GetComponent<ItemScript>();
			OnPickUp(component.itemType, component.Amount);
		}
	}

	public FloorInfo GetGround()
	{
		FloorInfo floorInfo = new FloorInfo();
		floorInfo.height = Global.FLOORHEIGHT;
		Vector3 up = Vector3.up;
		Ray ray = new Ray(entityTransform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 10f, 1 << PhysicsLayer.FLOOR))
		{
			floorInfo.height = hitInfo.point.y;
			floorInfo.normal = hitInfo.normal;
		}
		if (floorInfo.normal.y < 0f)
		{
			floorInfo.normal = -floorInfo.normal;
		}
		return floorInfo;
	}

	public void RefreshWeaponListFromItemInfo()
	{
		int currentEquipWeaponSlot = userState.ItemInfoData.CurrentEquipWeaponSlot;
		if (IsLocal())
		{
			userState.ReturnBullets();
			StopSpecialAction();
			SetState(IDLE_STATE);
		}
		ClearWeaponList();
		weaponList.Add(null);
		weaponList.Add(null);
		weaponList.Add(null);
		weaponList.Add(null);
		if (userState.ItemInfoData.IsWeapon1Equiped)
		{
			NGUIBaseItem baseItem = userState.ItemInfoData.Weapon1.baseItem;
			if (baseItem != null)
			{
				Weapon weapon = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem.ItemClass, baseItem.name);
				weapon.SetWeaponPropertyWithNGUIBaseItem(baseItem);
				weaponList[0] = weapon;
			}
		}
		if (userState.ItemInfoData.IsWeapon2Equiped)
		{
			NGUIBaseItem baseItem2 = userState.ItemInfoData.Weapon2.baseItem;
			if (baseItem2 != null)
			{
				Weapon weapon2 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem2.ItemClass, baseItem2.name);
				weapon2.SetWeaponPropertyWithNGUIBaseItem(baseItem2);
				weaponList[1] = weapon2;
			}
		}
		if (userState.ItemInfoData.IsWeapon3Equiped)
		{
			NGUIBaseItem baseItem3 = userState.ItemInfoData.Weapon3.baseItem;
			if (baseItem3 != null)
			{
				Weapon weapon3 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem3.ItemClass, baseItem3.name);
				weapon3.SetWeaponPropertyWithNGUIBaseItem(baseItem3);
				weaponList[2] = weapon3;
			}
		}
		if (userState.ItemInfoData.IsWeapon4Equiped)
		{
			NGUIBaseItem baseItem4 = userState.ItemInfoData.Weapon4.baseItem;
			if (baseItem4 != null)
			{
				Weapon weapon4 = WeaponFactory.GetInstance().CreateWeapon((WeaponType)baseItem4.ItemClass, baseItem4.name);
				weapon4.SetWeaponPropertyWithNGUIBaseItem(baseItem4);
				weaponList[3] = weapon4;
			}
		}
		HandGrenade = null;
		if (userState.ItemInfoData.IsHandGrenadeEquiped)
		{
			NGUIBaseItem baseItem5 = userState.ItemInfoData.HandGrenade.baseItem;
			if (baseItem5 != null)
			{
				HandGrenade = new GrenadeInfo();
				HandGrenade.Level = baseItem5.ItemLevel;
				HandGrenade.Name = baseItem5.name;
				HandGrenade.DamageInit = Mathf.CeilToInt(baseItem5.itemStats[0].statValue);
				HandGrenade.Damage = HandGrenade.DamageInit;
				HandGrenade.ExplosionRangeInit = baseItem5.itemStats[10].statValue;
				HandGrenade.ExplosionRange = HandGrenade.ExplosionRangeInit;
				HandGrenade.elementType = ElementType.Explosion;
				if (baseItem5.itemStats[11].statValue > 0f)
				{
					HandGrenade.elementType = ElementType.Fire;
					HandGrenade.elementPara = (byte)baseItem5.itemStats[11].statValue;
				}
				else if (baseItem5.itemStats[12].statValue > 0f)
				{
					HandGrenade.elementType = ElementType.Shock;
					HandGrenade.elementPara = (byte)baseItem5.itemStats[12].statValue;
				}
				else if (baseItem5.itemStats[13].statValue > 0f)
				{
					HandGrenade.elementType = ElementType.Corrosive;
					HandGrenade.elementPara = (byte)baseItem5.itemStats[13].statValue;
				}
				HandGrenade.CriticalRateInit = baseItem5.itemStats[7].statValue;
				HandGrenade.CriticalRate = HandGrenade.CriticalRateInit;
				HandGrenade.CriticalDamageInit = baseItem5.itemStats[8].statValue;
				HandGrenade.CriticalDamage = HandGrenade.CriticalDamageInit;
			}
		}
		if (IsLocal())
		{
			List<string> list = new List<string>();
			List<AnimationClip> list2 = new List<AnimationClip>();
			foreach (AnimationState item in animationObject.GetComponent<Animation>())
			{
				if (!(item == null) && !(item.clip == null) && !(item.clip.name == "Take 001"))
				{
					list.Add(item.clip.name);
					list2.Add(item.clip);
				}
			}
			foreach (string item2 in list)
			{
				animationObject.GetComponent<Animation>().RemoveClip(item2);
			}
			list.Clear();
			foreach (AnimationClip item3 in list2)
			{
				Resources.UnloadAsset(item3);
			}
			list.Clear();
			AvatarBuilder.GetInstance().AddAnimationsForFirstPerson(animationObject, this);
		}
		foreach (Weapon weapon5 in weaponList)
		{
			if (weapon5 != null)
			{
				weapon5.Init(this);
			}
		}
		if (!IsLocal())
		{
			return;
		}
		Weapon w = null;
		int num = -1;
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i] != null)
			{
				w = weaponList[i];
				num = i;
				break;
			}
		}
		if (currentEquipWeaponSlot >= weaponList.Count || weaponList[currentEquipWeaponSlot] == null)
		{
			ChangeWeapon(w);
			userState.ItemInfoData.CurrentEquipWeaponSlot = (byte)num;
		}
		else
		{
			ChangeWeapon(weaponList[currentEquipWeaponSlot]);
		}
		if (NGUIBackPackUIScript.mInstance != null)
		{
			NGUIBackPackUIScript.mInstance.CreateAvatar();
		}
		if (ShopSellPageScript.mInstance != null)
		{
			ShopSellPageScript.mInstance.CreateAvatar();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ElementType element = GetWeapon().mCurrentElementType;
			if (GetWeapon().IsAllElement())
			{
				element = ElementType.AllElement;
			}
			PlayerRefreshWeaponRequest request = new PlayerRefreshWeaponRequest(weaponList, (byte)currentEquipWeaponSlot, element);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void ClearWeaponList()
	{
		if (weaponList == null)
		{
			return;
		}
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i] != null)
			{
				weaponList[i].DestroyWeapon();
			}
		}
		weaponList.Clear();
	}

	public void RefreshShieldFromItemInfo()
	{
		if (!IsLocal())
		{
			return;
		}
		Shield = 0;
		base.MaxShield = 0;
		base.ShieldRecovery = 0;
		BasicMaxShield = 0;
		ShieldRecoveryDelayInit = 999f;
		ShieldRecoveryDelay = 999f;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerShieldRecoveryRequest request = new PlayerShieldRecoveryRequest(0, 0, 0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		if (userState.ItemInfoData.IsShieldEquiped)
		{
			NGUIBaseItem baseItem = userState.ItemInfoData.Shield.baseItem;
			if (baseItem != null)
			{
				base.MaxShield = (int)baseItem.itemStats[0].statValue;
				BasicMaxShield = base.MaxShield;
				base.ShieldRecovery = (int)baseItem.itemStats[1].statValue;
				BasicShieldRecovery = base.ShieldRecovery;
				mShieldRecoveryStartTimer.SetTimer(baseItem.itemStats[2].statValue, true);
				mShieldRecoverySecondTimer.SetTimer(1f, true);
				ShieldRecoveryDelayInit = baseItem.itemStats[2].statValue;
				ShieldRecoveryDelay = ShieldRecoveryDelayInit;
			}
		}
	}

	public void SetCurrentCityAndSceneID(byte cityID, byte sceneID)
	{
		CurrentCityID = cityID;
		CurrentSceneID = sceneID;
	}

	public bool CheckIfImmuniteDamageDamage()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num < DamageImmunityRateBonus)
		{
			return true;
		}
		return false;
	}

	public int GetShieldCapacity()
	{
		return BasicMaxShield;
	}

	public int GetShieldRecovery()
	{
		return BasicShieldRecovery;
	}

	public float GetShieldRecoverDelay()
	{
		return ShieldRecoveryDelayInit;
	}

	public virtual void BeginFallDownState()
	{
	}

	public void CreateExtraShieldWithEffect(int extraShield)
	{
		if (extraShield > 0)
		{
			GameObject original = Resources.Load("RPG_effect/RPG_ProtectionCover") as GameObject;
			ExtraShieldObject = UnityEngine.Object.Instantiate(original, GetPosition() + Vector3.up * 1.2f, Quaternion.identity) as GameObject;
			ExtraShieldObject.transform.parent = entityTransform;
			ExtraShield = extraShield;
		}
	}

	public void ClearExtraShieldWithEffect()
	{
		ExtraShield = 0;
		if (ExtraShieldObject != null)
		{
			UnityEngine.Object.Destroy(ExtraShieldObject);
			ExtraShieldObject = null;
			EffectPlayer.GetInstance().PlayExtraShieldBreak(GetPosition() + Vector3.up * 1.2f);
		}
	}

	public void ClearExtraShieldWithoutEffect()
	{
		ExtraShield = 0;
		UnityEngine.Object.Destroy(ExtraShieldObject);
		ExtraShieldObject = null;
	}

	public void Apply_Shield_Chip_Grenade_States()
	{
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (itemInfoData.IsShieldEquiped)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.Shield.baseItem);
		}
		if (itemInfoData.IsSlot1Equiped)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.Slot1.baseItem);
		}
		if (itemInfoData.IsSlot2Equiped && itemInfoData.IsSlot2Enable)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.Slot2.baseItem);
		}
		if (itemInfoData.IsSlot3Enable && itemInfoData.IsSlot3Equiped)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.Slot3.baseItem);
		}
		if (itemInfoData.IsSlot4Enable && itemInfoData.IsSlot4Equiped)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.Slot4.baseItem);
		}
		if (itemInfoData.IsHandGrenadeEquiped)
		{
			Apply_Shield_Chip_Grenade_States(itemInfoData.HandGrenade.baseItem);
		}
	}

	public void Apply_Shield_Chip_Grenade_States(NGUIBaseItem item)
	{
		if (item != null)
		{
			if (item.ItemClass == ItemClasses.U_Shield)
			{
				detailedProperties.HpPercentageBonus += item.itemStats[6].statValue;
				hpRecoverValueByShield += Mathf.CeilToInt(item.itemStats[7].statValue);
			}
			else if (item.ItemClass == ItemClasses.V_Slot)
			{
				detailedProperties.DamageBonusAll += item.itemStats[0].statValue;
				detailedProperties.AccuracyBonusAll += item.itemStats[1].statValue;
				detailedProperties.AttackIntervalPercentageBonusAll += item.itemStats[2].statValue;
				detailedProperties.ReloadTimeBonusAll += item.itemStats[3].statValue;
				detailedProperties.MagsBonusAll += item.itemStats[4].statValue;
				detailedProperties.RecoilBonusAll -= item.itemStats[5].statValue;
				bulletRecoverValueByChip += Mathf.CeilToInt(item.itemStats[6].statValue);
				detailedProperties.CriticalRateBonusAll += item.itemStats[7].statValue;
				detailedProperties.CriticalDamageBonusAll += item.itemStats[8].statValue;
				detailedProperties.MeleeDamageBonus += item.itemStats[9].statValue;
				detailedProperties.MaxShieldPercentageBonus += item.itemStats[10].statValue;
				detailedProperties.ShieldRecoveryPercentageBonus += item.itemStats[11].statValue;
				detailedProperties.ShieldRecoverDelayBonusPercentage += item.itemStats[12].statValue;
				detailedProperties.ElementsResistanceBonus[0] += Mathf.CeilToInt(item.itemStats[13].statValue);
				detailedProperties.ElementsResistanceBonus[1] += Mathf.CeilToInt(item.itemStats[14].statValue);
				detailedProperties.ElementsResistanceBonus[2] += Mathf.CeilToInt(item.itemStats[15].statValue);
				detailedProperties.HpPercentageBonus += item.itemStats[16].statValue;
				hpRecoverValueByChip += Mathf.CeilToInt(item.itemStats[17].statValue);
				detailedProperties.SpeedBonus += item.itemStats[18].statValue;
				detailedProperties.DamageReductionBonus += Mathf.CeilToInt(item.itemStats[19].statValue);
				detailedProperties.DamageToHealthBonus += Mathf.CeilToInt(item.itemStats[20].statValue);
				detailedProperties.ElementsResistanceBonus[0] += Mathf.CeilToInt(item.itemStats[21].statValue);
				detailedProperties.ElementsResistanceBonus[1] += Mathf.CeilToInt(item.itemStats[21].statValue);
				detailedProperties.ElementsResistanceBonus[2] += Mathf.CeilToInt(item.itemStats[21].statValue);
				detailedProperties.ExplosionDamageReductionBonus += Mathf.CeilToInt(item.itemStats[22].statValue);
				detailedProperties.DamageImmunityRateBonus += item.itemStats[23].statValue;
				detailedProperties.DropRateBonus += item.itemStats[24].statValue;
			}
			else if (item.ItemClass == ItemClasses.Grenade)
			{
				detailedProperties.MeleeDamageBonus += item.itemStats[6].statValue;
				detailedProperties.HpPercentageBonus += item.itemStats[16].statValue;
				hpRecoverValueByGrenade += Mathf.CeilToInt(item.itemStats[17].statValue);
			}
		}
	}

	public int GetBulletInGuns(WeaponType wType)
	{
		int num = 0;
		foreach (Weapon weapon in GetWeaponList())
		{
			if (weapon != null && weapon.GetWeaponType() == wType)
			{
				num += weapon.BulletCountInGun;
			}
		}
		return num;
	}

	public virtual void LoadSceneOnDead()
	{
	}

	public virtual void LoadSceneOfBoss()
	{
	}

	public virtual void CancelMeleeAttack()
	{
		Knife.SetActive(false);
		UnityEngine.Object.Destroy(KnifeEffect);
		KnifeEffect = null;
	}

	public virtual void StopSpecialAction()
	{
		if (State == RELOAD_STATE)
		{
			weapon.StopReload();
		}
		else if (State == MELEE_ATTACK_STATE)
		{
			CancelMeleeAttack();
		}
		else if (State == SWITCH_WEAPON_DOWN_STATE)
		{
			SwitchWeapon();
		}
	}

	public void PlaySpeedDownEffect()
	{
		if (SpeedDownEffect == null)
		{
			SpeedDownEffect = EffectPlayer.GetInstance().PlaySpeedDownEffect(GetAnimationObject().transform);
		}
	}

	public void ClearSpeedDownEffect()
	{
		if (SpeedDownEffect != null)
		{
			UnityEngine.Object.Destroy(SpeedDownEffect);
			SpeedDownEffect = null;
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
		float num6 = num4 + num5;
		if (num6 < -0.6f)
		{
			num6 = -0.6f;
		}
		num = (int)((float)num * (1f + num6));
		if (num < 1)
		{
			num = 1;
		}
		return num;
	}

	public void RefreshInstanceRespawnTimes()
	{
		instanceRespawnTimes = 0;
	}

	public void InstanceRespawn()
	{
		instanceRespawnTimes++;
	}

	public int GetInstanceRespawnCost()
	{
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		if (instanceRespawnTimes < 5)
		{
			return (int)((float)(5 * (instanceRespawnTimes + 1)) * Mathf.Sqrt(charLevel * charLevel * charLevel));
		}
		return 5 * (instanceRespawnTimes - 4) * charLevel * charLevel;
	}

	public static GameObject GetPlayerByCollider(Collider c)
	{
		GameObject gameObject = c.gameObject;
		while (gameObject.transform.parent != null)
		{
			gameObject = gameObject.transform.parent.gameObject;
		}
		return gameObject;
	}
}
