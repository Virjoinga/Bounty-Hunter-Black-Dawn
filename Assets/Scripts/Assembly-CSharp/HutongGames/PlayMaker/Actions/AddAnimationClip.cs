using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a named Animation Clip to a Game Object. Optionally trims the Animation.")]
	[ActionCategory(ActionCategory.Animation)]
	public class AddAnimationClip : FsmStateAction
	{
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(AnimationClip))]
		[RequiredField]
		[Tooltip("NOTE: Make sure the clip is compatible with the object's hierarchy.")]
		public FsmObject animationClip;

		[Tooltip("Name the animation. Used by other actions to reference this animation.")]
		[RequiredField]
		public FsmString animationName;

		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt firstFrame;

		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt lastFrame;

		[Tooltip("Add an extra looping frame that matches the first frame.")]
		public FsmBool addLoopFrame;

		public override void Reset()
		{
			gameObject = null;
			animationClip = null;
			animationName = string.Empty;
			firstFrame = 0;
			lastFrame = 0;
			addLoopFrame = false;
		}

		public override void OnEnter()
		{
			DoAddAnimationClip();
			Finish();
		}

		private void DoAddAnimationClip()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null || ownerDefaultTarget.GetComponent<Animation>() == null)
			{
				return;
			}
			AnimationClip animationClip = this.animationClip.Value as AnimationClip;
			if (!(animationClip == null))
			{
				if (firstFrame.Value == 0 && lastFrame.Value == 0)
				{
					ownerDefaultTarget.GetComponent<Animation>().AddClip(animationClip, animationName.Value);
				}
				else
				{
					ownerDefaultTarget.GetComponent<Animation>().AddClip(animationClip, animationName.Value, firstFrame.Value, lastFrame.Value, addLoopFrame.Value);
				}
			}
		}
	}
}
