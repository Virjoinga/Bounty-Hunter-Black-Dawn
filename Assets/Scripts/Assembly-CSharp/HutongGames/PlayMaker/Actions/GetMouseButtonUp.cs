using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the specified Mouse Button is released. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonUp : FsmStateAction
	{
		[RequiredField]
		public MouseButton button;

		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			button = MouseButton.Left;
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool mouseButtonUp = Input.GetMouseButtonUp((int)button);
			if (mouseButtonUp)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = mouseButtonUp;
		}
	}
}
