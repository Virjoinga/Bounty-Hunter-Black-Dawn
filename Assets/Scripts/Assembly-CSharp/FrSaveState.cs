using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("GameEvent")]
[Tooltip("Custom Action...")]
public class FrSaveState : FsmStateAction
{
	[UIHint(UIHint.Variable)]
	[RequiredField]
	public FsmString stringVariable;

	public override void OnEnter()
	{
		FsmString fsmString = base.Fsm.GetFsmString(stringVariable.Name);
		fsmString.Value = base.Fsm.ActiveStateName;
		Debug.Log("State: " + fsmString.Value);
		GameApp.GetInstance().GetUserState().SetNpcState(base.Owner.name, fsmString.Value);
		QuestScript component = base.Owner.GetComponent<QuestScript>();
		component.UpdateFlag();
	}
}
