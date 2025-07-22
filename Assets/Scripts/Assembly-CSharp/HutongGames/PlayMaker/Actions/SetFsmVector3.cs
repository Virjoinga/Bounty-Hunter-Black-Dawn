using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Vector3 Variable in another FSM.")]
	public class SetFsmVector3 : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[UIHint(UIHint.FsmVector3)]
		[RequiredField]
		public FsmString variableName;

		[RequiredField]
		public FsmVector3 setValue;

		public bool everyFrame;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			setValue = null;
		}

		public override void OnEnter()
		{
			DoSetFsmVector3();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmVector3()
		{
			if (setValue == null)
			{
				return;
			}
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
				FsmVector3 fsmVector = fsm.FsmVariables.GetFsmVector3(variableName.Value);
				if (fsmVector != null)
				{
					fsmVector.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmVector3();
		}
	}
}
