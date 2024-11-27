using HutongGames.PlayMaker;

internal class PlayMakerMouseEvents : PlayMakerProxyBase
{
	private void OnMouseEnter()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseEnter);
			}
		}
	}

	private void OnMouseDown()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseDown);
			}
		}
	}

	private void OnMouseUp()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseUp);
				Fsm.LastClickedObject = base.gameObject;
			}
		}
	}

	private void OnMouseExit()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseExit);
			}
		}
	}

	private void OnMouseDrag()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseDrag);
			}
		}
	}

	private void OnMouseOver()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.MouseEvents)
			{
				playMakerFSM.Fsm.Event(FsmEvent.MouseOver);
			}
		}
	}
}
