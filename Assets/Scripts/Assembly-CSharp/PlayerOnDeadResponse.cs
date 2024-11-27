public class PlayerOnDeadResponse : Response
{
	protected int m_playerId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerId = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerId);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.DYING_STATE.OnDead();
		}
	}
}
