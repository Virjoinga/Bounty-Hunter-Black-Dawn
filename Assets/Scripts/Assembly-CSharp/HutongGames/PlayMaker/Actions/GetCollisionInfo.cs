namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last collision event and store in variables.")]
	public class GetCollisionInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		[UIHint(UIHint.Variable)]
		public FsmVector3 relativeVelocity;

		[UIHint(UIHint.Variable)]
		public FsmFloat relativeSpeed;

		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;

		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;

		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		[UIHint(UIHint.Variable)]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			relativeVelocity = null;
			relativeSpeed = null;
			contactPoint = null;
			contactNormal = null;
			physicsMaterialName = null;
		}

		private void StoreCollisionInfo()
		{
			if (base.Fsm.CollisionInfo != null)
			{
				gameObjectHit.Value = base.Fsm.CollisionInfo.gameObject;
				relativeSpeed.Value = base.Fsm.CollisionInfo.relativeVelocity.magnitude;
				relativeVelocity.Value = base.Fsm.CollisionInfo.relativeVelocity;
				physicsMaterialName.Value = base.Fsm.CollisionInfo.collider.material.name;
				if (base.Fsm.CollisionInfo.contacts != null && base.Fsm.CollisionInfo.contacts.Length > 0)
				{
					contactPoint.Value = base.Fsm.CollisionInfo.contacts[0].point;
					contactNormal.Value = base.Fsm.CollisionInfo.contacts[0].normal;
				}
			}
		}

		public override void OnEnter()
		{
			StoreCollisionInfo();
			Finish();
		}
	}
}
