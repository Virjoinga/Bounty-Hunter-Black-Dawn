using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a force to a Game Object. Use Vector3 variable and/or Float variables for each axis.")]
	[ActionCategory(ActionCategory.Physics)]
	public class AddForce : FsmStateAction
	{
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Optionally apply the force at a position on the object. This will also add some torque. The position is often returned from MousePick or GetCollisionInfo actions.")]
		public FsmVector3 atPosition;

		[UIHint(UIHint.Variable)]
		[Tooltip("A Vector3 force to add. Optionally override any axis with the X, Y, Z parameters.")]
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
			atPosition = new FsmVector3
			{
				UseVariable = true
			};
			vector = null;
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
			DoAddForce();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoAddForce();
		}

		private void DoAddForce()
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
			Vector3 force = ((!vector.IsNone) ? vector.Value : new Vector3(x.Value, y.Value, z.Value));
			if (!x.IsNone)
			{
				force.x = x.Value;
			}
			if (!y.IsNone)
			{
				force.y = y.Value;
			}
			if (!z.IsNone)
			{
				force.z = z.Value;
			}
			if (space == Space.World)
			{
				if (!atPosition.IsNone)
				{
					ownerDefaultTarget.GetComponent<Rigidbody>().AddForceAtPosition(force, atPosition.Value);
				}
				else
				{
					ownerDefaultTarget.GetComponent<Rigidbody>().AddForce(force);
				}
			}
			else
			{
				ownerDefaultTarget.GetComponent<Rigidbody>().AddRelativeForce(force);
			}
		}
	}
}
