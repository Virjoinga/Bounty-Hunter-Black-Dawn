public class WinState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.DoWin() && player.IsLocal())
		{
			GameApp.GetInstance().GetGameScene().State = GameState.GameOverUIWin;
		}
	}
}
