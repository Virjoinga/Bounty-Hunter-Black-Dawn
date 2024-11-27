using UnityEngine;

public class EntranceScript : MonoBehaviour
{
	public float radius = 5f;

	private void Update()
	{
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		if (gameScene != null)
		{
			Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			if (localPlayer != null && localPlayer.GetTransform() != null && Vector3.Distance(localPlayer.GetTransform().position, base.transform.position) < radius && !UIPortal.IsShow())
			{
				UIPortal.Show();
			}
		}
	}
}
