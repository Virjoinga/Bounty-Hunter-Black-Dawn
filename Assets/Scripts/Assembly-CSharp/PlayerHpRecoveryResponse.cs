internal class PlayerHpRecoveryResponse : Response
{
	protected int playerID;

	protected int m_hp;

	protected int m_MaxHp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
		m_MaxHp = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.MaxHp = m_MaxHp;
			remotePlayerByUserID.Hp = m_hp;
		}
	}
}
