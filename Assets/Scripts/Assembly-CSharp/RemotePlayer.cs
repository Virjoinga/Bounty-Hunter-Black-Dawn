using System.Collections.Generic;
using UnityEngine;

public class RemotePlayer : Player
{
	protected NetworkTransformInterpolation interpolation;

	protected float startFallDownTime;

	public RemotePlayer()
	{
		interpolation = new NetworkTransformInterpolation(this);
		userState = new UserState();
	}

	public NetworkTransformInterpolation GetInterpolation()
	{
		return interpolation;
	}

	public void UpdateTransform(Vector3 pos, float eulerAnglesY, int timeStamp, Transform transform)
	{
		bool run = true;
		if (base.inputController.inputInfo.moveDirection == Vector3.zero)
		{
			run = false;
		}
		NetworkTransform nTrans = new NetworkTransform(pos, eulerAnglesY, timeStamp, run);
		interpolation.SetTransform(transform);
		interpolation.ReceiveTransform(nTrans);
	}

	public override void Loop(float deltaTime)
	{
		if (base.CurrentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && base.CurrentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID && !(entityObject == null) && entityObject.activeSelf)
		{
			base.Loop(deltaTime);
			base.State.NextState(this, deltaTime);
			DYING_STATE.NextState(this, deltaTime);
			interpolation.Loop();
			base.AngleV = Mathf.LerpAngle(base.AngleV, base.TargetAngleV, deltaTime * 10f);
		}
	}

	public void CreateUserState(byte[] armorIDs, byte characterClass, byte sex, short characterLevel, byte avatarID)
	{
		userState.SetCharacterClass((CharacterClass)characterClass);
		userState.SetSex((Sex)sex);
		userState.SetCharLevel(characterLevel);
		userState.SetDecoration(armorIDs);
		Debug.Log("characterLevel:" + characterLevel);
		userState.InitDeafaultEquips();
		userState.SetAvatar(avatarID);
	}

	public override void Init()
	{
		base.Init();
		entityObject.name = userID.ToString();
	}

	public override void OnDead()
	{
		PlayAnimation(AnimationString.Dead, WrapMode.ClampForever);
		if (userState.GetSex() == Sex.M)
		{
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Player/player_killed_M", entityTransform.position);
		}
		else
		{
			AudioManager.GetInstance().PlaySoundAt("RPG_Audio/Player/player_killed_F", entityTransform.position);
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			CreateDeadBlood();
		}
	}

	private void ClearWeaponPrefabAndAnimation()
	{
		ClearWeaponList();
		List<string> list = new List<string>();
		foreach (AnimationState item in animationObject.GetComponent<Animation>())
		{
			if (!(item.clip.name == "Take 001"))
			{
				list.Add(item.clip.name);
			}
		}
		foreach (string item2 in list)
		{
			animationObject.GetComponent<Animation>().RemoveClip(item2);
		}
		list.Clear();
	}

	public bool RefreshWeaponList(WeaponInfo[] list, byte currentWeaponIndex)
	{
		ClearWeaponPrefabAndAnimation();
		if (base.CurrentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && base.CurrentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID)
		{
			for (int i = 0; i < list.Length; i++)
			{
				WeaponInfo weaponInfo = list[i];
				if (weaponInfo.mWeaponType != 0)
				{
					Weapon item = WeaponFactory.GetInstance().CreateThirdPersonWeapon(weaponInfo.mWeaponType, weaponInfo.mWeaponNameNumber);
					weaponList.Add(item);
				}
				else
				{
					weaponList.Add(null);
				}
			}
			AvatarBuilder.GetInstance().AddAnimationsForThirdPerson(animationObject, this);
			foreach (Weapon weapon in weaponList)
			{
				if (weapon != null)
				{
					weapon.Init(this);
				}
			}
			int num = -1;
			for (int j = 0; j < weaponList.Count; j++)
			{
				if (weaponList[j] != null)
				{
					num = j;
					break;
				}
			}
			if (weaponList.Count == 0 || num == -1)
			{
				return false;
			}
			if (currentWeaponIndex >= weaponList.Count || weaponList[currentWeaponIndex] == null)
			{
				ChangeWeaponInBag(num);
			}
			else
			{
				ChangeWeaponInBag(currentWeaponIndex);
			}
		}
		return true;
	}

	public override void ChangeWeaponInBag(int bagIndex)
	{
		if (bagIndex < weaponList.Count)
		{
			Weapon weapon = weaponList[bagIndex];
			if (weapon != null)
			{
				ChangeWeapon(weapon);
				currentBagIndex = bagIndex;
			}
		}
	}

