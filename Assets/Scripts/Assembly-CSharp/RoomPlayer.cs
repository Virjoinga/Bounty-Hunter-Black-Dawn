using UnityEngine;

public class RoomPlayer
{
	protected string playerName;

	protected short level;

	protected byte role;

	protected int seatID;

	protected int channelID;

	public short Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public byte Role
	{
		get
		{
			return role;
		}
		set
		{
			role = value;
		}
	}

	public int SeatID
	{
		get
		{
			return seatID;
		}
		set
		{
			seatID = value;
		}
	}

	public int ChannelID
	{
		get
		{
			return channelID;
		}
		set
		{
			channelID = value;
		}
	}

	public bool IsLocalPlayer
	{
		get
		{
			Debug.Log("Lobby.GetInstance().GetChannelID() : " + Lobby.GetInstance().GetChannelID());
			Debug.Log("channelID : " + channelID);
			return Lobby.GetInstance().GetChannelID() == channelID;
		}
	}

	public string getPlayerName()
	{
		return playerName;
	}

	public void setPlayerName(string playerName)
	{
		this.playerName = playerName;
	}
}
