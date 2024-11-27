using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerAccQuestsResponse : Response
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
			Debug.Log("UpdateAccQuest: " + m_quests[i]);
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
		UserState userState2 = GameApp.GetInstance().GetUserState();
		if (userState == null)
		{
			return;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < m_quests.Length; i++)
		{
			QuestState questState = userState.m_questStateContainer.AcceptQuestForRemotePlayer(m_quests[i], 1);
			bool canBeAccepted = false;
			if (!userState2.m_questStateContainer.CheckHasBeenAccepted(m_quests[i]) && userState2.m_questStateContainer.CheckCanBeAccepted(m_quests[i]))
			{
				canBeAccepted = true;
				QuestState questState2 = userState2.m_questStateContainer.AcceptQuest(m_quests[i]);
				list.Add(questState2.m_id);
			}
			if (questState != null && UIQuest.m_instance != null && UIQuest.m_instance.m_teamQuest.gameObject.active)
			{
				UITeamQuest component = UIQuest.m_instance.m_teamQuest.GetComponent<UITeamQuest>();
				component.AddTeamQuest(questState.m_quest.m_commonId, questState, canBeAccepted);
			}
		}
		if (list.Count > 0)
		{
			UpdatePlayerAccQuestsRequest request = new UpdatePlayerAccQuestsRequest(list);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			GameApp.GetInstance().GetGameScene().UpdateNpcFlag();
		}
	}
}
