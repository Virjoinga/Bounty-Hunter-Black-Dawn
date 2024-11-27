using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Float Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmFloat : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.FsmFloat)]
		[RequiredField]
		public FsmString variableName;

		[RequiredField]
		public FsmFloat setValue;

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
			DoSetFsmFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmFloat()
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
				FsmFloat fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
				if (fsmFloat != null)
				{
					fsmFloat.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmFloat();
		}
	}
}
