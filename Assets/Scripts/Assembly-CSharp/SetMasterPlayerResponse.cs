using System.Collections.Generic;
using UnityEngine;

internal class SetMasterPlayerResponse : Response
{
	public int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		Debug.Log("set master player:" + playerID);
	}

	public override void ProcessLogic()
	{
		Lobby.GetInstance().MasterPlayerID = playerID;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		if (playerID == Lobby.GetInstance().GetChannelID())
		{
			Lobby.GetInstance().IsMasterPlayer = true;
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.IsRoomMaster = true;
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null)
				{
					item.IsRoomMaster = false;
				}
			}
			Lobby.GetInstance().SetCurrentMarkQuest(GameApp.GetInstance().GetUserState().GetCurrentQuest());
			GameApp.GetInstance().GetUserState().m_questStateContainer.UploadMarkQuest();
			return;
		}
		Lobby.GetInstance().IsMasterPlayer = false;
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.IsRoomMaster = false;
		foreach (RemotePlayer item2 in remotePlayers)
		{
			if (item2.GetUserID() == playerID)
			{
				item2.IsRoomMaster = true;
			}
			else
			{
				item2.IsRoomMaster = false;
			}
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		robot.GetLobby().MasterPlayerID = playerID;
		if (playerID == robot.GetLobby().GetChannelID())
		{
			robot.GetLobby().IsMasterPlayer = true;
		}
		else
		{
			robot.GetLobby().IsMasterPlayer = false;
		}
	}
}
