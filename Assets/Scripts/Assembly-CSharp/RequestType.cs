public class RequestType
{
	public const short PlayerLogin = 1;

	public const short PlayerLogout = 2;

	public const short PlayerRegister = 3;

	public const short CreateRoom = 4;

	public const short JoinRoom = 5;

	public const short LeaveRoom = 6;

	public const short GetRoomList = 7;

	public const short GetRoomData = 8;

	public const short StartGame = 9;

	public const short SearchRoom = 10;

	public const short QuickJoin = 11;

	public const short PlayerLoginGameServerResponse = 12;

	public const short RoomTimeSynchronize = 13;

	public const short SetRoomPing = 14;

	public const short UploadData = 15;

	public const short UploadMithril = 16;

	public const short UploadArmorAndBag = 17;

	public const short CreateVSRoom = 18;

	public const short PlayerJoinTeamStartGame = 19;

	public const short ChangeSeat = 20;

	public const short SearchRoomAdvanced = 21;

	public const short UploadBattleState = 22;

	public const short UploadOperating = 23;

	public const short UploadPlayerQuests = 24;

	public const short GuestLogin = 25;

	public const short UploadTwitterAndFacebook = 26;

	public const short GetVSTDMRank = 27;

	public const short VSChangeSubMode = 29;

	public const short VSQuickMatch = 30;

	public const short JoinVSRoom = 31;

	public const short StartGameInVSRoom = 32;

	public const short ChangeSeatInVSRoom = 33;

	public const short CreateBossRushRoom = 34;

	public const short JoinBossRushRoom = 35;

	public const short Test = 50;

	public const short TimeSynchronize = 100;

	public const short GetSceneState = 101;

	public const short SendTransformState = 102;

	public const short SendPlayerInput = 103;

	public const short EnemySpawn = 104;

	public const short EnemyChangeTarget = 105;

	public const short EnemyState = 106;

	public const short EnemyHit = 107;

	public const short EnemyOnHit = 108;

	public const short EnemyDead = 109;

	public const short PlayerOnHit = 110;

	public const short PlayerChangeWeapon = 111;

	public const short PlayerChangeArmor = 112;

	public const short PlayerFireRocket = 114;

	public const short PlayerHitPlayer = 115;

	public const short EnemyShot = 116;

	public const short PlayerUseItem = 117;

	public const short PlayerHpRecovery = 118;

	public const short ItemSpawn = 119;

	public const short PickUpItem = 120;

	public const short PlayerOnKnocked = 121;

	public const short PlayerBuff = 123;

	public const short SendPlayerShootAngleV = 124;

	public const short PlayerRebirth = 125;

	public const short SendVSTime = 133;

	public const short ChangeGameScene = 134;

	public const short EnemyShieldRecovery = 135;

	public const short UploadEnemyInCity = 136;

	public const short UploadEnemyInPoint = 137;

	public const short DownloadEnemyInPoint = 138;

	public const short RequireEnemyInPoint = 139;

	public const short PlayerLeaveSpawnPoint = 141;

	public const short PlayerRefreshWeapon = 143;

	public const short DownloadQuestsRequest = 151;

	public const short UpdatePlayerAccQuests = 152;

	public const short UpdatePlayerCmpQuests = 153;

	public const short UpdatePlayerAccQuestSubState = 154;

	public const short PlayerFirstAidTeammate = 156;

	public const short PlayerRecoverFromDying = 157;

	public const short PlayerOnDead = 158;

	public const short Chat = 159;

	public const short ChangeQuestMark = 160;

	public const short PlayerShieldRecovery = 161;

	public const short Invation = 162;

	public const short InvitationComfirm = 163;

	public const short RequireExploreItemBlock = 164;

	public const short UploadExploreItemBlock = 165;

	public const short DownloadExploreItemBlock = 166;

	public const short PlayerLeaveExploreItemBlock = 167;

	public const short ItemExplored = 168;

	public const short PickUpQuestItem = 169;

	public const short PlayerChangeAvatar = 170;

	public const short ControllableItemCreate = 171;

	public const short ControllableItemState = 172;

	public const short ControllableItemOnHit = 173;

	public const short ControllableItemChangeTarget = 174;

	public const short ControllableItemDisappear = 175;

	public const short ControllableItemShieldRecovery = 176;

	public const short RemoveSpawnPoint = 177;

	public const short RemotePlayerHpRecovery = 178;

	public const short CreateExtraShield = 180;

	public const short ClearExtraShield = 181;

	public const short EnemySpeedDown = 182;

	public const short EnemySpeedDownOver = 183;

	public const short ControllableItemSendTransform = 184;

	public const short OpenChest = 185;

	public const short PlayThirdPersonSkillEffect = 186;

	public const short EnemyFullHpShield = 187;

	public const short ChangePlayerSubGameMode = 188;

	public const short PlayerLevelUpRequest = 189;

	public const short InvitationFailMessageRequest = 190;

	public const short UpdateExploreItemBlock = 191;

	public const short ChangeRemotePlayerState = 192;

	public const short VSReady = 193;

	public const short UpdateDamagePara = 194;

	public const short PlayRemotePlayerBuffEffect = 195;

	public const short ClearRemotePlayerBuffEffect = 196;

	public const short ControllableItemChangePVPTarget = 197;

	public const short VSRewaiting = 198;

	public const short PlayerMeleeAttack = 200;

	public const short VS_TDM_CAPTURING_POINT = 301;

	public const short VS_TDM_CREATE_TARGET_POINT_INFO = 303;

	public const short VarifyHackPlayer = 304;
}
