using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Network)]
	[Tooltip("Test if the Network View is controlled by a GameObject.")]
	public class NetworkViewIsMine : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Game Object with the NetworkView attached.")]
		[CheckForComponent(typeof(NetworkView))]
		public FsmOwnerDefault gameObject;

		[Tooltip("True if the network view is controlled by this object.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isMine;

		[Tooltip("Send this event if the network view controlled by this object.")]
		public FsmEvent isMineEvent;

		[Tooltip("Send this event if the network view is NOT controlled by this object.")]
		public FsmEvent isNotMineEvent;

		private NetworkView _networkView;

		private void _getNetworkView()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				_networkView = ownerDefaultTarget.GetComponent<NetworkView>();
			}
		}

		public override void Reset()
		{
			gameObject = null;
			isMine = null;
			isMineEvent = null;
			isNotMineEvent = null;
		}

		public override void OnEnter()
		{
			_getNetworkView();
			checkIsMine();
			Finish();
		}

		private void checkIsMine()
		{
			if (_networkView == null)
			{
				return;
			}
			bool flag = _networkView.isMine;
			isMine.Value = flag;
			if (flag)
			{
				if (isMineEvent != null)
				{
					base.Fsm.Event(isMineEvent);
				}
			}
			else if (isNotMineEvent != null)
			{
				base.Fsm.Event(isNotMineEvent);
			}
		}
	}
}
