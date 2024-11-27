using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Smoothly Rotates a Game Object so its forward vector points in the specified Direction.")]
	public class SmoothLookAtDirection : FsmStateAction
	{
		[Tooltip("The GameObject to rotate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The direction to smoothly rotate towards.")]
		[RequiredField]
		public FsmVector3 targetDirection;

		[Tooltip("Only rotate if Target Direction Vector length is greater than this threshold.")]
		public FsmFloat minMagnitude;

		[Tooltip("Keep this vector pointing up as the GameObject rotates.")]
		public FsmVector3 upVector;

		[Tooltip("Eliminate any tilt up/down as the GameObject rotates.")]
		[RequiredField]
		public FsmBool keepVertical;

		[HasFloatSlider(0.5f, 15f)]
		[RequiredField]
		[Tooltip("How quickly to rotate.")]
		public FsmFloat speed;

		[Tooltip("Perform in LateUpdate. This can help eliminate jitters in some situations.")]
		public bool lateUpdate;

		private GameObject previousGo;

		private Quaternion lastRotation;

		private Quaternion desiredRotation;

		public override void Reset()
		{
			gameObject = null;
			targetDirection = new FsmVector3
			{
				UseVariable = true
			};
			minMagnitude = 0.1f;
			upVector = new FsmVector3
			{
				UseVariable = true
			};
			keepVertical = true;
			speed = 5f;
			lateUpdate = true;
		}

		public override void OnEnter()
		{
			previousGo = null;
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				DoSmoothLookAtDirection();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSmoothLookAtDirection();
			}
		}

		private void DoSmoothLookAtDirection()
		{
			if (targetDirection.IsNone)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (previousGo != ownerDefaultTarget)
				{
					lastRotation = ownerDefaultTarget.transform.rotation;
					desiredRotation = lastRotation;
					previousGo = ownerDefaultTarget;
				}
				Vector3 value = targetDirection.Value;
				if (keepVertical.Value)
				{
					value.y = 0f;
				}
				if (value.sqrMagnitude > minMagnitude.Value)
				{
					desiredRotation = Quaternion.LookRotation(value, (!upVector.IsNone) ? upVector.Value : Vector3.up);
				}
				lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed.Value * Time.deltaTime);
				ownerDefaultTarget.transform.rotation = lastRotation;
			}
		}
	}
}
