using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Get Vector3 Length.")]
	public class SetHandsPosition : FsmStateAction
	{
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmFloat HandsDist;

		public FsmFloat HandsAngle;

		[RequiredField]
		public FsmVector3 SelectedPoint;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject Hand01Obj;

		public FsmGameObject Hand02Obj;

		public override void Reset()
		{
			HandsDist = new FsmFloat
			{
				UseVariable = true
			};
			HandsAngle = new FsmFloat
			{
				UseVariable = true
			};
			HandsDist.Value = 1f;
			gameObject = null;
			SelectedPoint = null;
			Hand01Obj = null;
			Hand02Obj = null;
		}

		public override void OnEnter()
		{
			DoCalc();
			Finish();
		}

		private void DoCalc()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null || SelectedPoint == null)
			{
				return;
			}
			float num = HandsDist.Value;
			if (Input.GetMouseButton(0))
			{
				num /= 2f;
			}
			num *= 0.25f + 0.75f * Mathf.Abs(Mathf.Cos(HandsAngle.Value * (float)Math.PI / 18f));
			Transform transform = ownerDefaultTarget.transform;
			Vector3 position = transform.position;
			Vector3 position2 = transform.InverseTransformPoint(SelectedPoint.Value);
			position2.x = (position2.z = 0f);
			position = transform.TransformPoint(position2);
			Vector3 eulerAngles = transform.eulerAngles;
			if (Hand01Obj != null)
			{
				Vector3 direction = default(Vector3);
				direction.x = num;
				direction.y = 0f;
				direction.z = 0f;
				transform.Rotate(0f, 0f, HandsAngle.Value * 10f);
				direction = transform.TransformDirection(direction);
				transform.eulerAngles = eulerAngles;
				direction += SelectedPoint.Value;
				direction -= position;
				direction.Normalize();
				Vector3 origin = direction + position;
				direction *= -1f;
				Ray ray = new Ray(origin, direction);
				Collider collider = ownerDefaultTarget.transform.GetComponent<Collider>();
				RaycastHit hitInfo;
				if (collider.Raycast(ray, out hitInfo, 100f))
				{
					Hand01Obj.Value.transform.position = hitInfo.point;
					Hand01Obj.Value.transform.forward = hitInfo.normal;
				}
			}
			if (Hand02Obj != null)
			{
				Vector3 direction2 = default(Vector3);
				direction2.x = 0f - num;
				direction2.y = 0f;
				direction2.z = 0f;
				transform.Rotate(0f, 0f, HandsAngle.Value * 10f);
				direction2 = transform.TransformDirection(direction2);
				transform.eulerAngles = eulerAngles;
				direction2 += SelectedPoint.Value;
				direction2 -= position;
				direction2.Normalize();
				Vector3 origin2 = direction2 + position;
				direction2 *= -1f;
				Ray ray2 = new Ray(origin2, direction2);
				Collider collider2 = ownerDefaultTarget.transform.GetComponent<Collider>();
				RaycastHit hitInfo2;
				if (collider2.Raycast(ray2, out hitInfo2, 100f))
				{
					Hand02Obj.Value.transform.position = hitInfo2.point;
					Hand02Obj.Value.transform.forward = hitInfo2.normal;
				}
			}
		}
	}
}
