using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Test if your peer type is client.")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkIsClient : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("True if running as client.")]
		public FsmBool isClient;

		[Tooltip("Event to send if running as client.")]
		public FsmEvent isClientEvent;

		[Tooltip("Event to send if not running as client.")]
		public FsmEvent isNotClientEvent;

		public override void Reset()
		{
			isClient = null;
		}

		public override void OnEnter()
		{
			DoCheckIsClient();
			Finish();
		}

		private void DoCheckIsClient()
		{
			isClient.Value = Network.isClient;
			if (Network.isClient && isClientEvent != null)
			{
				base.Fsm.Event(isClientEvent);
			}
			else if (!Network.isClient && isNotClientEvent != null)
			{
				base.Fsm.Event(isNotClientEvent);
			}
		}
	}
}
