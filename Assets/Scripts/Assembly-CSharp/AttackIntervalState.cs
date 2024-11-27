using UnityEngine;

public class AttackIntervalState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
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
			if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				player.PlayWalkSound();
			}
		}
		if (!player.GetWeapon().IsTypeOfLoopShootingWeapon())
		{
			if (!inputController.inputInfo.fire || (player.IsLocal() && (!weapon.HaveBullets() || weapon.NoBulletsInGun())))
			{
				if (player.IsLocal() && weapon.NoBulletsInGun())
				{
					if (weapon.HaveBullets())
					{
						player.Reload();
					}
					else
					{
						player.SetState(Player.IDLE_STATE);
					}
					return;
				}
				if (player.GetWeapon().GetWaitForAttackAnimationStop() && ((player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.Attack) && !player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.Attack, 1f)) || (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.RunAttack) && !player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.RunAttack, 1f)) || (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyAttack) && !player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyAttack, 1f)) || (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyRunAttack) && !player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.FlyRunAttack, 1f)) || (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + AnimationString.DyingAttack) && !player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + AnimationString.DyingAttack, 1f))))
				{
					return;
				}
			}
			if (player.GetWeapon().CoolDown())
			{
				player.SetState(Player.IDLE_STATE);
				return;
			}
			bool flag = false;
			if (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + text + empty + AnimationString.Attack))
			{
				if (player.AnimationPlayed(player.GetWeaponAnimationSuffix() + text + empty + AnimationString.Attack, 1f))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			if (inputController.inputInfo.IsMoving() && !player.DYING_STATE.InDyingState)
			{
				if (player.IsLocal())
				{
					player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + text + AnimationString.Run, WrapMode.Loop);
					return;
				}
				player.PlayAnimationAllLayers(player.GetWeaponAnimationSuffix() + AnimationString.RunTPS + "_" + inputController.inputInfo.dir, WrapMode.Loop);
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
		}
		else
		{
			player.SetState(Player.IDLE_STATE);
		}
	}
}
