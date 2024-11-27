namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Right n characters from a String.")]
	[ActionCategory(ActionCategory.String)]
	public class GetStringRight : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		public FsmInt charCount;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			charCount = 0;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetStringRight();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetStringRight();
		}

		private void DoGetStringRight()
		{
			if (stringVariable != null && storeResult != null)
			{
				string value = stringVariable.Value;
				storeResult.Value = value.Substring(value.Length - charCount.Value, charCount.Value);
			}
		}
	}
}
