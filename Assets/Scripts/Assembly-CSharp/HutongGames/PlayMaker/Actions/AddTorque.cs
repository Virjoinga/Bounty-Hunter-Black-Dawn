using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Adds torque (rotational force) to a Game Object.")]
	public class AddTorque : FsmStateAction
	{
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("A Vector3 torque. Optionally override any axis with the X, Y, Z parameters.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		[Tooltip("To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		public Space space;

		public ForceMode forceMode;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			x = new FsmFloat
			{
				UseVariable = true
			};
			y = new FsmFloat
			{
				UseVariable = true
			};
			z = new FsmFloat
			{
				UseVariable = true
			};
			space = Space.World;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAddTorque();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoAddTorque();
		}

		private void DoAddTorque()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget.GetComponent<Rigidbody>() == null)
			{
				LogWarning("Missing rigid body: " + ownerDefaultTarget.name);
				return;
			}
			Vector3 torque = ((!vector.IsNone) ? vector.Value : new Vector3(x.Value, y.Value, z.Value));
			if (!x.IsNone)
			{
				torque.x = x.Value;
			}
			if (!y.IsNone)
			{
				torque.y = y.Value;
			}
			if (!z.IsNone)
			{
				torque.z = z.Value;
			}
			if (space == Space.World)
			{
				ownerDefaultTarget.GetComponent<Rigidbody>().AddTorque(torque, forceMode);
			}
			else
			{
				ownerDefaultTarget.GetComponent<Rigidbody>().AddRelativeTorque(torque, forceMode);
			}
		}
	}
}
