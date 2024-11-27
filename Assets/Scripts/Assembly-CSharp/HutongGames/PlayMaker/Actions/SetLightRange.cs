using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the Range of a Light.")]
	public class SetLightRange : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		public FsmFloat lightRange;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightRange = 20f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightRange();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetLightRange();
		}

		private void DoSetLightRange()
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
					light.range = lightRange.Value;
				}
			}
		}
	}
}
