namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts an Integer value to a Float value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertIntToFloat : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			floatVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertIntToFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertIntToFloat();
		}

		private void DoConvertIntToFloat()
		{
			floatVariable.Value = intVariable.Value;
		}
	}
}
