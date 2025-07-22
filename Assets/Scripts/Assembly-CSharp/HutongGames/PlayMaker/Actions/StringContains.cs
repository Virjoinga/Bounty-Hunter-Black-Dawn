namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a String contains another String.")]
	public class StringContains : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The String variable to test.")]
		public FsmString stringVariable;

		[RequiredField]
		[Tooltip("Test if the String variable contains this string.")]
		public FsmString containsString;

		[Tooltip("Event to send if true.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if false.")]
		public FsmEvent falseEvent;

		[Tooltip("Store the true/false result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			containsString = string.Empty;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringContains();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringContains();
		}

		private void DoStringContains()
		{
			if (!stringVariable.IsNone && !containsString.IsNone)
			{
				bool flag = stringVariable.Value.Contains(containsString.Value);
				if (storeResult != null)
				{
					storeResult.Value = flag;
				}
				if (flag && trueEvent != null)
				{
					base.Fsm.Event(trueEvent);
				}
				else if (!flag && falseEvent != null)
				{
					base.Fsm.Event(falseEvent);
				}
			}
		}
	}
}
