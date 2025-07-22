internal class GetVSTDMRankResponse : Response
{
	private VSTDMRank m_rank = new VSTDMRank();

	public override void ReadData(byte[] data)
	{
		BytesBuffer buffer = new BytesBuffer(data);
		m_rank.ReadFromBuffer(buffer);
	}

	public override void ProcessLogic()
	{
		if (GameApp.GetInstance().GetUserState() != null)
		{
			GameApp.GetInstance().GetUserState().m_statistics[0] = m_rank;
			GameApp.GetInstance().GetUserState().SetVSTDMStatsId(m_rank.Id);
		}
		if (UIVS.instance != null)
		{
			UIVS.instance.NotifyGetRankSuccess();
		}
	}
}
