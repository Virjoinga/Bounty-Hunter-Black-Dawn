using UnityEngine;

public class AimButtonScript : MonoBehaviour
{
	private void OnClick()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.CanAim())
		{
			if (localPlayer.InAimState)
			{
				localPlayer.Aim(false);
			}
			else
			{
				localPlayer.Aim(true);
			}
		}
	}
}
