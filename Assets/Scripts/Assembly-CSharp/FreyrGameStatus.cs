public class FreyrGameStatus
{
	public enum FreyrGameReward
	{
		Inactive = 0,
		Available = 1,
		Active = 2
	}

	public FreyrGame freyrGame { get; set; }

	public byte videoTimes { get; set; }

	public int lastYearToWatchVideo { get; set; }

	public int lastDayToWatchVideo { get; set; }

	public FreyrGameReward clickIconOfReward { get; set; }
}
