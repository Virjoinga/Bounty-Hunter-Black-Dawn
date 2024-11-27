using UnityEngine;

public class IdleState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		if (player.Knife != null && player.Knife.activeSelf)
		{
			player.CancelMeleeAttack();
		}
		bool inAimState = player.InAimState;
		string text = string.Empty;
		if (inAimState && player.IsLocal())
		{
			text = "aimdown_";
		}
		string empty = string.Empty;
		WeaponType weaponType = player.GetWeapon().GetWeaponType();
		if (player.IsLocal())
		{
			if (inAimState)
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomIn(deltaTime);
			}
			else
			{
				GameApp.GetInstance().GetGameScene().GetCamera()
					.ZoomOut(deltaTime);
			}
			player.Move(inputController.inputInfo.moveDirection);
			if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				player.PlayWalkSound();
			}
		}
		if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
		{
			if (player.IsLocal())
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + text + AnimationString.Run, WrapMode.Loop);
			}
			else
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.RunTPS + "_" + inputController.inputInfo.dir, WrapMode.Loop);
			}
		}
		else
		{
			player.ResetRunningPhase();
			if (player.IsLocal())
			{
				AudioManager.GetInstance().StopSound(player.FootStepAudio);
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + text + empty + AnimationString.Idle, WrapMode.Loop);
			}
			else if (player.DYING_STATE.InDyingState)
			{
				player.PlayAnimationAllLayers(AnimationString.DyingTPS, WrapMode.Loop);
			}
			else
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Idle, WrapMode.Loop);
			}
		}
		if (!inputController.inputInfo.fire)
		{
			return;
		}
		if (player.IsLocal())
		{
			if (!player.DYING_STATE.InDyingState && player.GetWeapon().HaveBullets())
			{
				if (player.GetWeapon().NoBulletsInGun() && player.GetWeapon().IsTypeOfLoopShootingWeapon())
				{
					player.Reload();
				}
				else
				{
					player.SetState(Player.ATTACK_STATE);
				}
			}
		}
		else
		{
			player.SetState(Player.ATTACK_STATE);
		}
	}
}
