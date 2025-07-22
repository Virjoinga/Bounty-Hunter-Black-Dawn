using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Moves a Game Object towards a Target. Optionally sends an event when successful. The Target can be specified as a Game Object or a world Position. If you specify both, then the Position is used as a local offset from the Object's Position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class MoveTowards : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmGameObject targetObject;

		public FsmVector3 targetPosition;

		public FsmBool ignoreVertical;

		[HasFloatSlider(0f, 20f)]
		public FsmFloat maxSpeed;

		[HasFloatSlider(0f, 5f)]
		public FsmFloat finishDistance;

		public FsmEvent finishEvent;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			maxSpeed = 10f;
			finishDistance = 1f;
			finishEvent = null;
		}

		public override void OnUpdate()
		{
			DoMoveTowards();
		}

		private void DoMoveTowards()
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
				if (ignoreVertical.Value)
				{
					vector.y = ownerDefaultTarget.transform.position.y;
				}
				ownerDefaultTarget.transform.position = Vector3.MoveTowards(ownerDefaultTarget.transform.position, vector, maxSpeed.Value * Time.deltaTime);
				float magnitude = (ownerDefaultTarget.transform.position - vector).magnitude;
				if (magnitude < finishDistance.Value)
				{
					base.Fsm.Event(finishEvent);
					Finish();
				}
			}
		}
	}
}
