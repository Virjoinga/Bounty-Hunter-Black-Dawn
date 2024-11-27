using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clamps the value of Float Variable to a Min/Max range.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatClamp : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat minValue;

		[RequiredField]
		public FsmFloat maxValue;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			minValue = null;
			maxValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoClamp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoClamp();
		}

		private void DoClamp()
		{
			floatVariable.Value = Mathf.Clamp(floatVariable.Value, minValue.Value, maxValue.Value);
		}
	}
}
