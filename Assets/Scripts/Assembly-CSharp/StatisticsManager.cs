using System.Collections.Generic;

public class StatisticsManager
{
	private static StatisticsManager instance = null;

	public static List<VSTDMRank> m_vsTDMRank = new List<VSTDMRank>();

	public static int m_vsTDMRankCount;

	public static StatisticsManager GetInstance()
	{
		if (instance == null)
		{
			instance = new StatisticsManager();
		}
		return instance;
	}
}
