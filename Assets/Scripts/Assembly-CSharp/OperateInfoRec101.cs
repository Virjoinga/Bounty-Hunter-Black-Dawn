using System.IO;

public class OperateInfoRec101 : ISubRecordset
{
	private OperatingInfo m_OperInfo;

	public OperateInfoRec101(OperatingInfo operInfo)
	{
		m_OperInfo = operInfo;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(m_OperInfo.mIndex);
		int count = m_OperInfo.mInfo.Count;
		bw.Write(count);
		int cash = GameApp.GetInstance().GetUserState().GetCash();
		int charLevel = GameApp.GetInstance().GetUserState().GetCharLevel();
		m_OperInfo.mInfo[0] = cash;
		m_OperInfo.mInfo[22] = charLevel;
		for (int i = 0; i < count; i++)
		{
			bw.Write(m_OperInfo.mInfo[i]);
		}
	}

	public void LoadData(BinaryReader br)
	{
		m_OperInfo.mIndex = br.ReadInt32();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			m_OperInfo.mInfo[i] = br.ReadInt32();
		}
	}
}
