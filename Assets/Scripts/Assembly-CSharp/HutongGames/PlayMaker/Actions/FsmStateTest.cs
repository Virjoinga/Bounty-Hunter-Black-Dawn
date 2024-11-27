using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if an FSM is in the specified State.")]
	public class FsmStateTest : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;

		[Tooltip("Optional name of Fsm on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		public FsmString stateName;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public bool everyFrame;

		private GameObject previousGo;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			stateName = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFsmStateTest();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFsmStateTest();
		}

		private void DoFsmStateTest()
		{
			GameObject value = gameObject.Value;
			if (value == null)
			{
				return;
			}
			if (value != previousGo)
			{
				fsm = ActionHelpers.GetGameObjectFsm(value, fsmName.Value);
				previousGo = value;
			}
			if (!(fsm == null))
			{
				bool value2 = false;
				if (fsm.ActiveStateName == stateName.Value)
				{
					base.Fsm.Event(trueEvent);
					value2 = true;
				}
				else
				{
					base.Fsm.Event(falseEvent);
				}
				storeResult.Value = value2;
			}
		}
	}
}
