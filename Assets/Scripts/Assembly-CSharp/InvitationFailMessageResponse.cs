using UnityEngine;

public class InvitationFailMessageResponse : Response
{
	private int m_ID;

	private byte m_Type;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_ID = bytesBuffer.ReadInt();
		m_Type = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		string empty = string.Empty;
		string text = string.Empty;
		switch (m_Type)
		{
		case 0:
			text = LocalizationManager.GetInstance().GetString("MSG_TELEPROT_IN_MENU");
			break;
		case 1:
			text = LocalizationManager.GetInstance().GetString("MSG_TELEPROT_DYING");
			break;
		case 2:
			text = LocalizationManager.GetInstance().GetString("MSG_TELEPORT_BOSS_FIGHT");
			break;
		case 3:
			text = LocalizationManager.GetInstance().GetString("MSG_TELEPORT_PLAYER_REJECT");
			break;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (m_ID == channelID)
		{
			empty = localPlayer.GetDisplayName();
		}
		else
		{
			if (m_Type == 2)
			{
				return;
			}
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_ID);
			if (remotePlayerByUserID == null)
			{
				Debug.Log("Can't find remote player " + m_ID);
				return;
			}
			empty = remotePlayerByUserID.GetDisplayName();
		}
		UserStateHUD.GetInstance().InfoBox.PushInfo(empty + " " + text);
	}
}
