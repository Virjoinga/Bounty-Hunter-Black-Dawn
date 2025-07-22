using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Plays the Audio Clip set with Set Audio Clip or in the Audio Source inspector on a Game Object. Optionally plays a one shot Audio Clip.")]
	[ActionCategory(ActionCategory.Audio)]
	public class AudioPlay : FsmStateAction
	{
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		[Tooltip("The GameObject with the AudioSource component.")]
		public FsmOwnerDefault gameObject;

		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume;

		[Tooltip("Optionally play a 'one shot' AudioClip.")]
		[ObjectType(typeof(AudioClip))]
		public FsmObject oneShotClip;

		[Tooltip("Event to send when the AudioClip finished playing.")]
		public FsmEvent finishedEvent;

		private AudioSource audio;

		public override void Reset()
		{
			gameObject = null;
			volume = 1f;
			oneShotClip = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				audio = ownerDefaultTarget.GetComponent<AudioSource>();
				if (audio != null)
				{
					AudioClip audioClip = oneShotClip.Value as AudioClip;
					if (audioClip == null)
					{
						audio.Play();
						if (!volume.IsNone)
						{
							audio.volume = volume.Value;
						}
					}
					else if (!volume.IsNone)
					{
						audio.PlayOneShot(audioClip, volume.Value);
					}
					else
					{
						audio.PlayOneShot(audioClip);
					}
					return;
				}
			}
			Finish();
		}

		public override void OnUpdate()
		{
			if (audio == null)
			{
				Finish();
			}
			else if (!audio.isPlaying)
			{
				base.Fsm.Event(finishedEvent);
				Finish();
			}
		}
	}
}
