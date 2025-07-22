namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Game Object Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugGameObject : FsmStateAction
	{
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			gameObject = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!gameObject.IsNone)
			{
				text = gameObject.Name + ": " + gameObject;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
