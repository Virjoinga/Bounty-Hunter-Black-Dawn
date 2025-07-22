namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts an Integer value to a String value with an optional format.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertIntToString : FsmStateAction
	{
		[Tooltip("The int variable to convert.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		[Tooltip("A string variable to store the converted value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		[Tooltip("Optional Format, allows for leading zeroes. E.g., 0000")]
		public FsmString format;

		[Tooltip("Repeat every frame. Useful if the int variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			stringVariable = null;
			everyFrame = false;
			format = null;
		}

		public override void OnEnter()
		{
			DoConvertIntToString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertIntToString();
		}

		private void DoConvertIntToString()
		{
			if (format.IsNone || string.IsNullOrEmpty(format.Value))
			{
				stringVariable.Value = intVariable.Value.ToString();
			}
			else
			{
				stringVariable.Value = intVariable.Value.ToString(format.Value);
			}
		}
	}
}
