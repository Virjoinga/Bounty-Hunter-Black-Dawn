internal class GetRoomDataResponse : Response
{
	private Room room;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		short roomID = bytesBuffer.ReadShort();
		byte b = bytesBuffer.ReadByte();
		room = new Room(b);
		RoomPlayer[] allPlayers = room.GetAllPlayers();
		byte b2 = 0;
		for (int i = 0; i < b; i++)
		{
			string text = bytesBuffer.ReadString();
			if (text == null)
			{
				allPlayers[i] = null;
				bytesBuffer.ReadShort();
				bytesBuffer.ReadByte();
				bytesBuffer.ReadByte();
				bytesBuffer.ReadInt();
				continue;
			}
			short level = bytesBuffer.ReadShort();
			byte role = bytesBuffer.ReadByte();
			byte seatID = bytesBuffer.ReadByte();
			int channelID = bytesBuffer.ReadInt();
			allPlayers[i] = new RoomPlayer();
			allPlayers[i].setPlayerName(text);
			allPlayers[i].Level = level;
			allPlayers[i].Role = role;
			allPlayers[i].SeatID = seatID;
			allPlayers[i].ChannelID = channelID;
			b2++;
		}
		room.setRoomID(roomID);
		room.setJoinedPlayer(b2);
	}

	public override void ProcessLogic()
	{
		if (UITeam.m_instance != null && UITeam.m_instance.m_roomList.gameObject.active)
		{
			UITeam.m_instance.m_roomList.UpdateRoomData(room);
		}
		else if (UIVSTeam.m_instance != null && UIVSTeam.m_instance.m_roomList.gameObject.active)
		{
			UIVSTeam.m_instance.m_roomList.UpdateRoomData(room);
		}
		else if (UIBossRushTeam.m_instance != null && UIBossRushTeam.m_instance.m_roomList.gameObject.active)
		{
			UIBossRushTeam.m_instance.m_roomList.UpdateRoomData(room);
		}
	}

	public override void ProcessRobotLogic(RobotUser robot)
	{
	}
}
