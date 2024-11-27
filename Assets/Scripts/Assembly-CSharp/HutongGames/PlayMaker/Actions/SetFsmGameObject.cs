using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Game Object Variable in another FSM. Accept null reference")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmGameObject : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmGameObject)]
		public FsmString variableName;

		public FsmGameObject setValue;

		public bool everyFrame;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmGameObject();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmGameObject()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != goLastFrame)
			{
				goLastFrame = ownerDefaultTarget;
				fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
			}
			if (!(fsm == null))
			{
				FsmGameObject fsmGameObject = fsm.FsmVariables.GetFsmGameObject(variableName.Value);
				if (fsmGameObject != null)
				{
					fsmGameObject.Value = ((setValue != null) ? setValue.Value : null);
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmGameObject();
		}
	}
}
