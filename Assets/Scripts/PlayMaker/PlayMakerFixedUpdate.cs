internal class PlayMakerFixedUpdate : PlayMakerProxyBase
{
	private void FixedUpdate()
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleFixedUpdate)
			{
				playMakerFSM.Fsm.FixedUpdate();
			}
		}
	}
}
