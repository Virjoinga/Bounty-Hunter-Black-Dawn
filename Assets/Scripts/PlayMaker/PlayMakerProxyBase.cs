using UnityEngine;

internal class PlayMakerProxyBase : MonoBehaviour
{
	protected PlayMakerFSM[] playMakerFSMs;

	public void Awake()
	{
		Reset();
	}

	public void Reset()
	{
		playMakerFSMs = GetComponents<PlayMakerFSM>();
	}
}
