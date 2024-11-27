namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts an String value to an Int value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertStringToInt : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			stringVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertStringToInt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertStringToInt();
		}

		private void DoConvertStringToInt()
		{
			intVariable.Value = int.Parse(stringVariable.Value);
		}
	}
}
