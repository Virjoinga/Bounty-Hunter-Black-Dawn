using System;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmLogEntry
	{
		public FsmLogType LogType { get; set; }

		public FsmState State { get; set; }

		public FsmState SentByState { get; set; }

		public FsmStateAction Action { get; set; }

		public FsmEvent Event { get; set; }

		public FsmTransition Transition { get; set; }

		public FsmEventTarget EventTarget { get; set; }

		public float Time { get; set; }

		public string Text { get; set; }

		public string Text2 { get; set; }

		public int FrameCount { get; set; }

		public FsmVariables FsmVariablesCopy { get; set; }

		public FsmVariables GlobalVariablesCopy { get; set; }
	}
}
