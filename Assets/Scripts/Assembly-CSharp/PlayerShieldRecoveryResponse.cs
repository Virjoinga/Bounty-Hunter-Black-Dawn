public class PlayerShieldRecoveryResponse : Response
{
	protected int playerID;

	protected int m_Shield;

	protected int m_MaxShield;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_Shield = bytesBuffer.ReadInt();
		m_MaxShield = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.MaxShield = m_MaxShield;
			remotePlayerByUserID.Shield = m_Shield;
		}
	}
}
