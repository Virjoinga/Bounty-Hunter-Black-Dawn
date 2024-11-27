using UnityEngine;

internal class JoinRoomResponse : Response
{
	protected short roomID;

	protected byte mapID;

	protected byte gameMode;

	protected byte winCondition;

	protected short winValue;

	protected byte autoBalance;

	protected byte seatID;

	protected short questMark;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		mapID = bytesBuffer.ReadByte();
		Debug.Log("roomID: " + roomID);
		if (roomID != -1)
		{
			gameMode = bytesBuffer.ReadByte();
			winCondition = bytesBuffer.ReadByte();
			winValue = bytesBuffer.ReadShort();
			autoBalance = bytesBuffer.ReadByte();
			seatID = bytesBuffer.ReadByte();
			Debug.Log("seatID: " + seatID);
			questMark = bytesBuffer.ReadShort();
		}
	}

	public override void ProcessLogic()
	{
		if (roomID != -1)
		{
			Debug.Log("join room..");
			Lobby.GetInstance().IsMasterPlayer = false;
			Lobby.GetInstance().SetCurrentRoomID(roomID);
			Lobby.GetInstance().SetCurrentRoomMapID(mapID);
			GameApp.GetInstance().GetGameMode().ModePlay = (Mode)gameMode;
			Lobby.GetInstance().WinCondition = winCondition;
			Lobby.GetInstance().WinValue = winValue;
			Lobby.GetInstance().AutoBalance = autoBalance;
			Lobby.GetInstance().CurrentSeatID = seatID;
			Lobby.GetInstance().SetCurrentMarkQuest(questMark);
			Lobby.GetInstance().GetVSClock().SetTotalGameSeconds(winValue * 60);
			PlayerJoinTeamStartGameRequest request = new PlayerJoinTeamStartGameRequest();
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			if (UITeam.m_instance != null && UITeam.m_instance.m_roomList.gameObject.active)
			{
				UITeam.m_instance.m_roomList.JoinRoomSuccess();
			}
		}
		else
		{
			Lobby.GetInstance().SetCurrentRoomID(-1);
			if (UITeam.m_instance != null && UITeam.m_instance.m_roomList.gameObject.active)
			{
				UITeam.m_instance.m_roomList.JoinRoomFailed();
			}
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		if (roomID != -1)
		{
			robot.Notify(RobotStateEvent.JoinRoomSuccess);
			PlayerJoinTeamStartGameRequest request = new PlayerJoinTeamStartGameRequest();
			robot.GetNetworkManager().SendRequestAsRobot(request, robot);
		}
		else
		{
			robot.GetRobotRoom().JoinRoomFailed();
		}
	}
}
