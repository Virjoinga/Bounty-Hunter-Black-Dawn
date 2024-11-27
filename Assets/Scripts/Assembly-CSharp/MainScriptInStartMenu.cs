using UnityEngine;

public class MainScriptInStartMenu : MonoBehaviour
{
	protected NetworkManager networkMgr;

	public bool bInit;

	public void Init()
	{
		GameApp.GetInstance().CreateLootManager();
		GameApp.GetInstance().InitGameWorld();
		GameApp.GetInstance().GetGameWorld().InitLocalPlayer();
		bInit = true;
	}

	private void Update()
	{
		if (bInit && GameApp.GetInstance().GetGameMode().IsConnNetwork())
		{
			TimeManager.GetInstance().Loop();
			networkMgr = GameApp.GetInstance().GetNetworkManager();
			networkMgr.ProcessReceivedPackets();
		}
	}
}
