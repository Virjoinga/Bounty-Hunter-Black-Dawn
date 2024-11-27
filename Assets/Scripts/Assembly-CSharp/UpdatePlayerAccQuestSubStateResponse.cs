public class UpdatePlayerAccQuestSubStateResponse : Response
{
	protected int m_playerChannleID;

	protected short m_questId;

	protected byte m_questsSubState;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerChannleID = bytesBuffer.ReadInt();
		m_questId = bytesBuffer.ReadShort();
		m_questsSubState = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerChannleID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		UserState userState = remotePlayerByUserID.GetUserState();
		if (userState != null)
		{
			QuestState objFromAccStateLst = userState.m_questStateContainer.GetObjFromAccStateLst(m_questId);
			if (objFromAccStateLst != null)
			{
				objFromAccStateLst.m_status = (QuestPhase)m_questsSubState;
			}
		}
	}
}
