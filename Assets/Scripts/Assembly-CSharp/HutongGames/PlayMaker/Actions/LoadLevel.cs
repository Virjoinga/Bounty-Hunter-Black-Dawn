using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Level)]
	[Tooltip("Loads a Level by Name. NOTE: Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	public class LoadLevel : FsmStateAction
	{
		[Tooltip("The name of the level to load. NOTE: Must be in the list of levels defined in File->Build Settings... ")]
		[RequiredField]
		public FsmString levelName;

		[Tooltip("Load the level additively, keeping the current scene.")]
		public bool additive;

		[Tooltip("Load the level asynchronously in the background.")]
		public bool async;

		[Tooltip("Event to send when the level has loaded. NOTE: This only makes sense if the FSM is still in the scene!")]
		public FsmEvent loadedEvent;

		public FsmBool dontDestroyOnLoad;

		private AsyncOperation asyncOperation;

		public override void Reset()
		{
			levelName = string.Empty;
			additive = false;
			async = false;
			loadedEvent = null;
			dontDestroyOnLoad = false;
		}

		public override void OnEnter()
		{
			if (dontDestroyOnLoad.Value)
			{
				Transform root = base.Owner.transform.root;
				Object.DontDestroyOnLoad(root.gameObject);
			}
			if (additive)
			{
				if (async)
				{
					asyncOperation = Application.LoadLevelAdditiveAsync(levelName.Value);
					Debug.Log("LoadLevelAdditiveAsyc: " + levelName.Value);
					return;
				}
				Application.LoadLevelAdditive(levelName.Value);
				Debug.Log("LoadLevelAdditive: " + levelName.Value);
			}
			else
			{
				if (async)
				{
					asyncOperation = Application.LoadLevelAsync(levelName.Value);
					Debug.Log("LoadLevelAsync: " + levelName.Value);
					return;
				}
				Application.LoadLevel(levelName.Value);
				Debug.Log("LoadLevel: " + levelName.Value);
			}
			Log("LOAD COMPLETE");
			base.Fsm.Event(loadedEvent);
			Finish();
		}

		public override void OnUpdate()
		{
			if (asyncOperation.isDone)
			{
				base.Fsm.Event(loadedEvent);
				Finish();
			}
		}
	}
}
