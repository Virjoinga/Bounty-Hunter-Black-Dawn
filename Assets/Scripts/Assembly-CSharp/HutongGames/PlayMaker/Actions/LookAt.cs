using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a Game Object so its forward vector points at a Target. The Target can be specified as a GameObject or a world Position. If you specify both, then Position specifies a local offset from the target object's Position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class LookAt : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to rorate.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The GameObject to Look At.")]
		public FsmGameObject targetObject;

		[Tooltip("World position to look at, or local offset from Target Object if specified.")]
		public FsmVector3 targetPosition;

		[Tooltip("Rotate the GameObject to point its up direction vector in the direction hinted at by the Up Vector. See Unity Look At docs for more details.")]
		public FsmVector3 upVector;

		[Tooltip("Don't rotate vertically.")]
		public FsmBool keepVertical;

		[Title("Draw Debug Line")]
		[Tooltip("Draw a debug line from the GameObject to the Target.")]
		public FsmBool debug;

		[Tooltip("Color to use for the debug line.")]
		public FsmColor debugLineColor;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame = true;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			upVector = new FsmVector3
			{
				UseVariable = true
			};
			keepVertical = true;
			debug = false;
			debugLineColor = Color.yellow;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoLookAt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnLateUpdate()
		{
			DoLookAt();
		}

		private void DoLookAt()
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
				if (keepVertical.Value)
				{
					vector.y = ownerDefaultTarget.transform.position.y;
				}
				ownerDefaultTarget.transform.LookAt(vector, (!upVector.IsNone) ? upVector.Value : Vector3.up);
				if (debug.Value)
				{
					Debug.DrawLine(ownerDefaultTarget.transform.position, vector, debugLineColor.Value);
				}
			}
		}
	}
}
