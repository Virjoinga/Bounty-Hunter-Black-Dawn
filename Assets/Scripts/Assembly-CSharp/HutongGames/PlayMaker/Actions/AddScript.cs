using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	public class AddScript : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object to add the script to.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.ScriptComponent)]
		[RequiredField]
		[Tooltip("The Script to add to the Game Object.")]
		public FsmString script;

		[Tooltip("Remove the script from the Game Object when this State is exited.")]
		public FsmBool removeOnExit;

		private Component addedComponent;

		public override void Reset()
		{
			gameObject = null;
			script = null;
		}

		public override void OnEnter()
		{
			DoAddComponent((gameObject.OwnerOption != 0) ? gameObject.GameObject.Value : base.Owner);
			Finish();
		}

		public override void OnExit()
		{
			if (removeOnExit.Value && addedComponent != null)
			{
				Object.Destroy(addedComponent);
			}
		}

		private void DoAddComponent(GameObject go)
		{
			addedComponent = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/Scripts/Assembly-CSharp/HutongGames/PlayMaker/Actions/AddScript.cs (45,21)", script.Value);
			if (addedComponent == null)
			{
				LogError("Can't add script: " + script.Value);
			}
		}
	}
}
