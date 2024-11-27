using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
	[ActionCategory(ActionCategory.Animation)]
	public class StopAnimation : FsmStateAction
	{
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Leave empty to stop all playing animations.")]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoStopAnimation();
			Finish();
		}

		private void DoStopAnimation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (ownerDefaultTarget.GetComponent<Animation>() == null)
				{
					LogWarning("Missing animation component: " + ownerDefaultTarget.name);
				}
				else
				{
					ownerDefaultTarget.GetComponent<Animation>().Stop(animName.Value);
				}
			}
		}
	}
}
