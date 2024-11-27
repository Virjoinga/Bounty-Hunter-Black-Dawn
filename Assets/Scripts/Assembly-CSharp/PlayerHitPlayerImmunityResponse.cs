using UnityEngine;

internal class PlayerHitPlayerImmunityResponse : Response
{
	protected int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID != channelID)
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				NumberManager.GetInstance().ShowImmunity(remotePlayerByUserID.GetPosition() + Vector3.up * 1.5f);
			}
		}
	}
}
