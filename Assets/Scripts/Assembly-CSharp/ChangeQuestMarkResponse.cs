public class ChangeQuestMarkResponse : Response
{
	public short m_questCommonId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_questCommonId = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		short currentQuest = GameApp.GetInstance().GetUserState().GetCurrentQuest();
		Lobby.GetInstance().SetCurrentMarkQuest(m_questCommonId);
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.CheckHasBeenAcceptedWithCommonID(m_questCommonId))
		{
			GameApp.GetInstance().GetUserState().SetCurrentQuest(m_questCommonId);
		}
		if (GameApp.GetInstance().GetUserState().GetCurrentQuest() != currentQuest)
		{
			GameApp.GetInstance().GetUserState().m_questStateContainer.UpdateQuestProgressForHUD(0.1f);
		}
		if (UIQuest.m_instance != null && UIQuest.m_instance.m_teamQuest.gameObject.active)
		{
			UITeamQuest component = UIQuest.m_instance.m_teamQuest.GetComponent<UITeamQuest>();
			component.UpdateMarkQuest(m_questCommonId);
		}
	}
}
