using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Samples a Color on a continuous Colors gradient.")]
	public class ColorRamp : FsmStateAction
	{
		[RequiredField]
		public FsmColor[] colors;

		[RequiredField]
		public FsmFloat sampleAt;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor storeColor;

		public bool everyFrame;

		public override void Reset()
		{
			colors = new FsmColor[3];
			sampleAt = 0f;
			storeColor = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoColorRamp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoColorRamp();
		}

		private void DoColorRamp()
		{
			if (colors != null && colors.Length != 0 && sampleAt != null && storeColor != null)
			{
				float num = Mathf.Clamp(sampleAt.Value, 0f, colors.Length - 1);
				Color value;
				if (num == 0f)
				{
					value = colors[0].Value;
				}
				else if (num == (float)colors.Length)
				{
					value = colors[colors.Length - 1].Value;
				}
				else
				{
					Color value2 = colors[Mathf.FloorToInt(num)].Value;
					Color value3 = colors[Mathf.CeilToInt(num)].Value;
					num -= Mathf.Floor(num);
					value = Color.Lerp(value2, value3, num);
				}
				storeColor.Value = value;
			}
		}

		public override string ErrorCheck()
		{
			if (colors.Length < 2)
			{
				return "Define at least 2 colors to make a gradient.";
			}
			return null;
		}
	}
}
