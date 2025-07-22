using UnityEngine;

public class UIArenaSurvival : MonoBehaviour, IArenaMode
{
	private SubMode mode = SubMode.Arena_Survival;

	public void Go(byte sceneID)
	{
		if (GameApp.GetInstance().GetGameMode().IsPlayingArenaSubMode(mode) && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetUserID(), InvitationRequest.Type.Arena, mode, sceneID, 0);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		else
		{
			SceneConfig sceneConfig = GameApp.GetInstance().GetGameWorld().GetSceneConfig(sceneID);
			Arena.GetInstance().Enter(mode, sceneID, 0);
		}
	}
}
