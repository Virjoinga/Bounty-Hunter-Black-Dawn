public class PlayerLoseState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.DoVSLose() && player.IsLocal())
		{
			GameApp.GetInstance().GetGameScene().State = GameState.GameOverUILose;
		}
	}
}
