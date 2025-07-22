using UnityEngine;

public class PlayerRecoverFromDyingResponse : Response
{
	protected int m_playerId;

	protected int m_hp;

	protected int m_shield;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerId = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
		m_shield = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		Debug.Log("RecoverFromDying..................");
		if (channelID == m_playerId)
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null && localPlayer.DYING_STATE.InDyingState)
			{
				Debug.Log("RecoverFromDying hp: " + m_hp);
				Debug.Log("RecoverFromDying shield: " + m_shield);
				localPlayer.DYING_STATE.OnRecoverFromDying(m_hp, m_shield);
			}
		}
		else
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_playerId);
			if (remotePlayerByUserID != null && remotePlayerByUserID.DYING_STATE.InDyingState)
			{
				remotePlayerByUserID.DYING_STATE.OnRecoverFromDying(m_hp, m_shield);
				Debug.Log("RecoverFromDying hp: " + m_hp);
				Debug.Log("RecoverFromDying shield: " + m_shield);
			}
		}
	}
}
