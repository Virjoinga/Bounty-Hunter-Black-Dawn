using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Play an animation on a subset of the hierarchy. E.g., A waving animation on the upper body.")]
	public class AddMixingTransform : FsmStateAction
	{
		[Tooltip("The GameObject playing the animation.")]
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The name of the animation to mix. NOTE: The animation should already be added to the Animation Component on the GameObject.")]
		public FsmString animationName;

		[RequiredField]
		[Tooltip("The mixing transform. E.g., root/upper_body/left_shoulder")]
		public FsmString transform;

		[Tooltip("If recursive is true all children of the mix transform will also be animated.")]
		public FsmBool recursive;

		public override void Reset()
		{
			gameObject = null;
			animationName = string.Empty;
			transform = string.Empty;
			recursive = true;
		}

		public override void OnEnter()
		{
			DoAddMixingTransform();
			Finish();
		}

		private void DoAddMixingTransform()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && !(ownerDefaultTarget.GetComponent<Animation>() == null))
			{
				AnimationState animationState = ownerDefaultTarget.GetComponent<Animation>()[animationName.Value];
				if (!(animationState == null))
				{
					Transform mix = ownerDefaultTarget.transform.Find(transform.Value);
					animationState.AddMixingTransform(mix, recursive.Value);
				}
			}
		}
	}
}
