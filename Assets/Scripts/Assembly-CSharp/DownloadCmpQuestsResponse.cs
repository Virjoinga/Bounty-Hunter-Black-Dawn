using UnityEngine;

public class DownloadCmpQuestsResponse : Response
{
	protected int m_playerChannleID;

	protected short[] m_compQuests;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerChannleID = bytesBuffer.ReadInt();
		int num = bytesBuffer.ReadShort();
		m_compQuests = new short[num];
		for (int i = 0; i < num; i++)
		{
			m_compQuests[i] = bytesBuffer.ReadShort();
			Debug.Log("Downloadcomp: " + m_compQuests[i]);
		}
	}

	public override void ProcessLogic()
	{
		Debug.Log("DownloadCmp:" + m_compQuests.Length);
		if (m_compQuests.Length <= 0)
		{
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerChannleID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		UserState userState = remotePlayerByUserID.GetUserState();
		if (userState != null)
		{
			userState.m_questStateContainer.m_completedQuestLst.Clear();
			for (int i = 0; i < m_compQuests.Length; i++)
			{
				userState.m_questStateContainer.CompletedQuestForRemotePlayer(m_compQuests[i]);
			}
		}
	}
}
