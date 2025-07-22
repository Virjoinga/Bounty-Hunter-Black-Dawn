public class UploadQuestsRequest : Request
{
	protected short[] m_quests;

	protected byte[] m_questsSubState;

	protected short[] m_compQuests;

	public UploadQuestsRequest(QuestStateContainer questStateContainer)
	{
		requestID = 24;
		m_quests = new short[questStateContainer.m_accStateLst.Count];
		m_questsSubState = new byte[questStateContainer.m_accStateLst.Count];
		for (int i = 0; i < m_quests.Length; i++)
		{
			m_quests[i] = (short)questStateContainer.m_accStateLst[i].m_id;
			m_questsSubState[i] = (byte)questStateContainer.m_accStateLst[i].m_status;
		}
		m_compQuests = new short[questStateContainer.m_completedQuestLst.Count];
		for (int j = 0; j < m_compQuests.Length; j++)
		{
			m_compQuests[j] = (short)questStateContainer.m_completedQuestLst[j].m_id;
		}
	}

	public override byte[] GetBody()
	{
		int num = m_quests.Length;
		int num2 = m_compQuests.Length;
		BytesBuffer bytesBuffer = new BytesBuffer(4 + (num + num2) * 2 + num);
		bytesBuffer.AddShort((short)num);
		for (int i = 0; i < num; i++)
		{
			bytesBuffer.AddShort(m_quests[i]);
			bytesBuffer.AddByte(m_questsSubState[i]);
		}
		bytesBuffer.AddShort((short)num2);
		for (int j = 0; j < num2; j++)
		{
			bytesBuffer.AddShort(m_compQuests[j]);
		}
		return bytesBuffer.GetBytes();
	}
}
