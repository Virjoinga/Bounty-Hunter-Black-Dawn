using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Action version of Unity's Smooth Follow script.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SmoothFollowAction : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The game object to control. E.g. The camera.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The GameObject to follow.")]
		public FsmGameObject targetObject;

		[Tooltip("The distance in the x-z plane to the target.")]
		[RequiredField]
		public FsmFloat distance;

		[Tooltip("The height we want the camera to be above the target")]
		[RequiredField]
		public FsmFloat height;

		[Tooltip("How much to dampen height movement.")]
		[RequiredField]
		public FsmFloat heightDamping;

		[RequiredField]
		[Tooltip("How much to dampen rotation changes.")]
		public FsmFloat rotationDamping;

		private GameObject cachedObect;

		private Transform myTransform;

		private Transform targetTransform;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			distance = 10f;
			height = 5f;
			heightDamping = 2f;
			rotationDamping = 3f;
		}

		public override void OnLateUpdate()
		{
			if (targetObject.Value == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (cachedObect != ownerDefaultTarget)
				{
					cachedObect = ownerDefaultTarget;
					myTransform = ownerDefaultTarget.transform;
					targetTransform = targetObject.Value.transform;
				}
				float y = targetTransform.eulerAngles.y;
				float to = targetTransform.position.y + height.Value;
				float y2 = myTransform.eulerAngles.y;
				float y3 = myTransform.position.y;
				y2 = Mathf.LerpAngle(y2, y, rotationDamping.Value * Time.deltaTime);
				y3 = Mathf.Lerp(y3, to, heightDamping.Value * Time.deltaTime);
				Quaternion quaternion = Quaternion.Euler(0f, y2, 0f);
				myTransform.position = targetTransform.position;
				myTransform.position -= quaternion * Vector3.forward * distance.Value;
				myTransform.position = new Vector3(myTransform.position.x, y3, myTransform.position.z);
				myTransform.LookAt(targetTransform);
			}
		}
	}
}
