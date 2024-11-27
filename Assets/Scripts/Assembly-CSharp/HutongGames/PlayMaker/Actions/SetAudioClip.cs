using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class SetAudioClip : FsmStateAction
	{
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		[Tooltip("The GameObject with the AudioSource component.")]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(AudioClip))]
		[Tooltip("The AudioClip to set.")]
		public FsmObject audioClip;

		public override void Reset()
		{
			gameObject = null;
			audioClip = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource audio = ownerDefaultTarget.GetComponent<AudioSource>();
				if (audio != null)
				{
					audio.clip = audioClip.Value as AudioClip;
				}
			}
			Finish();
		}
	}
}
