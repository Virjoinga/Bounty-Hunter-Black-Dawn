using System.Collections.Generic;
using UnityEngine;

public class DownloadAccQuestsResponse : Response
{
	protected int m_playerChannleID;

	protected short[] m_quests;

	protected byte[] m_questsSubState;

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
			Debug.Log("DownloadAcc: " + m_quests[i]);
		}
	}

	public override void ProcessLogic()
	{
		if (m_quests.Length <= 0)
		{
			return;
		}
		Debug.Log("DownloadAcc:" + m_quests.Length);
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
		userState.m_questStateContainer.m_accStateLst.Clear();
		List<int> list = new List<int>();
		for (int i = 0; i < m_quests.Length; i++)
		{
			Debug.Log("DownloadAccQuestsResponse: " + m_quests[i] + "," + m_questsSubState[i]);
			userState.m_questStateContainer.AcceptQuestForRemotePlayer(m_quests[i], m_questsSubState[i]);
			UserState userState2 = GameApp.GetInstance().GetUserState();
			if (!userState2.m_questStateContainer.CheckHasBeenAccepted(m_quests[i]) && userState2.m_questStateContainer.CheckCanBeAccepted(m_quests[i]))
			{
				QuestState questState = userState2.m_questStateContainer.AcceptQuest(m_quests[i]);
				list.Add(questState.m_id);
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
