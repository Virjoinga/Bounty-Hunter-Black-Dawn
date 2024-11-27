using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clear the host list which was received by MasterServer Request Host List")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerClearHostList : FsmStateAction
	{
		public override void OnEnter()
		{
			MasterServer.ClearHostList();
			Finish();
		}
	}
}
