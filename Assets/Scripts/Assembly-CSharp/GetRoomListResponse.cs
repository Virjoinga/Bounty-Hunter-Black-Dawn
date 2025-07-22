using System.Collections.Generic;
using UnityEngine;

internal class GetRoomListResponse : Response
{
	private List<Room> roomList = new List<Room>();

	private byte recommendRoomNumber;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		recommendRoomNumber = bytesBuffer.ReadByte();
		for (int i = 0; i < b; i++)
		{
			short roomID = bytesBuffer.ReadShort();
			string text = bytesBuffer.ReadStringShortLength();
			byte mapID = bytesBuffer.ReadByte();
			byte gameMode = bytesBuffer.ReadByte();
			byte maxPlayerNum = bytesBuffer.ReadByte();
			byte joinedPlayer = bytesBuffer.ReadByte();
			byte b2 = bytesBuffer.ReadByte();
			short ping = bytesBuffer.ReadShort();
			short questMark = bytesBuffer.ReadShort();
			short minJoinLevel = bytesBuffer.ReadShort();
			short maxJoinLevel = bytesBuffer.ReadShort();
			string comment = bytesBuffer.ReadStringShortLength();
			short roomPassword = -1;
			if (b2 == 1)
			{
				roomPassword = bytesBuffer.ReadShort();
			}
			Room room = new Room(maxPlayerNum);
			room.setRoomID(roomID);
			room.setRoomName(text);
			room.setMapID(mapID);
			room.setGameMode(gameMode);
			room.setJoinedPlayer(joinedPlayer);
			room.setHasPassword(b2);
			room.setPing(ping);
			room.setMinJoinLevel(minJoinLevel);
			room.setMaxJoinLevel(maxJoinLevel);
			room.setRoomPassword(roomPassword);
			room.setQuestMark(questMark);
			room.setComment(comment);
			roomList.Add(room);
			Debug.Log("get room " + text);
		}
	}

	public override void ProcessLogic()
	{
		if (UITeam.m_instance != null && UITeam.m_instance.m_roomList.gameObject.active)
		{
			Debug.Log("update rooms:" + roomList.Count);
			UITeam.m_instance.m_roomList.BeginUpdateRooms(roomList, recommendRoomNumber);
		}
		else if (UIVSTeam.m_instance != null && UIVSTeam.m_instance.m_roomList.gameObject.active)
		{
			Debug.Log("update rooms:" + roomList.Count);
			UIVSTeam.m_instance.m_roomList.BeginUpdateRooms(roomList, recommendRoomNumber);
		}
		else if (UIBossRushTeam.m_instance != null && UIBossRushTeam.m_instance.m_roomList.gameObject.active)
		{
			Debug.Log("update rooms:" + roomList.Count);
			UIBossRushTeam.m_instance.m_roomList.BeginUpdateRooms(roomList, recommendRoomNumber);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
		foreach (Room room in roomList)
		{
			if (room != null)
			{
				if (room.getGameMode() != 4)
				{
					JoinRoomRequest request = new JoinRoomRequest(room.getRoomID(), robot.GetUserState().GetCharLevel(), robot.GetTimeManager().Ping);
					robot.GetNetworkManager().SendRequestAsRobot(request, robot);
					return;
				}
				if (Random.Range(0, 100) > 50)
				{
					JoinVSRoomRequest request2 = new JoinVSRoomRequest(room.getRoomID(), robot.GetTimeManager().Ping);
					robot.GetNetworkManager().SendRequestAsRobot(request2, robot);
					return;
				}
			}
		}
		robot.GetRobotRoom().JoinRoomFailed();
	}
}
