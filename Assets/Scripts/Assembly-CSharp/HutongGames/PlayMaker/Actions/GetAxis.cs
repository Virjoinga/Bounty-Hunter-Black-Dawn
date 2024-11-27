using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the value of the specified Input Axis and stores it in a Float Variable. See Unity Input Manager doc.")]
	public class GetAxis : FsmStateAction
	{
		[RequiredField]
		public FsmString axisName;

		public FsmFloat multiplier;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat store;

		public bool everyFrame;

		public override void Reset()
		{
			axisName = string.Empty;
			multiplier = 1f;
			store = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetAxis();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetAxis();
		}

		private void DoGetAxis()
		{
			float num = Input.GetAxis(axisName.Value);
			if (!multiplier.IsNone)
			{
				num *= multiplier.Value;
			}
			store.Value = num;
		}
	}
}
