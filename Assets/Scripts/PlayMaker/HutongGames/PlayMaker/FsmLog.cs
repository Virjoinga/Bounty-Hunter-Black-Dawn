using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class FsmLog
	{
		private const int MaxLogSize = 100000;

		private static int currentFrame;

		private static float currentFrameTime;

		private static readonly List<FsmLog> Logs = new List<FsmLog>();

		private List<FsmLogEntry> entries = new List<FsmLogEntry>();

		public static bool LoggingEnabled { get; set; }

		public static bool MirrorDebugLog { get; set; }

		public static bool EnableDebugFlow { get; set; }

		public Fsm Fsm { get; private set; }

		public List<FsmLogEntry> Entries
		{
			get
			{
				return entries;
			}
		}

		private FsmLog(Fsm fsm)
		{
			Fsm = fsm;
		}

		public static FsmLog GetLog(Fsm fsm)
		{
			if (fsm == null)
			{
				return null;
			}
			foreach (FsmLog log in Logs)
			{
				if (log.Fsm == fsm)
				{
					return log;
				}
			}
			FsmLog fsmLog = new FsmLog(fsm);
			Logs.Add(fsmLog);
			return fsmLog;
		}

		public static void ClearLogs()
		{
			foreach (FsmLog log in Logs)
			{
				log.Clear();
			}
		}

		private void AddEntry(FsmLogEntry entry)
		{
			entry.FrameCount = Time.frameCount;
			entry.Time = FsmTime.RealtimeSinceStartup;
			if (!string.IsNullOrEmpty(entry.Text))
			{
				entry.Text = FormatTime(entry.Time) + " " + entry.Text;
			}
			entries.Add(entry);
			if (entries.Count > 100000)
			{
				entries.RemoveRange(0, 1000);
			}
			switch (entry.LogType)
			{
			case FsmLogType.Error:
				Debug.LogError(FormatUnityLogString(entry.Text));
				return;
			case FsmLogType.Warning:
				Debug.LogWarning(FormatUnityLogString(entry.Text));
				return;
			}
			if (MirrorDebugLog)
			{
				Debug.Log(FormatUnityLogString(entry.Text));
			}
		}

		public void LogEvent(FsmEvent fsmEvent, FsmState state)
		{
			FsmLogEntry fsmLogEntry = new FsmLogEntry();
			fsmLogEntry.LogType = FsmLogType.Event;
			fsmLogEntry.State = state;
			fsmLogEntry.SentByState = Fsm.EventData.SentByState;
			fsmLogEntry.Action = Fsm.EventData.SentByAction;
			fsmLogEntry.Event = fsmEvent;
			fsmLogEntry.Text = "EVENT: " + fsmEvent.Name;
			FsmLogEntry entry = fsmLogEntry;
			AddEntry(entry);
		}

		public void LogSendEvent(FsmState state, FsmEvent fsmEvent, FsmEventTarget eventTarget)
		{
			if (state != null && fsmEvent != null && !fsmEvent.IsSystemEvent)
			{
				FsmLogEntry fsmLogEntry = new FsmLogEntry();
				fsmLogEntry.LogType = FsmLogType.SendEvent;
				fsmLogEntry.State = state;
				fsmLogEntry.Event = fsmEvent;
				fsmLogEntry.Text = "SEND EVENT: " + fsmEvent.Name;
				fsmLogEntry.EventTarget = new FsmEventTarget(eventTarget);
				FsmLogEntry entry = fsmLogEntry;
				AddEntry(entry);
			}
		}

		public void LogExitState(FsmState state)
		{
			if (state != null)
			{
				FsmLogEntry fsmLogEntry = new FsmLogEntry();
				fsmLogEntry.LogType = FsmLogType.ExitState;
				fsmLogEntry.State = state;
				fsmLogEntry.Text = "EXIT: " + state.Name;
				FsmLogEntry fsmLogEntry2 = fsmLogEntry;
				if (EnableDebugFlow && state.Fsm.EnableDebugFlow && !PlayMakerFSM.ApplicationIsQuitting)
				{
					fsmLogEntry2.FsmVariablesCopy = new FsmVariables(state.Fsm.Variables);
					fsmLogEntry2.GlobalVariablesCopy = new FsmVariables(FsmVariables.GlobalVariables);
				}
				AddEntry(fsmLogEntry2);
			}
		}

		public void LogEnterState(FsmState state)
		{
			if (state != null)
			{
				FsmLogEntry fsmLogEntry = new FsmLogEntry();
				fsmLogEntry.LogType = FsmLogType.EnterState;
				fsmLogEntry.State = state;
				fsmLogEntry.Text = "ENTER: " + state.Name;
				FsmLogEntry fsmLogEntry2 = fsmLogEntry;
				if (EnableDebugFlow && state.Fsm.EnableDebugFlow)
				{
					fsmLogEntry2.FsmVariablesCopy = new FsmVariables(state.Fsm.Variables);
					fsmLogEntry2.GlobalVariablesCopy = new FsmVariables(FsmVariables.GlobalVariables);
				}
				AddEntry(fsmLogEntry2);
			}
		}

		public void LogTransition(FsmState fromState, FsmTransition transition)
		{
			FsmLogEntry fsmLogEntry = new FsmLogEntry();
			fsmLogEntry.LogType = FsmLogType.Transition;
			fsmLogEntry.State = fromState;
			fsmLogEntry.Transition = transition;
			FsmLogEntry entry = fsmLogEntry;
			AddEntry(entry);
		}

		public void LogBreak()
		{
			FsmLogEntry fsmLogEntry = new FsmLogEntry();
			fsmLogEntry.LogType = FsmLogType.Break;
			fsmLogEntry.State = Fsm.ExecutingState;
			fsmLogEntry.Text = "BREAK";
			FsmLogEntry entry = fsmLogEntry;
			Debug.Log(FormatUnityLogString("Breakpoint"));
			AddEntry(entry);
		}

		public void LogAction(FsmLogType logType, string text)
		{
			if (Fsm.ExecutingAction != null)
			{
				FsmLogEntry fsmLogEntry = new FsmLogEntry();
				fsmLogEntry.LogType = logType;
				fsmLogEntry.State = Fsm.ExecutingState;
				fsmLogEntry.Action = Fsm.ExecutingAction;
				fsmLogEntry.Text = FsmUtility.StripNamespace(Fsm.ExecutingAction.ToString()) + " : " + text;
				FsmLogEntry entry = fsmLogEntry;
				AddEntry(entry);
			}
		}

		public void Log(FsmLogType logType, string text)
		{
			FsmLogEntry fsmLogEntry = new FsmLogEntry();
			fsmLogEntry.LogType = logType;
			fsmLogEntry.State = Fsm.ExecutingState;
			fsmLogEntry.Text = text;
			FsmLogEntry entry = fsmLogEntry;
			AddEntry(entry);
		}

		public void LogStart(FsmState startState)
		{
			FsmLogEntry fsmLogEntry = new FsmLogEntry();
			fsmLogEntry.LogType = FsmLogType.Start;
			fsmLogEntry.State = startState;
			fsmLogEntry.Text = "START";
			FsmLogEntry entry = fsmLogEntry;
			AddEntry(entry);
		}

		public void Log(string text)
		{
			Log(FsmLogType.Info, text);
		}

		public void LogWarning(string text)
		{
			Log(FsmLogType.Warning, text);
		}

		public void LogError(string text)
		{
			Log(FsmLogType.Error, text);
		}

		private string FormatUnityLogString(string text)
		{
			string text2 = Fsm.GetFullFsmLabel(Fsm);
			if (Fsm.ExecutingState != null)
			{
				text2 = text2 + " : " + Fsm.ExecutingState.Name;
			}
			return text2 + " : " + text;
		}

		public void Clear()
		{
			if (entries != null)
			{
				entries.Clear();
			}
		}

		public void OnDestroy()
		{
			Logs.Remove(this);
			Clear();
			entries = null;
			Fsm = null;
		}

		public static string FormatTime(float time)
		{
			return new DateTime((long)(time * 10000000f)).ToString("mm:ss:ff");
		}
	}
}
