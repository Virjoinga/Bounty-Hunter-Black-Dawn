using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerCmpQuestsRequest : Request
{
	protected short[] m_quests;

	public UpdatePlayerCmpQuestsRequest(short[] quests)
	{
		requestID = 153;
		m_quests = new short[quests.Length];
		for (int i = 0; i < quests.Length; i++)
		{
			m_quests[i] = quests[i];
			Debug.Log("UpdatePlayerCmpQuestsRequest.Acc: " + i + ": " + m_quests[i]);
		}
	}

	public UpdatePlayerCmpQuestsRequest(List<short> quests)
	{
		requestID = 153;
		m_quests = new short[quests.Count];
		for (int i = 0; i < quests.Count; i++)
		{
			m_quests[i] = quests[i];
			Debug.Log("UpdatePlayerCmpQuestsRequest.Acc: " + i + ": " + m_quests[i]);
		}
	}

	public UpdatePlayerCmpQuestsRequest(short questId)
	{
		requestID = 153;
		m_quests = new short[1];
		m_quests[0] = questId;
		Debug.Log("UpdatePlayerCmpQuestsRequest.Acc: " + 0 + ": " + m_quests[0]);
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
