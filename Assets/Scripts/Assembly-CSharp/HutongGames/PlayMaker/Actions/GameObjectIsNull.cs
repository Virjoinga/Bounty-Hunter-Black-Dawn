namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Game Object Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsNull : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;

		public FsmEvent isNull;

		public FsmEvent isNotNull;

		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			isNull = null;
			isNotNull = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsGameObjectNull();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsGameObjectNull();
		}

		private void DoIsGameObjectNull()
		{
			bool flag = gameObject.Value == null;
			if (storeResult != null)
			{
				storeResult.Value = flag;
			}
			base.Fsm.Event((!flag) ? isNotNull : isNull);
		}
	}
}
