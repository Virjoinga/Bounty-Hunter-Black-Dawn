namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event after an optional delay. NOTE: To send events between FSMs they must be marked as Global in the Events Browser.")]
	public class SendEvent : FsmStateAction
	{
		[Tooltip("Where to send the event.")]
		public FsmEventTarget eventTarget;

		[Tooltip("The event to send. NOTE: Events must be marked Global to send between FSMs.")]
		[RequiredField]
		public FsmEvent sendEvent;

		[Tooltip("Optional delay in seconds.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		[Tooltip("Repeat every frame. Rarely needed.")]
		public bool everyFrame;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			eventTarget = null;
			sendEvent = null;
			delay = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			if (delay.Value < 0.001f)
			{
				base.Fsm.Event(eventTarget, sendEvent);
				Finish();
			}
			else
			{
				delayedEvent = base.Fsm.DelayedEvent(eventTarget, sendEvent, delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (!everyFrame && DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}
	}
}
