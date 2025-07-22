using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of an Object Variable in another FSM.")]
	public class SetFsmObject : FsmStateAction
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
		public FsmObject setValue;

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
				FsmObject fsmObject = fsm.FsmVariables.GetFsmObject(variableName.Value);
				if (fsmObject != null)
				{
					fsmObject.Value = setValue.Value;
				}
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}
	}
}
