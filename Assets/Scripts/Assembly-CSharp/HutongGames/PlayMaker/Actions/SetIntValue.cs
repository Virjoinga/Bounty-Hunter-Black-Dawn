namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of an Integer Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class SetIntValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		[RequiredField]
		public FsmInt intValue;

		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			intValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			intVariable.Value = intValue.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			intVariable.Value = intValue.Value;
		}
	}
}
