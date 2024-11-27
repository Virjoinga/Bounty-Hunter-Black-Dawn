using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserState
{
	public class RoleState
	{
		public string roleName = string.Empty;

		protected string cash;

		protected string exp;

		public byte[] decoration = new byte[Global.DECORATION_PART_NUM];

		public int weaponId;

		public WeaponType weaponType;

		public short currentQuest;

		public short[] Bullets = new short[8];

		public short[] MaxBullets = new short[8];

		public byte[] decorationHead = new byte[Global.TOTAL_ARMOR_HEAD_NUM];

		public byte[] decorationFace = new byte[Global.TOTAL_ARMOR_FACE_NUM];

		public byte[] decorationWaist = new byte[Global.TOTAL_ARMOR_WAIST_NUM];

		public byte[] BulletLevels = new byte[8];

		public byte[] MaxBulletLevels = new byte[8];

		public byte[] BulletPriceFactors = new byte[8];

		public byte[] BulletInMags = new byte[8];

		public byte[] avatars = new byte[Global.TOTAL_AVATAR_NUM];

		public byte avatarID;

		public byte lastPortalIndex;

		public bool gambelUsed;

		public int vsTDMStatsId;

		public CharacterClass CharClass { get; set; }

		public Sex SexType { get; set; }

		public short CharLevel { get; set; }

		public RoleState()
		{
			for (int i = 0; i < 7; i++)
			{
				MaxBulletLevels[i] = 10;
			}
			MaxBulletLevels[7] = 5;
			BulletPriceFactors[0] = 2;
			BulletInMags[0] = 36;
			BulletPriceFactors[1] = 3;
			BulletInMags[1] = 30;
			BulletPriceFactors[2] = 1;
			BulletInMags[2] = 30;
			BulletPriceFactors[3] = 2;
			BulletInMags[3] = 12;
			BulletPriceFactors[4] = 3;
			BulletInMags[4] = 9;
			BulletPriceFactors[5] = 3;
			BulletInMags[5] = 6;
			BulletPriceFactors[6] = 4;
			BulletInMags[6] = 3;
			BulletPriceFactors[7] = 6;
			BulletInMags[7] = 1;
		}

		public void Init()
		{
			avatarID = 0;
			roleName = string.Empty;
			CharClass = CharacterClass.Soldier;
			SexType = Sex.M;
			CharLevel = 1;
			SetExp(0);
			SetCash(Convert.ToInt32("3000"));
			weaponId = 1;
			weaponType = WeaponType.SubMachineGun;
			currentQuest = 0;
			avatarID = 0;
			lastPortalIndex = 0;
			vsTDMStatsId = -1;
			InitDecoratoins(decorationHead);
			InitDecoratoins(decorationFace);
			InitDecoratoins(decorationWaist);
			avatars[0] = 2;
			for (int i = 1; i < Global.TOTAL_AVATAR_NUM; i++)
			{
				avatars[i] = 0;
			}
			for (int j = 0; j < decoration.Length; j++)
			{
				decoration[j] = 0;
			}
			gambelUsed = false;
		}

		private void InitDecoratoins(byte[] decorations)
		{
			for (int i = 0; i < decorations.Length; i++)
			{
				decorations[i] = 0;
			}
		}

		public void SetRole(string name, CharacterClass charClass, Sex sex)
		{
			roleName = name;
			CharClass = charClass;
			SexType = sex;
		}

		public string GetUIIdleAnimation(WeaponType wType)
		{
			string empty = string.Empty;
			switch (wType)
			{
			case WeaponType.SubMachineGun:
			case WeaponType.AssaultRifle:
				return "SMG01_ui";
			case WeaponType.Pistol:
			case WeaponType.Revolver:
				return "revolver01_ui";
			case WeaponType.RPG:
				return "rpg01_ui";
			case WeaponType.Sniper:
				return "sniper_rifle_ui";
			case WeaponType.ShotGun:
				return "shootgun01_ui";
			default:
				return "SMG01_ui";
			}
		}

		public int GetCash()
		{
			return AntiCracking.DecryptBufferStr(cash, "no_acc");
		}

		public void SetCash(int _cash)
		{
			cash = AntiCracking.CryptBufferStr(Mathf.Min(Global.MAX_CASH, _cash), "no_acc");
		}

		public void AddCash(int _cash)
		{
			SetCash(Mathf.Min(Global.MAX_CASH, GetCash() + _cash));
		}

		public int GetExp()
		{
			return AntiCracking.DecryptBufferStr(exp, "no_add");
		}

		public void SetExp(int _exp)
		{
			exp = AntiCracking.CryptBufferStr(_exp, "no_add");
		}

		public int AddExp(int _exp)
		{
			int result = Mathf.Min(Exp.RequiredLevelUpExp[Global.MAX_CHAR_LEVEL], GetExp() + _exp);
			SetExp(result);
			return result;
		}

		public void SetVSTDMStatsId(int statsId)
		{
			vsTDMStatsId = statsId;
		}

		public int GetVSTDMStatsId()
		{
			return vsTDMStatsId;
		}

		public string GetUIIdleAnimation()
		{
			return GetUIIdleAnimation(weaponType);
		}
	}

	protected RoleState roleState;

	protected byte[] stageState = new byte[Global.TOTAL_STAGE];

	protected byte[] instanceState = new byte[Global.TOTAL_STAGE_INSTANCE];

	protected byte currentCityID;

	protected IBattleState[] battleStates = new IBattleState[3];

	public bool bGotoIAP;

	public QuestStateContainer m_questStateContainer = new QuestStateContainer();

	public NPCState m_npcState = new NPCState();

	public IStatistics[] m_statistics = new IStatistics[1];

	public int Enegy { get; set; }

	public OperatingInfo OperInfo { get; set; }

	public QuestInfo QuestInfo { get; set; }

	public int UserId { get; set; }

	public ItemInfo ItemInfoData { get; set; }

	public SkillTreeMgr SkillTreeManager { get; set; }

	public ArenaRewardsInfo ArenaRewards { get; set; }

	public UserState()
	{
		roleState = new RoleState();
		m_statistics = new IStatistics[1];
		m_statistics[0] = new VSTDMRank();
		battleStates = new IBattleState[3];
		battleStates[0] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_COOP_STATE);
		battleStates[1] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_TDM_STATE);
		battleStates[2] = BattleStateFactory.GetInstance().Create(GMBattleState.GM_FFA_STATE);
		OperInfo = new OperatingInfo();
		QuestInfo = new QuestInfo();
		m_npcState = new NPCState();
		ItemInfoData = new ItemInfo();
		SkillTreeManager = new SkillTreeMgr();
	}

	public void Init()
	{
		roleState.Init();
		Enegy = 5000;
		stageState = new byte[Global.TOTAL_STAGE];
		stageState[0] = 1;
		currentCityID = 0;
		instanceState = new byte[Global.TOTAL_STAGE_INSTANCE];
		instanceState[0] = 1;
		instanceState[1] = 1;
		ItemInfoData.Init();
		InitDeafaultEquips();
		string name = ItemInfoData.Weapon1.baseItem.name;
		string value = name[name.Length - 2].ToString() + name[name.Length - 1];
		roleState.weaponId = Convert.ToByte(value);
		roleState.weaponType = (WeaponType)ItemInfoData.Weapon1.baseItem.ItemClass;
		InitBattleStates();
		InitStatistics();
		OperInfo.Init();
		QuestInfo.Init();
		m_npcState.Init();
		m_questStateContainer.Init();
		SkillTreeManager.Init();
		GambelManager.GetInstance().Init();
		InGameMenu.ResetIndex();
	}

	public void InitGame()
	{
		GameApp.GetInstance().GetGameMode().SubModePlay = SubMode.Story;
		InitBattleStates();
		InitStatistics();
	}

	public void LoginAsUser()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			string userName = string.Empty;
			switch (AndroidConstant.version)
			{
			case AndroidConstant.Version.GooglePlay:
				userName = "gg:" + GameApp.GetInstance().UUID;
				break;
			case AndroidConstant.Version.Kindle:
			case AndroidConstant.Version.KindleCn:
				userName = "kk:" + GameApp.GetInstance().UUID;
				break;
			case AndroidConstant.Version.MM:
				userName = "mm:" + GameApp.GetInstance().UUID;
				break;
			}
			Lobby.GetInstance().SetUserName(GameApp.GetInstance().GetUserState().GetRoleName());
			NetworkManager networkManager = GameApp.GetInstance().CreateNetwork();
			PlayerLoginRequest playerLoginRequest = new PlayerLoginRequest();
			playerLoginRequest.userName = userName;
			playerLoginRequest.passWord = "123";
			playerLoginRequest.version = GlobalState.version;
			networkManager.SendRequest(playerLoginRequest);
			TimeManager.GetInstance().LastSynTime = Time.time;
			Lobby.GetInstance().IsGuest = false;
		}
		else
		{
			string empty = string.Empty;
			Lobby.GetInstance().SetUserName(GameApp.GetInstance().GetUserState().GetRoleName());
			NetworkManager networkManager2 = GameApp.GetInstance().CreateNetwork();
			PlayerLoginRequest playerLoginRequest2 = new PlayerLoginRequest();
			playerLoginRequest2.userName = empty;
			playerLoginRequest2.passWord = "123";
			playerLoginRequest2.version = GlobalState.version;
			networkManager2.SendRequest(playerLoginRequest2);
			TimeManager.GetInstance().LastSynTime = Time.time;
			Lobby.GetInstance().IsGuest = false;
		}
	}

	public RoleState GetRoleState()
	{
		return roleState;
	}

	public int GetVSTDMStatsId()
	{
		return roleState.vsTDMStatsId;
	}

	public void SetVSTDMStatsId(int statsId)
	{
		roleState.vsTDMStatsId = statsId;
	}

	public byte GetLastPortalIndex()
	{
		return roleState.lastPortalIndex;
	}

	public void SetLastPortalIndex(byte index)
	{
		roleState.lastPortalIndex = index;
	}

	public byte[] GetDecorationHead()
	{
		return roleState.decorationHead;
	}

	public byte[] GetDecorationFace()
	{
		return roleState.decorationFace;
	}

	public byte[] GetDecorationWaist()
	{
		return roleState.decorationWaist;
	}

	public void SetDecorationHead(byte[] decoration)
	{
		roleState.decorationHead = decoration;
	}

	public void SetDecorationFace(byte[] decoration)
	{
		roleState.decorationFace = decoration;
	}

	public void SetDecorationWaist(byte[] decoration)
	{
		roleState.decorationWaist = decoration;
	}

	public void CreateNewRole(string name, CharacterClass charClass, Sex sex)
	{
		Init();
		roleState.SetRole(name, charClass, sex);
	}

	public void SetRoleState(RoleState state)
	{
		roleState = state;
	}

	public void SetRoleName(string name)
	{
		roleState.roleName = name;
	}

	public string GetRoleName()
	{
		return roleState.roleName;
	}

	public void SetCharacterClass(CharacterClass CharClass)
	{
		roleState.CharClass = CharClass;
	}

	public CharacterClass GetCharacterClass()
	{
		return roleState.CharClass;
	}

	public void SetSex(Sex sex)
	{
		roleState.SexType = sex;
	}

	public Sex GetSex()
	{
		return roleState.SexType;
	}

	public void SetCharLevel(short level)
	{
		roleState.CharLevel = level;
	}

	public short GetCharLevel()
	{
		return roleState.CharLevel;
	}

	public short GetCharLevelForUI()
	{
		return roleState.CharLevel;
	}

	public void SetCurrentQuest(short questId)
	{
		roleState.currentQuest = questId;
	}

	public short GetCurrentQuest()
	{
		return roleState.currentQuest;
	}

	public byte GetMaxUnlockCity()
	{
		byte result = 0;
		for (int i = 0; i < stageState.Length; i++)
		{
			if (stageState[i] == 1)
			{
				result = (byte)i;
			}
		}
		return result;
	}

	public void SetDecoration(byte[] decoration)
	{
		roleState.decoration = decoration;
	}

	public byte[] GetDecoration()
	{
		return roleState.decoration;
	}

	public byte GetAvatar()
	{
		return roleState.avatarID;
	}

	public void SetAvatar(byte avatar)
	{
		roleState.avatarID = avatar;
	}

	public byte[] GetAllAvatar()
	{
		return roleState.avatars;
	}

	public void SetAllAvatar(byte[] avatars)
	{
		roleState.avatars = avatars;
	}

	public void AddBulletByWeaponType(WeaponType _type, short bulletCount)
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		roleState.Bullets[(int)(_type - 1)] += bulletCount;
		if (roleState.Bullets[(int)(_type - 1)] > roleState.MaxBullets[(int)(_type - 1)] - localPlayer.GetBulletInGuns(_type))
		{
			roleState.Bullets[(int)(_type - 1)] = (short)(roleState.MaxBullets[(int)(_type - 1)] - localPlayer.GetBulletInGuns(_type));
		}
	}

	public short GetBulletByWeaponType(WeaponType _type)
	{
		return roleState.Bullets[(int)(_type - 1)];
	}

	private void AddMaxBulletByWeaponType(WeaponType _type, short bulletCount)
	{
		roleState.MaxBullets[(int)(_type - 1)] += bulletCount;
	}

	public short GetMaxBulletByWeaponType(WeaponType _type)
	{
		return roleState.MaxBullets[(int)(_type - 1)];
	}

	public void AddBulletLevelByWeaponType(WeaponType _type, short bulletCount)
	{
		roleState.BulletLevels[(int)(_type - 1)]++;
		AddMaxBulletByWeaponType(_type, bulletCount);
	}

	public byte GetBulletLevelByWeaponType(WeaponType _type)
	{
		return roleState.BulletLevels[(int)(_type - 1)];
	}

	public byte GetMaxBulletLevelByWeaponType(WeaponType _type)
	{
		return roleState.MaxBulletLevels[(int)(_type - 1)];
	}

	public byte GetBulletPriceFactorByWeaponType(WeaponType _type)
	{
		ItemInfiniteBullet itemInfiniteBullet = (ItemInfiniteBullet)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
			.GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
		if (itemInfiniteBullet.IsUnlimitedBullet(_type))
		{
			return 0;
		}
		return roleState.BulletPriceFactors[(int)(_type - 1)];
	}

	public byte GetBulletInMagsByWeaponType(WeaponType _type)
	{
		return roleState.BulletInMags[(int)(_type - 1)];
	}

	public void SetWeapon(WeaponType type, int num)
	{
		roleState.weaponType = type;
		roleState.weaponId = num;
	}

	public int GetWeaponId()
	{
		return roleState.weaponId;
	}

	public WeaponType GetWeaponType()
	{
		return roleState.weaponType;
	}

	public void SetCurrentCityID(int cityId)
	{
		currentCityID = (byte)cityId;
	}

	public byte GetCurrentCityID()
	{
		return currentCityID;
	}

	public void InitDeafaultEquips()
	{
		GameObject obj = new GameObject();
		GameObject obj2 = new GameObject();
		GameObject obj3 = new GameObject();
		GameObject gameObject = new GameObject();
		LootManager lootManager = new LootManager();
		ItemBase itemBase = lootManager.CreateItemBaseFromGameObject(obj, 1, ItemClasses.SubmachineGun, 1, ItemQuality.Common, 0);
		NGUIGameItem nGUIGameItem = new NGUIGameItem(0, itemBase.mNGUIBaseItem);
		nGUIGameItem.baseItem.ItemClass = ItemClasses.SubmachineGun;
		ItemInfoData.IsWeapon1Equiped = true;
		ItemInfoData.Weapon1 = nGUIGameItem;
		UnityEngine.Object.Destroy(obj);
		ItemBase itemBase2 = lootManager.CreateItemBaseFromGameObject(obj2, 1, ItemClasses.Pistol, 1, ItemQuality.Common, 0);
		NGUIGameItem nGUIGameItem2 = new NGUIGameItem(0, itemBase2.mNGUIBaseItem);
		nGUIGameItem2.baseItem.ItemClass = ItemClasses.Pistol;
		ItemInfoData.IsWeapon2Equiped = true;
		ItemInfoData.Weapon2 = nGUIGameItem2;
		UnityEngine.Object.Destroy(obj2);
		ItemBase itemBase3 = lootManager.CreateItemBaseFromGameObject(obj3, 1, ItemClasses.U_Shield, 1, ItemQuality.Common, 0);
		NGUIGameItem nGUIGameItem3 = new NGUIGameItem(0, itemBase3.mNGUIBaseItem);
		nGUIGameItem3.baseItem.ItemClass = ItemClasses.U_Shield;
		ItemInfoData.BackPackItems[0] = nGUIGameItem3;
		UnityEngine.Object.Destroy(obj3);
		roleState.Bullets[0] = 216;
		roleState.MaxBullets[0] = 216;
		roleState.Bullets[1] = 180;
		roleState.MaxBullets[1] = 180;
		roleState.Bullets[2] = 180;
		roleState.MaxBullets[2] = 180;
		roleState.Bullets[3] = 72;
		roleState.MaxBullets[3] = 72;
		roleState.Bullets[4] = 54;
		roleState.MaxBullets[4] = 54;
		roleState.Bullets[5] = 36;
		roleState.MaxBullets[5] = 36;
		roleState.Bullets[6] = 18;
		roleState.MaxBullets[6] = 18;
		roleState.Bullets[7] = 3;
		roleState.MaxBullets[7] = 3;
		for (int i = 0; i < 8; i++)
		{
			roleState.BulletLevels[i] = 0;
		}
	}

	public void InitBattleStates()
	{
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			battleStates[i].Init();
		}
	}

	public void InitStatistics()
	{
		int num = 1;
		for (int i = 0; i < num; i++)
		{
			m_statistics[i].Init();
		}
	}

	public void RefreshAvatarStates()
	{
	}

	public void UseEnegy(int count)
	{
		Enegy -= count;
		Enegy = Mathf.Clamp(Enegy, 0, Global.MAX_ENEGY);
	}

	public void UnLockAllLevels()
	{
		byte[] array = new byte[Global.TOTAL_STAGE_INSTANCE];
		for (int i = 0; i < Global.TOTAL_STAGE_INSTANCE; i++)
		{
			array[i] = 1;
		}
		SetStageInstance(array);
	}

	public void EnterGodMode()
	{
		GameApp.GetInstance().GetGlobalState().SetMithril(100000000);
		SetCash(100000000);
		AddExp(183750);
		UnLockAllLevels();
	}

	public IQuestProgress GetQuestProgress()
	{
		return m_questStateContainer;
	}

	public byte[] GetStageState()
	{
		return stageState;
	}

	public void SetStageState(byte[] stages)
	{
		for (int i = 0; i < stages.Length; i++)
		{
			stageState[i] = stages[i];
		}
	}

	public void SetStageState(int stage_Idx, int state)
	{
		stageState[stage_Idx] = (byte)state;
	}

	public byte GetStageState(int stage_Idx)
	{
		return stageState[stage_Idx];
	}

	public byte[] GetStageInstanceState()
	{
		return instanceState;
	}

	public void SetStageInstance(byte[] instance)
	{
		for (int i = 0; i < instance.Length; i++)
		{
			instanceState[i] = instance[i];
		}
	}

	public void SetStageInstanceState(string instanceName, int state)
	{
		SceneConfig sceneConfig = GameConfig.GetInstance().sceneConfig[instanceName];
		instanceState[sceneConfig.SceneID - 32] = (byte)state;
		if (state != 1)
		{
			return;
		}
		SetStageState(sceneConfig.AreaID, 1);
		Dictionary<string, SceneConfig> sceneConfig2 = GameConfig.GetInstance().sceneConfig;
		foreach (SceneConfig value in sceneConfig2.Values)
		{
			Debug.Log("sc.FatherSceneID : " + sceneConfig.FatherSceneID);
			Debug.Log("s.SceneID : " + value.SceneID);
			Debug.Log("s.ArenaBelongToWhichSceneID : " + value.ArenaBelongToWhichSceneID);
			if (sceneConfig.FatherSceneID == value.SceneID || sceneConfig.FatherSceneID == value.ArenaBelongToWhichSceneID)
			{
				instanceState[value.SceneID - 32] = 1;
			}
		}
	}

	public byte GetStageInstanceState(int instance_Idx)
	{
		return instanceState[instance_Idx];
	}

	public int GetExp()
	{
		return roleState.GetExp();
	}

	public void SetExp(int exp)
	{
		roleState.SetExp(exp);
	}

	public int AddExp(int exp)
	{
		ItemExpScale itemExpScale = (ItemExpScale)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
			.GetGlobalIAPItem(IAPItemState.ItemType.ExpScale);
		exp = (int)((float)exp * itemExpScale.GetScale());
		int num = roleState.AddExp(exp);
		if (num >= GetNextLvRequiredExp(roleState.CharLevel) && roleState.CharLevel < Global.MAX_CHAR_LEVEL)
		{
			int charLevel = roleState.CharLevel;
			LevelUp();
			if (Time.timeScale == 0f)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.OnLevelUpInMenu();
			}
			else
			{
				AudioManager.GetInstance().PlaySound("RPG_Audio/Player/player_levelup");
			}
			EffectPlayer.GetInstance().PlayLevelUp();
			if (TutorialManager.GetInstance().IsLearnSkillTutorialOk())
			{
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("LOC_HINT_GET_SKILL_POINT"));
			}
			for (int i = 0; i < roleState.CharLevel - charLevel; i++)
			{
				SkillTreeManager.AddSkillPoint();
			}
			ItemInfoData.RefreshShopItems();
			Debug.Log("roleState.CharLevel : " + roleState.CharLevel);
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Higher_the_better);
			achievementTrigger.PutData(roleState.CharLevel);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PlayerLevelUpRequest request = new PlayerLevelUpRequest(roleState.CharLevel);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		return exp;
	}

	public void LevelUp()
	{
		roleState.CharLevel = GetLevelByExp(roleState.GetExp());
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		localPlayer.LevelUp(GetLevelByExp(roleState.GetExp()));
		(localPlayer as LocalPlayer).RecoverHP(10000f, false);
		(localPlayer as LocalPlayer).RecoverShiled(10000f);
	}

	public short GetLevelByExp(int exp)
	{
		roleState.CharLevel = Global.MAX_CHAR_LEVEL;
		for (int i = 1; i <= Global.MAX_CHAR_LEVEL; i++)
		{
			if (GetNextLvRequiredExp(i) > exp)
			{
				roleState.CharLevel = (short)i;
				break;
			}
		}
		return roleState.CharLevel;
	}

	public int GetNextLvRequiredExp(int charLevel)
	{
		return Exp.RequiredLevelUpExp[charLevel];
	}

	public int GetCash()
	{
		return roleState.GetCash();
	}

	public void SetCash(int cash)
	{
		roleState.SetCash(cash);
	}

	public void AddCash(int cash)
	{
		roleState.AddCash(cash);
	}

	public void BuyCashWithMithril(ExchangeName name)
	{
		ExchangeItem exchangeItem = IAPShop.GetInstance().GetExchangeList()[name];
		if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(exchangeItem.Mithril))
		{
			AddCash(exchangeItem.Cash);
			GameApp.GetInstance().Save();
		}
	}

	public bool CanBuy(int price)
	{
		if (roleState.GetCash() >= price)
		{
			return true;
		}
		return false;
	}

	public bool Buy(int price)
	{
		if (roleState.GetCash() >= price)
		{
			AddCash(-price);
			return true;
		}
		return false;
	}

	public void SetBattleStates(IBattleState[] states)
	{
		for (int i = 0; i < states.Length; i++)
		{
			battleStates[i].SetState(states[i]);
		}
	}

	public IBattleState[] GetBattleStates()
	{
		return battleStates;
	}

	public TDMState GetVSTDMBattleState()
	{
		return (TDMState)battleStates[1];
	}

	public string GetNpcState(string key)
	{
		return m_npcState.GetState(key);
	}

	public void SetNpcState(string key, string state)
	{
		m_npcState.SetState(key, state);
	}

	public void WriteCharClass(BytesBuffer buffer)
	{
		buffer.AddByte((byte)roleState.CharClass);
	}

	public void WriteSex(BytesBuffer buffer)
	{
		buffer.AddByte((byte)roleState.SexType);
	}

	public void WriteCash(BytesBuffer buffer)
	{
		buffer.AddInt(GetCash());
	}

	public void WriteCharLevel(BytesBuffer buffer)
	{
		buffer.AddShort(roleState.CharLevel);
	}

	public void WriteExp(BytesBuffer buffer)
	{
		buffer.AddInt(roleState.GetExp());
	}

	public void WriteEnergy(BytesBuffer buffer)
	{
		buffer.AddInt(Enegy);
	}

	public void WriteAvatar(BytesBuffer buffer)
	{
		buffer.AddByte(roleState.avatarID);
	}

	public void WriteDecoration(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.DECORATION_PART_NUM; i++)
		{
			buffer.AddByte(roleState.decoration[i]);
		}
	}

	public void WriteBullets(BytesBuffer buffer)
	{
		for (int i = 0; i < 8; i++)
		{
			buffer.AddShort(roleState.Bullets[i]);
			buffer.AddShort(roleState.MaxBullets[i]);
			buffer.AddByte(roleState.BulletLevels[i]);
		}
	}

	public void WriteStageState(BytesBuffer buffer)
	{
		for (int i = 0; i < Global.TOTAL_STAGE; i++)
		{
			buffer.AddByte(stageState[i]);
		}
	}

	public void WriteBattleState(BytesBuffer buffer)
	{
		for (int i = 0; i < battleStates.Length; i++)
		{
			battleStates[i].WriteToBuffer(buffer);
		}
	}

	public byte GetSceneId(string sceneName)
	{
		Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
		foreach (SceneConfig value in sceneConfig.Values)
		{
			if (sceneName.Equals(value.SceneFileName))
			{
				return value.SceneID;
			}
		}
		Debug.LogError("error scene name..");
		return 0;
	}

	public void SaveData(BinaryWriter bw)
	{
		IRecordset recordset = GameApp.GetInstance().GetGlobalState().GetRecordset(GlobalState.version);
		Debug.Log("save rms beging.....");
		recordset.SaveData(bw);
		Debug.Log("save rms end.....");
	}

	public void LoadData(string ver, BinaryReader br)
	{
		IRecordset recordset = GameApp.GetInstance().GetGlobalState().GetRecordset(ver);
		if (recordset != null)
		{
			roleState = recordset.LoadRoleData(br);
			recordset.LoadData(br);
		}
	}

	public RoleState LoadRoleData(BinaryReader br)
	{
		IRecordset recordset = GameApp.GetInstance().GetGlobalState().GetRecordset(GlobalState.version);
		if (recordset != null)
		{
			return recordset.LoadRoleData(br);
		}
		return null;
	}

	public void LoadLevel()
	{
		Application.LoadLevel("City" + currentCityID);
	}

	public void LoadLevel(int cityId)
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			gameScene.LeaveScene();
		}
		Application.LoadLevel("City" + cityId);
	}

	public void ReturnBullets()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		foreach (Weapon weapon in localPlayer.GetWeaponList())
		{
			if (weapon != null)
			{
				short bulletCount = (short)weapon.BulletCountInGun;
				weapon.BulletCountInGun = 0;
				AddBulletByWeaponType(weapon.GetWeaponType(), bulletCount);
			}
		}
	}

	public void SaveBullets(BinaryWriter bw)
	{
		for (int i = 0; i < 8; i++)
		{
			int num = 0;
			if (GameApp.GetInstance().GetGameWorld() != null)
			{
				LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				if (localPlayer != null)
				{
					foreach (Weapon weapon in localPlayer.GetWeaponList())
					{
						if (weapon != null && weapon.GetWeaponType() == (WeaponType)(i + 1))
						{
							num += weapon.BulletCountInGun;
						}
					}
				}
			}
			bw.Write((short)(roleState.Bullets[i] + num));
			bw.Write(roleState.MaxBullets[i]);
			bw.Write(roleState.BulletLevels[i]);
		}
	}

	public void LoadBullets(BinaryReader br)
	{
		for (int i = 0; i < 8; i++)
		{
			roleState.Bullets[i] = br.ReadInt16();
			roleState.MaxBullets[i] = br.ReadInt16();
			roleState.BulletLevels[i] = br.ReadByte();
		}
	}

	public void LoadDecorations(BinaryReader br)
	{
		int num = 0;
		num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			roleState.decorationHead[i] = br.ReadByte();
		}
		num = br.ReadInt32();
		for (int j = 0; j < num; j++)
		{
			roleState.decorationFace[j] = br.ReadByte();
		}
		num = br.ReadInt32();
		for (int k = 0; k < num; k++)
		{
			roleState.decorationWaist[k] = br.ReadByte();
		}
	}

	public void SaveDecorations(BinaryWriter bw)
	{
		bw.Write(Global.TOTAL_ARMOR_HEAD_NUM);
		for (int i = 0; i < Global.TOTAL_ARMOR_HEAD_NUM; i++)
		{
			bw.Write(roleState.decorationHead[i]);
		}
		bw.Write(Global.TOTAL_ARMOR_FACE_NUM);
		for (int j = 0; j < Global.TOTAL_ARMOR_FACE_NUM; j++)
		{
			bw.Write(roleState.decorationFace[j]);
		}
		bw.Write(Global.TOTAL_ARMOR_WAIST_NUM);
		for (int k = 0; k < Global.TOTAL_ARMOR_WAIST_NUM; k++)
		{
			bw.Write(roleState.decorationWaist[k]);
		}
	}

	public void SaveAvatars(BinaryWriter bw)
	{
		bw.Write(Global.TOTAL_AVATAR_NUM);
		for (int i = 0; i < Global.TOTAL_AVATAR_NUM; i++)
		{
			bw.Write(roleState.avatars[i]);
		}
	}

	public void LoadAvatars(BinaryReader br)
	{
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			roleState.avatars[i] = br.ReadByte();
		}
	}

	public bool HasNewbie1()
	{
		foreach (RoleStateInfo role in GameApp.GetInstance().GetGlobalState().GetRoles())
		{
			if (role.RoleName.Equals(GetRoleName()))
			{
				return role.bNewbie1;
			}
		}
		return false;
	}

	public bool HasNewbie2()
	{
		foreach (RoleStateInfo role in GameApp.GetInstance().GetGlobalState().GetRoles())
		{
			if (role.RoleName.Equals(GetRoleName()))
			{
				return role.bNewbie2;
			}
		}
		return false;
	}

	public void SetNewbie1(bool state)
	{
		foreach (RoleStateInfo role in GameApp.GetInstance().GetGlobalState().GetRoles())
		{
			if (role.RoleName.Equals(GetRoleName()))
			{
				role.bNewbie1 = state;
			}
		}
	}

	public void SetNewbie2(bool state)
	{
		foreach (RoleStateInfo role in GameApp.GetInstance().GetGlobalState().GetRoles())
		{
			if (role.RoleName.Equals(GetRoleName()))
			{
				role.bNewbie2 = state;
			}
		}
	}
}
