using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Color of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightColor : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmColor lightColor;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetLightColor();
		}

		private void DoSetLightColor()
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
					light.color = lightColor.Value;
				}
			}
		}
	}
}
