using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Fade from a fullscreen Color. NOTE: Uses OnGUI so requires a PlayMakerGUI component in the scene.")]
	[ActionCategory(ActionCategory.Camera)]
	public class CameraFadeIn : FsmStateAction
	{
		[RequiredField]
		public FsmColor color;

		[RequiredField]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat time;

		public FsmEvent finishEvent;

		[Tooltip("Ignore TimeScale. Useful if the game is paused.")]
		public bool realTime;

		private float startTime;

		private float currentTime;

		private Color colorLerp;

		public override void Reset()
		{
			color = Color.black;
			time = 1f;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			colorLerp = color.Value;
		}

		public override void OnUpdate()
		{
			if (realTime)
			{
				currentTime = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				currentTime += Time.deltaTime;
			}
			colorLerp = Color.Lerp(color.Value, Color.clear, currentTime / time.Value);
			if (currentTime > time.Value)
			{
				if (finishEvent != null)
				{
					base.Fsm.Event(finishEvent);
				}
				Finish();
			}
		}

		public override void OnGUI()
		{
			Color color = GUI.color;
			GUI.color = colorLerp;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = color;
		}
	}
}
