namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Builds a String from other Strings.")]
	[ActionCategory(ActionCategory.String)]
	public class BuildString : FsmStateAction
	{
		[RequiredField]
		public FsmString[] stringParts;

		public FsmString separator;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public bool everyFrame;

		private string result;

		public override void Reset()
		{
			stringParts = new FsmString[3];
			separator = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoBuildString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoBuildString();
		}

		private void DoBuildString()
		{
			if (storeResult != null)
			{
				result = string.Empty;
				FsmString[] array = stringParts;
				foreach (FsmString fsmString in array)
				{
					result += fsmString;
					result += separator.Value;
				}
				storeResult.Value = result;
			}
		}
	}
}
