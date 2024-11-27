using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUI Label.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUILabel : GUIContentAction
	{
		public override void OnGUI()
		{
			base.OnGUI();
			GUI.Label(rect, content, style.Value);
		}
	}
}
