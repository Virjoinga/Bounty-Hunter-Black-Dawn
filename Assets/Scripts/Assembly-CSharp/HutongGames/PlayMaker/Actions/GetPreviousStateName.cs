namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the name of the previously active state and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetPreviousStateName : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeName;

		public override void Reset()
		{
			storeName = null;
		}

		public override void OnEnter()
		{
			storeName.Value = ((base.Fsm.PreviousActiveState != null) ? base.Fsm.PreviousActiveState.Name : null);
			Finish();
		}
	}
}
