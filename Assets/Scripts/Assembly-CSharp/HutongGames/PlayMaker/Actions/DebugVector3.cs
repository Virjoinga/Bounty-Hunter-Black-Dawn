namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Vector3 Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugVector3 : FsmStateAction
	{
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			vector3Variable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!vector3Variable.IsNone)
			{
				text = vector3Variable.Name + ": " + vector3Variable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
