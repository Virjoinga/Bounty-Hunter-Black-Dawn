using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a Component attached to a GameObject and stores it in an Object variable. NOTE: Set the Object variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
	[ActionCategory(ActionCategory.UnityObject)]
	public class GetComponent : FsmStateAction
	{
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmObject storeComponent;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeComponent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetComponent();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetComponent();
		}

		private void DoGetComponent()
		{
			if (storeComponent != null)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
				if (!(ownerDefaultTarget == null))
				{
					storeComponent.Value = ownerDefaultTarget.GetComponent(storeComponent.ObjectType);
				}
			}
		}
	}
}
