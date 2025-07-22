using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Intensity of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightIntensity : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		public FsmFloat lightIntensity;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightIntensity = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightIntensity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetLightIntensity();
		}

		private void DoSetLightIntensity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Light light = ownerDefaultTarget.GetComponent<Light>();
				if (light == null)
				{
					LogError("Missing Light Component!");
				}
				else
				{
					light.intensity = lightIntensity.Value;
				}
			}
		}
	}
}
