public class WatchUIScreenState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		InputController inputController = player.inputController;
		player.GetWeapon().StopFire();
		player.GetWeapon().AutoDestructEffect();
		WeaponType weaponType = player.GetWeapon().GetWeaponType();
	}
}
