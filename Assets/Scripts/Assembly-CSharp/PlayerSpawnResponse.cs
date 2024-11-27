using UnityEngine;

internal class PlayerSpawnResponse : Response
{
	public int channelID;

	public byte seatID;

	public PlayerInfo playerInfo;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerInfo = new PlayerInfo();
		channelID = bytesBuffer.ReadInt();
		playerInfo.roleName = bytesBuffer.ReadString();
		seatID = bytesBuffer.ReadByte();
		playerInfo.currentCityID = bytesBuffer.ReadByte();
		playerInfo.currentSceneID = bytesBuffer.ReadByte();
		playerInfo.bagIdOfWeapon = bytesBuffer.ReadByte();
		playerInfo.currentElementTypeOfWeapon = bytesBuffer.ReadByte();
		playerInfo.hp = bytesBuffer.ReadInt();
		playerInfo.maxhp = bytesBuffer.ReadInt();
		playerInfo.shield = bytesBuffer.ReadInt();
		playerInfo.maxshield = bytesBuffer.ReadInt();
		playerInfo.extrashield = bytesBuffer.ReadInt();
		playerInfo.characterClass = bytesBuffer.ReadByte();
		playerInfo.sex = bytesBuffer.ReadByte();
		playerInfo.characterLevel = bytesBuffer.ReadShort();
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			playerInfo.weapons[i].mWeaponType = (WeaponType)bytesBuffer.ReadByte();
			playerInfo.weapons[i].mWeaponNameNumber = bytesBuffer.ReadByte();
		}
		for (int j = 0; j < Global.DECORATION_PART_NUM; j++)
		{
			playerInfo.armors[j] = bytesBuffer.ReadByte();
		}
		playerInfo.avatarID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Debug.Log("receive player spawn..");
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		if (!GameApp.GetInstance().GetGameWorld().RemotePlayerExists(channelID))
		{
			RemotePlayer remotePlayer = new RemotePlayer();
			remotePlayer.SetUserID(channelID);
			remotePlayer.SetDisplayName(playerInfo.roleName);
			remotePlayer.SetSeatID(seatID);
			remotePlayer.Team = (TeamName)(remotePlayer.GetSeatID() / 4);
			remotePlayer.CreateUserState(playerInfo.armors, playerInfo.characterClass, playerInfo.sex, playerInfo.characterLevel, playerInfo.avatarID);
			remotePlayer.Init();
			remotePlayer.RefreshAvatar();
			remotePlayer.Hp = playerInfo.hp;
			remotePlayer.MaxHp = playerInfo.maxhp;
			remotePlayer.Shield = playerInfo.shield;
			remotePlayer.MaxShield = playerInfo.maxshield;
			remotePlayer.CreateExtraShieldWithEffect(playerInfo.extrashield);
			Debug.Log("remotePlayer max HP:" + playerInfo.maxhp);
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
			GameApp.GetInstance().GetGameWorld().AddRemotePlayer(remotePlayer);
			GameApp.GetInstance().GetGameWorld().RecalculateEnemyAbility();
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
			gameScene.VSTimeStopResume();
			Debug.Log("spawn remoteplayer :" + channelID);
		}
		AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Operating_Room);
		achievementTrigger.PutData(GameApp.GetInstance().GetGameWorld().GetRemotePlayers()
			.Count);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
		}
	}
