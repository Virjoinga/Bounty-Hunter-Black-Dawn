using System.Collections.Generic;

public abstract class NpcState
{
	public abstract void NextState(Npc npc, float deltaTime);
}
public class NPCState
{
	private Dictionary<string, NPCQuestState> m_stateLst = new Dictionary<string, NPCQuestState>();

	public void Init()
	{
		m_stateLst.Clear();
		foreach (KeyValuePair<string, NpcConfig> item in GameConfig.GetInstance().npcConfig)
		{
			NPCQuestState nPCQuestState = new NPCQuestState();
			nPCQuestState.m_id = item.Value.m_id;
			nPCQuestState.m_name = item.Value.m_callName;
			nPCQuestState.m_state = item.Value.m_state;
			m_stateLst.Add(item.Key, nPCQuestState);
		}
	}

	public Dictionary<string, NPCQuestState> GetStateList()
	{
		return m_stateLst;
	}

	public void AddState(NPCQuestState npc)
	{
		m_stateLst.Add(npc.m_name, npc);
	}

	public void ClearStateList()
	{
		m_stateLst.Clear();
	}

	public string GetState(string key)
	{
		return m_stateLst[key].m_state;
	}

	public void SetState(string key, string state)
	{
		m_stateLst[key].m_state = state;
	}

	public string GetDialog(string key)
	{
		return GameConfig.GetInstance().npcConfig[key].m_dialog;
	}

	public NpcType GetType(string key)
	{
		return GameConfig.GetInstance().npcConfig[key].m_type;
	}

	public short GetId(string key)
	{
		return GameConfig.GetInstance().npcConfig[key].m_id;
	}

	public string GetDisplayName(string key)
	{
		return GameConfig.GetInstance().npcConfig[key].m_name;
	}
}