	public override void RefreshAvatar()
	{
		AvatarBuilder.GetInstance().ChangeDecorations(GetObject(), GetUserState().GetRoleState());
	}

	public void InitControllableItem(byte id, byte level, byte type, byte subType, int hp, int shield, Vector3 position, Quaternion rotation, short para1, short para2, short para3, short para4, short para5, short para6, short para7, short para8, short para9, short para10, short para11, short para12, short para13, short para14, short para15, short para16)
	{
		ControllableUnit controllableUnit = null;
		if (type == 0)
		{
			switch ((ESummonedType)subType)
			{
			case ESummonedType.ENGINEER_GUN:
				controllableUnit = new EngineerGun();
				break;
			case ESummonedType.HEALING_STATION:
				controllableUnit = new HealingStation();
				break;
			case ESummonedType.TRAPS:
				controllableUnit = new Traps();
				break;
			}
		}
		if (controllableUnit == null)
		{
			return;
		}
		controllableUnit.ID = id;
		controllableUnit.Level = level;
		controllableUnit.MaxHp = 100 * controllableUnit.Level;
		controllableUnit.Hp = hp;
		controllableUnit.MaxShield = 100 * controllableUnit.Level;
		controllableUnit.Shield = shield;
		controllableUnit.ShieldRecovery = 10;
		controllableUnit.Position = position;
		controllableUnit.Rotation = rotation;
		if (controllableUnit.ControllableType == EControllableType.SUMMONED)
		{
			SummonedItem summonedItem = controllableUnit as SummonedItem;
			if (summonedItem != null)
			{
				summonedItem.IsMaster = false;
				summonedItem.SetOwnerPlayer(this);
			}
		}
		controllableUnit.Init();
		controllableUnit.InitValuesAndRanges(para1, para2, para3, para4, para5, para6, para7, para8, para9, para10, para11, para12, para13, para14, para15, para16);
	}

	public void RecoveHP(float recoveryRate)
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			RemotePlayerHpRecoveryRequest request = new RemotePlayerHpRecoveryRequest(GetUserID(), (short)recoveryRate);
			Debug.Log("UserID = " + GetUserID() + "++++Recover :" + recoveryRate);
			NetworkManager networkManager = GameApp.GetInstance().GetNetworkManager();
			networkManager.SendRequest(request);
		}
	}

	public override void BeginFallDownState()
	{
		if (InSameScene())
		{
			SetState(Player.FALL_DOWN_STATE);
			PlayAnimationAllLayers(GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
			startFallDownTime = Time.time;
		}
	}

	public bool FallDownFinish()
	{
		return Time.time - startFallDownTime > Player.FALL_DOWN_DURATION;
	}

	public void StartMeleeAttack()
	{
		PlayAnimationAllLayers(AnimationString.MeleeAttack, WrapMode.ClampForever);
		mWeapon.gameObject.SetActive(false);
		Knife.SetActive(true);
		SetState(Player.MELEE_ATTACK_STATE);
	}

	public override void CancelMeleeAttack()
	{
		Knife.SetActive(false);
		mWeapon.gameObject.SetActive(true);
	}

	public override void StopSpecialAction()
	{
		if (base.State == Player.MELEE_ATTACK_STATE)
		{
			CancelMeleeAttack();
		}
	}

	public new void SetCurrentCityAndSceneID(byte cityID, byte sceneID)
	{
		base.CurrentCityID = cityID;
		base.CurrentSceneID = sceneID;
		UserStateHUD.GetInstance().AddRemotePlayer(this);
	}

	public override void ShowShieldBreak()
	{
	}

	public bool InSameScene()
	{
		return GameApp.GetInstance().GetUserState().GetCurrentCityID() == base.CurrentCityID && gameWorld.CurrentSceneID == base.CurrentSceneID;
	}

	public void ApplySkillsForAttacker(int attackerID, bool isCritical, WeaponType wType, DamageProperty.AttackerType attackerType)
	{
		if (attackerID != Lobby.GetInstance().GetChannelID())
		{
			return;
		}
		Player localPlayer = gameWorld.GetLocalPlayer();
		if (attackerType == DamageProperty.AttackerType._PlayerOrEnemy)
		{
			localPlayer.GetCharacterSkillManager().OnHitEnemyTrigger(localPlayer, wType, this);
		}
		if (Hp <= 0)
		{
			localPlayer.GetCharacterSkillManager().OnEnemyKillTrigger(localPlayer);
			if (isCritical)
			{
				localPlayer.GetCharacterSkillManager().OnEnemyCriticalKillTrigger(localPlayer);
			}
		}
	}
}
