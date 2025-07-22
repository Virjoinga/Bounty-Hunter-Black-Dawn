using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Bool Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmBool : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmBool)]
		public FsmString variableName;

		[RequiredField]
		public FsmBool setValue;

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
			DoSetFsmBool();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmBool()
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
				FsmBool fsmBool = fsm.FsmVariables.GetFsmBool(variableName.Value);
				if (fsmBool != null)
				{
					fsmBool.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}
	}
}
