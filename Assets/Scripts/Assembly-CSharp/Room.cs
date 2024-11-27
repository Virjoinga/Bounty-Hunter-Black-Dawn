public class Room
{
	protected short roomID;

	protected RoomPlayer[] players;

	protected byte maxPlayer;

	protected byte gameMode;

	protected byte joinedPlayer;

	protected string roomName;

	protected short passWord;

	protected byte hasPassword;

	protected byte mapID;

	protected short questMark;

	protected short ping = 300;

	protected short minJoinLevel;

	protected short maxJoinLevel;

	protected string comment;

	public Room(byte maxPlayerNum)
	{
		maxPlayer = maxPlayerNum;
		players = new RoomPlayer[maxPlayer];
	}

	public string getRoomName()
	{
		return roomName;
	}

	public void setRoomName(string roomName)
	{
		this.roomName = roomName;
	}

	public byte getGameMode()
	{
		return gameMode;
	}

	public void setGameMode(byte gameMode)
	{
		this.gameMode = gameMode;
	}

	public short getRoomPassword()
	{
		return passWord;
	}

	public void setRoomPassword(short password)
	{
		passWord = password;
	}

	public RoomPlayer[] GetAllPlayers()
	{
		return players;
	}

	public void setAllPlayer(RoomPlayer[] players)
	{
		this.players = players;
	}

	public short getRoomID()
	{
		return roomID;
	}

	public void setRoomID(short roomID)
	{
		this.roomID = roomID;
	}

	public byte getMapID()
	{
		return mapID;
	}

	public void setMapID(byte mapID)
	{
		this.mapID = mapID;
	}

	public byte getMaxPlayer()
	{
		return maxPlayer;
	}

	public byte getPlayerNum()
	{
		byte b = 0;
		for (int i = 0; i < players.Length; i++)
		{
			if (players[i] != null)
			{
				b++;
			}
		}
		return b;
	}

	public void setMaxPlayer(byte maxPlayer)
	{
		this.maxPlayer = maxPlayer;
	}

	public bool isHasPassword()
	{
		return hasPassword == 1;
	}

	public byte getJoinedPlayer()
	{
		return joinedPlayer;
	}

	public void setJoinedPlayer(byte joinedPlayer)
	{
		this.joinedPlayer = joinedPlayer;
	}

	public void setHasPassword(byte hasPassword)
	{
		this.hasPassword = hasPassword;
	}

	public short getPing()
	{
		return ping;
	}

	public void setPing(short ping)
	{
		this.ping = ping;
	}

	public short getMinJoinLevel()
	{
		return minJoinLevel;
	}

	public void setMinJoinLevel(short min)
	{
		minJoinLevel = min;
	}

	public short getMaxJoinLevel()
	{
		return maxJoinLevel;
	}

	public void setMaxJoinLevel(short max)
	{
		maxJoinLevel = max;
	}

	public short getQuestMark()
	{
		return questMark;
	}

	public void setQuestMark(short _questMark)
	{
		questMark = _questMark;
	}

	public string getComment()
	{
		return comment;
	}

	public void setComment(string txt)
	{
		comment = txt;
	}
}
