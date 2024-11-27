using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Gets the camera tagged MainCamera from the scene")]
	public class GetMainCamera : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeGameObject;

		public override void Reset()
		{
			storeGameObject = null;
		}

		public override void OnEnter()
		{
			storeGameObject.Value = ((!(Camera.main != null)) ? null : Camera.main.gameObject);
			Finish();
		}
	}
}
