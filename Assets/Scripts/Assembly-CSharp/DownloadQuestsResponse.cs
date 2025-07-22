using System.Collections.Generic;
using UnityEngine;

public class DownloadQuestsResponse : Response
{
	protected int m_playerChannleID;

	protected short[] m_quests;

	protected byte[] m_questsSubState;

	protected short[] m_compQuests;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerChannleID = bytesBuffer.ReadInt();
		int num = bytesBuffer.ReadShort();
		m_quests = new short[num];
		m_questsSubState = new byte[num];
		for (int i = 0; i < num; i++)
		{
			m_quests[i] = bytesBuffer.ReadShort();
			m_questsSubState[i] = bytesBuffer.ReadByte();
			Debug.Log("DownloadAccQuests: " + m_quests[i] + "- " + m_questsSubState[i]);
		}
		num = bytesBuffer.ReadShort();
		m_compQuests = new short[num];
		for (int j = 0; j < num; j++)
		{
			m_compQuests[j] = bytesBuffer.ReadShort();
			Debug.Log("DownloadCompQuests: " + m_compQuests[j]);
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
		userState.m_questStateContainer.m_completedQuestLst.Clear();
		for (int i = 0; i < m_compQuests.Length; i++)
		{
			userState.m_questStateContainer.CompletedQuestForRemotePlayer(m_compQuests[i]);
		}
		userState.m_questStateContainer.m_accStateLst.Clear();
		List<int> list = new List<int>();
		for (int j = 0; j < m_quests.Length; j++)
		{
			QuestState questState = userState.m_questStateContainer.AcceptQuestForRemotePlayer(m_quests[j], m_questsSubState[j]);
			UserState userState2 = GameApp.GetInstance().GetUserState();
			bool canBeAccepted = false;
			if (!userState2.m_questStateContainer.CheckHasBeenAccepted(m_quests[j]) && userState2.m_questStateContainer.CheckCanBeAccepted(m_quests[j]))
			{
				canBeAccepted = true;
				QuestState questState2 = userState2.m_questStateContainer.AcceptQuest(m_quests[j]);
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
		}
		if (UIQuest.m_instance != null && UIQuest.m_instance.m_teamQuest.gameObject.active)
		{
			UITeamQuest component2 = UIQuest.m_instance.m_teamQuest.GetComponent<UITeamQuest>();
			component2.UpdateCurrentQuest();
		}
	}
}
