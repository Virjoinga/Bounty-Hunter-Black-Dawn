using UnityEngine;

public class AttackState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.Knife != null && player.Knife.activeSelf)
		{
			player.CancelMeleeAttack();
		}
		InputController inputController = player.inputController;
		Weapon weapon = player.GetWeapon();
		bool inAimState = player.InAimState;
		string text = string.Empty;
		if (inAimState && player.IsLocal())
		{
			text = "aimdown_";
		}
		string empty = string.Empty;
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
			if (!player.DYING_STATE.InDyingState)
			{
				if (inputController.inputInfo.IsMoving())
				{
					player.PlayWalkSound();
				}
				else
				{
					player.ResetRunningPhase();
					AudioManager.GetInstance().StopSound(player.FootStepAudio);
				}
			}
			if (weapon.NoBulletsInGun())
			{
				if (weapon.HaveBullets())
				{
					if (weapon.IsTypeOfLoopShootingWeapon() || (!weapon.IsTypeOfLoopShootingWeapon() && weapon.CoolDown()))
					{
						player.Reload();
					}
				}
				else
				{
					player.SetState(Player.IDLE_STATE);
				}
				return;
			}
		}
		if (weapon.IsTypeOfLoopShootingWeapon())
		{
			player.PlayAttackSound();
		}
		weapon.Loop(deltaTime);
		weapon.AutoDestructEffect();
		WrapMode mode = WrapMode.Loop;
		if (!player.GetWeapon().IsTypeOfLoopShootingWeapon())
		{
			mode = WrapMode.ClampForever;
		}
		if (player.GetWeapon().CoolDown())
		{
			if (player.IsLocal())
			{
				player.Attack();
			}
			else if (!player.DYING_STATE.InDyingState)
			{
				weapon.CreateTrajectory();
			}
			if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				if (player.IsLocal())
				{
					player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + text + AnimationString.RunAttack, mode);
				}
				else
				{
					player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.RunAttackTPS + "_" + inputController.inputInfo.dir, mode);
				}
			}
			else if (player.IsLocal())
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + text + empty + AnimationString.Attack, mode);
			}
			else if (player.DYING_STATE.InDyingState)
			{
				player.PlayAnimationAllLayers(AnimationString.DyingTPS, mode);
			}
			else
			{
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.Attack, mode);
			}
		}
		else if (!weapon.IsTypeOfLoopShootingWeapon())
		{
			player.SetState(Player.ATTACK_INTERVAL_STATE);
		}
		if ((!inputController.inputInfo.fire || (player.IsLocal() && !weapon.HaveBullets())) && (!player.GetWeapon().GetWaitForAttackAnimationStop() || ((!player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.Attack) || player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.Attack, 1f)) && (!player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.RunAttack) || player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.RunAttack, 1f)) && (!player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyAttack) || player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyAttack, 1f)) && (!player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyRunAttack) || player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyRunAttack, 1f)) && (!player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.DyingAttack) || player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.DyingAttack, 1f)))))
		{
			player.SetState(Player.IDLE_STATE);
			if (player.IsLocal())
			{
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
	}
}
