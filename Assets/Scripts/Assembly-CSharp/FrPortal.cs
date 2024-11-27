using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custion Action...")]
public class FrPortal : FsmStateAction
{
	public override void OnEnter()
	{
		if (TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk())
		{
			UIPortal.Show();
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!UIPortal.IsShow() && TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk())
		{
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
			Finish();
		}
	}

	public override string ErrorCheck()
	{
		return null;
	}
}
