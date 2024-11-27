public abstract class Response
{
	public short responseID;

	public static Response CreateResponse(short responseID)
	{
		Response result = null;
		switch (responseID)
		{
		case 1:
			result = new PlayerLoginResponse();
			break;
		case 7:
			result = new GetRoomListResponse();
			break;
		case 4:
			result = new CreateRoomResponse();
			break;
		case 8:
			result = new GetRoomDataResponse();
			break;
		case 5:
			result = new JoinRoomResponse();
			break;
		case 9:
			result = new StartGameResponse();
			break;
		case 10:
			result = new SetMasterPlayerResponse();
			break;
		case 132:
			result = new PlayerSpawnResponse();
			break;
		case 12:
			result = new PlayerLoginGameServerResponse();
			break;
		case 13:
			result = new RoomTimeSynchronizeResponse();
			break;
		case 100:
			result = new TimeSynchronizeResponse();
			break;
		case 20:
			result = new ChangeSeatResponse();
			break;
		case 30:
			result = new VSQuickMatchResponse();
			break;
		case 32:
			result = new VSTDMStartGameResponse();
			break;
		case 50:
			result = new TestRespones();
			break;
		case 14:
			result = new DownloadMithrilResponse();
			break;
		case 101:
			result = new GetSceneStateResponse();
			break;
		case 102:
			result = new SendTransformStateResponse();
			break;
		case 103:
			result = new SendPlayerInputResponse();
			break;
		case 104:
			result = new EnemySpawnResponse();
			break;
		case 105:
			result = new EnemyChangeTargetResponse();
			break;
		case 106:
			result = new EnemyStateResponse();
			break;
		case 108:
			result = new EnemyOnHitResponse();
			break;
		case 110:
			result = new PlayerOnHitResponse();
			break;
		case 111:
			result = new PlayerChangeWeaponResponse();
			break;
		case 113:
			result = new PlayerLeaveGameResponse();
			break;
		case 114:
			result = new PlayerFireRocketResponse();
			break;
		case 115:
			result = new PlayerHitPlayerResponse();
			break;
		case 116:
			result = new EnemyShotResponse();
			break;
		case 117:
			result = new PlayerUseItemResponse();
			break;
		case 119:
			result = new ItemSpawnResponse();
			break;
		case 120:
			result = new PickUpItemResponse();
			break;
		case 121:
			result = new PlayerOnKnockedResponse();
			break;
		case 122:
			result = new PlayerUploadStatisticsResponse();
			break;
		case 123:
			result = new PlayerBuffResponse();
			break;
		case 124:
			result = new SendPlayerShootAngleVResponse();
			break;
		case 125:
			result = new PlayerRebirthResponse();
			break;
		case 126:
			result = new PlayerKillPlayerResponse();
			break;
		case 129:
			result = new VSGameAutoBalanceResponse();
			break;
		case 133:
			result = new SendVSTimeResponse();
			break;
		case 134:
			result = new ChangeGameSceneResponse();
			break;
		case 135:
			result = new PlayerChangeSceneResponse();
			break;
		case 137:
			result = new UploadEnemyInPointResponse();
			break;
		case 139:
			result = new RequireEnemyInPointResponse();
			break;
		case 138:
			result = new DownloadEnemyInPointResponse();
			break;
		case 140:
			result = new PlayerEnterSpawnPointResponse();
			break;
		case 151:
			result = new DownloadQuestsResponse();
			break;
		case 152:
			result = new UpdatePlayerAccQuestsResponse();
			break;
		case 153:
			result = new UpdatePlayerCmpQuestsResponse();
			break;
		case 154:
			result = new DownloadAccQuestsResponse();
			break;
		case 155:
			result = new DownloadCmpQuestsResponse();
			break;
		case 141:
			result = new PlayerLeaveSpawnPointResponse();
			break;
		case 142:
			result = new EnemyOnDeadResponse();
			break;
		case 143:
			result = new PlayerRefreshWeaponResponse();
			break;
		case 156:
			result = new PlayerFirstAidTeammateResponse();
			break;
		case 157:
			result = new PlayerRecoverFromDyingResponse();
			break;
		case 158:
			result = new PlayerOnDeadResponse();
			break;
		case 159:
			result = new ChatResponse();
			break;
		case 160:
			result = new ChangeQuestMarkResponse();
			break;
		case 162:
			result = new InvitationResponse();
			break;
		case 164:
			result = new PlayerEnterExploreItemBlockResponse();
			break;
		case 165:
			result = new PlayerLeaveExploreItemBlockResponse();
			break;
		case 166:
			result = new UploadExploreItemBlockResponse();
			break;
		case 167:
			result = new DownloadExploreItemBlockResponse();
			break;
		case 168:
			result = new RequireExploreItemBlockResponse();
			break;
		case 169:
			result = new PickUpQuestItemResponse();
			break;
		case 170:
			result = new PlayerChangeAvatarResponse();
			break;
		case 171:
			result = new ControllableItemCreateResponse();
			break;
		case 172:
			result = new ControllableItemStateResponse();
			break;
		case 173:
			result = new ControllableItemOnHitResponse();
			break;
		case 174:
			result = new ControllableItemChangeTargetResponse();
			break;
		case 175:
			result = new ControllableItemDisappearResponse();
			break;
		case 176:
			result = new PlayerHpRecoveryResponse();
			break;
		case 177:
			result = new RemotePlayerHpRecoveryResponse();
			break;
		case 178:
			result = new ItemExploredResponse();
			break;
		case 179:
			result = new BossStateResponse();
			break;
		case 180:
			result = new CreateExtraShieldResponse();
			break;
		case 181:
			result = new ClearExtraShieldResponse();
			break;
		case 182:
			result = new EnemySpeedDownResponse();
			break;
		case 183:
			result = new QuestPointDoesntExistResponse();
			break;
		case 185:
			result = new OpenChestResponse();
			break;
		case 186:
			result = new PlayThirdPersonSkillEffectResponse();
			break;
		case 187:
			result = new PlayerShieldRecoveryResponse();
			break;
		case 188:
			result = new PlayerLevelUpResponse();
			break;
		case 190:
			result = new UpdatePlayerAccQuestSubStateResponse();
			break;
		case 189:
			result = new InvitationFailMessageResponse();
			break;
		case 191:
			result = new UpdateExploreItemBlockResponse();
			break;
		case 192:
			result = new ChangeRemotePlayerStateResponse();
			break;
		case 23:
			result = new VSTDMTotalRankResponse();
			break;
		case 27:
			result = new GetVSTDMRankResponse();
			break;
		case 193:
			result = new VSReadyResponse();
			break;
		case 195:
			result = new PlayRemotePlayerBuffEffectResponse();
			break;
		case 196:
			result = new ClearRemotePlayerBuffEffectResponse();
			break;
		case 197:
			result = new PlayerDotDamageResponse();
			break;
		case 198:
			result = new ControllableItemChangePVPTargetResponse();
			break;
		case 199:
			result = new PlayerHitPlayerImmunityResponse();
			break;
		case 200:
			result = new PlayerMeleeAttackResponse();
			break;
		case 300:
			result = new VSTDMRestartResponse();
			break;
		case 302:
			result = new VSTDMTargetPointInfoResponse();
			break;
		case 303:
			result = new VSTDMCreateTargetPointInfoResponse();
			break;
		case 304:
			result = new VSTDMResultInfoResponse();
			break;
		case 305:
			result = new VSGetBattleStateResponse();
			break;
		case 31:
			result = new JoinVSRoomResponse();
			break;
		case 11:
			result = new QuickJoinResponse();
			break;
		case 33:
			result = new CreateBossRushRoomResponse();
			break;
		}
		return result;
	}

	public virtual void ReadData(byte[] data)
	{
	}

	public abstract void ProcessLogic();

	public virtual void ProcessRobotLogic(RobotUser robot)
	{
	}
}