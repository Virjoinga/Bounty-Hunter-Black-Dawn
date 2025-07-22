using UnityEngine;

public class UpdatePlayerCmpQuestsResponse : Response
{
	protected int m_playerChannleID;

	protected short[] m_quests;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerChannleID = bytesBuffer.ReadInt();
		int num = bytesBuffer.ReadByte();
		m_quests = new short[num];
		for (int i = 0; i < num; i++)
		{
			m_quests[i] = bytesBuffer.ReadShort();
			Debug.Log("UpdateCmpQuest: " + m_quests[i]);
		}
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerChannleID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		UserState userState = remotePlayerByUserID.GetUserState();
		if (userState == null)
		{
			return;
		}
		for (int i = 0; i < m_quests.Length; i++)
		{
			QuestCompleted questCompleted = userState.m_questStateContainer.CompletedQuestForRemotePlayer(m_quests[i]);
			if (questCompleted != null && UIQuest.m_instance != null && UIQuest.m_instance.m_teamQuest.active)
			{
				UITeamQuest component = UIQuest.m_instance.m_teamQuest.GetComponent<UITeamQuest>();
				component.ResetPlayerInfo();
			}
		}
	}
}
