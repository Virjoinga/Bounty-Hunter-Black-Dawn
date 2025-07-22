using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Rect Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmRect : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.FsmMaterial)]
		[RequiredField]
		public FsmString variableName;

		[RequiredField]
		public FsmRect setValue;

		public bool everyFrame;

		private GameObject goLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			variableName = string.Empty;
			setValue = null;
			everyFrame = false;
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
				FsmRect fsmRect = fsm.FsmVariables.GetFsmRect(variableName.Value);
				if (fsmRect != null)
				{
					fsmRect.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}
	}
}
