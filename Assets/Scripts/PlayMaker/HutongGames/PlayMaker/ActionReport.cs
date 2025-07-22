using System.Collections.Generic;

namespace HutongGames.PlayMaker
{
	public class ActionReport
	{
		public static readonly List<ActionReport> ActionReportList = new List<ActionReport>();

		public static int InfoCount;

		public static int ErrorCount;

		public PlayMakerFSM fsm;

		public FsmState state;

		public FsmStateAction action;

		public int actionIndex;

		public string logText;

		public bool isError;

		public static void Start()
		{
			ActionReportList.Clear();
			InfoCount = 0;
			ErrorCount = 0;
		}

		public static void Log(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string logLine, bool isError = false)
		{
			ActionReport actionReport = new ActionReport();
			actionReport.fsm = fsm;
			actionReport.state = state;
			actionReport.action = action;
			actionReport.actionIndex = actionIndex;
			actionReport.logText = logLine;
			actionReport.isError = isError;
			ActionReport item = actionReport;
			ActionReportList.Add(item);
			InfoCount++;
		}

		public static void LogError(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string logLine)
		{
			Log(fsm, state, action, actionIndex, logLine, true);
			ErrorCount++;
		}

		public static void Clear()
		{
			ActionReportList.Clear();
		}

		public static int GetCount()
		{
			return ActionReportList.Count;
		}
	}
}
