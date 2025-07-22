using UnityEngine;

public class SwitchWeaponOnState : PlayerState
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
			player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponOn, WrapMode.ClampForever);
			if (player.AnimationPlayed(player.GetWeaponAnimationSuffix() + AnimationString.SwitchWeaponOn, 1f))
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
				player.SetState(Player.IDLE_STATE);
			}
		}
	}
}
