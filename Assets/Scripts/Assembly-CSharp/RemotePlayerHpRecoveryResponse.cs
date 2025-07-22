using UnityEngine;

internal class RemotePlayerHpRecoveryResponse : Response
{
	protected int playerID;

	protected short m_hp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Debug.Log(playerID + "------------" + GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserID());
		if (GameApp.GetInstance().GetGameWorld().IsLocalPlayer(playerID))
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.RecoverHP(m_hp);
		}
	}
}
