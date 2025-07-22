using UnityEngine;

internal class CreateRoomResponse : Response
{
	public short roomID;

	public byte gameMode;

	public string roomName;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		gameMode = bytesBuffer.ReadByte();
		roomName = bytesBuffer.ReadStringShortLength();
	}

	public override void ProcessLogic()
	{
		Debug.Log("roomID: " + roomID);
		if (roomID == -1)
		{
			if (UITeam.m_instance != null)
			{
				UICreateRoom component = UITeam.m_instance.m_createRoom.GetComponent<UICreateRoom>();
				if (component != null && UITeam.m_instance.m_createRoom.activeSelf)
				{
					UIMsgBox.instance.ShowMessage(component, LocalizationManager.GetInstance().GetString("MSG_NET_CREATE_ROOM_FAILED"), 2, 19);
				}
			}
		}
		else
		{
			Lobby.GetInstance().SetUserName(roomName);
			Lobby.GetInstance().SetCurrentRoomID(roomID);
			Lobby.GetInstance().IsMasterPlayer = true;
			UICreateRoom component2 = UITeam.m_instance.m_createRoom.GetComponent<UICreateRoom>();
			component2.ChangeRoomName(roomName);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		if (roomID == -1)
		{
			robot.GetRobotRoom().CreateRoomFailed();
			return;
		}
		robot.GetLobby().SetCurrentRoomID(roomID);
		robot.GetLobby().IsMasterPlayer = true;
		robot.Notify(RobotStateEvent.CreateRoomSuccess);
		StartGameRequest request = new StartGameRequest();
		robot.GetNetworkManager().SendRequestAsRobot(request, robot);
	}
}
