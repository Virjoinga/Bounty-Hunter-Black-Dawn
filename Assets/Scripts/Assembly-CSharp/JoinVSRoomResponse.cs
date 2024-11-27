using UnityEngine;

public class JoinVSRoomResponse : Response
{
	protected short roomId;

	protected byte seatId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomId = bytesBuffer.ReadShort();
		if (roomId != -1)
		{
			seatId = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		UILoadingNet.m_instance.Hide();
		if (roomId != -1)
		{
			Lobby.GetInstance().SetCurrentRoomID(roomId);
			Lobby.GetInstance().IsMasterPlayer = false;
			GameApp.GetInstance().GetGameMode().ModePlay = Mode.VS_TDM;
			Lobby.GetInstance().CurrentSeatID = seatId;
		}
		else
		{
			Lobby.GetInstance().SetCurrentRoomID(-1);
			if (UIVSTeam.m_instance != null && UIVSTeam.m_instance.m_roomList.gameObject.active)
			{
				UIVSTeam.m_instance.m_roomList.JoinRoomFailed();
			}
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		if (roomId != -1)
		{
			robot.GetLobby().SetCurrentRoomID(roomId);
			robot.Notify(RobotStateEvent.StartVSGame);
		}
		else
		{
			robot.GetRobotRoom().JoinRoomFailed();
		}
		Debug.Log("robot start VS TDM game ..................");
	}
}
