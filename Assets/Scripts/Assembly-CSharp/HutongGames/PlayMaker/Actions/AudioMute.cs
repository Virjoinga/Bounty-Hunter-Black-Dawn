using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object.")]
	public class AudioMute : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmBool mute;

		public override void Reset()
		{
			gameObject = null;
			mute = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource audio = ownerDefaultTarget.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.mute = mute.Value;
				}
			}
			Finish();
		}
	}
}
