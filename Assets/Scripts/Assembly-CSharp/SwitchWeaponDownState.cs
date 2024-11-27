using UnityEngine;

public class SwitchWeaponDownState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal())
		{
			if (player.InAimState)
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomIn(deltaTime);
			}
			else
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomOut(deltaTime);
			}
			player.Move(player.inputController.inputInfo.moveDirection);
			if (player.inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				player.PlayWalkSound();
			}
			player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponDown, WrapMode.ClampForever);
			if (player.AnimationPlayed(player.GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponDown, 1f))
			{
				player.SwitchWeapon();
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponOn, WrapMode.ClampForever);
				player.SetState(Player.SWITCH_WEAPON_ON_STATE);
			}
		}
	}
}
