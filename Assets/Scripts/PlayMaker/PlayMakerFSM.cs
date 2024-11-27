using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[AddComponentMenu("PlayMaker/PlayMakerFSM")]
public class PlayMakerFSM : MonoBehaviour
{
	private static readonly List<PlayMakerFSM> fsmList = new List<PlayMakerFSM>();

	public static bool ApplicationIsQuitting;

	[SerializeField]
	[HideInInspector]
	private Fsm fsm;

	public static List<PlayMakerFSM> FsmList
	{
		get
		{
			return fsmList;
		}
	}

	public static bool DrawGizmos { get; set; }

	public Fsm Fsm
	{
		get
		{
			return fsm;
		}
	}

	public string FsmName
	{
		get
		{
			return fsm.Name;
		}
		set
		{
			fsm.Name = value;
		}
	}

	public string FsmDescription
	{
		get
		{
			return fsm.Description;
		}
		set
		{
			fsm.Description = value;
		}
	}

	public bool Active
	{
		get
		{
			return fsm.Active;
		}
	}

	public string ActiveStateName
	{
		get
		{
			if (fsm.ActiveState != null)
			{
				return fsm.ActiveState.Name;
			}
			return "";
		}
	}

	public FsmState[] FsmStates
	{
		get
		{
			return fsm.States;
		}
	}

	public FsmEvent[] FsmEvents
	{
		get
		{
			return fsm.Events;
		}
	}

	public FsmTransition[] FsmGlobalTransitions
	{
		get
		{
			return fsm.GlobalTransitions;
		}
	}

	public FsmVariables FsmVariables
	{
		get
		{
			return fsm.Variables;
		}
	}

	private void AddToFsmList()
	{
		if (!fsmList.Contains(this))
		{
			fsmList.Add(this);
		}
	}

	private void RemoveFromFsmList()
	{
		fsmList.Remove(this);
	}

	private void Reset()
	{
		if (fsm == null)
		{
			fsm = new Fsm();
		}
		fsm.Reset(this);
	}

	private void Awake()
	{
		if (fsm == null)
		{
			Reset();
		}
		if (fsm == null)
		{
			Debug.LogError("Could not initialize FSM!");
			base.enabled = false;
		}
		else
		{
			AddToFsmList();
		}
	}

	private void Start()
	{
		if (fsm == null)
		{
			Reset();
		}
		if (fsm != null)
		{
			fsm.Init(this);
			fsm.EnterStartState();
			if (fsm.MouseEvents && GetComponent<PlayMakerMouseEvents>() == null)
			{
				base.gameObject.AddComponent<PlayMakerMouseEvents>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleCollisionEnter && GetComponent<PlayMakerCollisionEnter>() == null)
			{
				base.gameObject.AddComponent<PlayMakerCollisionEnter>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleCollisionExit && GetComponent<PlayMakerCollisionExit>() == null)
			{
				base.gameObject.AddComponent<PlayMakerCollisionExit>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleCollisionStay && GetComponent<PlayMakerCollisionStay>() == null)
			{
				base.gameObject.AddComponent<PlayMakerCollisionStay>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleTriggerEnter && GetComponent<PlayMakerTriggerEnter>() == null)
			{
				base.gameObject.AddComponent<PlayMakerTriggerEnter>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleTriggerExit && GetComponent<PlayMakerTriggerExit>() == null)
			{
				base.gameObject.AddComponent<PlayMakerTriggerExit>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleTriggerStay && GetComponent<PlayMakerTriggerStay>() == null)
			{
				base.gameObject.AddComponent<PlayMakerTriggerStay>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleFixedUpdate && GetComponent<PlayMakerFixedUpdate>() == null)
			{
				base.gameObject.AddComponent<PlayMakerFixedUpdate>().hideFlags = HideFlags.HideInInspector;
			}
			if (fsm.HandleOnGUI && GetComponent<PlayMakerOnGUI>() == null)
			{
				PlayMakerOnGUI playMakerOnGUI = base.gameObject.AddComponent<PlayMakerOnGUI>();
				playMakerOnGUI.playMakerFSM = this;
			}
		}
	}

	private void OnEnable()
	{
		AddToFsmList();
		fsm.OnEnable();
	}

	private void FixedUpdate()
	{
		fsm.FixedUpdate();
	}

	private void Update()
	{
		fsm.Update();
	}

	private void LateUpdate()
	{
		FsmVariables.GlobalVariablesSynced = false;
		fsm.LateUpdate();
	}

	private void OnDisable()
	{
		RemoveFromFsmList();
		fsm.OnDisable();
	}

