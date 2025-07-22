using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to a Color.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToColor : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The bool variable to test.")]
		[RequiredField]
		public FsmBool boolVariable;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The color variable to set based on the bool variable value.")]
		public FsmColor colorVariable;

		[Tooltip("Color if bool variable is false.")]
		public FsmColor falseColor;

		[Tooltip("Color if bool variable is true.")]
		public FsmColor trueColor;

		[Tooltip("Repeat every frame. Useful if the bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			colorVariable = null;
			falseColor = Color.black;
			trueColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToColor();
		}

		private void DoConvertBoolToColor()
		{
			colorVariable.Value = ((!boolVariable.Value) ? falseColor.Value : trueColor.Value);
		}
	}
}
