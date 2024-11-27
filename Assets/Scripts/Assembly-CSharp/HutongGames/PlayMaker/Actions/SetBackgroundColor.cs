using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Sets the Background Color used by the Camera.")]
	public class SetBackgroundColor : FsmStateAction
	{
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmColor backgroundColor;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			backgroundColor = Color.black;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetBackgroundColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetBackgroundColor();
		}

		private void DoSetBackgroundColor()
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
					camera.backgroundColor = backgroundColor.Value;
				}
			}
		}
	}
}
