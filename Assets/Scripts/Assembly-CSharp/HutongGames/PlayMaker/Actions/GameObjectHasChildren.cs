using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object has children.")]
	public class GameObjectHasChildren : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event to send if the Game Object has children.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the Game Object does not have children.")]
		public FsmEvent falseEvent;

		[Tooltip("Store true/false in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoHasChildren();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoHasChildren();
		}

		private void DoHasChildren()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				bool flag = ownerDefaultTarget.transform.childCount > 0;
				storeResult.Value = flag;
				base.Fsm.Event((!flag) ? falseEvent : trueEvent);
			}
		}
	}
}
