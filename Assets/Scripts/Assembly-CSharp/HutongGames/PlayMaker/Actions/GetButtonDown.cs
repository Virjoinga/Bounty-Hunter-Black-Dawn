using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when a Button is pressed.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetButtonDown : FsmStateAction
	{
		[RequiredField]
		public FsmString buttonName;

		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public override void Reset()
		{
			buttonName = "Fire1";
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool buttonDown = Input.GetButtonDown(buttonName.Value);
			if (buttonDown)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = buttonDown;
		}
	}
}
