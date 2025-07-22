public class ReloadState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		bool inAimState = player.InAimState;
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
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
		if (player.IsPlayingAnimation(player.GetWeaponAnimationSuffix() + AnimationString.Reload) && player.AnimationPlayed(player.GetWeaponAnimationSuffix() + AnimationString.Reload, 1f))
		{
			player.ReloadComplete();
			player.SetState(Player.IDLE_STATE);
		}
	}
}
