using UnityEngine;

internal class GetSceneStateResponse : Response
{
	public byte playerLength;

	public PlayerInfo[] playerInfos;

	public byte mithrilDropRate;

	public float mithrilDropRateAttenuation;

	public byte minDrop;

	public byte maxDrop;

	public short pvpKillCash;

	public short pvpAssistCash;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerLength = bytesBuffer.ReadByte();
		playerInfos = new PlayerInfo[playerLength];
		for (int i = 0; i < playerLength; i++)
		{
			playerInfos[i] = new PlayerInfo();
			playerInfos[i].channelID = bytesBuffer.ReadInt();
			playerInfos[i].roleName = bytesBuffer.ReadString();
			playerInfos[i].seatID = bytesBuffer.ReadByte();
			playerInfos[i].currentCityID = bytesBuffer.ReadByte();
			playerInfos[i].currentSceneID = bytesBuffer.ReadByte();
			playerInfos[i].bagIdOfWeapon = bytesBuffer.ReadByte();
			playerInfos[i].currentElementTypeOfWeapon = bytesBuffer.ReadByte();
			playerInfos[i].hp = bytesBuffer.ReadInt();
			playerInfos[i].maxhp = bytesBuffer.ReadInt();
			playerInfos[i].shield = bytesBuffer.ReadInt();
			playerInfos[i].maxshield = bytesBuffer.ReadInt();
			playerInfos[i].extrashield = bytesBuffer.ReadInt();
			Debug.Log("playerInfos[i].hp: " + playerInfos[i].hp);
			Debug.Log("playerInfos[i].maxhp: " + playerInfos[i].maxhp);
			playerInfos[i].characterClass = bytesBuffer.ReadByte();
			playerInfos[i].sex = bytesBuffer.ReadByte();
			playerInfos[i].characterLevel = bytesBuffer.ReadShort();
			for (int j = 0; j < Global.BAG_MAX_NUM; j++)
			{
				playerInfos[i].weapons[j].mWeaponType = (WeaponType)bytesBuffer.ReadByte();
				playerInfos[i].weapons[j].mWeaponNameNumber = bytesBuffer.ReadByte();
			}
			for (int k = 0; k < Global.DECORATION_PART_NUM; k++)
			{
				playerInfos[i].armors[k] = bytesBuffer.ReadByte();
			}
			playerInfos[i].avatarID = bytesBuffer.ReadByte();
		}
		mithrilDropRate = bytesBuffer.ReadByte();
		minDrop = bytesBuffer.ReadByte();
		maxDrop = bytesBuffer.ReadByte();
		mithrilDropRateAttenuation = (float)(int)bytesBuffer.ReadByte() / 100f;
		pvpKillCash = bytesBuffer.ReadShort();
		pvpAssistCash = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameWorld.NeedGetSeneceState = false;
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		int channelID = Lobby.GetInstance().GetChannelID();
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		Debug.Log("playerLength: " + playerLength);
		for (int i = 0; i < playerLength; i++)
		{
			PlayerInfo playerInfo = playerInfos[i];
			if (channelID != playerInfo.channelID)
			{
				RemotePlayer remotePlayer = new RemotePlayer();
				remotePlayer.SetUserID(playerInfo.channelID);
				remotePlayer.SetDisplayName(playerInfo.roleName);
				remotePlayer.SetSeatID(playerInfo.seatID);
				remotePlayer.Team = (TeamName)(remotePlayer.GetSeatID() / 4);
				remotePlayer.CreateUserState(playerInfo.armors, playerInfo.characterClass, playerInfo.sex, playerInfo.characterLevel, playerInfo.avatarID);
				remotePlayer.Init();
				remotePlayer.RefreshAvatar();
				remotePlayer.Hp = playerInfo.hp;
				remotePlayer.MaxHp = playerInfo.maxhp;
				remotePlayer.Shield = playerInfo.shield;
				remotePlayer.MaxShield = playerInfo.maxshield;
				remotePlayer.CreateExtraShieldWithEffect(playerInfo.extrashield);
				remotePlayer.SetCurrentCityAndSceneID(playerInfo.currentCityID, playerInfo.currentSceneID);
				bool flag = remotePlayer.RefreshWeaponList(playerInfo.weapons, playerInfo.bagIdOfWeapon);
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					remotePlayer.DropAtSpawnPositionVS();
					remotePlayer.CreatePlayerNameSign();
				}
				else if (playerInfo.currentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && playerInfo.currentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID && flag)
				{
					remotePlayer.GetObject().SetActive(true);
					remotePlayer.DropAtSpawnPosition();
				}
				else
				{
					remotePlayer.GetObject().SetActive(false);
				}
				remotePlayer.GetWeapon().mCurrentElementType = (ElementType)playerInfo.currentElementTypeOfWeapon;
				if (playerInfo.currentElementTypeOfWeapon == 6)
				{
					remotePlayer.GetWeapon().SetAllElementParaForRemotePlayer();
				}
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
				}
				gameWorld.AddRemotePlayer(remotePlayer);
			}
		}
		gameWorld.GetLocalPlayer().SetUserID(Lobby.GetInstance().GetChannelID());
		gameWorld.CreateTeamSkills();
		MithrilDropInfo mithrilDropInfo = new MithrilDropInfo();
		mithrilDropInfo.dropRate = (float)(int)mithrilDropRate * 1f / 100f;
		mithrilDropInfo.minDrop = minDrop;
		mithrilDropInfo.maxDrop = maxDrop;
		mithrilDropInfo.dropRateAttenuation = mithrilDropRateAttenuation;
		gameScene.MithrilDrops = mithrilDropInfo;
		Debug.Log("drop rate:" + gameScene.MithrilDrops.dropRate + ", attenuation: " + gameScene.MithrilDrops.dropRateAttenuation + ", " + gameScene.MithrilDrops.minDrop + "~" + gameScene.MithrilDrops.maxDrop);
		gameScene.PVPReward.cashPerKill = pvpKillCash;
		gameScene.PVPReward.cashPerAssist = pvpAssistCash;
		Debug.Log("pvp rewards:" + gameScene.PVPReward.cashPerKill + "," + gameScene.PVPReward.cashPerAssist);
		UserState userState = GameApp.GetInstance().GetUserState();
		gameScene.VSTimeStopResume();
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			gameWorld.UploadEnemyInCurrentCity();
		}
		else
		{
			gameWorld.ClearCityInfo();
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			VSTDMCreateTargetPointInfoRequest request = new VSTDMCreateTargetPointInfoRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			DownloadQuestsRequest request2 = new DownloadQuestsRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
		}
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Operating_Room);
		achievementTrigger.PutData(GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
			.Count);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}

		public override void ProcessRobotLogic(RobotUser robot)
		{
		}
	}
