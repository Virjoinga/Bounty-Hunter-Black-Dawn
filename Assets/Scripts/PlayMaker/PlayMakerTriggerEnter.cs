using UnityEngine;

internal class PlayMakerTriggerEnter : PlayMakerProxyBase
{
	private void OnTriggerEnter(Collider other)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleTriggerEnter)
			{
				playMakerFSM.Fsm.OnTriggerEnter(other);
			}
		}
	}
}
