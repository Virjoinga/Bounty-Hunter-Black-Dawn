using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmState : INameable
	{
		private bool active;

		private bool finished;

		[NonSerialized]
		private Fsm fsm;

		[SerializeField]
		private string name;

		[SerializeField]
		private string description;

		[SerializeField]
		private byte colorIndex;

		[SerializeField]
		private Rect position;

		[SerializeField]
		private bool isBreakpoint;

		[SerializeField]
		private bool hideUnused;

		[SerializeField]
		private FsmTransition[] transitions = new FsmTransition[0];

		[NonSerialized]
		private FsmStateAction[] actions;

		[SerializeField]
		private ActionData actionData = new ActionData();

		public float StateTime { get; private set; }

		public float RealStartTime { get; private set; }

		public bool Active
		{
			get
			{
				return active;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return fsm != null;
			}
		}

		public Fsm Fsm
		{
			get
			{
				if (fsm == null)
				{
					Debug.LogError("get_fsm: Fsm not initialized: " + name);
				}
				return fsm;
			}
			set
			{
				if (value == null)
				{
					Debug.LogWarning("set_fsm: value == null: " + name);
				}
				fsm = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public Rect Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public bool IsBreakpoint
		{
			get
			{
				return isBreakpoint;
			}
			set
			{
				isBreakpoint = value;
			}
		}

		public bool HideUnused
		{
			get
			{
				return hideUnused;
			}
			set
			{
				hideUnused = value;
			}
		}

		public FsmStateAction[] Actions
		{
			get
			{
				if (fsm == null)
				{
					Debug.LogError("get_actions: Fsm not initialized: " + name);
				}
				return actions ?? (actions = actionData.LoadActions(this));
			}
			set
			{
				actions = value;
			}
		}

		public ActionData ActionData
		{
			get
			{
				return actionData;
			}
		}

		public FsmTransition[] Transitions
		{
			get
			{
				return transitions;
			}
			set
			{
				transitions = value;
			}
		}

		public string Description
		{
			get
			{
				return description ?? (description = "");
			}
			set
			{
				description = value;
			}
		}

		public int ColorIndex
		{
			get
			{
				return colorIndex;
			}
			set
			{
				colorIndex = (byte)value;
			}
		}

		public static string GetFullStateLabel(FsmState state)
		{
			if (state == null)
			{
				return "None (State)";
			}
			return Fsm.GetFullFsmLabel(state.Fsm) + " : " + state.Name;
		}

		public FsmState(Fsm fsm)
		{
			this.fsm = fsm;
		}

		public void CopyActionData(FsmState state)
		{
			actionData = state.actionData.Copy();
		}

		public void LoadActions()
		{
			actions = actionData.LoadActions(this);
		}

		public void SaveActions()
		{
			if (actions != null)
			{
				actionData.SaveActions(actions);
			}
		}

		public void OnEnter()
		{
			active = true;
			finished = false;
			RealStartTime = FsmTime.RealtimeSinceStartup;
			StateTime = 0f;
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (!fsmStateAction.Enabled)
				{
					fsmStateAction.Finished = true;
					continue;
				}
				fsmStateAction.Finished = false;
				Fsm.ExecutingAction = fsmStateAction;
				fsmStateAction.Init(this);
				fsmStateAction.OnEnter();
				Fsm.ExecutingAction = null;
				if (this != Fsm.ActiveState)
				{
					return;
				}
			}
			CheckFinished();
		}

		public void OnFixedUpdate()
		{
			if (finished)
			{
				return;
			}
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Active)
				{
					Fsm.ExecutingAction = fsmStateAction;
					fsmStateAction.Init(this);
					fsmStateAction.OnFixedUpdate();
					Fsm.ExecutingAction = null;
					if (this != Fsm.ActiveState)
					{
						return;
					}
				}
			}
			CheckFinished();
		}

		public void OnUpdate()
		{
			if (finished)
			{
				return;
			}
			StateTime += Time.deltaTime;
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Active)
				{
					Fsm.ExecutingAction = fsmStateAction;
					fsmStateAction.Init(this);
					fsmStateAction.OnUpdate();
					Fsm.ExecutingAction = null;
					if (this != Fsm.ActiveState)
					{
						return;
					}
				}
			}
			CheckFinished();
		}

		public void OnLateUpdate()
		{
			if (finished)
			{
				return;
			}
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Active)
				{
					Fsm.ExecutingAction = fsmStateAction;
					fsmStateAction.Init(this);
					fsmStateAction.OnLateUpdate();
					Fsm.ExecutingAction = null;
					if (this != Fsm.ActiveState)
					{
						return;
					}
				}
			}
			CheckFinished();
		}

		private void CheckFinished()
		{
			if (!active)
			{
				return;
			}
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (!fsmStateAction.Finished)
				{
					return;
				}
			}
			finished = true;
			fsm.Event(FsmEvent.Finished);
		}

		public void OnExit()
		{
			active = false;
			finished = false;
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Enabled)
				{
					Fsm.ExecutingAction = fsmStateAction;
					fsmStateAction.Init(this);
					fsmStateAction.OnExit();
				}
			}
		}
	}
}
