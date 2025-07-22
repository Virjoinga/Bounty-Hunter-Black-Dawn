using System;
using System.Collections.Generic;
using System.IO;

public class QuestInfoRec120 : ISubRecordset
{
	private QuestInfo m_info;

	public QuestInfoRec120(QuestInfo info)
	{
		m_info = info;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(m_info.m_commonQuestLst.Count);
		foreach (KeyValuePair<short, QuestInfo.QuestLog> item in m_info.m_commonQuestLst)
		{
			bw.Write(item.Value.m_id);
			bw.Write(Convert.ToString(item.Value.m_accQuestDate));
			bw.Write(Convert.ToString(item.Value.m_comQuestDate));
			bw.Write(item.Value.m_realSpendTime);
			bw.Write(item.Value.m_deadCount);
			bw.Write(item.Value.m_upload);
		}
	}

	public void LoadData(BinaryReader br)
	{
		m_info.m_commonQuestLst.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			QuestInfo.QuestLog questLog = new QuestInfo.QuestLog();
			questLog.m_id = br.ReadInt16();
			questLog.m_accQuestDate = Convert.ToDateTime(br.ReadString());
			questLog.m_comQuestDate = Convert.ToDateTime(br.ReadString());
			questLog.m_realSpendTime = br.ReadInt32();
			questLog.m_deadCount = br.ReadInt16();
			questLog.m_upload = br.ReadBoolean();
			m_info.m_commonQuestLst.Add(questLog.m_id, questLog);
		}
	}
}
