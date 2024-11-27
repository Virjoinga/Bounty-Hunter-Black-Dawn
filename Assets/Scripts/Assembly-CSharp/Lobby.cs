using System.Collections.Generic;

public class Lobby
{
	public class InvitationInfo
	{
		public SubMode mode;
	}

	protected List<Room> roomList = new List<Room>();

	protected static Lobby instance;

	protected short currentRoomID = -1;

	protected string userName;

	protected int channelID;

	protected Room currentRoom;

	protected byte currentRoomMapID;

	public short currentQuestMark;

	protected VSClock vsClock = new VSClock();

	protected InvitationInfo invInfo = new InvitationInfo();

	public bool IsMasterPlayer { get; set; }

	public byte CurrentRoomMaxPlayer { get; set; }

	public bool IsGuest { get; set; }

	public byte WinCondition { get; set; }

	public short WinValue { get; set; }

	public byte AutoBalance { get; set; }

	public byte CurrentSeatID { get; set; }

	public bool IsPostingScoreToSocialNetwork { get; set; }

	public int MasterPlayerID { get; set; }

	public InvitationInfo GetInvitationInfo()
	{
		return invInfo;
	}

	public VSClock GetVSClock()
	{
		return vsClock;
	}

	public Room GetCurrentRoom()
	{
		return currentRoom;
	}

	public void SetCurrentRoom(Room room)
	{
		currentRoom = room;
	}

	public byte GetCurrentRoomMapID()
	{
		return currentRoomMapID;
	}

	public void SetCurrentRoomMapID(byte mapID)
	{
		currentRoomMapID = mapID;
	}

	public static Lobby GetInstance()
	{
		if (instance == null)
		{
			instance = new Lobby();
			instance.IsMasterPlayer = false;
		}
		return instance;
	}

	public void SetChannelID(int channelID)
	{
		this.channelID = channelID;
	}

	public int GetChannelID()
	{
		return channelID;
	}

	public void SetCurrentRoomID(short roomID)
	{
		currentRoomID = roomID;
	}

	public short GetCurrentRoomID()
	{
		return currentRoomID;
	}

	public bool IsInRoomState()
	{
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && currentRoomID != -1)
		{
			return true;
		}
		return false;
	}

	public string GetUserName()
	{
		return userName;
	}

	public void SetUserName(string name)
	{
		userName = name;
	}

	public Room GetRoom(short roomID)
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			if (roomList[i].getRoomID() == roomID)
			{
				return roomList[i];
			}
		}
		return null;
	}

	public List<Room> GetRoomList()
	{
		return roomList;
	}

	public void SetupRoomList(List<Room> list)
	{
		roomList = list;
	}

	public short GetCurrentMarkQuest()
	{
		return currentQuestMark;
	}

	public void SetCurrentMarkQuest(short questCommonId)
	{
		currentQuestMark = questCommonId;
	}
}
