namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Gets info on the last Character Controller collision and store in variables.")]
	public class GetControllerHitInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;

		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;

		[UIHint(UIHint.Variable)]
		public FsmVector3 moveDirection;

		[UIHint(UIHint.Variable)]
		public FsmFloat moveLength;

		[Tooltip("Useful for triggering different effects. Audio, particles...")]
		[UIHint(UIHint.Variable)]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			contactPoint = null;
			contactNormal = null;
			moveDirection = null;
			moveLength = null;
			physicsMaterialName = null;
		}

		private void StoreTriggerInfo()
		{
			if (base.Fsm.ControllerCollider != null)
			{
				gameObjectHit.Value = base.Fsm.ControllerCollider.gameObject;
				contactPoint.Value = base.Fsm.ControllerCollider.point;
				contactNormal.Value = base.Fsm.ControllerCollider.normal;
				moveDirection.Value = base.Fsm.ControllerCollider.moveDirection;
				moveLength.Value = base.Fsm.ControllerCollider.moveLength;
				physicsMaterialName.Value = base.Fsm.ControllerCollider.collider.material.name;
			}
		}

		public override void OnEnter()
		{
			StoreTriggerInfo();
			Finish();
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}
	}
}
