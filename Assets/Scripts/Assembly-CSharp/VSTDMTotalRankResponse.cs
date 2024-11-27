using System.Collections.Generic;
using UnityEngine;

internal class VSTDMTotalRankResponse : Response
{
	private List<VSTDMRank> m_rank = new List<VSTDMRank>();

	private int m_count;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_count = bytesBuffer.ReadInt();
		int num = bytesBuffer.ReadByte();
		for (int i = 0; i < num; i++)
		{
			VSTDMRank vSTDMRank = new VSTDMRank();
			vSTDMRank.ReadFromBuffer(bytesBuffer);
			m_rank.Add(vSTDMRank);
		}
	}

	public override void ProcessLogic()
	{
		StatisticsManager.m_vsTDMRankCount = m_count;
		StatisticsManager.m_vsTDMRank.Clear();
		foreach (VSTDMRank item in m_rank)
		{
			StatisticsManager.m_vsTDMRank.Add(item);
		}
		Debug.Log("ProcessLogic : " + StatisticsManager.m_vsTDMRank.Count);
	}
}
