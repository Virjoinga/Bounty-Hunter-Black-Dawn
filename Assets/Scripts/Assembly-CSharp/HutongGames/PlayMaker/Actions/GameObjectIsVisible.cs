using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Game Object is visible.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsVisible : FsmStateAction
	{
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

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
			DoIsVisible();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsVisible();
		}

		private void DoIsVisible()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && !(ownerDefaultTarget.GetComponent<Renderer>() == null))
			{
				bool isVisible = ownerDefaultTarget.GetComponent<Renderer>().isVisible;
				storeResult.Value = isVisible;
				base.Fsm.Event((!isVisible) ? falseEvent : trueEvent);
			}
		}
	}
}
