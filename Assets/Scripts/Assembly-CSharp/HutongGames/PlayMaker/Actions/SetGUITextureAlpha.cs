using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Alpha of the GUITexture attached to a Game Object. Useful for fading GUI elements in/out.")]
	public class SetGUITextureAlpha : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmFloat alpha;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			alpha = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGUITextureAlpha();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGUITextureAlpha();
		}

		private void DoGUITextureAlpha()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null && ownerDefaultTarget.GetComponent<GUITexture>() != null)
			{
				Color color = ownerDefaultTarget.GetComponent<GUITexture>().color;
				ownerDefaultTarget.GetComponent<GUITexture>().color = new Color(color.r, color.g, color.b, alpha.Value);
			}
		}
	}
}
