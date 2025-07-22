using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random State Event after an optional delay. Use this to transition to a random state from the current state.")]
	public class RandomEvent : FsmStateAction
	{
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			delay = null;
		}

		public override void OnEnter()
		{
			if (base.State.Transitions.Length != 0)
			{
				if (delay.Value < 0.001f)
				{
					base.Fsm.Event(GetRandomEvent());
					Finish();
				}
				else
				{
					delayedEvent = base.Fsm.DelayedEvent(GetRandomEvent(), delay.Value);
				}
			}
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}

		private FsmEvent GetRandomEvent()
		{
			int num = Random.Range(0, base.State.Transitions.Length);
			return base.State.Transitions[num].FsmEvent;
		}
	}
}
