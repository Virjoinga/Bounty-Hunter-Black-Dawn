using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets Field of View used by the Camera.")]
	public class SetCameraFOV : FsmStateAction
	{
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmFloat fieldOfView;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			fieldOfView = 50f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetCameraFOV();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetCameraFOV();
		}

		private void DoSetCameraFOV()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != 0) ? this.gameObject.GameObject.Value : base.Owner);
			if (!(gameObject == null))
			{
				Camera camera = gameObject.GetComponent<Camera>();
				if (camera == null)
				{
					LogError("Missing Camera Component!");
				}
				else
				{
					camera.fieldOfView = fieldOfView.Value;
				}
			}
		}
	}
}
