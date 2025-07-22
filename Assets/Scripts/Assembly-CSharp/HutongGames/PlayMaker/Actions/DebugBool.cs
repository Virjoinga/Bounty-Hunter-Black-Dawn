namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugBool : FsmStateAction
	{
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			boolVariable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!boolVariable.IsNone)
			{
				text = boolVariable.Name + ": " + boolVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
