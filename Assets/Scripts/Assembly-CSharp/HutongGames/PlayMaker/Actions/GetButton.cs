using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the pressed state of the specified Button and stores it in a Bool Variable. See Unity Input Manager docs.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetButton : FsmStateAction
	{
		[RequiredField]
		public FsmString buttonName;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			buttonName = "Fire1";
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoGetButton();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetButton();
		}

		private void DoGetButton()
		{
			storeResult.Value = Input.GetButton(buttonName.Value);
		}
	}
}
