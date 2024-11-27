using UnityEngine;

internal class PlayMakerCollisionEnter : PlayMakerProxyBase
{
	private void OnCollisionEnter(Collision collisionInfo)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleCollisionEnter)
			{
				playMakerFSM.Fsm.OnCollisionEnter(collisionInfo);
			}
		}
	}
}
