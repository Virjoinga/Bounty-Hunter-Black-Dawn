using UnityEngine;

internal class PlayMakerTriggerExit : PlayMakerProxyBase
{
	private void OnTriggerExit(Collider other)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleTriggerExit)
			{
				playMakerFSM.Fsm.OnTriggerExit(other);
			}
		}
	}
}
