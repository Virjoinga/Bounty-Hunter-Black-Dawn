using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set Spot, Directional, or Point Light type.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightType : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		public LightType lightType;

		public override void Reset()
		{
			gameObject = null;
			lightType = LightType.Point;
		}

		public override void OnEnter()
		{
			DoSetLightType();
			Finish();
		}

		private void DoSetLightType()
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
					light.type = lightType;
				}
			}
		}
	}
}
