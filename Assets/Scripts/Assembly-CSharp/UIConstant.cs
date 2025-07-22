using UnityEngine;

public class UIConstant
{
	public const byte DATATABLE_ENEMY_DROP = 0;

	public const byte DATATABLE_CHEST_DROP = 1;

	public const byte DATATABLE_EQUIP_CLASS = 2;

	public const byte DATATABLE_SHIELD_CLASS = 3;

	public const byte DATATABLE_SLOT_CLASS = 4;

	public const byte DATATABLE_PILL_CLASS = 5;

	public const byte DATATABLE_MONEY_CLASS = 6;

	public const byte DATATABLE_BACKPACK_CLASS = 7;

	public const byte DATATABLE_SPECIAL_ITEM = 8;

	public const byte DATATABLE_SUBMACHINE_GUN_NAME = 9;

	public const byte DATATABLE_ASSAULT_RIFLE_NAME = 10;

	public const byte DATATABLE_PISTOL_NAME = 11;

	public const byte DATATABLE_REVOLVER_NAME = 12;

	public const byte DATATABLE_SHOTGUN_NAME = 13;

	public const byte DATATABLE_SNIPER_NAME = 14;

	public const byte DATATABLE_RPG_NAME = 15;

	public const byte DATATABLE_GRENADE_NAME = 16;

	public const byte DATATABLE_U_SHIELD_NAME = 17;

	public const byte DATATABLE_V_SLOT_NAME = 18;

	public const byte DATATABLE_W_PILLS_NAME = 19;

	public const byte DATATABLE_X_MONEY_NAME = 20;

	public const byte DATATABLE_Z_BACKPACK_NAME = 21;

	public const byte DATATABLE_SKILL_SCORE = 22;

	public const byte DATATABLE_WEAPON_SKILL_1 = 23;

	public const byte DATATABLE_WEAPON_SKILL_2 = 24;

	public const byte DATATABLE_SHIELD_SKILL_1 = 25;

	public const byte DATATABLE_SHIELD_SKILL_2 = 26;

	public const byte DATATABLE_SLOT_SKILL_1 = 27;

	public const byte DATATABLE_SLOT_SKILL_2 = 28;

	public const byte DATATABLE_SKILLS = 40;

	public const byte DATATABLE_BUFFS = 41;

	public const byte DATATABLE_SKILL_TREE = 42;

	public const byte DATATABLE_EQUIP_PREFIX = 43;

	public const byte DATATABLE_CHIP_PREFIX = 73;

	public const byte DATATABLE_AREA = 44;

	public const byte DATATABLE_SCENE = 45;

	public const byte DATATABLE_INSTANCE_PORTAL = 52;

	public const byte DATATABLE_ENEMY_CONFIG = 38;

	public const byte DATATABLE_ENEMY_SPAWN = 39;

	public const byte DATATABLE_DECORATION_HEAD = 46;

	public const byte DATATABLE_DECORATION_FACE = 47;

	public const byte DATATABLE_DECORATION_WAIST = 48;

	public const byte DATATABLE_AVATAR = 50;

	public const byte DATATABLE_ACHIEVEMENT = 49;

	public const byte DATATABLE_QUEST_MAIN = 29;

	public const byte DATATABLE_QUEST_EVENT_COUNT = 7;

	public const byte DATATABLE_QUEST_NPC = 37;

	public const byte DATATABLE_QUEST_ENEMY_SPAWN = 51;

	public const byte DATATABLE_INFINITE_ENEMY_SPAWN = 53;

	public const byte DATATABLE_TIPS = 74;

	public const byte DATATABLE_BOSS_RUSH = 75;

	public const string EMPTY_STRING = "[EMPTY]";

	public const int QUEST_FINAL_COMMONID = 66;

	public static Vector2 ScreenAdaptived;

	public static float ScreenLocalWidth = 960f;

	public static float ScreenLocalHeight = 640f;

	public static string HEY = "My Gears! ";

	public static string FULL_VERSION_URL = "https://itunes.apple.com/us/app/bounty-hunter-black-dawn/id657268954?ls=1&mt=8";

	public static string FACEBOOK_HOME = "http://www.facebook.com/pages/Freyr-Games/159454344141173";

	public static string TWITTER_HOME = "https://twitter.com/#!/FreyrGames";

	public static string RATE_PAGE_URL = "https://itunes.apple.com/us/app/bounty-hunter-black-dawn/id657268954?ls=1&mt=8";

	public static int[] GIFT_DAILY = new int[5] { 5, 10, 20, 30, 50 };

	public static Color[] COLOR_QUEST_DIFFICULTY = new Color[5]
	{
		new Color(0.7490196f, 0.7490196f, 0.7490196f),
		new Color(0.57254905f, 0.8156863f, 16f / 51f),
		new Color(1f, 1f, 0f),
		new Color(0.8862745f, 0.41960785f, 2f / 51f),
		new Color(1f, 0f, 0f)
	};

	public static Color[] COLOR_PLAYER_ICONS = new Color[8]
	{
		new Color(1f, 0.2f, 0f, 1f),
		new Color(1f, 0.6f, 0f, 1f),
		new Color(1f, 1f, 0f, 1f),
		new Color(0.2f, 1f, 0f, 1f),
		new Color(0f, 1f, 1f, 1f),
		new Color(0f, 0.6f, 1f, 1f),
		new Color(0f, 0.2f, 1f, 1f),
		new Color(0.6f, 0f, 1f, 1f)
	};

	public static Color[] COLOR_TEAM_PLAYER = new Color[2]
	{
		new Color(1f, 0f, 0.5294118f, 1f),
		new Color(0f, 0.7019608f, 1f, 1f)
	};

	public static Color[] COLOR_TEAM_TARGET_POINT = new Color[3]
	{
		new Color(1f, 0f, 0.5294118f, 1f),
		new Color(0f, 0.7019608f, 1f, 1f),
		new Color(1f, 1f, 1f, 1f)
	};

	public static Color COLOR_WIN_VALUE = new Color(1f, 1f, 1f, 1f);

	public static Color COLOR_PLAYER_SCORE = new Color(1f, 0f, 0f, 1f);

	public static Color COLOR_POPULATION = new Color(0f, 0.7019608f, 1f, 1f);

	public static string[] QUEST_ATTR_SPRITE = new string[2] { "subbk", "mainbk" };

	public static string[] QUEST_STATE_SPRITE = new string[3] { "unaccept", "uncomplete", "complete" };

	public static byte QUEST_STATE_CAN_ACCEPT = 0;

	public static byte QUEST_STATE_HAVE_ACCEPTED = 1;

	public static byte QUEST_STATE_CAN_SUBMIT = 2;

	public static string[] QUEST_SEAT_SPRITE = new string[4] { "parner1", "parner2", "parner3", "parner4" };

	public static string[] QUEST_STATE_BUTTON = new string[3] { "unaccept", "uncomplete", "complete" };

	public static string FormatNum(int val)
	{
		return string.Format("{0:N0}", val);
	}
}
