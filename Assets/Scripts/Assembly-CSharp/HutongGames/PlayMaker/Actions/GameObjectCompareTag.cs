namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Game Object has a tag.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectCompareTag : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;

		[RequiredField]
		[UIHint(UIHint.Tag)]
		public FsmString tag;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			tag = "Untagged";
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCompareTag();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCompareTag();
		}

		private void DoCompareTag()
		{
			bool flag = false;
			if (gameObject.Value != null)
			{
				flag = gameObject.Value.CompareTag(tag.Value);
			}
			storeResult.Value = flag;
			base.Fsm.Event((!flag) ? falseEvent : trueEvent);
		}
	}
}
