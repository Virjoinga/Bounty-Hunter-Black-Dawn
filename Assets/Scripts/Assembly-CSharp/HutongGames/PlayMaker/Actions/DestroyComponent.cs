using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Destroys a Component of an Object.")]
	public class DestroyComponent : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString component;

		private Component aComponent;

		public override void Reset()
		{
			aComponent = null;
			gameObject = null;
			component = null;
		}

		public override void OnEnter()
		{
			DoDestroyComponent((gameObject.OwnerOption != 0) ? gameObject.GameObject.Value : base.Owner);
			Finish();
		}

		private void DoDestroyComponent(GameObject go)
		{
			aComponent = go.GetComponent(component.Value);
			if (aComponent == null)
			{
				LogError("No such component: " + component.Value);
			}
			else
			{
				Object.Destroy(aComponent);
			}
		}
	}
}
