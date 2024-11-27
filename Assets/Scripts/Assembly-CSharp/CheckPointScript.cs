using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
	public GameObject SpawnPoint;

	public int CheckPointID;

	protected virtual void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			localPlayer.CurrentRespawnPoint = SpawnPoint;
			localPlayer.CurrentRespawnPointID = (short)CheckPointID;
		}
	}
}
