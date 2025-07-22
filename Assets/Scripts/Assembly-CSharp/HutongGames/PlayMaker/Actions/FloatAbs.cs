using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Float Variable to its absolute value.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAbs : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatAbs();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatAbs();
		}

		private void DoFloatAbs()
		{
			floatVariable.Value = Mathf.Abs(floatVariable.Value);
		}
	}
}
