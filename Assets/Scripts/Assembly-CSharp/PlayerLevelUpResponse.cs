public class PlayerLevelUpResponse : Response
{
	protected int playerID;

	protected short m_Level;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_Level = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.GetUserState().SetCharLevel(m_Level);
		}
	}
}
