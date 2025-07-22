using System;
using System.Collections.Generic;
using System.IO;

public class QuestStateRec100 : ISubRecordset
{
	private QuestStateContainer m_questState;

	public QuestStateRec100(QuestStateContainer state)
	{
		m_questState = state;
	}

	public void SaveData(BinaryWriter bw)
	{
		List<QuestState> accStateLst = m_questState.m_accStateLst;
		List<QuestCompleted> completedQuestLst = m_questState.m_completedQuestLst;
		bw.Write(accStateLst.Count);
		for (int i = 0; i < accStateLst.Count; i++)
		{
			QuestState questState = accStateLst[i];
			bw.Write(questState.m_id);
			bw.Write((int)questState.m_status);
			bw.Write(questState.m_animFlag);
			bw.Write(questState.m_conter.Count);
			foreach (int key in questState.m_conter.Keys)
			{
				bw.Write(questState.m_conter[key].m_id);
				bw.Write((byte)questState.m_conter[key].m_type);
				bw.Write(questState.m_conter[key].m_name);
				bw.Write(questState.m_conter[key].m_currNum);
				bw.Write(questState.m_conter[key].m_maxNum);
			}
		}
		bw.Write(completedQuestLst.Count);
		for (int j = 0; j < completedQuestLst.Count; j++)
		{
			QuestCompleted questCompleted = completedQuestLst[j];
			bw.Write(questCompleted.m_id);
			bw.Write(questCompleted.m_groupId);
			string value = questCompleted.m_dateTime.ToShortDateString();
			bw.Write(value);
		}
	}

	public void LoadData(BinaryReader br)
	{
		m_questState.m_accStateLst.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			QuestState questState = new QuestState();
			questState.m_id = br.ReadInt32();
			questState.m_status = (QuestPhase)br.ReadInt32();
			questState.m_animFlag = br.ReadBoolean();
			questState.m_quest = QuestManager.GetInstance().m_questLst[questState.m_id];
			int num2 = br.ReadInt32();
			questState.m_conter = new Dictionary<int, QuestState.Conter>();
			for (int j = 0; j < num2; j++)
			{
				QuestState.Conter conter = new QuestState.Conter();
				conter.m_id = br.ReadInt32();
				conter.m_type = (QuestConterType)br.ReadByte();
				conter.m_name = br.ReadString();
				conter.m_currNum = br.ReadInt32();
				conter.m_maxNum = br.ReadInt32();
				questState.m_conter.Add(conter.m_id, conter);
			}
			m_questState.m_accStateLst.Add(questState);
		}
		m_questState.m_completedQuestLst.Clear();
		num = br.ReadInt32();
		for (int k = 0; k < num; k++)
		{
			QuestCompleted questCompleted = new QuestCompleted();
			questCompleted.m_id = br.ReadInt32();
			questCompleted.m_groupId = br.ReadInt32();
			string s = br.ReadString();
			questCompleted.m_quest = QuestManager.GetInstance().m_questLst[questCompleted.m_id];
			questCompleted.m_dateTime = DateTime.Parse(s);
			m_questState.m_completedQuestLst.Add(questCompleted);
		}
	}
}
