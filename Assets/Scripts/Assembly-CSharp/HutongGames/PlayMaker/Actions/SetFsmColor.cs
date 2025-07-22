using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Color Variable in another FSM.")]
	public class SetFsmColor : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmColor)]
		public FsmString variableName;

		[RequiredField]
		public FsmColor setValue;

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
			DoSetFsmColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmColor()
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
				FsmColor fsmColor = fsm.FsmVariables.GetFsmColor(variableName.Value);
				if (fsmColor != null)
				{
					fsmColor.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmColor();
		}
	}
}
