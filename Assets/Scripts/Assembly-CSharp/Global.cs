using UnityEngine;

public class Global
{
	public const bool ROBOT = true;

	public const int AVATAR_PART_HEAD = 0;

	public const int AVATAR_PART_FACE = 1;

	public const int AVATAR_PART_WAIST = 2;

	public const int TOTAL_WEAPON_NUM = 35;

	public const byte STAGE_STATE_LOCK = 0;

	public const byte STAGE_STATE_UNLOCK = 1;

	public const byte STAGE_STATE_INSTANCE_START_INDEX = 32;

	public const byte STAGE_STATE_NAME_TRAINING = 0;

	public const byte STAGE_STATE_INSTANCE_NAME_CITY0 = 32;

	public const byte STAGE_STATE_INSTANCE_NAME_CITY1 = 37;

	public const byte STAGE_STATE_INSTANCE_NAME_CITY2 = 35;

	public const byte STAGE_STATE_INSTANCE_NAME_CITY3 = 42;

	public const byte STAGE_STATE_INSTANCE_NAME_SHADOWGUN = 36;

	public const byte STAGE_STATE_INSTANCE_NAME_VS1 = 44;

	public const byte STAGE_STATE_INSTANCE_NAME_VS2 = 45;

	public const byte STAGE_STATE_INSTANCE_NAME_VS1_1V1 = 47;

	public const byte STAGE_STATE_INSTANCE_NAME_VS2_1V1 = 48;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE0 = 33;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE_1 = 39;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE_2 = 34;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE_3 = 38;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE_4 = 43;

	public const byte STAGE_STATE_INSTANCE_NAME_INSTANCE_5 = 49;

	public const byte EQUIP_STATE_LOCKED = 0;

	public const byte EQUIP_STATE_UNLOCKED = 15;

	public const int TEAM_SIZE = 4;

	public const int PLAYER_NUM_IN_COOP = 4;

	public const float MIN_BLOOD_EFFECT_INTERVAL = 0.3f;

	public const float UNHURT_TIME = 4f;

	public const float VS_WAIT_REBIRTH_TIME = 15f;

	public const string SCREEN_SHOT_FILENAME = "tempscreens.png";

	public const float CAMERA_RAYCAST_MOVE_DISTANCE = 1.8f;

	public const float DEFENCE_UP_VALUE = 0.85f;

	public const float FIRST_AID_TIMER = 3f;

	public const int WEAPON_TYPE_COUNT = 8;

	public const bool IS_ENEMY_DAMAGE_CHEAT = false;

	public const bool ALL_MAP_UNLOCK = false;

	public const string SOUND_MENU_BACK_PATH = "RPG_Audio/Menu/menu_back";

	public const string SOUND_BROWSE_PATH = "RPG_Audio/Menu/menu_browse";

	public const string SOUND_BUY_PATH = "RPG_Audio/Menu/menu_buy";

	public const string SOUND_OK_PATH = "RPG_Audio/Menu/menu_ok";

	public const string SOUND_POPUP_PATH = "RPG_Audio/Menu/menu_popup";

	public const string SOUND_SELL_PATH = "RPG_Audio/Menu/menu_sell";

	public const string SOUND_OPTION1_PATH = "RPG_Audio/Menu/menu_option01";

	public const string SOUND_OPTION2_PATH = "RPG_Audio/Menu/menu_option02";

	public const string SOUND_FRUITMACHINE_PULL_PATH = "RPG_Audio/Menu/fruit_machine_pull";

	public const string SOUND_FRUITMACHINE_RELEASE_PATH = "RPG_Audio/Menu/fruit_machine_release";

	public const string SOUND_FRUITMACHINE_ROLLING_PATH = "RPG_Audio/Menu/fruit_machine_rolling";

	public const string SOUND_FRUITMACHINE_STOP_PATH = "RPG_Audio/Menu/fruit_machine_stop";

	public const string SOUND_PLAYER_RECOVER_FROM_DYING = "RPG_Audio/Player/saved_teammate";

	public const string SOUND_PLAYER_FIRST_AID = "RPG_Audio/Player/saving_teammate";

	public const string SOUND_PVP_3_STRIKE = "RPG_Audio/VS/PVP_3_strike";

	public const string SOUND_PVP_4_STRIKE = "RPG_Audio/VS/PVP_4_strike";

	public const string SOUND_PVP_5_STRIKE = "RPG_Audio/VS/PVP_5_strike";

	public const string SOUND_PVP_6_STRIKE = "RPG_Audio/VS/PVP_6_strike";

	public const string SOUND_PVP_7UP_STRIKE = "RPG_Audio/VS/PVP_7up_strike";

	public const string SOUND_PVP_FIRST_BLOOD = "RPG_Audio/VS/PVP_first_blood";

