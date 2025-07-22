using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public abstract class FsmStateAction : IFsmStateAction
	{
		private string name;

		private bool enabled = true;

		private bool isOpen = true;

		private bool finished;

		private GameObject owner;

		[NonSerialized]
		private FsmState fsmState;

		[NonSerialized]
		private Fsm fsm;

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

		public Fsm Fsm
		{
			get
			{
				return fsm;
			}
			set
			{
				fsm = value;
			}
		}

		public GameObject Owner
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

		public FsmState State
		{
			get
			{
				return fsmState;
			}
			set
			{
				fsmState = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				isOpen = value;
			}
		}

		public bool Finished
		{
			get
			{
				return finished;
			}
			set
			{
				finished = value;
			}
		}

		public bool Active
		{
			get
			{
				if (enabled)
				{
					return !finished;
				}
				return false;
			}
		}

		public virtual void Init(FsmState state)
		{
			fsmState = state;
			fsm = state.Fsm;
			owner = fsm.GameObject;
		}

		public virtual void Reset()
		{
		}

		public virtual void Awake()
		{
		}

		public void ChangeState(FsmEvent fsmEvent)
		{
			fsm.ChangeState(fsmEvent);
		}

		public void ChangeState(string eventName)
		{
			fsm.ChangeState(FsmEvent.GetFsmEvent(eventName));
		}

		public void Finish()
		{
			finished = true;
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnGUI()
		{
		}

		public virtual void OnLateUpdate()
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnDrawGizmos()
		{
		}

		public virtual void OnDrawGizmosSelected()
		{
		}

		public virtual void DoCollisionEnter(Collision collisionInfo)
		{
		}

		public virtual void DoCollisionStay(Collision collisionInfo)
		{
		}

		public virtual void DoCollisionExit(Collision collisionInfo)
		{
		}

		public virtual void DoTriggerEnter(Collider other)
		{
		}

		public virtual void DoTriggerStay(Collider other)
		{
		}

		public virtual void DoTriggerExit(Collider other)
		{
		}

		public virtual void DoControllerColliderHit(ControllerColliderHit collider)
		{
		}

		public void Log(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Info, text);
			}
		}

		public void LogWarning(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Warning, text);
			}
		}

		public void LogError(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Error, text);
			}
		}

		public virtual string ErrorCheck()
		{
			return string.Empty;
		}
	}
}
