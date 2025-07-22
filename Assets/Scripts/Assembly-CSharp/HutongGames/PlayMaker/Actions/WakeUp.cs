using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Forces a Game Object's Rigid Body to wake up.")]
	public class WakeUp : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoWakeUp();
			Finish();
		}

		private void DoWakeUp()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			if (!(gameObject == null) && !(gameObject.GetComponent<Rigidbody>() == null))
			{
				gameObject.GetComponent<Rigidbody>().WakeUp();
			}
		}
	}
}
