using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Game Object's Layer.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetLayer : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Layer)]
		public int layer;

		public override void Reset()
		{
			gameObject = null;
			layer = 0;
		}

		public override void OnEnter()
		{
			DoSetLayer();
			Finish();
		}

		private void DoSetLayer()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				ownerDefaultTarget.layer = layer;
			}
		}
	}
}