	private void OnDestroy()
	{
		RemoveFromFsmList();
		fsm.OnDestroy();
	}

	private void OnApplicationQuit()
	{
		ApplicationIsQuitting = true;
	}

	private void OnDrawGizmos()
	{
		if (fsm != null)
		{
			fsm.OnDrawGizmos();
		}
	}

	public void ChangeState(FsmEvent fsmEvent)
	{
		fsm.Event(fsmEvent);
	}

	[Obsolete("Use SendEvent(string) instead.")]
	public void ChangeState(string eventName)
	{
		fsm.Event(eventName);
	}

	public void SendEvent(string eventName)
	{
		fsm.Event(eventName);
	}

	[RPC]
	public void SendRemoteFsmEvent(string eventName)
	{
		fsm.Event(eventName);
	}

	public static void BroadcastEvent(string fsmEventName)
	{
		if (!string.IsNullOrEmpty(fsmEventName))
		{
			BroadcastEvent(FsmEvent.GetFsmEvent(fsmEventName));
		}
	}

	public static void BroadcastEvent(FsmEvent fsmEvent)
	{
		List<PlayMakerFSM> list = new List<PlayMakerFSM>(FsmList);
		foreach (PlayMakerFSM item in list)
		{
			if (!(item == null) && item.Fsm != null)
			{
				item.Fsm.ChangeState(fsmEvent);
			}
		}
	}

	private void OnBecameVisible()
	{
		fsm.Event(FsmEvent.BecameVisible);
	}

	private void OnBecameInvisible()
	{
		fsm.Event(FsmEvent.BecameInvisible);
	}

	private void OnLevelWasLoaded()
	{
		fsm.Event(FsmEvent.LevelLoaded);
	}

	private void OnControllerColliderHit(ControllerColliderHit hitCollider)
	{
		fsm.OnControllerColliderHit(hitCollider);
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		Fsm.EventData.Player = player;
		fsm.Event(FsmEvent.PlayerConnected);
	}

	private void OnServerInitialized()
	{
		fsm.Event(FsmEvent.ServerInitialized);
	}

	private void OnConnectedToServer()
	{
		fsm.Event(FsmEvent.ConnectedToServer);
	}

	private void OnPlayerDisconnected(NetworkPlayer player)
	{
		Fsm.EventData.Player = player;
		fsm.Event(FsmEvent.PlayerDisconnected);
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Fsm.EventData.DisconnectionInfo = info;
		fsm.Event(FsmEvent.DisconnectedFromServer);
	}

	private void OnFailedToConnect(NetworkConnectionError error)
	{
		Fsm.EventData.ConnectionError = error;
		fsm.Event(FsmEvent.FailedToConnect);
	}

