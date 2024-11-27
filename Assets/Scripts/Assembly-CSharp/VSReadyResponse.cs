using System.Collections.Generic;

public class VSReadyResponse : Response
{
	private int m_UserID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_UserID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		if (m_UserID == Lobby.GetInstance().GetChannelID())
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.IsPVPReady = true;
		}
		else
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(m_UserID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.IsPVPReady = true;
			}
		}
		bool flag = true;
		flag = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.IsPVPReady;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (!item.IsPVPReady)
			{
				flag = false;
				break;
			}
		}
		if (flag && UIVS.instance != null)
		{
			UIVS.instance.NotifyAllReady();
		}
	}
}
