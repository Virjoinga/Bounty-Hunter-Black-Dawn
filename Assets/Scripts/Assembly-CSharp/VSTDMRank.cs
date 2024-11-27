public class VSTDMRank : IStatistics
{
	private int m_id;

	private string m_name;

	private byte m_charClass;

	private int m_win;

	private int m_lose;

	private int m_offline;

	private int m_score;

	private int m_bonus;

	public int Id
	{
		get
		{
			return m_id;
		}
	}

	public CharacterClass CharacterClass
	{
		get
		{
			return (CharacterClass)m_charClass;
		}
	}

	public string Name
	{
		get
		{
			return m_name;
		}
	}

	public int Win
	{
		get
		{
			return m_win;
		}
	}

	public int Lost
	{
		get
		{
			return m_lose;
		}
	}

	public int Offline
	{
		get
		{
			return m_offline;
		}
	}

	public int Score
	{
		get
		{
			return m_score;
		}
	}

	public int Bonus
	{
		get
		{
			return m_bonus;
		}
		set
		{
			m_bonus = value;
		}
	}

	public void Init()
	{
		m_id = 0;
		m_name = string.Empty;
		m_charClass = 0;
		m_win = 0;
		m_lose = 0;
		m_offline = 0;
		m_score = 0;
		m_bonus = 0;
	}

	public void WriteToBuffer(BytesBuffer buffer)
	{
		buffer.AddInt(m_id);
		buffer.AddStringShortLength(m_name);
		buffer.AddByte(m_charClass);
		buffer.AddInt(m_win);
		buffer.AddInt(m_lose);
		buffer.AddInt(m_offline);
		buffer.AddInt(m_score);
		buffer.AddInt(m_bonus);
	}

	public void ReadFromBuffer(BytesBuffer buffer)
	{
		m_id = buffer.ReadInt();
		m_name = buffer.ReadStringShortLength();
		m_charClass = buffer.ReadByte();
		m_win = buffer.ReadInt();
		m_lose = buffer.ReadInt();
		m_offline = buffer.ReadInt();
		m_score = buffer.ReadInt();
		m_bonus = buffer.ReadInt();
	}
}
