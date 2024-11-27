using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the specified Mouse Button is pressed. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonDown : FsmStateAction
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
			bool mouseButtonDown = Input.GetMouseButtonDown((int)button);
			if (mouseButtonDown)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = mouseButtonDown;
		}
	}
}
