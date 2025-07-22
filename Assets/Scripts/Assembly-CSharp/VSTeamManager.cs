public class VSTeamManager
{
	protected TeamName mTeamName;

	protected short mScore;

	public VSTeamManager(TeamName team)
	{
		mTeamName = team;
	}

	public virtual TeamName GetTeamName()
	{
		return mTeamName;
	}

	public void SetScore(short score)
	{
		mScore = score;
	}

	public short GetScore()
	{
		return mScore;
	}
}
