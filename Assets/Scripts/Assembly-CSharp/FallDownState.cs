using UnityEngine;

public class FallDownState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		if (!player.KnockedComplete())
		{
			player.GetKnocked();
		}
		if (player.IsLocal())
		{
			LocalPlayer localPlayer = (LocalPlayer)player;
			if (localPlayer.FallDownCompleted() && localPlayer.KnockedComplete())
			{
				localPlayer.DYING_STATE.OnDying();
				if (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + AnimationString.Reload))
				{
					if (player.AnimationPlayed(player.GetWeaponAnimationSuffix() + AnimationString.Reload, 1f))
					{
						player.ReloadComplete();
						localPlayer.SetState(Player.IDLE_STATE);
					}
				}
				else
				{
					localPlayer.SetState(Player.IDLE_STATE);
				}
			}
			else if (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + AnimationString.Reload) && player.AnimationPlayed(player.GetWeaponAnimationSuffix() + AnimationString.Reload, 1f))
			{
				player.ReloadComplete();
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
			}
		}
		else
		{
			RemotePlayer remotePlayer = player as RemotePlayer;
			if (remotePlayer != null && remotePlayer.FallDownFinish() && remotePlayer.KnockedComplete())
			{
				player.DYING_STATE.OnDying();
				player.SetState(Player.IDLE_STATE);
			}
		}
	}
}
