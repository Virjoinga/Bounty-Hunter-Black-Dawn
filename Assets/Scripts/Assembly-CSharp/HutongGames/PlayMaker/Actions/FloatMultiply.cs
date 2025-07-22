namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Multiplies one Float by another.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatMultiply : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat multiplyBy;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			multiplyBy = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value *= multiplyBy.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value *= multiplyBy.Value;
		}
	}
}
