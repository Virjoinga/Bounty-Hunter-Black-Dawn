using UnityEngine;

internal class PlayMakerTriggerStay : PlayMakerProxyBase
{
	private void OnTriggerStay(Collider other)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleTriggerStay)
			{
				playMakerFSM.Fsm.OnTriggerStay(other);
			}
		}
	}
}
