using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a Game Object is a Child of another Game Object.")]
	public class GameObjectIsChildOf : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmGameObject isChildOf;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;

		public override void Reset()
		{
			gameObject = null;
			isChildOf = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoIsChildOf(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoIsChildOf(GameObject go)
		{
			if (!(go == null) && isChildOf != null)
			{
				bool flag = go.transform.IsChildOf(isChildOf.Value.transform);
				storeResult.Value = flag;
				base.Fsm.Event((!flag) ? falseEvent : trueEvent);
			}
		}
	}
}
