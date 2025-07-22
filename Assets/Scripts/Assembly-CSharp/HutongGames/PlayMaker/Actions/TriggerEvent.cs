using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Detect collisions with objects that have RigidBody components. \nNOTE: The system events, TRIGGER ENTER, TRIGGER STAY, and TRIGGER EXIT are sent when any object collides with the trigger. Use this action to filter collisions by Tag.")]
	[ActionCategory(ActionCategory.Physics)]
	public class TriggerEvent : FsmStateAction
	{
		public TriggerType trigger;

		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;

		public override void Reset()
		{
			trigger = TriggerType.OnTriggerEnter;
			collideTag = "Untagged";
			sendEvent = null;
			storeCollider = null;
		}

		public override void Awake()
		{
			switch (trigger)
			{
			case TriggerType.OnTriggerEnter:
				base.Fsm.HandleTriggerEnter = true;
				break;
			case TriggerType.OnTriggerStay:
				base.Fsm.HandleTriggerStay = true;
				break;
			case TriggerType.OnTriggerExit:
				base.Fsm.HandleTriggerExit = true;
				break;
			}
		}

		private void StoreCollisionInfo(Collider collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
		}

		public override void DoTriggerEnter(Collider other)
		{
			if (trigger == TriggerType.OnTriggerEnter && other.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(other);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoTriggerStay(Collider other)
		{
			if (trigger == TriggerType.OnTriggerStay && other.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(other);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoTriggerExit(Collider other)
		{
			if (trigger == TriggerType.OnTriggerExit && other.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(other);
				base.Fsm.Event(sendEvent);
			}
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}
	}
}
