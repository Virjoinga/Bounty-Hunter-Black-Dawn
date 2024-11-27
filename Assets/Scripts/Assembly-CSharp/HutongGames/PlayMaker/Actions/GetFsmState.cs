using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets the name of the specified FSMs current state. Either reference the fsm component directly, or find it on a game object.")]
	public class GetFsmState : FsmStateAction
	{
		public PlayMakerFSM fsmComponent;

		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of Fsm on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public bool everyFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			fsmComponent = null;
			gameObject = null;
			fsmName = string.Empty;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmState();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmState();
		}

		private void DoGetFsmState()
		{
			if (fsm == null)
			{
				if (fsmComponent != null)
				{
					fsm = fsmComponent;
				}
				else
				{
					GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
					if (ownerDefaultTarget != null)
					{
						fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
					}
				}
				if (fsm == null)
				{
					storeResult.Value = string.Empty;
					return;
				}
			}
			storeResult.Value = fsm.ActiveStateName;
		}
	}
}
