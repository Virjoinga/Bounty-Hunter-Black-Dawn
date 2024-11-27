using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custom Action...")]
public class FrTeam : FsmStateAction
{
	public override void OnEnter()
	{
		if (TutorialManager.GetInstance().IsShopTutorialOk())
		{
			InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
			if (inGameUIScript.FrGetCurrentPhase() != 9)
			{
				inGameUIScript.FrGoToPhase(9, false, true, false);
			}
		}
	}

	public override void OnLateUpdate()
	{
		if (TutorialManager.GetInstance().IsShopTutorialOk())
		{
			InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
			if (inGameUIScript.FrGetCurrentPhase() != 9)
			{
				Finish();
			}
		}
		else
		{
			Finish();
		}
	}
}
