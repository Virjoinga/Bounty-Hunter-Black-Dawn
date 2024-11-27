namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends Events based on the value of a Boolean Variable.")]
	[ActionCategory(ActionCategory.Logic)]
	public class BoolTest : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		public FsmEvent isTrue;

		public FsmEvent isFalse;

		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			isTrue = null;
			isFalse = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			base.Fsm.Event((!boolVariable.Value) ? isFalse : isTrue);
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			base.Fsm.Event((!boolVariable.Value) ? isFalse : isTrue);
		}
	}
}
