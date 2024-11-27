namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of an Integer Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugInt : FsmStateAction
	{
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			intVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!intVariable.IsNone)
			{
				text = intVariable.Name + ": " + intVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
