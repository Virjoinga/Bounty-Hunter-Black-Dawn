using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the number of children that a Game Object has.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChildCount : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetChildCount();
			Finish();
		}

		private void DoGetChildCount()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				storeResult.Value = ownerDefaultTarget.transform.childCount;
			}
		}
	}
}
