using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rewinds the named animation.")]
	[ActionCategory(ActionCategory.Animation)]
	public class RewindAnimation : FsmStateAction
	{
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoRewindAnimation();
			Finish();
		}

		private void DoRewindAnimation()
		{
			if (string.IsNullOrEmpty(animName.Value))
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (ownerDefaultTarget.GetComponent<Animation>() == null)
				{
					LogWarning("Missing animation component: " + ownerDefaultTarget.name);
				}
				else
				{
					ownerDefaultTarget.GetComponent<Animation>().Rewind(animName.Value);
				}
			}
		}
	}
}
