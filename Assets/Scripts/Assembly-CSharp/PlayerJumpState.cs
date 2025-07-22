public class PlayerJumpState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		WeaponType weaponType = player.GetWeapon().GetWeaponType();
		bool flag = player.Jump();
		player.Move(inputController.inputInfo.moveDirection);
		if (flag)
		{
			player.SetState(Player.IDLE_STATE);
		}
	}
}
