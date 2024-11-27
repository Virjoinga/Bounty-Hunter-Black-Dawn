using HutongGames.PlayMaker;

[Tooltip("Custom Action...")]
[ActionCategory("GameEvent")]
public class FrLoadState : FsmStateAction
{
	[RequiredField]
	[UIHint(UIHint.Variable)]
	public FsmString stringVariable;

	public override void Reset()
	{
		stringVariable = null;
	}

	public override void OnEnter()
	{
		FsmString fsmString = base.Fsm.GetFsmString(stringVariable.Name);
		fsmString.Value = GameApp.GetInstance().GetUserState().GetNpcState(base.Owner.name);
		Finish();
	}
}
