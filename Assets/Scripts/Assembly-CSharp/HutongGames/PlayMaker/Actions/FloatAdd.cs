using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a value to a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAdd : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat add;

		public bool everyFrame;

		public bool perSecond;

		public override void Reset()
		{
			floatVariable = null;
			add = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoFloatAdd();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatAdd();
		}

		private void DoFloatAdd()
		{
			if (!perSecond)
			{
				floatVariable.Value += add.Value;
			}
			else
			{
				floatVariable.Value += add.Value * Time.deltaTime;
			}
		}
	}
}
