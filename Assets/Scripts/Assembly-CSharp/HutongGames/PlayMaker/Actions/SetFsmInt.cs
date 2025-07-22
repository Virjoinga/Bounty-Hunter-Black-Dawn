using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of an Integer Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmInt : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmInt)]
		public FsmString variableName;

		[RequiredField]
		public FsmInt setValue;

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
			DoSetFsmInt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmInt()
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
				FsmInt fsmInt = fsm.FsmVariables.GetFsmInt(variableName.Value);
				if (fsmInt != null)
				{
					fsmInt.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmInt();
		}
	}
}
