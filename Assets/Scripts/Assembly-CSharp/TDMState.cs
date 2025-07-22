public class TDMState : IBattleState
{
	public short kills;

	public short dead;

	public short assist;

	public short score;

	public short bonus;

	public void Init()
	{
		kills = 0;
		dead = 0;
		assist = 0;
		score = 0;
		bonus = 0;
	}

	public void WriteToBuffer(BytesBuffer buffer)
	{
		buffer.AddShort(kills);
		buffer.AddShort(dead);
		buffer.AddShort(assist);
		buffer.AddShort(score);
		buffer.AddShort(bonus);
	}

	public void ReadFromBuffer(BytesBuffer buffer)
	{
		kills = buffer.ReadShort();
		dead = buffer.ReadShort();
		assist = buffer.ReadShort();
		score = buffer.ReadShort();
		bonus = buffer.ReadShort();
	}

	public void SetState(IBattleState state)
	{
		TDMState tDMState = (TDMState)state;
		kills = tDMState.kills;
		dead = tDMState.dead;
		assist = tDMState.assist;
		score = tDMState.score;
		bonus = tDMState.bonus;
	}

	public void SetMaxKills(int kills)
	{
		this.kills = (short)kills;
	}

	public void AtomicKills()
	{
		kills++;
	}

	public void AtomicDead()
	{
		dead++;
	}

	public void AtomicAssist()
	{
		assist++;
	}
}
