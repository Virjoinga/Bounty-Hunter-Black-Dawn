using UnityEngine;

internal class DownloadMithrilResponse : Response
{
	public int m_mithrilDelta;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_mithrilDelta = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameApp.GetInstance().GetGlobalState().AddMithril(m_mithrilDelta);
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript != null)
		{
			Debug.Log("m_mithrilDelta: " + m_mithrilDelta);
			inGameUIScript.ShowCompenstaeForNet(m_mithrilDelta);
		}
	}
}
