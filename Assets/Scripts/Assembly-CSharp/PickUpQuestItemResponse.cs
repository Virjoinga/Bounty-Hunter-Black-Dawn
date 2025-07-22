internal class PickUpQuestItemResponse : Response
{
	protected short m_specialID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_specialID = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem(m_specialID);
		GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection(m_specialID);
	}
}