	public const string SOUND_PVP_MATCH_START = "RPG_Audio/VS/PVP_match_start";

	public const string SOUND_PVP_MATCH_WIN = "RPG_Audio/VS/PVP_match_win";

	public const string SOUND_PVP_MATCH_LOSE = "RPG_Audio/VS/PVP_match_lose";

	public const string SOUND_PVP_MATCH_COUNT_DOWN = "RPG_Audio/VS/PVP_match_count_down";

	public const string SOUND_PVP_POINT_CAPTURED = "RPG_Audio/VS/PVP_point_captured";

	public const string SOUND_PVP_POINT_LOST = "RPG_Audio/VS/PVP_point_lost";

	public const string SOUND_UPGRADE_OVER = "RPG_Audio/Menu/upgrade_over";

	public const string SOUND_UPGRADEING = "RPG_Audio/Menu/upgradeing";

	public const float UNIT_LEVEL_FACTOR = 0.03f;

	public const float WEAPON_LEVEL_FACTOR = 0.07f;

	public const int LEVEL_TO_HAVE_RPG = 9;

	public const int LEVEL_TO_HAVE_CHIP = 3;

	public const string LOC_ITEM_PARRA_DAMAGE = "LOC_ITEM_PARRA_DAMAGE";

	public const string LOC_ITEM_PARRA_ACCURACY = "LOC_ITEM_PARRA_ACCURACY";

	public const string LOC_ITEM_PARRA_FIRERATE = "LOC_ITEM_PARRA_FIRERATE";

	public const string LOC_ITEM_PARRA_RELOAD_SPEED = "LOC_ITEM_PARRA_RELOAD_SPEED";

	public const string LOC_ITEM_PARRA_CRIT_CHANCE = "LOC_ITEM_PARRA_CRIT_CHANCE";

	public const string LOC_ITEM_PARRA_CRIT_DAMAGE = "LOC_ITEM_PARRA_CRIT_DAMAGE";

	public const string LOC_ITEM_PARRA_RECOIL = "LOC_ITEM_PARRA_RECOIL";

	public const string LOC_ITEM_PARRA_RANGE = "LOC_ITEM_PARRA_RANGE";

	public const string LOC_ITEM_PARRA_FIRE_DAMAGE = "LOC_ITEM_PARRA_FIRE_DAMAGE";

	public const string LOC_ITEM_PARRA_SHOCK_DAMAGE = "LOC_ITEM_PARRA_SHOCK_DAMAGE";

	public const string LOC_ITEM_PARRA_CORRO_DAMAGE = "LOC_ITEM_PARRA_CORRO_DAMAGE";

	public const string LOC_ITEM_PARRA_MELEE_DAMAGE = "LOC_ITEM_PARRA_MELEE_DAMAGE";

	public const string LOC_ITEM_PARRA_WEAPON_ZOOM = "LOC_ITEM_PARRA_WEAPON_ZOOM";

	public const string LOC_ITEM_PARRA_SHIELD_CAP = "LOC_ITEM_PARRA_SHIELD_CAP";

	public const string LOC_ITEM_PARRA_SHIELD_RECOVERY_SPEED = "LOC_ITEM_PARRA_SHIELD_RECOVERY_SPEED";

	public const string LOC_ITEM_PARRA_SHIELD_RECOVERY_DELAY = "LOC_ITEM_PARRA_SHIELD_RECOVERY_DELAY";

	public const string LOC_ITEM_PARRA_SHIELD_FIRE_RESIST = "LOC_ITEM_PARRA_SHIELD_FIRE_RESIST";

	public const string LOC_ITEM_PARRA_SHIELD_SHOCK_RESIST = "LOC_ITEM_PARRA_SHIELD_SHOCK_RESIST";

	public const string LOC_ITEM_PARRA_SHIELD_CORRO_RESIST = "LOC_ITEM_PARRA_SHIELD_CORRO_RESIST";

	public const string LOC_ITEM_PARRA_SHIELD_HP_BONUS = "LOC_ITEM_PARRA_SHIELD_HP_BONUS";

	public const string LOC_ITEM_PARRA_SHIELD_HP_RECOVERY = "LOC_ITEM_PARRA_SHIELD_HP_RECOVERY";

	public const int BULLET_IN_CHEST = 2;

	public const string GAME_ID = "com.ifreyr.bo01";

	public const string GAME_ACHIEVEMENT_SUB_ID = ".achievement_";

	public const int ITEM_SELF_DESTROY_HEIGHT = -75;

	public const int CLEAR_SKILL_POINTS_MITHRIL = 100;

	public const int BUY_SKILL_POINT_MITHRIL = 100;

