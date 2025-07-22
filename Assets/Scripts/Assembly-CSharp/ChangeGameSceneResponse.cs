using UnityEngine;

internal class ChangeGameSceneResponse : Response
{
	public byte playerLength;

	public PlayerInfo[] playerInfos;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerLength = bytesBuffer.ReadByte();
		playerInfos = new PlayerInfo[playerLength];
		for (int i = 0; i < playerLength; i++)
		{
			playerInfos[i] = new PlayerInfo();
			playerInfos[i].channelID = bytesBuffer.ReadInt();
			playerInfos[i].currentCityID = bytesBuffer.ReadByte();
			playerInfos[i].currentSceneID = bytesBuffer.ReadByte();
			playerInfos[i].bagIdOfWeapon = bytesBuffer.ReadByte();
			playerInfos[i].currentElementTypeOfWeapon = bytesBuffer.ReadByte();
			for (int j = 0; j < Global.BAG_MAX_NUM; j++)
			{
				playerInfos[i].weapons[j].mWeaponType = (WeaponType)bytesBuffer.ReadByte();
				playerInfos[i].weapons[j].mWeaponNameNumber = bytesBuffer.ReadByte();
			}
			byte b = bytesBuffer.ReadByte();
			for (byte b2 = 0; b2 < b; b2++)
			{
				SummonedInfo summonedInfo = new SummonedInfo();
				summonedInfo.id = bytesBuffer.ReadByte();
				summonedInfo.level = bytesBuffer.ReadByte();
				summonedInfo.type = bytesBuffer.ReadByte();
				summonedInfo.hp = bytesBuffer.ReadInt();
				summonedInfo.shield = bytesBuffer.ReadInt();
				short num = bytesBuffer.ReadShort();
				short num2 = bytesBuffer.ReadShort();
				short num3 = bytesBuffer.ReadShort();
				summonedInfo.position = new Vector3((float)num / 10f, (float)num2 / 10f, (float)num3 / 10f);
				short num4 = bytesBuffer.ReadShort();
				short num5 = bytesBuffer.ReadShort();
				short num6 = bytesBuffer.ReadShort();
				summonedInfo.rotation = Quaternion.Euler((float)num4 / 10f, (float)num5 / 10f, (float)num6 / 10f);
				summonedInfo.para1 = bytesBuffer.ReadShort();
				summonedInfo.para2 = bytesBuffer.ReadShort();
				summonedInfo.para3 = bytesBuffer.ReadShort();
				summonedInfo.para4 = bytesBuffer.ReadShort();
				summonedInfo.para5 = bytesBuffer.ReadShort();
				summonedInfo.para6 = bytesBuffer.ReadShort();
				summonedInfo.para7 = bytesBuffer.ReadShort();
				summonedInfo.para8 = bytesBuffer.ReadShort();
				summonedInfo.para9 = bytesBuffer.ReadShort();
				summonedInfo.para10 = bytesBuffer.ReadShort();
				summonedInfo.para11 = bytesBuffer.ReadShort();
				summonedInfo.para12 = bytesBuffer.ReadShort();
				summonedInfo.para13 = bytesBuffer.ReadShort();
				summonedInfo.para14 = bytesBuffer.ReadShort();
				summonedInfo.para15 = bytesBuffer.ReadShort();
				summonedInfo.para16 = bytesBuffer.ReadShort();
				playerInfos[i].summonedList.Add(summonedInfo);
			}
		}
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		int channelID = Lobby.GetInstance().GetChannelID();
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		for (int i = 0; i < playerLength; i++)
		{
			PlayerInfo playerInfo = playerInfos[i];
			if (channelID == playerInfo.channelID)
			{
				continue;
			}
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerInfo.channelID);
			if (remotePlayerByUserID == null)
			{
				continue;
			}
			remotePlayerByUserID.SetCurrentCityAndSceneID(playerInfo.currentCityID, playerInfo.currentSceneID);
			bool flag = remotePlayerByUserID.RefreshWeaponList(playerInfo.weapons, playerInfo.bagIdOfWeapon);
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				remotePlayerByUserID.DropAtSpawnPositionVS();
				continue;
			}
			if (playerInfo.currentCityID == GameApp.GetInstance().GetUserState().GetCurrentCityID() && playerInfo.currentSceneID == GameApp.GetInstance().GetGameWorld().CurrentSceneID && flag)
			{
				remotePlayerByUserID.GetObject().SetActive(true);
				remotePlayerByUserID.ActivatePlayer(true);
				remotePlayerByUserID.DropAtSpawnPosition();
				if (remotePlayerByUserID.State == Player.DEAD_STATE)
				{
					remotePlayerByUserID.SetState(Player.IDLE_STATE);
				}
				foreach (SummonedInfo summoned in playerInfo.summonedList)
				{
					remotePlayerByUserID.InitControllableItem(summoned.id, summoned.level, 0, summoned.type, summoned.hp, summoned.shield, summoned.position, summoned.rotation, summoned.para1, summoned.para2, summoned.para3, summoned.para4, summoned.para5, summoned.para6, summoned.para7, summoned.para8, summoned.para9, summoned.para10, summoned.para11, summoned.para12, summoned.para13, summoned.para14, summoned.para15, summoned.para16);
				}
			}
			else
			{
				remotePlayerByUserID.GetObject().SetActive(false);
			}
			remotePlayerByUserID.GetWeapon().mCurrentElementType = (ElementType)playerInfo.currentElementTypeOfWeapon;
			if (playerInfo.currentElementTypeOfWeapon == 6)
			{
				remotePlayerByUserID.GetWeapon().SetAllElementParaForRemotePlayer();
			}
		}
		TimeManager.GetInstance().InitNetworkTimer();
		GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
	}
}
