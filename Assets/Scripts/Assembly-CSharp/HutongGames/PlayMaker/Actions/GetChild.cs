using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Finds the Child of a Game Object by Name and/or Tag. Use this to find attach points etc. NOTE: This action will search recursively through all children and return the first match; To find a specific child use Find Child.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChild : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmString childName;

		[UIHint(UIHint.Tag)]
		public FsmString withTag;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			childName = string.Empty;
			withTag = "Untagged";
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = DoGetChildByName(base.Fsm.GetOwnerDefaultTarget(gameObject), childName.Value, withTag.Value);
			Finish();
		}

		private static GameObject DoGetChildByName(GameObject root, string name, string tag)
		{
			if (root == null)
			{
				return null;
			}
			foreach (Transform item in root.transform)
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (item.name == name)
					{
						if (string.IsNullOrEmpty(tag))
						{
							return item.gameObject;
						}
						if (item.tag.Equals(tag))
						{
							return item.gameObject;
						}
					}
				}
				else if (!string.IsNullOrEmpty(tag) && item.tag == tag)
				{
					return item.gameObject;
				}
				GameObject gameObject = DoGetChildByName(item.gameObject, name, tag);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(childName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
				return "Specify Child Name, Tag, or both.";
			}
			return null;
		}
	}
}
