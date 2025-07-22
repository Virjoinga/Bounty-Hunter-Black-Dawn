using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Performs math operations on 2 Floats: Add, Subtract, Multiply, Divide, Min, Max.")]
	public class FloatOperator : FsmStateAction
	{
		public enum Operation
		{
			Add = 0,
			Subtract = 1,
			Multiply = 2,
			Divide = 3,
			Min = 4,
			Max = 5
		}

		[RequiredField]
		public FsmFloat float1;

		[RequiredField]
		public FsmFloat float2;

		public Operation operation;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			float1 = null;
			float2 = null;
			operation = Operation.Add;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatOperator();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatOperator();
		}

		private void DoFloatOperator()
		{
			float value = float1.Value;
			float value2 = float2.Value;
			switch (operation)
			{
			case Operation.Add:
				storeResult.Value = value + value2;
				break;
			case Operation.Subtract:
				storeResult.Value = value - value2;
				break;
			case Operation.Multiply:
				storeResult.Value = value * value2;
				break;
			case Operation.Divide:
				storeResult.Value = value / value2;
				break;
			case Operation.Min:
				storeResult.Value = Mathf.Min(value, value2);
				break;
			case Operation.Max:
				storeResult.Value = Mathf.Max(value, value2);
				break;
			}
		}
	}
}
