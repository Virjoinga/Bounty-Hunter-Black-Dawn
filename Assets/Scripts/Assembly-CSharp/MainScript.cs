using UnityEngine;

public class MainScript : MonoBehaviour
{
	protected NetworkManager networkMgr;

	public bool bInit;

	private void Start()
	{
	}

	public void Init()
	{
		GameApp.GetInstance().CreateLootManager();
		GameApp.GetInstance().InitGameWorld();
		GameApp.GetInstance().GetGameWorld().InitLocalPlayer();
		GameApp.GetInstance().CreateGameScene();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
		}
		bInit = true;
	}

	private void Update()
	{
		if (bInit)
		{
			if (GameApp.GetInstance().GetGameMode().IsConnNetwork())
			{
				TimeManager.GetInstance().Loop();
				networkMgr = GameApp.GetInstance().GetNetworkManager();
				networkMgr.ProcessReceivedPackets();
			}
			GameApp.GetInstance().Loop(Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		if (bInit)
		{
			GameApp.GetInstance().LateLoop(Time.deltaTime);
		}
	}
}
