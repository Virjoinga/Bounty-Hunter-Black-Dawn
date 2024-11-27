using HutongGames.PlayMaker;

[Tooltip("Custion Action...")]
[ActionCategory("GameEvent")]
public class FrCheckCondition : FsmStateAction
{
	public FsmEvent equalTrueEvent;

	public FsmEvent notEqualTrueEvent;

	public override void Reset()
	{
		equalTrueEvent = null;
		notEqualTrueEvent = null;
	}

	public override void OnEnter()
	{
		InGameUIScript inGameUIScript = GameApp.GetInstance().GetUIStateManager() as InGameUIScript;
		if (inGameUIScript.FrGetCurrentPhase() == 17 || inGameUIScript.FrGetCurrentPhase() == 21 || !InGameUIScript.bInited)
		{
			base.Fsm.DelayedEvent(notEqualTrueEvent, 2f);
		}
		else
		{
			base.Fsm.Event(equalTrueEvent);
		}
	}
}
