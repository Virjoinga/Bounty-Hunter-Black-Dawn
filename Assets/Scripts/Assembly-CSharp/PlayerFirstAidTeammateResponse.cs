using UnityEngine;

public class PlayerFirstAidTeammateResponse : Response
{
	protected int m_playerId;

	protected byte m_state;

	protected int m_teammateId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerId = bytesBuffer.ReadInt();
		m_teammateId = bytesBuffer.ReadInt();
		m_state = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		FirstAidPhase state = (FirstAidPhase)m_state;
		Debug.Log("FirstAidPhase: " + state);
		if (m_playerId == channelID)
		{
			switch (state)
			{
			case FirstAidPhase.Failed:
				UserStateHUD.GetInstance().InfoBox.PushInfo(LocalizationManager.GetInstance().GetString("MSG_FIRSTAID_FAILED"));
				UserStateHUD.GetInstance().StopToSave();
				break;
			case FirstAidPhase.Begin:
			{
				LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer.StartFirstAidTimer();
				localPlayer.StopSpecialAction();
				localPlayer.SetState(Player.FIRST_AID_STATE);
				UserStateHUD.GetInstance().Save();
				break;
			}
			default:
				UserStateHUD.GetInstance().StopToSave();
				break;
			}
			return;
		}
		if (channelID == m_teammateId)
		{
			switch (state)
			{
			case FirstAidPhase.Begin:
			{
				LocalPlayer localPlayer3 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer3.StartSaveTimer();
				localPlayer3.DYING_STATE.GetDyingTimer().Pause();
				break;
			}
			case FirstAidPhase.Failed:
			{
				LocalPlayer localPlayer4 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				localPlayer4.StopSaveTimer();
				localPlayer4.DYING_STATE.GetDyingTimer().Resume();
				break;
			}
			case FirstAidPhase.Success:
			{
				LocalPlayer localPlayer2 = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
				if (localPlayer2.InPlayingState())
				{
					AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.First_Friend);
					achievementTrigger.PutData(1);
					AchievementManager.GetInstance().Trigger(achievementTrigger);
					achievementTrigger = AchievementTrigger.Create(AchievementID.Right_here_Right_now);
					achievementTrigger.PutData((int)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.DYING_STATE.GetDyingTimer().GetTimeSpan());
					AchievementManager.GetInstance().Trigger(achievementTrigger);
					localPlayer2.StopSaveTimer();
					PlayerRecoverFromDyingRequest request = new PlayerRecoverFromDyingRequest((short)(localPlayer2.RebirthHealthPercentage * 100f));
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				break;
			}
			}
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerId);
		if (remotePlayerByUserID != null && remotePlayerByUserID.InSameScene())
		{
			switch (state)
			{
			case FirstAidPhase.Begin:
				remotePlayerByUserID.SetState(Player.FIRST_AID_STATE);
				break;
			case FirstAidPhase.Failed:
				remotePlayerByUserID.SetState(Player.IDLE_STATE);
				break;
			case FirstAidPhase.Success:
				remotePlayerByUserID.SetState(Player.IDLE_STATE);
				break;
			}
		}
	}
}
