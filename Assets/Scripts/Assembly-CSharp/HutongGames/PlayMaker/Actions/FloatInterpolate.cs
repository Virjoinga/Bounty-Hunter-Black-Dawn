using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Interpolates between 2 Float values over a specified Time.")]
	public class FloatInterpolate : FsmStateAction
	{
		public InterpolationType mode;

		[RequiredField]
		public FsmFloat fromFloat;

		[RequiredField]
		public FsmFloat toFloat;

		[RequiredField]
		public FsmFloat time;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		public FsmEvent finishEvent;

		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		private float startTime;

		private float currentTime;

		public override void Reset()
		{
			mode = InterpolationType.Linear;
			fromFloat = null;
			toFloat = null;
			time = 1f;
			storeResult = null;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			if (storeResult == null)
			{
				Finish();
			}
			else
			{
				storeResult.Value = fromFloat.Value;
			}
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
			float num = currentTime / time.Value;
			switch (mode)
			{
			case InterpolationType.Linear:
				storeResult.Value = Mathf.Lerp(fromFloat.Value, toFloat.Value, num);
				break;
			case InterpolationType.EaseInOut:
				storeResult.Value = Mathf.SmoothStep(fromFloat.Value, toFloat.Value, num);
				break;
			}
			if (num > 1f)
			{
				if (finishEvent != null)
				{
					base.Fsm.Event(finishEvent);
				}
				Finish();
			}
		}
	}
}
