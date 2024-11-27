using UnityEngine;

public class FirstAidState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal())
		{
			Timer firstAidTimer = player.GetFirstAidTimer();
			if (firstAidTimer.Ready())
			{
				player.SetState(Player.IDLE_STATE);
			}
		}
		player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.FirstAid, WrapMode.Loop);
	}
}
