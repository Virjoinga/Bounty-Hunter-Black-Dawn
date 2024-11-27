using System;
using UnityEngine;

public class PlayerLoginGameServerResponse : Response
{
	private bool loginSuccess;

	private int channelID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		if (b == 1)
		{
			loginSuccess = true;
		}
		else
		{
			loginSuccess = false;
		}
		channelID = bytesBuffer.ReadInt();
		Debug.Log("loginResult:" + b);
	}

	public override void ProcessLogic()
	{
		if (loginSuccess)
		{
			TimeManager.GetInstance().LastSynTime = Time.time;
			Lobby.GetInstance().SetChannelID(channelID);
			UploadDataRequest request = new UploadDataRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 9)
			{
				UploadQuestsRequest request2 = new UploadQuestsRequest(GameApp.GetInstance().GetUserState().m_questStateContainer);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				UITeam.m_instance.m_roomList.LoginSuccess();
				GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			}
			else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 41)
			{
				UIVSTeam.m_instance.m_roomList.LoginSuccess();
				GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			}
			else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 43)
			{
				UIBossRushTeam.m_instance.m_roomList.LoginSuccess();
				GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			}
			else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 37)
			{
				UIVS.instance.NotifyLoginSuccess();
				GameApp.GetInstance().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			}
			TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
			TimeSynchronizeRequest request3 = new TimeSynchronizeRequest(0, (long)timeSpan.TotalMilliseconds);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.Journey);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			GameApp.GetInstance().GetGameMode().TypeOfNetwork = NetworkType.MultiPlayer_Internet;
		}
		else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 9)
		{
			if (!(UITeam.m_instance == null))
			{
				UIMsgBox.instance.ShowSystemMessage(UITeam.m_instance.m_roomList, LocalizationManager.GetInstance().GetString("MSG_NET_LOGIN_SERVER_FAILED"), 2, 17);
			}
		}
		else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 41)
		{
			if (!(UIVSTeam.m_instance == null))
			{
				UIMsgBox.instance.ShowSystemMessage(UIVSTeam.m_instance.m_roomList, LocalizationManager.GetInstance().GetString("MSG_NET_LOGIN_SERVER_FAILED"), 2, 17);
			}
		}
		else if (GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 43 && !(UIBossRushTeam.m_instance == null))
		{
			UIMsgBox.instance.ShowSystemMessage(UIBossRushTeam.m_instance.m_roomList, LocalizationManager.GetInstance().GetString("MSG_NET_LOGIN_SERVER_FAILED"), 2, 17);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		if (loginSuccess)
		{
			Debug.Log(string.Concat("Robot ", robot, " login gameserver"));
			robot.GetTimeManager().LastSynTime = Time.time;
			robot.GetLobby().SetChannelID(channelID);
			robot.GetGameApp().GetGameMode().TypeOfNetwork = NetworkType.MultiPlayer_Internet;
			robot.GetGameApp().GetGameMode().PlayerStatus = PlayerStateNetwork.WaitingInRoom;
			UploadDataRequest request = new UploadDataRequest(robot);
			robot.GetNetworkManager().SendRequestAsRobot(request, robot);
			robot.Notify(RobotStateEvent.LoginSuccess);
			robot.GetRobotLogin().LoginSuccess();
		}
	}
}
