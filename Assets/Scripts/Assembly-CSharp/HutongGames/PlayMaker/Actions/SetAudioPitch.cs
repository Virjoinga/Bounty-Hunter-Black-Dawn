using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Sets the Pitch of the Audio Clip played by the AudioSource component on a Game Object.")]
	public class SetAudioPitch : FsmStateAction
	{
		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmFloat pitch;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			pitch = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetAudioPitch();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetAudioPitch();
		}

		private void DoSetAudioPitch()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource audio = ownerDefaultTarget.GetComponent<AudioSource>();
				if (audio != null && !pitch.IsNone)
				{
					audio.pitch = pitch.Value;
				}
			}
		}
	}
}
