using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
	[ActionCategory(ActionCategory.GUIElement)]
	public class SetGUITexture : FsmStateAction
	{
		[CheckForComponent(typeof(GUITexture))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmTexture texture;

		public override void Reset()
		{
			gameObject = null;
			texture = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null && ownerDefaultTarget.GetComponent<GUITexture>() != null)
			{
				ownerDefaultTarget.GetComponent<GUITexture>().texture = texture.Value;
			}
			Finish();
		}
	}
}
