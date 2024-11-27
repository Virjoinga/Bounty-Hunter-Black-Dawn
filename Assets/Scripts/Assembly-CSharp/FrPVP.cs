using HutongGames.PlayMaker;

[Tooltip("Custion Action...")]
[ActionCategory("GameEvent")]
public class FrPVP : FsmStateAction
{
	public override void OnEnter()
	{
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsMapTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk() && GameApp.GetInstance().GetUIStateManager().FrGetCurrentPhase() == 6)
		{
			if (GameApp.GetInstance().GetGameMode().TypeOfNetwork == NetworkType.Single)
			{
				GameApp.GetInstance().GetUIStateManager().FrGoToPhase(41, false, false, false);
			}
			else
			{
				InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID(), InvitationRequest.Type.VS, SubMode.Story, GameApp.GetInstance().GetGameWorld().CurrentSceneID, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		Finish();
	}

	public override string ErrorCheck()
	{
		return null;
	}
}
