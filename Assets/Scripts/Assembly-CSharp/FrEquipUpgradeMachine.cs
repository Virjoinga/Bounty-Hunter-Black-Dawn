using HutongGames.PlayMaker;

[ActionCategory("GameEvent")]
[Tooltip("Custion Action...")]
public class FrEquipUpgradeMachine : FsmStateAction
{
	public FsmEvent closeEvent;

	public override void Reset()
	{
		closeEvent = null;
	}

	public override void OnEnter()
	{
		UIEquipmentUpgrade.Show();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.FrGetCurrentPhase() != 40)
		{
			base.Fsm.DelayedEvent(closeEvent, 0.5f);
		}
	}

	public override string ErrorCheck()
	{
		return null;
	}
}