	private void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		Fsm.EventData.NetworkMessageInfo = info;
		fsm.Event(FsmEvent.NetworkInstantiate);
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!FsmVariables.GlobalVariablesSynced)
		{
			FsmVariables.GlobalVariablesSynced = true;
			NetworkSyncVariables(stream, FsmVariables.GlobalVariables);
		}
		NetworkSyncVariables(stream, Fsm.Variables);
	}

	private static void NetworkSyncVariables(BitStream stream, FsmVariables variables)
	{
		FsmInt[] intVariables;
		FsmQuaternion[] quaternionVariables;
		FsmVector3[] vector3Variables;
		FsmColor[] colorVariables;
		FsmVector2[] vector2Variables;
		if (stream.isWriting)
		{
			FsmString[] stringVariables = variables.StringVariables;
			foreach (FsmString fsmString in stringVariables)
			{
				if (fsmString.NetworkSync)
				{
					char[] array = fsmString.Value.ToCharArray();
					int value = array.Length;
					stream.Serialize(ref value);
					for (int j = 0; j < value; j++)
					{
						stream.Serialize(ref array[j]);
					}
				}
			}
			FsmBool[] boolVariables = variables.BoolVariables;
			foreach (FsmBool fsmBool in boolVariables)
			{
				if (fsmBool.NetworkSync)
				{
					bool value2 = fsmBool.Value;
					stream.Serialize(ref value2);
				}
			}
			FsmFloat[] floatVariables = variables.FloatVariables;
			foreach (FsmFloat fsmFloat in floatVariables)
			{
				if (fsmFloat.NetworkSync)
				{
					float value3 = fsmFloat.Value;
					stream.Serialize(ref value3);
				}
			}
			intVariables = variables.IntVariables;
			foreach (FsmInt fsmInt in intVariables)
			{
				if (fsmInt.NetworkSync)
				{
					int value4 = fsmInt.Value;
					stream.Serialize(ref value4);
				}
			}
			quaternionVariables = variables.QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in quaternionVariables)
			{
				if (fsmQuaternion.NetworkSync)
				{
					Quaternion value5 = fsmQuaternion.Value;
					stream.Serialize(ref value5);
				}
			}
			vector3Variables = variables.Vector3Variables;
			foreach (FsmVector3 fsmVector in vector3Variables)
			{
				if (fsmVector.NetworkSync)
				{
					Vector3 value6 = fsmVector.Value;
					stream.Serialize(ref value6);
				}
			}
			colorVariables = variables.ColorVariables;
			foreach (FsmColor fsmColor in colorVariables)
			{
				if (fsmColor.NetworkSync)
				{
					Color value7 = fsmColor.Value;
					stream.Serialize(ref value7.r);
					stream.Serialize(ref value7.g);
					stream.Serialize(ref value7.b);
					stream.Serialize(ref value7.a);
				}
			}
			vector2Variables = variables.Vector2Variables;
			foreach (FsmVector2 fsmVector2 in vector2Variables)
			{
				if (fsmVector2.NetworkSync)
				{
					Vector2 value8 = fsmVector2.Value;
					stream.Serialize(ref value8.x);
					stream.Serialize(ref value8.y);
				}
			}
			return;
		}
		FsmString[] stringVariables2 = variables.StringVariables;
		foreach (FsmString fsmString2 in stringVariables2)
		{
			if (fsmString2.NetworkSync)
			{
				int value9 = 0;
				stream.Serialize(ref value9);
				char[] array2 = new char[value9];
				for (int num5 = 0; num5 < value9; num5++)
				{
					stream.Serialize(ref array2[num5]);
				}
				fsmString2.Value = new string(array2);
			}
		}
		FsmBool[] boolVariables2 = variables.BoolVariables;
		foreach (FsmBool fsmBool2 in boolVariables2)
		{
			if (fsmBool2.NetworkSync)
			{
				bool value10 = false;
				stream.Serialize(ref value10);
				fsmBool2.Value = value10;
			}
		}
		FsmFloat[] floatVariables2 = variables.FloatVariables;
		foreach (FsmFloat fsmFloat2 in floatVariables2)
		{
			if (fsmFloat2.NetworkSync)
			{
				float value11 = 0f;
				stream.Serialize(ref value11);
				fsmFloat2.Value = value11;
			}
		}
		intVariables = variables.IntVariables;
		foreach (FsmInt fsmInt2 in intVariables)
		{
			if (fsmInt2.NetworkSync)
			{
				int value12 = 0;
				stream.Serialize(ref value12);
				fsmInt2.Value = value12;
			}
		}
		quaternionVariables = variables.QuaternionVariables;
		foreach (FsmQuaternion fsmQuaternion2 in quaternionVariables)
		{
			if (fsmQuaternion2.NetworkSync)
			{
				Quaternion value13 = Quaternion.identity;
				stream.Serialize(ref value13);
				fsmQuaternion2.Value = value13;
			}
		}
		vector3Variables = variables.Vector3Variables;
		foreach (FsmVector3 fsmVector3 in vector3Variables)
		{
			if (fsmVector3.NetworkSync)
			{
				Vector3 value14 = Vector3.zero;
				stream.Serialize(ref value14);
				fsmVector3.Value = value14;
			}
		}
		colorVariables = variables.ColorVariables;
		foreach (FsmColor fsmColor2 in colorVariables)
		{
			if (fsmColor2.NetworkSync)
			{
				float value15 = 0f;
				stream.Serialize(ref value15);
				float value16 = 0f;
				stream.Serialize(ref value16);
				float value17 = 0f;
				stream.Serialize(ref value17);
				float value18 = 0f;
				stream.Serialize(ref value18);
				fsmColor2.Value = new Color(value15, value16, value17, value18);
			}
		}
		vector2Variables = variables.Vector2Variables;
		foreach (FsmVector2 fsmVector4 in vector2Variables)
		{
			if (fsmVector4.NetworkSync)
			{
				float value19 = 0f;
				stream.Serialize(ref value19);
				float value20 = 0f;
				stream.Serialize(ref value20);
				fsmVector4.Value = new Vector2(value19, value20);
			}
		}
	}

	private void OnMasterServerEvent(MasterServerEvent masterServerEvent)
	{
		Fsm.EventData.MasterServerEvent = masterServerEvent;
		fsm.Event(FsmEvent.MasterServerEvent);
	}
}
