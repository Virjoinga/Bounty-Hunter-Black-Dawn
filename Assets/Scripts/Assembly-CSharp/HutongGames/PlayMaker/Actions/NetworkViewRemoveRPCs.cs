using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Remove the RPC function calls accociated with a Game Object.\n\nNOTE: The Game Object must have a NetworkView component attached.")]
	public class NetworkViewRemoveRPCs : FsmStateAction
	{
		[Tooltip("Remove the RPC function calls accociated with this Game Object.\n\nNOTE: The GameObject must have a NetworkView component attached.")]
		[RequiredField]
		[CheckForComponent(typeof(NetworkView))]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoRemoveRPCsFromViewID();
			Finish();
		}

		private void DoRemoveRPCsFromViewID()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && !(ownerDefaultTarget.GetComponent<NetworkView>() == null))
			{
				Network.RemoveRPCs(ownerDefaultTarget.GetComponent<NetworkView>().viewID);
			}
		}
	}
}
