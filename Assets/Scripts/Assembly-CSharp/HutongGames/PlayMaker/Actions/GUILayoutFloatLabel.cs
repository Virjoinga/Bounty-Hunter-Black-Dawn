using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout Label for a Float Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutFloatLabel : GUILayoutAction
	{
		[Tooltip("Text to put before the float variable.")]
		public FsmString prefix;

		[Tooltip("Float variable to display.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			prefix = string.Empty;
			style = string.Empty;
			floatVariable = null;
		}

		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(prefix.Value + floatVariable.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(prefix.Value + floatVariable.Value), style.Value, base.LayoutOptions);
			}
		}
	}
}
