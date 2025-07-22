using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Material Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetFsmMaterial : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmMaterial)]
		public FsmString variableName;

		[RequiredField]
		public FsmMaterial setValue;

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
				FsmMaterial fsmMaterial = fsm.FsmVariables.GetFsmMaterial(variableName.Value);
				if (fsmMaterial != null)
				{
					fsmMaterial.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}
	}
}
