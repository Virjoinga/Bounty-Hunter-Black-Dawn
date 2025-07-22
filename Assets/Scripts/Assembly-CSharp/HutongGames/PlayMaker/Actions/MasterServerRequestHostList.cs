using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Request a host list from the master server.\n\nUse MasterServer Get Host Data to get info on each host in the host list.")]
	[ActionCategory(ActionCategory.Network)]
	public class MasterServerRequestHostList : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The unique game type name.")]
		public FsmString gameTypeName;

		[Tooltip("Event sent when the host list has arrived. NOTE: The action will not Finish until the host list arrives.")]
		public FsmEvent HostListArrivedEvent;

		public override void Reset()
		{
			gameTypeName = null;
			HostListArrivedEvent = null;
		}

		public override void OnEnter()
		{
			DoMasterServerRequestHost();
		}

		public override void OnUpdate()
		{
			WatchServerRequestHost();
		}

		private void DoMasterServerRequestHost()
		{
			MasterServer.ClearHostList();
			MasterServer.RequestHostList(gameTypeName.Value);
		}

		private void WatchServerRequestHost()
		{
			if (MasterServer.PollHostList().Length != 0)
			{
				base.Fsm.Event(HostListArrivedEvent);
				Finish();
			}
		}
	}
}
