using HutongGames.PlayMaker;
using UnityEngine;

[ExecuteInEditMode]
internal class PlayMakerOnGUI : MonoBehaviour
{
	public PlayMakerFSM playMakerFSM;

	public bool previewInEditMode = true;

	public void Start()
	{
		if (playMakerFSM != null)
		{
			playMakerFSM.Fsm.HandleOnGUI = true;
		}
	}

	public void OnGUI()
	{
		if (previewInEditMode && !Application.isPlaying)
		{
			DoEditGUI();
		}
		else
		{
			if (!(playMakerFSM != null) || playMakerFSM.Fsm == null || playMakerFSM.Fsm.ActiveState == null || !playMakerFSM.Fsm.HandleOnGUI)
			{
				return;
			}
			FsmStateAction[] actions = playMakerFSM.Fsm.ActiveState.Actions;
			foreach (FsmStateAction fsmStateAction in actions)
			{
				if (fsmStateAction.Active)
				{
					fsmStateAction.OnGUI();
				}
			}
		}
	}

	private static void DoEditGUI()
	{
		if (PlayMakerGUI.SelectedFSM == null)
		{
			return;
		}
		FsmState editState = PlayMakerGUI.SelectedFSM.EditState;
		if (editState == null || !editState.IsInitialized)
		{
			return;
		}
		FsmStateAction[] actions = editState.Actions;
		FsmStateAction[] array = actions;
		foreach (FsmStateAction fsmStateAction in array)
		{
			if (fsmStateAction.Active)
			{
				fsmStateAction.OnGUI();
			}
		}
	}
}
