using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmEvent : IComparable, INameable
	{
		private static List<FsmEvent> eventList;

		[SerializeField]
		private string name;

		[SerializeField]
		private bool isSystemEvent;

		[SerializeField]
		private bool isGlobal;

		public static PlayMakerGlobals GlobalsComponent
		{
			get
			{
				return PlayMakerGlobals.Instance;
			}
		}

		public static List<string> globalEvents
		{
			get
			{
				return PlayMakerGlobals.Instance.Events;
			}
		}

		public static List<FsmEvent> EventList
		{
			get
			{
				if (eventList == null)
				{
					eventList = new List<FsmEvent>();
					AddSystemEvents();
					AddGlobalEvents();
				}
				return eventList;
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

		public bool IsSystemEvent
		{
			get
			{
				return isSystemEvent;
			}
			set
			{
				isSystemEvent = value;
			}
		}

		public bool IsMouseEvent
		{
			get
			{
				if (this != MouseDown && this != MouseDrag && this != MouseEnter && this != MouseExit && this != MouseOver)
				{
					return this == MouseUp;
				}
				return true;
			}
		}

		public bool IsGlobal
		{
			get
			{
				return isGlobal;
			}
			set
			{
				if (value)
				{
					if (!globalEvents.Contains(name))
					{
						globalEvents.Add(name);
					}
				}
				else
				{
					globalEvents.Remove(name);
				}
				isGlobal = value;
			}
		}

		public string Path { get; set; }

		public static FsmEvent BecameInvisible { get; private set; }

		public static FsmEvent BecameVisible { get; private set; }

		public static FsmEvent CollisionEnter { get; private set; }

		public static FsmEvent CollisionExit { get; private set; }

		public static FsmEvent CollisionStay { get; private set; }

		public static FsmEvent ControllerColliderHit { get; private set; }

		public static FsmEvent Finished { get; private set; }

		public static FsmEvent LevelLoaded { get; private set; }

		public static FsmEvent MouseDown { get; private set; }

		public static FsmEvent MouseDrag { get; private set; }

		public static FsmEvent MouseEnter { get; private set; }

		public static FsmEvent MouseExit { get; private set; }

		public static FsmEvent MouseOver { get; private set; }

		public static FsmEvent MouseUp { get; private set; }

		public static FsmEvent TriggerEnter { get; private set; }

		public static FsmEvent TriggerExit { get; private set; }

		public static FsmEvent TriggerStay { get; private set; }

		public static FsmEvent PlayerConnected { get; private set; }

		public static FsmEvent ServerInitialized { get; private set; }

		public static FsmEvent ConnectedToServer { get; private set; }

		public static FsmEvent PlayerDisconnected { get; private set; }

		public static FsmEvent DisconnectedFromServer { get; private set; }

		public static FsmEvent FailedToConnect { get; private set; }

		public static FsmEvent FailedToConnectToMasterServer { get; private set; }

		public static FsmEvent MasterServerEvent { get; private set; }

		public static FsmEvent NetworkInstantiate { get; private set; }

		public static bool IsNullOrEmpty(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				return string.IsNullOrEmpty(fsmEvent.name);
			}
			return true;
		}

		public FsmEvent(string name)
		{
			this.name = name;
			if (!EventListContainsEvent(EventList, name))
			{
				EventList.Add(this);
			}
		}

		public FsmEvent(FsmEvent source)
		{
			name = source.name;
			isSystemEvent = source.isSystemEvent;
			isGlobal = source.isGlobal;
		}

		int IComparable.CompareTo(object obj)
		{
			FsmEvent fsmEvent = (FsmEvent)obj;
			if (isSystemEvent && !fsmEvent.isSystemEvent)
			{
				return -1;
			}
			if (!isSystemEvent && fsmEvent.isSystemEvent)
			{
				return 1;
			}
			return string.CompareOrdinal(name, fsmEvent.name);
		}

		public static bool EventListContainsEvent(List<FsmEvent> fsmEventList, string fsmEventName)
		{
			if (fsmEventList == null || string.IsNullOrEmpty(fsmEventName))
			{
				return false;
			}
			foreach (FsmEvent fsmEvent in fsmEventList)
			{
				if (fsmEvent.Name == fsmEventName)
				{
					return true;
				}
			}
			return false;
		}

		public static void RemoveEventFromEventList(FsmEvent fsmEvent)
		{
			if (fsmEvent.isSystemEvent)
			{
				Debug.LogError("RemoveEventFromEventList: Trying to delete System Event: " + fsmEvent.Name);
			}
			EventList.Remove(fsmEvent);
		}

		public static FsmEvent FindEvent(string eventName)
		{
			foreach (FsmEvent @event in EventList)
			{
				if (@event.name == eventName)
				{
					return @event;
				}
			}
			return null;
		}

		public static bool IsEventGlobal(string eventName)
		{
			return globalEvents.Contains(eventName);
		}

		public static bool EventListContains(string eventName)
		{
			return FindEvent(eventName) != null;
		}

		public static FsmEvent GetFsmEvent(string eventName)
		{
			foreach (FsmEvent @event in EventList)
			{
				if (string.CompareOrdinal(@event.Name, eventName) == 0)
				{
					return Application.isPlaying ? @event : new FsmEvent(@event);
				}
			}
			FsmEvent fsmEvent = new FsmEvent(eventName);
			if (!Application.isPlaying)
			{
				return new FsmEvent(fsmEvent);
			}
			return fsmEvent;
		}

		public static FsmEvent GetFsmEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent == null)
			{
				return null;
			}
			foreach (FsmEvent @event in EventList)
			{
				if (string.CompareOrdinal(@event.Name, fsmEvent.Name) == 0)
				{
					if (fsmEvent.IsGlobal || globalEvents.Contains(fsmEvent.name))
					{
						@event.IsGlobal = true;
					}
					return Application.isPlaying ? @event : new FsmEvent(@event);
				}
			}
			if (fsmEvent.isSystemEvent)
			{
				Debug.LogError("Missing System Event: " + fsmEvent.Name);
			}
			return AddFsmEvent(fsmEvent);
		}

		public static FsmEvent AddFsmEvent(FsmEvent fsmEvent)
		{
			eventList.Add(fsmEvent);
			return fsmEvent;
		}

		private static void AddSystemEvents()
		{
			BecameInvisible = AddSystemEvent("BECAME INVISIBLE", "System Events");
			BecameVisible = AddSystemEvent("BECAME VISIBLE", "System Events");
			CollisionEnter = AddSystemEvent("COLLISION ENTER", "System Events");
			CollisionExit = AddSystemEvent("COLLISION EXIT", "System Events");
			CollisionStay = AddSystemEvent("COLLISION STAY", "System Events");
			ControllerColliderHit = AddSystemEvent("CONTROLLER COLLIDER HIT", "System Events");
			Finished = AddSystemEvent("FINISHED", "System Events");
			LevelLoaded = AddSystemEvent("LEVEL LOADED", "System Events");
			MouseDown = AddSystemEvent("MOUSE DOWN", "System Events");
			MouseDrag = AddSystemEvent("MOUSE DRAG", "System Events");
			MouseEnter = AddSystemEvent("MOUSE ENTER", "System Events");
			MouseExit = AddSystemEvent("MOUSE EXIT", "System Events");
			MouseOver = AddSystemEvent("MOUSE OVER", "System Events");
			MouseUp = AddSystemEvent("MOUSE UP", "System Events");
			TriggerEnter = AddSystemEvent("TRIGGER ENTER", "System Events");
			TriggerExit = AddSystemEvent("TRIGGER EXIT", "System Events");
			TriggerStay = AddSystemEvent("TRIGGER STAY", "System Events");
			PlayerConnected = AddSystemEvent("PLAYER CONNECTED", "Network Events");
			ServerInitialized = AddSystemEvent("SERVER INITIALIZED", "Network Events");
			ConnectedToServer = AddSystemEvent("CONNECTED TO SERVER", "Network Events");
			PlayerDisconnected = AddSystemEvent("PLAYER DISCONNECTED", "Network Events");
			DisconnectedFromServer = AddSystemEvent("DISCONNECTED FROM SERVER", "Network Events");
			FailedToConnect = AddSystemEvent("FAILED TO CONNECT", "Network Events");
			FailedToConnectToMasterServer = AddSystemEvent("FAILED TO CONNECT TO MASTER SERVER", "Network Events");
			MasterServerEvent = AddSystemEvent("MASTER SERVER EVENT", "Network Events");
			NetworkInstantiate = AddSystemEvent("NETWORK INSTANTIATE", "Network Events");
		}

		private static FsmEvent AddSystemEvent(string eventName, string path = "")
		{
			FsmEvent fsmEvent = new FsmEvent(eventName);
			fsmEvent.IsSystemEvent = true;
			fsmEvent.Path = ((path == "") ? "" : (path + "/"));
			return fsmEvent;
		}

		private static void AddGlobalEvents()
		{
			foreach (string globalEvent in globalEvents)
			{
				new FsmEvent(globalEvent);
			}
		}

		public static void SanityCheckEventList()
		{
			List<FsmEvent> list = new List<FsmEvent>();
			foreach (FsmEvent @event in EventList)
			{
				if (!EventListContainsEvent(list, @event.Name))
				{
					list.Add(@event);
				}
			}
			eventList = list;
		}
	}
}
