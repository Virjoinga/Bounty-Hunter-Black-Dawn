using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Subtracts a value from a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatSubtract : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat subtract;

		public bool everyFrame;

		public bool perSecond;

		public override void Reset()
		{
			floatVariable = null;
			subtract = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoFloatSubtract();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatSubtract();
		}

		private void DoFloatSubtract()
		{
			if (!perSecond)
			{
				floatVariable.Value -= subtract.Value;
			}
			else
			{
				floatVariable.Value -= subtract.Value * Time.deltaTime;
			}
		}
	}
}
