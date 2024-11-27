using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Loads a Level by Index number. Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
	[ActionCategory(ActionCategory.Level)]
	public class LoadLevelNum : FsmStateAction
	{
		[RequiredField]
		public FsmInt levelIndex;

		public bool additive;

		public FsmEvent loadedEvent;

		public FsmBool dontDestroyOnLoad;

		public override void Reset()
		{
			levelIndex = null;
			additive = false;
			loadedEvent = null;
			dontDestroyOnLoad = true;
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
				Application.LoadLevelAdditive(levelIndex.Value);
			}
			else
			{
				Application.LoadLevel(levelIndex.Value);
			}
			base.Fsm.Event(loadedEvent);
			Finish();
		}
	}
}
