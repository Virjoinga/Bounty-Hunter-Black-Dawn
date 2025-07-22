using UnityEngine;

internal class PlayerKillPlayerResponse : Response
{
	protected int killerID;

	protected int assistID;

	protected int killedID;

	protected short killCount;

	protected short killScore;

	protected byte firstBlood;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		killerID = bytesBuffer.ReadInt();
		assistID = bytesBuffer.ReadInt();
		killedID = bytesBuffer.ReadInt();
		killCount = bytesBuffer.ReadShort();
		killScore = bytesBuffer.ReadShort();
		firstBlood = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Debug.Log(killerID + " Killed " + killedID + "! Assist: " + assistID + "   Kill Streak: " + killCount + "   Kill Score: " + killScore);
		Player player = GetPlayer(killerID);
		Player player2 = GetPlayer(assistID);
		Player player3 = GetPlayer(killedID);
		GMBattleState gMBattleState = GMBattleState.GM_TDM_STATE;
		if (GameApp.GetInstance().GetGameMode().ModePlay != Mode.VS_TDM)
		{
			return;
		}
		gMBattleState = GMBattleState.GM_TDM_STATE;
		if (player != null)
		{
			Debug.Log("Killer----" + player.GetDisplayName() + "||||||Streak-----" + killCount);
			((TDMState)player.GetUserState().GetBattleStates()[(int)gMBattleState]).AtomicKills();
		}
		if (player2 != null)
		{
			Debug.Log("Assist----" + player2.GetDisplayName());
			((TDMState)player2.GetUserState().GetBattleStates()[(int)gMBattleState]).AtomicAssist();
		}
		if (player3 != null)
		{
			Debug.Log("Dead----" + player3.GetDisplayName());
			((TDMState)player3.GetUserState().GetBattleStates()[(int)gMBattleState]).AtomicDead();
			if (killedID == Lobby.GetInstance().GetChannelID())
			{
				InGameMenuManager.GetInstance().Close();
			}
		}
		if (player != null && player3 != null)
		{
			UserStateHUD.GetInstance().InfoBox.PushKillInfo(player, player3);
		}
		if (player != null && killCount > 1)
		{
			UserStateHUD.GetInstance().InfoBox.PushMulitKillInfo(player, killCount);
		}
		if (firstBlood == 1)
		{
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_first_blood");
			UserStateHUD.GetInstance().InfoBox.PushFirstBloodInfo(player);
		}
		switch (killCount)
		{
		case 0:
		case 1:
		case 2:
			break;
		case 3:
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_3_strike");
			break;
		case 4:
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_4_strike");
			break;
		case 5:
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_5_strike");
			break;
		case 6:
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_6_strike");
			break;
		default:
			AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_7up_strike");
			break;
		}
	}

	private Player GetPlayer(int id)
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (id == channelID)
		{
			return localPlayer;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(id);
		if (remotePlayerByUserID != null)
		{
			return remotePlayerByUserID;
		}
		return null;
	}
}
