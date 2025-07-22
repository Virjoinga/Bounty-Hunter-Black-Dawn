using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Velocity of a Game Object. To leave any axis unchanged, set variable to 'None'. NOTE: Game object must have a rigidbody.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetVelocity : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat x;

		public FsmFloat y;

		public FsmFloat z;

		public Space space;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
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
			space = Space.Self;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoSetVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && !(ownerDefaultTarget.GetComponent<Rigidbody>() == null))
			{
				Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : ((space != 0) ? ownerDefaultTarget.transform.InverseTransformDirection(ownerDefaultTarget.GetComponent<Rigidbody>().velocity) : ownerDefaultTarget.GetComponent<Rigidbody>().velocity));
				if (!x.IsNone)
				{
					vector.x = x.Value;
				}
				if (!y.IsNone)
				{
					vector.y = y.Value;
				}
				if (!z.IsNone)
				{
					vector.z = z.Value;
				}
				ownerDefaultTarget.GetComponent<Rigidbody>().velocity = ((space != 0) ? ownerDefaultTarget.transform.TransformDirection(vector) : vector);
			}
		}
	}
}
