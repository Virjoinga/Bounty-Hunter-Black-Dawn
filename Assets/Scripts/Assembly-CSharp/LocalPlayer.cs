using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
	public enum DamagePara
	{
		DamageImmunityRate = 0,
		DamageReduction = 1,
		ElementResistanceFire = 2,
		ElementResistanceShock = 3,
		ElementResistanceCorrosive = 4,
		ElementResistanceExplosion = 5
	}

	private string hpStr;

	private string shieldStr;

	protected Timer sendingTimer = new Timer();

	protected Timer sendingShootDirTimer = new Timer();

	protected NetworkManager networkMgr;

	protected NetworkTransform lastTrans = new NetworkTransform(Vector3.zero, 0f, 0, false);

	protected WeaponType lastWT = WeaponType.AssaultRifle;

	protected Timer hpRecoveryTimer = new Timer();

	protected Timer bulletRecoveryTimer = new Timer();

	protected Timer extraShieldDurationTimer = new Timer();

	protected Timer uploadScoreTimer = new Timer();

	protected UnityEngine.AI.NavMeshAgent agent;

	protected List<GameObject> mStreamingVolumeList = new List<GameObject>();

	protected List<BulletRecoverTimer> bulletRecoverTimers = new List<BulletRecoverTimer>();

	protected List<HpRecoverTimer> hpRecoverTimers = new List<HpRecoverTimer>();

	private float shieldRecoveredDeltaTime;

	protected Vector3 mEntityLocalPosition;

	protected TweenPosition m_tweenPos;

	protected bool mNeedLevelUpSfx;

	public float m_deadRotateAngle;

	public GameObject HealingEffect;

	public bool IsInstantHealing;

	protected float startInstantHealingStationTime;

	protected float instantHealingEffectTime = 1f;

	protected int maxUsedBulletWithoutReload;

	protected int usedBulletWithoutReload;

	private SceneConfig mSceneConfig;

	private int currentSceneID = -1;

	private int currentCityID = -1;

	public override int Hp
	{
		get
		{
			return AntiCracking.DecryptBufferStr(hpStr, "xue");
		}
		set
		{
			hpStr = AntiCracking.CryptBufferStr(value, "xue");
		}
	}

	public override int Shield
	{
		get
		{
			return AntiCracking.DecryptBufferStr(shieldStr, "dun");
		}
		set
		{
			shieldStr = AntiCracking.CryptBufferStr(value, "dun");
		}
	}

	public bool IsInMorphine { get; set; }

	public int MaxUsedBulletWithoutReload
	{
		get
		{
			return maxUsedBulletWithoutReload;
		}
	}

	public int UsedBulletWithoutReload
	{
		get
		{
			return usedBulletWithoutReload;
		}
		set
		{
			usedBulletWithoutReload = value;
			if (usedBulletWithoutReload >= maxUsedBulletWithoutReload)
			{
				maxUsedBulletWithoutReload = usedBulletWithoutReload;
			}
		}
	}

	public LocalPlayer()
	{
		userState = GameApp.GetInstance().GetUserState();
	}

	public override void SetObject(GameObject obj)
	{
		base.SetObject(obj);
		mEntityLocalPosition = entityTransform.Find("Entity").localPosition;
	}

	public override void Init()
	{
		base.Init();
		base.inputController.Init();
		base.aimAssistController.Init();
		base.cameraVibrateController.Init();
		sendingTimer.SetTimer(0.2f, false);
		sendingShootDirTimer.SetTimer(0.5f, false);
		networkMgr = GameApp.GetInstance().GetNetworkManager();
		SetUserID(Lobby.GetInstance().GetChannelID());
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			base.MaxHp = VSMath.GetHpInVS(base.MaxHp);
		}
		mDisplayName = userState.GetRoleName();
		Hp = base.MaxHp;
		Shield = base.MaxShield;
		base.MeleeATK = 25;
		BasicMeleeATK = base.MeleeATK;
		BasicCriticalDamage = 150;
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
		}
		hpRecoveryTimer.SetTimer(1f, false);
		bulletRecoveryTimer.SetTimer(10f, false);
		uploadScoreTimer.SetTimer(30f, false);
		if (userState.Enegy < 500)
		{
			userState.Enegy = 500;
		}
		mNeedLevelUpSfx = false;
		BasicDropRate = 1f;
		base.DropRate = BasicDropRate;
		mDotTimer.SetTimer(1f, false);
		ItemNewbie1 itemNewbie = (ItemNewbie1)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
			.GetGlobalIAPItem(IAPItemState.ItemType.Newbie1);
		itemNewbie.CheckItem();
		ItemNewbie2 itemNewbie2 = (ItemNewbie2)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
			.GetGlobalIAPItem(IAPItemState.ItemType.Newbie2);
		itemNewbie2.CheckItem();
	}

	public void CreateNavMeshAgent()
	{
		if (agent == null)
		{
			entityObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			agent = entityObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			agent.speed = base.Speed;
			agent.angularSpeed = 360f;
			agent.stoppingDistance = 2f;
			agent.acceleration = 1000f;
			agent.enabled = false;
			agent.updateRotation = false;
		}
	}

	public void AutoMoveToWithNav(Vector3 pos)
	{
		agent.enabled = true;
		agent.SetDestination(pos);
	}

	public void StopWithNav()
	{
		agent.Stop();
		agent.enabled = false;
	}

	public void RestoreEntityLocalPosition()
	{
		if (m_tweenPos != null)
		{
			m_tweenPos.enabled = false;
		}
		entityTransform.Find("Entity").transform.localPosition = mEntityLocalPosition;
	}

	public void SetEntityLocalPositionInDying()
	{
		entityTransform.Find("Entity").transform.localPosition = new Vector3(0f, Player.FALL_DOWN_OFFSET, 0f);
	}

	public void OnLevelUpInMenu()
	{
		mNeedLevelUpSfx = true;
	}

	public override void ChangeWeaponInBag(int bagIndex)
	{
		Weapon weapon = null;
		Weapon weapon2 = base.weapon;
		if (bagIndex >= weaponList.Count)
		{
			return;
		}
		weapon = weaponList[bagIndex];
		if (weapon == null || weapon == weapon2)
		{
			return;
		}
		if (base.InAimState)
		{
			Aim(false);
		}
		RemoveOrResetBulletRecoverTimeByWeaponType(weapon2.GetWeaponType());
		if (weapon != null)
		{
			userState.ItemInfoData.CurrentEquipWeaponSlot = (byte)bagIndex;
			weapon.StopFire();
			weapon.AutoDestructEffect();
			if (base.InAimState)
			{
				Aim(false);
			}
			SetWeaponToChange(weapon);
			PlayAnimation(GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponDown, WrapMode.ClampForever);
			SetState(Player.SWITCH_WEAPON_DOWN_STATE);
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			weapon.GetElementTypeAndPara();
			ElementType element = weapon.mCurrentElementType;
			if (weapon.IsAllElement())
			{
				element = ElementType.AllElement;
			}
			PlayerChangeWeaponRequest request = new PlayerChangeWeaponRequest((byte)bagIndex, element);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void RefreshAvatar()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerChangeAvatarRequest request = new PlayerChangeAvatarRequest(GetUserState());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void Loop(float deltaTime)
	{
		base.Loop(deltaTime);
		base.inputController.Process();
		base.aimAssistController.Process();
		base.cameraVibrateController.Process();
		if (weapon.GetWeaponType() != lastWT)
		{
			lastWT = weapon.GetWeaponType();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			SendShootDir();
			SendInput();
		}
		base.State.NextState(this, deltaTime);
		DYING_STATE.NextState(this, deltaTime);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && InPlayingState() && !InDyingState() && !InFallDownState())
		{
			SendTransform();
		}
		if (CanRecoverHPState())
		{
			CheckHPRecovery();
		}
		if (CanRecoverShieldState())
		{
			DoShieldRecovery(deltaTime);
		}
		if (CanRecoverBulletState())
		{
			CheckBulletRecovery();
		}
		if (base.ExtraShield > 0)
		{
			CheckExtraShieldDuration();
		}
		weapon.RecoverRecoil(deltaTime);
		if (uploadScoreTimer.Ready())
		{
			uploadScoreTimer.Do();
		}
		FirstPersonCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
		if (camera != null)
		{
			base.AngleV = camera.AngelV;
		}
		if (mNeedLevelUpSfx && Time.timeScale != 0f)
		{
			AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_levelup");
			mNeedLevelUpSfx = false;
		}
		if (IsInMorphine)
		{
			if (userState.GetSex() == Sex.M)
			{
				AudioManager.GetInstance().PlaySoundSingleLoop("RPG_Audio/Player/Sniper_M_Skill");
			}
			else
			{
				AudioManager.GetInstance().PlaySoundSingleLoop("RPG_Audio/Player/Sniper_F_Skill");
			}
		}
		if (IsInstantHealing && Time.time - startInstantHealingStationTime >= instantHealingEffectTime)
		{
			RemoveHealingEffect();
		}
		if (InPlayingState())
		{
			DoDot();
		}
	}

	public void CheckHPRecovery()
	{
		if (hpRecoveryTimer.Ready() && InPlayingState())
		{
			int hp = Hp;
			if (hp < base.MaxHp)
			{
				if (hp + base.HpRecoverValueByShield > base.MaxHp)
				{
					hpRecoverValueByShield = base.MaxHp - hp;
				}
				AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Hit_me_please, AchievementTrigger.Type.Data);
				achievementTrigger.PutData(base.HpRecoverValueByShield);
				AchievementManager.GetInstance().Trigger(achievementTrigger);
			}
			RecoverHP(base.HpRecoverValueByShield + base.HpRecoverValueByChip + base.HpRecoverValueByGrenade, false);
			hpRecoveryTimer.Do();
		}
		CheckHpRecoveryBuff();
	}

	public void CheckExtraShieldDuration()
	{
		if (extraShieldDurationTimer.Ready() && InPlayingState())
		{
			ClearExtraShield();
		}
	}

	public void CheckBulletRecovery()
	{
		if (bulletRecoveryTimer.Ready() && InPlayingState())
		{
			userState.AddBulletByWeaponType(weapon.GetWeaponType(), (short)Mathf.CeilToInt((float)(userState.GetMaxBulletByWeaponType(weapon.GetWeaponType()) * base.BulletRecoverValueByChip) / 100f));
			bulletRecoveryTimer.Do();
		}
		CheckBulletRecoveryBuff();
	}

	public void CheckBulletRecoveryBuff()
	{
		for (int i = 0; i < bulletRecoverTimers.Count; i++)
		{
			if (bulletRecoverTimers[i].timer.Ready())
			{
				if (bulletRecoverTimers[i].wTypes.Contains(SkillTargetType.AllWeapon) || bulletRecoverTimers[i].wTypes.Contains((SkillTargetType)weapon.GetWeaponType()) || bulletRecoverTimers[i].wTypes[0] == SkillTargetType.None)
				{
					userState.AddBulletByWeaponType(weapon.GetWeaponType(), (short)Mathf.CeilToInt((float)(userState.GetMaxBulletByWeaponType(weapon.GetWeaponType()) * bulletRecoverTimers[i].RecoverPercentageTick) / 100f));
				}
				bulletRecoverTimers[i].TriggeredCount++;
				bulletRecoverTimers[i].timer.Do();
			}
		}
		RemoveTimeOverTimers();
	}

	public void CheckHpRecoveryBuff()
	{
		for (int i = 0; i < hpRecoverTimers.Count; i++)
		{
			if (hpRecoverTimers[i].timer.Ready())
			{
				RecoverHP((float)base.MaxHp * hpRecoverTimers[i].RecoverPercentage);
				hpRecoverTimers[i].TriggeredCount++;
				hpRecoverTimers[i].timer.Do();
			}
		}
		RemoveTimeOverTimers();
	}

	public void RecoverShiled(float recovery)
	{
		int shield = Shield;
		shield += (int)recovery;
		shield = Mathf.Clamp(shield, 0, base.MaxShield);
		Shield = shield;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerShieldRecoveryRequest request = new PlayerShieldRecoveryRequest(0, (short)recovery, (short)base.MaxShield);
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			networkMgr.SendRequest(request);
		}
	}

	public void RecoverHP(float recoveryRate)
	{
		RecoverHP(recoveryRate, true);
	}

	public void RecoverHP(float recoveryRate, bool needHealingEffect)
	{
		recoveryRate += detailedProperties.HpRecoveryBonus;
		recoveryRate *= 1f + detailedProperties.HpRecoveryPercentageBonus;
		int hp = Hp;
		int num2 = (Hp = (int)Mathf.Clamp(recoveryRate + (float)hp, 0f, base.MaxHp));
		if (needHealingEffect && recoveryRate > 0f && HealingEffect == null)
		{
			IsInstantHealing = true;
			startInstantHealingStationTime = Time.time;
			HealingEffect = EffectPlayer.GetInstance().PlayHealingEffect();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerHpRecoveryRequest request = new PlayerHpRecoveryRequest(0, (short)recoveryRate, (short)base.MaxHp);
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			networkMgr.SendRequest(request);
		}
		if (num2 == 1)
		{
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
		}
		else
		{
			AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Last_Blood, AchievementTrigger.Type.Stop);
			AchievementManager.GetInstance().Trigger(trigger2);
		}
	}

	public void DamageToHealth(int damage)
	{
		int num = (int)((float)(damage * base.DamageToHealthPercentage) * 0.01f);
		RecoverHP(num);
	}

	public void SendInput()
	{
		InputInfo inputInfo = base.inputController.inputInfo;
		InputInfo previousInputInfo = base.inputController.previousInputInfo;
		bool flag = inputInfo.fire;
		if (!weapon.CheckBullets())
		{
			flag = false;
		}
		if (weapon.GetWeaponType() == WeaponType.LaserGun)
		{
			LaserCannon laserCannon = (LaserCannon)weapon;
			if (laserCannon != null && laserCannon.IsOverHeat)
			{
				flag = false;
			}
		}
		if (base.State != Player.IDLE_STATE && base.State != Player.ATTACK_STATE)
		{
			flag = false;
		}
		if (inputInfo.IsMoving() != previousInputInfo.IsMoving() || flag != previousInputInfo.fire)
		{
			SendPlayerInputRequest request = new SendPlayerInputRequest(flag, inputInfo.IsMoving());
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			previousInputInfo.moveDirection = inputInfo.moveDirection;
			previousInputInfo.fire = flag;
		}
	}

	public void SendTransform()
	{
		if (sendingTimer.Ready() && TimeManager.GetInstance().NetworkTimeSynchronized)
		{
			if (lastTrans.Pos != entityTransform.position || lastTrans.Angle != entityTransform.eulerAngles.y)
			{
				SendTransformStateRequest request = new SendTransformStateRequest(entityTransform.position, entityTransform.eulerAngles);
				networkMgr = GameApp.GetInstance().GetNetworkManager();
				networkMgr.SendRequest(request);
				lastTrans.Pos = entityTransform.position;
				lastTrans.Angle = entityTransform.eulerAngles.y;
			}
			sendingTimer.Do();
		}
	}

	public void SendShootDir()
	{
		if (sendingShootDirTimer.Ready())
		{
			InputInfo inputInfo = base.inputController.inputInfo;
			short angleV = (short)GameApp.GetInstance().GetGameScene().GetCamera()
				.AngelV;
			SendPlayerShootAngleVRequest request = new SendPlayerShootAngleVRequest(angleV);
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			networkMgr.SendRequest(request);
			sendingShootDirTimer.Do();
		}
	}

	public override void PlayAnimation(string name, WrapMode mode)
	{
		base.PlayAnimation(name, mode);
	}

	public override bool IsLocal()
	{
		return true;
	}

	public override void OnDead()
	{
		base.OnDead();
		AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.I_need_no_weapons, AchievementTrigger.Type.Stop);
		AchievementManager.GetInstance().Trigger(trigger);
		AchievementTrigger trigger2 = AchievementTrigger.Create(AchievementID.Reaper, AchievementTrigger.Type.Stop);
		AchievementManager.GetInstance().Trigger(trigger2);
		m_deadRotateAngle = 0f;
		deadAnimationTimer.SetTimer(3f, false);
		ClearDot();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerOnDeadRequest request = new PlayerOnDeadRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				deadAnimationTimer.SetTimer(15f, false);
			}
		}
	}

	public void ClearDot()
	{
		mElementDotList.Clear();
	}

	public override bool CheckLose()
	{
		return rebirthTimer.Ready();
	}

	public override bool StartWaitRebirth()
	{
		return false;
	}

	public override bool FallDownCompleted()
	{
		return fallDownTimer.Ready();
	}

	public override bool DeadAnimationCompleted()
	{
		return deadAnimationTimer.Ready();
	}

	public override bool WinAnimationCompleted()
	{
		return winAnimationTimer.Ready();
	}

	public void AddStreamingVolume(GameObject volume)
	{
		if (!mStreamingVolumeList.Contains(volume))
		{
			mStreamingVolumeList.Add(volume);
		}
	}

	public void RemoveStreamingVolume(GameObject volume)
	{
		if (mStreamingVolumeList.Contains(volume))
		{
			mStreamingVolumeList.Remove(volume);
		}
	}

	public bool HasStreamingVolume(GameObject volume)
	{
		return mStreamingVolumeList.Contains(volume);
	}

	public void ClearStreamingVolume()
	{
		mStreamingVolumeList.Clear();
	}

	public void SendRebirthRequest()
	{
		if (base.SendingRebirthRequest)
		{
			return;
		}
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		float num4 = 0f;
		float num5 = 0f;
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		int num6 = Random.Range(0, array.Length);
		List<GameObject> list = new List<GameObject>();
		for (int i = num6; i < array.Length; i++)
		{
			list.Add(array[i]);
		}
		for (int j = 0; j < num6; j++)
		{
			list.Add(array[j]);
		}
		for (int k = 0; k < list.Count; k++)
		{
			if (!list[k].name.StartsWith(ObjectNamePrefix.PLAYER_SPAWN_POINT))
			{
				continue;
			}
			Vector3 vector = list[k].transform.position + new Vector3(0f, 1f, 0f);
			float num7 = 9999f;
			bool flag = false;
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item == null || !item.InPlayingState() || IsSameTeam(item))
				{
					continue;
				}
				Vector3 vector2 = item.GetTransform().position + new Vector3(0f, 1f, 0f);
				Vector3 direction = vector2 - vector;
				float sqrMagnitude = new Vector3(direction.x, 0f, direction.z).sqrMagnitude;
				if (sqrMagnitude < num7)
				{
					num7 = sqrMagnitude;
				}
				if (!flag && sqrMagnitude < 2025f)
				{
					Ray ray = new Ray(vector, direction);
					float magnitude = direction.magnitude;
					RaycastHit hitInfo;
					if (!Physics.Raycast(ray, out hitInfo, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (num7 > num4)
				{
					num2 = k;
					num4 = num7;
				}
				continue;
			}
			if (num7 < 225f)
			{
				if (num7 > num5)
				{
					num3 = k;
					num5 = num7;
				}
				continue;
			}
			num = k;
			break;
		}
		if (num == -1)
		{
			num = num3;
		}
		if (num == -1)
		{
			num = num2;
		}
		if (num == -1)
		{
			num = 0;
		}
		num = (num + num6) % array.Length;
		PlayerRebirthRequest request = new PlayerRebirthRequest((byte)num);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		base.SendingRebirthRequest = true;
	}

	public void AddBulletRecoverTimer(BulletRecoverTimer timer)
	{
		RemoveRecoverTimersByID(timer.SkillID);
		bulletRecoverTimers.Add(timer);
		bulletRecoverTimers[bulletRecoverTimers.Count - 1].timer.Do();
	}

	public void AddHpRecoverTimer(HpRecoverTimer timer)
	{
		RemoveRecoverTimersByID(timer.SkillID);
		hpRecoverTimers.Add(timer);
		hpRecoverTimers[hpRecoverTimers.Count - 1].timer.Do();
	}

	public void RemoveRecoverTimersByID(short skillID)
	{
		for (int i = 0; i < bulletRecoverTimers.Count; i++)
		{
			if (bulletRecoverTimers[i].SkillID == skillID)
			{
				bulletRecoverTimers.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < hpRecoverTimers.Count; j++)
		{
			if (hpRecoverTimers[j].SkillID == skillID)
			{
				hpRecoverTimers.RemoveAt(j);
				j--;
			}
		}
	}

	public void RemoveOrResetBulletRecoverTimeByWeaponType(WeaponType wType)
	{
		for (int i = 0; i < bulletRecoverTimers.Count; i++)
		{
			if (bulletRecoverTimers[i].wTypes.Contains(SkillTargetType.AllWeapon))
			{
				bulletRecoverTimers[i].timer.Do();
			}
			else if (bulletRecoverTimers[i].wTypes.Contains((SkillTargetType)wType))
			{
				bulletRecoverTimers.RemoveAt(i);
				i--;
			}
		}
	}

	public void RemoveTimeOverTimers()
	{
		for (int i = 0; i < bulletRecoverTimers.Count; i++)
		{
			if (bulletRecoverTimers[i].TriggeredCount >= bulletRecoverTimers[i].TotalCount)
			{
				bulletRecoverTimers.RemoveAt(i);
				i--;
			}
		}
		for (int j = 0; j < hpRecoverTimers.Count; j++)
		{
			if (hpRecoverTimers[j].TriggeredCount >= hpRecoverTimers[j].TotalCount)
			{
				hpRecoverTimers.RemoveAt(j);
				j--;
			}
		}
	}

	protected override void DoShieldRecovery(float _deltaTime)
	{
		if (!mShieldRecoveryStartTimer.Ready())
		{
			return;
		}
		int shield = Shield;
		if (shield == base.MaxShield)
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			shieldRecoveredDeltaTime += _deltaTime;
			if ((float)base.ShieldRecovery * shieldRecoveredDeltaTime > 1f)
			{
				shield += (int)((float)base.ShieldRecovery * shieldRecoveredDeltaTime);
				shieldRecoveredDeltaTime = 0f;
				if (shield > base.MaxShield)
				{
					shield = base.MaxShield;
				}
				Shield = shield;
			}
		}
		else
		{
			SendShieldRecoveryRequest();
		}
	}

	protected override void ResetShieldRecoveryStartTimer()
	{
		mShieldRecoveryStartTimer.SetTimer(base.ShieldRecoveryDelay, true);
		base.ResetShieldRecoveryStartTimer();
	}

	protected override void SendShieldRecoveryRequest()
	{
		if (mShieldRecoverySecondTimer.Ready())
		{
			mShieldRecoverySecondTimer.Do();
			PlayerShieldRecoveryRequest request = new PlayerShieldRecoveryRequest(0, (short)base.ShieldRecovery, (short)base.MaxShield);
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			networkMgr.SendRequest(request);
			int shield = Shield;
			shield += base.ShieldRecovery;
			if (shield > base.MaxShield)
			{
				shield = base.MaxShield;
			}
			Shield = shield;
		}
	}

	public void CreateExtraShield(int extraShield, float duration)
	{
		extraShieldDurationTimer.SetTimer(duration, false);
		base.inputController.Block = true;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			CreateExtraShieldRequest request = new CreateExtraShieldRequest(extraShield);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else
		{
			CreateExtraShieldWithEffect(extraShield);
		}
	}

	public void ClearExtraShield()
	{
		extraShieldDurationTimer.Disable();
		base.inputController.Block = false;
		if (HealingEffect != null && !IsInstantHealing)
		{
			RemoveHealingEffect();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			ClearExtraShieldRequest request = new ClearExtraShieldRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else
		{
			ClearExtraShieldWithEffect();
		}
	}

	public bool CanUnEquipWeapon(UIMsgListener msgListener)
	{
		int num = 0;
		foreach (Weapon weapon in GetWeaponList())
		{
			if (weapon != null)
			{
				num++;
			}
		}
		Debug.Log(num);
		if (num <= 1)
		{
			UIMsgBox.instance.ShowMessage(msgListener, LocalizationManager.GetInstance().GetString("MSG_LAST_WEAPON"), 2);
			Debug.Log("Only One Weapon Equipped, you can't take it off any more!");
			return false;
		}
		return true;
	}

	public override void BeginFallDownState()
	{
		StopSpecialAction();
		if (base.InAimState)
		{
			Aim(false);
		}
		SetState(Player.FALL_DOWN_STATE);
		AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_fall");
		fallDownTimer.SetTimer(0.7f, false);
		GameObject gameObject = entityTransform.Find("Entity").gameObject;
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0f, gameObject.transform.localPosition.z);
		if (m_tweenPos == null)
		{
			m_tweenPos = gameObject.AddComponent<TweenPosition>();
		}
		m_tweenPos.delay = 0f;
		m_tweenPos.duration = Player.FALL_DOWN_DURATION;
		m_tweenPos.method = UITweener.Method.EaseOut;
		m_tweenPos.style = UITweener.Style.Once;
		m_tweenPos.eventReceiver = null;
		m_tweenPos.callWhenFinished = null;
		m_tweenPos.onFinished = null;
		m_tweenPos.enabled = true;
		m_tweenPos.from = gameObject.transform.localPosition;
		m_tweenPos.to = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + Player.FALL_DOWN_OFFSET, gameObject.transform.localPosition.z);
		m_tweenPos.Play(true);
	}

	public SceneConfig GetCurrentSceneConfig()
	{
		if (GameApp.GetInstance().GetGameWorld().CurrentSceneID != currentSceneID || GameApp.GetInstance().GetUserState().GetCurrentCityID() != currentCityID)
		{
			currentSceneID = GameApp.GetInstance().GetGameWorld().CurrentSceneID;
			currentCityID = GameApp.GetInstance().GetUserState().GetCurrentCityID();
			Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
			foreach (SceneConfig value in sceneConfig.Values)
			{
				if (GameApp.GetInstance().GetGameWorld().CurrentSceneID == value.SceneID && userState.GetCurrentCityID() == value.AreaID)
				{
					mSceneConfig = value;
					Debug.Log("CurrentScene : " + mSceneConfig.SceneName);
					break;
				}
			}
		}
		return mSceneConfig;
	}

	public void ReturnToCity()
	{
		int fatherSceneID = GameApp.GetInstance().GetGameWorld().GetSceneConfig(GameApp.GetInstance().GetGameWorld().CurrentSceneID)
			.FatherSceneID;
		SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(fatherSceneID);
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		gameScene.LeaveScene();
		Application.LoadLevel(sceneConfig.SceneFileName);
	}

	public override void ShowShieldBreak()
	{
		EffectPlayer.GetInstance().PlayLocalPlayerShieldBreak();
		AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_shield_depleted");
	}

	public override void LoadSceneOnDead()
	{
		if (CurrentRespawnPoint != null)
		{
			RespawnStreamingScript component = CurrentRespawnPoint.GetComponent<RespawnStreamingScript>();
			if (null != component)
			{
				LoadStreamingVolume(component);
			}
		}
	}

	public override void LoadSceneOfBoss()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag(TagName.BOSS_STREAMING_VOLUME);
		if (null != gameObject)
		{
			RespawnStreamingScript component = gameObject.GetComponent<RespawnStreamingScript>();
			if (null != component)
			{
				LoadStreamingVolume(component);
			}
		}
	}

	protected void LoadStreamingVolume(RespawnStreamingScript script)
	{
		List<GameObject> list = new List<GameObject>();
		List<GameObject> list2 = new List<GameObject>();
		foreach (GameObject mStreamingVolume in mStreamingVolumeList)
		{
			if (!script.StreamingVolumes.Contains(mStreamingVolume))
			{
				list.Add(mStreamingVolume);
			}
		}
		foreach (GameObject streamingVolume in script.StreamingVolumes)
		{
			if (!mStreamingVolumeList.Contains(streamingVolume))
			{
				list2.Add(streamingVolume);
			}
		}
		foreach (GameObject item in list2)
		{
			SceneStreamingTriggerScript component = item.GetComponent<SceneStreamingTriggerScript>();
			if (null != component)
			{
				component.Load();
			}
		}
		foreach (GameObject item2 in list)
		{
			SceneStreamingTriggerScript component2 = item2.GetComponent<SceneStreamingTriggerScript>();
			if (null != component2)
			{
				component2.Unload();
			}
		}
	}

	public void RemoveHealingEffect()
	{
		IsInstantHealing = false;
		Object.Destroy(HealingEffect);
		HealingEffect = null;
	}

	public bool CheckIfOnGround()
	{
		if (entityTransform == null)
		{
			return true;
		}
		Ray ray = new Ray(entityTransform.position + Vector3.up * 0.2f, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 2f, 1 << PhysicsLayer.FLOOR))
		{
			return true;
		}
		ray = new Ray(entityTransform.position + Vector3.up * 100f, Vector3.down);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.FLOOR))
		{
			entityTransform.position = hitInfo.point + Vector3.up * 0.2f;
			return false;
		}
		return true;
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
		int[] array5 = new int[3];
		if (mElementDotList.Count > 0)
		{
			ElementDotData elementDotData = (ElementDotData)mElementDotList[0];
			array[(int)(elementDotData.type - 2)] += elementDotData.damage;
			array2[(int)(elementDotData.type - 2)] = elementDotData.isPenetration;
			array3[(int)(elementDotData.type - 2)] = elementDotData.unitLevel;
			array4[(int)(elementDotData.type - 2)] = elementDotData.weaponLevel;
			array5[(int)(elementDotData.type - 2)] = elementDotData.attackerID;
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
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					int num2 = array[i];
					PlayerHitPlayerRequest request = new PlayerHitPlayerRequest(array5[i], (short)num2, GetUserID(), array2[i], (byte)(i + 2), false, false, 0f, 0, WeaponType.NoGun, DamageProperty.AttackerType._Dot);
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
			damageProperty.damage = array[i];
			damageProperty.criticalAttack = false;
			damageProperty.isPenetration = array2[i];
			damageProperty.unitLevel = array3[i];
			damageProperty.weaponLevel = array4[i];
			damageProperty.wType = WeaponType.NoGun;
			damageProperty.attackerType = DamageProperty.AttackerType._Dot;
			OnHit(damageProperty.damage, null);
		}
	}

	public void UpdateAllDamageParas()
	{
		UpdateDamagePara(DamagePara.DamageImmunityRate);
		UpdateDamagePara(DamagePara.DamageReduction);
		UpdateDamagePara(DamagePara.ElementResistanceFire);
		UpdateDamagePara(DamagePara.ElementResistanceShock);
		UpdateDamagePara(DamagePara.ElementResistanceCorrosive);
		UpdateDamagePara(DamagePara.ElementResistanceExplosion);
	}

	public void UpdateDamagePara(DamagePara para)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			int value = 0;
			switch (para)
			{
			case DamagePara.DamageImmunityRate:
				value = Mathf.CeilToInt(base.DamageImmunityRateBonus * 100f);
				break;
			case DamagePara.DamageReduction:
				value = base.DamageReduction;
				break;
			case DamagePara.ElementResistanceFire:
				value = GetElementResistance(ElementType.Fire);
				break;
			case DamagePara.ElementResistanceShock:
				value = GetElementResistance(ElementType.Shock);
				break;
			case DamagePara.ElementResistanceCorrosive:
				value = GetElementResistance(ElementType.Corrosive);
				break;
			case DamagePara.ElementResistanceExplosion:
				value = base.ExplosionDamageReduction;
				break;
			}
			UpdateDamageParaRequest request = new UpdateDamageParaRequest(para, value);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void AddElementDotData(ElementType eType, int damage, int dotTime, bool isPenetration, int unitLevel, int weaponLevel, WeaponType wType, int attackerID)
	{
		ElementDotData elementDotData = default(ElementDotData);
		elementDotData.type = eType;
		elementDotData.time = dotTime;
		elementDotData.damage = damage;
		elementDotData.isPenetration = isPenetration;
		elementDotData.unitLevel = unitLevel;
		elementDotData.weaponLevel = weaponLevel;
		elementDotData.weaponType = wType;
		elementDotData.attackerID = attackerID;
		mElementDotList.Clear();
		mElementDotList.Add(elementDotData);
		mDotTimer.Do();
	}
}
