namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to a Float value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToFloat : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The bool variable to test.")]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		[RequiredField]
		[Tooltip("The float variable to set based on the bool variable value.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		[Tooltip("Float value if bool variable is false.")]
		public FsmFloat falseValue;

		[Tooltip("Float value if bool variable is true.")]
		public FsmFloat trueValue;

		[Tooltip("Repeat every frame. Useful if the bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			floatVariable = null;
			falseValue = 0f;
			trueValue = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToFloat();
		}

		private void DoConvertBoolToFloat()
		{
			floatVariable.Value = ((!boolVariable.Value) ? falseValue.Value : trueValue.Value);
		}
	}
}
