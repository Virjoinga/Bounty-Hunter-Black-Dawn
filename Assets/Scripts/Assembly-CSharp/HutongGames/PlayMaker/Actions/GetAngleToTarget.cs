using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Angle between a Game Object's forward axis and a Target. The Target can be defined as a Game Object or a world Position. If you specify both, then the Position will be used as a local offset from the Object's position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class GetAngleToTarget : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmGameObject targetObject;

		public FsmVector3 targetPosition;

		public FsmBool ignoreHeight;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeAngle;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			ignoreHeight = true;
			storeAngle = null;
			everyFrame = false;
		}

		public override void OnLateUpdate()
		{
			DoGetAngleToTarget();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoGetAngleToTarget()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			GameObject value = targetObject.Value;
			if (!(value == null) || !targetPosition.IsNone)
			{
				Vector3 vector = ((!(value != null)) ? targetPosition.Value : (targetPosition.IsNone ? value.transform.position : value.transform.TransformPoint(targetPosition.Value)));
				if (ignoreHeight.Value)
				{
					vector.y = ownerDefaultTarget.transform.position.y;
				}
				Vector3 from = vector - ownerDefaultTarget.transform.position;
				storeAngle.Value = Vector3.Angle(from, ownerDefaultTarget.transform.forward);
			}
		}
	}
}