	public const int BULLET_EXTENT_GOLD_LEVEL = 5;

	public const float FIRST_TIME_BLACK_MARKET_CD = 120f;

	public const int ENTER_BLACK_MARKET_MITHRIL = 25;

	public const int REFRESH_SHOP_MITHRIL = 10;

	public const int REFRESH_BLACK_MARKET_MITHRIL = 15;

	public const int SKILL_MAX_LEVEL = 5;

	public const int RATE_REWARD_MITHRIL = 100;

	public const int FIRST_HUNTER_REWARD_MITHRIL = 100;

	public const int SKILL_ID_AVATAR = 31001;

	public const int SKILL_ID_MORPHINE = 31076;

	public const float BULLET_RECOVER_TIME = 10f;

	public const float ARENA_BULLET_BONUS_RATE = 0.2f;

	public const int VS_TDM_POINTS_COUNT = 5;

	public const byte VS_POINT_OWNER_NO_TEAM = 0;

	public const byte VS_POINT_OWNER_RED_TEAM = 1;

	public const byte VS_POINT_OWNER_BLUE_TEAM = 2;

	public const float GRENADE_EXPLOSION_TIME = 3f;

	public const int MAX_ELEMENT_RESISTANCE = 70;

	public const byte TIPS_PVE = 0;

	public const byte TIPS_PVP = 1;

	public const float PENETRATION_DAMAGE_RATE = 0.3f;

	public const float PVP_DAMAGE_RESILIENCE = 0.4f;

	public const int BAG_EXTEND_MAX_TIMES = 2;

	public static int TOTAL_ACHIEVEMENT_NUM = 1;

	public static byte BAG_DEFAULT_NUM = 4;

	public static int BAG_MAX_NUM = 4;

	public static int DECORATION_PART_NUM = 3;

	public static int TOTAL_ITEM_NUM = 11;

	public static int TOTAL_ITEM_CATEGORY_NUM = 3;

	public static int TOTAL_ITEM_CATEGORY_HP = 6;

	public static int TOTAL_ITEM_CATEGORY_REVIVAL = 2;

	public static int TOTAL_ITEM_CATEGORY_ASSIST = 3;

	public static int[] TOTAL_ITEM_CATEGORY = new int[3] { 6, 2, 3 };

	public static int[] ITEM_HP = new int[6] { 0, 1, 2, 3, 10, 4 };

	public static int[] ITEM_REVIVAL = new int[2] { 5, 6 };

	public static int[] ITEM_ASSIST = new int[3] { 7, 8, 9 };

	public static int TOTAL_AVATAR_NUM = 3;

	public static int TOTAL_ARMOR_NUM = 11;

	public static int TOTAL_ARMOR_HEAD_NUM = 11;

	public static int TOTAL_ARMOR_FACE_NUM = 11;

	public static int TOTAL_ARMOR_WAIST_NUM = 11;

	public static int CAMERA_ZOOM_SPEED = 10;

	public static int FLOORHEIGHT = 0;

	public static int TOTAL_STAGE = 4;

	public static int TOTAL_STAGE_INSTANCE = 19;

	public static byte STORAGE_MAX_PANEL = 6;

	public static byte STORAGE_MAX_NUM = 54;

	public static int MAX_CASH = 999999999;

	public static int MAX_ENEGY = 999999999;

	public static int MAX_MITHRIL = 999999999;

	public static short MAX_CHAR_LEVEL = 25;

	public static int MAX_LEVEL_WEAPONW = 8;

	public static int TOTAL_IAP_CATEGORY_NUM = 2;

	public static int TOTAL_IAP_PURCHASE = 9;

	public static int TOTAL_IAP_EXCHANGE = 6;

	public static int IAP_CATEGORY_PURCHASE = 0;

	public static int IAP_CATEGORY_EXCHANGE = 1;

	public static int[] TOTAL_IAP_CATEGORY = new int[2] { 9, 6 };

	public static int SURVIVAL_DIFFICULTY_UP_EVERY_ROUND = 13;

	public static ThreadPriority Priority = ThreadPriority.Normal;

	public static byte[,] ALL_STATE = new byte[5, 5]
	{
		{ 1, 2, 2, 2, 2 },
		{ 1, 1, 2, 2, 2 },
		{ 1, 1, 0, 2, 2 },
		{ 1, 1, 1, 2, 2 },
		{ 1, 1, 1, 1, 2 }
	};

	public static int[] BAG_EXTEND_MITHRIL = new int[2] { 99, 299 };

	public static int[] EQUIP_TYPE_COUNT = new int[10] { 0, 5, 5, 2, 3, 4, 3, 3, 4, 5 };

	public static float VARIFY_HACK_PLAYER_TIME_INTERVAL = 60f;
}
