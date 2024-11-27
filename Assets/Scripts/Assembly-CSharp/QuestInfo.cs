using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestInfo : IInfoHandle
{
	public class QuestLog
	{
		public short m_id;

		public DateTime m_accQuestDate = new DateTime(1970, 1, 1, 8, 0, 0);

		public DateTime m_comQuestDate = new DateTime(1970, 1, 1, 8, 0, 0);

		public int m_realSpendTime;

		public short m_deadCount;

		public bool m_upload;
	}

	public const int UPLOAD_QUEST_TIME = 240;

	public Dictionary<short, QuestLog> m_commonQuestLst = new Dictionary<short, QuestLog>();

	public Timer m_updatePeriod = new Timer();

	public QuestInfo()
	{
		m_updatePeriod.SetTimer(1f, false);
	}

	public void Init()
	{
		m_commonQuestLst.Clear();
	}

	public void AccQuest(short id)
	{
		if (!m_commonQuestLst.ContainsKey(id))
		{
			QuestLog questLog = new QuestLog();
			questLog.m_id = id;
			questLog.m_realSpendTime = 0;
			questLog.m_deadCount = 0;
			questLog.m_upload = false;
			questLog.m_accQuestDate = DateTime.Now;
			m_commonQuestLst.Add(id, questLog);
		}
	}

	public void SubmitQuest(short id)
	{
		if (m_commonQuestLst.ContainsKey(id))
		{
			m_commonQuestLst[id].m_comQuestDate = DateTime.Now;
		}
	}

	public void AbandonQuest(short id)
	{
		if (m_commonQuestLst.ContainsKey(id))
		{
			m_commonQuestLst.Remove(id);
		}
	}

	public void UpdateQuestTime(short id)
	{
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameWorld().IsInstanceScene() && m_updatePeriod.Ready() && m_commonQuestLst.ContainsKey(id))
		{
			m_updatePeriod.Do();
			m_commonQuestLst[id].m_realSpendTime++;
		}
	}

	public void UpdateDeadTime(short id)
	{
		if (!GameApp.GetInstance().GetGameMode().IsVSMode() && GameApp.GetInstance().GetGameWorld().IsInstanceScene() && m_commonQuestLst.ContainsKey(id))
		{
			m_commonQuestLst[id].m_deadCount++;
		}
	}

	public long GetTotalMilliseconds(DateTime date)
	{
		TimeSpan timeSpan = new TimeSpan(date.Ticks - new DateTime(1970, 1, 1, 8, 0, 0).Ticks);
		return (long)timeSpan.TotalMilliseconds;
	}

	public void AfterUpload()
	{
		foreach (KeyValuePair<short, QuestLog> item in m_commonQuestLst)
		{
			if (item.Value.m_realSpendTime > 240 && !item.Value.m_upload && item.Value.m_comQuestDate.Year != 1970)
			{
				item.Value.m_upload = true;
				Debug.Log("QuestInfo AfterUpload: " + item.Value.m_id);
			}
		}
	}

	public byte[] WriteToBuffer()
	{
		int num = 0;
		foreach (KeyValuePair<short, QuestLog> item in m_commonQuestLst)
		{
			if (item.Value.m_realSpendTime > 240 && !item.Value.m_upload)
			{
				num++;
			}
		}
		BytesBuffer bytesBuffer = new BytesBuffer(6 + BytesBuffer.GetStringShortLength(GameApp.GetInstance().GetUserState().GetRoleName()) + 2 + num * 24);
		bytesBuffer.AddShort((short)int.Parse(GlobalState.version));
		bytesBuffer.AddInt(GlobalState.user_id);
		bytesBuffer.AddStringShortLength(GameApp.GetInstance().GetUserState().GetRoleName());
		bytesBuffer.AddShort((short)num);
		foreach (KeyValuePair<short, QuestLog> item2 in m_commonQuestLst)
		{
			if (item2.Value.m_realSpendTime > 240 && !item2.Value.m_upload)
			{
				bytesBuffer.AddShort(item2.Value.m_id);
				bytesBuffer.AddInt(item2.Value.m_realSpendTime);
				bytesBuffer.AddShort(item2.Value.m_deadCount);
				bytesBuffer.AddLong(GetTotalMilliseconds(item2.Value.m_accQuestDate));
				bytesBuffer.AddLong(GetTotalMilliseconds(item2.Value.m_comQuestDate));
			}
		}
		return bytesBuffer.GetBytes();
	}
}
