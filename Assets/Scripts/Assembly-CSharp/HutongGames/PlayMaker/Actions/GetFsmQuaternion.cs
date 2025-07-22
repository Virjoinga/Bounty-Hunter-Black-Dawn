using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a Quaternion Variable from another FSM.")]
	public class GetFsmQuaternion : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.FsmQuaternion)]
		[RequiredField]
		public FsmString variableName;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion storeValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private GameObject goLastFrame;

		protected PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = string.Empty;
			variableName = string.Empty;
			storeValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmVariable();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariable();
		}

		private void DoGetFsmVariable()
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
			if (!(fsm == null) && storeValue != null)
			{
				FsmQuaternion fsmQuaternion = fsm.FsmVariables.GetFsmQuaternion(variableName.Value);
				if (fsmQuaternion != null)
				{
					storeValue.Value = fsmQuaternion.Value;
				}
			}
		}
	}
}
