using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUI Box.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUIBox : GUIContentAction
	{
		public override void OnGUI()
		{
			base.OnGUI();
			GUI.Box(rect, content, style.Value);
		}
	}
}
