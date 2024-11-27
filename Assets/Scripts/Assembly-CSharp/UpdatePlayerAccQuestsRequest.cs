using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerAccQuestsRequest : Request
{
	protected short[] m_quests;

	public UpdatePlayerAccQuestsRequest(short[] quests)
	{
		requestID = 152;
		m_quests = new short[quests.Length];
		for (int i = 0; i < quests.Length; i++)
		{
			m_quests[i] = quests[i];
			Debug.Log("UpdatePlayerAccQuestsRequest.Acc: " + i + ": " + m_quests[i]);
		}
	}

	public UpdatePlayerAccQuestsRequest(short questId)
	{
		requestID = 152;
		m_quests = new short[1];
		m_quests[0] = questId;
		Debug.Log("UpdatePlayerAccQuestsRequest.Acc: " + 0 + ": " + m_quests[0]);
	}

	public UpdatePlayerAccQuestsRequest(List<short> quests)
	{
		requestID = 152;
		m_quests = new short[quests.Count];
		for (int i = 0; i < quests.Count; i++)
		{
			m_quests[i] = quests[i];
			Debug.Log("UpdatePlayerAccQuestsRequest.Acc: " + i + ": " + m_quests[i]);
		}
	}

	public UpdatePlayerAccQuestsRequest(List<int> quests)
	{
		requestID = 152;
		m_quests = new short[quests.Count];
		for (int i = 0; i < quests.Count; i++)
		{
			m_quests[i] = (short)quests[i];
			Debug.Log("UpdatePlayerAccQuestsRequest.Acc: " + i + ": " + m_quests[i]);
		}
	}

	public override byte[] GetBody()
	{
		int num = m_quests.Length;
		BytesBuffer bytesBuffer = new BytesBuffer(5 + num * 2);
		bytesBuffer.AddInt(Lobby.GetInstance().GetChannelID());
		bytesBuffer.AddByte((byte)num);
		for (int i = 0; i < num; i++)
		{
			bytesBuffer.AddShort(m_quests[i]);
		}
		return bytesBuffer.GetBytes();
	}
}
