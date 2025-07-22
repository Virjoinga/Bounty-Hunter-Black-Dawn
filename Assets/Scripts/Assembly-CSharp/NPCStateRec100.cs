using System.Collections.Generic;
using System.IO;

public class NPCStateRec100 : ISubRecordset
{
	private NPCState m_npcState;

	public NPCStateRec100(NPCState state)
	{
		m_npcState = state;
	}

	public void SaveData(BinaryWriter bw)
	{
		Dictionary<string, NPCQuestState> stateList = m_npcState.GetStateList();
		bw.Write(stateList.Count);
		foreach (string key in stateList.Keys)
		{
			NPCQuestState nPCQuestState = stateList[key];
			bw.Write(nPCQuestState.m_id);
			bw.Write(nPCQuestState.m_name);
			bw.Write(nPCQuestState.m_state);
		}
	}

	public void LoadData(BinaryReader br)
	{
		m_npcState.ClearStateList();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			NPCQuestState nPCQuestState = new NPCQuestState();
			nPCQuestState.m_id = br.ReadInt16();
			nPCQuestState.m_name = br.ReadString();
			nPCQuestState.m_state = br.ReadString();
			m_npcState.AddState(nPCQuestState);
		}
	}
}
