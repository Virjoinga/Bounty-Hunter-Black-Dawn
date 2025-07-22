using UnityEngine;

internal class PlayMakerCollisionExit : PlayMakerProxyBase
{
	private void OnCollisionExit(Collision collisionInfo)
	{
		PlayMakerFSM[] array = playMakerFSMs;
		foreach (PlayMakerFSM playMakerFSM in array)
		{
			if (playMakerFSM.Fsm.HandleCollisionExit)
			{
				playMakerFSM.Fsm.OnCollisionExit(collisionInfo);
			}
		}
	}
}
