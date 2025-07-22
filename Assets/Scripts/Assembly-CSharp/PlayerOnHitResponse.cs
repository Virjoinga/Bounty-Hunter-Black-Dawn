internal class PlayerOnHitResponse : Response
{
	protected int playerID;

	protected int m_hp;

	protected int m_shield;

	protected int m_extrashield;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
		m_shield = bytesBuffer.ReadInt();
		m_extrashield = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			localPlayer.UnderAttackSetExtraShield(m_extrashield);
			localPlayer.UnderAttackSetShield(m_shield);
			localPlayer.UnderAttackSetHP(m_hp);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.UnderAttackSetExtraShield(m_extrashield);
			remotePlayerByUserID.UnderAttackSetShield(m_shield);
			remotePlayerByUserID.UnderAttackSetHP(m_hp);
		}
	}
}
