using UnityEngine;

internal class PlayerRebirthResponse : Response
{
	protected int playerID;

	protected short spawnPointIndex;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		spawnPointIndex = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
			if (null != component)
			{
				component.ResetAngleV();
			}
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				localPlayer.DropAtSpawnPositionVS();
			}
			else
			{
				localPlayer.ReSpawnAtPoint(spawnPointIndex);
			}
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			GameMode gameMode = GameApp.GetInstance().GetGameMode();
			if (gameMode.SubModePlay == SubMode.Boss)
			{
				gameWorld.ChangeSubModeFromBossToStory();
			}
			localPlayer.ClearExtraShieldWithoutEffect();
			if (localPlayer.HealingEffect != null && !localPlayer.IsInstantHealing)
			{
				localPlayer.RemoveHealingEffect();
			}
			localPlayer.GetCharacterSkillManager().RemoveAllBuff();
			localPlayer.ClearExtraShield();
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				remotePlayerByUserID.DropAtSpawnPositionVS();
			}
			else
			{
				remotePlayerByUserID.ReSpawnAtPoint(spawnPointIndex);
			}
			GameApp.GetInstance().GetGameWorld().CreateTeamSkills();
			remotePlayerByUserID.ClearExtraShieldWithoutEffect();
		}
	}
}
