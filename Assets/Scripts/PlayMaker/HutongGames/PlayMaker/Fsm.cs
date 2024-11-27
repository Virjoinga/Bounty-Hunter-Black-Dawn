using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class Fsm : INameable, IComparable
	{
		private const string StartStateName = "State 1";

		[SerializeField]
		private MonoBehaviour owner;

		[SerializeField]
		private string name = "FSM";

		[SerializeField]
		private string startState;

		[SerializeField]
		private FsmState[] states = new FsmState[1];

		[SerializeField]
		private FsmEvent[] events = new FsmEvent[0];

		[SerializeField]
		private FsmTransition[] globalTransitions = new FsmTransition[0];

		[SerializeField]
		private FsmVariables variables = new FsmVariables();

		[SerializeField]
		private string description = "";

		[SerializeField]
		private string docUrl;

		[SerializeField]
		private bool showStateLabel = true;

		[SerializeField]
		private int maxReEnterStateCount = 100;

		[SerializeField]
		private string watermark = "";

		private static Fsm breakAtFsm;

		private static FsmState breakAtState;

		private static FsmStateAction breakAtAction;

		private bool activeStateEntered;

		private int reEnterStateCount;

		public List<FsmEvent> ExposedEvents = new List<FsmEvent>();

		private static Color debugLookAtColor = Color.yellow;

		private static Color debugRaycastColor = Color.red;

		private FsmLog myLog;

		public bool RestartOnEnable = true;

		public bool EnableDebugFlow = true;

		public bool StepFrame;

		private bool enterStartState;

		private int startCount;

		private readonly List<DelayedEvent> delayedEvents = new List<DelayedEvent>();

		private readonly List<DelayedEvent> updateEvents = new List<DelayedEvent>();

		private readonly List<DelayedEvent> removeEvents = new List<DelayedEvent>();

		public static FsmEventData EventData = new FsmEventData();

		[SerializeField]
		private string activeStateName;

		[NonSerialized]
		private FsmState activeState;

		public static readonly Color[] StateColors = new Color[8]
		{
			Color.grey,
			new Color(0.54509807f, 57f / 85f, 0.9411765f),
			new Color(0.24313726f, 0.7607843f, 0.6901961f),
			new Color(22f / 51f, 0.7607843f, 0.24313726f),
			new Color(1f, 0.8745098f, 16f / 85f),
			new Color(1f, 47f / 85f, 16f / 85f),
			new Color(0.7607843f, 0.24313726f, 0.2509804f),
			new Color(0.54509807f, 0.24313726f, 0.7607843f)
		};

		private static readonly FsmEventTarget targetSelf = new FsmEventTarget();

		public static List<Fsm> FsmList
		{
			get
			{
				List<Fsm> list = new List<Fsm>();
				foreach (PlayMakerFSM fsm in PlayMakerFSM.FsmList)
				{
					if (fsm != null && fsm.Fsm != null)
					{
						list.Add(fsm.Fsm);
					}
				}
				return list;
			}
		}

		public static List<Fsm> SortedFsmList
		{
			get
			{
				List<Fsm> fsmList = FsmList;
				fsmList.Sort();
				return fsmList;
			}
		}

		public List<DelayedEvent> DelayedEvents
		{
			get
			{
				return delayedEvents;
			}
		}

		public MonoBehaviour Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
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

		public string StartState
		{
			get
			{
				return startState;
			}
			set
			{
				startState = value;
			}
		}

		public FsmState[] States
		{
			get
			{
				return states;
			}
			set
			{
				states = value;
			}
		}

		public FsmEvent[] Events
		{
			get
			{
				return events;
			}
			set
			{
				events = value;
			}
		}

		public FsmTransition[] GlobalTransitions
		{
			get
			{
				return globalTransitions;
			}
			set
			{
				globalTransitions = value;
			}
		}

		public FsmVariables Variables
		{
			get
			{
				return variables;
			}
			set
			{
				variables = value;
			}
		}

		public FsmEventTarget EventTarget { get; set; }

		public bool Initialized { get; private set; }

		public bool Active
		{
			get
			{
				if (owner != null && owner.enabled && owner.gameObject != null)
				{
					return owner.gameObject.active;
				}
				return false;
			}
		}

		public FsmState ActiveState
		{
			get
			{
				if (activeState == null && activeStateName != "")
				{
					activeState = GetState(activeStateName);
				}
				return activeState;
			}
			private set
			{
				activeState = value;
				activeStateName = ((activeState == null) ? "" : activeState.Name);
			}
		}

		public string ActiveStateName
		{
			get
			{
				return activeStateName;
			}
		}

		public FsmState PreviousActiveState { get; private set; }

		public FsmTransition LastTransition { get; private set; }

		public int MaxReEnterStateCount
		{
			set
			{
				maxReEnterStateCount = value;
			}
		}

		public string OwnerName
		{
			get
			{
				if (!(owner != null))
				{
					return "";
				}
				return owner.name;
			}
		}

		public string OwnerDebugName
		{
			get
			{
				if (!(owner != null))
				{
					return "[missing Owner]";
				}
				return owner.name;
			}
		}

		public GameObject GameObject
		{
			get
			{
				if (!(Owner != null))
				{
					return null;
				}
				return Owner.gameObject;
			}
		}

		public string GameObjectName
		{
			get
			{
				if (!(Owner != null))
				{
					return "[missing GameObject]";
				}
				return Owner.gameObject.name;
			}
		}

		public FsmLog MyLog
		{
			get
			{
				return myLog ?? (myLog = FsmLog.GetLog(this));
			}
		}

		public bool IsModifiedPrefabInstance { get; set; }

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string Watermark
		{
			get
			{
				return watermark;
			}
			set
			{
				watermark = value;
			}
		}

		public bool ShowStateLabel
		{
			get
			{
				return showStateLabel;
			}
			set
			{
				showStateLabel = value;
			}
		}

		public static Color DebugLookAtColor
		{
			get
			{
				return debugLookAtColor;
			}
			set
			{
				debugLookAtColor = value;
			}
		}

		public static Color DebugRaycastColor
		{
			get
			{
				return debugRaycastColor;
			}
			set
			{
				debugRaycastColor = value;
			}
		}

		private string GuiLabel
		{
			get
			{
				return OwnerName + " : " + Name;
			}
		}

		public string DocUrl
		{
			get
			{
				return docUrl;
			}
			set
			{
				docUrl = value;
			}
		}

		public FsmState EditState { get; set; }

		public static GameObject LastClickedObject { get; set; }

		public static Fsm ExecutingFsm { get; private set; }

		public static FsmStateAction ExecutingAction { get; set; }

		public static FsmState ExecutingState { get; private set; }

		public static Fsm BreakAtFsm
		{
			get
			{
				return breakAtFsm ?? ExecutingFsm;
			}
		}

		public static FsmState BreakAtState
		{
			get
			{
				return breakAtState ?? ExecutingState;
			}
		}

		public static FsmStateAction BreakAtAction
		{
			get
			{
				return breakAtAction ?? ExecutingAction;
			}
		}

		public static bool StepToStateChange { get; set; }

		public static Fsm StepFsm { get; set; }

		public bool SwitchedState { get; set; }

		public static string LastError { get; private set; }

		public static bool IsErrorBreak { get; private set; }

		public static bool IsBreak { get; private set; }

		public static bool BreakpointsEnabled { get; set; }

		public bool HitBreakpoint { get; set; }

		public bool MouseEvents { get; set; }

		public bool HandleTriggerEnter { get; set; }

		public bool HandleTriggerExit { get; set; }

		public bool HandleTriggerStay { get; set; }

		public bool HandleCollisionEnter { get; set; }

		public bool HandleCollisionExit { get; set; }

		public bool HandleCollisionStay { get; set; }

		public bool HandleOnGUI { get; set; }

		public bool HandleFixedUpdate { get; set; }

		public Collision CollisionInfo { get; private set; }

		public Collider TriggerCollider { get; private set; }

		public ControllerColliderHit ControllerCollider { get; private set; }

		public RaycastHit RaycastHitInfo { get; set; }

		public void Reset(MonoBehaviour component)
		{
			owner = component;
			name = "FSM";
			description = "";
			docUrl = "";
			globalTransitions = new FsmTransition[0];
			events = new FsmEvent[0];
			variables = new FsmVariables();
			states = new FsmState[1];
			States[0] = new FsmState(this)
			{
				Fsm = this,
				Name = "State 1",
				Position = new Rect(200f, 200f, 100f, 16f)
			};
			startState = "State 1";
		}

		public void Init(MonoBehaviour component)
		{
			owner = component;
			if (!Application.isEditor)
			{
				FsmLog.LoggingEnabled = false;
			}
			InitData();
		}

		public void Reinitialize()
		{
			Initialized = false;
			InitData();
		}

		public void InitData()
		{
			if (Initialized)
			{
				return;
			}
			Initialized = true;
			for (int i = 0; i < events.Length; i++)
			{
				if (events[i] == null)
				{
					continue;
				}
				FsmEvent fsmEvent = FsmEvent.GetFsmEvent(events[i]);
				events[i] = fsmEvent;
				if (fsmEvent.IsSystemEvent)
				{
					if (fsmEvent == FsmEvent.TriggerEnter)
					{
						HandleTriggerEnter = true;
					}
					if (fsmEvent == FsmEvent.TriggerExit)
					{
						HandleTriggerExit = true;
					}
					if (fsmEvent == FsmEvent.TriggerStay)
					{
						HandleTriggerStay = true;
					}
					if (fsmEvent == FsmEvent.CollisionEnter)
					{
						HandleCollisionEnter = true;
					}
					if (fsmEvent == FsmEvent.CollisionExit)
					{
						HandleCollisionExit = true;
					}
					if (fsmEvent == FsmEvent.CollisionStay)
					{
						HandleCollisionStay = true;
					}
					if (fsmEvent.IsMouseEvent)
					{
						MouseEvents = true;
					}
				}
			}
			for (int j = 0; j < ExposedEvents.Count; j++)
			{
				if (ExposedEvents[j] != null)
				{
					ExposedEvents[j] = FsmEvent.GetFsmEvent(ExposedEvents[j]);
				}
			}
			FsmState[] array = states;
			foreach (FsmState fsmState in array)
			{
				fsmState.Fsm = this;
				fsmState.LoadActions();
				FsmTransition[] transitions = fsmState.Transitions;
				foreach (FsmTransition fsmTransition in transitions)
				{
					if (!string.IsNullOrEmpty(fsmTransition.EventName))
					{
						FsmEvent @event = GetEvent(fsmTransition.EventName);
						fsmTransition.FsmEvent = @event;
					}
				}
			}
			FsmTransition[] array2 = globalTransitions;
			foreach (FsmTransition fsmTransition2 in array2)
			{
				fsmTransition2.FsmEvent = GetEvent(fsmTransition2.EventName);
			}
		}

		public void OnEnable()
		{
			Start();
		}

		public void Start()
		{
			ExecutingFsm = this;
			ExecutingState = null;
			ExecutingAction = null;
			LastTransition = null;
			HitBreakpoint = false;
			SwitchedState = false;
			if (RestartOnEnable && startCount > 0)
			{
				EnterStartState();
			}
		}

		public void EnterStartState()
		{
			startCount++;
			ActiveState = GetState(startState);
			if (FsmLog.LoggingEnabled)
			{
				MyLog.LogStart(activeState);
			}
			if (activeState != null)
			{
				enterStartState = true;
				return;
			}
			owner.enabled = false;
			MyLog.LogError("Missing Start State!");
		}

		public void Update()
		{
			FsmTime.RealtimeBugFix();
			if (owner == null)
			{
				return;
			}
			ExecutingFsm = this;
			ExecutingState = null;
			ExecutingAction = null;
			if (HitBreakpoint)
			{
				return;
			}
			if (enterStartState)
			{
				enterStartState = false;
				if (BreakpointsEnabled && ActiveState.IsBreakpoint)
				{
					ExecutingState = activeState;
					DoBreakpoint();
					return;
				}
				EnterState(activeState);
			}
			if (!activeStateEntered)
			{
				Continue();
			}
			UpdateDelayedEvents();
			if (ActiveState != null)
			{
				UpdateState(ActiveState);
			}
		}

		public void UpdateDelayedEvents()
		{
			removeEvents.Clear();
			updateEvents.Clear();
			updateEvents.AddRange(delayedEvents);
			foreach (DelayedEvent updateEvent in updateEvents)
			{
				updateEvent.Update();
				if (updateEvent.Finished)
				{
					removeEvents.Add(updateEvent);
				}
			}
			foreach (DelayedEvent removeEvent in removeEvents)
			{
				delayedEvents.Remove(removeEvent);
			}
		}

		public void ClearDelayedEvents()
		{
			delayedEvents.Clear();
		}

		public void FixedUpdate()
		{
			ExecutingFsm = this;
			if (ActiveState != null && !enterStartState)
			{
				FixedUpdateState(ActiveState);
			}
		}

		public void LateUpdate()
		{
			ExecutingFsm = this;
			if (ActiveState != null)
			{
				LateUpdateState(ActiveState);
			}
		}

		public void OnDisable()
		{
			if (RestartOnEnable)
			{
				Stop();
			}
		}

		public void Stop()
		{
			ExecutingFsm = this;
			if (ActiveState != null)
			{
				ExitState(ActiveState);
			}
			LastTransition = null;
			SwitchedState = false;
			HitBreakpoint = false;
		}

		public bool HasEvent(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return false;
			}
			FsmEvent[] array = events;
			foreach (FsmEvent fsmEvent in array)
			{
				if (fsmEvent.Name == eventName)
				{
					return true;
				}
			}
			return false;
		}

		public void ChangeState(FsmEvent fsmEvent)
		{
			if (FsmEvent.IsNullOrEmpty(fsmEvent) || !owner.enabled || !owner.gameObject.active)
			{
				return;
			}
			ExecutingFsm = this;
			FsmTransition[] array = globalTransitions;
			foreach (FsmTransition fsmTransition in array)
			{
				if (fsmTransition.FsmEvent == fsmEvent)
				{
					if (FsmLog.LoggingEnabled)
					{
						MyLog.LogEvent(fsmEvent, activeState);
					}
					if (DoTransition(fsmTransition, true))
					{
						return;
					}
				}
			}
			if (ActiveState == null)
			{
				return;
			}
			FsmTransition[] transitions = ActiveState.Transitions;
			foreach (FsmTransition fsmTransition2 in transitions)
			{
				if (fsmTransition2.FsmEvent == fsmEvent)
				{
					if (FsmLog.LoggingEnabled)
					{
						MyLog.LogEvent(fsmEvent, activeState);
					}
					if (DoTransition(fsmTransition2, false))
					{
						break;
					}
				}
			}
		}

		private static void SetEventDataSentBy()
		{
			EventData.SentByFsm = ExecutingFsm;
			EventData.SentByState = ExecutingState;
			EventData.SentByAction = ExecutingAction;
		}

		private static void SetEventDataSentBy(FsmEventData eventData)
		{
			EventData.SentByFsm = eventData.SentByFsm;
			EventData.SentByState = eventData.SentByState;
			EventData.SentByAction = eventData.SentByAction;
		}

		private static FsmEventData NewEventData()
		{
			FsmEventData fsmEventData = new FsmEventData();
			fsmEventData.SentByFsm = ExecutingFsm;
			fsmEventData.SentByState = ExecutingState;
			fsmEventData.SentByAction = ExecutingAction;
			return fsmEventData;
		}

		public void Event(FsmEventTarget eventTarget, string fsmEventName)
		{
			if (!string.IsNullOrEmpty(fsmEventName))
			{
				Event(eventTarget, FsmEvent.GetFsmEvent(fsmEventName));
			}
		}

		public void Event(FsmEventTarget eventTarget, FsmEvent fsmEvent)
		{
			if (eventTarget == null)
			{
				eventTarget = targetSelf;
			}
			if (FsmLog.LoggingEnabled && eventTarget.target != 0)
			{
				MyLog.LogSendEvent(activeState, fsmEvent, eventTarget);
			}
			switch (eventTarget.target)
			{
			case FsmEventTarget.EventTarget.Self:
				SetEventDataSentBy();
				ChangeState(fsmEvent);
				break;
			case FsmEventTarget.EventTarget.GameObject:
			{
				GameObject ownerDefaultTarget = GetOwnerDefaultTarget(eventTarget.gameObject);
				BroadcastEventToGameObject(ownerDefaultTarget, fsmEvent, NewEventData(), eventTarget.sendToChildren.Value, eventTarget.excludeSelf.Value);
				break;
			}
			case FsmEventTarget.EventTarget.GameObjectFSM:
			{
				GameObject ownerDefaultTarget = GetOwnerDefaultTarget(eventTarget.gameObject);
				SendEventToFsmOnGameObject(ownerDefaultTarget, eventTarget.fsmName.Value, fsmEvent);
				break;
			}
			case FsmEventTarget.EventTarget.FSMComponent:
				if (eventTarget.fsmComponent != null)
				{
					SetEventDataSentBy();
					eventTarget.fsmComponent.Fsm.ChangeState(fsmEvent);
				}
				break;
			case FsmEventTarget.EventTarget.BroadcastAll:
				BroadcastEvent(fsmEvent, eventTarget.excludeSelf.Value);
				break;
			}
		}

		public void Event(string fsmEventName)
		{
			if (!string.IsNullOrEmpty(fsmEventName))
			{
				Event(FsmEvent.GetFsmEvent(fsmEventName));
			}
		}

		public void Event(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				Event(EventTarget, fsmEvent);
			}
		}

		public DelayedEvent DelayedEvent(FsmEvent fsmEvent, float delay)
		{
			DelayedEvent delayedEvent = new DelayedEvent(this, fsmEvent, delay);
			delayedEvents.Add(delayedEvent);
			return delayedEvent;
		}

		public DelayedEvent DelayedEvent(FsmEventTarget eventTarget, FsmEvent fsmEvent, float delay)
		{
			DelayedEvent delayedEvent = new DelayedEvent(this, eventTarget, fsmEvent, delay);
			delayedEvents.Add(delayedEvent);
			return delayedEvent;
		}

		public void BroadcastEvent(string fsmEventName, bool excludeSelf = false)
		{
			if (!string.IsNullOrEmpty(fsmEventName))
			{
				BroadcastEvent(FsmEvent.GetFsmEvent(fsmEventName), excludeSelf);
			}
		}

		public void BroadcastEvent(FsmEvent fsmEvent, bool excludeSelf = false)
		{
			FsmEventData eventDataSentBy = NewEventData();
			List<PlayMakerFSM> list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);
			foreach (PlayMakerFSM item in list)
			{
				if (!(item == null) && item.Fsm != null && (!excludeSelf || item.Fsm != this))
				{
					SetEventDataSentBy(eventDataSentBy);
					item.Fsm.ChangeState(fsmEvent);
				}
			}
		}

		public void BroadcastEventToGameObject(GameObject go, string fsmEventName, bool sendToChildren, bool excludeSelf = false)
		{
			if (!string.IsNullOrEmpty(fsmEventName))
			{
				BroadcastEventToGameObject(go, FsmEvent.GetFsmEvent(fsmEventName), NewEventData(), sendToChildren, excludeSelf);
			}
		}

		public void BroadcastEventToGameObject(GameObject go, FsmEvent fsmEvent, FsmEventData eventData, bool sendToChildren, bool excludeSelf = false)
		{
			if (go == null)
			{
				return;
			}
			List<Fsm> list = new List<Fsm>();
			foreach (PlayMakerFSM fsm in PlayMakerFSM.FsmList)
			{
				if (fsm != null && fsm.gameObject == go)
				{
					list.Add(fsm.Fsm);
				}
			}
			foreach (Fsm item in list)
			{
				if (!excludeSelf || item != this)
				{
					SetEventDataSentBy(eventData);
					item.ChangeState(fsmEvent);
				}
			}
			if (sendToChildren)
			{
				for (int i = 0; i < go.transform.childCount; i++)
				{
					BroadcastEventToGameObject(go.transform.GetChild(i).gameObject, fsmEvent, eventData, true, excludeSelf);
				}
			}
		}

		public void SendEventToFsmOnGameObject(GameObject gameObject, string fsmName, string fsmEventName)
		{
			if (!string.IsNullOrEmpty(fsmEventName))
			{
				SendEventToFsmOnGameObject(gameObject, fsmName, FsmEvent.GetFsmEvent(fsmEventName));
			}
		}

		public void SendEventToFsmOnGameObject(GameObject gameObject, string fsmName, FsmEvent fsmEvent)
		{
			if (gameObject == null)
			{
				return;
			}
			SetEventDataSentBy();
			List<PlayMakerFSM> list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);
			if (string.IsNullOrEmpty(fsmName))
			{
				foreach (PlayMakerFSM item in list)
				{
					if (item.gameObject == gameObject)
					{
						item.Fsm.ChangeState(fsmEvent);
					}
				}
				return;
			}
			foreach (PlayMakerFSM item2 in list)
			{
				if (item2.gameObject == gameObject && fsmName == item2.Fsm.Name)
				{
					item2.Fsm.ChangeState(fsmEvent);
					break;
				}
			}
		}

		private bool DoTransition(FsmTransition transition, bool isGlobal)
		{
			FsmState state = GetState(transition.ToState);
			if (state == null)
			{
				return false;
			}
			LastTransition = transition;
			if (Application.isEditor)
			{
				MyLog.LogTransition(isGlobal ? null : ActiveState, transition);
			}
			SwitchState(state);
			return true;
		}

		private void SwitchState(FsmState toState)
		{
			if (toState != null)
			{
				if (ActiveState != null)
				{
					PreviousActiveState = ActiveState;
					ExitState(ActiveState);
				}
				if ((BreakpointsEnabled && toState.IsBreakpoint) || (StepToStateChange && (StepFsm == null || StepFsm == this)))
				{
					ExecutingState = toState;
					ActiveState = toState;
					DoBreakpoint();
				}
				else
				{
					EnterState(toState);
				}
			}
		}

		public void GotoPreviousState()
		{
			if (PreviousActiveState != null)
			{
				SwitchState(PreviousActiveState);
			}
		}

		private void EnterState(FsmState state)
		{
			EventTarget = null;
			ExecutingState = state;
			SwitchedState = true;
			ActiveState = state;
			reEnterStateCount++;
			if (reEnterStateCount > maxReEnterStateCount)
			{
				if (FsmLog.LoggingEnabled)
				{
					MyLog.LogWarning("Possible infinite loop!");
				}
			}
			else
			{
				if (FsmLog.LoggingEnabled)
				{
					MyLog.LogEnterState(state);
				}
				state.Fsm = ExecutingFsm;
				state.OnEnter();
			}
			activeStateEntered = true;
			reEnterStateCount = 0;
		}

		private static void FixedUpdateState(FsmState state)
		{
			ExecutingState = state;
			state.Fsm = ExecutingFsm;
			state.OnFixedUpdate();
		}

		private static void UpdateState(FsmState state)
		{
			ExecutingState = state;
			state.Fsm = ExecutingFsm;
			state.OnUpdate();
		}

		private static void LateUpdateState(FsmState state)
		{
			ExecutingState = state;
			state.Fsm = ExecutingFsm;
			state.OnLateUpdate();
		}

		private void ExitState(FsmState state)
		{
			ExecutingState = state;
			if (FsmLog.LoggingEnabled)
			{
				MyLog.LogExitState(state);
			}
			ActiveState = null;
			state.Fsm = ExecutingFsm;
			state.OnExit();
		}

		public static string GetFullFsmLabel(Fsm fsm)
		{
			if (fsm == null)
			{
				return "None (FSM)";
			}
			return fsm.OwnerName + " : " + fsm.Name;
		}

		public GameObject GetOwnerDefaultTarget(FsmOwnerDefault ownerDefault)
		{
			if (ownerDefault == null)
			{
				return null;
			}
			if (ownerDefault.OwnerOption != 0)
			{
				return ownerDefault.GameObject.Value;
			}
			return GameObject;
		}

		public FsmState GetState(string stateName)
		{
			FsmState[] array = states;
			foreach (FsmState fsmState in array)
			{
				if (fsmState.Name == stateName)
				{
					return fsmState;
				}
			}
			return null;
		}

		public FsmEvent GetEvent(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			FsmEvent fsmEvent = FsmEvent.GetFsmEvent(eventName);
			List<FsmEvent> list = new List<FsmEvent>(events);
			if (!FsmEvent.EventListContainsEvent(list, eventName))
			{
				list.Add(fsmEvent);
			}
			events = list.ToArray();
			return fsmEvent;
		}

		public int CompareTo(object obj)
		{
			Fsm fsm = obj as Fsm;
			if (fsm != null)
			{
				return GuiLabel.CompareTo(fsm.GuiLabel);
			}
			return 0;
		}

		public FsmObject GetFsmObject(string varName)
		{
			return variables.GetFsmObject(varName);
		}

		public FsmMaterial GetFsmMaterial(string varName)
		{
			return variables.GetFsmMaterial(varName);
		}

		public FsmTexture GetFsmTexture(string varName)
		{
			return variables.GetFsmTexture(varName);
		}

		public FsmFloat GetFsmFloat(string varName)
		{
			return variables.GetFsmFloat(varName);
		}

		public FsmInt GetFsmInt(string varName)
		{
			return variables.GetFsmInt(varName);
		}

		public FsmBool GetFsmBool(string varName)
		{
			return variables.GetFsmBool(varName);
		}

		public FsmString GetFsmString(string varName)
		{
			return variables.GetFsmString(varName);
		}

		public FsmVector2 GetFsmVector2(string varName)
		{
			return variables.GetFsmVector2(varName);
		}

		public FsmVector3 GetFsmVector3(string varName)
		{
			return variables.GetFsmVector3(varName);
		}

		public FsmRect GetFsmRect(string varName)
		{
			return variables.GetFsmRect(varName);
		}

		public FsmQuaternion GetFsmQuaternion(string varName)
		{
			return variables.GetFsmQuaternion(varName);
		}

		public FsmColor GetFsmColor(string varName)
		{
			return variables.GetFsmColor(varName);
		}

		public FsmGameObject GetFsmGameObject(string varName)
		{
			return variables.GetFsmGameObject(varName);
		}

		public void OnDrawGizmos()
		{
			if (PlayMakerFSM.DrawGizmos)
			{
				Gizmos.DrawIcon(owner.transform.position, "PlaymakerIcon.tiff");
			}
			if (EditState != null)
			{
				EditState.Fsm = this;
				FsmStateAction[] actions = EditState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					fsmStateAction.OnDrawGizmos();
				}
			}
		}

		public void OnDrawGizmosSelected()
		{
			if (EditState != null)
			{
				EditState.Fsm = this;
				FsmStateAction[] actions = EditState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					fsmStateAction.OnDrawGizmosSelected();
				}
			}
		}

		public void OnCollisionEnter(Collision collisionInfo)
		{
			CollisionInfo = collisionInfo;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoCollisionEnter(collisionInfo);
					}
				}
			}
			Event(FsmEvent.CollisionEnter);
		}

		public void OnCollisionStay(Collision collisionInfo)
		{
			CollisionInfo = collisionInfo;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoCollisionStay(collisionInfo);
					}
				}
			}
			Event(FsmEvent.CollisionStay);
		}

		public void OnCollisionExit(Collision collisionInfo)
		{
			CollisionInfo = collisionInfo;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoCollisionExit(collisionInfo);
					}
				}
			}
			Event(FsmEvent.CollisionExit);
		}

		public void OnTriggerEnter(Collider other)
		{
			TriggerCollider = other;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoTriggerEnter(other);
					}
				}
			}
			Event(FsmEvent.TriggerEnter);
		}

		public void OnTriggerStay(Collider other)
		{
			TriggerCollider = other;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoTriggerStay(other);
					}
				}
			}
			Event(FsmEvent.TriggerStay);
		}

		public void OnTriggerExit(Collider other)
		{
			TriggerCollider = other;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoTriggerExit(other);
					}
				}
			}
			Event(FsmEvent.TriggerExit);
		}

		public void OnControllerColliderHit(ControllerColliderHit collider)
		{
			ControllerCollider = collider;
			if (ActiveState != null)
			{
				FsmStateAction[] actions = ActiveState.Actions;
				foreach (FsmStateAction fsmStateAction in actions)
				{
					if (fsmStateAction.Active)
					{
						fsmStateAction.DoControllerColliderHit(collider);
					}
				}
			}
			Event(FsmEvent.ControllerColliderHit);
		}

		private void DoBreakpoint()
		{
			activeStateEntered = false;
			DoBreak();
		}

		public void DoBreakError(string error)
		{
			IsErrorBreak = true;
			LastError = error;
			DoBreak();
		}

		private void DoBreak()
		{
			breakAtFsm = ExecutingFsm;
			breakAtState = ExecutingState;
			breakAtAction = ExecutingAction;
			HitBreakpoint = true;
			IsBreak = true;
			if (FsmLog.LoggingEnabled)
			{
				MyLog.LogBreak();
			}
			StepToStateChange = false;
		}

		private void Continue()
		{
			HitBreakpoint = false;
			IsErrorBreak = false;
			IsBreak = false;
			ExecutingFsm = this;
			ExecutingState = ActiveState;
			activeStateEntered = true;
			enterStartState = false;
			EnterState(ActiveState);
		}

		public void OnDestroy()
		{
			if (myLog != null)
			{
				myLog.OnDestroy();
			}
		}
	}
}
