using System;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmEventTarget
	{
		public enum EventTarget
		{
			Self = 0,
			GameObject = 1,
			GameObjectFSM = 2,
			FSMComponent = 3,
			BroadcastAll = 4
		}

		public EventTarget target;

		public FsmBool excludeSelf;

		public FsmOwnerDefault gameObject;

		public FsmString fsmName;

		public FsmBool sendToChildren;

		public PlayMakerFSM fsmComponent;

		public FsmEventTarget()
		{
			ResetParameters();
		}

		public FsmEventTarget(FsmEventTarget source)
		{
			target = source.target;
			excludeSelf = new FsmBool(source.excludeSelf);
			gameObject = new FsmOwnerDefault(source.gameObject);
			fsmName = new FsmString(source.fsmName);
			sendToChildren = new FsmBool(source.sendToChildren);
			fsmComponent = source.fsmComponent;
		}

		public void ResetParameters()
		{
			excludeSelf = false;
			gameObject = null;
			fsmName = "";
			sendToChildren = false;
			fsmComponent = null;
		}
	}
}
