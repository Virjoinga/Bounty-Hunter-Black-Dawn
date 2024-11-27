namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of a float variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class FloatChanged : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmEvent changedEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		private float previousValue;

		public override void Reset()
		{
			floatVariable = null;
			changedEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			if (floatVariable.IsNone)
			{
				Finish();
			}
			else
			{
				previousValue = floatVariable.Value;
			}
		}

		public override void OnUpdate()
		{
			storeResult.Value = false;
			if (floatVariable.Value != previousValue)
			{
				storeResult.Value = true;
				base.Fsm.Event(changedEvent);
			}
		}
	}
}
