public class PlayerKnockedState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		player.GetKnocked();
	}
}
