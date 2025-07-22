using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Detect collisions with Game Objects that have RigidBody components.\nNOTE: The system events, COLLISION ENTER, COLLISION STAY, and COLLISION EXIT are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	[ActionCategory(ActionCategory.Physics)]
	public class CollisionEvent : FsmStateAction
	{
		public CollisionType collision;

		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeForce;

		public override void Reset()
		{
			collision = CollisionType.OnCollisionEnter;
			collideTag = "Untagged";
			sendEvent = null;
			storeCollider = null;
			storeForce = null;
		}

		public override void Awake()
		{
			switch (collision)
			{
			case CollisionType.OnCollisionEnter:
				base.Fsm.HandleCollisionEnter = true;
				break;
			case CollisionType.OnCollisionStay:
				base.Fsm.HandleCollisionStay = true;
				break;
			case CollisionType.OnCollisionExit:
				base.Fsm.HandleCollisionExit = true;
				break;
			}
		}

		private void StoreCollisionInfo(Collision collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
			storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		public override void DoCollisionEnter(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionEnter && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionStay(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionStay && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionExit(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionExit && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
		{
			if (collision == CollisionType.OnControllerColliderHit && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				if (storeCollider != null)
				{
					storeCollider.Value = collisionInfo.gameObject;
				}
				storeForce.Value = 0f;
				base.Fsm.Event(sendEvent);
			}
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}
	}
}
