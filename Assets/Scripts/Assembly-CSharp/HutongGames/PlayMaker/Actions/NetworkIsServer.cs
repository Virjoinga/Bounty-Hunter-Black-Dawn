using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if your peer type is server.")]
	public class NetworkIsServer : FsmStateAction
	{
		[Tooltip("True if running as server.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isServer;

		[Tooltip("Event to send if running as server.")]
		public FsmEvent isServerEvent;

		[Tooltip("Event to send if not running as server.")]
		public FsmEvent isNotServerEvent;

		public override void Reset()
		{
			isServer = null;
		}

		public override void OnEnter()
		{
			DoCheckIsServer();
			Finish();
		}

		private void DoCheckIsServer()
		{
			isServer.Value = Network.isServer;
			if (Network.isServer && isServerEvent != null)
			{
				base.Fsm.Event(isServerEvent);
			}
			else if (!Network.isServer && isNotServerEvent != null)
			{
				base.Fsm.Event(isNotServerEvent);
			}
		}
	}
}