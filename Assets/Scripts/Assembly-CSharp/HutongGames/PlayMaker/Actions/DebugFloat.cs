namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Float Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugFloat : FsmStateAction
	{
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			floatVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!floatVariable.IsNone)
			{
				text = floatVariable.Name + ": " + floatVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
