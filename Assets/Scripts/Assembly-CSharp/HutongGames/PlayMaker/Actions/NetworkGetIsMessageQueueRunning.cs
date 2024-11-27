using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get if network messages are enabled or disabled.\n\nIf disabled no RPC call execution or network view synchronization takes place")]
	[ActionCategory(ActionCategory.Network)]
	public class NetworkGetIsMessageQueueRunning : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Is Message Queue Running. If this is disabled no RPC call execution or network view synchronization takes place")]
		public FsmBool result;

		public override void Reset()
		{
			result = null;
		}

		public override void OnEnter()
		{
			result.Value = Network.isMessageQueueRunning;
			Finish();
		}
	}
}
