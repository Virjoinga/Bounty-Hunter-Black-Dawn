using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a String Variable in another FSM.")]
	public class SetFsmString : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmString)]
		public FsmString variableName;

		[RequiredField]
		public FsmString setValue;

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
			DoSetFsmString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmString()
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
				FsmString fsmString = fsm.FsmVariables.GetFsmString(variableName.Value);
				if (fsmString != null)
				{
					fsmString.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmString();
		}
	}
}
