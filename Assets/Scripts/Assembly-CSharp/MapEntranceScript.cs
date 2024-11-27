using UnityEngine;

public class MapEntranceScript : MonoBehaviour, EffectsCameraListener
{
	public string mapName = "Instance1";

	public float radius = 2f;

	public SubMode mode;

	public int sceneID = 1;

	public int param;

	private bool bOK = true;

	private bool bTeleportActive;

	private void Start()
	{
	}

	private void Update()
	{
		if (bTeleportActive)
		{
			return;
		}
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene == null)
		{
			return;
		}
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null || !(localPlayer.GetTransform() != null))
		{
			return;
		}
		if (Vector3.Distance(localPlayer.GetTransform().position, base.transform.position) < radius)
		{
			if (bOK)
			{
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					SceneConfig sceneConfig = GameConfig.GetInstance().sceneConfig[mapName];
					InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
						.GetUserID(), InvitationRequest.Type.Story, mode, sceneConfig.SceneID, (short)param);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					bOK = false;
				}
				else
				{
					GameApp.GetInstance().GetGameMode().SubModePlay = mode;
					EnterMap();
				}
			}
		}
		else if (!bOK)
		{
			bOK = true;
		}
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		EffectsCamera.instance.RemoveListener();
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		gameScene.LeaveScene();
		Application.LoadLevel(mapName);
	}

	public void EnterMap()
	{
		Debug.Log(GameApp.GetInstance().GetGameMode().SubModePlay);
		bTeleportActive = true;
		EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
	}
}
