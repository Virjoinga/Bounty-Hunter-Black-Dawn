using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds the Child of a Game Object by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
	public class FindChild : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The name of the child. Note, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger")]
		public FsmString childName;

		[RequiredField]
		[Tooltip("Store the child in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			childName = string.Empty;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoFindChild();
			Finish();
		}

		private void DoFindChild()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Transform transform = ownerDefaultTarget.transform.Find(childName.Value);
				storeResult.Value = ((!(transform != null)) ? null : transform.gameObject);
			}
		}
	}
}
