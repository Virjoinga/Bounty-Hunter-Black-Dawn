using UnityEngine;

internal class PlayMakerCollisionStay : PlayMakerProxyBase
{
	private void OnCollisionStay(Collision collisionInfo)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleCollisionStay)
			{
				playMakerFSM.Fsm.OnCollisionStay(collisionInfo);
			}
		}
	}
}
