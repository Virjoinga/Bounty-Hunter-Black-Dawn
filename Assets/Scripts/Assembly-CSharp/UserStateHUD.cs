using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStateHUD
{
	public class InfoBoxHUD
	{
		public class InfoHUD
		{
			public enum InfoType
			{
				PopInfo = 0,
				NumberInfo = 1
			}

			public string StrInfo { get; set; }

			public int NumberInfo { get; set; }

			public new InfoType GetType { get; set; }
		}

		private List<InfoHUD> list = new List<InfoHUD>();

		public void SetNumberInfo(string str, int number)
		{
			SetInfo(str, number, InfoHUD.InfoType.NumberInfo);
		}

		public void CloseNumberInfo()
		{
			SetInfo("/Hide", 0, InfoHUD.InfoType.NumberInfo);
		}

		public void PushInfo(string str)
		{
			SetInfo(str, 0, InfoHUD.InfoType.PopInfo);
		}

		public void PushFirstBloodInfo(Player killer)
		{
			string @string = LocalizationManager.GetInstance().GetString("LOC_PVP_1ST_BLOOD");
			string text = ModifyTeamNameColor(killer.GetDisplayName(), killer.Team);
			SetInfo(text + @string, 0, InfoHUD.InfoType.PopInfo);
		}

		public void PushKillInfo(Player killer, Player killed)
		{
			string @string = LocalizationManager.GetInstance().GetString("LOC_PVP_A_KILL_B");
			string text = ModifyTeamNameColor(killer.GetDisplayName(), killer.Team);
			string text2 = ModifyTeamNameColor(killed.GetDisplayName(), killed.Team);
			SetInfo(text + @string + text2, 0, InfoHUD.InfoType.PopInfo);
		}

		public void PushMulitKillInfo(Player killer, int count)
		{
			string text = LocalizationManager.GetInstance().GetString("LOC_PVP_KILL_STREAK").Replace("%d", string.Empty + count);
			string text2 = ModifyTeamNameColor(killer.GetDisplayName(), killer.Team);
			SetInfo(text2 + text, 0, InfoHUD.InfoType.PopInfo);
		}

		private string ModifyTeamNameColor(string name, TeamName teamName)
		{
			switch (teamName)
			{
			case TeamName.Red:
				return "[ff0000]" + name + "[-]";
			case TeamName.Blue:
				return "[00cfff]" + name + "[-]";
			default:
				return name;
			}
		}

		public void PushMissionInfo(string disc, int curNum, int maxNum)
		{
			SetInfo("[ffba00]" + disc + "[-] [0cff00]" + curNum + "/" + maxNum + "[-]", 0, InfoHUD.InfoType.PopInfo);
		}

		public void PushMissionUpdate()
		{
			SetInfo("[00eaff]" + LocalizationManager.GetInstance().GetString("MSG_MISSION_UPDATE") + "[-]", 0, InfoHUD.InfoType.PopInfo);
		}

		public void SetInfo(string str, int number, InfoHUD.InfoType type)
		{
			InfoHUD infoHUD = new InfoHUD();
			infoHUD.StrInfo = str;
			infoHUD.GetType = type;
			infoHUD.NumberInfo = number;
			list.Add(infoHUD);
		}

		public InfoHUD PopInfo()
		{
			if (Count() > 0)
			{
				InfoHUD result = list[0];
				list.RemoveAt(0);
				return result;
			}
			return null;
		}

		public int Count()
		{
			return list.Count;
		}

		public void PushBulletDescription(WeaponType bulletType, int bulletCount)
		{
			string str = "[0cff00]" + ItemBase.ItemClassName((ItemClasses)bulletType) + " " + LocalizationManager.GetInstance().GetString("MSG_PICKUP_AMMO") + " : " + bulletCount + "[-]";
			PushInfo(str);
		}

		public void PushMoneyDescription(int money)
		{
			string str = "[ffba00]" + LocalizationManager.GetInstance().GetString("MSG_PICKUP_MONEY") + " : " + money + "[-]";
			PushInfo(str);
		}

		public void PushEquipDescription(NGUIBaseItem baseItem)
		{
			string str = "Get " + baseItem.DisplayName;
			PushInfo(str);
		}
	}

	public class ChatBoxHUD
	{
		private List<string> mList;

		private bool bHasRead;

		public List<string> ChatList
		{
			get
			{
				bHasRead = true;
				return mList;
			}
		}

		public bool HasRead
		{
			get
			{
				return bHasRead;
			}
		}

		public ChatBoxHUD()
		{
			mList = new List<string>();
			bHasRead = true;
		}

		public void Add(string text)
		{
			if (mList.Count == 30)
			{
				mList.RemoveAt(0);
			}
			mList.Add(text);
			bHasRead = false;
		}

		public void Clear()
		{
			mList.Clear();
			bHasRead = false;
		}
	}

	public class UserBuffHUD
	{
		public enum FullScreenEffect
		{
			None = -1,
			Avatar = 0,
			Morphine = 1
		}

		public string Name { get; set; }

		public string Detail { get; set; }

		public float Time { get; set; }

		public FullScreenEffect BuffEffect { get; set; }
	}

	public class GameUnitHUD
	{
		public enum Type
		{
			None = 0,
			Enemy = 1,
			RemotePlayer = 2,
			LocalPlayer = 3
		}

		public GameUnit target;

		public Type type;

		private static Color[] enemyLevelColors = new Color[5]
		{
			new Color(0.7490196f, 0.7490196f, 0.7490196f),
			new Color(0.57254905f, 0.8156863f, 16f / 51f),
			new Color(1f, 1f, 16f / 51f),
			new Color(0.8862745f, 0.41960785f, 2f / 51f),
			new Color(1f, 0f, 0f)
		};

		private static Color[] remotePlayerColors = new Color[8]
		{
			new Color(1f, 0f, 84f / 85f),
			new Color(1f, 0.9411765f, 0f),
			new Color(0f, 76f / 85f, 1f),
			new Color(46f / 85f, 0f, 1f),
			new Color(1f, 0f, 0.47058824f),
			new Color(1f, 0.47058824f, 0f),
			new Color(0f, 0.47058824f, 1f),
			new Color(46f / 85f, 0f, 0.47058824f)
		};

		public bool IsActive
		{
			get
			{
				if (target is Enemy)
				{
					Enemy enemy = (Enemy)target;
					return enemy.IsActive();
				}
				if (target is Player)
				{
					Player player = (Player)target;
					if (GameApp.GetInstance().GetGameWorld().CurrentSceneID == player.CurrentSceneID && GameApp.GetInstance().GetUserState().GetCurrentCityID() == player.CurrentCityID)
					{
						return true;
					}
					return false;
				}
				return false;
			}
		}

		public bool IsAlive
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return !player.DYING_STATE.InDyingState;
				}
				return HpPercent != 0f;
			}
		}

		public bool IsPVPReady
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return player.IsPVPReady;
				}
				return false;
			}
		}

		public Vector3 Position
		{
			get
			{
				Transform transform = target.GetTransform();
				if (transform == null)
				{
					return Vector3.zero;
				}
				return target.GetTransform().position;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return target.GetTransform().localScale;
			}
		}

		public string Name
		{
			get
			{
				string text = target.GetDisplayName();
				if (type == Type.Enemy)
				{
					text = LocalizationManager.GetInstance().GetString(text);
				}
				return text;
			}
		}

		public string LevelStr
		{
			get
			{
				if (target is Enemy)
				{
					Enemy enemy = (Enemy)target;
					return "Lv." + enemy.Level;
				}
				if (target is Player)
				{
					Player player = (Player)target;
					return "Lv." + player.GetUserState().GetCharLevel();
				}
				return "Lv.0";
			}
		}

		public int Level
		{
			get
			{
				if (target is Enemy)
				{
					Enemy enemy = (Enemy)target;
					return enemy.Level;
				}
				if (target is Player)
				{
					Player player = (Player)target;
					return player.GetUserState().GetCharLevel();
				}
				return 0;
			}
		}

		public float HpPercent
		{
			get
			{
				if (target.MaxHp != 0)
				{
					return (float)target.Hp / (float)target.MaxHp;
				}
				return 0f;
			}
		}

		public float SpPercent
		{
			get
			{
				if (target.MaxShield != 0)
				{
					return (float)target.Shield / (float)target.MaxShield;
				}
				return 0f;
			}
		}

		public bool HasShelid
		{
			get
			{
				return target.MaxShield != 0;
			}
		}

		public Transform Transform
		{
			get
			{
				return target.GetTransform();
			}
		}

		public int IconIndex
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return player.GetSeatID();
				}
				return -1;
			}
		}

		public bool IsRoomMaster
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return player.IsRoomMaster;
				}
				return false;
			}
		}

		public bool IsDangerEnemy
		{
			get
			{
				if (target is Enemy)
				{
					int num = Level - GameApp.GetInstance().GetUserState().GetCharLevel();
					return num > 2;
				}
				return false;
			}
		}

		public Color Color
		{
			get
			{
				return GetColor(target);
			}
		}

		public string ClassIconSpriteName
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return GetInstance().GetUserClassIcon(player.GetUserState().GetCharacterClass());
				}
				return string.Empty;
			}
		}

		public string ClassName
		{
			get
			{
				if (target is Player)
				{
					Player player = (Player)target;
					return player.GetUserState().GetCharacterClass().ToString();
				}
				return string.Empty;
			}
		}

		public static Color GetColor(GameUnit target)
		{
			if (target is Player)
			{
				Player player = (Player)target;
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					return UIConstant.COLOR_TEAM_PLAYER[(int)player.Team];
				}
				return remotePlayerColors[player.GetSeatID()];
			}
			if (target is Enemy)
			{
				Enemy enemy = (Enemy)target;
				int num = enemy.Level - GameApp.GetInstance().GetUserState().GetCharLevel();
				if (num <= -2)
				{
					return enemyLevelColors[0];
				}
				switch (num)
				{
				case -1:
					return enemyLevelColors[1];
				case 0:
					return enemyLevelColors[2];
				case 1:
					return enemyLevelColors[3];
				default:
					return enemyLevelColors[4];
				}
			}
			return Color.white;
		}

		public bool isSame(GameUnitHUD another)
		{
			return Name.Equals(another.Name);
		}

		public TeamName GetTeamName()
		{
			if (target is Player)
			{
				Player player = (Player)target;
				return player.Team;
			}
			return TeamName.FreeForAll;
		}

		public IStatistics[] GetStatistics()
		{
			if (target is Player)
			{
				Player player = (Player)target;
				return player.GetUserState().m_statistics;
			}
			return null;
		}
	}

	public class DamageHUD
	{
		public bool Critical { get; set; }

		public int Damage { get; set; }

		public ElementType ElementType { get; set; }

		public Vector3 EnemyPosition { get; set; }
	}

	public class SkillHUD
	{
		private int id;

		private CharacterInstantSkill skill;

		public float CDPercent
		{
			get
			{
				if (skill == null)
				{
					skill = GetSkill();
				}
				if (skill == null)
				{
					return 0f;
				}
				return skill.GetCoolDownLeftTimeMS() / skill.CoolDownTime;
			}
		}

		public bool Enable
		{
			get
			{
				skill = GetSkill();
				if (skill == null)
				{
					return false;
				}
				return true;
			}
		}

		public string IconFileName
		{
			get
			{
				if (skill == null)
				{
					skill = GetSkill();
				}
				if (skill == null)
				{
					return string.Empty;
				}
				return skill.IconName;
			}
		}

		public SkillHUD(int id)
		{
			this.id = id;
		}

		private CharacterInstantSkill GetSkill()
		{
			skill = null;
			CharacterSkillManager characterSkillManager = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetCharacterSkillManager();
			if (characterSkillManager != null)
			{
				int num = 0;
				List<CharacterInstantSkill> initiativeSkillList = characterSkillManager.GetInitiativeSkillList();
				for (int i = 0; i < initiativeSkillList.Count; i++)
				{
					if (num < id)
					{
						num++;
						continue;
					}
					skill = initiativeSkillList[i];
					break;
				}
			}
			return skill;
		}

		public void Apply()
		{
			if (skill != null)
			{
				skill.ApplySkill(GameApp.GetInstance().GetGameWorld().GetLocalPlayer());
				if (skill.ApplySuccess)
				{
					AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Plan_B, AchievementTrigger.Type.Data);
					achievementTrigger.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger);
				}
			}
		}

		public void Clear()
		{
			skill = null;
		}
	}

	public class JoystickHUD
	{
		private float moveJoystickRatio;

		public bool IsInit { get; set; }

		public bool IsFixed { get; set; }

		public Vector2[] TouchPos { get; set; }

		public bool isMoveJoystickPressed { get; set; }

		public bool isPressedToRotateCamera { get; set; }

		public Vector2 MoveCenter { get; set; }

		public Vector2 ShootCenter { get; set; }

		public float Radius { get; set; }

		public float MoveJoystickRatio
		{
			get
			{
				return moveJoystickRatio;
			}
		}

		public JoystickHUD()
		{
			IsFixed = false;
			isMoveJoystickPressed = false;
			isPressedToRotateCamera = false;
			MoveCenter = Vector2.zero;
			ShootCenter = Vector2.zero;
			TouchPos = new Vector2[2];
			TouchPos[0] = Vector2.zero;
			TouchPos[1] = Vector2.zero;
			Radius = 0f;
			moveJoystickRatio = 0.3f;
		}
	}

	public class VSBattleField
	{
		private bool newBattle;

		private int revivePrice;

		public bool NewBattle
		{
			get
			{
				return newBattle;
			}
			set
			{
				if (value)
				{
					RedTeam.State = VSUserTeam.VSTeamState.Playing;
					BlueTeam.State = VSUserTeam.VSTeamState.Playing;
				}
				newBattle = value;
			}
		}

		public int RevivePrice
		{
			get
			{
				return revivePrice;
			}
		}

		public bool AlreadyFirstBlood { get; set; }

		public VSUserTeam RedTeam { get; set; }

		public VSUserTeam BlueTeam { get; set; }

		public List<VSBattleFieldPoint> PointInfo { get; set; }

		public VSBattleField()
		{
			newBattle = false;
			PointInfo = new List<VSBattleFieldPoint>();
			RedTeam = new VSUserTeam();
			BlueTeam = new VSUserTeam();
			RedTeam.State = VSUserTeam.VSTeamState.Playing;
			BlueTeam.State = VSUserTeam.VSTeamState.Playing;
		}

		public void SetBattleFieldState()
		{
			VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
			ClearPoint();
			foreach (KeyValuePair<int, TargetPointInfo> item in vSTDMManager.pointInfo)
			{
				AddPoint(item.Value, vSTDMManager.PointCanCapture(item.Key));
			}
			RedTeam.Resource = vSTDMManager.RedTeam.GetScore();
			BlueTeam.Resource = vSTDMManager.BlueTeam.GetScore();
			AddPlayer(GameApp.GetInstance().GetGameWorld().GetLocalPlayer());
			foreach (RemotePlayer remotePlayer in GameApp.GetInstance().GetGameWorld().GetRemotePlayers())
			{
				AddPlayer(remotePlayer);
			}
			RedTeam.Rank();
			BlueTeam.Rank();
		}

		public void AddPoint(TargetPointInfo pointInfo, bool isCapturing)
		{
			VSBattleFieldPoint vSBattleFieldPoint = new VSBattleFieldPoint();
			vSBattleFieldPoint.Owner = pointInfo.GetOwner();
			vSBattleFieldPoint.IsCapturing = isCapturing;
			vSBattleFieldPoint.CapturingTime = pointInfo.GetTime();
			vSBattleFieldPoint.TotalCaptureTime = pointInfo.GetCaptureTime();
			vSBattleFieldPoint.PointID = pointInfo.GetPointID();
			PointInfo.Add(vSBattleFieldPoint);
			if (vSBattleFieldPoint.Owner == 1)
			{
				RedTeam.Point++;
			}
			if (vSBattleFieldPoint.Owner == 2)
			{
				BlueTeam.Point++;
			}
		}

		public void AddPlayer(Player player)
		{
			VSUserTeam vSUserTeam = null;
			if (player.Team == TeamName.Red)
			{
				vSUserTeam = RedTeam;
			}
			else
			{
				if (player.Team != TeamName.Blue)
				{
					return;
				}
				vSUserTeam = BlueTeam;
			}
			VSUserState vSUserState = new VSUserState();
			vSUserState.ClassSpriteName = GetInstance().GetUserClassIcon(player.GetUserState().GetCharacterClass());
			vSUserState.Name = player.GetDisplayName();
			vSUserState.LastKill = player.GetUserState().GetVSTDMBattleState().kills;
			vSUserState.LastDead = player.GetUserState().GetVSTDMBattleState().dead;
			vSUserState.LastAssist = player.GetUserState().GetVSTDMBattleState().assist;
			vSUserState.Bonus = player.GetUserState().GetVSTDMBattleState().bonus;
			vSUserState.Score = player.GetUserState().GetVSTDMBattleState().score;
			vSUserState.IsLocal = player is LocalPlayer;
			vSUserTeam.VSUserStateList.Add(vSUserState);
		}

		public void RecordRevive()
		{
			revivePrice = ((revivePrice * 2 <= 65536) ? (revivePrice * 2) : 65536);
		}

		private void ClearPoint()
		{
			RedTeam.Point = 0;
			RedTeam.Resource = 0;
			RedTeam.VSUserStateList.Clear();
			BlueTeam.Point = 0;
			BlueTeam.Resource = 0;
			BlueTeam.VSUserStateList.Clear();
			PointInfo.Clear();
		}

		public void Clear()
		{
			revivePrice = 2;
			AlreadyFirstBlood = false;
			NewBattle = true;
			ClearPoint();
		}
	}

	public class VSBattleFieldPoint
	{
		public int PointID { get; set; }

		public byte Owner { get; set; }

		public bool IsCapturing { get; set; }

		public float CapturingTime { get; set; }

		public float TotalCaptureTime { get; set; }
	}

	public class VSUserTeam
	{
		public enum VSTeamState
		{
			Playing = 0,
			Win = 1,
			Lost = 2
		}

		public UIVS.Mode GameMode { get; set; }

		public VSTeamState State { get; set; }

		public int Point { get; set; }

		public int Resource { get; set; }

		public List<VSUserState> VSUserStateList { get; set; }

		public VSUserTeam()
		{
			VSUserStateList = new List<VSUserState>();
		}

		public void SetScoreVisible(bool visible)
		{
			foreach (VSUserState vSUserState in VSUserStateList)
			{
				vSUserState.ScoreVisible = visible;
			}
		}

		public void Rank()
		{
			VSUserStateList.Sort(RankByScore);
			for (int i = 0; i < VSUserStateList.Count; i++)
			{
				VSUserStateList[i].Rank = i + 1;
			}
		}

		private int RankByScore(VSUserState state1, VSUserState state2)
		{
			if (state1.Score <= state2.Score)
			{
				return 1;
			}
			return -1;
		}
	}

	public class VSUserState
	{
		public bool IsLocal { get; set; }

		public int Rank { get; set; }

		public int Bonus { get; set; }

		public int Score { get; set; }

		public int LastKill { get; set; }

		public int LastDead { get; set; }

		public int LastAssist { get; set; }

		public int Win { get; set; }

		public int Lost { get; set; }

		public int Offline { get; set; }

		public string Name { get; set; }

		public string ClassSpriteName { get; set; }

		public bool ScoreVisible { get; set; }

		public VSUserState()
		{
			ScoreVisible = true;
		}
	}

	public const string COLOR_MISSON_UPDATE = "[00eaff]";

	public const string COLOR_MISSON_INFO = "[ffba00]";

	public const string COLOR_MISSON_INFO_NUMBER = "[0cff00]";

	public const string COLOR_BULLET = "[a3ffe8]";

	public const string COLOR_USER_LEVEL = "[ffeea0]";

	public const string COLOR_GOLD_PICK_UP = "[ffba00]";

	public const string COLOR_BULLET_PICK_UP = "[0cff00]";

	public const string COLOR_RED_TEAM_NAME = "[ff0000]";

	public const string COLOR_BLUE_TEAM_NAME = "[00cfff]";

	private const int MITHRIL_COST_OF_RETURN = 5;

	private const float CD_OF_RETURN = 300f;

	private const int MIN_CHAT_NUM = 30;

	private static UserStateHUD instance;

	private GameUnitHUD mTargetAimed;

	private ArrayList mDamageList;

	private Dictionary<string, GameUnitHUD> mEnemyList;

	private Dictionary<string, GameUnitHUD> mRemotePlayerList;

	private Dictionary<string, GameUnitHUD> mPlayerList;

	private Dictionary<string, Vector2> mQuestList;

	private List<GameUnitHUD> mAttackerList;

	private SkillHUD mSkill1;

	private SkillHUD mSkill2;

	private ItemBase nearestItem;

	private ChestScript nearestChest;

	private bool bDyingManInSight;

	private ChatBoxHUD mChatBoxHUD;

	private GameObject mCallBackWhenSave;

	private JoystickHUD mJoystickHUD;

	private InfoBoxHUD mInfoBoxHUD;

	private Timer startCoolDownTime = new Timer();

	private bool bMissionUpdate;

	private float missionUpdateDelay;

	private bool bWeaponListOpen;

	private VSBattleField mVSBattleField;

	private int inBattleFieldPoint;

	private TeamName captureSign = TeamName.FreeForAll;

	private TeamName teamSign = TeamName.FreeForAll;

	private bool bVSTDMLockVisible;

	private GameUnitHUD emptyGameUnit = new GameUnitHUD();

	private bool bShowFillAllButton;

	private byte[] mutex = new byte[1];

	private List<int> mSlotQueueID = new List<int>();

	private SummonedItem mSummonedItem;

	public bool Reload { get; set; }

	public JoystickHUD Joystick
	{
		get
		{
			return mJoystickHUD;
		}
	}

	public InfoBoxHUD InfoBox
	{
		get
		{
			return mInfoBoxHUD;
		}
	}

	public GameUnitHUD TargetAimed
	{
		get
		{
			return mTargetAimed;
		}
		set
		{
			mTargetAimed = value;
		}
	}

	public SkillHUD Skill1
	{
		get
		{
			return mSkill1;
		}
	}

	public SkillHUD Skill2
	{
		get
		{
			return mSkill2;
		}
	}

	public ChatBoxHUD ChatBox
	{
		get
		{
			return mChatBoxHUD;
		}
	}

	public GameObject CallBackWhenSave { get; set; }

	public ItemBase NearestItem
	{
		get
		{
			return nearestItem;
		}
		set
		{
			nearestItem = value;
		}
	}

	public ChestScript NearestChest
	{
		get
		{
			return nearestChest;
		}
		set
		{
			nearestChest = value;
		}
	}

	private UserStateHUD()
	{
		mTargetAimed = new GameUnitHUD();
		mDamageList = new ArrayList();
		mInfoBoxHUD = new InfoBoxHUD();
		mEnemyList = new Dictionary<string, GameUnitHUD>();
		mRemotePlayerList = new Dictionary<string, GameUnitHUD>();
		mPlayerList = new Dictionary<string, GameUnitHUD>();
		mQuestList = new Dictionary<string, Vector2>();
		mAttackerList = new List<GameUnitHUD>();
		mSkill1 = new SkillHUD(0);
		mSkill2 = new SkillHUD(1);
		mChatBoxHUD = new ChatBoxHUD();
		mJoystickHUD = new JoystickHUD();
		mVSBattleField = new VSBattleField();
		startCoolDownTime.SetTimer(300f, true);
		mSlotQueueID.Clear();
		mSlotQueueID.Add(0);
		mSlotQueueID.Add(0);
		mSlotQueueID.Add(0);
		mSlotQueueID.Add(0);
	}

	public static UserStateHUD GetInstance()
	{
		if (instance == null)
		{
			instance = new UserStateHUD();
		}
		return instance;
	}

	public void Init()
	{
		Clear();
	}

	public void Clear()
	{
		Debug.Log("Clear");
		CancelTargetAimed();
		mDamageList.Clear();
		mEnemyList.Clear();
		mRemotePlayerList.Clear();
		mPlayerList.Clear();
		mAttackerList.Clear();
		mSkill1.Clear();
		mSkill2.Clear();
		bMissionUpdate = false;
		bWeaponListOpen = false;
		inBattleFieldPoint = -1;
		mVSBattleField.Clear();
		captureSign = TeamName.FreeForAll;
		teamSign = TeamName.FreeForAll;
		bShowFillAllButton = false;
		Debug.Log("***************************HUDClear");
	}

	public CharacterClass GetUserClass()
	{
		return GameApp.GetInstance().GetUserState().GetCharacterClass();
	}

	public string GetUserClassIcon(CharacterClass className)
	{
		switch (className)
		{
		case CharacterClass.Soldier:
			return "pro_zhan";
		case CharacterClass.Prayer:
			return "pro_qi";
		case CharacterClass.Sniper:
			return "pro_ju";
		case CharacterClass.Engineer:
			return "pro_gong";
		case CharacterClass.Stealth:
			return string.Empty;
		default:
			return string.Empty;
		}
	}

	public bool IsUserRoomMaster()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.IsRoomMaster;
		}
		return false;
	}

	public string GetUserClassIconName()
	{
		return GetUserClassIcon(GetUserClass());
	}

	public int GetUserLevel()
	{
		return GameApp.GetInstance().GetUserState().GetCharLevel();
	}

	public string GetUserLevelStr()
	{
		return "[ffeea0]" + GameApp.GetInstance().GetUserState().GetCharLevel() + "[-]";
	}

	public int GetUserSkillPoint()
	{
		return GameApp.GetInstance().GetUserState().SkillTreeManager.GetSkillPointsLeft();
	}

	public int GetUserHp()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Hp;
	}

	public int GetUserMaxHp()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.MaxHp;
	}

	public int GetUserShield()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Shield;
	}

	public int GetUserMaxShield()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.MaxShield;
	}

	public int GetUserExp()
	{
		return GameApp.GetInstance().GetUserState().GetExp();
	}

	public int GetNextLvRequiredExp()
	{
		return GameApp.GetInstance().GetUserState().GetNextLvRequiredExp(GameApp.GetInstance().GetUserState().GetCharLevel());
	}

	public Transform GetUserTransform()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform();
	}

	public bool IsUserDead()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.State == Player.DEAD_STATE;
	}

	public bool IsUserDying()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InFallDownState() || GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.DYING_STATE.InDyingState;
	}

	public bool IsHasGrenade()
	{
		return GameApp.GetInstance().GetGameWorld() != null && GameApp.GetInstance().GetGameWorld().GetLocalPlayer() != null && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.HandGrenade != null;
	}

	public float GetPercentOfUserSave()
	{
		Timer saveTimer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetSaveTimer();
		if (saveTimer.Enable())
		{
			return saveTimer.GetTimeSpan() / saveTimer.GetInterval();
		}
		return 0f;
	}

	public float GetPercentOfAliveTime()
	{
		float interval = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.DYING_STATE.GetDyingTimer().GetInterval();
		float timeSpan = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.DYING_STATE.GetDyingTimer().GetTimeSpan();
		return timeSpan / interval;
	}

	public float GetAliveTime()
	{
		if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InFallDownState())
		{
			return 9f;
		}
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.DYING_STATE.GetDyingTimer().GetInterval() - GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.DYING_STATE.GetDyingTimer().GetTimeSpan();
	}

	public UserBuffHUD[] GetUserBuffs()
	{
		List<CharacterStateSkill> stateSkillList = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCharacterSkillManager()
			.GetStateSkillList();
		List<UserBuffHUD> list = new List<UserBuffHUD>();
		for (int i = 0; i < stateSkillList.Count; i++)
		{
			if (!stateSkillList[i].IsPermanent)
			{
				UserBuffHUD userBuffHUD = new UserBuffHUD();
				userBuffHUD.Name = stateSkillList[i].IconName;
				userBuffHUD.Time = stateSkillList[i].GetLeftTime();
				userBuffHUD.BuffEffect = UserBuffHUD.FullScreenEffect.None;
				if (stateSkillList[i].IsPermanent)
				{
					userBuffHUD.Detail = "Passive";
				}
				else
				{
					userBuffHUD.Detail = stateSkillList[i].GetLeftTime() + "s";
				}
				if (stateSkillList[i].skillID == 31001)
				{
					userBuffHUD.BuffEffect = UserBuffHUD.FullScreenEffect.Avatar;
				}
				else if (stateSkillList[i].skillID == 31076)
				{
					userBuffHUD.BuffEffect = UserBuffHUD.FullScreenEffect.Morphine;
				}
				list.Add(userBuffHUD);
			}
		}
		stateSkillList = null;
		return list.ToArray();
	}

	public string GetBulletInfo()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		ItemInfiniteBullet itemInfiniteBullet = (ItemInfiniteBullet)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
			.GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
		bool flag = itemInfiniteBullet.IsUnlimitedBullet(localPlayer.GetWeapon().GetWeaponType());
		return "[a3ffe8]" + localPlayer.GetWeapon().BulletCountInGun + "/" + ((!flag) ? (string.Empty + localPlayer.GetUserState().GetBulletByWeaponType(localPlayer.GetWeapon().GetWeaponType())) : "/max") + "[-]";
	}

	public string GetGrenadeInfo()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		return "[a3ffe8]" + ((localPlayer.HandGrenade != null) ? (string.Empty + localPlayer.GetUserState().GetBulletByWeaponType(WeaponType.Grenade)) : "-") + "[-]";
	}

	public void SetTargetAimed(GameUnit target)
	{
		if (target != null)
		{
			if (target is Enemy)
			{
				mTargetAimed = CreateGameUnitHUD(target as Enemy);
			}
			else if (target is RemotePlayer)
			{
				mTargetAimed = CreateGameUnitHUD(target as RemotePlayer);
			}
		}
		else
		{
			mTargetAimed = emptyGameUnit;
		}
	}

	private GameUnitHUD CreateGameUnitHUD(Enemy enemy)
	{
		GameUnitHUD gameUnitHUD = new GameUnitHUD();
		gameUnitHUD.type = GameUnitHUD.Type.Enemy;
		gameUnitHUD.target = enemy;
		return gameUnitHUD;
	}

	private GameUnitHUD CreateGameUnitHUD(RemotePlayer remotePlayer)
	{
		GameUnitHUD gameUnitHUD = new GameUnitHUD();
		gameUnitHUD.type = GameUnitHUD.Type.RemotePlayer;
		gameUnitHUD.target = remotePlayer;
		return gameUnitHUD;
	}

	private GameUnitHUD CreateGameUnitHUD()
	{
		GameUnitHUD gameUnitHUD = new GameUnitHUD();
		gameUnitHUD.type = GameUnitHUD.Type.LocalPlayer;
		gameUnitHUD.target = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		return gameUnitHUD;
	}

	public void CancelTargetAimed()
	{
		mTargetAimed = emptyGameUnit;
	}

	public GameUnitHUD GetAimedTarget()
	{
		return null;
	}

	public void PushDamage(int damage, bool critical, ElementType elementType, Vector3 position)
	{
		lock (mutex)
		{
			DamageHUD damageHUD = new DamageHUD();
			damageHUD.Critical = critical;
			damageHUD.Damage = damage;
			damageHUD.ElementType = elementType;
			damageHUD.EnemyPosition = position;
			mDamageList.Add(damageHUD);
		}
	}

	public DamageHUD PopDamage()
	{
		lock (mutex)
		{
			if (mDamageList.Count > 0)
			{
				DamageHUD result = mDamageList[0] as DamageHUD;
				mDamageList.RemoveAt(0);
				return result;
			}
			return null;
		}
	}

	public bool IsMissionUpdate()
	{
		bool result = bMissionUpdate;
		bMissionUpdate = false;
		return result;
	}

	public void UpdateMission()
	{
		bMissionUpdate = true;
	}

	public string GetMissionDetail()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestProgress();
		}
		return GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestProgress();
	}

	public bool IsMissionAccomplished()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return GameApp.GetInstance().GetUserState().m_questStateContainer.CanBeSubmittedMarkQuestProgress();
		}
		return GameApp.GetInstance().GetUserState().m_questStateContainer.CanBeSubmittedCurrentQuestProgress();
	}

	public bool SwitchToNextMission()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return GameApp.GetInstance().GetUserState().m_questStateContainer.SwitchMarkQuestProgress();
		}
		return GameApp.GetInstance().GetUserState().m_questStateContainer.SwitchCurrentQuestProgress();
	}

	public int GetMissionCount()
	{
		return GameApp.GetInstance().GetUserState().m_questStateContainer.m_accStateLst.Count;
	}

	public void SetMissionUpdateDelay(float delay)
	{
		missionUpdateDelay = Mathf.Max(0f, delay);
	}

	public float GetMissionUpdateDelay()
	{
		return missionUpdateDelay;
	}

	public void AddEnemy(Enemy enemy)
	{
		if (!mEnemyList.ContainsKey(enemy.Name))
		{
			GameUnitHUD value = CreateGameUnitHUD(enemy);
			mEnemyList.Add(enemy.Name, value);
		}
	}

	public void RemoveEnemy(string enemyName)
	{
		if (mEnemyList.ContainsKey(enemyName))
		{
			mEnemyList.Remove(enemyName);
		}
	}

	public void ClearEnemy()
	{
		mEnemyList.Clear();
	}

	public Dictionary<string, GameUnitHUD> GetEnemyList()
	{
		return mEnemyList;
	}

	public Dictionary<string, QuestPoint> GetQuestList()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			return GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestPoint();
		}
		return GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestPoint();
	}

	public void AddRemotePlayer(RemotePlayer player)
	{
		Debug.Log("player.GetUserID().ToString() : " + player.GetUserID());
		if (!mRemotePlayerList.ContainsKey(player.GetUserID().ToString()) && (!GameApp.GetInstance().GetGameMode().IsVSMode() || (GameApp.GetInstance().GetGameMode().IsVSMode() && player.Team == GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Team)))
		{
			GameUnitHUD value = CreateGameUnitHUD(player);
			mRemotePlayerList.Add(player.GetUserID().ToString(), value);
			mPlayerList.Add(player.GetUserID().ToString(), value);
		}
	}

	public void RemoveRemotePlayer(string userID)
	{
		Debug.Log("RemoveRemotePlayer");
		if (mRemotePlayerList.ContainsKey(userID))
		{
			mRemotePlayerList.Remove(userID);
			mPlayerList.Remove(userID);
		}
	}

	public void ClearRemotePlayer()
	{
		foreach (KeyValuePair<string, GameUnitHUD> mRemotePlayer in mRemotePlayerList)
		{
			mPlayerList.Remove(mRemotePlayer.Key);
		}
		mRemotePlayerList.Clear();
	}

	public Dictionary<string, GameUnitHUD> GetRemotePlayerList()
	{
		return mRemotePlayerList;
	}

	public void AddLocalPlayer()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		Debug.Log("player.GetUserID().ToString() : " + localPlayer.GetUserID());
		if (!mPlayerList.ContainsKey(localPlayer.GetUserID().ToString()))
		{
			GameUnitHUD value = CreateGameUnitHUD();
			mPlayerList.Add(localPlayer.GetUserID().ToString(), value);
		}
	}

	public void RemoveLocalPlayer()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer != null && mPlayerList.ContainsKey(localPlayer.GetUserID().ToString()))
		{
			mPlayerList.Remove(localPlayer.GetUserID().ToString());
		}
	}

	public Dictionary<string, GameUnitHUD> GetAllPlayerList()
	{
		return mPlayerList;
	}

	public void PushUnitWhoAttacksUser(GameUnit unit)
	{
		if (unit != null && unit is Enemy)
		{
			GameUnitHUD gameUnitHUD = CreateGameUnitHUD(unit as Enemy);
			mAttackerList.Add(gameUnitHUD);
			float num = (MathUtil.GetAngleBetweenUserHorizontal(gameUnitHUD.Position) + 360f) % 360f;
			if (num > 45f && num <= 180f)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.CameraVibrateController.Vibrate(CameraVibrateController.Direction.Left);
			}
			else if (num > 180f && num <= 315f)
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.CameraVibrateController.Vibrate(CameraVibrateController.Direction.Right);
			}
			else
			{
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.CameraVibrateController.Vibrate(CameraVibrateController.Direction.Middle);
			}
		}
	}

	public List<GameUnitHUD> PopUnitWhoAttacksUser()
	{
		List<GameUnitHUD> list = new List<GameUnitHUD>();
		foreach (GameUnitHUD mAttacker in mAttackerList)
		{
			list.Add(mAttacker);
		}
		mAttackerList.Clear();
		return list;
	}

	public string[] GetUserWeapons()
	{
		ItemInfo itemInfoData = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserState()
			.ItemInfoData;
		int currentEquipWeaponSlot = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserState()
			.ItemInfoData.CurrentEquipWeaponSlot;
		List<Weapon> weaponList = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetWeaponList();
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < weaponList.Count; i++)
		{
			if (weaponList[i] != null)
			{
				if (i != currentEquipWeaponSlot)
				{
					list.Add(i);
				}
			}
			else
			{
				list2.Add(i);
			}
		}
		List<int> list3 = new List<int>();
		foreach (int item in list)
		{
			list3.Add(item);
		}
		foreach (int item2 in list2)
		{
			list3.Add(item2);
		}
		mSlotQueueID[0] = currentEquipWeaponSlot;
		for (int j = 1; j < 4; j++)
		{
			mSlotQueueID[j] = list3[j - 1];
		}
		string[] array = new string[4];
		int num = 0;
		NGUIGameItem nGUIGameItem = null;
		bool flag = false;
		bool flag2 = false;
		using (List<int>.Enumerator enumerator3 = mSlotQueueID.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				switch (enumerator3.Current)
				{
				case 0:
					nGUIGameItem = itemInfoData.Weapon1;
					flag = false;
					flag2 = itemInfoData.IsWeapon1Equiped;
					break;
				case 1:
					nGUIGameItem = itemInfoData.Weapon2;
					flag = false;
					flag2 = itemInfoData.IsWeapon2Equiped;
					break;
				case 2:
					nGUIGameItem = itemInfoData.Weapon3;
					flag = !itemInfoData.IsWeapon3Enable;
					flag2 = itemInfoData.IsWeapon3Equiped;
					break;
				default:
					nGUIGameItem = itemInfoData.Weapon4;
					flag = !itemInfoData.IsWeapon4Enable;
					flag2 = itemInfoData.IsWeapon4Equiped;
					break;
				}
				if (flag)
				{
					array[num] = "lock";
				}
				else if (flag2 && nGUIGameItem != null)
				{
					array[num] = nGUIGameItem.baseItem.previewIconName;
				}
				else
				{
					array[num] = "gun";
				}
				num++;
			}
			return array;
		}
	}

	public void ChangeWeapon(int index)
	{
		int currentEquipWeaponSlot = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserState()
			.ItemInfoData.CurrentEquipWeaponSlot;
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.ChangeWeaponInBag(mSlotQueueID[index + 1]);
	}

	public void SetWeaponListOpen(bool state)
	{
		bWeaponListOpen = state;
	}

	public bool IsWeaponListOpen()
	{
		return bWeaponListOpen;
	}

	public void Save()
	{
		CallBackWhenSave = mCallBackWhenSave;
	}

	public bool StopToSave()
	{
		if (CallBackWhenSave != null)
		{
			CallBackWhenSave = null;
			return true;
		}
		return false;
	}

	public void SetDyingRemotoPlayerInSight(GameObject callBack)
	{
		mCallBackWhenSave = callBack;
		if (mCallBackWhenSave == null)
		{
			bDyingManInSight = false;
		}
		else
		{
			bDyingManInSight = true;
		}
	}

	public bool HasDyingRemotePlayerInSight()
	{
		return bDyingManInSight;
	}

	public float GetPercentOfSaveTime()
	{
		float interval = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetFirstAidTimer()
			.GetInterval();
		float timeSpan = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetFirstAidTimer()
			.GetTimeSpan();
		if (interval == 0f)
		{
			return 0f;
		}
		return timeSpan / interval;
	}

	public void ShowInteractButton(ItemBase nearestItem, ChestScript nearestChest)
	{
		this.nearestItem = nearestItem;
		this.nearestChest = nearestChest;
	}

	public void HideInteractButton()
	{
		nearestItem = null;
		nearestChest = null;
	}

	public bool IsReturnCDOK()
	{
		return startCoolDownTime.Ready();
	}

	public float GetPercentOfReturnCD()
	{
		return Mathf.Min(startCoolDownTime.GetTimeSpan() / 300f, 1f);
	}

	public void ResetReturnCD()
	{
		startCoolDownTime.Do();
	}

	public int GetMithrilCostOfReturn()
	{
		return 5;
	}

	public bool IsUserSummonExit()
	{
		mSummonedItem = GameApp.GetInstance().GetGameScene().GetMasterSummonedItem();
		return mSummonedItem != null && mSummonedItem.SummonedType != ESummonedType.TRAPS;
	}

	public float GetSummonHPPercent()
	{
		if (mSummonedItem == null)
		{
			return 0f;
		}
		return (float)mSummonedItem.Hp / (float)mSummonedItem.MaxHp;
	}

	public float GetSummonShieldPercent()
	{
		if (mSummonedItem == null)
		{
			return 0f;
		}
		return (float)mSummonedItem.Shield / (float)mSummonedItem.MaxShield;
	}

	public void UpdateVSBattleFieldState()
	{
		mVSBattleField.SetBattleFieldState();
	}

	public VSBattleField GetVSBattleFieldState()
	{
		return mVSBattleField;
	}

	public TeamName GetUserTeamName(byte owner)
	{
		switch (owner)
		{
		case 1:
			return TeamName.Red;
		case 2:
			return TeamName.Blue;
		default:
			return TeamName.FreeForAll;
		}
	}

	public TeamName GetUserTeamName()
	{
		return GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Team;
	}

	public int GetUserInBattleFieldPoint()
	{
		return inBattleFieldPoint;
	}

	public void SetUserInBattleFieldPoint(int point)
	{
		inBattleFieldPoint = point;
	}

	public void SetUserOutBattleFieldPoint()
	{
		inBattleFieldPoint = -1;
	}

	public void SetVSTDMTeamSignVisible(TeamName teamName)
	{
		teamSign = teamName;
	}

	public TeamName GetVSTDMTeamSignVisible()
	{
		TeamName result = teamSign;
		teamSign = TeamName.FreeForAll;
		return result;
	}

	public void SetVSTDMCaptureSignVisible(TeamName teamName)
	{
		captureSign = teamName;
	}

	public TeamName GetVSTDMCaptureSignVisible()
	{
		TeamName result = captureSign;
		captureSign = TeamName.FreeForAll;
		return result;
	}

	public void SetVSTDMLockVisible()
	{
		bVSTDMLockVisible = true;
	}

	public bool GetVSTDMLockVisible()
	{
		bool result = bVSTDMLockVisible;
		bVSTDMLockVisible = false;
		return result;
	}

	public Dictionary<string, TDMCapturePointScript> GetCapturePointList()
	{
		if (GameApp.GetInstance().GetGameScene() == null)
		{
			return null;
		}
		return GameApp.GetInstance().GetGameScene().GetCapturePoint();
	}

	public float GetRemainUnhurtTimePercent()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			LocalPlayer localPlayer = gameWorld.GetLocalPlayer();
			return localPlayer.RemainUnhurtTime();
		}
		return 0f;
	}

	public bool IsShowFillAllButton()
	{
		return bShowFillAllButton;
	}

	public void SetShowFillAllButton(bool visible)
	{
		bShowFillAllButton = visible;
	}

	public int GetPillCountInBag()
	{
		if (GameApp.GetInstance().GetUserState().ItemInfoData != null)
		{
			return GameApp.GetInstance().GetUserState().ItemInfoData.GetPillCount();
		}
		return 0;
	}
}
