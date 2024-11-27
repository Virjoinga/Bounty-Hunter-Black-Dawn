using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Applies a force to a Game Object that simulates explosion effects. The explosion force will fall off linearly with distance. Hint: Use the Explosion Action instead to apply an explosion force to all objects in a blast radius.")]
	public class AddExplosionForce : FsmStateAction
	{
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("Hint: this is often the position returned from a GetCollisionInfo action.")]
		public FsmVector3 center;

		[RequiredField]
		public FsmFloat force;

		[RequiredField]
		public FsmFloat radius;

		[Tooltip("Explosions often look better when given an extra artificial upward force.")]
		public FsmFloat upwardsModifier;

		public ForceMode forceMode;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			center = new FsmVector3
			{
				UseVariable = true
			};
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAddExplosionForce();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoAddExplosionForce();
		}

		private void DoAddExplosionForce()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			if (!(gameObject == null) && center != null && !(gameObject.GetComponent<Rigidbody>() == null))
			{
				gameObject.GetComponent<Rigidbody>().AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
			}
		}
	}
}
